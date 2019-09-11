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

drop table if exists Position;


/*==============================================================*/
/* Table: Corporation                                           */
/*==============================================================*/
create table Corporation
(
   Id                   bigint not null auto_increment comment '主键',
   Code                 varchar(200) not null comment '唯一编码',
   Level                int,
   Name                 varchar(50) not null comment '公司名称',
   ParentId             bigint comment '母公司Id',
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
   Code                 varchar(200) not null comment '唯一编码',
   Name                 varchar(50) not null comment '部门名称',
   Level                int not null,
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
/* Table: Position                                              */
/*==============================================================*/
create table Position
(
   Id                   bigint not null auto_increment comment '主键',
   DeptId               bigint not null comment '所属部门',
   Code                 varchar(50) not null comment '唯一编码',
   Name                 varchar(50) not null comment '职位名称',
   FunctionId           bigint not null comment '职能Id,取自字典表',
   PositionLevelId      bigint not null comment '岗位级别Id,取自字典表',
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


INSERT INTO `hero_organization`.`Corporation`(`Id`, `Code`, `Level`, `Name`, `ParentId`, `Mold`, `Address`, `Logo`, `LogoPosition`, `CorporateRepresentative`, `RegisterDate`, `OpenDate`, `Trade`, `Memo`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (1, '1000', 1, 'surging-cloud', 0, 1, '北京朝阳区', '', '', '', '2019-09-11 13:03:14', '2019-09-11 13:03:14', '计算机行业', 'surging开发社区', NULL, '2019-09-11 13:13:44', NULL, NULL, 0, NULL, NULL);


INSERT INTO `hero_organization`.`Department`(`Id`, `Code`, `Name`, `Level`, `ParentId`, `CorporationId`, `Location`, `DeptTypeId`, `BriefIntro`, `Memo`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (1, '1000.0001', 'surging-cloud后端开发部', 1, 0, 1, '', 16, '', '', NULL, '2019-09-11 13:14:13', NULL, NULL, 0, NULL, NULL);
INSERT INTO `hero_organization`.`Department`(`Id`, `Code`, `Name`, `Level`, `ParentId`, `CorporationId`, `Location`, `DeptTypeId`, `BriefIntro`, `Memo`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (2, '1000.0002', 'surging-cloud前端开发部', 1, 0, 1, '', 16, '', '', NULL, '2019-09-11 13:15:07', NULL, NULL, 0, NULL, NULL);
INSERT INTO `hero_organization`.`Department`(`Id`, `Code`, `Name`, `Level`, `ParentId`, `CorporationId`, `Location`, `DeptTypeId`, `BriefIntro`, `Memo`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (3, '1000.0003', 'surging-cloud测试部', 1, 0, 1, '', 17, '', '', NULL, '2019-09-11 13:16:48', NULL, NULL, 0, NULL, NULL);


INSERT INTO `hero_organization`.`Position`(`Id`, `DeptId`, `Code`, `Name`, `FunctionId`, `PositionLevelId`, `BriefIntro`, `Memo`, `PostResponsibility`, `IsLeadershipPost`, `IsLeadingOfficial`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (1, 1, '1000.0001.0001', '技术经理', 1, 11, '部门经理', '', '负责主导后端产品开发', b'1', b'1', NULL, '2019-09-11 13:14:14', NULL, NULL, 0, NULL, NULL);
INSERT INTO `hero_organization`.`Position`(`Id`, `DeptId`, `Code`, `Name`, `FunctionId`, `PositionLevelId`, `BriefIntro`, `Memo`, `PostResponsibility`, `IsLeadershipPost`, `IsLeadingOfficial`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (2, 1, '1000.0001.0002', '高级开发工程师', 1, 11, '后端开发', '', 'surging.hero技术开发', b'0', b'0', NULL, '2019-09-11 13:14:14', NULL, NULL, 0, NULL, NULL);
INSERT INTO `hero_organization`.`Position`(`Id`, `DeptId`, `Code`, `Name`, `FunctionId`, `PositionLevelId`, `BriefIntro`, `Memo`, `PostResponsibility`, `IsLeadershipPost`, `IsLeadingOfficial`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (3, 1, '1000.0001.0003', '中级开发工程师', 1, 11, '后端开发', '', 'surging.hero技术开发', b'0', b'0', NULL, '2019-09-11 13:14:14', NULL, NULL, 0, NULL, NULL);
INSERT INTO `hero_organization`.`Position`(`Id`, `DeptId`, `Code`, `Name`, `FunctionId`, `PositionLevelId`, `BriefIntro`, `Memo`, `PostResponsibility`, `IsLeadershipPost`, `IsLeadingOfficial`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (4, 1, '1000.0001.0004', '初级开发工程师', 1, 11, '后端开发', '', 'surging.hero技术开发', b'0', b'0', NULL, '2019-09-11 13:14:14', NULL, NULL, 0, NULL, NULL);
INSERT INTO `hero_organization`.`Position`(`Id`, `DeptId`, `Code`, `Name`, `FunctionId`, `PositionLevelId`, `BriefIntro`, `Memo`, `PostResponsibility`, `IsLeadershipPost`, `IsLeadingOfficial`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (5, 2, '1000.0002.0001', '技术经理', 1, 11, '部门经理', '', '负责主导前端产品开发', b'1', b'1', NULL, '2019-09-11 13:15:07', NULL, NULL, 0, NULL, NULL);
INSERT INTO `hero_organization`.`Position`(`Id`, `DeptId`, `Code`, `Name`, `FunctionId`, `PositionLevelId`, `BriefIntro`, `Memo`, `PostResponsibility`, `IsLeadershipPost`, `IsLeadingOfficial`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (6, 2, '1000.0002.0002', '高级开发工程师', 1, 11, '后端开发', '', 'surging.hero技术开发', b'0', b'0', NULL, '2019-09-11 13:15:07', NULL, NULL, 0, NULL, NULL);
INSERT INTO `hero_organization`.`Position`(`Id`, `DeptId`, `Code`, `Name`, `FunctionId`, `PositionLevelId`, `BriefIntro`, `Memo`, `PostResponsibility`, `IsLeadershipPost`, `IsLeadingOfficial`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (7, 2, '1000.0002.0003', '中级开发工程师', 1, 11, '后端开发', '', 'surging.hero技术开发', b'0', b'0', NULL, '2019-09-11 13:15:07', NULL, NULL, 0, NULL, NULL);
INSERT INTO `hero_organization`.`Position`(`Id`, `DeptId`, `Code`, `Name`, `FunctionId`, `PositionLevelId`, `BriefIntro`, `Memo`, `PostResponsibility`, `IsLeadershipPost`, `IsLeadingOfficial`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (8, 2, '1000.0002.0004', '初级开发工程师', 1, 11, '后端开发', '', 'surging.hero技术开发', b'0', b'0', NULL, '2019-09-11 13:15:07', NULL, NULL, 0, NULL, NULL);
INSERT INTO `hero_organization`.`Position`(`Id`, `DeptId`, `Code`, `Name`, `FunctionId`, `PositionLevelId`, `BriefIntro`, `Memo`, `PostResponsibility`, `IsLeadershipPost`, `IsLeadingOfficial`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (9, 3, '1000.0003.0001', '测试经理', 1, 11, '部门经理', '', '负责主导产品测试相关工作', b'1', b'1', NULL, '2019-09-11 13:16:48', NULL, NULL, 0, NULL, NULL);
INSERT INTO `hero_organization`.`Position`(`Id`, `DeptId`, `Code`, `Name`, `FunctionId`, `PositionLevelId`, `BriefIntro`, `Memo`, `PostResponsibility`, `IsLeadershipPost`, `IsLeadingOfficial`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (10, 3, '1000.0003.0002', '高级测试工程师', 1, 11, '性能测试', '', 'surging.hero产品测试', b'0', b'0', NULL, '2019-09-11 13:16:48', NULL, NULL, 0, NULL, NULL);
INSERT INTO `hero_organization`.`Position`(`Id`, `DeptId`, `Code`, `Name`, `FunctionId`, `PositionLevelId`, `BriefIntro`, `Memo`, `PostResponsibility`, `IsLeadershipPost`, `IsLeadingOfficial`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (11, 3, '1000.0003.0003', '中级测试工程师', 1, 11, '产品测试', '', 'surging.hero产品测试', b'0', b'0', NULL, '2019-09-11 13:16:49', NULL, NULL, 0, NULL, NULL);
INSERT INTO `hero_organization`.`Position`(`Id`, `DeptId`, `Code`, `Name`, `FunctionId`, `PositionLevelId`, `BriefIntro`, `Memo`, `PostResponsibility`, `IsLeadershipPost`, `IsLeadingOfficial`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (12, 3, '1000.0003.0004', '初级测试工程师', 1, 11, '产品测试', '', 'surging.hero产品测试', b'0', b'0', NULL, '2019-09-11 13:16:49', NULL, NULL, 0, NULL, NULL);

