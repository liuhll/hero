/*==============================================================*/
/* DBMS name:      MySQL 5.0                                    */
/* Created on:     2019/7/16 22:43:28                           */
/*==============================================================*/
drop database if exists hero_auth;
create database hero_auth default character set utf8mb4 collate utf8mb4_general_ci;
use hero_auth;

drop table if exists Action;

drop table if exists File;

drop table if exists Menu;

drop table if exists Operation;

drop table if exists Permission;

drop table if exists Role;

drop table if exists RolePermission;

drop table if exists UserInfo;

drop table if exists UserRole;

drop table if exists UserUsergroupRelation;

drop table if exists Usergroup;

drop table if exists UsergroupRole;

/*==============================================================*/
/* Table: Action                                                */
/*==============================================================*/
create table Action
(
   Id                   bigint not null auto_increment comment '主键',
   Name                 varchar(50) not null comment '名称',
   WebApi               varchar(50) not null comment 'webapi',
   Method               int not null comment '请求方法',
   DisableNetwork       bit comment '是否禁用外网',
   EnableAuthorization  bit comment '是否需要认证',
   AllowPermission      bit comment '是否需要鉴权',
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
   Name                 varchar(50) not null comment '菜单名称',
   Anchor               varchar(50) not null comment '前端菜单页面锚点',
   Level                int not null comment '层级',
   Mold                 int not null comment '菜单类型',
   Icon                 varchar(100) comment 'icon图标',
   FrontEndComponent    varchar(100) comment '前端组件',
   Sort                 int comment '排序',
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
   Icon                 varchar(50) comment '图标',
   Mold                 int not null comment '操作类型:1.增2.删3.改4.查5.其他操作',
   Sort                 int comment '排序',
   Status               int not null comment '状态',
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
   Name                 varchar(50) not null comment '角色名称',
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

alter table Role comment '角色表';

/*==============================================================*/
/* Table: RolePermission                                        */
/*==============================================================*/
create table RolePermission
(
   Id                   bigint not null auto_increment comment '主键',
   RoleId               bigint not null,
   PerssionId           char(10) not null,
   CreateBy             bigint comment '创建人',
   CreateTime           datetime comment '创建日期',
   UpdateBy             bigint comment '修改人',
   UpdateTime           datetime comment '修改日期',
   primary key (Id)
);

alter table RolePermission comment '角色权限表';

/*==============================================================*/
/* Table: UserInfo                                              */
/*==============================================================*/
create table UserInfo
(
   Id                   bigint not null auto_increment comment '主键',
   UserName             varchar(50) not null comment '用户名',
   Password             varchar(100) not null comment '密码',
   ChineseName          varchar(50) not null comment '中文名',
   Email                varchar(50) not null comment '电子邮件',
   Phone                varchar(22) not null comment '联系电话',
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
   primary key (Id)
);

alter table UserRole comment '用户角色关系表';

/*==============================================================*/
/* Table: UserUsergroupRelation                                 */
/*==============================================================*/
create table UserUsergroupRelation
(
   Id                   bigint not null auto_increment comment '主键',
   UserId               bigint not null,
   UserGroupId          bigint not null,
   CreateBy             bigint comment '创建人',
   CreateTime           datetime comment '创建日期',
   UpdateBy             bigint comment '修改人',
   UpdateTime           datetime comment '修改日期',
   primary key (Id)
);

alter table UserUsergroupRelation comment '用户与用户关系表';

/*==============================================================*/
/* Table: Usergroup                                             */
/*==============================================================*/
create table Usergroup
(
   Id                   bigint not null auto_increment comment '主键',
   ParentId             bigint not null comment '父用户组Id',
   GroupCode            varchar(50),
   GroupName            varchar(50) not null comment '用户组名称',
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

alter table Usergroup comment '用户组表';

/*==============================================================*/
/* Table: UsergroupRole                                         */
/*==============================================================*/
create table UsergroupRole
(
   Id                   bigint not null auto_increment comment '主键',
   UserGroupId          bigint not null,
   RoleId               bigint not null,
   CreateBy             bigint comment '创建人',
   CreateTime           datetime comment '创建日期',
   UpdateBy             bigint comment '修改人',
   UpdateTime           datetime comment '修改日期',
   primary key (Id)
);

alter table UsergroupRole comment '用户组角色关系表';

