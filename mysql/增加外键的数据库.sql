/*
Navicat MySQL Data Transfer

Source Server         : localhost_3306
Source Server Version : 50553
Source Host           : localhost:3306
Source Database       : mydatabase

Target Server Type    : MYSQL
Target Server Version : 50553
File Encoding         : 65001

Date: 2018-04-13 14:51:01
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for character
-- ----------------------------
DROP TABLE IF EXISTS `character`;
CREATE TABLE `character` (
  `Id` bigint(255) NOT NULL AUTO_INCREMENT,
  `Userid` bigint(255) DEFAULT NULL,
  `Name` varchar(255) DEFAULT NULL,
  `Level` int(11) DEFAULT '1',
  `Ap` int(11) DEFAULT '0',
  `Sp` int(11) DEFAULT '0',
  `World` int(11) DEFAULT '0',
  `Exp` int(11) DEFAULT '0',
  `Str` int(11) DEFAULT '0',
  `Dex` int(11) DEFAULT '0',
  `Luk` int(11) DEFAULT '0',
  `Int_` int(11) DEFAULT '0',
  `Hp` int(11) DEFAULT '50',
  `Mp` int(11) DEFAULT '50',
  `Maxhp` int(11) DEFAULT '50',
  `Maxmp` int(11) DEFAULT '50',
  `Job` int(11) DEFAULT '0',
  `Skin` int(11) DEFAULT '0',
  `Fame` int(11) DEFAULT '0',
  `Hair` int(11) DEFAULT '0',
  `Face` int(11) DEFAULT '0',
  `MapId` int(11) DEFAULT '0',
  `Gm` int(11) DEFAULT '0',
  `Party` int(11) DEFAULT '0',
  `Spawnpoint` int(11) DEFAULT '0',
  PRIMARY KEY (`Id`),
  KEY `Userid` (`Userid`),
  CONSTRAINT `character_ibfk_1` FOREIGN KEY (`Userid`) REFERENCES `users` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for inventoryequipment
-- ----------------------------
DROP TABLE IF EXISTS `inventoryequipment`;
CREATE TABLE `inventoryequipment` (
  `Id` bigint(255) NOT NULL AUTO_INCREMENT,
  `InventoryitemsId` bigint(255) DEFAULT NULL,
  `UpgradeSlots` int(11) DEFAULT NULL,
  `Level` int(11) DEFAULT NULL,
  `Str` int(11) DEFAULT NULL,
  `Dex` int(11) DEFAULT NULL,
  `Int` int(11) DEFAULT NULL,
  `Luk` int(11) DEFAULT NULL,
  `Hp` int(11) DEFAULT NULL,
  `Mp` int(11) DEFAULT NULL,
  `Watk` int(11) DEFAULT NULL,
  `Matk` int(11) DEFAULT NULL,
  `Wdef` int(11) DEFAULT NULL,
  `Mdef` int(11) DEFAULT NULL,
  `Acc` int(11) DEFAULT NULL,
  `Avoid` int(11) DEFAULT NULL,
  `Hands` int(11) DEFAULT NULL,
  `Speed` int(11) DEFAULT NULL,
  `Jump` int(11) DEFAULT NULL,
  `Owner` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `inventoryequipment_ibfk_1` (`InventoryitemsId`),
  CONSTRAINT `inventoryequipment_ibfk_1` FOREIGN KEY (`InventoryitemsId`) REFERENCES `inventoryitems` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for inventoryitems
-- ----------------------------
DROP TABLE IF EXISTS `inventoryitems`;
CREATE TABLE `inventoryitems` (
  `Id` bigint(255) NOT NULL AUTO_INCREMENT,
  `Type` tinyint(3) unsigned DEFAULT '0',
  `Cid` bigint(255) DEFAULT '0',
  `UserId` bigint(255) DEFAULT '0',
  `ItemId` int(11) DEFAULT '0',
  `InventoryType` int(11) DEFAULT '0',
  `Position` int(11) DEFAULT '0',
  `Quantity` int(11) DEFAULT '0',
  `Owner` tinytext,
  `Uniqueid` int(11) DEFAULT '-1',
  `Flag` int(2) DEFAULT '0',
  `Expiredate` bigint(20) DEFAULT '-1',
  `Sender` varchar(13) DEFAULT '',
  PRIMARY KEY (`Id`),
  KEY `FK_inventoryitems_1` (`Cid`) USING BTREE,
  KEY `FK_inventoryitems_2` (`UserId`),
  KEY `FK_inventoryitems_3` (`Uniqueid`) USING BTREE,
  CONSTRAINT `inventoryitems_ibfk_1` FOREIGN KEY (`Cid`) REFERENCES `character` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=gbk;

-- ----------------------------
-- Table structure for inventoryslot
-- ----------------------------
DROP TABLE IF EXISTS `inventoryslot`;
CREATE TABLE `inventoryslot` (
  `Id` bigint(255) NOT NULL AUTO_INCREMENT,
  `Chid` bigint(255) DEFAULT NULL COMMENT '玩家ID',
  `Equip` int(11) DEFAULT '32' COMMENT '装备',
  `Use` int(11) DEFAULT '32' COMMENT '消耗',
  `Setup` int(11) DEFAULT '32' COMMENT '设置',
  `Etc` int(11) DEFAULT '32' COMMENT '其他',
  `Cash` int(11) DEFAULT '32' COMMENT '现金',
  `Elab` int(11) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `Chid` (`Chid`),
  CONSTRAINT `inventoryslot_ibfk_1` FOREIGN KEY (`Chid`) REFERENCES `character` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for userinfo
-- ----------------------------
DROP TABLE IF EXISTS `userinfo`;
CREATE TABLE `userinfo` (
  `id` bigint(255) NOT NULL AUTO_INCREMENT,
  `accid` bigint(255) DEFAULT NULL COMMENT '玩家ID',
  `BirthTime` varchar(255) DEFAULT NULL COMMENT '出生时间',
  `HomePhone` varchar(255) DEFAULT NULL COMMENT '家庭号码',
  `Problem` varchar(255) DEFAULT NULL COMMENT '问题答案',
  `Email` varchar(255) DEFAULT NULL COMMENT '邮箱地址',
  `IDCard` varchar(255) DEFAULT NULL COMMENT '身份证',
  `PhoneId` varchar(11) DEFAULT NULL COMMENT '手机号码',
  `Name` varchar(255) DEFAULT NULL COMMENT '玩家姓名',
  PRIMARY KEY (`id`),
  KEY `accid` (`accid`),
  CONSTRAINT `userinfo_ibfk_1` FOREIGN KEY (`accid`) REFERENCES `users` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for users
-- ----------------------------
DROP TABLE IF EXISTS `users`;
CREATE TABLE `users` (
  `Id` bigint(255) NOT NULL AUTO_INCREMENT,
  `Name` varchar(255) DEFAULT NULL,
  `Passw` varchar(255) DEFAULT NULL,
  `Gender` bigint(1) DEFAULT '0',
  `Loggedin` int(11) DEFAULT NULL,
  `Gm` int(11) DEFAULT NULL,
  `LastLogin` datetime DEFAULT NULL,
  `Macs` varchar(255) DEFAULT NULL,
  `ACash` int(11) DEFAULT NULL,
  `Mpoints` int(11) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for users_copy
-- ----------------------------
DROP TABLE IF EXISTS `users_copy`;
CREATE TABLE `users_copy` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(255) DEFAULT NULL,
  `Passw` varchar(255) DEFAULT NULL,
  `Gender` int(11) DEFAULT NULL,
  `createdat` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `loggedin` int(11) DEFAULT NULL COMMENT '登录状态',
  `lastlogin` timestamp NULL DEFAULT NULL COMMENT '最后登陆时间',
  PRIMARY KEY (`Id`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8;
