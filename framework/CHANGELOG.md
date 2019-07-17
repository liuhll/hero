# Changelog
项目的所有变更日志均需要记录到该日志中

## [Unreleased]

## [2.0.0-bate.4] - 2019.07.18
### Added
- 新增surging打包脚本
- 新增Dapper、Domain、Validation等组件包


### Changed
- 重构异常处理
- 将消息返回结果数据名称重命名为Data
- 优化序列化器,默认使用camelCase风格
- 重构签发token的方式
- 支持通过RpcContext设置token的payload和获取payload