Param(
    [parameter(Mandatory=$false)][string]$registry,
    [parameter(Mandatory=$false)][string]$dockerUser,
    [parameter(Mandatory=$false)][string]$dockerPassword,
    [parameter(Mandatory=$true)][string]$imageTag,
    [parameter(Mandatory=$false)][string]$execPath,
    [parameter(Mandatory=$false)][string]$deployEnv,
    [parameter(Mandatory=$false)][bool]$deployInfrastructure=$true,
    [parameter(Mandatory=$false)][bool]$useImageProfix=$true,
    [parameter(Mandatory=$false)][int]$replicasNum=1,
    [parameter(Mandatory=$false)][string]$imageProfix="surging.hero.",
    [parameter(Mandatory=$false)][string]$dockerOrg="surgingcloud"
)

. "./utilities.ps1"

$externalDns = & ExecKube -cmd 'get svc ingress-nginx -n ingress-nginx -o=jsonpath="{.status.loadBalancer.ingress[0].ip}"'
Write-Host "Ingress ip is: $externalDns" -ForegroundColor Yellow 

if (-not [bool]($externalDns -as [ipaddress])) {
    Write-Host "please install ingress plugin in first" -ForegroundColor Red
    Write-Host "please call deploy-ingress.ps1" -ForegroundColor Red
    exit
}

if ([string]::IsNullOrEmpty($imageTag)) {
    Write-Host "please set docker image tag" -ForegroundColor Red
    exit
}

ExecKube -cmd 'delete configmap -l=app=surging-hero'

$configFilePrefix = 'hero-conf' 
if (-not [string]::IsNullOrEmpty($deployEnv)){
  $configFilePrefix = $configFilePrefix + '-' + $deployEnv
}
$configFile = $configFilePrefix + '.yml'
if (-not (Test-Path -Path './*' -Include $configFile)) {
    Write-Host "not exsit $configFile" -ForegroundColor Red
    exit
}
ExecKube -cmd "create -f $configFile"


$useDockerHub = [string]::IsNullOrEmpty($registry)

# if we have login/pwd add the secret to k8s
if (-not [string]::IsNullOrEmpty($dockerUser)) {
    $registryFDQN =  if (-not $useDockerHub) {$registry} else {""}

    Write-Host "using account $dockerUser login $registryFDQN" -ForegroundColor Yellow
    if ($useDockerHub) {
        docker login -u $dockerUser -p $dockerPassword
    }
    else {
        docker login -u $dockerUser -p $dockerPassword $registryFDQN
    }
    
    if (-not $LastExitCode -eq 0) {
        Write-Host "Login failed" -ForegroundColor Red
        exit
    }


    # Try to delete the Docker registry key secret
    ExecKube -cmd 'delete secret docker-registry registry-key'

    # Create the Docker registry key secret
    ExecKube -cmd 'create secret docker-registry registry-key `
    --docker-server=$registryFDQN `
    --docker-username=$dockerUser `
    --docker-password=$dockerPassword `
    --docker-email=not@used.com'
}

Write-Host "Removing existing services & deployments.." -ForegroundColor Yellow
ExecKube -cmd 'delete deployments -l=app=surging-hero'
ExecKube -cmd 'delete services -l=app=surging-hero'
ExecKube -cmd 'delete ingress -l=app=surging-hero'
ExecKube -cmd 'delete pods -l=app=surging-hero --grace-period=0 --force'

if ($deployInfrastructure) {
    ExecKube -cmd 'delete deployments -l=app=surging-hero-middle'
    ExecKube -cmd 'delete services -l=app=surging-hero-middle'
    ExecKube -cmd 'delete ingress -l=app=surging-hero-middle'
    ExecKube -cmd 'delete pods -l=app=surging-hero-middle --grace-period=0 --force'
    Write-Host 'Deploying infrastructure deployments (consul,mysql, redis, RabbitMQ...)' -ForegroundColor Yellow
    ExecKube -cmd 'apply -f consul.yml -f mysql.yml -f rabbitmq.yml -f redis.yml'
}

$services = (Get-Content "./ServiceComponents")
$getwayDomain = "gateway.surginghero.com"
foreach($service in $services) {
    Write-Host "Deploying code deployment and service for $service" -ForegroundColor Yellow
    ExecKube -cmd "apply -f $service/$service-deployments.yml -f $service/$service-services.yml"
    Write-Host "Deploying code ingress for $service" -ForegroundColor Yellow
    if ($service -eq 'gateway'){
        if (-not [string]::IsNullOrEmpty($deployEnv)){
            ExecKube -cmd "apply -f $service/$service-ingress-$deployEnv.yml"
            $getwayDomain = "gateway.${deployEnv}.surginghero.com"
        }else{
            ExecKube -cmd "apply -f $service/$service-ingress.yml"
        }
    }else{
        if (-not [string]::IsNullOrEmpty($deployEnv)){
            ExecKube -cmd "apply -f $service/$service-ingress.yml"
        }
    }
}

$deploys = (ExecKube -cmd 'get deploy -l=app=surging-hero'| %{ $_.Split(" ",[StringSplitOptions]"RemoveEmptyEntries")[0]})
for($i=1;$i -lt $deploys.Length; $i++) { 
    $deploy = $deploys[$i]
    $image_repository="${deploy}=${registryFDQN}${dockerOrg}/${deploy}:${imageTag}"
    if ($useImageProfix) {
        $image_repository="${deploy}=${registryFDQN}${dockerOrg}/${imageProfix}${deploy}:${imageTag}"
    }
    
    Write-Host "Update Image containers to use prefix '${registryFDQN}${dockerOrg}/${deploy}' and tag '$imageTag'" -ForegroundColor Yellow
    ExecKube -cmd "set image deployments/$deploy $image_repository"

    Write-Host "Execute rollout for $deploy..." -ForegroundColor Yellow
    ExecKube -cmd "rollout resume deployments/$deploy"

    if ($replicasNum -gt 1) {
        Write-Host "Execute scale for $deploy..." -ForegroundColor Yellow
        ExecKube -cmd "scale deploy ${deploy} --replicas=${replicasNum}"
    }
}
Write-Host "update hosts" -ForegroundColor Yellow
(Get-Content ./hosts) -replace "{ingress.ip}",$externalDns |
Set-Content ./hosts 
if (-not (Get-Content ./hosts).Contains("$externalDns $getwayDomain")){
    Add-Content -Path ./hosts -Value "$externalDns $getwayDomain"
}


