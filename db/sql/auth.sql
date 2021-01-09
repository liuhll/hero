/*==============================================================*/
/* DBMS name:      MySQL 5.0                                    */
/* Created on:     2021/1/7 9:36:04                             */
/*==============================================================*/

SET NAMES utf8mb4;
SET FOREIGN_KEY_CHECKS = 0;
drop database if exists hero_auth;
create database hero_auth;
use hero_auth;

drop table if exists Action;

drop table if exists File;

drop table if exists Menu;

drop table if exists Operation;

drop table if exists OperationActionRelation;

drop table if exists Permission;

drop table if exists Role;

drop table if exists RoleDataPermissionOrgRelation;

drop table if exists RoleOrganization;

drop table if exists RolePermission;

drop table if exists UserGroup;

drop table if exists UserGroupDataPermissionOrgRelation;

drop table if exists UserGroupOrganization;

drop table if exists UserGroupPermission;

drop table if exists UserGroupRole;

drop table if exists UserInfo;

drop table if exists UserRole;

drop table if exists UserUserGroupRelation;

drop table if exists Tenant;

/*==============================================================*/
/* Table: Action                                                */
/*==============================================================*/
create table Action
(
   Id                   bigint not null auto_increment comment '主键',
   ServiceId            varchar(500) not null comment '服务id',
   ServiceHost          varchar(50) not null comment '服务主机',
   Application          varchar(50) not null comment '所属应用服务',
   Name                 varchar(50) comment '名称',
   WebApi               varchar(50) not null comment 'webapi',
   Method               varchar(50) not null comment '请求方法',
   DisableNetwork       bit comment '是否禁用外网',
   EnableAuthorization  bit comment '是否需要认证',
   AllowPermission      bit comment '是否需要鉴权',
   Developer            varchar(50) comment '开发者',
   Date                 date comment '开发日期',
   Status               int not null comment '状态',
   CreateBy             bigint comment '创建人',
   CreateTime           datetime comment '创建日期',
   UpdateBy             bigint comment '修改人',
   UpdateTime           datetime comment '修改日期',
   primary key (Id),
   unique key AK_Key_2 (ServiceId)
);

alter table Action comment '功能表';

/*==============================================================*/
/* Table: File                                                  */
/*==============================================================*/
create table File
(
   Id                   bigint not null auto_increment comment '主键',
   PermissionId         bigint not null comment 'PermissionId',
   Name                 varchar(50) not null comment '名称',
   FIleName             varchar(50) not null,
   FilePath             varchar(100) not null,
   Memo                 varchar(100) comment '备注',
   Status               int not null comment '状态',
   CreateBy             bigint comment '创建人',
   CreateTime           datetime comment '创建日期',
   UpdateBy             bigint comment '修改人',
   UpdateTime           datetime comment '修改日期',
   IsDeleted            int comment '软删除标识',
   DeleteBy             bigint comment '删除用户',
   DeleteTime           datetime comment '删除时间',
   TenantId             bigint comment '租户Id',
   primary key (Id)
);

alter table File comment '文件表';

/*==============================================================*/
/* Table: Menu                                                  */
/*==============================================================*/
create table Menu
(
   Id                   bigint not null auto_increment comment '主键',
   PermissionId         bigint not null comment 'PermissionId',
   ParentId             bigint comment '父级菜单Id',
   Code                 varchar(200),
   Name                 varchar(50) not null comment '菜单标识',
   Title                varchar(50) not null comment '菜单名称',
   Level                int not null comment '层级',
   Path                 varchar(50) comment '前端菜单页面锚点',
   Mold                 int not null comment '菜单类型',
   AlwaysShow           bit not null comment '是否总是显示前端',
   Icon                 varchar(100) comment 'icon图标',
   Component            varchar(100) comment '前端组件',
   Sort                 int comment '排序',
   Memo                 varchar(100) comment '备注',
   CreateBy             bigint comment '创建人',
   CreateTime           datetime comment '创建日期',
   UpdateBy             bigint comment '修改人',
   UpdateTime           datetime comment '修改日期',
   IsDeleted            int comment '软删除标识',
   DeleteBy             bigint comment '删除用户',
   DeleteTime           datetime comment '删除时间',
   primary key (Id)
);

alter table Menu comment '菜单表';

/*==============================================================*/
/* Table: Operation                                             */
/*==============================================================*/
create table Operation
(
   Id                   bigint not null auto_increment comment '主键',
   PermissionId         bigint not null comment 'PermissionId',
   MenuId               bigint not null comment '菜单Id',
   Name                 varchar(50) not null comment '名称',
   Title                varchar(50) not null comment '菜单名称',
   Code                 varchar(50) not null comment '编码',
   Level                int not null comment '所属层级',
   Mold                 int not null comment '操作类型:1.增2.删3.改4.查5.其他操作',
   Sort                 int comment '排序',
   Memo                 varchar(100) comment '备注',
   CreateBy             bigint comment '创建人',
   CreateTime           datetime comment '创建日期',
   UpdateBy             bigint comment '修改人',
   UpdateTime           datetime comment '修改日期',
   IsDeleted            int comment '软删除标识',
   DeleteBy             bigint comment '删除用户',
   DeleteTime           datetime comment '删除时间',
   primary key (Id)
);

alter table Operation comment '操作表';

/*==============================================================*/
/* Table: OperationActionRelation                               */
/*==============================================================*/
create table OperationActionRelation
(
   Id                   bigint not null auto_increment comment '主键',
   OperationId          bigint not null comment '操作Id',
   ActionId             bigint not null comment 'ActionId',
   ServiceId            varchar(500) not null comment '服务Id',
   CreateBy             bigint comment '创建人',
   CreateTime           datetime comment '创建日期',
   UpdateBy             bigint comment '修改人',
   UpdateTime           datetime comment '修改日期',
   primary key (Id)
);

alter table OperationActionRelation comment '操作功能关系表';

/*==============================================================*/
/* Table: Permission                                            */
/*==============================================================*/
create table Permission
(
   Id                   bigint not null auto_increment comment '主键',
   Name                 varchar(50) not null comment '权限名称',
   Mold                 int not null comment '权限类型 1.菜单  2. 操作 3. 页面元素 4. 文件',
   Memo                 varchar(100) comment '备注',
   Status               int not null comment '状态',
   CreateBy             bigint comment '创建人',
   CreateTime           datetime comment '创建日期',
   UpdateBy             bigint comment '修改人',
   UpdateTime           datetime comment '修改日期',
   IsDeleted            int comment '软删除标识',
   DeleteBy             bigint comment '删除用户',
   DeleteTime           datetime comment '删除时间',
   primary key (Id)
);

alter table Permission comment '权限表';

/*==============================================================*/
/* Table: Role                                                  */
/*==============================================================*/
create table Role
(
   Id                   bigint not null auto_increment comment '主键',
   Identification       varchar(50) not null comment '唯一标识',
   IsAllOrg             bit not null default 1 comment '是否归属所有部门',
   Name                 varchar(50) not null comment '角色名称',
   Memo                 varchar(100) comment '备注',
   Status               int not null comment '状态',
   DataPermissionType   int not null default 1 comment '数据权限类型',
   CreateBy             bigint comment '创建人',
   CreateTime           datetime comment '创建日期',
   UpdateBy             bigint comment '修改人',
   UpdateTime           datetime comment '修改日期',
   IsDeleted            int comment '软删除标识',
   DeleteBy             bigint comment '删除用户',
   DeleteTime           datetime comment '删除时间',
   TenantId             bigint comment '租户Id',
   primary key (Id)
);

alter table Role comment '角色表';

/*==============================================================*/
/* Table: RoleDataPermissionOrgRelation                         */
/*==============================================================*/
create table RoleDataPermissionOrgRelation
(
   Id                   bigint not null auto_increment comment '主键',
   RoleId               bigint not null comment '角色Id',
   OrgId                bigint not null comment '组织机构Id',
   CreateBy             bigint comment '创建人',
   CreateTime           datetime comment '创建日期',
   UpdateBy             bigint comment '修改人',
   UpdateTime           datetime comment '修改日期',
   TenantId             bigint comment '租户Id',
   primary key (Id)
);

alter table RoleDataPermissionOrgRelation comment '角色的自定义数据权限表【查看的组织机构数据】';

/*==============================================================*/
/* Table: RoleOrganization                                      */
/*==============================================================*/
create table RoleOrganization
(
   Id                   bigint not null auto_increment comment '主键',
   RoleId               bigint not null comment '角色Id',
   OrgId                bigint not null comment '所属组织Id',
   CreateBy             bigint comment '创建人',
   CreateTime           datetime comment '创建日期',
   UpdateBy             bigint comment '修改人',
   UpdateTime           datetime comment '修改日期',
   TenantId             bigint comment '租户Id',
   primary key (Id)
);

alter table RoleOrganization comment '角色组织关系表';

/*==============================================================*/
/* Table: RolePermission                                        */
/*==============================================================*/
create table RolePermission
(
   Id                   bigint not null auto_increment comment '主键',
   RoleId               bigint not null,
   PermissionId         bigint not null,
   CreateBy             bigint comment '创建人',
   CreateTime           datetime comment '创建日期',
   UpdateBy             bigint comment '修改人',
   UpdateTime           datetime comment '修改日期',
   TenantId             bigint comment '租户Id',
   primary key (Id)
);

alter table RolePermission comment '角色权限表';

/*==============================================================*/
/* Table: UserGroup                                             */
/*==============================================================*/
create table UserGroup
(
   Id                   bigint not null auto_increment comment '主键',
   Identification       varchar(50) not null comment '唯一标识',
   IsAllOrg             bit not null default 1 comment '是否归属所有部门',
   Name                 varchar(50) not null comment '用户组名称',
   Memo                 varchar(200),
   Status               int not null comment '状态',
   DataPermissionType   int not null default 1 comment '数据权限类型',
   CreateBy             bigint comment '创建人',
   CreateTime           datetime comment '创建日期',
   UpdateBy             bigint comment '修改人',
   UpdateTime           datetime comment '修改日期',
   IsDeleted            int comment '软删除标识',
   DeleteBy             bigint comment '删除用户',
   DeleteTime           datetime comment '删除时间',
   TenantId             bigint comment '租户Id',
   primary key (Id)
);

alter table UserGroup comment '用户组表';

/*==============================================================*/
/* Table: UserGroupDataPermissionOrgRelation                    */
/*==============================================================*/
create table UserGroupDataPermissionOrgRelation
(
   Id                   bigint not null auto_increment comment '主键',
   UserGroupId          bigint not null comment '用户组Id',
   OrgId                bigint not null comment '组织机构Id',
   CreateBy             bigint comment '创建人',
   CreateTime           datetime comment '创建日期',
   UpdateBy             bigint comment '修改人',
   UpdateTime           datetime comment '修改日期',
   TenantId             bigint comment '租户Id',
   primary key (Id)
);

alter table UserGroupDataPermissionOrgRelation comment '用户组的自定义数据权限表【查看的组织机构数据】';

/*==============================================================*/
/* Table: UserGroupOrganization                                 */
/*==============================================================*/
create table UserGroupOrganization
(
   Id                   bigint not null auto_increment comment '主键',
   UserGroupId          bigint not null comment '用户组Id',
   OrgId                bigint not null comment '所属组织Id',
   CreateBy             bigint comment '创建人',
   CreateTime           datetime comment '创建日期',
   UpdateBy             bigint comment '修改人',
   UpdateTime           datetime comment '修改日期',
   TenantId             bigint comment '租户Id',
   primary key (Id)
);

alter table UserGroupOrganization comment '用户组组织关系表';

/*==============================================================*/
/* Table: UserGroupPermission                                   */
/*==============================================================*/
create table UserGroupPermission
(
   Id                   bigint not null auto_increment comment '主键',
   UserGroupId          bigint not null,
   PermissionId         bigint not null,
   CreateBy             bigint comment '创建人',
   CreateTime           datetime comment '创建日期',
   UpdateBy             bigint comment '修改人',
   UpdateTime           datetime comment '修改日期',
   TenantId             bigint comment '租户Id',
   primary key (Id)
);

alter table UserGroupPermission comment '用户组权限表';

/*==============================================================*/
/* Table: UserGroupRole                                         */
/*==============================================================*/
create table UserGroupRole
(
   Id                   bigint not null auto_increment comment '主键',
   UserGroupId          bigint not null,
   RoleId               bigint not null,
   CreateBy             bigint comment '创建人',
   CreateTime           datetime comment '创建日期',
   UpdateBy             bigint comment '修改人',
   UpdateTime           datetime comment '修改日期',
   TenantId             bigint comment '租户Id',
   primary key (Id)
);

alter table UserGroupRole comment '用户组角色关系表';

/*==============================================================*/
/* Table: UserInfo                                              */
/*==============================================================*/
create table UserInfo
(
   Id                   bigint not null auto_increment comment '主键',
   UserName             varchar(50) not null comment '用户名',
   OrgId                bigint comment '所属部门Id',
   PositionId           bigint comment '职位Id',
   Password             varchar(100) not null comment '密码',
   ChineseName          varchar(50) not null comment '中文名',
   Email                varchar(50) comment '电子邮件',
   Phone                varchar(22) comment '联系电话',
   Gender               int comment '性别',
   Birth                date comment '生日',
   NativePlace          varchar(50) comment '籍贯',
   Address              varchar(100) comment '住址',
   Folk                 varchar(50) comment '民族',
   PoliticalStatus      int comment '政治面貌',
   GraduateInstitutions varchar(50) comment '毕业院校',
   Education            varchar(50) comment '学历',
   Major                varchar(50) comment '专业',
   Resume               varchar(500) comment '简历',
   Memo                 varchar(500) comment '备注',
   LastLoginTime        datetime comment '最后登录时间',
   LoginFailedCount     int not null comment '登录失败次数',
   Status               int not null comment '状态',
   CreateBy             bigint comment '创建人',
   CreateTime           datetime comment '创建日期',
   UpdateBy             bigint comment '修改人',
   UpdateTime           datetime comment '修改日期',
   IsDeleted            int comment '软删除标识',
   DeleteBy             bigint comment '删除用户',
   DeleteTime           datetime comment '删除时间',
   TenantId             bigint comment '租户Id',
   primary key (Id)
);

alter table UserInfo comment '用户表';

/*==============================================================*/
/* Table: UserRole                                              */
/*==============================================================*/
create table UserRole
(
   Id                   bigint not null auto_increment comment '主键',
   UserId               bigint not null,
   RoleId               bigint not null,
   CreateBy             bigint comment '创建人',
   CreateTime           datetime comment '创建日期',
   UpdateBy             bigint comment '修改人',
   UpdateTime           datetime comment '修改日期',
   TenantId             bigint comment '租户Id',
   primary key (Id)
);

alter table UserRole comment '用户角色关系表';

/*==============================================================*/
/* Table: UserUserGroupRelation                                 */
/*==============================================================*/
create table UserUserGroupRelation
(
   Id                   bigint not null auto_increment comment '主键',
   UserId               bigint not null,
   UserGroupId          bigint not null,
   CreateBy             bigint comment '创建人',
   CreateTime           datetime comment '创建日期',
   UpdateBy             bigint comment '修改人',
   UpdateTime           datetime comment '修改日期',
   TenantId             bigint comment '租户Id',
   primary key (Id)
);

alter table UserUserGroupRelation comment '用户与用户关系表';

/*==============================================================*/
/* Table: Tenant                                                */
/*==============================================================*/
create table Tenant
(
   Id                   bigint not null auto_increment comment '主键',
   Name                 varchar(50) not null comment '名称',
   Memo                 varchar(100) comment '备注',
   Status               int not null comment '状态',
   CreateBy             bigint comment '创建人',
   CreateTime           datetime comment '创建日期',
   UpdateBy             bigint comment '修改人',
   UpdateTime           datetime comment '修改日期',
   IsDeleted            int comment '软删除标识',
   DeleteBy             bigint comment '删除用户',
   DeleteTime           datetime comment '删除时间',
   primary key (Id)
);

alter table Tenant comment '租户表';


INSERT INTO `hero_auth`.`Action`(`Id`, `ServiceId`, `ServiceHost`, `Application`, `Name`, `WebApi`, `Method`, `DisableNetwork`, `EnableAuthorization`, `AllowPermission`, `Developer`, `Date`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (1, 'Surging.Core.CPlatform.Module.IEchoService.Locate_key_routePath_httpMethod', 'Surging.Core.CPlatform', 'IEchoService', NULL, '/locate', 'GET', b'0', b'1', b'0', NULL, NULL, 1, 1, '2020-12-14 16:21:26', 1, '2020-12-23 15:16:24');
INSERT INTO `hero_auth`.`Action`(`Id`, `ServiceId`, `ServiceHost`, `Application`, `Name`, `WebApi`, `Method`, `DisableNetwork`, `EnableAuthorization`, `AllowPermission`, `Developer`, `Date`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (2, 'Surging.Hero.Auth.IApplication.User.IUserAppService.Create_input', 'Surging.Hero.Auth', 'IUserAppService', '新增用户', 'api/user', 'POST', b'0', b'1', b'0', '刘洪亮', '2020-07-04', 1, 1, '2020-12-14 16:21:27', 1, '2021-01-07 21:32:35');
INSERT INTO `hero_auth`.`Action`(`Id`, `ServiceId`, `ServiceHost`, `Application`, `Name`, `WebApi`, `Method`, `DisableNetwork`, `EnableAuthorization`, `AllowPermission`, `Developer`, `Date`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (3, 'Surging.Hero.Auth.IApplication.User.IUserAppService.Update_input', 'Surging.Hero.Auth', 'IUserAppService', '更新用户', 'api/user', 'PUT', b'0', b'1', b'0', '刘洪亮', '2020-07-04', 1, 1, '2020-12-14 16:21:27', 1, '2021-01-07 21:32:35');
INSERT INTO `hero_auth`.`Action`(`Id`, `ServiceId`, `ServiceHost`, `Application`, `Name`, `WebApi`, `Method`, `DisableNetwork`, `EnableAuthorization`, `AllowPermission`, `Developer`, `Date`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (4, 'Surging.Hero.Auth.IApplication.User.IUserAppService.Delete_id', 'Surging.Hero.Auth', 'IUserAppService', '删除用户', 'api/user/{id}', 'DELETE', b'0', b'1', b'0', '刘洪亮', '2020-07-04', 1, 1, '2020-12-14 16:21:27', 1, '2021-01-07 21:32:35');
INSERT INTO `hero_auth`.`Action`(`Id`, `ServiceId`, `ServiceHost`, `Application`, `Name`, `WebApi`, `Method`, `DisableNetwork`, `EnableAuthorization`, `AllowPermission`, `Developer`, `Date`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (5, 'Surging.Hero.Auth.IApplication.User.IUserAppService.Search_query', 'Surging.Hero.Auth', 'IUserAppService', '查询用户', 'api/user/search', 'POST', b'0', b'1', b'0', '刘洪亮', '2020-07-04', 1, 1, '2020-12-14 16:21:27', 1, '2021-01-07 21:32:35');
INSERT INTO `hero_auth`.`Action`(`Id`, `ServiceId`, `ServiceHost`, `Application`, `Name`, `WebApi`, `Method`, `DisableNetwork`, `EnableAuthorization`, `AllowPermission`, `Developer`, `Date`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (6, 'Surging.Hero.Auth.IApplication.User.IUserAppService.UpdateStatus_input', 'Surging.Hero.Auth', 'IUserAppService', '激活/冻结用户', 'api/user/status', 'PUT', b'0', b'1', b'0', '刘洪亮', '2020-07-04', 1, 1, '2020-12-14 16:21:27', 1, '2021-01-07 21:32:35');
INSERT INTO `hero_auth`.`Action`(`Id`, `ServiceId`, `ServiceHost`, `Application`, `Name`, `WebApi`, `Method`, `DisableNetwork`, `EnableAuthorization`, `AllowPermission`, `Developer`, `Date`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (7, 'Surging.Hero.Auth.IApplication.User.IUserAppService.ResetPassword_input', 'Surging.Hero.Auth', 'IUserAppService', '重置密码', 'api/user/password', 'PUT', b'0', b'1', b'0', '刘洪亮', '2020-07-04', 1, 1, '2020-12-14 16:21:27', 1, '2021-01-07 21:32:35');
INSERT INTO `hero_auth`.`Action`(`Id`, `ServiceId`, `ServiceHost`, `Application`, `Name`, `WebApi`, `Method`, `DisableNetwork`, `EnableAuthorization`, `AllowPermission`, `Developer`, `Date`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (8, 'Surging.Hero.Auth.IApplication.User.IUserAppService.GetOrgUser_orgId_includeSubOrg', 'Surging.Hero.Auth', 'IUserAppService', '根据组织id获取部门的用户', 'api/user/org', 'GET', b'1', b'1', b'0', '刘洪亮', '2020-07-04', 1, 1, '2020-12-14 16:21:27', 1, '2021-01-07 21:32:36');
INSERT INTO `hero_auth`.`Action`(`Id`, `ServiceId`, `ServiceHost`, `Application`, `Name`, `WebApi`, `Method`, `DisableNetwork`, `EnableAuthorization`, `AllowPermission`, `Developer`, `Date`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (9, 'Surging.Hero.Auth.IApplication.User.IUserAppService.Get_id', 'Surging.Hero.Auth', 'IUserAppService', '获取用户信息', 'api/user/{id}', 'GET', b'0', b'1', b'1', '刘洪亮', '2020-07-04', 1, 1, '2020-12-14 16:21:27', 1, '2021-01-07 21:32:36');
INSERT INTO `hero_auth`.`Action`(`Id`, `ServiceId`, `ServiceHost`, `Application`, `Name`, `WebApi`, `Method`, `DisableNetwork`, `EnableAuthorization`, `AllowPermission`, `Developer`, `Date`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (10, 'Surging.Hero.Auth.IApplication.User.IUserAppService.ResetUserOrgInfo_id', 'Surging.Hero.Auth', 'IUserAppService', '修改某个用户的部门信息', 'api/user/user/org/{id}', 'POST', b'1', b'1', b'0', '刘洪亮', '2020-07-04', 1, 1, '2020-12-14 16:21:27', 1, '2021-01-07 21:32:36');
INSERT INTO `hero_auth`.`Action`(`Id`, `ServiceId`, `ServiceHost`, `Application`, `Name`, `WebApi`, `Method`, `DisableNetwork`, `EnableAuthorization`, `AllowPermission`, `Developer`, `Date`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (11, 'Surging.Hero.Auth.IApplication.User.IUserAppService.GetPositionUserCount_positionId', 'Surging.Hero.Auth', 'IUserAppService', '获取某个职位的用户数', 'api/user/position/count/{positionId}', 'GET', b'0', b'1', b'0', '刘洪亮', '2020-07-04', 1, 1, '2020-12-14 16:21:27', 1, '2021-01-07 21:32:36');
INSERT INTO `hero_auth`.`Action`(`Id`, `ServiceId`, `ServiceHost`, `Application`, `Name`, `WebApi`, `Method`, `DisableNetwork`, `EnableAuthorization`, `AllowPermission`, `Developer`, `Date`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (12, 'Surging.Hero.Auth.IApplication.User.IUserAppService.GetUserBasicInfo_id', 'Surging.Hero.Auth', 'IUserAppService', '获取某个用户的基本信息【不包含角色信息,有缓存】', 'api/user/user/basic/{id}', 'GET', b'1', b'1', b'1', '刘洪亮', '2020-12-11', 1, 1, '2020-12-14 16:21:27', 1, '2021-01-07 21:32:36');
INSERT INTO `hero_auth`.`Action`(`Id`, `ServiceId`, `ServiceHost`, `Application`, `Name`, `WebApi`, `Method`, `DisableNetwork`, `EnableAuthorization`, `AllowPermission`, `Developer`, `Date`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (13, 'Surging.Hero.Auth.IApplication.UserGroup.IUserGroupAppService.Create_input', 'Surging.Hero.Auth', 'IUserGroupAppService', '新增用户组', 'api/usergroup', 'POST', b'0', b'1', b'0', '刘洪亮', '2020-07-04', 1, 1, '2020-12-14 16:21:27', 1, '2021-01-07 21:32:33');
INSERT INTO `hero_auth`.`Action`(`Id`, `ServiceId`, `ServiceHost`, `Application`, `Name`, `WebApi`, `Method`, `DisableNetwork`, `EnableAuthorization`, `AllowPermission`, `Developer`, `Date`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (14, 'Surging.Hero.Auth.IApplication.UserGroup.IUserGroupAppService.Update_input', 'Surging.Hero.Auth', 'IUserGroupAppService', '更新用户组', 'api/usergroup', 'PUT', b'0', b'1', b'0', '刘洪亮', '2020-07-04', 1, 1, '2020-12-14 16:21:27', 1, '2021-01-07 21:32:33');
INSERT INTO `hero_auth`.`Action`(`Id`, `ServiceId`, `ServiceHost`, `Application`, `Name`, `WebApi`, `Method`, `DisableNetwork`, `EnableAuthorization`, `AllowPermission`, `Developer`, `Date`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (15, 'Surging.Hero.Auth.IApplication.UserGroup.IUserGroupAppService.UpdateStatus_input', 'Surging.Hero.Auth', 'IUserGroupAppService', '冻结/激活用户组状态', 'api/usergroup/status', 'PUT', b'0', b'1', b'0', '刘洪亮', '2020-07-04', 1, 1, '2020-12-14 16:21:27', 1, '2021-01-07 21:32:34');
INSERT INTO `hero_auth`.`Action`(`Id`, `ServiceId`, `ServiceHost`, `Application`, `Name`, `WebApi`, `Method`, `DisableNetwork`, `EnableAuthorization`, `AllowPermission`, `Developer`, `Date`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (16, 'Surging.Hero.Auth.IApplication.UserGroup.IUserGroupAppService.Get_id', 'Surging.Hero.Auth', 'IUserGroupAppService', '获取用户组信息', 'api/usergroup/{id}', 'GET', b'0', b'1', b'1', '刘洪亮', '2020-07-04', 1, 1, '2020-12-14 16:21:27', 1, '2021-01-07 21:32:34');
INSERT INTO `hero_auth`.`Action`(`Id`, `ServiceId`, `ServiceHost`, `Application`, `Name`, `WebApi`, `Method`, `DisableNetwork`, `EnableAuthorization`, `AllowPermission`, `Developer`, `Date`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (17, 'Surging.Hero.Auth.IApplication.UserGroup.IUserGroupAppService.Delete_id', 'Surging.Hero.Auth', 'IUserGroupAppService', '删除用户组', 'api/usergroup/{id}', 'DELETE', b'0', b'1', b'0', '刘洪亮', '2020-07-04', 1, 1, '2020-12-14 16:21:27', 1, '2021-01-07 21:32:34');
INSERT INTO `hero_auth`.`Action`(`Id`, `ServiceId`, `ServiceHost`, `Application`, `Name`, `WebApi`, `Method`, `DisableNetwork`, `EnableAuthorization`, `AllowPermission`, `Developer`, `Date`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (18, 'Surging.Hero.Auth.IApplication.UserGroup.IUserGroupAppService.Search_query', 'Surging.Hero.Auth', 'IUserGroupAppService', '查询用户组', 'api/usergroup/search', 'POST', b'0', b'1', b'0', '刘洪亮', '2020-12-08', 1, 1, '2020-12-14 16:21:27', 1, '2021-01-07 21:32:34');
INSERT INTO `hero_auth`.`Action`(`Id`, `ServiceId`, `ServiceHost`, `Application`, `Name`, `WebApi`, `Method`, `DisableNetwork`, `EnableAuthorization`, `AllowPermission`, `Developer`, `Date`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (19, 'Surging.Hero.Auth.IApplication.UserGroup.IUserGroupAppService.AllocationUsers_input', 'Surging.Hero.Auth', 'IUserGroupAppService', '添加用户组用户', 'api/usergroup/users', 'POST', b'0', b'1', b'0', '刘洪亮', '2020-12-08', 1, 1, '2020-12-14 16:21:27', 1, '2021-01-07 21:32:34');
INSERT INTO `hero_auth`.`Action`(`Id`, `ServiceId`, `ServiceHost`, `Application`, `Name`, `WebApi`, `Method`, `DisableNetwork`, `EnableAuthorization`, `AllowPermission`, `Developer`, `Date`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (20, 'Surging.Hero.Auth.IApplication.UserGroup.IUserGroupAppService.SearchUserGroupUser_query', 'Surging.Hero.Auth', 'IUserGroupAppService', '查询用户组用户', 'api/usergroup/users/search', 'POST', b'0', b'1', b'0', '刘洪亮', '2020-12-08', 1, 1, '2020-12-14 16:21:27', 1, '2021-01-07 21:32:34');
INSERT INTO `hero_auth`.`Action`(`Id`, `ServiceId`, `ServiceHost`, `Application`, `Name`, `WebApi`, `Method`, `DisableNetwork`, `EnableAuthorization`, `AllowPermission`, `Developer`, `Date`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (21, 'Surging.Hero.Auth.IApplication.UserGroup.IUserGroupAppService.DeleteUserGroupUser_input', 'Surging.Hero.Auth', 'IUserGroupAppService', '删除用户组用户', 'api/usergroup/users', 'DELETE', b'0', b'1', b'0', '刘洪亮', '2020-12-08', 1, 1, '2020-12-14 16:21:27', 1, '2021-01-07 21:32:34');
INSERT INTO `hero_auth`.`Action`(`Id`, `ServiceId`, `ServiceHost`, `Application`, `Name`, `WebApi`, `Method`, `DisableNetwork`, `EnableAuthorization`, `AllowPermission`, `Developer`, `Date`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (22, 'Surging.Hero.Auth.IApplication.Role.IRoleAppService.Create_input', 'Surging.Hero.Auth', 'IRoleAppService', '新增角色', 'api/role', 'POST', b'0', b'1', b'0', '刘洪亮', '2020-07-04', 1, 1, '2020-12-14 16:21:27', 1, '2021-01-07 21:32:38');
INSERT INTO `hero_auth`.`Action`(`Id`, `ServiceId`, `ServiceHost`, `Application`, `Name`, `WebApi`, `Method`, `DisableNetwork`, `EnableAuthorization`, `AllowPermission`, `Developer`, `Date`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (23, 'Surging.Hero.Auth.IApplication.Role.IRoleAppService.Update_input', 'Surging.Hero.Auth', 'IRoleAppService', '更新角色', 'api/role', 'PUT', b'0', b'1', b'0', '刘洪亮', '2020-07-04', 1, 1, '2020-12-14 16:21:27', 1, '2021-01-07 21:32:38');
INSERT INTO `hero_auth`.`Action`(`Id`, `ServiceId`, `ServiceHost`, `Application`, `Name`, `WebApi`, `Method`, `DisableNetwork`, `EnableAuthorization`, `AllowPermission`, `Developer`, `Date`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (24, 'Surging.Hero.Auth.IApplication.Role.IRoleAppService.Status_input', 'Surging.Hero.Auth', 'IRoleAppService', '激活/冻结角色', 'api/role/status', 'PUT', b'0', b'1', b'0', '刘洪亮', '2020-07-04', 1, 1, '2020-12-14 16:21:27', 1, '2021-01-07 21:32:38');
INSERT INTO `hero_auth`.`Action`(`Id`, `ServiceId`, `ServiceHost`, `Application`, `Name`, `WebApi`, `Method`, `DisableNetwork`, `EnableAuthorization`, `AllowPermission`, `Developer`, `Date`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (25, 'Surging.Hero.Auth.IApplication.Role.IRoleAppService.Get_id', 'Surging.Hero.Auth', 'IRoleAppService', '获取角色信息', 'api/role/{id}', 'GET', b'0', b'1', b'1', '刘洪亮', '2020-07-04', 1, 1, '2020-12-14 16:21:27', 1, '2021-01-07 21:32:38');
INSERT INTO `hero_auth`.`Action`(`Id`, `ServiceId`, `ServiceHost`, `Application`, `Name`, `WebApi`, `Method`, `DisableNetwork`, `EnableAuthorization`, `AllowPermission`, `Developer`, `Date`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (26, 'Surging.Hero.Auth.IApplication.Role.IRoleAppService.Search_query', 'Surging.Hero.Auth', 'IRoleAppService', '查询角色', 'api/role/search', 'POST', b'0', b'1', b'0', '刘洪亮', '2020-07-04', 1, 1, '2020-12-14 16:21:27', 1, '2021-01-07 21:32:38');
INSERT INTO `hero_auth`.`Action`(`Id`, `ServiceId`, `ServiceHost`, `Application`, `Name`, `WebApi`, `Method`, `DisableNetwork`, `EnableAuthorization`, `AllowPermission`, `Developer`, `Date`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (27, 'Surging.Hero.Auth.IApplication.Role.IRoleAppService.List_searchKey', 'Surging.Hero.Auth', 'IRoleAppService', '获取角色列表', 'api/role/list', 'GET', b'0', b'1', b'1', '刘洪亮', '2020-07-04', 1, 1, '2020-12-14 16:21:27', 1, '2021-01-07 21:32:38');
INSERT INTO `hero_auth`.`Action`(`Id`, `ServiceId`, `ServiceHost`, `Application`, `Name`, `WebApi`, `Method`, `DisableNetwork`, `EnableAuthorization`, `AllowPermission`, `Developer`, `Date`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (28, 'Surging.Hero.Auth.IApplication.Role.IRoleAppService.Delete_id', 'Surging.Hero.Auth', 'IRoleAppService', '删除角色', 'api/role/{id}', 'DELETE', b'0', b'1', b'0', '刘洪亮', '2020-07-04', 1, 1, '2020-12-14 16:21:27', 1, '2021-01-07 21:32:39');
INSERT INTO `hero_auth`.`Action`(`Id`, `ServiceId`, `ServiceHost`, `Application`, `Name`, `WebApi`, `Method`, `DisableNetwork`, `EnableAuthorization`, `AllowPermission`, `Developer`, `Date`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (29, 'Surging.Hero.Auth.IApplication.Permission.IPermissionAppService.CreateMenu_input', 'Surging.Hero.Auth', 'IPermissionAppService', '新增菜单', 'api/permission/menu', 'POST', b'0', b'1', b'0', '刘洪亮', '2020-07-04', 1, 1, '2020-12-14 16:21:27', 1, '2021-01-07 21:32:39');
INSERT INTO `hero_auth`.`Action`(`Id`, `ServiceId`, `ServiceHost`, `Application`, `Name`, `WebApi`, `Method`, `DisableNetwork`, `EnableAuthorization`, `AllowPermission`, `Developer`, `Date`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (30, 'Surging.Hero.Auth.IApplication.Permission.IPermissionAppService.UpdateMenu_input', 'Surging.Hero.Auth', 'IPermissionAppService', '编辑菜单', 'api/permission/menu', 'PUT', b'0', b'1', b'0', '刘洪亮', '2020-07-04', 1, 1, '2020-12-14 16:21:27', 1, '2021-01-07 21:32:39');
INSERT INTO `hero_auth`.`Action`(`Id`, `ServiceId`, `ServiceHost`, `Application`, `Name`, `WebApi`, `Method`, `DisableNetwork`, `EnableAuthorization`, `AllowPermission`, `Developer`, `Date`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (31, 'Surging.Hero.Auth.IApplication.Permission.IPermissionAppService.GetMenu_permissionId', 'Surging.Hero.Auth', 'IPermissionAppService', '根据权限id获取菜单信息', 'api/permission/menu/{permissionId}', 'GET', b'0', b'1', b'1', '刘洪亮', '2020-07-04', 1, 1, '2020-12-14 16:21:27', 1, '2021-01-07 21:32:39');
INSERT INTO `hero_auth`.`Action`(`Id`, `ServiceId`, `ServiceHost`, `Application`, `Name`, `WebApi`, `Method`, `DisableNetwork`, `EnableAuthorization`, `AllowPermission`, `Developer`, `Date`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (32, 'Surging.Hero.Auth.IApplication.Permission.IPermissionAppService.CreateOperation_input', 'Surging.Hero.Auth', 'IPermissionAppService', '新增操作', 'api/permission/operation', 'POST', b'0', b'1', b'0', '刘洪亮', '2020-07-04', 1, 1, '2020-12-14 16:21:27', 1, '2021-01-07 21:32:39');
INSERT INTO `hero_auth`.`Action`(`Id`, `ServiceId`, `ServiceHost`, `Application`, `Name`, `WebApi`, `Method`, `DisableNetwork`, `EnableAuthorization`, `AllowPermission`, `Developer`, `Date`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (33, 'Surging.Hero.Auth.IApplication.Permission.IPermissionAppService.UpdateOperation_input', 'Surging.Hero.Auth', 'IPermissionAppService', '更新操作', 'api/permission/operation', 'PUT', b'0', b'1', b'0', '刘洪亮', '2020-07-04', 1, 1, '2020-12-14 16:21:28', 1, '2021-01-07 21:32:39');
INSERT INTO `hero_auth`.`Action`(`Id`, `ServiceId`, `ServiceHost`, `Application`, `Name`, `WebApi`, `Method`, `DisableNetwork`, `EnableAuthorization`, `AllowPermission`, `Developer`, `Date`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (34, 'Surging.Hero.Auth.IApplication.Permission.IPermissionAppService.GetOperation_permissionId', 'Surging.Hero.Auth', 'IPermissionAppService', '根据权限id获取操作', 'api/permission/operation/{permissionId}', 'GET', b'0', b'1', b'1', '刘洪亮', '2020-07-04', 1, 1, '2020-12-14 16:21:28', 1, '2021-01-07 21:32:40');
INSERT INTO `hero_auth`.`Action`(`Id`, `ServiceId`, `ServiceHost`, `Application`, `Name`, `WebApi`, `Method`, `DisableNetwork`, `EnableAuthorization`, `AllowPermission`, `Developer`, `Date`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (35, 'Surging.Hero.Auth.IApplication.Permission.IPermissionAppService.GetTree', 'Surging.Hero.Auth', 'IPermissionAppService', '获取权限树', 'api/permission/tree', 'GET', b'0', b'1', b'1', '刘洪亮', '2020-07-04', 1, 1, '2020-12-14 16:21:28', 1, '2021-01-07 21:32:40');
INSERT INTO `hero_auth`.`Action`(`Id`, `ServiceId`, `ServiceHost`, `Application`, `Name`, `WebApi`, `Method`, `DisableNetwork`, `EnableAuthorization`, `AllowPermission`, `Developer`, `Date`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (36, 'Surging.Hero.Auth.IApplication.Permission.IPermissionAppService.Delete_input', 'Surging.Hero.Auth', 'IPermissionAppService', '删除菜单/操作', 'api/permission', 'DELETE', b'0', b'1', b'0', '刘洪亮', '2020-07-04', 1, 1, '2020-12-14 16:21:28', 1, '2021-01-07 21:32:40');
INSERT INTO `hero_auth`.`Action`(`Id`, `ServiceId`, `ServiceHost`, `Application`, `Name`, `WebApi`, `Method`, `DisableNetwork`, `EnableAuthorization`, `AllowPermission`, `Developer`, `Date`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (37, 'Surging.Hero.Auth.IApplication.Permission.IPermissionAppService.Check_serviceId', 'Surging.Hero.Auth', 'IPermissionAppService', '接口鉴权', 'api/permission/check', 'POST', b'1', b'1', b'0', '刘洪亮', NULL, 1, 1, '2020-12-14 16:21:28', 1, '2021-01-07 21:32:40');
INSERT INTO `hero_auth`.`Action`(`Id`, `ServiceId`, `ServiceHost`, `Application`, `Name`, `WebApi`, `Method`, `DisableNetwork`, `EnableAuthorization`, `AllowPermission`, `Developer`, `Date`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (38, 'Surging.Hero.Auth.IApplication.Authorization.IAccountAppService.Login_input', 'Surging.Hero.Auth', 'IAccountAppService', '用户登录接口', 'api/account/login', 'POST', b'0', b'0', b'0', '刘洪亮', '2019-07-14', 1, 1, '2020-12-14 16:21:28', 1, '2021-01-07 21:32:40');
INSERT INTO `hero_auth`.`Action`(`Id`, `ServiceId`, `ServiceHost`, `Application`, `Name`, `WebApi`, `Method`, `DisableNetwork`, `EnableAuthorization`, `AllowPermission`, `Developer`, `Date`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (39, 'Surging.Hero.Auth.IApplication.Authorization.IAccountAppService.GetLoginUser', 'Surging.Hero.Auth', 'IAccountAppService', '获取登录用户信息', 'api/account/userinfo', 'GET', b'0', b'1', b'1', '刘洪亮', '2019-09-21', 1, 1, '2020-12-14 16:21:28', 1, '2021-01-07 21:32:41');
INSERT INTO `hero_auth`.`Action`(`Id`, `ServiceId`, `ServiceHost`, `Application`, `Name`, `WebApi`, `Method`, `DisableNetwork`, `EnableAuthorization`, `AllowPermission`, `Developer`, `Date`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (40, 'Surging.Hero.Auth.IApplication.Authorization.IAccountAppService.GetUserTreeMenu', 'Surging.Hero.Auth', 'IAccountAppService', '获取登录用户菜单权限列表', 'api/account/menu/tree', 'GET', b'0', b'1', b'1', '刘洪亮', '2019-09-23', 1, 1, '2020-12-14 16:21:28', 1, '2021-01-07 21:32:41');
INSERT INTO `hero_auth`.`Action`(`Id`, `ServiceId`, `ServiceHost`, `Application`, `Name`, `WebApi`, `Method`, `DisableNetwork`, `EnableAuthorization`, `AllowPermission`, `Developer`, `Date`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (41, 'Surging.Hero.Auth.IApplication.Authorization.IAccountAppService.GetUserMenu', 'Surging.Hero.Auth', 'IAccountAppService', '获取登录用户菜单权限列表', 'api/account/menu', 'GET', b'0', b'1', b'1', '刘洪亮', '2019-09-23', 1, 1, '2020-12-14 16:21:28', 1, '2021-01-07 21:32:41');
INSERT INTO `hero_auth`.`Action`(`Id`, `ServiceId`, `ServiceHost`, `Application`, `Name`, `WebApi`, `Method`, `DisableNetwork`, `EnableAuthorization`, `AllowPermission`, `Developer`, `Date`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (42, 'Surging.Hero.Auth.IApplication.Authorization.IAccountAppService.GetUserOperation_menuId', 'Surging.Hero.Auth', 'IAccountAppService', '通过菜单Id获取用户的操作权限', 'api/account/operation/{menuId}', 'GET', b'0', b'1', b'1', '刘洪亮', '2019-09-23', 1, 1, '2020-12-14 16:21:28', 1, '2021-01-07 21:32:41');
INSERT INTO `hero_auth`.`Action`(`Id`, `ServiceId`, `ServiceHost`, `Application`, `Name`, `WebApi`, `Method`, `DisableNetwork`, `EnableAuthorization`, `AllowPermission`, `Developer`, `Date`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (43, 'Surging.Hero.Auth.IApplication.Action.IActionAppService.InitActions_actions', 'Surging.Hero.Auth', 'IActionAppService', '初始化服务方法', 'api/action/init', 'POST', b'1', b'1', b'0', '刘洪亮', '2020-07-04', 1, 1, '2020-12-14 16:21:28', 1, '2021-01-07 21:32:41');
INSERT INTO `hero_auth`.`Action`(`Id`, `ServiceId`, `ServiceHost`, `Application`, `Name`, `WebApi`, `Method`, `DisableNetwork`, `EnableAuthorization`, `AllowPermission`, `Developer`, `Date`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (44, 'Surging.Hero.Auth.IApplication.Action.IActionAppService.InitAllActions', 'Surging.Hero.Auth', 'IActionAppService', '初始化所有服务方法', 'api/action/init/all', 'POST', b'0', b'1', b'1', '刘洪亮', '2020-07-04', 1, 1, '2020-12-14 16:21:28', 1, '2021-01-07 21:32:41');
INSERT INTO `hero_auth`.`Action`(`Id`, `ServiceId`, `ServiceHost`, `Application`, `Name`, `WebApi`, `Method`, `DisableNetwork`, `EnableAuthorization`, `AllowPermission`, `Developer`, `Date`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (45, 'Surging.Hero.Auth.IApplication.Action.IActionAppService.GetServiceHosts_query', 'Surging.Hero.Auth', 'IActionAppService', '获取微服务主机列表', 'api/action/host/search', 'POST', b'0', b'1', b'1', '刘洪亮', '2020-07-04', 1, 1, '2020-12-14 16:21:28', 1, '2021-01-07 21:32:42');
INSERT INTO `hero_auth`.`Action`(`Id`, `ServiceId`, `ServiceHost`, `Application`, `Name`, `WebApi`, `Method`, `DisableNetwork`, `EnableAuthorization`, `AllowPermission`, `Developer`, `Date`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (46, 'Surging.Hero.Auth.IApplication.Action.IActionAppService.GetAppServices_query', 'Surging.Hero.Auth', 'IActionAppService', '获取应用服务', 'api/action/service/search', 'POST', b'0', b'1', b'1', '刘洪亮', '2020-07-04', 1, 1, '2020-12-14 16:21:28', 1, '2021-01-07 21:32:42');
INSERT INTO `hero_auth`.`Action`(`Id`, `ServiceId`, `ServiceHost`, `Application`, `Name`, `WebApi`, `Method`, `DisableNetwork`, `EnableAuthorization`, `AllowPermission`, `Developer`, `Date`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (47, 'Surging.Hero.Auth.IApplication.Action.IActionAppService.GetServices_query', 'Surging.Hero.Auth', 'IActionAppService', '查询服务方法', 'api/action/action/search', 'POST', b'0', b'1', b'1', '刘洪亮', '2020-07-04', 1, 1, '2020-12-14 16:21:28', 1, '2021-01-07 21:32:42');
INSERT INTO `hero_auth`.`Action`(`Id`, `ServiceId`, `ServiceHost`, `Application`, `Name`, `WebApi`, `Method`, `DisableNetwork`, `EnableAuthorization`, `AllowPermission`, `Developer`, `Date`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (48, 'Surging.Hero.Auth.IApplication.Action.IActionAppService.GetServicesTree', 'Surging.Hero.Auth', 'IActionAppService', '获取服务方法树形结构', 'api/action/service/tree', 'GET', b'0', b'1', b'1', '刘洪亮', '2020-07-04', 1, 1, '2020-12-14 16:21:28', 1, '2021-01-07 21:32:42');
INSERT INTO `hero_auth`.`Action`(`Id`, `ServiceId`, `ServiceHost`, `Application`, `Name`, `WebApi`, `Method`, `DisableNetwork`, `EnableAuthorization`, `AllowPermission`, `Developer`, `Date`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (49, 'Surging.Hero.BasicData.IApplication.Wordbook.IWordbookAppService.Create_input', 'Surging.Hero.BasicData', 'IWordbookAppService', '创建字典', 'api/wordbook', 'POST', b'0', b'1', b'0', '刘洪亮', '2020-07-04', 1, 1, '2020-12-14 16:21:28', 1, '2021-01-07 21:32:42');
INSERT INTO `hero_auth`.`Action`(`Id`, `ServiceId`, `ServiceHost`, `Application`, `Name`, `WebApi`, `Method`, `DisableNetwork`, `EnableAuthorization`, `AllowPermission`, `Developer`, `Date`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (50, 'Surging.Hero.BasicData.IApplication.Wordbook.IWordbookAppService.Update_input', 'Surging.Hero.BasicData', 'IWordbookAppService', '更新字典', 'api/wordbook', 'PUT', b'0', b'1', b'0', '刘洪亮', '2020-07-04', 1, 1, '2020-12-14 16:21:28', 1, '2021-01-07 21:32:42');
INSERT INTO `hero_auth`.`Action`(`Id`, `ServiceId`, `ServiceHost`, `Application`, `Name`, `WebApi`, `Method`, `DisableNetwork`, `EnableAuthorization`, `AllowPermission`, `Developer`, `Date`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (51, 'Surging.Hero.BasicData.IApplication.Wordbook.IWordbookAppService.Delete_id', 'Surging.Hero.BasicData', 'IWordbookAppService', '删除字典', 'api/wordbook/{id}', 'DELETE', b'0', b'1', b'0', '刘洪亮', '2020-07-04', 1, 1, '2020-12-14 16:21:28', 1, '2021-01-07 21:32:43');
INSERT INTO `hero_auth`.`Action`(`Id`, `ServiceId`, `ServiceHost`, `Application`, `Name`, `WebApi`, `Method`, `DisableNetwork`, `EnableAuthorization`, `AllowPermission`, `Developer`, `Date`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (52, 'Surging.Hero.BasicData.IApplication.Wordbook.IWordbookAppService.Search_query', 'Surging.Hero.BasicData', 'IWordbookAppService', '查询字典', 'api/wordbook/search', 'POST', b'0', b'1', b'1', '刘洪亮', '2020-07-04', 1, 1, '2020-12-14 16:21:28', 1, '2021-01-07 21:32:43');
INSERT INTO `hero_auth`.`Action`(`Id`, `ServiceId`, `ServiceHost`, `Application`, `Name`, `WebApi`, `Method`, `DisableNetwork`, `EnableAuthorization`, `AllowPermission`, `Developer`, `Date`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (53, 'Surging.Hero.BasicData.IApplication.Wordbook.IWordbookAppService.Get_id', 'Surging.Hero.BasicData', 'IWordbookAppService', '获取字典信息', 'api/wordbook/{id}', 'GET', b'0', b'1', b'1', '刘洪亮', '2020-07-04', 1, 1, '2020-12-14 16:21:28', 1, '2021-01-07 21:32:43');
INSERT INTO `hero_auth`.`Action`(`Id`, `ServiceId`, `ServiceHost`, `Application`, `Name`, `WebApi`, `Method`, `DisableNetwork`, `EnableAuthorization`, `AllowPermission`, `Developer`, `Date`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (54, 'Surging.Hero.BasicData.IApplication.Wordbook.IWordbookAppService.GetWordbookItems_input', 'Surging.Hero.BasicData', 'IWordbookAppService', '获取字典项', 'api/wordbook/items/page', 'POST', b'0', b'1', b'0', '刘洪亮', '2020-07-04', 1, 1, '2020-12-14 16:21:28', 1, '2021-01-07 21:32:43');
INSERT INTO `hero_auth`.`Action`(`Id`, `ServiceId`, `ServiceHost`, `Application`, `Name`, `WebApi`, `Method`, `DisableNetwork`, `EnableAuthorization`, `AllowPermission`, `Developer`, `Date`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (55, 'Surging.Hero.BasicData.IApplication.Wordbook.IWordbookAppService.CreateWordbookItem_input', 'Surging.Hero.BasicData', 'IWordbookAppService', '新增字典项', 'api/wordbook/item', 'POST', b'0', b'1', b'0', '刘洪亮', '2020-07-04', 1, 1, '2020-12-14 16:21:28', 1, '2021-01-07 21:32:43');
INSERT INTO `hero_auth`.`Action`(`Id`, `ServiceId`, `ServiceHost`, `Application`, `Name`, `WebApi`, `Method`, `DisableNetwork`, `EnableAuthorization`, `AllowPermission`, `Developer`, `Date`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (56, 'Surging.Hero.BasicData.IApplication.Wordbook.IWordbookAppService.UpdateWordbookItem_input', 'Surging.Hero.BasicData', 'IWordbookAppService', '更新字典项', 'api/wordbook/item', 'PUT', b'0', b'1', b'0', '刘洪亮', '2020-07-04', 1, 1, '2020-12-14 16:21:28', 1, '2021-01-07 21:32:43');
INSERT INTO `hero_auth`.`Action`(`Id`, `ServiceId`, `ServiceHost`, `Application`, `Name`, `WebApi`, `Method`, `DisableNetwork`, `EnableAuthorization`, `AllowPermission`, `Developer`, `Date`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (57, 'Surging.Hero.BasicData.IApplication.Wordbook.IWordbookAppService.DeleteWordbookItem_id', 'Surging.Hero.BasicData', 'IWordbookAppService', '删除字典项', 'api/wordbook/item/{id}', 'DELETE', b'0', b'1', b'0', '刘洪亮', '2020-07-04', 1, 1, '2020-12-14 16:21:28', 1, '2021-01-07 21:32:44');
INSERT INTO `hero_auth`.`Action`(`Id`, `ServiceId`, `ServiceHost`, `Application`, `Name`, `WebApi`, `Method`, `DisableNetwork`, `EnableAuthorization`, `AllowPermission`, `Developer`, `Date`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (58, 'Surging.Hero.BasicData.IApplication.Wordbook.IWordbookAppService.GetWordbookItem_id', 'Surging.Hero.BasicData', 'IWordbookAppService', '获取字典项目', 'api/wordbook/item/{id}', 'GET', b'0', b'1', b'1', '刘洪亮', '2020-07-04', 1, 1, '2020-12-14 16:21:28', 1, '2021-01-07 21:32:44');
INSERT INTO `hero_auth`.`Action`(`Id`, `ServiceId`, `ServiceHost`, `Application`, `Name`, `WebApi`, `Method`, `DisableNetwork`, `EnableAuthorization`, `AllowPermission`, `Developer`, `Date`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (59, 'Surging.Hero.BasicData.IApplication.Wordbook.IWordbookAppService.Check_input', 'Surging.Hero.BasicData', 'IWordbookAppService', '检查是否存在某个字典项', 'api/wordbook/check', 'POST', b'1', b'1', b'0', '刘洪亮', '2020-07-04', 1, 1, '2020-12-14 16:21:28', 1, '2021-01-07 21:32:44');
INSERT INTO `hero_auth`.`Action`(`Id`, `ServiceId`, `ServiceHost`, `Application`, `Name`, `WebApi`, `Method`, `DisableNetwork`, `EnableAuthorization`, `AllowPermission`, `Developer`, `Date`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (60, 'Surging.Hero.BasicData.IApplication.Wordbook.IWordbookAppService.GetWordbookItemsByCode_code', 'Surging.Hero.BasicData', 'IWordbookAppService', '根据字典标识获取字典项', 'api/wordbook/items/code/{code}', 'GET', b'0', b'1', b'1', '刘洪亮', '2020-07-04', 1, 1, '2020-12-14 16:21:28', 1, '2021-01-07 21:32:44');
INSERT INTO `hero_auth`.`Action`(`Id`, `ServiceId`, `ServiceHost`, `Application`, `Name`, `WebApi`, `Method`, `DisableNetwork`, `EnableAuthorization`, `AllowPermission`, `Developer`, `Date`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (61, 'Surging.Hero.BasicData.IApplication.Wordbook.IWordbookAppService.GetWordbookItemByKey_wordbookCode_key', 'Surging.Hero.BasicData', 'IWordbookAppService', '根据字典项的key获取字典信息', 'api/wordbook/item/key/{key}', 'GET', b'0', b'1', b'1', '刘洪亮', '2020-12-13', 1, 1, '2020-12-14 17:20:17', 1, '2021-01-07 21:32:44');
INSERT INTO `hero_auth`.`Action`(`Id`, `ServiceId`, `ServiceHost`, `Application`, `Name`, `WebApi`, `Method`, `DisableNetwork`, `EnableAuthorization`, `AllowPermission`, `Developer`, `Date`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (62, 'Surging.Hero.Organization.IApplication.Position.IPositionAppService.Get_id', 'Surging.Hero.Organization', 'IPositionAppService', '获取职位信息', 'api/position/{id}', 'GET', b'0', b'1', b'1', '刘洪亮', '2020-07-04', 1, 1, '2020-12-14 17:20:17', 1, '2021-01-07 21:32:46');
INSERT INTO `hero_auth`.`Action`(`Id`, `ServiceId`, `ServiceHost`, `Application`, `Name`, `WebApi`, `Method`, `DisableNetwork`, `EnableAuthorization`, `AllowPermission`, `Developer`, `Date`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (63, 'Surging.Hero.Organization.IApplication.Position.IPositionAppService.GetDeptPosition_deptId', 'Surging.Hero.Organization', 'IPositionAppService', '根据部门id获取部门职位', 'api/position/dept/{deptId}', 'GET', b'0', b'1', b'1', '刘洪亮', '2020-07-04', 1, 1, '2020-12-14 17:20:17', 1, '2021-01-07 21:32:46');
INSERT INTO `hero_auth`.`Action`(`Id`, `ServiceId`, `ServiceHost`, `Application`, `Name`, `WebApi`, `Method`, `DisableNetwork`, `EnableAuthorization`, `AllowPermission`, `Developer`, `Date`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (64, 'Surging.Hero.Organization.IApplication.Position.IPositionAppService.GetDeptPositionByOrgId_orgId', 'Surging.Hero.Organization', 'IPositionAppService', '根据组织机构id获取职位列表', 'api/position/org/{orgId}', 'GET', b'0', b'1', b'1', '刘洪亮', '2020-07-04', 1, 1, '2020-12-14 17:20:17', 1, '2021-01-07 21:32:46');
INSERT INTO `hero_auth`.`Action`(`Id`, `ServiceId`, `ServiceHost`, `Application`, `Name`, `WebApi`, `Method`, `DisableNetwork`, `EnableAuthorization`, `AllowPermission`, `Developer`, `Date`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (65, 'Surging.Hero.Organization.IApplication.Position.IPositionAppService.CheckCanDeletePosition_positionId', 'Surging.Hero.Organization', 'IPositionAppService', '检查某个职位是否允许删除', 'api/position/check/{positionId}', 'POST', b'0', b'1', b'1', '刘洪亮', '2020-07-04', 1, 1, '2020-12-14 17:20:17', 1, '2021-01-07 21:32:46');
INSERT INTO `hero_auth`.`Action`(`Id`, `ServiceId`, `ServiceHost`, `Application`, `Name`, `WebApi`, `Method`, `DisableNetwork`, `EnableAuthorization`, `AllowPermission`, `Developer`, `Date`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (66, 'Surging.Hero.Organization.IApplication.Position.IPositionAppService.CheckExsit_positionId', 'Surging.Hero.Organization', 'IPositionAppService', '检查某个职位是否存在', 'api/position/checkexsit', 'POST', b'1', b'1', b'0', '刘洪亮', '2020-07-04', 1, 1, '2020-12-14 17:20:17', 1, '2021-01-07 21:32:46');
INSERT INTO `hero_auth`.`Action`(`Id`, `ServiceId`, `ServiceHost`, `Application`, `Name`, `WebApi`, `Method`, `DisableNetwork`, `EnableAuthorization`, `AllowPermission`, `Developer`, `Date`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (67, 'Surging.Hero.Organization.IApplication.Organization.IOrganizationAppService.GetTree', 'Surging.Hero.Organization', 'IOrganizationAppService', '获取组织结构树形结构', 'api/organization/tree', 'GET', b'0', b'1', b'1', '刘洪亮', '2020-07-04', 1, 1, '2020-12-14 17:20:17', 1, '2021-01-07 21:32:46');
INSERT INTO `hero_auth`.`Action`(`Id`, `ServiceId`, `ServiceHost`, `Application`, `Name`, `WebApi`, `Method`, `DisableNetwork`, `EnableAuthorization`, `AllowPermission`, `Developer`, `Date`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (68, 'Surging.Hero.Organization.IApplication.Organization.IOrganizationAppService.Search_query', 'Surging.Hero.Organization', 'IOrganizationAppService', '查询组织机构', 'api/organization/search', 'POST', b'0', b'1', b'1', '刘洪亮', '2020-07-04', 1, 1, '2020-12-14 17:20:17', 1, '2021-01-07 21:32:47');
INSERT INTO `hero_auth`.`Action`(`Id`, `ServiceId`, `ServiceHost`, `Application`, `Name`, `WebApi`, `Method`, `DisableNetwork`, `EnableAuthorization`, `AllowPermission`, `Developer`, `Date`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (69, 'Surging.Hero.Organization.IApplication.Organization.IOrganizationAppService.GetSubOrgIds_orgId', 'Surging.Hero.Organization', 'IOrganizationAppService', '获取子机构Id', 'api/organization/suborgs/{orgId}', 'GET', b'1', b'1', b'0', '刘洪亮', '2020-07-04', 1, 1, '2020-12-14 17:20:17', 1, '2021-01-07 21:32:47');
INSERT INTO `hero_auth`.`Action`(`Id`, `ServiceId`, `ServiceHost`, `Application`, `Name`, `WebApi`, `Method`, `DisableNetwork`, `EnableAuthorization`, `AllowPermission`, `Developer`, `Date`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (70, 'Surging.Hero.Organization.IApplication.Department.IDepartmentAppService.Create_input', 'Surging.Hero.Organization', 'IDepartmentAppService', '新增部门信息', 'api/department', 'POST', b'0', b'1', b'0', '刘洪亮', '2020-07-04', 1, 1, '2020-12-14 17:20:17', 1, '2021-01-07 21:32:47');
INSERT INTO `hero_auth`.`Action`(`Id`, `ServiceId`, `ServiceHost`, `Application`, `Name`, `WebApi`, `Method`, `DisableNetwork`, `EnableAuthorization`, `AllowPermission`, `Developer`, `Date`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (71, 'Surging.Hero.Organization.IApplication.Department.IDepartmentAppService.Update_input', 'Surging.Hero.Organization', 'IDepartmentAppService', '更新部门信息', 'api/department', 'PUT', b'0', b'1', b'0', '刘洪亮', '2020-07-04', 1, 1, '2020-12-14 17:20:17', 1, '2021-01-07 21:32:47');
INSERT INTO `hero_auth`.`Action`(`Id`, `ServiceId`, `ServiceHost`, `Application`, `Name`, `WebApi`, `Method`, `DisableNetwork`, `EnableAuthorization`, `AllowPermission`, `Developer`, `Date`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (72, 'Surging.Hero.Organization.IApplication.Department.IDepartmentAppService.DeleteByOrgId_orgId', 'Surging.Hero.Organization', 'IDepartmentAppService', '删除部门信息', 'api/department/{orgId}', 'DELETE', b'0', b'1', b'0', '刘洪亮', '2020-07-04', 1, 1, '2020-12-14 17:20:17', 1, '2021-01-07 21:32:48');
INSERT INTO `hero_auth`.`Action`(`Id`, `ServiceId`, `ServiceHost`, `Application`, `Name`, `WebApi`, `Method`, `DisableNetwork`, `EnableAuthorization`, `AllowPermission`, `Developer`, `Date`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (73, 'Surging.Hero.Organization.IApplication.Department.IDepartmentAppService.Get_id', 'Surging.Hero.Organization', 'IDepartmentAppService', '根据部门id获取部门信息', 'api/department/{id}', 'GET', b'0', b'1', b'1', '刘洪亮', '2020-07-04', 1, 1, '2020-12-14 17:20:17', 1, '2021-01-07 21:32:48');
INSERT INTO `hero_auth`.`Action`(`Id`, `ServiceId`, `ServiceHost`, `Application`, `Name`, `WebApi`, `Method`, `DisableNetwork`, `EnableAuthorization`, `AllowPermission`, `Developer`, `Date`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (74, 'Surging.Hero.Organization.IApplication.Department.IDepartmentAppService.GetByOrgId_orgId', 'Surging.Hero.Organization', 'IDepartmentAppService', '根据组织id获取部门信息', 'api/department/org/{orgId}', 'GET', b'0', b'1', b'1', '刘洪亮', '2020-07-04', 1, 1, '2020-12-14 17:20:17', 1, '2021-01-07 21:32:48');
INSERT INTO `hero_auth`.`Action`(`Id`, `ServiceId`, `ServiceHost`, `Application`, `Name`, `WebApi`, `Method`, `DisableNetwork`, `EnableAuthorization`, `AllowPermission`, `Developer`, `Date`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (75, 'Surging.Hero.Organization.IApplication.Department.IDepartmentAppService.Check_orgId', 'Surging.Hero.Organization', 'IDepartmentAppService', '检查一个部门是否存在', 'api/department/check/{orgId}', 'POST', b'1', b'1', b'0', '刘洪亮', '2020-07-04', 1, 1, '2020-12-14 17:20:17', 1, '2021-01-07 21:32:48');
INSERT INTO `hero_auth`.`Action`(`Id`, `ServiceId`, `ServiceHost`, `Application`, `Name`, `WebApi`, `Method`, `DisableNetwork`, `EnableAuthorization`, `AllowPermission`, `Developer`, `Date`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (76, 'Surging.Hero.Organization.IApplication.Corporation.ICorporationAppService.Create_input', 'Surging.Hero.Organization', 'ICorporationAppService', '新增公司信息', 'api/corporation', 'POST', b'0', b'1', b'0', '刘洪亮', '2020-07-04', 1, 1, '2020-12-14 17:20:17', 1, '2021-01-07 21:32:48');
INSERT INTO `hero_auth`.`Action`(`Id`, `ServiceId`, `ServiceHost`, `Application`, `Name`, `WebApi`, `Method`, `DisableNetwork`, `EnableAuthorization`, `AllowPermission`, `Developer`, `Date`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (77, 'Surging.Hero.Organization.IApplication.Corporation.ICorporationAppService.Update_input', 'Surging.Hero.Organization', 'ICorporationAppService', '更新公司信息', 'api/corporation', 'PUT', b'0', b'1', b'0', '刘洪亮', '2020-07-04', 1, 1, '2020-12-14 17:20:17', 1, '2021-01-07 21:32:49');
INSERT INTO `hero_auth`.`Action`(`Id`, `ServiceId`, `ServiceHost`, `Application`, `Name`, `WebApi`, `Method`, `DisableNetwork`, `EnableAuthorization`, `AllowPermission`, `Developer`, `Date`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (78, 'Surging.Hero.Organization.IApplication.Corporation.ICorporationAppService.DeleteByOrgId_orgId', 'Surging.Hero.Organization', 'ICorporationAppService', '删除公司', 'api/corporation/{orgId}', 'DELETE', b'0', b'1', b'0', '刘洪亮', '2020-07-04', 1, 1, '2020-12-14 17:20:17', 1, '2021-01-07 21:32:49');
INSERT INTO `hero_auth`.`Action`(`Id`, `ServiceId`, `ServiceHost`, `Application`, `Name`, `WebApi`, `Method`, `DisableNetwork`, `EnableAuthorization`, `AllowPermission`, `Developer`, `Date`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (79, 'Surging.Hero.Organization.IApplication.Corporation.ICorporationAppService.GetByOrgId_orgId', 'Surging.Hero.Organization', 'ICorporationAppService', '获取公司信息', 'api/corporation/org/{orgId}', 'GET', b'0', b'1', b'1', '刘洪亮', '2020-07-04', 1, 1, '2020-12-14 17:20:17', 1, '2021-01-07 21:32:49');
INSERT INTO `hero_auth`.`Action`(`Id`, `ServiceId`, `ServiceHost`, `Application`, `Name`, `WebApi`, `Method`, `DisableNetwork`, `EnableAuthorization`, `AllowPermission`, `Developer`, `Date`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (80, 'Surging.Hero.Auth.IApplication.Permission.IPermissionAppService.GetDataPermissionTypes', 'Surging.Hero.Auth', 'IPermissionAppService', '获取数据权限类型', 'api/permission/datapermissiontypes', 'GET', b'0', b'1', b'1', '刘洪亮', '2020-12-31', 1, 1, '2020-12-21 23:46:27', 1, '2021-01-07 21:32:40');
INSERT INTO `hero_auth`.`Action`(`Id`, `ServiceId`, `ServiceHost`, `Application`, `Name`, `WebApi`, `Method`, `DisableNetwork`, `EnableAuthorization`, `AllowPermission`, `Developer`, `Date`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (97, 'Surging.Hero.Auth.IApplication.User.IUserAppService.CheckCreateUser_orgId', 'Surging.Hero.Auth', 'IUserAppService', '根据组织Id判断当前登录用户是否有权限创建该部门的用户', 'api/user/check/{orgId}', 'POST', b'0', b'1', b'0', '刘洪亮', '2020-12-23', 1, 1, '2020-12-23 15:16:26', 1, '2021-01-07 21:32:37');
INSERT INTO `hero_auth`.`Action`(`Id`, `ServiceId`, `ServiceHost`, `Application`, `Name`, `WebApi`, `Method`, `DisableNetwork`, `EnableAuthorization`, `AllowPermission`, `Developer`, `Date`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (98, 'Surging.Hero.Organization.IApplication.Organization.IOrganizationAppService.GetOrg_orgId', 'Surging.Hero.Organization', 'IOrganizationAppService', '根据组织id获取组织机信息', 'api/organization/{orgId}', 'GET', b'0', b'1', b'0', '刘洪亮', '2020-12-22', 1, 1, '2020-12-23 15:16:34', 1, '2021-01-07 21:32:47');
INSERT INTO `hero_auth`.`Action`(`Id`, `ServiceId`, `ServiceHost`, `Application`, `Name`, `WebApi`, `Method`, `DisableNetwork`, `EnableAuthorization`, `AllowPermission`, `Developer`, `Date`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (99, 'Surging.Cloud.CPlatform.Module.IEchoService.Locate_key_routePath_httpMethod', 'Surging.Cloud.CPlatform', 'IEchoService', NULL, '/locate', 'GET', b'0', b'1', b'0', NULL, NULL, 1, 1, '2020-12-28 14:48:38', 1, '2021-01-07 21:32:33');
INSERT INTO `hero_auth`.`Action`(`Id`, `ServiceId`, `ServiceHost`, `Application`, `Name`, `WebApi`, `Method`, `DisableNetwork`, `EnableAuthorization`, `AllowPermission`, `Developer`, `Date`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (100, 'Surging.Hero.Auth.IApplication.User.IUserAppService.GetOrgUserCount_orgId_includeSubOrg', 'Surging.Hero.Auth', 'IUserAppService', '根据orgid判断是否存在用户', 'api/user/getorgusercount', 'GET', b'1', b'1', b'0', '刘洪亮', '2020-12-24', 1, 1, '2020-12-28 14:48:39', 1, '2021-01-07 21:32:36');
INSERT INTO `hero_auth`.`Action`(`Id`, `ServiceId`, `ServiceHost`, `Application`, `Name`, `WebApi`, `Method`, `DisableNetwork`, `EnableAuthorization`, `AllowPermission`, `Developer`, `Date`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (101, 'Surging.Hero.Auth.IApplication.Authorization.IAccountAppService.GetUserOperationByMenuName_menuName', 'Surging.Hero.Auth', 'IAccountAppService', '通过菜单名称获取用户的操作权限', 'api/account/operation/name/{menuName}', 'GET', b'0', b'1', b'1', '刘洪亮', '2019-09-23', 1, 1, '2020-12-28 14:48:44', 1, '2021-01-07 21:32:41');
INSERT INTO `hero_auth`.`Action`(`Id`, `ServiceId`, `ServiceHost`, `Application`, `Name`, `WebApi`, `Method`, `DisableNetwork`, `EnableAuthorization`, `AllowPermission`, `Developer`, `Date`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (102, 'Surging.Hero.Organization.IApplication.Organization.IOrganizationAppService.GetOwnTree', 'Surging.Hero.Organization', 'IOrganizationAppService', '获取当前登录用户有数据权限的组织树', 'api/organization/own/tree', 'GET', b'0', b'1', b'0', '刘洪亮', '2020-12-28', 1, 1, '2020-12-28 14:48:47', 1, '2021-01-07 21:32:46');
INSERT INTO `hero_auth`.`Action`(`Id`, `ServiceId`, `ServiceHost`, `Application`, `Name`, `WebApi`, `Method`, `DisableNetwork`, `EnableAuthorization`, `AllowPermission`, `Developer`, `Date`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (103, 'Surging.Hero.Organization.IApplication.Organization.IOrganizationAppService.GetParentsOrgs_orgId', 'Surging.Hero.Organization', 'IOrganizationAppService', '获取指定Org的父级组织机构', 'api/organization/parents/{orgId}', 'GET', b'1', b'1', b'0', '刘洪亮', '2020-12-28', 1, 1, '2020-12-28 14:48:48', 1, '2021-01-07 21:32:47');
INSERT INTO `hero_auth`.`Action`(`Id`, `ServiceId`, `ServiceHost`, `Application`, `Name`, `WebApi`, `Method`, `DisableNetwork`, `EnableAuthorization`, `AllowPermission`, `Developer`, `Date`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (104, 'Surging.Hero.Auth.IApplication.Tenant.ITenantAppService.Create_input', 'Surging.Hero.Auth', 'ITenantAppService', '新增租户', 'api/tenant', 'POST', b'0', b'1', b'0', '刘洪亮', '2021-01-04', 1, 1, '2021-01-05 22:27:33', 1, '2021-01-07 21:32:37');
INSERT INTO `hero_auth`.`Action`(`Id`, `ServiceId`, `ServiceHost`, `Application`, `Name`, `WebApi`, `Method`, `DisableNetwork`, `EnableAuthorization`, `AllowPermission`, `Developer`, `Date`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (105, 'Surging.Hero.Auth.IApplication.Tenant.ITenantAppService.Update_input', 'Surging.Hero.Auth', 'ITenantAppService', '更新租户', 'api/tenant', 'PUT', b'0', b'1', b'0', '刘洪亮', '2021-01-04', 1, 1, '2021-01-05 22:27:34', 1, '2021-01-07 21:32:37');
INSERT INTO `hero_auth`.`Action`(`Id`, `ServiceId`, `ServiceHost`, `Application`, `Name`, `WebApi`, `Method`, `DisableNetwork`, `EnableAuthorization`, `AllowPermission`, `Developer`, `Date`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (106, 'Surging.Hero.Auth.IApplication.Tenant.ITenantAppService.Delete_id', 'Surging.Hero.Auth', 'ITenantAppService', '删除租户', 'api/tenant/{id}', 'DELETE', b'0', b'1', b'0', '刘洪亮', '2021-01-05', 1, 1, '2021-01-05 22:27:34', 1, '2021-01-07 21:32:37');
INSERT INTO `hero_auth`.`Action`(`Id`, `ServiceId`, `ServiceHost`, `Application`, `Name`, `WebApi`, `Method`, `DisableNetwork`, `EnableAuthorization`, `AllowPermission`, `Developer`, `Date`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (107, 'Surging.Hero.Auth.IApplication.Tenant.ITenantAppService.Search_query', 'Surging.Hero.Auth', 'ITenantAppService', '搜索租户', 'api/tenant/search', 'POST', b'0', b'1', b'0', '刘洪亮', '2021-01-05', 1, 1, '2021-01-05 22:27:34', 1, '2021-01-07 21:32:37');
INSERT INTO `hero_auth`.`Action`(`Id`, `ServiceId`, `ServiceHost`, `Application`, `Name`, `WebApi`, `Method`, `DisableNetwork`, `EnableAuthorization`, `AllowPermission`, `Developer`, `Date`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (108, 'Surging.Hero.Auth.IApplication.Tenant.ITenantAppService.List', 'Surging.Hero.Auth', 'ITenantAppService', '获取租户列表', 'api/tenant/list', 'POST', b'0', b'0', b'0', '刘洪亮', '2021-01-05', 1, 1, '2021-01-05 22:27:35', 1, '2021-01-07 21:32:37');
INSERT INTO `hero_auth`.`Action`(`Id`, `ServiceId`, `ServiceHost`, `Application`, `Name`, `WebApi`, `Method`, `DisableNetwork`, `EnableAuthorization`, `AllowPermission`, `Developer`, `Date`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (109, 'Surging.Hero.Auth.IApplication.Tenant.ITenantAppService.Status_input', 'Surging.Hero.Auth', 'ITenantAppService', '激活/冻结租户', 'api/tenant/status', 'PUT', b'0', b'1', b'0', '刘洪亮', '2021-01-05', 1, 1, '2021-01-05 22:27:35', 1, '2021-01-07 21:32:38');
INSERT INTO `hero_auth`.`Action`(`Id`, `ServiceId`, `ServiceHost`, `Application`, `Name`, `WebApi`, `Method`, `DisableNetwork`, `EnableAuthorization`, `AllowPermission`, `Developer`, `Date`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (110, 'Surging.Hero.BasicData.IApplication.Captcha.ICaptchaAppService.GetCaptcha_uuid', 'Surging.Hero.BasicData', 'ICaptchaAppService', '获取随机验证码', 'api/captcha/getcaptcha', 'GET', b'1', b'0', b'0', '刘洪亮', '2020-12-31', 1, 1, '2021-01-05 22:27:42', 1, '2021-01-07 21:32:45');
INSERT INTO `hero_auth`.`Action`(`Id`, `ServiceId`, `ServiceHost`, `Application`, `Name`, `WebApi`, `Method`, `DisableNetwork`, `EnableAuthorization`, `AllowPermission`, `Developer`, `Date`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (111, 'Surging.Hero.Organization.IApplication.Corporation.ICorporationAppService.CreateByTenant_input', 'Surging.Hero.Organization', 'ICorporationAppService', '新增租户时,默认增加对应的组织机构', 'api/corporation/createbytenant', 'POST', b'1', b'1', b'0', '刘洪亮', '2021-01-05', 1, 1, '2021-01-05 22:27:46', 1, '2021-01-07 21:32:48');
INSERT INTO `hero_auth`.`Action`(`Id`, `ServiceId`, `ServiceHost`, `Application`, `Name`, `WebApi`, `Method`, `DisableNetwork`, `EnableAuthorization`, `AllowPermission`, `Developer`, `Date`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (112, 'Surging.Hero.BasicData.IApplication.SystemConfig.ISystemConfigAppService.GetSystemConfig', 'Surging.Hero.BasicData', 'ISystemConfigAppService', '获取系统配置', 'api/systemconfig', 'GET', b'0', b'1', b'1', '刘洪亮', '2021-01-07', 1, 1, '2021-01-07 21:32:44', 1, '2021-01-07 21:32:44');
INSERT INTO `hero_auth`.`Action`(`Id`, `ServiceId`, `ServiceHost`, `Application`, `Name`, `WebApi`, `Method`, `DisableNetwork`, `EnableAuthorization`, `AllowPermission`, `Developer`, `Date`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (113, 'Surging.Hero.BasicData.IApplication.SystemConfig.ISystemConfigAppService.SetSystemConfig_input', 'Surging.Hero.BasicData', 'ISystemConfigAppService', '设置系统配置', 'api/systemconfig', 'POST,PUT', b'0', b'1', b'0', '刘洪亮', '2021-01-07', 1, 1, '2021-01-07 21:32:45', 1, '2021-01-07 21:32:45');


INSERT INTO `hero_auth`.`Menu`(`Id`, `PermissionId`, `ParentId`, `Code`, `Name`, `Title`, `Level`, `Path`, `Mold`, `AlwaysShow`, `Icon`, `Component`, `Sort`, `Memo`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (1, 1, 0, '0001', 'authorization', '权限管理', 1, NULL, 0, b'1', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL);
INSERT INTO `hero_auth`.`Menu`(`Id`, `PermissionId`, `ParentId`, `Code`, `Name`, `Title`, `Level`, `Path`, `Mold`, `AlwaysShow`, `Icon`, `Component`, `Sort`, `Memo`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (2, 2, 1, '0001.0001', 'user', '用户管理', 2, NULL, 1, b'1', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL);
INSERT INTO `hero_auth`.`Menu`(`Id`, `PermissionId`, `ParentId`, `Code`, `Name`, `Title`, `Level`, `Path`, `Mold`, `AlwaysShow`, `Icon`, `Component`, `Sort`, `Memo`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (3, 3, 1, '0001.0002', 'usergroup', '用户组管理', 2, NULL, 1, b'1', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL);
INSERT INTO `hero_auth`.`Menu`(`Id`, `PermissionId`, `ParentId`, `Code`, `Name`, `Title`, `Level`, `Path`, `Mold`, `AlwaysShow`, `Icon`, `Component`, `Sort`, `Memo`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (4, 4, 1, '0001.0003', 'role', '角色管理', 2, NULL, 1, b'1', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL);
INSERT INTO `hero_auth`.`Menu`(`Id`, `PermissionId`, `ParentId`, `Code`, `Name`, `Title`, `Level`, `Path`, `Mold`, `AlwaysShow`, `Icon`, `Component`, `Sort`, `Memo`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (5, 5, 1, '0001.0004', 'organization', '组织机构', 2, NULL, 1, b'1', NULL, NULL, 0, NULL, NULL, '2020-12-14 17:32:40', 1, '2020-12-14 17:32:49', 0, NULL, NULL);
INSERT INTO `hero_auth`.`Menu`(`Id`, `PermissionId`, `ParentId`, `Code`, `Name`, `Title`, `Level`, `Path`, `Mold`, `AlwaysShow`, `Icon`, `Component`, `Sort`, `Memo`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (6, 6, 1, '0001.0005', 'menu', '菜单管理', 2, NULL, 1, b'1', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL);
INSERT INTO `hero_auth`.`Menu`(`Id`, `PermissionId`, `ParentId`, `Code`, `Name`, `Title`, `Level`, `Path`, `Mold`, `AlwaysShow`, `Icon`, `Component`, `Sort`, `Memo`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (7, 7, 0, '2000', 'system', '系统管理', 1, NULL, 0, b'1', NULL, NULL, 0, NULL, 1, '2020-12-14 16:06:47', 1, '2020-12-14 16:06:47', 0, NULL, NULL);
INSERT INTO `hero_auth`.`Menu`(`Id`, `PermissionId`, `ParentId`, `Code`, `Name`, `Title`, `Level`, `Path`, `Mold`, `AlwaysShow`, `Icon`, `Component`, `Sort`, `Memo`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (8, 8, 7, '2000.0001', 'wordbook', '字典管理', 2, NULL, 1, b'1', NULL, NULL, 0, NULL, 1, '2020-12-14 16:07:12', 1, '2020-12-14 16:07:12', 0, NULL, NULL);
INSERT INTO `hero_auth`.`Menu`(`Id`, `PermissionId`, `ParentId`, `Code`, `Name`, `Title`, `Level`, `Path`, `Mold`, `AlwaysShow`, `Icon`, `Component`, `Sort`, `Memo`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (9, 52, 1, '0001.0006', 'tenant', '租户管理', 2, NULL, 1, b'1', NULL, NULL, 0, NULL, 1, '2021-01-06 15:52:16', 1, '2021-01-06 15:52:16', 0, NULL, NULL);
INSERT INTO `hero_auth`.`Menu`(`Id`, `PermissionId`, `ParentId`, `Code`, `Name`, `Title`, `Level`, `Path`, `Mold`, `AlwaysShow`, `Icon`, `Component`, `Sort`, `Memo`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (10, 59, 7, '2000.0002', 'setting', '系统设置', 2, NULL, 1, b'1', NULL, NULL, 0, NULL, 1, '2021-01-07 21:30:04', 1, '2021-01-07 21:30:04', 0, NULL, NULL);

INSERT INTO `hero_auth`.`Operation`(`Id`, `PermissionId`, `MenuId`, `Name`, `Title`, `Code`, `Level`, `Mold`, `Sort`, `Memo`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (1, 9, 2, 'user-create', '新增', '0001.0001.0001', 3, 0, NULL, NULL, 1, '2020-12-14 16:26:23', 11, '2021-01-08 22:24:55', 0, NULL, NULL);
INSERT INTO `hero_auth`.`Operation`(`Id`, `PermissionId`, `MenuId`, `Name`, `Title`, `Code`, `Level`, `Mold`, `Sort`, `Memo`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (2, 10, 2, 'user-update', '修改', '0001.0001.0002', 3, 1, NULL, NULL, 1, '2020-12-14 16:26:58', 13, '2021-01-02 00:41:26', 0, NULL, NULL);
INSERT INTO `hero_auth`.`Operation`(`Id`, `PermissionId`, `MenuId`, `Name`, `Title`, `Code`, `Level`, `Mold`, `Sort`, `Memo`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (3, 11, 2, 'user-delete', '删除', '0001.0001.0003', 3, 4, NULL, NULL, 1, '2020-12-14 16:27:58', 1, '2020-12-14 16:27:58', 0, NULL, NULL);
INSERT INTO `hero_auth`.`Operation`(`Id`, `PermissionId`, `MenuId`, `Name`, `Title`, `Code`, `Level`, `Mold`, `Sort`, `Memo`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (4, 12, 2, 'user-search', '查询', '0001.0001.0004', 3, 2, NULL, NULL, 1, '2020-12-14 16:28:23', 1, '2020-12-28 14:58:29', 0, NULL, NULL);
INSERT INTO `hero_auth`.`Operation`(`Id`, `PermissionId`, `MenuId`, `Name`, `Title`, `Code`, `Level`, `Mold`, `Sort`, `Memo`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (5, 13, 2, 'user-status', '冻结/激活状态', '0001.0001.0005', 3, 5, NULL, NULL, 1, '2020-12-14 16:29:08', 1, '2020-12-14 16:29:08', 0, NULL, NULL);
INSERT INTO `hero_auth`.`Operation`(`Id`, `PermissionId`, `MenuId`, `Name`, `Title`, `Code`, `Level`, `Mold`, `Sort`, `Memo`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (6, 14, 2, 'user-reset-password', '重置密码', '0001.0001.0006', 3, 5, NULL, NULL, 1, '2020-12-14 16:29:44', 1, '2020-12-14 16:29:44', 0, NULL, NULL);
INSERT INTO `hero_auth`.`Operation`(`Id`, `PermissionId`, `MenuId`, `Name`, `Title`, `Code`, `Level`, `Mold`, `Sort`, `Memo`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (7, 15, 3, 'usergroup-create', '新增', '0001.0002.0001', 3, 0, NULL, NULL, 1, '2020-12-14 16:30:34', 1, '2020-12-14 16:30:34', 0, NULL, NULL);
INSERT INTO `hero_auth`.`Operation`(`Id`, `PermissionId`, `MenuId`, `Name`, `Title`, `Code`, `Level`, `Mold`, `Sort`, `Memo`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (8, 16, 3, 'usergroup-update', '修改', '0001.0002.0002', 3, 1, NULL, NULL, 1, '2020-12-14 16:31:02', 1, '2020-12-14 16:31:02', 0, NULL, NULL);
INSERT INTO `hero_auth`.`Operation`(`Id`, `PermissionId`, `MenuId`, `Name`, `Title`, `Code`, `Level`, `Mold`, `Sort`, `Memo`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (9, 17, 3, 'usergroup-delete', '删除', '0001.0002.0003', 3, 4, NULL, NULL, 1, '2020-12-14 16:31:41', 1, '2020-12-14 16:31:41', 0, NULL, NULL);
INSERT INTO `hero_auth`.`Operation`(`Id`, `PermissionId`, `MenuId`, `Name`, `Title`, `Code`, `Level`, `Mold`, `Sort`, `Memo`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (10, 18, 3, 'usergroup-search', '查询', '0001.0002.0004', 3, 2, NULL, NULL, 1, '2020-12-14 16:32:23', 1, '2020-12-21 23:47:17', 0, NULL, NULL);
INSERT INTO `hero_auth`.`Operation`(`Id`, `PermissionId`, `MenuId`, `Name`, `Title`, `Code`, `Level`, `Mold`, `Sort`, `Memo`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (11, 19, 3, 'usergroup-status', '冻结/激活状态', '0001.0002.0005', 3, 5, NULL, NULL, 1, '2020-12-14 16:33:54', 1, '2020-12-14 16:33:54', 0, NULL, NULL);
INSERT INTO `hero_auth`.`Operation`(`Id`, `PermissionId`, `MenuId`, `Name`, `Title`, `Code`, `Level`, `Mold`, `Sort`, `Memo`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (12, 20, 3, 'usergroup-allocation-user', '更新用户组成员', '0001.0002.0006', 3, 5, NULL, NULL, 1, '2020-12-14 16:35:22', 1, '2020-12-14 16:35:22', 0, NULL, NULL);
INSERT INTO `hero_auth`.`Operation`(`Id`, `PermissionId`, `MenuId`, `Name`, `Title`, `Code`, `Level`, `Mold`, `Sort`, `Memo`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (13, 21, 3, 'usergroup-delete-user', '删除用户组成员', '0001.0002.0007', 3, 4, NULL, NULL, 1, '2020-12-14 16:36:18', 1, '2020-12-14 16:36:18', 0, NULL, NULL);
INSERT INTO `hero_auth`.`Operation`(`Id`, `PermissionId`, `MenuId`, `Name`, `Title`, `Code`, `Level`, `Mold`, `Sort`, `Memo`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (14, 22, 3, 'usergroup-search-user', '查看用户组成员', '0001.0002.0008', 3, 5, NULL, NULL, 1, '2020-12-14 16:36:59', 1, '2020-12-14 16:36:59', 0, NULL, NULL);
INSERT INTO `hero_auth`.`Operation`(`Id`, `PermissionId`, `MenuId`, `Name`, `Title`, `Code`, `Level`, `Mold`, `Sort`, `Memo`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (15, 23, 4, 'role-create', '新增', '0001.0003.0001', 3, 0, NULL, NULL, 1, '2020-12-14 16:38:43', 1, '2020-12-28 15:24:51', 0, NULL, NULL);
INSERT INTO `hero_auth`.`Operation`(`Id`, `PermissionId`, `MenuId`, `Name`, `Title`, `Code`, `Level`, `Mold`, `Sort`, `Memo`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (16, 24, 4, 'role-update', '修改', '0001.0003.0002', 3, 1, NULL, NULL, 1, '2020-12-14 16:39:24', 1, '2020-12-28 15:25:06', 0, NULL, NULL);
INSERT INTO `hero_auth`.`Operation`(`Id`, `PermissionId`, `MenuId`, `Name`, `Title`, `Code`, `Level`, `Mold`, `Sort`, `Memo`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (17, 25, 4, 'role-delete', '删除', '0001.0003.0003', 3, 4, NULL, NULL, 1, '2020-12-14 16:39:55', 1, '2020-12-14 16:39:55', 0, NULL, NULL);
INSERT INTO `hero_auth`.`Operation`(`Id`, `PermissionId`, `MenuId`, `Name`, `Title`, `Code`, `Level`, `Mold`, `Sort`, `Memo`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (18, 26, 4, 'role-search', '查询', '0001.0003.0004', 3, 2, NULL, NULL, 1, '2020-12-14 16:40:37', 1, '2020-12-21 23:47:30', 0, NULL, NULL);
INSERT INTO `hero_auth`.`Operation`(`Id`, `PermissionId`, `MenuId`, `Name`, `Title`, `Code`, `Level`, `Mold`, `Sort`, `Memo`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (19, 27, 5, 'corporation-create', '新增公司', '0001.0004.0001', 3, 0, NULL, NULL, 1, '2020-12-14 16:43:29', 1, '2020-12-14 17:23:17', 0, NULL, NULL);
INSERT INTO `hero_auth`.`Operation`(`Id`, `PermissionId`, `MenuId`, `Name`, `Title`, `Code`, `Level`, `Mold`, `Sort`, `Memo`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (20, 28, 5, 'corporation-update', '修改公司', '0001.0004.0002', 3, 1, NULL, NULL, 1, '2020-12-14 17:24:45', 1, '2020-12-14 17:25:38', 0, NULL, NULL);
INSERT INTO `hero_auth`.`Operation`(`Id`, `PermissionId`, `MenuId`, `Name`, `Title`, `Code`, `Level`, `Mold`, `Sort`, `Memo`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (21, 29, 5, 'corporation-detele', '删除公司', '0001.0004.0003', 3, 4, NULL, NULL, 1, '2020-12-14 17:25:28', 1, '2020-12-14 17:25:28', 0, NULL, NULL);
INSERT INTO `hero_auth`.`Operation`(`Id`, `PermissionId`, `MenuId`, `Name`, `Title`, `Code`, `Level`, `Mold`, `Sort`, `Memo`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (22, 30, 5, 'department-create', '新增部门', '0001.0004.0004', 3, 0, NULL, NULL, 1, '2020-12-14 17:26:25', 1, '2020-12-14 17:26:25', 0, NULL, NULL);
INSERT INTO `hero_auth`.`Operation`(`Id`, `PermissionId`, `MenuId`, `Name`, `Title`, `Code`, `Level`, `Mold`, `Sort`, `Memo`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (23, 31, 5, 'department-update', '修改部门', '0001.0004.0005', 3, 1, NULL, NULL, 1, '2020-12-14 17:26:49', 1, '2020-12-14 17:26:49', 0, NULL, NULL);
INSERT INTO `hero_auth`.`Operation`(`Id`, `PermissionId`, `MenuId`, `Name`, `Title`, `Code`, `Level`, `Mold`, `Sort`, `Memo`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (24, 32, 5, 'department-delete', '删除部门', '0001.0004.0006', 3, 4, NULL, NULL, 1, '2020-12-14 17:27:10', 1, '2020-12-14 17:27:10', 0, NULL, NULL);
INSERT INTO `hero_auth`.`Operation`(`Id`, `PermissionId`, `MenuId`, `Name`, `Title`, `Code`, `Level`, `Mold`, `Sort`, `Memo`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (25, 33, 5, 'department-look', '查看组织机构', '0001.0004.0007', 3, 2, NULL, NULL, 1, '2020-12-14 17:27:38', 1, '2020-12-14 17:27:38', 0, NULL, NULL);
INSERT INTO `hero_auth`.`Operation`(`Id`, `PermissionId`, `MenuId`, `Name`, `Title`, `Code`, `Level`, `Mold`, `Sort`, `Memo`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (26, 34, 6, 'menu-create', '新增菜单', '0001.0005.0001', 3, 0, NULL, NULL, 1, '2020-12-14 17:28:36', 1, '2020-12-14 17:28:36', 0, NULL, NULL);
INSERT INTO `hero_auth`.`Operation`(`Id`, `PermissionId`, `MenuId`, `Name`, `Title`, `Code`, `Level`, `Mold`, `Sort`, `Memo`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (27, 35, 6, 'menu-update', '修改菜单', '0001.0005.0002', 3, 1, NULL, NULL, 1, '2020-12-14 17:29:13', 1, '2020-12-16 02:50:00', 0, NULL, NULL);
INSERT INTO `hero_auth`.`Operation`(`Id`, `PermissionId`, `MenuId`, `Name`, `Title`, `Code`, `Level`, `Mold`, `Sort`, `Memo`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (28, 36, 6, 'menu-delete', '删除菜单', '0001.0005.0003', 3, 4, NULL, NULL, 1, '2020-12-14 17:29:34', 1, '2020-12-16 02:50:18', 0, NULL, NULL);
INSERT INTO `hero_auth`.`Operation`(`Id`, `PermissionId`, `MenuId`, `Name`, `Title`, `Code`, `Level`, `Mold`, `Sort`, `Memo`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (29, 37, 6, 'operation-create', '新增操作', '0001.0005.0004', 3, 0, NULL, NULL, 1, '2020-12-14 17:30:32', 1, '2020-12-14 17:30:32', 0, NULL, NULL);
INSERT INTO `hero_auth`.`Operation`(`Id`, `PermissionId`, `MenuId`, `Name`, `Title`, `Code`, `Level`, `Mold`, `Sort`, `Memo`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (30, 38, 6, 'operation-update', '修改操作', '0001.0005.0005', 3, 1, NULL, NULL, 1, '2020-12-14 17:31:05', 1, '2020-12-14 17:31:05', 0, NULL, NULL);
INSERT INTO `hero_auth`.`Operation`(`Id`, `PermissionId`, `MenuId`, `Name`, `Title`, `Code`, `Level`, `Mold`, `Sort`, `Memo`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (31, 39, 6, 'operation-delete', '删除操作', '0001.0005.0006', 3, 4, NULL, NULL, 1, '2020-12-14 17:31:36', 1, '2020-12-14 17:31:36', 0, NULL, NULL);
INSERT INTO `hero_auth`.`Operation`(`Id`, `PermissionId`, `MenuId`, `Name`, `Title`, `Code`, `Level`, `Mold`, `Sort`, `Memo`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (32, 40, 6, 'operation-look', '查看', '0001.0005.0007', 3, 2, NULL, NULL, 1, '2020-12-14 17:32:18', 1, '2020-12-14 17:32:18', 0, NULL, NULL);
INSERT INTO `hero_auth`.`Operation`(`Id`, `PermissionId`, `MenuId`, `Name`, `Title`, `Code`, `Level`, `Mold`, `Sort`, `Memo`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (33, 41, 8, 'wordbook-create', '新增字典', '2000.0001.0001', 3, 0, NULL, NULL, 1, '2020-12-14 17:34:32', 1, '2020-12-14 17:34:32', 0, NULL, NULL);
INSERT INTO `hero_auth`.`Operation`(`Id`, `PermissionId`, `MenuId`, `Name`, `Title`, `Code`, `Level`, `Mold`, `Sort`, `Memo`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (34, 42, 8, 'wordbook-update', '修改字典', '2000.0001.0002', 3, 1, NULL, NULL, 1, '2020-12-14 17:34:52', 1, '2020-12-14 17:34:52', 0, NULL, NULL);
INSERT INTO `hero_auth`.`Operation`(`Id`, `PermissionId`, `MenuId`, `Name`, `Title`, `Code`, `Level`, `Mold`, `Sort`, `Memo`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (35, 43, 8, 'wordbook-delete', '删除字典', '2000.0001.0003', 3, 4, NULL, NULL, 1, '2020-12-14 17:35:08', 1, '2020-12-14 17:35:08', 0, NULL, NULL);
INSERT INTO `hero_auth`.`Operation`(`Id`, `PermissionId`, `MenuId`, `Name`, `Title`, `Code`, `Level`, `Mold`, `Sort`, `Memo`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (36, 44, 8, 'wordbook-item-look', '查看字典项', '2000.0001.0004', 3, 5, NULL, NULL, 1, '2020-12-14 17:36:35', 1, '2020-12-14 17:37:12', 0, NULL, NULL);
INSERT INTO `hero_auth`.`Operation`(`Id`, `PermissionId`, `MenuId`, `Name`, `Title`, `Code`, `Level`, `Mold`, `Sort`, `Memo`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (37, 45, 8, 'wordbook-item-create', '新增字典项', '2000.0001.0005', 3, 0, NULL, NULL, 1, '2020-12-14 17:37:56', 1, '2020-12-14 17:37:56', 0, NULL, NULL);
INSERT INTO `hero_auth`.`Operation`(`Id`, `PermissionId`, `MenuId`, `Name`, `Title`, `Code`, `Level`, `Mold`, `Sort`, `Memo`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (38, 46, 8, 'wordbook-item-update', '更新字典项', '2000.0001.0006', 3, 1, NULL, NULL, 1, '2020-12-14 17:38:24', 1, '2020-12-14 17:38:24', 0, NULL, NULL);
INSERT INTO `hero_auth`.`Operation`(`Id`, `PermissionId`, `MenuId`, `Name`, `Title`, `Code`, `Level`, `Mold`, `Sort`, `Memo`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (39, 47, 8, 'wordbook-item-delete', '删除字典项', '2000.0001.0007', 3, 4, NULL, NULL, 1, '2020-12-14 17:39:02', 1, '2020-12-14 17:39:02', 0, NULL, NULL);
INSERT INTO `hero_auth`.`Operation`(`Id`, `PermissionId`, `MenuId`, `Name`, `Title`, `Code`, `Level`, `Mold`, `Sort`, `Memo`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (40, 48, 8, 'wordbook-search', '查询/查看字典', '2000.0001.0008', 3, 2, NULL, NULL, 1, '2020-12-14 17:39:45', 1, '2020-12-14 17:39:45', 0, NULL, NULL);
INSERT INTO `hero_auth`.`Operation`(`Id`, `PermissionId`, `MenuId`, `Name`, `Title`, `Code`, `Level`, `Mold`, `Sort`, `Memo`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (41, 49, 4, 'role-status', '冻结/激活状态', '0001.0003.0005', 3, 5, NULL, NULL, 1, '2020-12-24 21:28:56', 1, '2020-12-24 21:28:56', 0, NULL, NULL);
INSERT INTO `hero_auth`.`Operation`(`Id`, `PermissionId`, `MenuId`, `Name`, `Title`, `Code`, `Level`, `Mold`, `Sort`, `Memo`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (42, 50, 4, 'role-look', '查看', '0001.0003.0006', 3, 3, NULL, NULL, 1, '2020-12-24 21:30:35', 1, '2020-12-24 21:30:35', 0, NULL, NULL);
INSERT INTO `hero_auth`.`Operation`(`Id`, `PermissionId`, `MenuId`, `Name`, `Title`, `Code`, `Level`, `Mold`, `Sort`, `Memo`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (43, 51, 2, 'user-look', '查看', '0001.0001.0007', 3, 3, NULL, NULL, 1, '2020-12-24 21:32:39', 1, '2020-12-24 21:32:39', 0, NULL, NULL);
INSERT INTO `hero_auth`.`Operation`(`Id`, `PermissionId`, `MenuId`, `Name`, `Title`, `Code`, `Level`, `Mold`, `Sort`, `Memo`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (45, 54, 9, 'tenant-create', '新增', '0001.0006.0001', 3, 0, NULL, NULL, 1, '2021-01-06 15:56:14', 1, '2021-01-06 15:56:22', 0, NULL, NULL);
INSERT INTO `hero_auth`.`Operation`(`Id`, `PermissionId`, `MenuId`, `Name`, `Title`, `Code`, `Level`, `Mold`, `Sort`, `Memo`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (46, 55, 9, 'tenant-update', '修改', '0001.0006.0001', 3, 1, NULL, NULL, 1, '2021-01-06 15:56:48', 1, '2021-01-06 15:56:48', 0, NULL, NULL);
INSERT INTO `hero_auth`.`Operation`(`Id`, `PermissionId`, `MenuId`, `Name`, `Title`, `Code`, `Level`, `Mold`, `Sort`, `Memo`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (47, 56, 9, 'tenant-delete', '删除', '0001.0006.0001', 3, 4, NULL, NULL, 1, '2021-01-06 15:57:28', 1, '2021-01-06 15:57:28', 0, NULL, NULL);
INSERT INTO `hero_auth`.`Operation`(`Id`, `PermissionId`, `MenuId`, `Name`, `Title`, `Code`, `Level`, `Mold`, `Sort`, `Memo`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (48, 57, 9, 'tenant-search', '查询', '0001.0006.0001', 3, 2, NULL, NULL, 1, '2021-01-06 16:06:22', 1, '2021-01-06 16:06:22', 0, NULL, NULL);
INSERT INTO `hero_auth`.`Operation`(`Id`, `PermissionId`, `MenuId`, `Name`, `Title`, `Code`, `Level`, `Mold`, `Sort`, `Memo`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (49, 58, 9, 'tenant-status', '冻结/激活状态', '0001.0006.0001', 3, 5, NULL, NULL, 1, '2021-01-06 16:07:31', 1, '2021-01-06 16:07:31', 0, NULL, NULL);
INSERT INTO `hero_auth`.`Operation`(`Id`, `PermissionId`, `MenuId`, `Name`, `Title`, `Code`, `Level`, `Mold`, `Sort`, `Memo`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (50, 60, 10, 'setting-look', '查看', '2000.0002.0001', 3, 3, NULL, NULL, 1, '2021-01-07 21:31:17', 1, '2021-01-07 21:31:17', 0, NULL, NULL);
INSERT INTO `hero_auth`.`Operation`(`Id`, `PermissionId`, `MenuId`, `Name`, `Title`, `Code`, `Level`, `Mold`, `Sort`, `Memo`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (51, 61, 10, 'setting-config', '设置', '2000.0002.0001', 3, 5, NULL, NULL, 1, '2021-01-07 21:33:44', 1, '2021-01-07 21:33:44', 0, NULL, NULL);


INSERT INTO `hero_auth`.`OperationActionRelation`(`Id`, `OperationId`, `ActionId`, `ServiceId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (3, 3, 4, 'Surging.Hero.Auth.IApplication.User.IUserAppService.Delete_id', 1, '2020-12-14 16:27:58', 1, '2020-12-14 16:27:58');
INSERT INTO `hero_auth`.`OperationActionRelation`(`Id`, `OperationId`, `ActionId`, `ServiceId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (4, 5, 6, 'Surging.Hero.Auth.IApplication.User.IUserAppService.UpdateStatus_input', 1, '2020-12-14 16:29:08', 1, '2020-12-14 16:29:08');
INSERT INTO `hero_auth`.`OperationActionRelation`(`Id`, `OperationId`, `ActionId`, `ServiceId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (5, 6, 7, 'Surging.Hero.Auth.IApplication.User.IUserAppService.ResetPassword_input', 1, '2020-12-14 16:29:44', 1, '2020-12-14 16:29:44');
INSERT INTO `hero_auth`.`OperationActionRelation`(`Id`, `OperationId`, `ActionId`, `ServiceId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (6, 7, 13, 'Surging.Hero.Auth.IApplication.UserGroup.IUserGroupAppService.Create_input', 1, '2020-12-14 16:30:34', 1, '2020-12-14 16:30:34');
INSERT INTO `hero_auth`.`OperationActionRelation`(`Id`, `OperationId`, `ActionId`, `ServiceId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (7, 8, 14, 'Surging.Hero.Auth.IApplication.UserGroup.IUserGroupAppService.Update_input', 1, '2020-12-14 16:31:02', 1, '2020-12-14 16:31:02');
INSERT INTO `hero_auth`.`OperationActionRelation`(`Id`, `OperationId`, `ActionId`, `ServiceId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (8, 9, 17, 'Surging.Hero.Auth.IApplication.UserGroup.IUserGroupAppService.Delete_id', 1, '2020-12-14 16:31:41', 1, '2020-12-14 16:31:41');
INSERT INTO `hero_auth`.`OperationActionRelation`(`Id`, `OperationId`, `ActionId`, `ServiceId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (9, 11, 15, 'Surging.Hero.Auth.IApplication.UserGroup.IUserGroupAppService.UpdateStatus_input', 1, '2020-12-14 16:33:54', 1, '2020-12-14 16:33:54');
INSERT INTO `hero_auth`.`OperationActionRelation`(`Id`, `OperationId`, `ActionId`, `ServiceId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (10, 12, 19, 'Surging.Hero.Auth.IApplication.UserGroup.IUserGroupAppService.AllocationUsers_input', 1, '2020-12-14 16:35:22', 1, '2020-12-14 16:35:22');
INSERT INTO `hero_auth`.`OperationActionRelation`(`Id`, `OperationId`, `ActionId`, `ServiceId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (11, 13, 21, 'Surging.Hero.Auth.IApplication.UserGroup.IUserGroupAppService.DeleteUserGroupUser_input', 1, '2020-12-14 16:36:18', 1, '2020-12-14 16:36:18');
INSERT INTO `hero_auth`.`OperationActionRelation`(`Id`, `OperationId`, `ActionId`, `ServiceId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (12, 14, 20, 'Surging.Hero.Auth.IApplication.UserGroup.IUserGroupAppService.SearchUserGroupUser_query', 1, '2020-12-14 16:36:59', 1, '2020-12-14 16:36:59');
INSERT INTO `hero_auth`.`OperationActionRelation`(`Id`, `OperationId`, `ActionId`, `ServiceId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (15, 17, 28, 'Surging.Hero.Auth.IApplication.Role.IRoleAppService.Delete_id', 1, '2020-12-14 16:39:55', 1, '2020-12-14 16:39:55');
INSERT INTO `hero_auth`.`OperationActionRelation`(`Id`, `OperationId`, `ActionId`, `ServiceId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (17, 19, 76, 'Surging.Hero.Organization.IApplication.Corporation.ICorporationAppService.Create_input', 1, '2020-12-14 17:23:17', 1, '2020-12-14 17:23:17');
INSERT INTO `hero_auth`.`OperationActionRelation`(`Id`, `OperationId`, `ActionId`, `ServiceId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (19, 21, 78, 'Surging.Hero.Organization.IApplication.Corporation.ICorporationAppService.DeleteByOrgId_orgId', 1, '2020-12-14 17:25:28', 1, '2020-12-14 17:25:28');
INSERT INTO `hero_auth`.`OperationActionRelation`(`Id`, `OperationId`, `ActionId`, `ServiceId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (20, 20, 77, 'Surging.Hero.Organization.IApplication.Corporation.ICorporationAppService.Update_input', 1, '2020-12-14 17:25:38', 1, '2020-12-14 17:25:38');
INSERT INTO `hero_auth`.`OperationActionRelation`(`Id`, `OperationId`, `ActionId`, `ServiceId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (21, 22, 70, 'Surging.Hero.Organization.IApplication.Department.IDepartmentAppService.Create_input', 1, '2020-12-14 17:26:25', 1, '2020-12-14 17:26:25');
INSERT INTO `hero_auth`.`OperationActionRelation`(`Id`, `OperationId`, `ActionId`, `ServiceId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (22, 23, 71, 'Surging.Hero.Organization.IApplication.Department.IDepartmentAppService.Update_input', 1, '2020-12-14 17:26:50', 1, '2020-12-14 17:26:50');
INSERT INTO `hero_auth`.`OperationActionRelation`(`Id`, `OperationId`, `ActionId`, `ServiceId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (23, 26, 29, 'Surging.Hero.Auth.IApplication.Permission.IPermissionAppService.CreateMenu_input', 1, '2020-12-14 17:28:36', 1, '2020-12-14 17:28:36');
INSERT INTO `hero_auth`.`OperationActionRelation`(`Id`, `OperationId`, `ActionId`, `ServiceId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (24, 29, 32, 'Surging.Hero.Auth.IApplication.Permission.IPermissionAppService.CreateOperation_input', 1, '2020-12-14 17:30:32', 1, '2020-12-14 17:30:32');
INSERT INTO `hero_auth`.`OperationActionRelation`(`Id`, `OperationId`, `ActionId`, `ServiceId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (25, 30, 33, 'Surging.Hero.Auth.IApplication.Permission.IPermissionAppService.UpdateOperation_input', 1, '2020-12-14 17:31:05', 1, '2020-12-14 17:31:05');
INSERT INTO `hero_auth`.`OperationActionRelation`(`Id`, `OperationId`, `ActionId`, `ServiceId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (26, 31, 36, 'Surging.Hero.Auth.IApplication.Permission.IPermissionAppService.Delete_input', 1, '2020-12-14 17:31:36', 1, '2020-12-14 17:31:36');
INSERT INTO `hero_auth`.`OperationActionRelation`(`Id`, `OperationId`, `ActionId`, `ServiceId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (27, 33, 49, 'Surging.Hero.BasicData.IApplication.Wordbook.IWordbookAppService.Create_input', 1, '2020-12-14 17:34:32', 1, '2020-12-14 17:34:32');
INSERT INTO `hero_auth`.`OperationActionRelation`(`Id`, `OperationId`, `ActionId`, `ServiceId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (28, 34, 50, 'Surging.Hero.BasicData.IApplication.Wordbook.IWordbookAppService.Update_input', 1, '2020-12-14 17:34:52', 1, '2020-12-14 17:34:52');
INSERT INTO `hero_auth`.`OperationActionRelation`(`Id`, `OperationId`, `ActionId`, `ServiceId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (30, 36, 54, 'Surging.Hero.BasicData.IApplication.Wordbook.IWordbookAppService.GetWordbookItems_input', 1, '2020-12-14 17:37:12', 1, '2020-12-14 17:37:12');
INSERT INTO `hero_auth`.`OperationActionRelation`(`Id`, `OperationId`, `ActionId`, `ServiceId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (31, 37, 55, 'Surging.Hero.BasicData.IApplication.Wordbook.IWordbookAppService.CreateWordbookItem_input', 1, '2020-12-14 17:37:56', 1, '2020-12-14 17:37:56');
INSERT INTO `hero_auth`.`OperationActionRelation`(`Id`, `OperationId`, `ActionId`, `ServiceId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (32, 38, 56, 'Surging.Hero.BasicData.IApplication.Wordbook.IWordbookAppService.UpdateWordbookItem_input', 1, '2020-12-14 17:38:24', 1, '2020-12-14 17:38:24');
INSERT INTO `hero_auth`.`OperationActionRelation`(`Id`, `OperationId`, `ActionId`, `ServiceId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (33, 39, 57, 'Surging.Hero.BasicData.IApplication.Wordbook.IWordbookAppService.DeleteWordbookItem_id', 1, '2020-12-14 17:39:02', 1, '2020-12-14 17:39:02');
INSERT INTO `hero_auth`.`OperationActionRelation`(`Id`, `OperationId`, `ActionId`, `ServiceId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (35, 27, 33, 'Surging.Hero.Auth.IApplication.Permission.IPermissionAppService.UpdateOperation_input', 1, '2020-12-16 02:50:00', 1, '2020-12-16 02:50:00');
INSERT INTO `hero_auth`.`OperationActionRelation`(`Id`, `OperationId`, `ActionId`, `ServiceId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (36, 28, 36, 'Surging.Hero.Auth.IApplication.Permission.IPermissionAppService.Delete_input', 1, '2020-12-16 02:50:18', 1, '2020-12-16 02:50:18');
INSERT INTO `hero_auth`.`OperationActionRelation`(`Id`, `OperationId`, `ActionId`, `ServiceId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (38, 10, 18, 'Surging.Hero.Auth.IApplication.UserGroup.IUserGroupAppService.Search_query', 1, '2020-12-21 23:47:17', 1, '2020-12-21 23:47:17');
INSERT INTO `hero_auth`.`OperationActionRelation`(`Id`, `OperationId`, `ActionId`, `ServiceId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (39, 18, 26, 'Surging.Hero.Auth.IApplication.Role.IRoleAppService.Search_query', 1, '2020-12-21 23:47:30', 1, '2020-12-21 23:47:30');
INSERT INTO `hero_auth`.`OperationActionRelation`(`Id`, `OperationId`, `ActionId`, `ServiceId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (52, 41, 24, 'Surging.Hero.Auth.IApplication.Role.IRoleAppService.Status_input', 1, '2020-12-24 21:28:56', 1, '2020-12-24 21:28:56');
INSERT INTO `hero_auth`.`OperationActionRelation`(`Id`, `OperationId`, `ActionId`, `ServiceId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (61, 4, 5, 'Surging.Hero.Auth.IApplication.User.IUserAppService.Search_query', 1, '2020-12-28 14:58:30', 1, '2020-12-28 14:58:30');
INSERT INTO `hero_auth`.`OperationActionRelation`(`Id`, `OperationId`, `ActionId`, `ServiceId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (62, 4, 102, 'Surging.Hero.Organization.IApplication.Organization.IOrganizationAppService.GetOwnTree', 1, '2020-12-28 14:58:30', 1, '2020-12-28 14:58:30');
INSERT INTO `hero_auth`.`OperationActionRelation`(`Id`, `OperationId`, `ActionId`, `ServiceId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (63, 15, 22, 'Surging.Hero.Auth.IApplication.Role.IRoleAppService.Create_input', 1, '2020-12-28 15:24:51', 1, '2020-12-28 15:24:51');
INSERT INTO `hero_auth`.`OperationActionRelation`(`Id`, `OperationId`, `ActionId`, `ServiceId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (64, 15, 102, 'Surging.Hero.Organization.IApplication.Organization.IOrganizationAppService.GetOwnTree', 1, '2020-12-28 15:24:52', 1, '2020-12-28 15:24:52');
INSERT INTO `hero_auth`.`OperationActionRelation`(`Id`, `OperationId`, `ActionId`, `ServiceId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (65, 16, 23, 'Surging.Hero.Auth.IApplication.Role.IRoleAppService.Update_input', 1, '2020-12-28 15:25:06', 1, '2020-12-28 15:25:06');
INSERT INTO `hero_auth`.`OperationActionRelation`(`Id`, `OperationId`, `ActionId`, `ServiceId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (66, 16, 102, 'Surging.Hero.Organization.IApplication.Organization.IOrganizationAppService.GetOwnTree', 1, '2020-12-28 15:25:07', 1, '2020-12-28 15:25:07');
INSERT INTO `hero_auth`.`OperationActionRelation`(`Id`, `OperationId`, `ActionId`, `ServiceId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (67, 2, 3, 'Surging.Hero.Auth.IApplication.User.IUserAppService.Update_input', 13, '2021-01-02 00:41:26', 13, '2021-01-02 00:41:26');
INSERT INTO `hero_auth`.`OperationActionRelation`(`Id`, `OperationId`, `ActionId`, `ServiceId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (68, 2, 97, 'Surging.Hero.Auth.IApplication.User.IUserAppService.CheckCreateUser_orgId', 13, '2021-01-02 00:41:27', 13, '2021-01-02 00:41:27');
INSERT INTO `hero_auth`.`OperationActionRelation`(`Id`, `OperationId`, `ActionId`, `ServiceId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (69, 2, 102, 'Surging.Hero.Organization.IApplication.Organization.IOrganizationAppService.GetOwnTree', 13, '2021-01-02 00:41:27', 13, '2021-01-02 00:41:27');
INSERT INTO `hero_auth`.`OperationActionRelation`(`Id`, `OperationId`, `ActionId`, `ServiceId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (72, 45, 104, 'Surging.Hero.Auth.IApplication.Tenant.ITenantAppService.Create_input', 1, '2021-01-06 15:56:23', 1, '2021-01-06 15:56:23');
INSERT INTO `hero_auth`.`OperationActionRelation`(`Id`, `OperationId`, `ActionId`, `ServiceId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (73, 46, 105, 'Surging.Hero.Auth.IApplication.Tenant.ITenantAppService.Update_input', 1, '2021-01-06 15:56:48', 1, '2021-01-06 15:56:48');
INSERT INTO `hero_auth`.`OperationActionRelation`(`Id`, `OperationId`, `ActionId`, `ServiceId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (74, 47, 106, 'Surging.Hero.Auth.IApplication.Tenant.ITenantAppService.Delete_id', 1, '2021-01-06 15:57:28', 1, '2021-01-06 15:57:28');
INSERT INTO `hero_auth`.`OperationActionRelation`(`Id`, `OperationId`, `ActionId`, `ServiceId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (75, 48, 107, 'Surging.Hero.Auth.IApplication.Tenant.ITenantAppService.Search_query', 1, '2021-01-06 16:06:22', 1, '2021-01-06 16:06:22');
INSERT INTO `hero_auth`.`OperationActionRelation`(`Id`, `OperationId`, `ActionId`, `ServiceId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (76, 49, 109, 'Surging.Hero.Auth.IApplication.Tenant.ITenantAppService.Status_input', 1, '2021-01-06 16:07:31', 1, '2021-01-06 16:07:31');
INSERT INTO `hero_auth`.`OperationActionRelation`(`Id`, `OperationId`, `ActionId`, `ServiceId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (77, 51, 113, 'Surging.Hero.BasicData.IApplication.SystemConfig.ISystemConfigAppService.SetSystemConfig_input', 1, '2021-01-07 21:33:44', 1, '2021-01-07 21:33:44');
INSERT INTO `hero_auth`.`OperationActionRelation`(`Id`, `OperationId`, `ActionId`, `ServiceId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (78, 1, 2, 'Surging.Hero.Auth.IApplication.User.IUserAppService.Create_input', 11, '2021-01-08 22:24:55', 11, '2021-01-08 22:24:55');
INSERT INTO `hero_auth`.`OperationActionRelation`(`Id`, `OperationId`, `ActionId`, `ServiceId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (79, 1, 97, 'Surging.Hero.Auth.IApplication.User.IUserAppService.CheckCreateUser_orgId', 11, '2021-01-08 22:24:55', 11, '2021-01-08 22:24:55');
INSERT INTO `hero_auth`.`OperationActionRelation`(`Id`, `OperationId`, `ActionId`, `ServiceId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (80, 1, 102, 'Surging.Hero.Organization.IApplication.Organization.IOrganizationAppService.GetOwnTree', 11, '2021-01-08 22:24:55', 11, '2021-01-08 22:24:55');


INSERT INTO `hero_auth`.`Permission`(`Id`, `Name`, `Mold`, `Memo`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (1, 'auth', 0, NULL, 1, NULL, NULL, NULL, NULL, 0, NULL, NULL);
INSERT INTO `hero_auth`.`Permission`(`Id`, `Name`, `Mold`, `Memo`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (2, 'user', 0, NULL, 1, NULL, NULL, NULL, NULL, 0, NULL, NULL);
INSERT INTO `hero_auth`.`Permission`(`Id`, `Name`, `Mold`, `Memo`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (3, 'usergroup', 0, NULL, 1, NULL, NULL, NULL, NULL, 0, NULL, NULL);
INSERT INTO `hero_auth`.`Permission`(`Id`, `Name`, `Mold`, `Memo`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (4, 'role', 0, NULL, 1, NULL, NULL, NULL, NULL, 0, NULL, NULL);
INSERT INTO `hero_auth`.`Permission`(`Id`, `Name`, `Mold`, `Memo`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (5, 'organization', 0, NULL, 1, NULL, '2020-12-14 17:32:40', 1, '2020-12-14 17:32:49', 0, NULL, NULL);
INSERT INTO `hero_auth`.`Permission`(`Id`, `Name`, `Mold`, `Memo`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (6, 'menu', 0, NULL, 1, NULL, NULL, NULL, NULL, 0, NULL, NULL);
INSERT INTO `hero_auth`.`Permission`(`Id`, `Name`, `Mold`, `Memo`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (7, 'system', 0, NULL, 1, 1, '2020-12-14 16:06:47', 1, '2020-12-14 16:06:47', 0, NULL, NULL);
INSERT INTO `hero_auth`.`Permission`(`Id`, `Name`, `Mold`, `Memo`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (8, 'wordbook', 0, NULL, 1, 1, '2020-12-14 16:07:12', 1, '2020-12-14 16:07:12', 0, NULL, NULL);
INSERT INTO `hero_auth`.`Permission`(`Id`, `Name`, `Mold`, `Memo`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (9, 'user-create', 1, NULL, 1, 1, '2020-12-14 16:26:23', 11, '2021-01-08 22:24:55', 0, NULL, NULL);
INSERT INTO `hero_auth`.`Permission`(`Id`, `Name`, `Mold`, `Memo`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (10, 'user-update', 1, NULL, 1, 1, '2020-12-14 16:26:58', 13, '2021-01-02 00:41:26', 0, NULL, NULL);
INSERT INTO `hero_auth`.`Permission`(`Id`, `Name`, `Mold`, `Memo`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (11, 'user-delete', 1, NULL, 1, 1, '2020-12-14 16:27:58', 1, '2020-12-14 16:27:58', 0, NULL, NULL);
INSERT INTO `hero_auth`.`Permission`(`Id`, `Name`, `Mold`, `Memo`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (12, 'user-search', 1, NULL, 1, 1, '2020-12-14 16:28:23', 1, '2020-12-28 14:58:29', 0, NULL, NULL);
INSERT INTO `hero_auth`.`Permission`(`Id`, `Name`, `Mold`, `Memo`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (13, 'user-status', 1, NULL, 1, 1, '2020-12-14 16:29:08', 1, '2020-12-14 16:29:08', 0, NULL, NULL);
INSERT INTO `hero_auth`.`Permission`(`Id`, `Name`, `Mold`, `Memo`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (14, 'user-reset-password', 1, NULL, 1, 1, '2020-12-14 16:29:44', 1, '2020-12-14 16:29:44', 0, NULL, NULL);
INSERT INTO `hero_auth`.`Permission`(`Id`, `Name`, `Mold`, `Memo`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (15, 'usergroup-create', 1, NULL, 1, 1, '2020-12-14 16:30:34', 1, '2020-12-14 16:30:34', 0, NULL, NULL);
INSERT INTO `hero_auth`.`Permission`(`Id`, `Name`, `Mold`, `Memo`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (16, 'usergroup-update', 1, NULL, 1, 1, '2020-12-14 16:31:02', 1, '2020-12-14 16:31:02', 0, NULL, NULL);
INSERT INTO `hero_auth`.`Permission`(`Id`, `Name`, `Mold`, `Memo`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (17, 'usergroup-delete', 1, NULL, 1, 1, '2020-12-14 16:31:41', 1, '2020-12-14 16:31:41', 0, NULL, NULL);
INSERT INTO `hero_auth`.`Permission`(`Id`, `Name`, `Mold`, `Memo`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (18, 'usergroup-search', 1, NULL, 1, 1, '2020-12-14 16:32:23', 1, '2020-12-21 23:47:17', 0, NULL, NULL);
INSERT INTO `hero_auth`.`Permission`(`Id`, `Name`, `Mold`, `Memo`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (19, 'usergroup-status', 1, NULL, 1, 1, '2020-12-14 16:33:54', 1, '2020-12-14 16:33:54', 0, NULL, NULL);
INSERT INTO `hero_auth`.`Permission`(`Id`, `Name`, `Mold`, `Memo`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (20, 'usergroup-allocation-user', 1, NULL, 1, 1, '2020-12-14 16:35:22', 1, '2020-12-14 16:35:22', 0, NULL, NULL);
INSERT INTO `hero_auth`.`Permission`(`Id`, `Name`, `Mold`, `Memo`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (21, 'usergroup-delete-user', 1, NULL, 1, 1, '2020-12-14 16:36:18', 1, '2020-12-14 16:36:18', 0, NULL, NULL);
INSERT INTO `hero_auth`.`Permission`(`Id`, `Name`, `Mold`, `Memo`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (22, 'usergroup-search-user', 1, NULL, 1, 1, '2020-12-14 16:36:59', 1, '2020-12-14 16:36:59', 0, NULL, NULL);
INSERT INTO `hero_auth`.`Permission`(`Id`, `Name`, `Mold`, `Memo`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (23, 'role-create', 1, NULL, 1, 1, '2020-12-14 16:38:43', 1, '2020-12-28 15:24:51', 0, NULL, NULL);
INSERT INTO `hero_auth`.`Permission`(`Id`, `Name`, `Mold`, `Memo`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (24, 'role-update', 1, NULL, 1, 1, '2020-12-14 16:39:24', 1, '2020-12-28 15:25:06', 0, NULL, NULL);
INSERT INTO `hero_auth`.`Permission`(`Id`, `Name`, `Mold`, `Memo`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (25, 'role-delete', 1, NULL, 1, 1, '2020-12-14 16:39:55', 1, '2020-12-14 16:39:55', 0, NULL, NULL);
INSERT INTO `hero_auth`.`Permission`(`Id`, `Name`, `Mold`, `Memo`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (26, 'role-search', 1, NULL, 1, 1, '2020-12-14 16:40:37', 1, '2020-12-21 23:47:30', 0, NULL, NULL);
INSERT INTO `hero_auth`.`Permission`(`Id`, `Name`, `Mold`, `Memo`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (27, 'corporation-create', 1, NULL, 1, 1, '2020-12-14 16:43:29', 1, '2020-12-14 17:23:17', 0, NULL, NULL);
INSERT INTO `hero_auth`.`Permission`(`Id`, `Name`, `Mold`, `Memo`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (28, 'corporation-update', 1, NULL, 1, 1, '2020-12-14 17:24:45', 1, '2020-12-14 17:25:38', 0, NULL, NULL);
INSERT INTO `hero_auth`.`Permission`(`Id`, `Name`, `Mold`, `Memo`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (29, 'corporation-detele', 1, NULL, 1, 1, '2020-12-14 17:25:28', 1, '2020-12-14 17:25:28', 0, NULL, NULL);
INSERT INTO `hero_auth`.`Permission`(`Id`, `Name`, `Mold`, `Memo`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (30, 'department-create', 1, NULL, 1, 1, '2020-12-14 17:26:25', 1, '2020-12-14 17:26:25', 0, NULL, NULL);
INSERT INTO `hero_auth`.`Permission`(`Id`, `Name`, `Mold`, `Memo`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (31, 'department-update', 1, NULL, 1, 1, '2020-12-14 17:26:49', 1, '2020-12-14 17:26:49', 0, NULL, NULL);
INSERT INTO `hero_auth`.`Permission`(`Id`, `Name`, `Mold`, `Memo`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (32, 'department-delete', 1, NULL, 1, 1, '2020-12-14 17:27:10', 1, '2020-12-14 17:27:10', 0, NULL, NULL);
INSERT INTO `hero_auth`.`Permission`(`Id`, `Name`, `Mold`, `Memo`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (33, 'department-look', 1, NULL, 1, 1, '2020-12-14 17:27:38', 1, '2020-12-14 17:27:38', 0, NULL, NULL);
INSERT INTO `hero_auth`.`Permission`(`Id`, `Name`, `Mold`, `Memo`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (34, 'menu-create', 1, NULL, 1, 1, '2020-12-14 17:28:36', 1, '2020-12-14 17:28:36', 0, NULL, NULL);
INSERT INTO `hero_auth`.`Permission`(`Id`, `Name`, `Mold`, `Memo`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (35, 'menu-update', 1, NULL, 1, 1, '2020-12-14 17:29:13', 1, '2020-12-16 02:50:00', 0, NULL, NULL);
INSERT INTO `hero_auth`.`Permission`(`Id`, `Name`, `Mold`, `Memo`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (36, 'menu-delete', 1, NULL, 1, 1, '2020-12-14 17:29:34', 1, '2020-12-16 02:50:18', 0, NULL, NULL);
INSERT INTO `hero_auth`.`Permission`(`Id`, `Name`, `Mold`, `Memo`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (37, 'operation-create', 1, NULL, 1, 1, '2020-12-14 17:30:32', 1, '2020-12-14 17:30:32', 0, NULL, NULL);
INSERT INTO `hero_auth`.`Permission`(`Id`, `Name`, `Mold`, `Memo`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (38, 'operation-update', 1, NULL, 1, 1, '2020-12-14 17:31:05', 1, '2020-12-14 17:31:05', 0, NULL, NULL);
INSERT INTO `hero_auth`.`Permission`(`Id`, `Name`, `Mold`, `Memo`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (39, 'operation-delete', 1, NULL, 1, 1, '2020-12-14 17:31:36', 1, '2020-12-14 17:31:36', 0, NULL, NULL);
INSERT INTO `hero_auth`.`Permission`(`Id`, `Name`, `Mold`, `Memo`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (40, 'operation-look', 1, NULL, 1, 1, '2020-12-14 17:32:18', 1, '2020-12-14 17:32:18', 0, NULL, NULL);
INSERT INTO `hero_auth`.`Permission`(`Id`, `Name`, `Mold`, `Memo`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (41, 'wordbook-create', 1, NULL, 1, 1, '2020-12-14 17:34:32', 1, '2020-12-14 17:34:32', 0, NULL, NULL);
INSERT INTO `hero_auth`.`Permission`(`Id`, `Name`, `Mold`, `Memo`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (42, 'wordbook-update', 1, NULL, 1, 1, '2020-12-14 17:34:52', 1, '2020-12-14 17:34:52', 0, NULL, NULL);
INSERT INTO `hero_auth`.`Permission`(`Id`, `Name`, `Mold`, `Memo`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (43, 'wordbook-delete', 1, NULL, 1, 1, '2020-12-14 17:35:08', 1, '2020-12-14 17:35:08', 0, NULL, NULL);
INSERT INTO `hero_auth`.`Permission`(`Id`, `Name`, `Mold`, `Memo`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (44, 'wordbook-item-look', 1, NULL, 1, 1, '2020-12-14 17:36:35', 1, '2020-12-14 17:37:12', 0, NULL, NULL);
INSERT INTO `hero_auth`.`Permission`(`Id`, `Name`, `Mold`, `Memo`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (45, 'wordbook-item-create', 1, NULL, 1, 1, '2020-12-14 17:37:56', 1, '2020-12-14 17:37:56', 0, NULL, NULL);
INSERT INTO `hero_auth`.`Permission`(`Id`, `Name`, `Mold`, `Memo`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (46, 'wordbook-item-update', 1, NULL, 1, 1, '2020-12-14 17:38:24', 1, '2020-12-14 17:38:24', 0, NULL, NULL);
INSERT INTO `hero_auth`.`Permission`(`Id`, `Name`, `Mold`, `Memo`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (47, 'wordbook-item-delete', 1, NULL, 1, 1, '2020-12-14 17:39:02', 1, '2020-12-14 17:39:02', 0, NULL, NULL);
INSERT INTO `hero_auth`.`Permission`(`Id`, `Name`, `Mold`, `Memo`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (48, 'wordbook-search', 1, NULL, 1, 1, '2020-12-14 17:39:45', 1, '2020-12-14 17:39:45', 0, NULL, NULL);
INSERT INTO `hero_auth`.`Permission`(`Id`, `Name`, `Mold`, `Memo`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (49, 'role-status', 1, NULL, 1, 1, '2020-12-24 21:28:55', 1, '2020-12-24 21:28:55', 0, NULL, NULL);
INSERT INTO `hero_auth`.`Permission`(`Id`, `Name`, `Mold`, `Memo`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (50, 'role-look', 1, NULL, 1, 1, '2020-12-24 21:30:35', 1, '2020-12-24 21:30:35', 0, NULL, NULL);
INSERT INTO `hero_auth`.`Permission`(`Id`, `Name`, `Mold`, `Memo`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (51, 'user-look', 1, NULL, 1, 1, '2020-12-24 21:32:39', 1, '2020-12-24 21:32:39', 0, NULL, NULL);
INSERT INTO `hero_auth`.`Permission`(`Id`, `Name`, `Mold`, `Memo`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (52, 'tenant', 0, NULL, 1, 1, '2021-01-06 15:52:16', 1, '2021-01-06 15:52:16', 0, NULL, NULL);
INSERT INTO `hero_auth`.`Permission`(`Id`, `Name`, `Mold`, `Memo`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (54, 'tenant-create', 1, NULL, 1, 1, '2021-01-06 15:56:14', 1, '2021-01-06 15:56:22', 0, NULL, NULL);
INSERT INTO `hero_auth`.`Permission`(`Id`, `Name`, `Mold`, `Memo`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (55, 'tenant-update', 1, NULL, 1, 1, '2021-01-06 15:56:48', 1, '2021-01-06 15:56:48', 0, NULL, NULL);
INSERT INTO `hero_auth`.`Permission`(`Id`, `Name`, `Mold`, `Memo`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (56, 'tenant-delete', 1, NULL, 1, 1, '2021-01-06 15:57:28', 1, '2021-01-06 15:57:28', 0, NULL, NULL);
INSERT INTO `hero_auth`.`Permission`(`Id`, `Name`, `Mold`, `Memo`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (57, 'tenant-search', 1, NULL, 1, 1, '2021-01-06 16:06:22', 1, '2021-01-06 16:06:22', 0, NULL, NULL);
INSERT INTO `hero_auth`.`Permission`(`Id`, `Name`, `Mold`, `Memo`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (58, 'tenant-status', 1, NULL, 1, 1, '2021-01-06 16:07:31', 1, '2021-01-06 16:07:31', 0, NULL, NULL);
INSERT INTO `hero_auth`.`Permission`(`Id`, `Name`, `Mold`, `Memo`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (59, 'setting', 0, NULL, 1, 1, '2021-01-07 21:30:04', 1, '2021-01-07 21:30:04', 0, NULL, NULL);
INSERT INTO `hero_auth`.`Permission`(`Id`, `Name`, `Mold`, `Memo`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (60, 'setting-look', 1, NULL, 1, 1, '2021-01-07 21:31:13', 1, '2021-01-07 21:31:13', 0, NULL, NULL);
INSERT INTO `hero_auth`.`Permission`(`Id`, `Name`, `Mold`, `Memo`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (61, 'setting-config', 1, NULL, 1, 1, '2021-01-07 21:33:44', 1, '2021-01-07 21:33:44', 0, NULL, NULL);

INSERT INTO `hero_auth`.`Role`(`Id`, `Identification`, `Name`, `IsAllOrg`, `Memo`, `Status`, `DataPermissionType`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`, `TenantId`) VALUES (1, 'supermanager', '管理员', b'1', '初始化系统时新增', 1, 9999, 1, '2020-12-14 16:07:27', 11, '2021-01-08 22:25:49', 0, NULL, NULL, 1);
INSERT INTO `hero_auth`.`Role`(`Id`, `Identification`, `Name`, `IsAllOrg`, `Memo`, `Status`, `DataPermissionType`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`, `TenantId`) VALUES (8, 'normdev', '开发者', b'1', '开发人员专用', 1, 3, 1, '2020-12-24 22:17:26', 1, '2021-01-08 22:27:42', 0, NULL, NULL, 1);
INSERT INTO `hero_auth`.`Role`(`Id`, `Identification`, `Name`, `IsAllOrg`, `Memo`, `Status`, `DataPermissionType`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`, `TenantId`) VALUES (9, 'normtest', '普通员工', b'1', '普通员工拥有的权限', 1, 1, 1, '2020-12-24 22:23:32', 1, '2020-12-29 09:17:04', 0, NULL, NULL, 1);
INSERT INTO `hero_auth`.`Role`(`Id`, `Identification`, `Name`, `IsAllOrg`, `Memo`, `Status`, `DataPermissionType`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`, `TenantId`) VALUES (11, 'testrole1', '测试1', b'0', '测试新增部门', 1, 3, 1, '2020-12-27 19:13:10', 1, '2021-01-02 00:31:37', 0, NULL, NULL, 1);
INSERT INTO `hero_auth`.`Role`(`Id`, `Identification`, `Name`, `IsAllOrg`, `Memo`, `Status`, `DataPermissionType`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`, `TenantId`) VALUES (13, 'roletest2', '角色测试2', b'0', '测试角色2', 1, 3, 1, '2020-12-27 19:27:57', 1, '2020-12-27 19:47:41', 1, 1, '2020-12-27 19:47:41', 1);
INSERT INTO `hero_auth`.`Role`(`Id`, `Identification`, `Name`, `IsAllOrg`, `Memo`, `Status`, `DataPermissionType`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`, `TenantId`) VALUES (14, 'approle', '移动组角色', b'0', '测试角色修改', 1, 3, 1, '2020-12-27 19:30:29', 1, '2021-01-01 23:51:27', 0, NULL, NULL, 1);
INSERT INTO `hero_auth`.`Role`(`Id`, `Identification`, `Name`, `IsAllOrg`, `Memo`, `Status`, `DataPermissionType`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`, `TenantId`) VALUES (19, 'test-2_admin', '管理员', b'1', '创建租户时,初始化的角色', 1, 9999, 1, '2021-01-07 14:31:53', 1, '2021-01-07 14:31:53', 0, NULL, NULL, 19);
INSERT INTO `hero_auth`.`Role`(`Id`, `Identification`, `Name`, `IsAllOrg`, `Memo`, `Status`, `DataPermissionType`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`, `TenantId`) VALUES (20, 'test3_admin', '管理员', b'1', '创建租户时,初始化的角色', 1, 9999, 1, '2021-01-07 14:57:50', 1, '2021-01-07 14:57:50', 0, NULL, NULL, 20);
INSERT INTO `hero_auth`.`Role`(`Id`, `Identification`, `Name`, `IsAllOrg`, `Memo`, `Status`, `DataPermissionType`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`, `TenantId`) VALUES (21, 'test-tenant4_admin', '管理员', b'1', '创建租户时,初始化的角色', 1, 9999, 1, '2021-01-07 15:04:01', 1, '2021-01-07 15:04:01', 0, NULL, NULL, 21);
INSERT INTO `hero_auth`.`Role`(`Id`, `Identification`, `Name`, `IsAllOrg`, `Memo`, `Status`, `DataPermissionType`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`, `TenantId`) VALUES (22, 'surging-cloud_admin', '管理员', b'1', '创建租户时,初始化的角色', 1, 9999, 1, '2021-01-07 16:24:13', 23, '2021-01-07 16:33:17', 0, NULL, NULL, 22);

INSERT INTO `hero_auth`.`RoleDataPermissionOrgRelation`(`Id`, `RoleId`, `OrgId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (180, 14, 8, 1, '2021-01-01 23:51:28', NULL, NULL, 1);
INSERT INTO `hero_auth`.`RoleDataPermissionOrgRelation`(`Id`, `RoleId`, `OrgId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (181, 14, 26, 1, '2021-01-01 23:51:28', NULL, NULL, 1);
INSERT INTO `hero_auth`.`RoleDataPermissionOrgRelation`(`Id`, `RoleId`, `OrgId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (182, 14, 27, 1, '2021-01-01 23:51:28', NULL, NULL, 1);
INSERT INTO `hero_auth`.`RoleDataPermissionOrgRelation`(`Id`, `RoleId`, `OrgId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (183, 14, 28, 1, '2021-01-01 23:51:28', NULL, NULL, 1);
INSERT INTO `hero_auth`.`RoleDataPermissionOrgRelation`(`Id`, `RoleId`, `OrgId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (184, 14, 29, 1, '2021-01-01 23:51:28', NULL, NULL, 1);
INSERT INTO `hero_auth`.`RoleDataPermissionOrgRelation`(`Id`, `RoleId`, `OrgId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (185, 11, 2, 1, '2021-01-02 00:31:38', NULL, NULL, 1);
INSERT INTO `hero_auth`.`RoleDataPermissionOrgRelation`(`Id`, `RoleId`, `OrgId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (186, 11, 3, 1, '2021-01-02 00:31:38', NULL, NULL, 1);
INSERT INTO `hero_auth`.`RoleDataPermissionOrgRelation`(`Id`, `RoleId`, `OrgId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (187, 11, 7, 1, '2021-01-02 00:31:38', NULL, NULL, 1);
INSERT INTO `hero_auth`.`RoleDataPermissionOrgRelation`(`Id`, `RoleId`, `OrgId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (188, 8, 2, 1, '2021-01-08 22:27:42', NULL, NULL, 1);
INSERT INTO `hero_auth`.`RoleDataPermissionOrgRelation`(`Id`, `RoleId`, `OrgId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (189, 8, 3, 1, '2021-01-08 22:27:42', NULL, NULL, 1);
INSERT INTO `hero_auth`.`RoleDataPermissionOrgRelation`(`Id`, `RoleId`, `OrgId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (190, 8, 7, 1, '2021-01-08 22:27:42', NULL, NULL, 1);
INSERT INTO `hero_auth`.`RoleDataPermissionOrgRelation`(`Id`, `RoleId`, `OrgId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (191, 8, 8, 1, '2021-01-08 22:27:42', NULL, NULL, 1);
INSERT INTO `hero_auth`.`RoleDataPermissionOrgRelation`(`Id`, `RoleId`, `OrgId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (192, 8, 26, 1, '2021-01-08 22:27:42', NULL, NULL, 1);
INSERT INTO `hero_auth`.`RoleDataPermissionOrgRelation`(`Id`, `RoleId`, `OrgId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (193, 8, 27, 1, '2021-01-08 22:27:42', NULL, NULL, 1);
INSERT INTO `hero_auth`.`RoleDataPermissionOrgRelation`(`Id`, `RoleId`, `OrgId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (194, 8, 28, 1, '2021-01-08 22:27:42', NULL, NULL, 1);
INSERT INTO `hero_auth`.`RoleDataPermissionOrgRelation`(`Id`, `RoleId`, `OrgId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (195, 8, 29, 1, '2021-01-08 22:27:42', NULL, NULL, 1);

INSERT INTO `hero_auth`.`RoleOrganization`(`Id`, `RoleId`, `OrgId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (50, 14, 27, 1, '2021-01-01 23:51:28', 1, '2021-01-01 23:51:28', 1);
INSERT INTO `hero_auth`.`RoleOrganization`(`Id`, `RoleId`, `OrgId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (51, 14, 28, 1, '2021-01-01 23:51:28', 1, '2021-01-01 23:51:28', 1);
INSERT INTO `hero_auth`.`RoleOrganization`(`Id`, `RoleId`, `OrgId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (52, 11, 2, 1, '2021-01-02 00:31:37', 1, '2021-01-02 00:31:37', 1);


INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (1346, 5, 2, 1, '2020-12-24 16:51:31', NULL, NULL, 1);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (1347, 5, 9, 1, '2020-12-24 16:51:31', NULL, NULL, 1);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (1348, 5, 10, 1, '2020-12-24 16:51:31', NULL, NULL, 1);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (1349, 5, 11, 1, '2020-12-24 16:51:31', NULL, NULL, 1);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (1350, 5, 12, 1, '2020-12-24 16:51:31', NULL, NULL, 1);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (1351, 5, 13, 1, '2020-12-24 16:51:31', NULL, NULL, 1);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (1352, 5, 14, 1, '2020-12-24 16:51:31', NULL, NULL, 1);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (1353, 5, 4, 1, '2020-12-24 16:51:31', NULL, NULL, 1);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (1354, 5, 23, 1, '2020-12-24 16:51:31', NULL, NULL, 1);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (1355, 5, 24, 1, '2020-12-24 16:51:31', NULL, NULL, 1);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (1356, 5, 25, 1, '2020-12-24 16:51:31', NULL, NULL, 1);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (1357, 5, 26, 1, '2020-12-24 16:51:31', NULL, NULL, 1);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (1370, 6, 2, 1, '2020-12-24 21:08:22', NULL, NULL, 1);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (1371, 6, 9, 1, '2020-12-24 21:08:22', NULL, NULL, 1);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (1372, 6, 10, 1, '2020-12-24 21:08:22', NULL, NULL, 1);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (1373, 6, 11, 1, '2020-12-24 21:08:22', NULL, NULL, 1);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (1374, 6, 12, 1, '2020-12-24 21:08:22', NULL, NULL, 1);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (1375, 6, 13, 1, '2020-12-24 21:08:22', NULL, NULL, 1);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (1376, 6, 14, 1, '2020-12-24 21:08:22', NULL, NULL, 1);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (1377, 6, 15, 1, '2020-12-24 21:08:22', NULL, NULL, 1);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (1378, 6, 16, 1, '2020-12-24 21:08:22', NULL, NULL, 1);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (1379, 6, 18, 1, '2020-12-24 21:08:22', NULL, NULL, 1);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (1380, 6, 4, 1, '2020-12-24 21:08:22', NULL, NULL, 1);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (1381, 6, 23, 1, '2020-12-24 21:08:22', NULL, NULL, 1);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (1382, 6, 24, 1, '2020-12-24 21:08:22', NULL, NULL, 1);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (1383, 6, 25, 1, '2020-12-24 21:08:22', NULL, NULL, 1);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (1384, 6, 26, 1, '2020-12-24 21:08:22', NULL, NULL, 1);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (1433, 7, 9, 2, '2020-12-24 21:15:23', NULL, NULL, 1);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (1434, 7, 10, 2, '2020-12-24 21:15:23', NULL, NULL, 1);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (1435, 7, 12, 2, '2020-12-24 21:15:23', NULL, NULL, 1);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (1436, 7, 13, 2, '2020-12-24 21:15:23', NULL, NULL, 1);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (1437, 7, 15, 2, '2020-12-24 21:15:23', NULL, NULL, 1);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (2643, 9, 12, 1, '2020-12-29 09:17:04', NULL, NULL, 1);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (2644, 9, 51, 1, '2020-12-29 09:17:04', NULL, NULL, 1);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (2645, 9, 26, 1, '2020-12-29 09:17:04', NULL, NULL, 1);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (2646, 9, 50, 1, '2020-12-29 09:17:04', NULL, NULL, 1);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (2800, 14, 2, 1, '2021-01-01 23:51:28', NULL, NULL, 1);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (2801, 14, 9, 1, '2021-01-01 23:51:28', NULL, NULL, 1);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (2802, 14, 10, 1, '2021-01-01 23:51:28', NULL, NULL, 1);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (2803, 14, 11, 1, '2021-01-01 23:51:28', NULL, NULL, 1);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (2804, 14, 12, 1, '2021-01-01 23:51:28', NULL, NULL, 1);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (2805, 14, 13, 1, '2021-01-01 23:51:28', NULL, NULL, 1);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (2806, 14, 14, 1, '2021-01-01 23:51:28', NULL, NULL, 1);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (2807, 14, 51, 1, '2021-01-01 23:51:28', NULL, NULL, 1);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (2808, 14, 3, 1, '2021-01-01 23:51:28', NULL, NULL, 1);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (2809, 14, 15, 1, '2021-01-01 23:51:28', NULL, NULL, 1);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (2810, 14, 16, 1, '2021-01-01 23:51:28', NULL, NULL, 1);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (2811, 14, 17, 1, '2021-01-01 23:51:28', NULL, NULL, 1);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (2812, 14, 18, 1, '2021-01-01 23:51:28', NULL, NULL, 1);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (2813, 14, 19, 1, '2021-01-01 23:51:28', NULL, NULL, 1);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (2814, 14, 20, 1, '2021-01-01 23:51:28', NULL, NULL, 1);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (2815, 14, 21, 1, '2021-01-01 23:51:28', NULL, NULL, 1);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (2816, 14, 22, 1, '2021-01-01 23:51:28', NULL, NULL, 1);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (2817, 11, 9, 1, '2021-01-02 00:31:37', NULL, NULL, 1);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (2818, 11, 10, 1, '2021-01-02 00:31:37', NULL, NULL, 1);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (3389, 22, 1, 23, '2021-01-07 16:33:17', NULL, NULL, 22);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (3390, 22, 2, 23, '2021-01-07 16:33:17', NULL, NULL, 22);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (3391, 22, 3, 23, '2021-01-07 16:33:17', NULL, NULL, 22);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (3392, 22, 4, 23, '2021-01-07 16:33:17', NULL, NULL, 22);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (3393, 22, 5, 23, '2021-01-07 16:33:17', NULL, NULL, 22);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (3394, 22, 6, 23, '2021-01-07 16:33:17', NULL, NULL, 22);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (3395, 22, 7, 23, '2021-01-07 16:33:17', NULL, NULL, 22);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (3396, 22, 8, 23, '2021-01-07 16:33:17', NULL, NULL, 22);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (3397, 22, 9, 23, '2021-01-07 16:33:17', NULL, NULL, 22);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (3398, 22, 10, 23, '2021-01-07 16:33:17', NULL, NULL, 22);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (3399, 22, 11, 23, '2021-01-07 16:33:17', NULL, NULL, 22);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (3400, 22, 12, 23, '2021-01-07 16:33:17', NULL, NULL, 22);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (3401, 22, 13, 23, '2021-01-07 16:33:17', NULL, NULL, 22);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (3402, 22, 14, 23, '2021-01-07 16:33:17', NULL, NULL, 22);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (3403, 22, 15, 23, '2021-01-07 16:33:17', NULL, NULL, 22);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (3404, 22, 16, 23, '2021-01-07 16:33:17', NULL, NULL, 22);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (3405, 22, 17, 23, '2021-01-07 16:33:17', NULL, NULL, 22);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (3406, 22, 18, 23, '2021-01-07 16:33:17', NULL, NULL, 22);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (3407, 22, 19, 23, '2021-01-07 16:33:17', NULL, NULL, 22);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (3408, 22, 20, 23, '2021-01-07 16:33:17', NULL, NULL, 22);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (3409, 22, 21, 23, '2021-01-07 16:33:17', NULL, NULL, 22);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (3410, 22, 22, 23, '2021-01-07 16:33:17', NULL, NULL, 22);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (3411, 22, 23, 23, '2021-01-07 16:33:17', NULL, NULL, 22);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (3412, 22, 24, 23, '2021-01-07 16:33:17', NULL, NULL, 22);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (3413, 22, 25, 23, '2021-01-07 16:33:17', NULL, NULL, 22);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (3414, 22, 26, 23, '2021-01-07 16:33:17', NULL, NULL, 22);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (3415, 22, 27, 23, '2021-01-07 16:33:17', NULL, NULL, 22);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (3416, 22, 28, 23, '2021-01-07 16:33:17', NULL, NULL, 22);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (3417, 22, 29, 23, '2021-01-07 16:33:17', NULL, NULL, 22);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (3418, 22, 30, 23, '2021-01-07 16:33:17', NULL, NULL, 22);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (3419, 22, 31, 23, '2021-01-07 16:33:17', NULL, NULL, 22);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (3420, 22, 32, 23, '2021-01-07 16:33:17', NULL, NULL, 22);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (3421, 22, 33, 23, '2021-01-07 16:33:17', NULL, NULL, 22);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (3422, 22, 34, 23, '2021-01-07 16:33:17', NULL, NULL, 22);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (3423, 22, 35, 23, '2021-01-07 16:33:17', NULL, NULL, 22);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (3424, 22, 36, 23, '2021-01-07 16:33:17', NULL, NULL, 22);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (3425, 22, 37, 23, '2021-01-07 16:33:17', NULL, NULL, 22);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (3426, 22, 38, 23, '2021-01-07 16:33:17', NULL, NULL, 22);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (3427, 22, 39, 23, '2021-01-07 16:33:17', NULL, NULL, 22);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (3428, 22, 40, 23, '2021-01-07 16:33:17', NULL, NULL, 22);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (3429, 22, 41, 23, '2021-01-07 16:33:17', NULL, NULL, 22);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (3430, 22, 42, 23, '2021-01-07 16:33:17', NULL, NULL, 22);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (3431, 22, 43, 23, '2021-01-07 16:33:17', NULL, NULL, 22);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (3432, 22, 44, 23, '2021-01-07 16:33:17', NULL, NULL, 22);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (3433, 22, 45, 23, '2021-01-07 16:33:17', NULL, NULL, 22);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (3434, 22, 46, 23, '2021-01-07 16:33:17', NULL, NULL, 22);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (3435, 22, 47, 23, '2021-01-07 16:33:17', NULL, NULL, 22);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (3436, 22, 48, 23, '2021-01-07 16:33:17', NULL, NULL, 22);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (3437, 22, 49, 23, '2021-01-07 16:33:17', NULL, NULL, 22);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (3438, 22, 50, 23, '2021-01-07 16:33:17', NULL, NULL, 22);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (3439, 22, 51, 23, '2021-01-07 16:33:17', NULL, NULL, 22);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (3440, 22, 52, 23, '2021-01-07 16:33:17', NULL, NULL, 22);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (3441, 22, 54, 23, '2021-01-07 16:33:17', NULL, NULL, 22);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (3442, 22, 55, 23, '2021-01-07 16:33:17', NULL, NULL, 22);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (3443, 22, 56, 23, '2021-01-07 16:33:17', NULL, NULL, 22);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (3444, 22, 57, 23, '2021-01-07 16:33:17', NULL, NULL, 22);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (3445, 22, 58, 23, '2021-01-07 16:33:17', NULL, NULL, 22);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (3506, 1, 1, 11, '2021-01-08 22:25:50', NULL, NULL, 1);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (3507, 1, 2, 11, '2021-01-08 22:25:50', NULL, NULL, 1);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (3508, 1, 9, 11, '2021-01-08 22:25:50', NULL, NULL, 1);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (3509, 1, 10, 11, '2021-01-08 22:25:50', NULL, NULL, 1);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (3510, 1, 11, 11, '2021-01-08 22:25:50', NULL, NULL, 1);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (3511, 1, 12, 11, '2021-01-08 22:25:50', NULL, NULL, 1);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (3512, 1, 13, 11, '2021-01-08 22:25:50', NULL, NULL, 1);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (3513, 1, 14, 11, '2021-01-08 22:25:50', NULL, NULL, 1);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (3514, 1, 51, 11, '2021-01-08 22:25:50', NULL, NULL, 1);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (3515, 1, 3, 11, '2021-01-08 22:25:50', NULL, NULL, 1);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (3516, 1, 15, 11, '2021-01-08 22:25:50', NULL, NULL, 1);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (3517, 1, 16, 11, '2021-01-08 22:25:50', NULL, NULL, 1);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (3518, 1, 17, 11, '2021-01-08 22:25:50', NULL, NULL, 1);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (3519, 1, 18, 11, '2021-01-08 22:25:50', NULL, NULL, 1);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (3520, 1, 19, 11, '2021-01-08 22:25:50', NULL, NULL, 1);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (3521, 1, 20, 11, '2021-01-08 22:25:50', NULL, NULL, 1);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (3522, 1, 21, 11, '2021-01-08 22:25:50', NULL, NULL, 1);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (3523, 1, 22, 11, '2021-01-08 22:25:50', NULL, NULL, 1);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (3524, 1, 4, 11, '2021-01-08 22:25:50', NULL, NULL, 1);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (3525, 1, 23, 11, '2021-01-08 22:25:50', NULL, NULL, 1);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (3526, 1, 24, 11, '2021-01-08 22:25:50', NULL, NULL, 1);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (3527, 1, 25, 11, '2021-01-08 22:25:50', NULL, NULL, 1);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (3528, 1, 26, 11, '2021-01-08 22:25:50', NULL, NULL, 1);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (3529, 1, 49, 11, '2021-01-08 22:25:50', NULL, NULL, 1);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (3530, 1, 50, 11, '2021-01-08 22:25:50', NULL, NULL, 1);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (3531, 1, 5, 11, '2021-01-08 22:25:50', NULL, NULL, 1);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (3532, 1, 27, 11, '2021-01-08 22:25:50', NULL, NULL, 1);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (3533, 1, 28, 11, '2021-01-08 22:25:50', NULL, NULL, 1);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (3534, 1, 29, 11, '2021-01-08 22:25:50', NULL, NULL, 1);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (3535, 1, 30, 11, '2021-01-08 22:25:50', NULL, NULL, 1);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (3536, 1, 31, 11, '2021-01-08 22:25:50', NULL, NULL, 1);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (3537, 1, 32, 11, '2021-01-08 22:25:50', NULL, NULL, 1);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (3538, 1, 33, 11, '2021-01-08 22:25:50', NULL, NULL, 1);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (3539, 1, 6, 11, '2021-01-08 22:25:50', NULL, NULL, 1);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (3540, 1, 34, 11, '2021-01-08 22:25:50', NULL, NULL, 1);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (3541, 1, 35, 11, '2021-01-08 22:25:50', NULL, NULL, 1);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (3542, 1, 36, 11, '2021-01-08 22:25:50', NULL, NULL, 1);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (3543, 1, 37, 11, '2021-01-08 22:25:50', NULL, NULL, 1);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (3544, 1, 38, 11, '2021-01-08 22:25:50', NULL, NULL, 1);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (3545, 1, 39, 11, '2021-01-08 22:25:50', NULL, NULL, 1);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (3546, 1, 40, 11, '2021-01-08 22:25:50', NULL, NULL, 1);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (3547, 1, 52, 11, '2021-01-08 22:25:50', NULL, NULL, 1);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (3548, 1, 54, 11, '2021-01-08 22:25:50', NULL, NULL, 1);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (3549, 1, 55, 11, '2021-01-08 22:25:50', NULL, NULL, 1);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (3550, 1, 56, 11, '2021-01-08 22:25:50', NULL, NULL, 1);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (3551, 1, 57, 11, '2021-01-08 22:25:50', NULL, NULL, 1);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (3552, 1, 58, 11, '2021-01-08 22:25:50', NULL, NULL, 1);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (3553, 1, 7, 11, '2021-01-08 22:25:50', NULL, NULL, 1);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (3554, 1, 8, 11, '2021-01-08 22:25:50', NULL, NULL, 1);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (3555, 1, 41, 11, '2021-01-08 22:25:50', NULL, NULL, 1);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (3556, 1, 42, 11, '2021-01-08 22:25:50', NULL, NULL, 1);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (3557, 1, 43, 11, '2021-01-08 22:25:50', NULL, NULL, 1);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (3558, 1, 44, 11, '2021-01-08 22:25:50', NULL, NULL, 1);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (3559, 1, 45, 11, '2021-01-08 22:25:50', NULL, NULL, 1);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (3560, 1, 46, 11, '2021-01-08 22:25:50', NULL, NULL, 1);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (3561, 1, 47, 11, '2021-01-08 22:25:50', NULL, NULL, 1);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (3562, 1, 48, 11, '2021-01-08 22:25:50', NULL, NULL, 1);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (3563, 1, 59, 11, '2021-01-08 22:25:50', NULL, NULL, 1);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (3564, 1, 60, 11, '2021-01-08 22:25:50', NULL, NULL, 1);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (3565, 1, 61, 11, '2021-01-08 22:25:50', NULL, NULL, 1);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (3566, 8, 12, 1, '2021-01-08 22:27:42', NULL, NULL, 1);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (3567, 8, 13, 1, '2021-01-08 22:27:42', NULL, NULL, 1);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (3568, 8, 18, 1, '2021-01-08 22:27:42', NULL, NULL, 1);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (3569, 8, 26, 1, '2021-01-08 22:27:42', NULL, NULL, 1);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (3570, 8, 50, 1, '2021-01-08 22:27:42', NULL, NULL, 1);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (3571, 8, 33, 1, '2021-01-08 22:27:42', NULL, NULL, 1);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (3572, 8, 40, 1, '2021-01-08 22:27:42', NULL, NULL, 1);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (3573, 8, 57, 1, '2021-01-08 22:27:42', NULL, NULL, 1);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (3574, 8, 58, 1, '2021-01-08 22:27:42', NULL, NULL, 1);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (3575, 8, 48, 1, '2021-01-08 22:27:42', NULL, NULL, 1);
INSERT INTO `hero_auth`.`RolePermission`(`Id`, `RoleId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (3576, 8, 60, 1, '2021-01-08 22:27:42', NULL, NULL, 1);

INSERT INTO `hero_auth`.`Tenant`(`Id`, `Name`, `Memo`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (1, 'Hero社区', 'Hero权限管理系统社区', 1, 1, '2021-01-06 11:13:50', 1, '2021-01-07 13:29:55', 0, NULL, NULL);
INSERT INTO `hero_auth`.`Tenant`(`Id`, `Name`, `Memo`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (22, 'surging.cloud社区', 'surging.cloud社区租户', 1, 1, '2021-01-07 16:24:13', 1, '2021-01-07 21:23:54', 0, NULL, NULL);

INSERT INTO `hero_auth`.`UserGroup`(`Id`, `Name`, `IsAllOrg`, `Identification`, `Memo`, `Status`, `DataPermissionType`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`, `TenantId`) VALUES (20, '测试用户组12', b'0', 'usergroup12', '测试用', 1, 3, 1, '2020-12-27 22:13:38', 1, '2021-01-08 11:19:29', 0, NULL, NULL, 1);
INSERT INTO `hero_auth`.`UserGroup`(`Id`, `Name`, `IsAllOrg`, `Identification`, `Memo`, `Status`, `DataPermissionType`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`, `TenantId`) VALUES (21, '测试用户组2', b'0', 'testgroup2', NULL, 1, 9999, 1, '2020-12-28 19:44:52', 1, '2021-01-09 15:13:00', 0, NULL, NULL, 1);

INSERT INTO `hero_auth`.`UserGroupDataPermissionOrgRelation`(`Id`, `UserGroupId`, `OrgId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (75, 20, 2, 1, '2021-01-08 11:19:30', NULL, NULL, 1);
INSERT INTO `hero_auth`.`UserGroupDataPermissionOrgRelation`(`Id`, `UserGroupId`, `OrgId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (76, 20, 3, 1, '2021-01-08 11:19:30', NULL, NULL, 1);
INSERT INTO `hero_auth`.`UserGroupDataPermissionOrgRelation`(`Id`, `UserGroupId`, `OrgId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (77, 20, 7, 1, '2021-01-08 11:19:30', NULL, NULL, 1);
INSERT INTO `hero_auth`.`UserGroupDataPermissionOrgRelation`(`Id`, `UserGroupId`, `OrgId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (78, 20, 26, 1, '2021-01-08 11:19:30', NULL, NULL, 1);

INSERT INTO `hero_auth`.`UserGroupOrganization`(`Id`, `UserGroupId`, `OrgId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (24, 20, 8, 1, '2021-01-08 11:19:30', 1, '2021-01-08 11:19:30', 1);
INSERT INTO `hero_auth`.`UserGroupOrganization`(`Id`, `UserGroupId`, `OrgId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (25, 21, 2, 1, '2021-01-09 15:13:01', 1, '2021-01-09 15:13:01', 1);

INSERT INTO `hero_auth`.`UserGroupPermission`(`Id`, `UserGroupId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (448, 20, 12, 1, '2021-01-08 11:19:30', NULL, NULL, 1);
INSERT INTO `hero_auth`.`UserGroupPermission`(`Id`, `UserGroupId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (449, 20, 15, 1, '2021-01-08 11:19:30', NULL, NULL, 1);
INSERT INTO `hero_auth`.`UserGroupPermission`(`Id`, `UserGroupId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (450, 20, 16, 1, '2021-01-08 11:19:30', NULL, NULL, 1);
INSERT INTO `hero_auth`.`UserGroupPermission`(`Id`, `UserGroupId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (451, 20, 17, 1, '2021-01-08 11:19:30', NULL, NULL, 1);
INSERT INTO `hero_auth`.`UserGroupPermission`(`Id`, `UserGroupId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (452, 20, 18, 1, '2021-01-08 11:19:30', NULL, NULL, 1);
INSERT INTO `hero_auth`.`UserGroupPermission`(`Id`, `UserGroupId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (453, 20, 19, 1, '2021-01-08 11:19:30', NULL, NULL, 1);
INSERT INTO `hero_auth`.`UserGroupPermission`(`Id`, `UserGroupId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (454, 20, 20, 1, '2021-01-08 11:19:30', NULL, NULL, 1);
INSERT INTO `hero_auth`.`UserGroupPermission`(`Id`, `UserGroupId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (455, 20, 21, 1, '2021-01-08 11:19:30', NULL, NULL, 1);
INSERT INTO `hero_auth`.`UserGroupPermission`(`Id`, `UserGroupId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (456, 20, 22, 1, '2021-01-08 11:19:30', NULL, NULL, 1);
INSERT INTO `hero_auth`.`UserGroupPermission`(`Id`, `UserGroupId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (457, 20, 3, 1, '2021-01-08 11:19:30', NULL, NULL, 1);
INSERT INTO `hero_auth`.`UserGroupPermission`(`Id`, `UserGroupId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (458, 21, 9, 1, '2021-01-09 15:13:01', NULL, NULL, 1);
INSERT INTO `hero_auth`.`UserGroupPermission`(`Id`, `UserGroupId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (459, 21, 10, 1, '2021-01-09 15:13:01', NULL, NULL, 1);
INSERT INTO `hero_auth`.`UserGroupPermission`(`Id`, `UserGroupId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (460, 21, 11, 1, '2021-01-09 15:13:01', NULL, NULL, 1);
INSERT INTO `hero_auth`.`UserGroupPermission`(`Id`, `UserGroupId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (461, 21, 12, 1, '2021-01-09 15:13:01', NULL, NULL, 1);
INSERT INTO `hero_auth`.`UserGroupPermission`(`Id`, `UserGroupId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (462, 21, 13, 1, '2021-01-09 15:13:01', NULL, NULL, 1);
INSERT INTO `hero_auth`.`UserGroupPermission`(`Id`, `UserGroupId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (463, 21, 14, 1, '2021-01-09 15:13:01', NULL, NULL, 1);
INSERT INTO `hero_auth`.`UserGroupPermission`(`Id`, `UserGroupId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (464, 21, 15, 1, '2021-01-09 15:13:01', NULL, NULL, 1);
INSERT INTO `hero_auth`.`UserGroupPermission`(`Id`, `UserGroupId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (465, 21, 16, 1, '2021-01-09 15:13:01', NULL, NULL, 1);
INSERT INTO `hero_auth`.`UserGroupPermission`(`Id`, `UserGroupId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (466, 21, 17, 1, '2021-01-09 15:13:01', NULL, NULL, 1);
INSERT INTO `hero_auth`.`UserGroupPermission`(`Id`, `UserGroupId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (467, 21, 18, 1, '2021-01-09 15:13:01', NULL, NULL, 1);
INSERT INTO `hero_auth`.`UserGroupPermission`(`Id`, `UserGroupId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (468, 21, 19, 1, '2021-01-09 15:13:01', NULL, NULL, 1);
INSERT INTO `hero_auth`.`UserGroupPermission`(`Id`, `UserGroupId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (469, 21, 20, 1, '2021-01-09 15:13:01', NULL, NULL, 1);
INSERT INTO `hero_auth`.`UserGroupPermission`(`Id`, `UserGroupId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (470, 21, 21, 1, '2021-01-09 15:13:01', NULL, NULL, 1);
INSERT INTO `hero_auth`.`UserGroupPermission`(`Id`, `UserGroupId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (471, 21, 22, 1, '2021-01-09 15:13:01', NULL, NULL, 1);
INSERT INTO `hero_auth`.`UserGroupPermission`(`Id`, `UserGroupId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (472, 21, 23, 1, '2021-01-09 15:13:01', NULL, NULL, 1);
INSERT INTO `hero_auth`.`UserGroupPermission`(`Id`, `UserGroupId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (473, 21, 24, 1, '2021-01-09 15:13:01', NULL, NULL, 1);
INSERT INTO `hero_auth`.`UserGroupPermission`(`Id`, `UserGroupId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (474, 21, 25, 1, '2021-01-09 15:13:01', NULL, NULL, 1);
INSERT INTO `hero_auth`.`UserGroupPermission`(`Id`, `UserGroupId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (475, 21, 26, 1, '2021-01-09 15:13:01', NULL, NULL, 1);
INSERT INTO `hero_auth`.`UserGroupPermission`(`Id`, `UserGroupId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (476, 21, 27, 1, '2021-01-09 15:13:01', NULL, NULL, 1);
INSERT INTO `hero_auth`.`UserGroupPermission`(`Id`, `UserGroupId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (477, 21, 28, 1, '2021-01-09 15:13:01', NULL, NULL, 1);
INSERT INTO `hero_auth`.`UserGroupPermission`(`Id`, `UserGroupId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (478, 21, 29, 1, '2021-01-09 15:13:01', NULL, NULL, 1);
INSERT INTO `hero_auth`.`UserGroupPermission`(`Id`, `UserGroupId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (479, 21, 30, 1, '2021-01-09 15:13:01', NULL, NULL, 1);
INSERT INTO `hero_auth`.`UserGroupPermission`(`Id`, `UserGroupId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (480, 21, 31, 1, '2021-01-09 15:13:01', NULL, NULL, 1);
INSERT INTO `hero_auth`.`UserGroupPermission`(`Id`, `UserGroupId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (481, 21, 32, 1, '2021-01-09 15:13:01', NULL, NULL, 1);
INSERT INTO `hero_auth`.`UserGroupPermission`(`Id`, `UserGroupId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (482, 21, 33, 1, '2021-01-09 15:13:01', NULL, NULL, 1);
INSERT INTO `hero_auth`.`UserGroupPermission`(`Id`, `UserGroupId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (483, 21, 34, 1, '2021-01-09 15:13:01', NULL, NULL, 1);
INSERT INTO `hero_auth`.`UserGroupPermission`(`Id`, `UserGroupId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (484, 21, 35, 1, '2021-01-09 15:13:01', NULL, NULL, 1);
INSERT INTO `hero_auth`.`UserGroupPermission`(`Id`, `UserGroupId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (485, 21, 36, 1, '2021-01-09 15:13:01', NULL, NULL, 1);
INSERT INTO `hero_auth`.`UserGroupPermission`(`Id`, `UserGroupId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (486, 21, 37, 1, '2021-01-09 15:13:01', NULL, NULL, 1);
INSERT INTO `hero_auth`.`UserGroupPermission`(`Id`, `UserGroupId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (487, 21, 38, 1, '2021-01-09 15:13:01', NULL, NULL, 1);
INSERT INTO `hero_auth`.`UserGroupPermission`(`Id`, `UserGroupId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (488, 21, 39, 1, '2021-01-09 15:13:01', NULL, NULL, 1);
INSERT INTO `hero_auth`.`UserGroupPermission`(`Id`, `UserGroupId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (489, 21, 40, 1, '2021-01-09 15:13:01', NULL, NULL, 1);
INSERT INTO `hero_auth`.`UserGroupPermission`(`Id`, `UserGroupId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (490, 21, 49, 1, '2021-01-09 15:13:01', NULL, NULL, 1);
INSERT INTO `hero_auth`.`UserGroupPermission`(`Id`, `UserGroupId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (491, 21, 50, 1, '2021-01-09 15:13:01', NULL, NULL, 1);
INSERT INTO `hero_auth`.`UserGroupPermission`(`Id`, `UserGroupId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (492, 21, 51, 1, '2021-01-09 15:13:01', NULL, NULL, 1);
INSERT INTO `hero_auth`.`UserGroupPermission`(`Id`, `UserGroupId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (493, 21, 54, 1, '2021-01-09 15:13:01', NULL, NULL, 1);
INSERT INTO `hero_auth`.`UserGroupPermission`(`Id`, `UserGroupId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (494, 21, 55, 1, '2021-01-09 15:13:01', NULL, NULL, 1);
INSERT INTO `hero_auth`.`UserGroupPermission`(`Id`, `UserGroupId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (495, 21, 56, 1, '2021-01-09 15:13:01', NULL, NULL, 1);
INSERT INTO `hero_auth`.`UserGroupPermission`(`Id`, `UserGroupId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (496, 21, 57, 1, '2021-01-09 15:13:01', NULL, NULL, 1);
INSERT INTO `hero_auth`.`UserGroupPermission`(`Id`, `UserGroupId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (497, 21, 58, 1, '2021-01-09 15:13:01', NULL, NULL, 1);
INSERT INTO `hero_auth`.`UserGroupPermission`(`Id`, `UserGroupId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (498, 21, 1, 1, '2021-01-09 15:13:01', NULL, NULL, 1);
INSERT INTO `hero_auth`.`UserGroupPermission`(`Id`, `UserGroupId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (499, 21, 2, 1, '2021-01-09 15:13:01', NULL, NULL, 1);
INSERT INTO `hero_auth`.`UserGroupPermission`(`Id`, `UserGroupId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (500, 21, 3, 1, '2021-01-09 15:13:01', NULL, NULL, 1);
INSERT INTO `hero_auth`.`UserGroupPermission`(`Id`, `UserGroupId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (501, 21, 4, 1, '2021-01-09 15:13:01', NULL, NULL, 1);
INSERT INTO `hero_auth`.`UserGroupPermission`(`Id`, `UserGroupId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (502, 21, 5, 1, '2021-01-09 15:13:01', NULL, NULL, 1);
INSERT INTO `hero_auth`.`UserGroupPermission`(`Id`, `UserGroupId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (503, 21, 6, 1, '2021-01-09 15:13:01', NULL, NULL, 1);
INSERT INTO `hero_auth`.`UserGroupPermission`(`Id`, `UserGroupId`, `PermissionId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (504, 21, 52, 1, '2021-01-09 15:13:01', NULL, NULL, 1);

INSERT INTO `hero_auth`.`UserGroupRole`(`Id`, `UserGroupId`, `RoleId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (85, 20, 11, 1, '2021-01-08 11:19:29', 1, '2021-01-08 11:19:29', 1);
INSERT INTO `hero_auth`.`UserGroupRole`(`Id`, `UserGroupId`, `RoleId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (86, 20, 9, 1, '2021-01-08 11:19:29', 1, '2021-01-08 11:19:29', 1);
INSERT INTO `hero_auth`.`UserGroupRole`(`Id`, `UserGroupId`, `RoleId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (87, 21, 11, 1, '2021-01-09 15:13:00', 1, '2021-01-09 15:13:00', 1);
INSERT INTO `hero_auth`.`UserGroupRole`(`Id`, `UserGroupId`, `RoleId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (88, 21, 9, 1, '2021-01-09 15:13:01', 1, '2021-01-09 15:13:01', 1);

INSERT INTO `hero_auth`.`UserInfo`(`Id`, `UserName`, `OrgId`, `PositionId`, `Password`, `ChineseName`, `Email`, `Phone`, `Gender`, `Birth`, `NativePlace`, `Address`, `Folk`, `PoliticalStatus`, `GraduateInstitutions`, `Education`, `Major`, `Resume`, `Memo`, `LastLoginTime`, `LoginFailedCount`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`, `TenantId`) VALUES (1, 'admin', 3, 1, 'de4b550727f5b0ff46328be48c0765c3', '管理员', 'admin@liuhl-hero.com', '13122222222', 1, NULL, NULL, NULL, NULL, NULL, NULL, '1', NULL, '', NULL, NULL, 0, 1, 1, '2020-12-14 16:22:46', 1, '2021-01-09 15:13:31', 0, NULL, NULL, 1);
INSERT INTO `hero_auth`.`UserInfo`(`Id`, `UserName`, `OrgId`, `PositionId`, `Password`, `ChineseName`, `Email`, `Phone`, `Gender`, `Birth`, `NativePlace`, `Address`, `Folk`, `PoliticalStatus`, `GraduateInstitutions`, `Education`, `Major`, `Resume`, `Memo`, `LastLoginTime`, `LoginFailedCount`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`, `TenantId`) VALUES (10, 'zhangsan', 30, 25, 'fe86ad88645a1da4789cf32a4c373cd5', '张三', '13128729199@qq.com', '13128729199', 1, '2020-12-01', NULL, NULL, NULL, NULL, '云南大学', '3', '', '', NULL, NULL, 0, 1, 1, '2020-12-24 22:19:22', 1, '2020-12-25 10:55:38', 0, NULL, NULL, 1);
INSERT INTO `hero_auth`.`UserInfo`(`Id`, `UserName`, `OrgId`, `PositionId`, `Password`, `ChineseName`, `Email`, `Phone`, `Gender`, `Birth`, `NativePlace`, `Address`, `Folk`, `PoliticalStatus`, `GraduateInstitutions`, `Education`, `Major`, `Resume`, `Memo`, `LastLoginTime`, `LoginFailedCount`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`, `TenantId`) VALUES (11, 'zhaoyun', 26, 19, '1bc5b260cc438f77808664b164668d43', '赵云', '13128921112@qq.com', '13128921112', 1, NULL, NULL, NULL, NULL, NULL, '云南农业大学', '3', '', '', NULL, NULL, 0, 1, 10, '2020-12-24 22:21:40', 1, '2020-12-28 16:22:40', 0, NULL, NULL, 1);
INSERT INTO `hero_auth`.`UserInfo`(`Id`, `UserName`, `OrgId`, `PositionId`, `Password`, `ChineseName`, `Email`, `Phone`, `Gender`, `Birth`, `NativePlace`, `Address`, `Folk`, `PoliticalStatus`, `GraduateInstitutions`, `Education`, `Major`, `Resume`, `Memo`, `LastLoginTime`, `LoginFailedCount`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`, `TenantId`) VALUES (12, 'zhangfei', 3, 1, '4ca0399d8973bcc65867660e3ab51495', '张飞', '13128717111@qq.com', '13128717111', 1, '1995-12-04', NULL, NULL, NULL, NULL, '北京大学', NULL, NULL, '', NULL, NULL, 0, 1, 1, '2020-12-24 22:26:08', 14, '2021-01-02 00:28:27', 0, NULL, NULL, 1);
INSERT INTO `hero_auth`.`UserInfo`(`Id`, `UserName`, `OrgId`, `PositionId`, `Password`, `ChineseName`, `Email`, `Phone`, `Gender`, `Birth`, `NativePlace`, `Address`, `Folk`, `PoliticalStatus`, `GraduateInstitutions`, `Education`, `Major`, `Resume`, `Memo`, `LastLoginTime`, `LoginFailedCount`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`, `TenantId`) VALUES (13, 'guanyu', 8, 8, '1be1d2fe3a755499e4caecd1e00558ac', '关羽', '13128729333@qq.com', '13128729333', 1, '2020-12-01', NULL, NULL, NULL, NULL, '清华大学', NULL, '', '', NULL, NULL, 0, 1, 1, '2020-12-24 22:32:08', 11, '2021-01-08 23:57:15', 0, NULL, NULL, 1);
INSERT INTO `hero_auth`.`UserInfo`(`Id`, `UserName`, `OrgId`, `PositionId`, `Password`, `ChineseName`, `Email`, `Phone`, `Gender`, `Birth`, `NativePlace`, `Address`, `Folk`, `PoliticalStatus`, `GraduateInstitutions`, `Education`, `Major`, `Resume`, `Memo`, `LastLoginTime`, `LoginFailedCount`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`, `TenantId`) VALUES (14, 'liubei', 30, 24, '6af8ff795077ecfa8666800bb458d77a', '刘备', '15122221111@qq.com', '15122221111', 1, NULL, NULL, NULL, NULL, NULL, NULL, '1', NULL, '', NULL, NULL, 0, 1, 1, '2020-12-24 22:34:28', 1, '2021-01-06 15:58:08', 0, NULL, NULL, 1);
INSERT INTO `hero_auth`.`UserInfo`(`Id`, `UserName`, `OrgId`, `PositionId`, `Password`, `ChineseName`, `Email`, `Phone`, `Gender`, `Birth`, `NativePlace`, `Address`, `Folk`, `PoliticalStatus`, `GraduateInstitutions`, `Education`, `Major`, `Resume`, `Memo`, `LastLoginTime`, `LoginFailedCount`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`, `TenantId`) VALUES (15, 'wangzhaojun', 10, 22, '4de085ad6bbf84d7d1e87bdff7ad33e2', '王昭君', '13125544144@qq.com', '13125544144', 0, '1990-06-14', NULL, NULL, NULL, NULL, '浙江大学', NULL, NULL, '', NULL, NULL, 0, 1, 1, '2021-01-06 15:59:20', 1, '2021-01-09 00:00:50', 0, NULL, NULL, 1);
INSERT INTO `hero_auth`.`UserInfo`(`Id`, `UserName`, `OrgId`, `PositionId`, `Password`, `ChineseName`, `Email`, `Phone`, `Gender`, `Birth`, `NativePlace`, `Address`, `Folk`, `PoliticalStatus`, `GraduateInstitutions`, `Education`, `Major`, `Resume`, `Memo`, `LastLoginTime`, `LoginFailedCount`, `Status`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`, `TenantId`) VALUES (23, 'admin', 3, NULL, 'de4b550727f5b0ff46328be48c0765c3', '管理员', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '创建租户时,初始化的用户', NULL, 0, 1, 1, '2021-01-07 16:24:14', 1, '2021-01-07 16:24:14', 0, NULL, NULL, 22);

INSERT INTO `hero_auth`.`UserRole`(`Id`, `UserId`, `RoleId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (50, 10, 9, 1, '2020-12-25 10:55:39', 1, '2020-12-25 10:55:39', 1);
INSERT INTO `hero_auth`.`UserRole`(`Id`, `UserId`, `RoleId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (55, 11, 8, 1, '2020-12-28 16:22:40', 1, '2020-12-28 16:22:40', 1);
INSERT INTO `hero_auth`.`UserRole`(`Id`, `UserId`, `RoleId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (72, 12, 9, 1, '2020-12-30 22:16:33', 1, '2020-12-30 22:16:33', 1);
INSERT INTO `hero_auth`.`UserRole`(`Id`, `UserId`, `RoleId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (83, 14, 9, 1, '2021-01-06 15:57:47', 1, '2021-01-06 15:57:47', 1);
INSERT INTO `hero_auth`.`UserRole`(`Id`, `UserId`, `RoleId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (84, 14, 8, 1, '2021-01-06 15:57:48', 1, '2021-01-06 15:57:48', 1);
INSERT INTO `hero_auth`.`UserRole`(`Id`, `UserId`, `RoleId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (91, 23, 22, 1, '2021-01-07 16:24:14', 1, '2021-01-07 16:24:14', 22);
INSERT INTO `hero_auth`.`UserRole`(`Id`, `UserId`, `RoleId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (93, 1, 1, 1, '2021-01-08 11:06:40', 1, '2021-01-08 11:06:40', 1);
INSERT INTO `hero_auth`.`UserRole`(`Id`, `UserId`, `RoleId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (95, 13, 8, 13, '2021-01-08 22:29:03', 13, '2021-01-08 22:29:03', 1);
INSERT INTO `hero_auth`.`UserRole`(`Id`, `UserId`, `RoleId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (96, 15, 9, 1, '2021-01-09 00:00:51', 1, '2021-01-09 00:00:51', 1);

INSERT INTO `hero_auth`.`UserUserGroupRelation`(`Id`, `UserId`, `UserGroupId`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `TenantId`) VALUES (21, 13, 20, 13, '2021-01-08 22:29:03', 13, '2021-01-08 22:29:03', 1);

