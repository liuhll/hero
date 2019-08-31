Param(
    [parameter(Mandatory=$false)][string]$settingFile="./.setting"
)

. "./utilities.ps1"


$configSetting = LoadConfig -settingFile $settingFile

(Get-Content .\metallb\metallb-config.yml) -replace "{node1}",$configSetting.node1 -replace "{node2}",$configSetting.node2 |
Set-Content .\metallb\metallb-config.yml

ExecKube -cmd "apply -f metallb/metallb-config.yml -f metallb/metallb.yml -f nginx-ingress/cloud-generic.yml -f nginx-ingress/cm.yml -f nginx-ingress/mandatory.yml"