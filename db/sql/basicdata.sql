/*==============================================================*/
/* DBMS name:      MySQL 5.0                                    */
/* Created on:     2021/1/6 17:45:36                            */
/*==============================================================*/


drop table if exists SystemConfig;

drop table if exists Wordbook;

drop table if exists WordbookItem;

/*==============================================================*/
/* Table: SystemConfig                                          */
/*==============================================================*/
create table SystemConfig
(
   Id                   bigint not null auto_increment comment '主键',
   SysName              varchar(50) not null comment '系统名称',
   DomainName           varchar(50) comment '域名',
   Administrator        varchar(50) comment '管理员',
   Logo                 varchar(50) comment 'login',
   LogoPosition         varchar(100) comment 'login存放位置',
   LogoSite             varchar(50) comment 'logosite',
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

alter table SystemConfig comment 'SystemConfig';

/*==============================================================*/
/* Table: Wordbook                                              */
/*==============================================================*/
create table Wordbook
(
   Id                   bigint not null auto_increment,
   Code                 varchar(50) not null comment '字典编码',
   Name                 varchar(50) not null comment '字典名称',
   Type                 int not null comment '1. 系统类型字典 2. 业务类型字典',
   Memo                 varchar(100) comment '备注',
   IsSysPreset          bit not null comment '是否系统预设',
   IsDeleted            int not null default 0 comment '软删除标识',
   DeleteBy             bigint comment '删除用户',
   DeleteTime           datetime comment '删除时间',
   CreateBy             bigint comment '创建人',
   CreateTime           date comment '创建时间',
   UpdateBy             bigint comment '更新人',
   UpdateTime           date comment '更新时间',
   TenantId             bigint comment '租户Id',
   primary key (Id)
);

alter table Wordbook comment '字典表';

/*==============================================================*/
/* Table: WordbookItem                                          */
/*==============================================================*/
create table WordbookItem
(
   Id                   bigint not null auto_increment comment '主键Id',
   WordBookId           bigint not null comment '字典Id',
   Key                  varchar(50) not null comment 'Key值',
   Value                varchar(50) not null comment 'Value值',
   Memo                 varchar(100) comment '备注',
   Sort                 int comment '排序',
   IsDeleted            int not null default 0 comment '软删除标识',
   DeleteBy             bigint comment '删除用户',
   DeleteTime           datetime comment '删除时间',
   CreateBy             bigint comment '创建人',
   CreateTime           date comment '创建时间',
   UpdateBy             bigint comment '更新人',
   UpdateTime           date comment '更新时间',
   TenantId             bigint comment '租户Id',
   primary key (Id)
);

alter table WordbookItem comment '字典项表';

