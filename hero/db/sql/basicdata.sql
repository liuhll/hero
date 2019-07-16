/*==============================================================*/
/* DBMS name:      MySQL 5.0                                    */
/* Created on:     2019/7/16 22:44:10                           */
/*==============================================================*/
drop database if exists hero_basicdata;
create database hero_basicdata default character set utf8mb4 collate utf8mb4_general_ci;
use hero_basicdata;

drop table if exists SystemConfig;

drop table if exists Wordbook;

/*==============================================================*/
/* Table: SystemConfig                                          */
/*==============================================================*/
create table SystemConfig
(
   Id                   bigint not null auto_increment comment '主键',
   ConfigName           varchar(50) not null comment '配置名称',
   ConfigCode           varchar(50) not null comment '配置值',
   ConfigValue          varchar(50) not null comment '配置值',
   Seq                  int not null comment '序号',
   IsSysPreSet          int not null comment '0. 否 1.是',
   CreateBy             bigint comment '创建人',
   CreateTime           datetime comment '创建日期',
   UpdateBy             bigint comment '修改人',
   UpdateTime           datetime comment '修改日期',
   IsDeleted            int comment '软删除标识',
   DeleteBy             bigint comment '删除用户',
   DeleteTime           datetime comment '删除时间',
   primary key (Id)
);

alter table SystemConfig comment 'SystemConfig';

/*==============================================================*/
/* Table: Wordbook                                              */
/*==============================================================*/
create table Wordbook
(
   Id                   bigint not null auto_increment comment '主键',
   Code                 varchar(50) not null comment '唯一编码',
   Name                 varchar(50) not null comment '字典名称',
   Value                varchar(50) not null comment '字典值',
   ParentId             bigint not null comment '父级Id',
   Seq                  int not null comment '序号',
   TypeName             varchar(50) comment '分类名称',
   HasChild             int not null comment '0.没有 1.有',
   IsSysPreSet          int not null comment '0. 否 1.是',
   CreateBy             bigint comment '创建人',
   CreateTime           datetime comment '创建日期',
   UpdateBy             bigint comment '修改人',
   UpdateTime           datetime comment '修改日期',
   IsDeleted            int comment '软删除标识',
   DeleteBy             bigint comment '删除用户',
   DeleteTime           datetime comment '删除时间',
   primary key (Id)
);

alter table Wordbook comment '字典表';

