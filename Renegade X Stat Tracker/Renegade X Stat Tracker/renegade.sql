/*
Navicat MySQL Data Transfer

Source Server         : Local Development (MySQL)
Source Server Version : 50612
Source Host           : localhost:3306
Source Database       : renegade

Target Server Type    : MYSQL
Target Server Version : 50612
File Encoding         : 65001

Date: 2014-10-26 13:52:52
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for building_kills
-- ----------------------------
DROP TABLE IF EXISTS `building_kills`;
CREATE TABLE `building_kills` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `killer` bigint(255) DEFAULT NULL,
  `building` int(255) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of building_kills
-- ----------------------------

-- ----------------------------
-- Table structure for damage
-- ----------------------------
DROP TABLE IF EXISTS `damage`;
CREATE TABLE `damage` (
  `id` int(11) NOT NULL,
  `value` varchar(50) DEFAULT NULL,
  `name` varchar(50) DEFAULT NULL,
  `type` tinyint(4) DEFAULT '0',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of damage
-- ----------------------------

-- ----------------------------
-- Table structure for kills
-- ----------------------------
DROP TABLE IF EXISTS `kills`;
CREATE TABLE `kills` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `killer` bigint(255) DEFAULT NULL,
  `victim` bigint(255) DEFAULT NULL,
  `damage` int(11) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `steamids` (`killer`,`victim`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of kills
-- ----------------------------

-- ----------------------------
-- Table structure for players
-- ----------------------------
DROP TABLE IF EXISTS `players`;
CREATE TABLE `players` (
  `steamid` bigint(20) NOT NULL,
  `name` varchar(255) NOT NULL,
  `ip` varchar(18) NOT NULL,
  `kills` int(10) unsigned DEFAULT '0',
  `deaths` int(10) unsigned DEFAULT '0',
  `suicides` int(10) unsigned DEFAULT '0',
  `killstreak` smallint(5) unsigned DEFAULT '0',
  `deathstreak` smallint(5) unsigned DEFAULT '0',
  `buildings` int(10) unsigned DEFAULT '0',
  `destroyed` int(10) unsigned DEFAULT '0',
  `headshots` int(10) unsigned DEFAULT '0',
  `runover` int(10) unsigned DEFAULT '0',
  `level` int(255) unsigned DEFAULT '0',
  `spoken` int(255) unsigned DEFAULT '0',
  `banned` tinyint(255) DEFAULT '0',
  `online` tinyint(255) DEFAULT '0',
  `experience` int(255) unsigned DEFAULT '0',
  `plevel` int(255) unsigned DEFAULT '1',
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `email` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`id`,`steamid`)
) ENGINE=InnoDB AUTO_INCREMENT=19 DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of players
-- ----------------------------
INSERT INTO `players` VALUES ('76561197988827752', 'JaTochNietDan', '127.0.0.1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '1', '1', 'contact@jatochnietdan.com');
INSERT INTO `players` VALUES ('76561197994142246', 'RedCat', '127.0.0.1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '2', null);
INSERT INTO `players` VALUES ('76561197971348015', 'DirtyDoes', '127.0.0.1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '3', null);
INSERT INTO `players` VALUES ('76561197973062864', 'Shadalax', '127.0.0.1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '4', null);
INSERT INTO `players` VALUES ('76561197982112916', 'Malcom X', '127.0.0.1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '5', null);
INSERT INTO `players` VALUES ('76561197985392500', 'Baker', '127.0.0.1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '6', null);
INSERT INTO `players` VALUES ('76561197992043612', 'Science', '127.0.0.1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '7', null);
INSERT INTO `players` VALUES ('76561197996677111', 'MuchFrog', '127.0.0.1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '8', null);
INSERT INTO `players` VALUES ('76561197996745001', 'Concrete', '127.0.0.1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '9', null);
INSERT INTO `players` VALUES ('76561198014636212', 'Havok', '127.0.0.1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '10', null);
INSERT INTO `players` VALUES ('76561198016162251', 'Renegade', '127.0.0.1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '11', null);
INSERT INTO `players` VALUES ('76561198023444915', 'Chair', '127.0.0.1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '12', null);
INSERT INTO `players` VALUES ('76561198056174633', 'Barely', '127.0.0.1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '13', null);
INSERT INTO `players` VALUES ('76561197996063239', 'Xanexe', '127.0.0.1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '14', null);
INSERT INTO `players` VALUES ('76561198014065099', 'Renegadde', '127.0.0.1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '15', null);
INSERT INTO `players` VALUES ('76561198023443123', 'Chaiwr', '127.0.0.1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '16', null);
INSERT INTO `players` VALUES ('76561197989065769', 'Barwely', '127.0.0.1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '17', null);
INSERT INTO `players` VALUES ('76561198096726791', 'Xanaex', '127.0.0.1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '1', '18', null);
