-- phpMyAdmin SQL Dump
-- version 5.0.2
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Erstellungszeit: 29. Apr 2020 um 18:57
-- Server-Version: 10.4.11-MariaDB
-- PHP-Version: 7.4.5

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";

CREATE DATABASE IF NOT EXISTS mediacontent CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Datenbank: `mediacontent`
--

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `mediacontent`
--

CREATE TABLE `mediacontent` (
  `ID` varchar(40) NOT NULL,
  `Name` varchar(50) NOT NULL,
  `Category` varchar(50) NOT NULL,
  `Tunersource` bit(1) NOT NULL,
  `LiveStream` bit(1) NOT NULL,
  `Imagelocation` varchar(250) DEFAULT NULL,
  `Link` varchar(250) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Daten für Tabelle `mediacontent`
--

INSERT INTO `mediacontent` (`ID`, `Name`, `Category`, `Tunersource`, `LiveStream`, `Imagelocation`, `Link`) VALUES
('4ee8bcf8006a012bc48a8c6f33c9fad60ce08191', 'Evitas Rache', 'zdf', b'0', b'0', 'https://www.zdf.de/assets/evitas-rache-100~1280x720?cb=1583764637324', 'https://zdfvodnone-vh.akamaihd.net/i/meta-files/zdf/smil/m3u8/300/20/03/200328_1345_sendung_sof/3/200328_1345_sendung_sof.smil/master.m3u8'),
('5cd15b46d450555f90e19153f7d9e8f60eed228a', 'Sturm der Liebe - Heisse Gefuehle in der Almhuette', 'ard', b'0', b'0', 'https://img.ardmediathek.de/standard/00/74/91/61/32/-2062574680/16x9/1920?mandant=ard', 'https://dasersteuni-vh.akamaihd.net/i/de/2020/04/02/4678372c-4292-43d6-982a-361dfcbbd079/,640-1_641582,320-1_641582,960-1_641582,480-1_641582,512-1_641582,1280-1_641582,1920-1_641582,.mp4.csmil/master.m3u8?set-segment-duration=responsive'),
('a419660ad146b1db7cdbe75c4ba49a305966f055', 'ARD - Live-Stream', 'ard', b'0', b'1', 'https://live.daserste.de/live/das-erste-live-114~_v-varxl_7c95b0.jpg', 'https://mcdn.daserste.de/daserste/de/master_3744.m3u8'),
('bc9295899c092306ec1da79010f79ec5ee00ba68', 'Arte - Live-Stream', 'arte', b'0', b'1', 'https://api-cdn.arte.tv/api/mami/v1/program/de/076595-001-F/940x530', 'https://artelive-lh.akamaihd.net/i/artelive_de@393591/master.m3u8'),
('ce87d14c31407f373274bae177174219071f7156', 'ZDF - Live-Stream', 'zdf', b'0', b'1', 'https://www.zdf.de/static/0.63.2017/img/logos/epg/zdf.svg', 'https://www.zdf.de/live-tv');

--
-- Indizes der exportierten Tabellen
--

--
-- Indizes für die Tabelle `mediacontent`
--
ALTER TABLE `mediacontent`
  ADD PRIMARY KEY (`ID`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
