/*==============================================================*/
/* DBMS name:      MySQL 5.0                                    */
/* Created on:     2021/1/6 17:46:01                            */
/*==============================================================*/


drop table if exists Corporation;

drop table if exists Department;

drop table if exists Organization;

drop table if exists Position;

/*==============================================================*/
/* Table: Corporation                                           */
/*==============================================================*/
create table Corporation
(
   Id                   bigint not null auto_increment comment '主键',
   OrgId                bigint not null comment '对应的组织机构Id',
   Mold                 int not null comment '0.集团公司;1.单体公司;2.子公司3.控股公司',
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
   TenantId             bigint comment '租户Id',
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
   OrgId                bigint not null comment '对应的组织机构Id',
   Location             varchar(100) comment '部门位置',
   DeptTypeKey          varchar(50) comment '部门类型,取自字典表',
   BriefIntro           varchar(100) comment '部门简介',
   CreateBy             bigint comment '创建人',
   CreateTime           datetime comment '创建日期',
   UpdateBy             bigint comment '修改人',
   UpdateTime           datetime comment '修改日期',
   IsDeleted            int comment '软删除标识',
   DeleteBy             bigint comment '删除用户',
   DeleteTime           datetime comment '删除时间',
   TenantId             bigint comment '租户Id',
   primary key (Id),
   key AK_Key_1 (Id)
);

alter table Department comment '部门表';

/*==============================================================*/
/* Table: Organization                                          */
/*==============================================================*/
create table Organization
(
   Id                   bigint not null auto_increment comment '主键',
   ParentId             bigint not null comment '父级组织机构Id',
   Name                 varchar(50) not null comment '公司名称',
   Code                 varchar(200) not null comment '唯一编码',
   Level                int comment '层级',
   OrgType              int not null default 1 comment '组织机构类型',
   CreateBy             bigint comment '创建人',
   CreateTime           datetime comment '创建日期',
   UpdateBy             bigint comment '修改人',
   UpdateTime           datetime comment '修改日期',
   IsDeleted            int comment '软删除标识',
   DeleteBy             bigint comment '删除用户',
   DeleteTime           datetime comment '删除时间',
   TenantId             bigint comment '租户Id',
   primary key (Id),
   key AK_Key_1 (Id)
);

alter table Organization comment '组织机构表';

/*==============================================================*/
/* Table: Position                                              */
/*==============================================================*/
create table Position
(
   Id                   bigint not null auto_increment comment '主键',
   DeptId               bigint not null comment '所属部门',
   Code                 varchar(50) not null comment '唯一编码',
   Name                 varchar(50) not null comment '职位名称',
   FunctionKey          varchar(50) comment '职能Key,取自字典表',
   PositionLevelKey     varchar(50) comment '岗位级别Key,取自字典表',
   BriefIntro           varchar(100) comment '岗位说明',
   Memo                 varchar(500) comment '备注',
   PostResponsibility   varchar(200) not null comment '岗位职责',
   IsLeadershipPost     bit not null comment '是否领导岗位',
   IsLeadingOfficial    bit not null comment '是否负责人岗位',
   CreateBy             bigint comment '创建人',
   CreateTime           datetime comment '创建日期',
   UpdateBy             bigint comment '修改人',
   UpdateTime           datetime comment '修改日期',
   IsDeleted            int comment '软删除标识',
   DeleteBy             bigint comment '删除用户',
   DeleteTime           datetime comment '删除时间',
   TenantId             bigint comment '租户Id',
   primary key (Id),
   key AK_Key_1 (Id)
);

alter table Position comment '职位表';

