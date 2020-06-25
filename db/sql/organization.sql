/*==============================================================*/
/* DBMS name:      MySQL 5.0                                    */
/* Created on:     2019/7/16 22:44:42                           */
/*==============================================================*/
SET NAMES utf8mb4;
SET FOREIGN_KEY_CHECKS = 0;
drop database if exists hero_organization;
create database hero_organization;
use hero_organization;

drop table if exists Organization;

drop table if exists Corporation;

drop table if exists Department;

drop table if exists Position;

-- ----------------------------
-- Table structure for Corporation
-- ----------------------------
DROP TABLE IF EXISTS `Corporation`;
CREATE TABLE `Corporation`  (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '主键',
  `OrgId` bigint(20) NOT NULL COMMENT '组织机构Id',
  `Mold` int(11) NOT NULL COMMENT '0.集团公司;1.单体公司;2.子公司3.控股公司',
  `Address` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '公司地址',
  `Logo` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT 'logo名称',
  `LogoPosition` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT 'logo存放位置',
  `CorporateRepresentative` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '法人代表',
  `RegisterDate` datetime(0) NULL DEFAULT NULL COMMENT '注册日期',
  `OpenDate` datetime(0) NULL DEFAULT NULL COMMENT '开业时间',
  `Trade` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '所属行业',
  `Memo` varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '备注',
  `CreateBy` bigint(20) NULL DEFAULT NULL COMMENT '创建人',
  `CreateTime` datetime(0) NULL DEFAULT NULL COMMENT '创建日期',
  `UpdateBy` bigint(20) NULL DEFAULT NULL COMMENT '修改人',
  `UpdateTime` datetime(0) NULL DEFAULT NULL COMMENT '修改日期',
  `IsDeleted` int(11) NULL DEFAULT NULL COMMENT '软删除标识',
  `DeleteBy` bigint(20) NULL DEFAULT NULL COMMENT '删除用户',
  `DeleteTime` datetime(0) NULL DEFAULT NULL COMMENT '删除时间',
  PRIMARY KEY (`Id`) USING BTREE,
  INDEX `AK_Key_1`(`Id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 2 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci COMMENT = '公司信息表' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for Department
-- ----------------------------
DROP TABLE IF EXISTS `Department`;
CREATE TABLE `Department`  (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '主键',
  `OrgId` bigint(20) NOT NULL COMMENT '组织机构Id',
  `Location` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '部门位置',
  `DeptTypeId` bigint(20) NULL DEFAULT NULL COMMENT '部门类型,取自字典表',
  `BriefIntro` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '部门简介',
  `Memo` varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '备注',
  `CreateBy` bigint(20) NULL DEFAULT NULL COMMENT '创建人',
  `CreateTime` datetime(0) NULL DEFAULT NULL COMMENT '创建日期',
  `UpdateBy` bigint(20) NULL DEFAULT NULL COMMENT '修改人',
  `UpdateTime` datetime(0) NULL DEFAULT NULL COMMENT '修改日期',
  `IsDeleted` int(11) NULL DEFAULT NULL COMMENT '软删除标识',
  `DeleteBy` bigint(20) NULL DEFAULT NULL COMMENT '删除用户',
  `DeleteTime` datetime(0) NULL DEFAULT NULL COMMENT '删除时间',
  PRIMARY KEY (`Id`) USING BTREE,
  INDEX `AK_Key_1`(`Id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 4 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci COMMENT = '部门表' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for Organization
-- ----------------------------
DROP TABLE IF EXISTS `Organization`;
CREATE TABLE `Organization`  (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT,
  `ParentId` bigint(20) NULL DEFAULT NULL,
  `Code` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Name` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  `Level` int(10) NOT NULL,
  `OrgType` int(255) NULL DEFAULT NULL,
  `CreateBy` bigint(20) NULL DEFAULT NULL COMMENT '创建人',
  `CreateTime` datetime(0) NULL DEFAULT NULL COMMENT '创建日期',
  `UpdateBy` bigint(20) NULL DEFAULT NULL COMMENT '修改人',
  `UpdateTime` datetime(0) NULL DEFAULT NULL COMMENT '修改日期',
  `IsDeleted` int(11) NULL DEFAULT NULL COMMENT '软删除标识',
  `DeleteBy` bigint(20) NULL DEFAULT NULL COMMENT '删除用户',
  `DeleteTime` datetime(0) NULL DEFAULT NULL COMMENT '删除时间',
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 4 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci COMMENT = '组织机构信息表' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for Position
-- ----------------------------
DROP TABLE IF EXISTS `Position`;
CREATE TABLE `Position`  (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '主键',
  `DeptId` bigint(20) NOT NULL COMMENT '所属部门',
  `Code` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '唯一编码',
  `Name` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '职位名称',
  `FunctionId` bigint(20) NOT NULL COMMENT '职能Id,取自字典表',
  `PositionLevelId` bigint(20) NOT NULL COMMENT '岗位级别Id,取自字典表',
  `BriefIntro` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '岗位说明',
  `Memo` varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '备注',
  `PostResponsibility` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '岗位职责',
  `IsLeadershipPost` bit(1) NOT NULL COMMENT '是否领导岗位',
  `IsLeadingOfficial` bit(1) NOT NULL COMMENT '是否负责人岗位',
  `CreateBy` bigint(20) NULL DEFAULT NULL COMMENT '创建人',
  `CreateTime` datetime(0) NULL DEFAULT NULL COMMENT '创建日期',
  `UpdateBy` bigint(20) NULL DEFAULT NULL COMMENT '修改人',
  `UpdateTime` datetime(0) NULL DEFAULT NULL COMMENT '修改日期',
  `IsDeleted` int(11) NULL DEFAULT NULL COMMENT '软删除标识',
  `DeleteBy` bigint(20) NULL DEFAULT NULL COMMENT '删除用户',
  `DeleteTime` datetime(0) NULL DEFAULT NULL COMMENT '删除时间',
  PRIMARY KEY (`Id`) USING BTREE,
  INDEX `AK_Key_1`(`Id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 13 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci COMMENT = '职位表' ROW_FORMAT = Dynamic;

SET FOREIGN_KEY_CHECKS = 1;

INSERT INTO `hero_organization`.`Corporation`(`Id`, `OrgId`, `Mold`, `Address`, `Logo`, `LogoPosition`, `CorporateRepresentative`, `RegisterDate`, `OpenDate`, `Trade`, `Memo`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (2, 4, 1, '中国北京通州', '', '', '', '2020-06-25 08:36:18', '2020-06-25 08:36:18', '', '', 1, '2020-06-25 16:37:18', 1, '2020-06-25 17:15:57', 0, NULL, NULL);

INSERT INTO `hero_organization`.`Department`(`Id`, `OrgId`, `Location`, `DeptTypeId`, `BriefIntro`, `Memo`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (4, 5, 'A001区', 16, '负责hero产品后端开发', NULL, 1, '2020-06-25 16:43:24', 1, '2020-06-25 17:03:55', 0, NULL, NULL);
INSERT INTO `hero_organization`.`Department`(`Id`, `OrgId`, `Location`, `DeptTypeId`, `BriefIntro`, `Memo`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (5, 6, 'A002', 16, '负责hero产品前端开发', NULL, 1, '2020-06-25 17:01:17', 1, '2020-06-25 17:01:17', 0, NULL, NULL);
INSERT INTO `hero_organization`.`Department`(`Id`, `OrgId`, `Location`, `DeptTypeId`, `BriefIntro`, `Memo`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (6, 7, 'A003', 17, '负责产品测试，保证产品质量', NULL, 1, '2020-06-25 17:11:32', 1, '2020-06-25 17:11:32', 0, NULL, NULL);
INSERT INTO `hero_organization`.`Department`(`Id`, `OrgId`, `Location`, `DeptTypeId`, `BriefIntro`, `Memo`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (7, 8, 'A003', 16, '负责产品设计需求管理', NULL, 1, '2020-06-25 17:15:23', 1, '2020-06-25 17:15:23', 0, NULL, NULL);

INSERT INTO `hero_organization`.`Organization`(`Id`, `ParentId`, `Code`, `Name`, `Level`, `OrgType`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (4, 0, '0001', 'hero开发社区', 1, 0, 1, '2020-06-25 16:37:18', 1, '2020-06-25 16:37:18', 0, NULL, NULL);
INSERT INTO `hero_organization`.`Organization`(`Id`, `ParentId`, `Code`, `Name`, `Level`, `OrgType`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (5, 4, '0001.0001', '后端开发组', 2, 1, 1, '2020-06-25 16:43:24', 1, '2020-06-25 17:03:55', 0, NULL, NULL);
INSERT INTO `hero_organization`.`Organization`(`Id`, `ParentId`, `Code`, `Name`, `Level`, `OrgType`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (6, 4, '0001.0002', '前端开发组', 2, 1, 1, '2020-06-25 17:01:17', 1, '2020-06-25 17:01:17', 0, NULL, NULL);
INSERT INTO `hero_organization`.`Organization`(`Id`, `ParentId`, `Code`, `Name`, `Level`, `OrgType`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (7, 4, '0001.0003', '测试组', 2, 1, 1, '2020-06-25 17:11:32', 1, '2020-06-25 17:11:32', 0, NULL, NULL);


INSERT INTO `hero_organization`.`Position`(`Id`, `DeptId`, `Code`, `Name`, `FunctionId`, `PositionLevelId`, `BriefIntro`, `Memo`, `PostResponsibility`, `IsLeadershipPost`, `IsLeadingOfficial`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (13, 4, '0001.0001.0001', '后端开发组长', 1, 10, NULL, NULL, '负责产品后台开发任务管理与分配、数据库设计、产品技术选型', b'1', b'1', 1, '2020-06-25 16:43:24', 1, '2020-06-25 17:03:55', 0, NULL, NULL);
INSERT INTO `hero_organization`.`Position`(`Id`, `DeptId`, `Code`, `Name`, `FunctionId`, `PositionLevelId`, `BriefIntro`, `Memo`, `PostResponsibility`, `IsLeadershipPost`, `IsLeadingOfficial`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (14, 5, '0001.0002.0001', '前端技术经理', 1, 10, NULL, NULL, '负责hero产品前端开发任务管理与技术选型,产品进度监控', b'1', b'1', 1, '2020-06-25 17:01:17', 1, '2020-06-25 17:01:17', 0, NULL, NULL);
INSERT INTO `hero_organization`.`Position`(`Id`, `DeptId`, `Code`, `Name`, `FunctionId`, `PositionLevelId`, `BriefIntro`, `Memo`, `PostResponsibility`, `IsLeadershipPost`, `IsLeadingOfficial`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (15, 5, '0001.0002.0002', '高级前端开发工程师', 1, 9, NULL, NULL, '负责前端技术开发, 协助技术经理进行技术选型', b'0', b'0', 1, '2020-06-25 17:01:17', 1, '2020-06-25 17:01:17', 0, NULL, NULL);
INSERT INTO `hero_organization`.`Position`(`Id`, `DeptId`, `Code`, `Name`, `FunctionId`, `PositionLevelId`, `BriefIntro`, `Memo`, `PostResponsibility`, `IsLeadershipPost`, `IsLeadingOfficial`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (16, 5, '0001.0002.0003', '中级前端开发工程师', 1, 9, NULL, NULL, '负责前端产品开发', b'0', b'0', 1, '2020-06-25 17:01:17', 1, '2020-06-25 17:01:17', 0, NULL, NULL);
INSERT INTO `hero_organization`.`Position`(`Id`, `DeptId`, `Code`, `Name`, `FunctionId`, `PositionLevelId`, `BriefIntro`, `Memo`, `PostResponsibility`, `IsLeadershipPost`, `IsLeadingOfficial`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (17, 5, '0001.0002.0004', '初级前端开发工程师', 1, 9, NULL, NULL, '负责前端开发任务', b'0', b'0', 1, '2020-06-25 17:01:17', 1, '2020-06-25 17:01:17', 0, NULL, NULL);
INSERT INTO `hero_organization`.`Position`(`Id`, `DeptId`, `Code`, `Name`, `FunctionId`, `PositionLevelId`, `BriefIntro`, `Memo`, `PostResponsibility`, `IsLeadershipPost`, `IsLeadingOfficial`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (18, 4, '0001.0001.0002', '高级后端开发工程师', 1, 9, NULL, NULL, '负责产品后台开发开发', b'0', b'0', 1, '2020-06-25 17:03:55', 1, '2020-06-25 17:03:55', 0, NULL, NULL);
INSERT INTO `hero_organization`.`Position`(`Id`, `DeptId`, `Code`, `Name`, `FunctionId`, `PositionLevelId`, `BriefIntro`, `Memo`, `PostResponsibility`, `IsLeadershipPost`, `IsLeadingOfficial`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (19, 4, '0001.0001.0003', '中级后端开发工程师', 1, 9, NULL, NULL, '负责产品后端API接口开发', b'0', b'0', 1, '2020-06-25 17:03:55', 1, '2020-06-25 17:03:55', 0, NULL, NULL);
INSERT INTO `hero_organization`.`Position`(`Id`, `DeptId`, `Code`, `Name`, `FunctionId`, `PositionLevelId`, `BriefIntro`, `Memo`, `PostResponsibility`, `IsLeadershipPost`, `IsLeadingOfficial`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (20, 4, '0001.0001.0004', '初级后台开发工程师', 1, 9, NULL, NULL, '负责产品后台API开发', b'0', b'0', 1, '2020-06-25 17:03:55', 1, '2020-06-25 17:03:55', 0, NULL, NULL);
INSERT INTO `hero_organization`.`Position`(`Id`, `DeptId`, `Code`, `Name`, `FunctionId`, `PositionLevelId`, `BriefIntro`, `Memo`, `PostResponsibility`, `IsLeadershipPost`, `IsLeadingOfficial`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (21, 6, '0001.0003.0001', '测试经理', 1, 10, NULL, NULL, '负责产品测试流程设计与测试任务管理管理、分配，保证产品质量', b'1', b'1', 1, '2020-06-25 17:11:32', 1, '2020-06-25 17:11:32', 0, NULL, NULL);
INSERT INTO `hero_organization`.`Position`(`Id`, `DeptId`, `Code`, `Name`, `FunctionId`, `PositionLevelId`, `BriefIntro`, `Memo`, `PostResponsibility`, `IsLeadershipPost`, `IsLeadingOfficial`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (22, 6, '0001.0003.0002', '高级测试工程师', 1, 9, NULL, NULL, '负责产品测试', b'0', b'0', 1, '2020-06-25 17:11:32', 1, '2020-06-25 17:11:32', 0, NULL, NULL);
INSERT INTO `hero_organization`.`Position`(`Id`, `DeptId`, `Code`, `Name`, `FunctionId`, `PositionLevelId`, `BriefIntro`, `Memo`, `PostResponsibility`, `IsLeadershipPost`, `IsLeadingOfficial`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (23, 6, '0001.0003.0003', '中级测试工程师', 1, 9, NULL, NULL, '负责产品测试', b'0', b'0', 1, '2020-06-25 17:11:32', 1, '2020-06-25 17:11:32', 0, NULL, NULL);
INSERT INTO `hero_organization`.`Position`(`Id`, `DeptId`, `Code`, `Name`, `FunctionId`, `PositionLevelId`, `BriefIntro`, `Memo`, `PostResponsibility`, `IsLeadershipPost`, `IsLeadingOfficial`, `CreateBy`, `CreateTime`, `UpdateBy`, `UpdateTime`, `IsDeleted`, `DeleteBy`, `DeleteTime`) VALUES (24, 6, '0001.0003.0004', '初级测试测试', 1, 8, NULL, NULL, '负责产品测试', b'0', b'0', 1, '2020-06-25 17:11:32', 1, '2020-06-25 17:11:32', 0, NULL, NULL);
