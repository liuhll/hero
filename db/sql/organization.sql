/*==============================================================*/
/* DBMS name:      MySQL 5.0                                    */
/* Created on:     2019/7/16 22:44:42                           */
/*==============================================================*/
SET NAMES utf8mb4;
SET FOREIGN_KEY_CHECKS = 0;
drop database if exists hero_organization;
create database hero_organization;
use hero_organization;
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
   Name                 varchar(50) not null comment '公司名称',
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
   Name                 varchar(50) not null comment '组织机构名称',
   Identification       varchar(50) not null comment '唯一标识',
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
   primary key (Id),
   key AK_Key_1 (Id)
);

alter table Position comment '职位表';


INSERT INTO `hero_organization`.`Corporation`(`Id`, `OrgId`, `Name`, `Mold`, `Address`, `Logo`, `LogoPosition`, `CorporateRepresentative`, `RegisterDate`, `OpenDate`, `Trade`, `Memo`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (1, 1, '1', 0, '中国北京通州', '', '', '', '2020-12-14 08:36:18', '2020-12-14 14:52:27', '', '1', 1, '2020-12-14 14:52:36', 1, '2020-12-14 14:52:55', 0, NULL, NULL);


INSERT INTO `hero_organization`.`Department`(`Id`, `OrgId`, `Location`, `DeptTypeKey`, `BriefIntro`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (1, 2, '朝阳区', 'Develop', NULL, NULL, '2020-12-14 16:15:41', 1, '2020-12-14 16:16:39', 0, NULL, NULL);
INSERT INTO `hero_organization`.`Department`(`Id`, `OrgId`, `Location`, `DeptTypeKey`, `BriefIntro`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (2, 3, 'A001区', 'Develop', '负责产品架构研发相关工作', 1, '2020-12-14 16:09:49', 1, '2020-12-14 16:09:49', 0, NULL, NULL);
INSERT INTO `hero_organization`.`Department`(`Id`, `OrgId`, `Location`, `DeptTypeKey`, `BriefIntro`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (6, 7, 'A002区', 'Develop', '负责产品功能实现，完成开发需求', 1, '2020-12-14 16:14:05', 1, '2020-12-14 16:14:05', 0, NULL, NULL);
INSERT INTO `hero_organization`.`Department`(`Id`, `OrgId`, `Location`, `DeptTypeKey`, `BriefIntro`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (7, 8, 'A002', 'Develop', '负责产品前端开发', 1, '2020-12-14 16:18:57', 1, '2020-12-14 16:18:57', 0, NULL, NULL);
INSERT INTO `hero_organization`.`Department`(`Id`, `OrgId`, `Location`, `DeptTypeKey`, `BriefIntro`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (8, 9, 'A002', 'Test', '负责产品测试', 1, '2020-12-14 16:20:11', 1, '2020-12-14 16:20:11', 0, NULL, NULL);
INSERT INTO `hero_organization`.`Department`(`Id`, `OrgId`, `Location`, `DeptTypeKey`, `BriefIntro`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (9, 10, 'A0003', 'Product', '负责产品设计，文档书写', 1, '2020-12-14 17:41:23', 1, '2020-12-14 17:41:41', 1, 1, '2020-12-14 17:41:41');


INSERT INTO `hero_organization`.`Organization`(`Id`, `ParentId`, `Name`, `Code`, `Level`, `OrgType`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (1, 0, 'Hero开发社区', '0001', 1, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL);
INSERT INTO `hero_organization`.`Organization`(`Id`, `ParentId`, `Name`, `Code`, `Level`, `OrgType`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (2, 1, '后端开发组', '0001.0001', 2, 1, NULL, '2020-12-14 16:15:41', 1, '2020-12-14 16:16:39', 0, NULL, NULL);
INSERT INTO `hero_organization`.`Organization`(`Id`, `ParentId`, `Name`, `Code`, `Level`, `OrgType`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (3, 2, '架构组', '0001.0001.0001', 3, 1, 1, '2020-12-14 16:09:49', 1, '2020-12-14 16:09:49', 0, NULL, NULL);
INSERT INTO `hero_organization`.`Organization`(`Id`, `ParentId`, `Name`, `Code`, `Level`, `OrgType`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (7, 2, '业务开发组', '0001.0001.0002', 3, 1, 1, '2020-12-14 16:14:05', 1, '2020-12-14 16:14:05', 0, NULL, NULL);
INSERT INTO `hero_organization`.`Organization`(`Id`, `ParentId`, `Name`, `Code`, `Level`, `OrgType`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (8, 1, '前端开发组', '0001.0002', 2, 1, 1, '2020-12-14 16:18:57', 1, '2020-12-14 16:18:57', 0, NULL, NULL);
INSERT INTO `hero_organization`.`Organization`(`Id`, `ParentId`, `Name`, `Code`, `Level`, `OrgType`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (9, 1, '测试组', '0001.0003', 2, 1, 1, '2020-12-14 16:20:11', 1, '2020-12-14 16:20:11', 0, NULL, NULL);
INSERT INTO `hero_organization`.`Organization`(`Id`, `ParentId`, `Name`, `Code`, `Level`, `OrgType`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (10, 1, '产品组', '0001.0004', 2, 1, 1, '2020-12-14 17:41:22', 1, '2020-12-14 17:41:22', 0, NULL, NULL);


INSERT INTO `hero_organization`.`Position`(`Id`, `DeptId`, `Code`, `Name`, `FunctionKey`, `PositionLevelKey`, `BriefIntro`, `Memo`, `PostResponsibility`, `IsLeadershipPost`, `IsLeadingOfficial`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (1, 2, '0001.0001.0001.0001', '架构师', 'TechnicalPost', 'Director ', NULL, NULL, '负责技术架构选型、架构研发相关工作', b'1', b'0', 1, '2020-12-14 16:09:49', 1, '2020-12-14 16:09:49', 0, NULL, NULL);
INSERT INTO `hero_organization`.`Position`(`Id`, `DeptId`, `Code`, `Name`, `FunctionKey`, `PositionLevelKey`, `BriefIntro`, `Memo`, `PostResponsibility`, `IsLeadershipPost`, `IsLeadingOfficial`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (5, 6, '0001.0001.0002.0001', '高级开发工程师', 'TechnicalPost', 'Commissioner', NULL, NULL, '负责API设计，业务功能开发，开发文档写作', b'0', b'0', 1, '2020-12-14 16:14:05', 1, '2020-12-14 16:14:05', 0, NULL, NULL);
INSERT INTO `hero_organization`.`Position`(`Id`, `DeptId`, `Code`, `Name`, `FunctionKey`, `PositionLevelKey`, `BriefIntro`, `Memo`, `PostResponsibility`, `IsLeadershipPost`, `IsLeadingOfficial`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (6, 6, '0001.0001.0002.0002', '中级开发工程师', 'TechnicalPost', 'Commissioner', NULL, NULL, '负责API设计，业务功能开发，开发文档写作', b'0', b'0', 1, '2020-12-14 16:14:05', 1, '2020-12-14 16:14:05', 0, NULL, NULL);
INSERT INTO `hero_organization`.`Position`(`Id`, `DeptId`, `Code`, `Name`, `FunctionKey`, `PositionLevelKey`, `BriefIntro`, `Memo`, `PostResponsibility`, `IsLeadershipPost`, `IsLeadingOfficial`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (7, 1, '0001.0001.0001', '后台开发主管', 'TechnicalPost', 'Director ', NULL, NULL, '负责管理后台开发任务与产品设计', b'1', b'0', 1, '2020-12-14 16:15:41', 1, '2020-12-14 16:16:39', 0, NULL, NULL);
INSERT INTO `hero_organization`.`Position`(`Id`, `DeptId`, `Code`, `Name`, `FunctionKey`, `PositionLevelKey`, `BriefIntro`, `Memo`, `PostResponsibility`, `IsLeadershipPost`, `IsLeadingOfficial`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (8, 7, '0001.0002.0001', '前端小组长', 'TechnicalPost', 'Director ', NULL, NULL, '负责主导前端产品开发、设计、技术选型', b'0', b'0', 1, '2020-12-14 16:18:57', 1, '2020-12-14 16:18:57', 0, NULL, NULL);
INSERT INTO `hero_organization`.`Position`(`Id`, `DeptId`, `Code`, `Name`, `FunctionKey`, `PositionLevelKey`, `BriefIntro`, `Memo`, `PostResponsibility`, `IsLeadershipPost`, `IsLeadingOfficial`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (9, 8, '0001.0003.0001', '测试小组长', 'TechnicalPost', 'Director ', NULL, NULL, '负责产品测试，提交测试报告', b'0', b'0', 1, '2020-12-14 16:20:11', 1, '2020-12-14 16:20:11', 0, NULL, NULL);
INSERT INTO `hero_organization`.`Position`(`Id`, `DeptId`, `Code`, `Name`, `FunctionKey`, `PositionLevelKey`, `BriefIntro`, `Memo`, `PostResponsibility`, `IsLeadershipPost`, `IsLeadingOfficial`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (10, 9, '0001.0004.0001', '产品经理', 'TechnicalPost', 'Director ', NULL, NULL, '负责产品的整理规划设计', b'1', b'1', 1, '2020-12-14 17:41:23', 1, '2020-12-14 17:41:41', 1, 1, '2020-12-14 17:41:41');
