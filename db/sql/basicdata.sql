/*==============================================================*/
/* DBMS name:      MySQL 5.0                                    */
/* Created on:     2019/7/16 22:44:10                           */
/*==============================================================*/
SET NAMES utf8mb4;
SET FOREIGN_KEY_CHECKS = 0;
drop database if exists hero_basicdata;
create database hero_basicdata;
use hero_basicdata;


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
   `Key`                  varchar(50) not null comment 'Key值',
   `Value`                varchar(50) not null comment 'Value值',
   Memo                 varchar(100) comment '备注',
   Sort                 int comment '排序',
   IsDeleted            int not null default 0 comment '软删除标识',
   DeleteBy             bigint comment '删除用户',
   DeleteTime           datetime comment '删除时间',
   CreateBy             bigint comment '创建人',
   CreateTime           date comment '创建时间',
   UpdateBy             bigint comment '更新人',
   UpdateTime           date comment '更新时间',
   primary key (Id)
);

alter table WordbookItem comment '字典项表';

INSERT INTO `hero_basicdata`.`Wordbook`(`Id`, `Code`, `Name`, `Type`, `Memo`, `IsSysPreset`, `IsDeleted`, `DeleteBy`, `DeleteTime`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (1, 'PositionFunction', '职能类型', 1, '标识岗位职能类型', b'1', 0, NULL, NULL, 0, '2019-08-10', NULL, NULL);
INSERT INTO `hero_basicdata`.`Wordbook`(`Id`, `Code`, `Name`, `Type`, `Memo`, `IsSysPreset`, `IsDeleted`, `DeleteBy`, `DeleteTime`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (2, 'PositionLevel', '岗位级别', 1, '标识岗位级别', b'1', 0, NULL, NULL, 0, '2019-08-10', NULL, NULL);
INSERT INTO `hero_basicdata`.`Wordbook`(`Id`, `Code`, `Name`, `Type`, `Memo`, `IsSysPreset`, `IsDeleted`, `DeleteBy`, `DeleteTime`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (3, 'DeptType', '部门类型', 1, '部门类型', b'1', 0, NULL, NULL, 0, '2019-08-11', NULL, NULL);

INSERT INTO `hero_basicdata`.`WordbookItem`(`Id`, `WordBookId`, `Key`, `Value`, `Memo`, `Sort`, `IsDeleted`, `DeleteBy`, `DeleteTime`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (1, 1, 'TechnicalPost', '专业技术岗位', '技术类岗位', 1, 0, NULL, NULL, NULL, '2019-08-10', NULL, '2019-07-28');
INSERT INTO `hero_basicdata`.`WordbookItem`(`Id`, `WordBookId`, `Key`, `Value`, `Memo`, `Sort`, `IsDeleted`, `DeleteBy`, `DeleteTime`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (2, 1, 'ManagementPost', '管理岗位', '管理类型岗位', 2, 0, NULL, NULL, NULL, '2019-08-10', NULL, NULL);
INSERT INTO `hero_basicdata`.`WordbookItem`(`Id`, `WordBookId`, `Key`, `Value`, `Memo`, `Sort`, `IsDeleted`, `DeleteBy`, `DeleteTime`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (3, 1, 'LogisticsPost', '后勤岗位', '后勤类岗位', 3, 0, NULL, NULL, NULL, '2019-08-10', NULL, NULL);
INSERT INTO `hero_basicdata`.`WordbookItem`(`Id`, `WordBookId`, `Key`, `Value`, `Memo`, `Sort`, `IsDeleted`, `DeleteBy`, `DeleteTime`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (4, 1, 'SalesPost', '销售类岗位', '销售类岗位', 4, 0, NULL, NULL, NULL, '2019-08-10', NULL, NULL);
INSERT INTO `hero_basicdata`.`WordbookItem`(`Id`, `WordBookId`, `Key`, `Value`, `Memo`, `Sort`, `IsDeleted`, `DeleteBy`, `DeleteTime`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (5, 1, 'AdministrativePost', '行政类岗位', '行政类岗位', 5, 0, NULL, NULL, NULL, '2019-08-10', NULL, NULL);
INSERT INTO `hero_basicdata`.`WordbookItem`(`Id`, `WordBookId`, `Key`, `Value`, `Memo`, `Sort`, `IsDeleted`, `DeleteBy`, `DeleteTime`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (6, 1, 'HrPost', '人力资源类岗位', '人力资源类岗位', 6, 0, NULL, NULL, NULL, '2019-08-10', NULL, NULL);
INSERT INTO `hero_basicdata`.`WordbookItem`(`Id`, `WordBookId`, `Key`, `Value`, `Memo`, `Sort`, `IsDeleted`, `DeleteBy`, `DeleteTime`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (7, 1, 'FinancialPost', '财务类型岗位', '财务类型岗位', 7, 0, NULL, NULL, NULL, '2019-08-10', NULL, NULL);
INSERT INTO `hero_basicdata`.`WordbookItem`(`Id`, `WordBookId`, `Key`, `Value`, `Memo`, `Sort`, `IsDeleted`, `DeleteBy`, `DeleteTime`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (8, 2, 'Assistant', '助理', '助理', 1, 0, NULL, NULL, NULL, '2019-08-10', NULL, NULL);
INSERT INTO `hero_basicdata`.`WordbookItem`(`Id`, `WordBookId`, `Key`, `Value`, `Memo`, `Sort`, `IsDeleted`, `DeleteBy`, `DeleteTime`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (9, 2, 'Commissioner', '专员', '专员', 2, 0, NULL, NULL, NULL, '2019-08-10', NULL, NULL);
INSERT INTO `hero_basicdata`.`WordbookItem`(`Id`, `WordBookId`, `Key`, `Value`, `Memo`, `Sort`, `IsDeleted`, `DeleteBy`, `DeleteTime`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (10, 2, 'Director ', '主管', '主管', 3, 0, NULL, NULL, NULL, '2019-08-10', NULL, NULL);
INSERT INTO `hero_basicdata`.`WordbookItem`(`Id`, `WordBookId`, `Key`, `Value`, `Memo`, `Sort`, `IsDeleted`, `DeleteBy`, `DeleteTime`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (11, 2, 'Manager', '经理', '经理', 4, 0, NULL, NULL, NULL, '2019-08-10', NULL, NULL);
INSERT INTO `hero_basicdata`.`WordbookItem`(`Id`, `WordBookId`, `Key`, `Value`, `Memo`, `Sort`, `IsDeleted`, `DeleteBy`, `DeleteTime`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (12, 2, 'Majordomo ', '总监', '总监', 5, 0, NULL, NULL, NULL, '2019-08-10', NULL, NULL);
INSERT INTO `hero_basicdata`.`WordbookItem`(`Id`, `WordBookId`, `Key`, `Value`, `Memo`, `Sort`, `IsDeleted`, `DeleteBy`, `DeleteTime`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (13, 2, 'GeneralManager', '总经理', '总经理', 6, 0, NULL, NULL, NULL, '2019-08-10', NULL, NULL);
INSERT INTO `hero_basicdata`.`WordbookItem`(`Id`, `WordBookId`, `Key`, `Value`, `Memo`, `Sort`, `IsDeleted`, `DeleteBy`, `DeleteTime`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (14, 2, 'Trustee ', '董事', '董事', 7, 0, NULL, NULL, NULL, '2019-08-10', NULL, NULL);
INSERT INTO `hero_basicdata`.`WordbookItem`(`Id`, `WordBookId`, `Key`, `Value`, `Memo`, `Sort`, `IsDeleted`, `DeleteBy`, `DeleteTime`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (15, 2, 'Chairman', '董事长', '董事长', 8, 0, NULL, NULL, NULL, '2019-08-10', NULL, NULL);
INSERT INTO `hero_basicdata`.`WordbookItem`(`Id`, `WordBookId`, `Key`, `Value`, `Memo`, `Sort`, `IsDeleted`, `DeleteBy`, `DeleteTime`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (16, 3, 'Develop', '技术', '负责技术开发', 1, 0, NULL, NULL, NULL, '2019-08-11', NULL, NULL);
INSERT INTO `hero_basicdata`.`WordbookItem`(`Id`, `WordBookId`, `Key`, `Value`, `Memo`, `Sort`, `IsDeleted`, `DeleteBy`, `DeleteTime`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (17, 3, 'Test', '测试', '负责产品测试', 2, 0, NULL, NULL, NULL, '2019-08-11', NULL, NULL);
INSERT INTO `hero_basicdata`.`WordbookItem`(`Id`, `WordBookId`, `Key`, `Value`, `Memo`, `Sort`, `IsDeleted`, `DeleteBy`, `DeleteTime`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (18, 3, 'Product', '产品', '负责产品规划', 3, 0, NULL, NULL, NULL, '2019-08-11', NULL, NULL);
INSERT INTO `hero_basicdata`.`WordbookItem`(`Id`, `WordBookId`, `Key`, `Value`, `Memo`, `Sort`, `IsDeleted`, `DeleteBy`, `DeleteTime`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (19, 3, 'Market', '市场', '负责产品市场推广', 4, 0, NULL, NULL, NULL, '2019-08-11', NULL, NULL);
INSERT INTO `hero_basicdata`.`WordbookItem`(`Id`, `WordBookId`, `Key`, `Value`, `Memo`, `Sort`, `IsDeleted`, `DeleteBy`, `DeleteTime`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`) VALUES (20, 3, 'Logistics', '后勤', '负责公司后勤管理', 5, 0, NULL, NULL, NULL, '2019-08-11', NULL, NULL);


