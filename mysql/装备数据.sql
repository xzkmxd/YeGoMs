/*
Navicat MySQL Data Transfer

Source Server         : localhost_3306
Source Server Version : 50553
Source Host           : localhost:3306
Source Database       : mydatabase

Target Server Type    : MYSQL
Target Server Version : 50553
File Encoding         : 65001

Date: 2018-04-04 13:34:57
*/

SET FOREIGN_KEY_CHECKS=0;

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
) ENGINE=InnoDB AUTO_INCREMENT=20943 DEFAULT CHARSET=gbk;

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
) ENGINE=MyISAM AUTO_INCREMENT=23 DEFAULT CHARSET=utf8;
