/*
Navicat MySQL Data Transfer

Source Server         : localhost_3306
Source Server Version : 50553
Source Host           : localhost:3306
Source Database       : mydatabase

Target Server Type    : MYSQL
Target Server Version : 50553
File Encoding         : 65001

Date: 2018-04-01 20:28:47
*/

SET FOREIGN_KEY_CHECKS=0;

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
  PRIMARY KEY (`Id`)
) ENGINE=MyISAM AUTO_INCREMENT=4 DEFAULT CHARSET=utf8;
