## 项目简介

1. 服务端基于.net5平台,开发框架是基于**surging.cloud**(**surging.cloud**是在surging基础上进行二次开发的微服务框架)
2. 采用mysql作为存储数据库、使用redis作为缓存数据库
3. 服务之间使用**dotnetty**实现的rpc框架进行通信,服务调用支持缓存缓存拦截,极大的提高了系统性能
4. 采用**zookeeper**作为服务注册中心;也支持consul作为服务注册中心(consul未经测试)
5. hero权限管理系统支持菜单、操作、数据权限
6. 前端使用vue2，基于**element-admin**框架进行开发
7. 项目地址：
   - 服务端: https://github.com/liuhll/hero
   - 前端: https://github.com/liuhll/hero-web


## surging.cloud
1. 增加异常处理模块，分为业务异常和非业务异常,rpc通信过程中,如果捕获到的是业务异常，被捕获之后服务消费者不会重试,且异常可以在服务之间进行传递,最终通过网关及时返回给调用者
2. 通过 `IdleStateHandler` 提供的心跳功能重构服务健康检查,提高rpc通信的可靠性
3. 通过 `Rabbit.Zookeeper` 重构基于zookeeper的服务注册中心,提高服务注册中心的可靠性
4. 修复通过`RpcContenxt`获取rpc通信上下文参数存在的bug,新增`ISurgingSession`获取rpc通信上下文参数【通过该接口可以获取当前登录用户的上下文信息】
5. 扩展了诸如Dapper、mongodb、AutoMapper、es、分布式锁等组件
6. 对rpc通信模型进行调整,生成的webapi支持restful架构风格
7. 使用JWT组件重构token签发方式、对登录认证、接口鉴权进行重构
8. 其他

## Demo部署
- 地址: http://www.liuhl-hero.top
- 账号:
  1. 管理员: admin 密码: 123qwe
  2. 普通用户: zhangsan 密码: 123qwe
  3. 使用admin登录后查看其它账号,密码均为:123qwe

## 服务介绍
当前共存在四个微服务:
1. 认证与授权服务(Auth): 负责用户、角色、用户组、权限管理,完成等用户的登录认证、接口鉴权等功能
2. 组织机构服务(Organization): 负责组织机构管理
3. 基础数据服务(BasicData): 提供基础数据和系统配置等
4. 网关(Gateway): 通过Kestrel主机对集群外部暴露Http访问端口，接收到客户端的请求后,将请求转到集群内部,与集群内部通过rpc进行通信

## todo
- [ ] hero支持租户、完成系统基础设置功能
- [ ] 完善开发者文档
- [ ] 完成hero的系统管理,支持查看微服务集群的服务状态、系统监控、服务链路跟踪、缓存数据等
- [ ] 封装文件上传服务、定时作业任务管理服务、日志管理服务等
- [ ] 封装基于ef的数据访问组件
- [ ] 封装分布式事务组件


