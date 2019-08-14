# 将服务部署到k8s集群

## Windows环境
1. 修改`.setting`文件的配置属性为当前主机的ip地址

2. 通过`deploy-ingress.ps1`脚本安装metallb插件和nginx-ingress插件
```powershell
./deploy-ingress.ps1
```

3. 通过`deploy.ps1`脚本部署surging.hero相关服务组件
```powershell
.\deploy.ps1 -imageTag v0.0.1  -deployEnv dev -replicasNum 2
```
4. 将`C:\Windows\System32\drivers\etc\hosts`替换为`./hosts`

> notes
> :todo 待完善

## Linux环境