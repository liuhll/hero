Param(
    [parameter(Mandatory=$false)][string]$registry,
    [parameter(Mandatory=$false)][string]$imageTag,
    [parameter(Mandatory=$false)][bool]$buildImages=$true,
    [parameter(Mandatory=$false)][bool]$pushImages=$true,
    [parameter(Mandatory=$false)][string]$dockerOrg="surgingcloud"
)

$debugMode = $PSCmdlet.MyInvocation.BoundParameters["Debug"].IsPresent
$useDockerHub = [string]::IsNullOrEmpty($registry)

if ([string]::IsNullOrEmpty($imageTag)) {
    $imageTag = $(git rev-parse --abbrev-ref HEAD)
}


Write-Host "Docker 镜像 Tag: $imageTag" -ForegroundColor Yellow

if ($buildImages) {
    Write-Host "构建docker镜像,tag 标签为: '$imageTag'" -ForegroundColor Yellow
    $env:TAG=$imageTag
    docker-compose -p .. -f ../docker-compose/surging.hero/docker-compose.yml build    
}

if ($pushImages) {
    Write-Host "开始推送Image到镜像库 $registry/$dockerOrg..." -ForegroundColor Yellow
    $services = (Get-Content "./ServiceComponents")
    foreach($service in $services){
       $imageFqdn = if ($useDockerHub){"$dockerOrg/${service}"} elseif($dockerOrg) {"$registry/$dockerOrg/${service}"} else { "$registry/${service}"}
       docker tag ifocservice/${service}:$imageTag ${imageFqdn}:$imageTag
       docker push ${imageFqdn}:$imageTag  
    }
}