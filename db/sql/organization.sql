/*==============================================================*/
/* DBMS name:      MySQL 5.0                                    */
/* Created on:     2021/1/7 9:31:41                             */
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
   Address              varchar(200) comment '公司地址',
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

INSERT INTO `hero_organization`.`Corporation`(`Id`, `OrgId`, `Mold`, `Address`, `Logo`, `LogoPosition`, `CorporateRepresentative`, `RegisterDate`, `OpenDate`, `Trade`, `Memo`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`, `TenantId`) VALUES (1, 1, 0, '中国北京通州', '', '', '', '2020-12-14 08:36:18', '2020-12-14 14:52:27', '', '1', 1, '2020-12-14 14:52:36', 1, '2021-01-09 00:03:20', 0, NULL, NULL, 1);
INSERT INTO `hero_organization`.`Corporation`(`Id`, `OrgId`, `Mold`, `Address`, `Logo`, `LogoPosition`, `CorporateRepresentative`, `RegisterDate`, `OpenDate`, `Trade`, `Memo`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`, `TenantId`) VALUES (8, 63, 0, NULL, NULL, NULL, NULL, '2021-01-06 23:30:24', '2021-01-06 23:30:24', NULL, NULL, 1, '2021-01-06 23:30:26', 1, '2021-01-06 23:30:26', 0, NULL, NULL, 10);
INSERT INTO `hero_organization`.`Corporation`(`Id`, `OrgId`, `Mold`, `Address`, `Logo`, `LogoPosition`, `CorporateRepresentative`, `RegisterDate`, `OpenDate`, `Trade`, `Memo`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`, `TenantId`) VALUES (12, 67, 0, NULL, NULL, NULL, NULL, '2021-01-07 14:19:08', '2021-01-07 14:19:08', NULL, NULL, 1, '2021-01-07 14:19:09', 1, '2021-01-07 14:19:09', 0, NULL, NULL, 18);
INSERT INTO `hero_organization`.`Corporation`(`Id`, `OrgId`, `Mold`, `Address`, `Logo`, `LogoPosition`, `CorporateRepresentative`, `RegisterDate`, `OpenDate`, `Trade`, `Memo`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`, `TenantId`) VALUES (13, 68, 0, NULL, NULL, NULL, NULL, '2021-01-07 14:31:54', '2021-01-07 14:31:54', NULL, NULL, 1, '2021-01-07 14:31:56', 1, '2021-01-07 14:31:56', 0, NULL, NULL, 19);
INSERT INTO `hero_organization`.`Corporation`(`Id`, `OrgId`, `Mold`, `Address`, `Logo`, `LogoPosition`, `CorporateRepresentative`, `RegisterDate`, `OpenDate`, `Trade`, `Memo`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`, `TenantId`) VALUES (14, 69, 0, NULL, NULL, NULL, NULL, '2021-01-07 14:57:51', '2021-01-07 14:57:51', NULL, NULL, 1, '2021-01-07 14:57:52', 1, '2021-01-07 14:57:52', 0, NULL, NULL, 20);
INSERT INTO `hero_organization`.`Corporation`(`Id`, `OrgId`, `Mold`, `Address`, `Logo`, `LogoPosition`, `CorporateRepresentative`, `RegisterDate`, `OpenDate`, `Trade`, `Memo`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`, `TenantId`) VALUES (15, 70, 0, NULL, NULL, NULL, NULL, '2021-01-07 15:04:02', '2021-01-07 15:04:02', NULL, NULL, 1, '2021-01-07 15:04:02', 1, '2021-01-07 15:04:02', 0, NULL, NULL, 21);
INSERT INTO `hero_organization`.`Corporation`(`Id`, `OrgId`, `Mold`, `Address`, `Logo`, `LogoPosition`, `CorporateRepresentative`, `RegisterDate`, `OpenDate`, `Trade`, `Memo`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`, `TenantId`) VALUES (16, 71, 0, NULL, NULL, NULL, NULL, '2021-01-07 16:24:14', '2021-01-07 16:24:14', NULL, NULL, 1, '2021-01-07 16:24:15', 1, '2021-01-07 16:24:15', 0, NULL, NULL, 22);

INSERT INTO `hero_organization`.`Department`(`Id`, `OrgId`, `Location`, `DeptTypeKey`, `BriefIntro`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`, `TenantId`) VALUES (1, 2, '朝阳区', 'Develop', '负责hero产品服务端研发相关工作', NULL, '2020-12-14 16:15:41', 1, '2021-01-09 00:02:58', 0, NULL, NULL, 1);
INSERT INTO `hero_organization`.`Department`(`Id`, `OrgId`, `Location`, `DeptTypeKey`, `BriefIntro`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`, `TenantId`) VALUES (2, 3, 'A001区', 'Develop', '负责产品架构研发相关工作', 1, '2020-12-14 16:09:49', 1, '2021-01-06 23:38:16', 0, NULL, NULL, 1);
INSERT INTO `hero_organization`.`Department`(`Id`, `OrgId`, `Location`, `DeptTypeKey`, `BriefIntro`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`, `TenantId`) VALUES (6, 7, 'A002区', 'Develop', '负责产品功能实现，完成开发需求', 1, '2020-12-14 16:14:05', 1, '2020-12-24 21:47:59', 0, NULL, NULL, 1);
INSERT INTO `hero_organization`.`Department`(`Id`, `OrgId`, `Location`, `DeptTypeKey`, `BriefIntro`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`, `TenantId`) VALUES (7, 8, 'A002', 'Develop', '负责产品前端开发', 1, '2020-12-14 16:18:57', 1, '2020-12-24 21:47:06', 0, NULL, NULL, 1);
INSERT INTO `hero_organization`.`Department`(`Id`, `OrgId`, `Location`, `DeptTypeKey`, `BriefIntro`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`, `TenantId`) VALUES (8, 9, 'A002', 'Test', '负责产品测试', 1, '2020-12-14 16:20:11', 1, '2020-12-24 22:05:55', 0, NULL, NULL, 1);
INSERT INTO `hero_organization`.`Department`(`Id`, `OrgId`, `Location`, `DeptTypeKey`, `BriefIntro`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`, `TenantId`) VALUES (9, 10, 'A0003', 'Product', '负责产品设计，文档书写', 1, '2020-12-14 17:41:23', 1, '2020-12-24 22:10:06', 0, NULL, NULL, 1);
INSERT INTO `hero_organization`.`Department`(`Id`, `OrgId`, `Location`, `DeptTypeKey`, `BriefIntro`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`, `TenantId`) VALUES (10, 26, 'A001', 'Develop', '负责产品前端研发工作', 1, '2020-12-24 21:55:28', 1, '2020-12-31 02:31:05', 0, NULL, NULL, 1);
INSERT INTO `hero_organization`.`Department`(`Id`, `OrgId`, `Location`, `DeptTypeKey`, `BriefIntro`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`, `TenantId`) VALUES (11, 27, 'A001', 'Develop', '负责产品移动APP研发工作', 1, '2020-12-24 21:56:56', 1, '2020-12-24 21:57:10', 0, NULL, NULL, 1);
INSERT INTO `hero_organization`.`Department`(`Id`, `OrgId`, `Location`, `DeptTypeKey`, `BriefIntro`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`, `TenantId`) VALUES (12, 28, 'A001', 'Develop', '负责ios APP研发工作', 1, '2020-12-24 21:58:04', 1, '2020-12-31 02:30:28', 0, NULL, NULL, 1);
INSERT INTO `hero_organization`.`Department`(`Id`, `OrgId`, `Location`, `DeptTypeKey`, `BriefIntro`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`, `TenantId`) VALUES (13, 29, 'A001', 'Develop', NULL, 1, '2020-12-24 21:58:45', 1, '2020-12-24 21:58:45', 0, NULL, NULL, 1);
INSERT INTO `hero_organization`.`Department`(`Id`, `OrgId`, `Location`, `DeptTypeKey`, `BriefIntro`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`, `TenantId`) VALUES (14, 30, 'A002', 'Logistics', '负责行政相关工作', 1, '2020-12-24 22:00:20', 1, '2020-12-24 22:11:40', 0, NULL, NULL, 1);

INSERT INTO `hero_organization`.`Organization`(`Id`, `ParentId`, `Name`, `Identification`, `Code`, `Level`, `OrgType`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`, `TenantId`) VALUES (1, 0, 'Hero开发社区', 'hero-web', '0001', 1, 0, NULL, '2021-01-09 00:03:20', 1, '2021-01-09 00:03:20', 0, NULL, NULL, 1);
INSERT INTO `hero_organization`.`Organization`(`Id`, `ParentId`, `Name`, `Identification`, `Code`, `Level`, `OrgType`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`, `TenantId`) VALUES (2, 1, '后端开发组', 'backendgroup', '0001.0001', 2, 1, NULL, '2020-12-14 16:15:41', 1, '2021-01-09 00:02:58', 0, NULL, NULL, 1);
INSERT INTO `hero_organization`.`Organization`(`Id`, `ParentId`, `Name`, `Identification`, `Code`, `Level`, `OrgType`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`, `TenantId`) VALUES (3, 2, '架构组', 'frameworkgroup', '0001.0001.0001', 3, 1, 1, '2020-12-14 16:09:49', 1, '2021-01-06 23:38:16', 0, NULL, NULL, 1);
INSERT INTO `hero_organization`.`Organization`(`Id`, `ParentId`, `Name`, `Identification`, `Code`, `Level`, `OrgType`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`, `TenantId`) VALUES (7, 2, '业务开发组', 'bussinessgroup', '0001.0001.0002', 3, 1, 1, '2020-12-14 16:14:05', 1, '2020-12-24 21:47:59', 0, NULL, NULL, 1);
INSERT INTO `hero_organization`.`Organization`(`Id`, `ParentId`, `Name`, `Identification`, `Code`, `Level`, `OrgType`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`, `TenantId`) VALUES (8, 1, '前端开发组', 'frontendgroup', '0001.0002', 2, 1, 1, '2020-12-14 16:18:57', 1, '2020-12-24 21:47:06', 0, NULL, NULL, 1);
INSERT INTO `hero_organization`.`Organization`(`Id`, `ParentId`, `Name`, `Identification`, `Code`, `Level`, `OrgType`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`, `TenantId`) VALUES (9, 1, '测试组', 'testgroup', '0001.0003', 2, 1, 1, '2020-12-14 16:20:11', 1, '2020-12-24 22:05:55', 0, NULL, NULL, 1);
INSERT INTO `hero_organization`.`Organization`(`Id`, `ParentId`, `Name`, `Identification`, `Code`, `Level`, `OrgType`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`, `TenantId`) VALUES (10, 1, '产品组', 'productgroup', '0001.0004', 2, 1, 1, '2020-12-14 17:41:22', 1, '2020-12-24 22:10:06', 0, NULL, NULL, 1);
INSERT INTO `hero_organization`.`Organization`(`Id`, `ParentId`, `Name`, `Identification`, `Code`, `Level`, `OrgType`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`, `TenantId`) VALUES (26, 8, 'web开发组', 'heroweb', '0001.0002.0001', 3, 1, 1, '2020-12-24 21:55:27', 1, '2020-12-31 02:31:05', 0, NULL, NULL, 1);
INSERT INTO `hero_organization`.`Organization`(`Id`, `ParentId`, `Name`, `Identification`, `Code`, `Level`, `OrgType`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`, `TenantId`) VALUES (27, 8, 'APP开发小组', 'heroapp', '0001.0002.0002', 3, 1, 1, '2020-12-24 21:56:56', 1, '2020-12-24 21:57:10', 0, NULL, NULL, 1);
INSERT INTO `hero_organization`.`Organization`(`Id`, `ParentId`, `Name`, `Identification`, `Code`, `Level`, `OrgType`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`, `TenantId`) VALUES (28, 27, 'IOS开发小组', 'heroappios', '0001.0002.0002.0001', 4, 1, 1, '2020-12-24 21:58:04', 1, '2020-12-31 02:30:28', 0, NULL, NULL, 1);
INSERT INTO `hero_organization`.`Organization`(`Id`, `ParentId`, `Name`, `Identification`, `Code`, `Level`, `OrgType`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`, `TenantId`) VALUES (29, 27, 'Android开发小组', 'heroappaddroid', '0001.0002.0002.0002', 4, 1, 1, '2020-12-24 21:58:45', 1, '2020-12-24 21:58:45', 0, NULL, NULL, 1);
INSERT INTO `hero_organization`.`Organization`(`Id`, `ParentId`, `Name`, `Identification`, `Code`, `Level`, `OrgType`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`, `TenantId`) VALUES (30, 1, '行政部', 'heroadministrative', '0001.0005', 2, 1, 1, '2020-12-24 22:00:20', 1, '2020-12-24 22:11:40', 0, NULL, NULL, 1);
INSERT INTO `hero_organization`.`Organization`(`Id`, `ParentId`, `Name`, `Identification`, `Code`, `Level`, `OrgType`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`, `TenantId`) VALUES (63, 0, 'surging.cloud社区', 'surging-cloud', '0002', 1, 0, 1, '2021-01-06 23:30:26', 1, '2021-01-06 23:30:26', 0, NULL, NULL, 10);
INSERT INTO `hero_organization`.`Organization`(`Id`, `ParentId`, `Name`, `Identification`, `Code`, `Level`, `OrgType`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`, `TenantId`) VALUES (67, 0, '测试租户1', 'test-tenant1', '0002', 1, 0, 1, '2021-01-07 14:19:09', 1, '2021-01-07 14:19:09', 0, NULL, NULL, 18);
INSERT INTO `hero_organization`.`Organization`(`Id`, `ParentId`, `Name`, `Identification`, `Code`, `Level`, `OrgType`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`, `TenantId`) VALUES (68, 0, '租户2', 'test-2', '0002', 1, 0, 1, '2021-01-07 14:31:56', 1, '2021-01-07 14:31:56', 0, NULL, NULL, 19);
INSERT INTO `hero_organization`.`Organization`(`Id`, `ParentId`, `Name`, `Identification`, `Code`, `Level`, `OrgType`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`, `TenantId`) VALUES (69, 0, '租户3', 'test3', '0002', 1, 0, 1, '2021-01-07 14:57:52', 1, '2021-01-07 14:57:52', 0, NULL, NULL, 20);
INSERT INTO `hero_organization`.`Organization`(`Id`, `ParentId`, `Name`, `Identification`, `Code`, `Level`, `OrgType`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`, `TenantId`) VALUES (70, 0, '租户4', 'test-tenant4', '0002', 1, 0, 1, '2021-01-07 15:04:02', 1, '2021-01-07 15:04:02', 0, NULL, NULL, 21);
INSERT INTO `hero_organization`.`Organization`(`Id`, `ParentId`, `Name`, `Identification`, `Code`, `Level`, `OrgType`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`, `TenantId`) VALUES (71, 0, 'surging.cloud社区', 'surging-cloud', '0002', 1, 0, 1, '2021-01-07 16:24:15', 1, '2021-01-07 16:24:15', 0, NULL, NULL, 22);

INSERT INTO `hero_organization`.`Position`(`Id`, `DeptId`, `Code`, `Name`, `FunctionKey`, `PositionLevelKey`, `BriefIntro`, `Memo`, `PostResponsibility`, `IsLeadershipPost`, `IsLeadingOfficial`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`, `TenantId`) VALUES (1, 2, '0001.0001.0001.0001', '高级架构师', 'TechnicalPost', 'Manager', NULL, NULL, '负责技术架构选型、架构研发相关工作', b'1', b'0', 1, '2020-12-14 16:09:49', 1, '2021-01-06 23:38:16', 0, NULL, NULL, 1);
INSERT INTO `hero_organization`.`Position`(`Id`, `DeptId`, `Code`, `Name`, `FunctionKey`, `PositionLevelKey`, `BriefIntro`, `Memo`, `PostResponsibility`, `IsLeadershipPost`, `IsLeadingOfficial`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`, `TenantId`) VALUES (5, 6, '0001.0001.0002.0001', '高级开发工程师', 'TechnicalPost', 'Commissioner', NULL, NULL, '负责API设计，业务功能开发，开发文档写作', b'0', b'0', 1, '2020-12-14 16:14:05', 1, '2020-12-24 21:47:59', 0, NULL, NULL, 1);
INSERT INTO `hero_organization`.`Position`(`Id`, `DeptId`, `Code`, `Name`, `FunctionKey`, `PositionLevelKey`, `BriefIntro`, `Memo`, `PostResponsibility`, `IsLeadershipPost`, `IsLeadingOfficial`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`, `TenantId`) VALUES (6, 6, '0001.0001.0002.0002', '中级开发工程师', 'TechnicalPost', 'Commissioner', NULL, NULL, '负责API设计，业务功能开发，开发文档写作', b'0', b'0', 1, '2020-12-14 16:14:05', 1, '2020-12-24 21:47:59', 0, NULL, NULL, 1);
INSERT INTO `hero_organization`.`Position`(`Id`, `DeptId`, `Code`, `Name`, `FunctionKey`, `PositionLevelKey`, `BriefIntro`, `Memo`, `PostResponsibility`, `IsLeadershipPost`, `IsLeadingOfficial`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`, `TenantId`) VALUES (7, 1, '0001.0001.0001', '后台开发主管', 'TechnicalPost', 'Manager', NULL, NULL, '负责管理后台开发任务与产品设计', b'1', b'0', 1, '2020-12-14 16:15:41', 1, '2021-01-09 00:02:58', 0, NULL, NULL, 1);
INSERT INTO `hero_organization`.`Position`(`Id`, `DeptId`, `Code`, `Name`, `FunctionKey`, `PositionLevelKey`, `BriefIntro`, `Memo`, `PostResponsibility`, `IsLeadershipPost`, `IsLeadingOfficial`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`, `TenantId`) VALUES (8, 7, '0001.0002.0001', '前端主管', 'TechnicalPost', 'Director ', NULL, NULL, '负责主导前端产品开发、设计、技术选型', b'0', b'0', 1, '2020-12-14 16:18:57', 1, '2020-12-24 21:47:06', 0, NULL, NULL, 1);
INSERT INTO `hero_organization`.`Position`(`Id`, `DeptId`, `Code`, `Name`, `FunctionKey`, `PositionLevelKey`, `BriefIntro`, `Memo`, `PostResponsibility`, `IsLeadershipPost`, `IsLeadingOfficial`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`, `TenantId`) VALUES (9, 8, '0001.0003.0001', '测试主管', 'TechnicalPost', 'Director ', NULL, NULL, '负责产品测试，提交测试报告', b'0', b'0', 1, '2020-12-14 16:20:11', 1, '2020-12-24 22:05:55', 0, NULL, NULL, 1);
INSERT INTO `hero_organization`.`Position`(`Id`, `DeptId`, `Code`, `Name`, `FunctionKey`, `PositionLevelKey`, `BriefIntro`, `Memo`, `PostResponsibility`, `IsLeadershipPost`, `IsLeadingOfficial`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`, `TenantId`) VALUES (11, 1, '0001.0001.0002', '高级开发工程师', 'TechnicalPost', 'Commissioner', NULL, NULL, '负责产品设计、API文档编写、编码实现等相关工作', b'0', b'0', 1, '2020-12-24 21:44:24', 1, '2021-01-09 00:02:58', 0, NULL, NULL, 1);
INSERT INTO `hero_organization`.`Position`(`Id`, `DeptId`, `Code`, `Name`, `FunctionKey`, `PositionLevelKey`, `BriefIntro`, `Memo`, `PostResponsibility`, `IsLeadershipPost`, `IsLeadingOfficial`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`, `TenantId`) VALUES (12, 1, '0001.0001.0003', '中级后端开发工程师', 'TechnicalPost', 'Commissioner', NULL, NULL, '负责产品设计、API文档编写、编码实现等相关工作', b'0', b'0', 1, '2020-12-24 21:44:24', 1, '2021-01-09 00:02:58', 0, NULL, NULL, 1);
INSERT INTO `hero_organization`.`Position`(`Id`, `DeptId`, `Code`, `Name`, `FunctionKey`, `PositionLevelKey`, `BriefIntro`, `Memo`, `PostResponsibility`, `IsLeadershipPost`, `IsLeadingOfficial`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`, `TenantId`) VALUES (13, 1, '0001.0001.0004', '初级后端开发工程师', 'TechnicalPost', 'Commissioner', NULL, NULL, '负责产品设计、API文档编写、编码实现等相关工作', b'0', b'0', 1, '2020-12-24 21:44:25', 1, '2021-01-09 00:02:58', 0, NULL, NULL, 1);
INSERT INTO `hero_organization`.`Position`(`Id`, `DeptId`, `Code`, `Name`, `FunctionKey`, `PositionLevelKey`, `BriefIntro`, `Memo`, `PostResponsibility`, `IsLeadershipPost`, `IsLeadingOfficial`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`, `TenantId`) VALUES (14, 2, '0001.0001.0001.0002', '初级架构师', 'TechnicalPost', 'Commissioner', NULL, NULL, '协助高级架构师完成架构工作', b'0', b'0', 1, '2020-12-24 21:45:47', 1, '2021-01-06 23:38:16', 0, NULL, NULL, 1);
INSERT INTO `hero_organization`.`Position`(`Id`, `DeptId`, `Code`, `Name`, `FunctionKey`, `PositionLevelKey`, `BriefIntro`, `Memo`, `PostResponsibility`, `IsLeadershipPost`, `IsLeadingOfficial`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`, `TenantId`) VALUES (15, 6, '0001.0001.0002.0003', '初级开发工程师', 'TechnicalPost', 'Commissioner', NULL, NULL, '负责产品业务研发相关工作', b'0', b'0', 1, '2020-12-24 21:47:45', 1, '2020-12-24 21:47:59', 0, NULL, NULL, 1);
INSERT INTO `hero_organization`.`Position`(`Id`, `DeptId`, `Code`, `Name`, `FunctionKey`, `PositionLevelKey`, `BriefIntro`, `Memo`, `PostResponsibility`, `IsLeadershipPost`, `IsLeadingOfficial`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`, `TenantId`) VALUES (16, 8, '0001.0003.0002', '高级测试工程师', 'TechnicalPost', 'Director ', NULL, NULL, '负责产品功能测试、性能测试', b'0', b'0', 1, '2020-12-24 22:02:08', 1, '2020-12-24 22:05:55', 0, NULL, NULL, 1);
INSERT INTO `hero_organization`.`Position`(`Id`, `DeptId`, `Code`, `Name`, `FunctionKey`, `PositionLevelKey`, `BriefIntro`, `Memo`, `PostResponsibility`, `IsLeadershipPost`, `IsLeadingOfficial`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`, `TenantId`) VALUES (17, 8, '0001.0003.0003', '中级测试工程师', 'TechnicalPost', 'Commissioner', NULL, NULL, '负责产品功能测试', b'0', b'0', 1, '2020-12-24 22:03:26', 1, '2020-12-24 22:05:55', 0, NULL, NULL, 1);
INSERT INTO `hero_organization`.`Position`(`Id`, `DeptId`, `Code`, `Name`, `FunctionKey`, `PositionLevelKey`, `BriefIntro`, `Memo`, `PostResponsibility`, `IsLeadershipPost`, `IsLeadingOfficial`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`, `TenantId`) VALUES (18, 8, '0001.0003.0004', '初级测试工程师', 'TechnicalPost', 'Commissioner', NULL, NULL, '负责产品功能测试', b'0', b'0', 1, '2020-12-24 22:03:26', 1, '2020-12-24 22:05:55', 0, NULL, NULL, 1);
INSERT INTO `hero_organization`.`Position`(`Id`, `DeptId`, `Code`, `Name`, `FunctionKey`, `PositionLevelKey`, `BriefIntro`, `Memo`, `PostResponsibility`, `IsLeadershipPost`, `IsLeadingOfficial`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`, `TenantId`) VALUES (19, 10, '0001.0002.0001.0001', '前端开发工程师', 'TechnicalPost', 'Commissioner', NULL, NULL, '负责产品前端开发工程师', b'0', b'0', 1, '2020-12-24 22:07:34', 1, '2020-12-31 02:31:05', 0, NULL, NULL, 1);
INSERT INTO `hero_organization`.`Position`(`Id`, `DeptId`, `Code`, `Name`, `FunctionKey`, `PositionLevelKey`, `BriefIntro`, `Memo`, `PostResponsibility`, `IsLeadershipPost`, `IsLeadingOfficial`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`, `TenantId`) VALUES (20, 10, '0001.0002.0001.0002', '中级前端开发工程师', 'TechnicalPost', 'Commissioner', NULL, NULL, '负责产品前端开发工程师\n', b'0', b'0', 1, '2020-12-24 22:08:48', 1, '2020-12-31 02:31:05', 0, NULL, NULL, 1);
INSERT INTO `hero_organization`.`Position`(`Id`, `DeptId`, `Code`, `Name`, `FunctionKey`, `PositionLevelKey`, `BriefIntro`, `Memo`, `PostResponsibility`, `IsLeadershipPost`, `IsLeadingOfficial`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`, `TenantId`) VALUES (21, 10, '0001.0002.0001.0003', '初级前端开发工程师', 'TechnicalPost', 'Commissioner', NULL, NULL, '负责产品前端开发工程师\n', b'0', b'0', 1, '2020-12-24 22:08:49', 1, '2020-12-31 02:31:05', 0, NULL, NULL, 1);
INSERT INTO `hero_organization`.`Position`(`Id`, `DeptId`, `Code`, `Name`, `FunctionKey`, `PositionLevelKey`, `BriefIntro`, `Memo`, `PostResponsibility`, `IsLeadershipPost`, `IsLeadingOfficial`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`, `TenantId`) VALUES (22, 9, '0001.0004.0001', '产品主管', 'TechnicalPost', 'Director ', NULL, NULL, '负责产品设计', b'0', b'0', 1, '2020-12-24 22:09:26', 1, '2020-12-24 22:10:06', 0, NULL, NULL, 1);
INSERT INTO `hero_organization`.`Position`(`Id`, `DeptId`, `Code`, `Name`, `FunctionKey`, `PositionLevelKey`, `BriefIntro`, `Memo`, `PostResponsibility`, `IsLeadershipPost`, `IsLeadingOfficial`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`, `TenantId`) VALUES (23, 9, '0001.0004.0002', '产品助理', 'TechnicalPost', 'Commissioner', NULL, NULL, '负责产品设计、文档编写', b'0', b'0', 1, '2020-12-24 22:10:06', 1, '2020-12-24 22:10:06', 0, NULL, NULL, 1);
INSERT INTO `hero_organization`.`Position`(`Id`, `DeptId`, `Code`, `Name`, `FunctionKey`, `PositionLevelKey`, `BriefIntro`, `Memo`, `PostResponsibility`, `IsLeadershipPost`, `IsLeadingOfficial`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`, `TenantId`) VALUES (24, 14, '0001.0005.0001', '行政经理', 'LogisticsPost', 'Assistant', NULL, NULL, '行政部门管理相关工作', b'0', b'0', 1, '2020-12-24 22:10:55', 1, '2020-12-24 22:11:41', 0, NULL, NULL, 1);
INSERT INTO `hero_organization`.`Position`(`Id`, `DeptId`, `Code`, `Name`, `FunctionKey`, `PositionLevelKey`, `BriefIntro`, `Memo`, `PostResponsibility`, `IsLeadershipPost`, `IsLeadingOfficial`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`, `TenantId`) VALUES (25, 14, '0001.0005.0002', '行政助理', 'LogisticsPost', 'Commissioner', NULL, NULL, '行政服务相关工作', b'0', b'0', 1, '2020-12-24 22:11:41', 1, '2020-12-24 22:11:41', 0, NULL, NULL, 1);