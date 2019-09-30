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
