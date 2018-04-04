/*
Navicat MySQL Data Transfer

Source Server         : localhost_3306
Source Server Version : 50553
Source Host           : localhost:3306
Source Database       : mydatabase

Target Server Type    : MYSQL
Target Server Version : 50553
File Encoding         : 65001

Date: 2018-04-03 00:35:12
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for character
-- ----------------------------
DROP TABLE IF EXISTS `character`;
CREATE TABLE `character` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Userid` int(11) DEFAULT NULL,
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
  PRIMARY KEY (`Id`)
) ENGINE=MyISAM AUTO_INCREMENT=24 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for inventoryitems
-- ----------------------------
DROP TABLE IF EXISTS `inventoryitems`;
CREATE TABLE `inventoryitems` (
  `Id` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `Type` tinyint(3) unsigned DEFAULT '0',
  `Cid` int(11) DEFAULT '0',
  `UserId` int(11) DEFAULT '0',
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
  KEY `FK_inventoryitems_3` (`Uniqueid`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=20930 DEFAULT CHARSET=gbk;

-- ----------------------------
-- Table structure for inventoryslot
-- ----------------------------
DROP TABLE IF EXISTS `inventoryslot`;
CREATE TABLE `inventoryslot` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Chid` int(11) DEFAULT NULL COMMENT '玩家ID',
  `Equip` int(11) DEFAULT '32' COMMENT '装备',
  `Use` int(11) DEFAULT '32' COMMENT '消耗',
  `Setup` int(11) DEFAULT '32' COMMENT '设置',
  `Etc` int(11) DEFAULT '32' COMMENT '其他',
  `Cash` int(11) DEFAULT '32' COMMENT '现金',
  `Elab` int(11) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=MyISAM AUTO_INCREMENT=20 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for userinfo
-- ----------------------------
DROP TABLE IF EXISTS `userinfo`;
CREATE TABLE `userinfo` (
  `id` bigint(21) NOT NULL AUTO_INCREMENT,
  `accid` bigint(20) DEFAULT NULL COMMENT '玩家ID',
  `BirthTime` varchar(255) DEFAULT NULL COMMENT '出生时间',
  `HomePhone` varchar(255) DEFAULT NULL COMMENT '家庭号码',
  `Problem` varchar(255) DEFAULT NULL COMMENT '问题答案',
  `Email` varchar(255) DEFAULT NULL COMMENT '邮箱地址',
  `IDCard` varchar(255) DEFAULT NULL COMMENT '身份证',
  `PhoneId` varchar(11) DEFAULT NULL COMMENT '手机号码',
  `Name` varchar(255) DEFAULT NULL COMMENT '玩家姓名',
  PRIMARY KEY (`id`)
) ENGINE=MyISAM AUTO_INCREMENT=3 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for users
-- ----------------------------
DROP TABLE IF EXISTS `users`;
CREATE TABLE `users` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
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
) ENGINE=MyISAM AUTO_INCREMENT=4 DEFAULT CHARSET=utf8;

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
) ENGINE=MyISAM AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;
