-- MySQL Workbench Forward Engineering

SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='TRADITIONAL,ALLOW_INVALID_DATES';

-- -----------------------------------------------------
-- Schema user_dashboard
-- -----------------------------------------------------

-- -----------------------------------------------------
-- Schema user_dashboard
-- -----------------------------------------------------
CREATE SCHEMA IF NOT EXISTS `user_dashboard` DEFAULT CHARACTER SET utf8 ;
USE `user_dashboard` ;

-- -----------------------------------------------------
-- Table `user_dashboard`.`User`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `user_dashboard`.`User` (
  `UserId` INT NOT NULL AUTO_INCREMENT,
  `FirstName` VARCHAR(255) NULL,
  `LastName` VARCHAR(255) NULL,
  `Email` VARCHAR(255) NULL,
  `Password` VARCHAR(255) NULL,
  `Level` INT NULL,
  `Description` VARCHAR(255) NULL,
  `CreatedAt` DATETIME NULL,
  `UpdatedAt` DATETIME NULL,
  PRIMARY KEY (`UserId`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `user_dashboard`.`Message`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `user_dashboard`.`Message` (
  `MessageId` INT NOT NULL AUTO_INCREMENT,
  `MessageText` VARCHAR(255) NULL,
  `CreatedAt` DATETIME NULL,
  `UpdatedAt` DATETIME NULL,
  `PosterId` INT NOT NULL,
  `ProfileId` INT NOT NULL,
  PRIMARY KEY (`MessageId`),
  INDEX `fk_Message_User1_idx` (`PosterId` ASC),
  INDEX `fk_Message_User2_idx` (`ProfileId` ASC),
  CONSTRAINT `fk_Message_User1`
    FOREIGN KEY (`PosterId`)
    REFERENCES `user_dashboard`.`User` (`UserId`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_Message_User2`
    FOREIGN KEY (`ProfileId`)
    REFERENCES `user_dashboard`.`User` (`UserId`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `user_dashboard`.`Comment`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `user_dashboard`.`Comment` (
  `CommentId` INT NOT NULL AUTO_INCREMENT,
  `CommentText` VARCHAR(255) NULL,
  `CreatedAt` DATETIME NULL,
  `UpdatedAt` DATETIME NULL,
  `UserId` INT NOT NULL,
  `MessageId` INT NOT NULL,
  PRIMARY KEY (`CommentId`),
  INDEX `fk_Comment_User_idx` (`UserId` ASC),
  INDEX `fk_Comment_Message1_idx` (`MessageId` ASC),
  CONSTRAINT `fk_Comment_User`
    FOREIGN KEY (`UserId`)
    REFERENCES `user_dashboard`.`User` (`UserId`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_Comment_Message1`
    FOREIGN KEY (`MessageId`)
    REFERENCES `user_dashboard`.`Message` (`MessageId`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;
