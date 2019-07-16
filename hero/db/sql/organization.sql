/*==============================================================*/
/* DBMS name:      MySQL 5.0                                    */
/* Created on:     2019/7/16 22:44:42                           */
/*==============================================================*/

drop database if exists hero_organization;
create database hero_organization default character set utf8mb4 collate utf8mb4_general_ci;
use hero_organization;

drop table if exists Corporation;

drop table if exists Department;

drop table if exists DeptPosition;

drop table if exists DeptUser;

drop table if exists Position;

/*==============================================================*/
/* Table: Corporation                                           */
/*==============================================================*/
create table Corporation
(
   Id                   bigint not null auto_increment comment '主键',
   Code                 varchar(50) not null comment '唯一编码',
   Name                 varchar(50) not null comment '公司名称',
   ParentId             bigint comment '母公司Id',
   Type                 int comment '0.集团公司;1.单体公司',
   Mold                 int not null comment '0.母公司;2.子公司3.控股公司',
   Address              varchar(200) not null comment '公司地址',
   Logo                 varchar(50) comment 'logo名称',
   LogoPosition         varchar(50) comment 'logo存放位置',
   CorporateRepresentative varchar(50) comment '法人代表',
   RegisterDate         datetime comment '注册日期',
   OpenDate             datetime comment '开业时间',
   Trade                varchar(50) comment '所属行业',
   Memo                 varchar(500) comment '备注',
   CreateBy             bigint comment '创建人',
   CreateTime           datetime comment '创建日期',
   UpdateBy             bigint comment '修改人',
   UpdateTime           datetime comment '修改日期',
   IsDeleted            int comment '软删除标识',
   DeleteBy             bigint comment '删除用户',
   DeleteTime           datetime comment '删除时间',
   primary key (Id),
   key AK_Key_1 (Id)
);

alter table Corporation comment '公司信息表';

/*==============================================================*/
/* Table: Department                                            */
/*==============================================================*/
create table Department
(
   Id                   bigint not null auto_increment comment '主键',
   Code                 varchar(50) not null comment '唯一编码',
   Name                 varchar(50) not null comment '部门名称',
   ParentId             bigint not null comment '上级部门Id',
   CorporationId        bigint comment '所属公司',
   Location             varchar(100) comment '部门位置',
   DeptTypeId           bigint comment '部门类型,取自字典表',
   BriefIntro           varchar(100) comment '部门简介',
   Memo                 varchar(500) comment '备注',
   CreateBy             bigint comment '创建人',
   CreateTime           datetime comment '创建日期',
   UpdateBy             bigint comment '修改人',
   UpdateTime           datetime comment '修改日期',
   IsDeleted            int comment '软删除标识',
   DeleteBy             bigint comment '删除用户',
   DeleteTime           datetime comment '删除时间',
   primary key (Id),
   key AK_Key_1 (Id)
);

alter table Department comment '部门表';

/*==============================================================*/
/* Table: DeptPosition                                          */
/*==============================================================*/
create table DeptPosition
(
   Id                   bigint not null auto_increment comment '主键',
   DeptId               bigint not null comment '部门Id',
   PositionId           bigint not null comment '职位Id',
   CreateBy             bigint comment '创建人',
   CreateTime           datetime comment '创建日期',
   UpdateBy             bigint comment '修改人',
   UpdateTime           datetime comment '修改日期',
   primary key (Id),
   key AK_Key_1 (Id)
);

alter table DeptPosition comment '部门岗位关系表';

/*==============================================================*/
/* Table: DeptUser                                              */
/*==============================================================*/
create table DeptUser
(
   Id                   bigint not null auto_increment comment '主键',
   DeptId               bigint not null comment '部门Id',
   UserId               bigint not null comment '用户Id',
   PositionId           bigint not null comment '职位Id',
   DirectLeaderId       bigint comment '直属领导Id',
   CreateBy             bigint comment '创建人',
   CreateTime           datetime comment '创建日期',
   UpdateBy             bigint comment '修改人',
   UpdateTime           datetime comment '修改日期',
   primary key (Id),
   key AK_Key_1 (Id)
);

alter table DeptUser comment '部门用户关系表';

/*==============================================================*/
/* Table: Position                                              */
/*==============================================================*/
create table Position
(
   Id                   bigint not null auto_increment comment '主键',
   Code                 varchar(50) not null comment '唯一编码',
   ParentId             bigint comment '上级职位Id',
   Name                 varchar(50) not null comment '职位名称',
   FunctionId           bigint not null comment '职能Id,取自字典表',
   PositionTypeId       bigint not null comment '岗位类型Id,取自字典表',
   BriefIntro           varchar(100) comment '岗位说明',
   Memo                 varchar(500) comment '备注',
   CreateBy             bigint comment '创建人',
   CreateTime           datetime comment '创建日期',
   UpdateBy             bigint comment '修改人',
   UpdateTime           datetime comment '修改日期',
   IsDeleted            int comment '软删除标识',
   DeleteBy             bigint comment '删除用户',
   DeleteTime           datetime comment '删除时间',
   primary key (Id),
   key AK_Key_1 (Id)
);

alter table Position comment '职位表';

