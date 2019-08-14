function ExecKube($cmd) {    
    $exp = $execPath + 'kubectl ' + $cmd
    Invoke-Expression $exp
}

function LoadConfig($settingFile) {
    $hashtable = @{}
    $payload = Get-Content -Path $settingFile |
    Where-Object { $_ -like '*=*' } |
    ForEach-Object {
       $infos = $_ -split '='
       $key = $infos[0].Trim()
       $value = $infos[1].Trim()
       $hashtable.$key = $value
    }
    return $hashtable 
}