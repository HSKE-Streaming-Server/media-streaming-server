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
('ce87d14c31407f373274bae177174219071f7156', 'ZDF - Live-Stream', 'zdf', b'0', b'1', 'https://www.zdf.de/static/0.63.2017/img/logos/epg/zdf.svg', 'https://www.zdf.de/live-tv'),
('00082C5B411111A07A362AEEDE0F546319FFFF49', 'MDR KLASSIK', 'radio', b'1', b'1', 'http://sundtek.de/picons/?g=MDR KLASSIK', 'http://localhost:22000/stream/MDR KLASSIK'),
('009DFE63F1AFDB7DD39E6BD299595656741DD1C4', 'MDR KULTUR', 'radio', b'1', b'1', 'http://sundtek.de/picons/?g=MDR KULTUR', 'http://localhost:22000/stream/MDR KULTUR'),
('00FC631239DCD65821449866518651DEE63905B2', 'MDR S-ANHALT MD', 'radio', b'1', b'1', 'http://sundtek.de/picons/?g=MDR S-ANHALT MD', 'http://localhost:22000/stream/MDR S-ANHALT MD'),
('02F8E9F27329EC332E35FC532467AB4D8585866E', 'HSE24 EXTRA', 'television', b'1', b'1', 'http://sundtek.de/picons/?g=HSE24 EXTRA', 'http://localhost:22000/stream/HSE24 EXTRA'),
('036F8409A330248C717459D14751F8D386C3ABEB', 'ProSieben MAXX', 'television', b'1', b'1', 'http://sundtek.de/picons/?g=ProSieben MAXX', 'http://localhost:22000/stream/ProSieben MAXX'),
('03AED9089688CEE8A42CAD952943D4C7D8ADD629', 'arte HD', 'television', b'1', b'1', 'http://sundtek.de/picons/?g=arte HD', 'http://localhost:22000/stream/arte HD'),
('04243D97E1FBA9F9B5E460FEC3CFA820717F9CE7', 'Bayern 2', 'radio', b'1', b'1', 'http://sundtek.de/picons/?g=Bayern 2', 'http://localhost:22000/stream/Bayern 2'),
('05A9711A07FB7DC384B41B7B32EADFE6D08A8410', 'QVC2', 'television', b'1', b'1', 'http://sundtek.de/picons/?g=QVC2', 'http://localhost:22000/stream/QVC2'),
('073C43229A1B48AF79534F054651B9B633AB89AD', 'tagesschau24 HD', 'television', b'1', b'1', 'http://sundtek.de/picons/?g=tagesschau24 HD', 'http://localhost:22000/stream/tagesschau24 HD'),
('08414D2BA52EDF53720FD0AD947170B893BAB18F', 'Astro TV', 'television', b'1', b'1', 'http://sundtek.de/picons/?g=Astro TV', 'http://localhost:22000/stream/Astro TV'),
('0A0BCE04B82F9AB1E690E50D8CF7E8F4FEF579C4', 'SWR3', 'radio', b'1', b'1', 'http://sundtek.de/picons/?g=SWR3', 'http://localhost:22000/stream/SWR3'),
('104721D5EBDD4F1C74819E83F9161BB11944DCF5', 'PHOENIX HD', 'television', b'1', b'1', 'http://sundtek.de/picons/?g=PHOENIX HD', 'http://localhost:22000/stream/PHOENIX HD'),
('1052BF3482A53476AC570B62D927F3AC24A05F05', 'HOME &amp; GARDEN TV', 'television', b'1', b'1', 'http://sundtek.de/picons/?g=HOME %26amp%3B GARDEN TV', 'http://localhost:22000/stream/HOME %26amp%3B GARDEN TV'),
('11888CC29E8B5F65D7165720F8C88FD0E080CBF0', 'MTV', 'television', b'1', b'1', 'http://sundtek.de/picons/?g=MTV', 'http://localhost:22000/stream/MTV'),
('12E6AF16307EB06E04C735E0639C095CEE49B992', 'Dlf Kultur', 'radio', b'1', b'1', 'http://sundtek.de/picons/?g=Dlf Kultur', 'http://localhost:22000/stream/Dlf Kultur'),
('1509EE780A8A35C28CE44D2587487F2DA2D434DF', 'DELUXE MUSIC', 'television', b'1', b'1', 'http://sundtek.de/picons/?g=DELUXE MUSIC', 'http://localhost:22000/stream/DELUXE MUSIC'),
('159FF8167FD0F67D432637E9019DBC8B8528514A', 'NHK WORLD-JAPAN (eng)', 'television', b'1', b'1', 'http://sundtek.de/picons/?g=NHK WORLD-JAPAN %28eng%29', 'http://localhost:22000/stream/NHK WORLD-JAPAN %28eng%29'),
('188C78665EDE609706586877D5E2615D64025745', 'NDR Blue', 'radio', b'1', b'1', 'http://sundtek.de/picons/?g=NDR Blue', 'http://localhost:22000/stream/NDR Blue'),
('1A459F27833135673BD4E011407EB3889C437816', '3sat HD', 'television', b'1', b'1', 'http://sundtek.de/picons/?g=3sat HD', 'http://localhost:22000/stream/3sat HD'),
('2167C87C9D3D3A388873E1244B8738B4F3E6CC1C', 'BR Fernsehen Süd', 'television', b'1', b'1', 'http://sundtek.de/picons/?g=BR Fernsehen Süd', 'http://localhost:22000/stream/BR Fernsehen Süd'),
('247D1E15324A7DA9967192368C2F635FA1636A6D', 'euronews', 'television', b'1', b'1', 'http://sundtek.de/picons/?g=euronews', 'http://localhost:22000/stream/euronews'),
('270A3C548704C7334761D02B77D126AFE726D8C2', 'SR 1 Europawelle', 'radio', b'1', b'1', 'http://sundtek.de/picons/?g=SR 1 Europawelle', 'http://localhost:22000/stream/SR 1 Europawelle'),
('27393911F480E7D4BE073E361C4F69E3FFE537CC', 'NDR 90', 'radio', b'1', b'1', 'http://sundtek.de/picons/?g=NDR 90%2C3', 'http://localhost:22000/stream/NDR 90%2C3'),
('27513AA9CBCB9880A6018DCCC1E88A0ED5861D74', 'hr2', 'radio', b'1', b'1', 'http://sundtek.de/picons/?g=hr2', 'http://localhost:22000/stream/hr2'),
('277AB2A5E0AAE2EC31951DA47088CD91B4BF57F0', 'SPORT1', 'television', b'1', b'1', 'http://sundtek.de/picons/?g=SPORT1', 'http://localhost:22000/stream/SPORT1'),
('295D9833ABA1B7668845CB026ACDE88AD4FA38E4', 'NDR FS MV', 'television', b'1', b'1', 'http://sundtek.de/picons/?g=NDR FS MV', 'http://localhost:22000/stream/NDR FS MV'),
('29E0AD0CDCDEDEDD4EDE29E4108EAD47FC30B8E6', 'SR Fernsehen HD', 'television', b'1', b'1', 'http://sundtek.de/picons/?g=SR Fernsehen HD', 'http://localhost:22000/stream/SR Fernsehen HD'),
('2A980C37DDA129DC3F4752E89610826FF853240F', 'BBC World News (eng)', 'television', b'1', b'1', 'http://sundtek.de/picons/?g=BBC World News %28eng%29', 'http://localhost:22000/stream/BBC World News %28eng%29'),
('2CDECACBD84C329FF957FEECE7C44DC861D1BF15', 'kabel eins', 'television', b'1', b'1', 'http://sundtek.de/picons/?g=kabel eins', 'http://localhost:22000/stream/kabel eins'),
('36BFD660DD2F7A2CF8DC42F15FDF54D17B268EA6', 'SAT.1 Gold', 'television', b'1', b'1', 'http://sundtek.de/picons/?g=SAT.1 Gold', 'http://localhost:22000/stream/SAT.1 Gold'),
('37D0D806B3C185EC6BE453867664B694B02293D0', '3sat', 'television', b'1', b'1', 'http://sundtek.de/picons/?g=3sat', 'http://localhost:22000/stream/3sat'),
('38AF870DC9276A6A5D51029225A31100328ECD24', 'Welt der Wunder TV', 'television', b'1', b'1', 'http://sundtek.de/picons/?g=Welt der Wunder TV', 'http://localhost:22000/stream/Welt der Wunder TV'),
('3C8B53E60CBC0CB6E04EBBA62E295454B2498135', 'hr-fernsehen HD', 'television', b'1', b'1', 'http://sundtek.de/picons/?g=hr-fernsehen HD', 'http://localhost:22000/stream/hr-fernsehen HD'),
('3FCB94046DDF1B069F678F96B26408BC6FC4672C', 'ONE', 'television', b'1', b'1', 'http://sundtek.de/picons/?g=ONE', 'http://localhost:22000/stream/ONE'),
('40E705E2A2E4F2E03ED2F69973110F1F20FFC03D', 'N24 Doku', 'television', b'1', b'1', 'http://sundtek.de/picons/?g=N24 Doku', 'http://localhost:22000/stream/N24 Doku'),
('41304778DCC13438FFB36AF7AB3B71777622A1B1', 'NDR FS HH HD', 'television', b'1', b'1', 'http://sundtek.de/picons/?g=NDR FS HH HD', 'http://localhost:22000/stream/NDR FS HH HD'),
('41FDC292D166BAC502F8A00E1054D67C7BBD5C8A', 'TV 5 Monde (fre)', 'television', b'1', b'1', 'http://sundtek.de/picons/?g=TV 5 Monde %28fre%29', 'http://localhost:22000/stream/TV 5 Monde %28fre%29'),
('43C055A775857FB01E88A443172B32F5C3C6C6FF', 'TOGGO plus', 'television', b'1', b'1', 'http://sundtek.de/picons/?g=TOGGO plus', 'http://localhost:22000/stream/TOGGO plus'),
('466B3410DE9C2411A829B4B4D0452E788886AF62', 'arte', 'television', b'1', b'1', 'http://sundtek.de/picons/?g=arte', 'http://localhost:22000/stream/arte'),
('47C6E1B5A05F2C456E69006A5FDA30124C979711', 'KiKA', 'television', b'1', b'1', 'http://sundtek.de/picons/?g=KiKA', 'http://localhost:22000/stream/KiKA'),
('487787EADED73972CA528D416E2D6B2EA888E5EA', 'Bibel TV', 'television', b'1', b'1', 'http://sundtek.de/picons/?g=Bibel TV', 'http://localhost:22000/stream/Bibel TV'),
('489D8A52DF63DBB5C7572A18B3DF3B998ADE1B85', '1-2-3.tv HD', 'television', b'1', b'1', 'http://sundtek.de/picons/?g=1-2-3.tv HD', 'http://localhost:22000/stream/1-2-3.tv HD'),
('4989077310B5995676BD659692CA5B190CCD3B4C', 'ZDF HD', 'television', b'1', b'1', 'http://sundtek.de/picons/?g=ZDF HD', 'http://localhost:22000/stream/ZDF HD'),
('49E34DFD00F254D72051084AEE078F8A8787C79E', 'VOXup', 'television', b'1', b'1', 'http://sundtek.de/picons/?g=VOXup', 'http://localhost:22000/stream/VOXup'),
('4ABA226FDD4CEB0EEF6FE9263DF8D51CA8C47FC1', 'HSE24 HD', 'television', b'1', b'1', 'http://sundtek.de/picons/?g=HSE24 HD', 'http://localhost:22000/stream/HSE24 HD'),
('4B9EB8FC6A77CE44C0E83DDF36D10190B4F58E86', 'BR Fernsehen Süd HD', 'television', b'1', b'1', 'http://sundtek.de/picons/?g=BR Fernsehen Süd HD', 'http://localhost:22000/stream/BR Fernsehen Süd HD'),
('4BBA73EADDD123B86059AE8D8C52020B8B04BF98', 'SWR1 RP', 'radio', b'1', b'1', 'http://sundtek.de/picons/?g=SWR1 RP', 'http://localhost:22000/stream/SWR1 RP'),
('4BE85BE7BD736667BC23197682FC1EDF935ECB42', 'SR 2 KulturRadio', 'radio', b'1', b'1', 'http://sundtek.de/picons/?g=SR 2 KulturRadio', 'http://localhost:22000/stream/SR 2 KulturRadio'),
('4C68081F280F97EDAAF4A86C5BEB3778CB9E0A06', 'JAM FM', 'radio', b'1', b'1', 'http://sundtek.de/picons/?g=JAM FM', 'http://localhost:22000/stream/JAM FM'),
('4D381BCFCCE6B11423256768433F97B523B3D51B', 'Eurosport 1', 'television', b'1', b'1', 'http://sundtek.de/picons/?g=Eurosport 1', 'http://localhost:22000/stream/Eurosport 1'),
('4D98CC7C50B57FB8BF20B276875207EA3EAD86BF', 'DASDING', 'radio', b'1', b'1', 'http://sundtek.de/picons/?g=DASDING', 'http://localhost:22000/stream/DASDING'),
('4EFFF9CA337955623A2BDE9069F122F78949F8FB', 'ANIXE+', 'television', b'1', b'1', 'http://sundtek.de/picons/?g=ANIXE%2B', 'http://localhost:22000/stream/ANIXE%2B'),
('4FBC7CFDA265FBAE463BFC83BABD85B578D1A772', 'MDR THÜRINGEN EF', 'radio', b'1', b'1', 'http://sundtek.de/picons/?g=MDR THÜRINGEN EF', 'http://localhost:22000/stream/MDR THÜRINGEN EF'),
('52BAEED357011B00C2D67E2D5636EB42613E1B8E', 'SWR4 RP', 'radio', b'1', b'1', 'http://sundtek.de/picons/?g=SWR4 RP', 'http://localhost:22000/stream/SWR4 RP'),
('537BB7B95DF07CCB9BC6F062044A38A0018FF440', 'WDR HD Köln', 'television', b'1', b'1', 'http://sundtek.de/picons/?g=WDR HD Köln', 'http://localhost:22000/stream/WDR HD Köln'),
('53FFE9AC2DAC2E3C28214404907626CBC6AB2EBF', 'Radio Horeb', 'radio', b'1', b'1', 'http://sundtek.de/picons/?g=Radio Horeb', 'http://localhost:22000/stream/Radio Horeb'),
('55B827F4DE7B03BE010B91185399513FA4D8A7E3', 'Sky Sport News HD', 'television', b'1', b'1', 'http://sundtek.de/picons/?g=Sky %C2%86Sport%C2%87 %C2%86News%C2%87 %C2%86HD%C2%87', 'http://localhost:22000/stream/Sky %C2%86Sport%C2%87 %C2%86News%C2%87 %C2%86HD%C2%87'),
('5AA1BFB1AF2F2A0B9D6EED5240B8E0887BE099D3', 'MDR JUMP', 'radio', b'1', b'1', 'http://sundtek.de/picons/?g=MDR JUMP', 'http://localhost:22000/stream/MDR JUMP'),
('5C37ED2E2464564DAFD9958E4694729156186950', 'health tv', 'television', b'1', b'1', 'http://sundtek.de/picons/?g=health tv', 'http://localhost:22000/stream/health tv'),
('5E5361B7692673EFBC0B7AB6B092E39291457F05', 'Das Erste', 'television', b'1', b'1', 'http://sundtek.de/picons/?g=Das Erste', 'http://localhost:22000/stream/Das Erste'),
('5FC6A3A0073C799A652D88839682C90676A0D0EB', 'WDR Event', 'radio', b'1', b'1', 'http://sundtek.de/picons/?g=WDR Event', 'http://localhost:22000/stream/WDR Event'),
('5FEDFC1D2B49C0FBB9F4F9562BC9A3BE7AB3DFB9', 'Bremen Vier', 'radio', b'1', b'1', 'http://sundtek.de/picons/?g=Bremen Vier', 'http://localhost:22000/stream/Bremen Vier'),
('61032F334BF5370BEA19B4C2D72233FF92793B67', 'RTL Radio', 'radio', b'1', b'1', 'http://sundtek.de/picons/?g=RTL Radio', 'http://localhost:22000/stream/RTL Radio'),
('6179970AB6D7BCC4063C0CF4F93B377F68C7CA97', 'MDR SACHSEN DD', 'radio', b'1', b'1', 'http://sundtek.de/picons/?g=MDR SACHSEN DD', 'http://localhost:22000/stream/MDR SACHSEN DD'),
('6312AF792CA21A1BA7C7A50CBAF84A1523797AE1', '1-2-3.tv', 'television', b'1', b'1', 'http://sundtek.de/picons/?g=1-2-3.tv', 'http://localhost:22000/stream/1-2-3.tv'),
('6453E2F9410FDF9FEF709E38DB96EF6D78BFD612', 'NDR 1 Radio MV SN', 'radio', b'1', b'1', 'http://sundtek.de/picons/?g=NDR 1 Radio MV SN', 'http://localhost:22000/stream/NDR 1 Radio MV SN'),
('681E26EA887BB348253135AFF879157318EECC9F', 'Comedy Central', 'television', b'1', b'1', 'http://sundtek.de/picons/?g=Comedy Central', 'http://localhost:22000/stream/Comedy Central'),
('6993015C4CE75C33AEFD550DF7BD3FC3E0DE1D28', 'Das Erste HD', 'television', b'1', b'1', 'http://sundtek.de/picons/?g=Das Erste HD', 'http://localhost:22000/stream/Das Erste HD'),
('6F465E3D20103E2CEA8C8EE576F7FC156C467DF9', 'ARD-alpha HD', 'television', b'1', b'1', 'http://sundtek.de/picons/?g=ARD-alpha HD', 'http://localhost:22000/stream/ARD-alpha HD'),
('726967C02A41AA0BB83473E256C1CAB547D26ECA', 'Deutsches Musik Fernsehen', 'television', b'1', b'1', 'http://sundtek.de/picons/?g=Deutsches Musik Fernsehen', 'http://localhost:22000/stream/Deutsches Musik Fernsehen'),
('7367DCDA85111A561455E76AB4754FC6E9B2C162', 'sonnenklar.TV', 'television', b'1', b'1', 'http://sundtek.de/picons/?g=sonnenklar.TV', 'http://localhost:22000/stream/sonnenklar.TV'),
('73746CC17569088B910947E905157C2C75A158C7', '1LIVE', 'radio', b'1', b'1', 'http://sundtek.de/picons/?g=1LIVE', 'http://localhost:22000/stream/1LIVE'),
('753387D0342D19EB01C6433D397A80CB9E09AB2C', 'SR Fernsehen', 'television', b'1', b'1', 'http://sundtek.de/picons/?g=SR Fernsehen', 'http://localhost:22000/stream/SR Fernsehen'),
('75A1A6E3E63C5F5ABE6B6D3D54C25A8274D2B799', 'N-JOY', 'radio', b'1', b'1', 'http://sundtek.de/picons/?g=N-JOY', 'http://localhost:22000/stream/N-JOY'),
('75AA6E519E80148F56935153109A618777307892', 'BR-KLASSIK', 'radio', b'1', b'1', 'http://sundtek.de/picons/?g=BR-KLASSIK', 'http://localhost:22000/stream/BR-KLASSIK'),
('77C6B11B56BBE9ABEFF5B5AC3046D66E4873C5D3', 'hr1', 'radio', b'1', b'1', 'http://sundtek.de/picons/?g=hr1', 'http://localhost:22000/stream/hr1'),
('7ACC637347DE3643853DFD262697125FF549114F', 'ProSieben', 'television', b'1', b'1', 'http://sundtek.de/picons/?g=ProSieben', 'http://localhost:22000/stream/ProSieben'),
('7C76F83403604341846628E113A9812B131DBD5E', 'TELE 5', 'television', b'1', b'1', 'http://sundtek.de/picons/?g=TELE 5', 'http://localhost:22000/stream/TELE 5'),
('7CFB12BEF374AE92C36C84069453B9758D04BE06', 'Inforadio', 'radio', b'1', b'1', 'http://sundtek.de/picons/?g=Inforadio', 'http://localhost:22000/stream/Inforadio'),
('7D69011797D7965BAC6C4BF699A5B147AE31F3D4', 'Juwelo TV', 'television', b'1', b'1', 'http://sundtek.de/picons/?g=Juwelo TV', 'http://localhost:22000/stream/Juwelo TV'),
('7EF38FF8212054ABEFD3972279A3CAE83CECB366', 'HSE24', 'television', b'1', b'1', 'http://sundtek.de/picons/?g=HSE24', 'http://localhost:22000/stream/HSE24'),
('80167DB3BC6C43B4F4BCE4D50B917679D4EAEEDD', 'hr3', 'radio', b'1', b'1', 'http://sundtek.de/picons/?g=hr3', 'http://localhost:22000/stream/hr3'),
('803F37B9D77049E3737D50B033DE4CCBC1510B6C', 'Zee.One', 'television', b'1', b'1', 'http://sundtek.de/picons/?g=Zee.One', 'http://localhost:22000/stream/Zee.One'),
('81DB09237D1CFA2234A891FDD82EDFEAC81310FA', 'WDR Köln', 'television', b'1', b'1', 'http://sundtek.de/picons/?g=WDR Köln', 'http://localhost:22000/stream/WDR Köln'),
('8281B851B46E84F42DA7BF09D51F3BEE16296D60', 'Bremen Eins', 'radio', b'1', b'1', 'http://sundtek.de/picons/?g=Bremen Eins', 'http://localhost:22000/stream/Bremen Eins'),
('84EBD56F59E0338282636D37F424CB359D00C34D', 'NDR 2 NDS', 'radio', b'1', b'1', 'http://sundtek.de/picons/?g=NDR 2 NDS', 'http://localhost:22000/stream/NDR 2 NDS'),
('871DF13B0BCFF0B9A01FBCCFDCF40ACCB5E9AB72', 'rbb Berlin HD', 'television', b'1', b'1', 'http://sundtek.de/picons/?g=rbb Berlin HD', 'http://localhost:22000/stream/rbb Berlin HD'),
('8B40E8C19E58A6B6932FC170F01F6CA7F072795E', 'Radio Paloma', 'radio', b'1', b'1', 'http://sundtek.de/picons/?g=Radio Paloma', 'http://localhost:22000/stream/Radio Paloma'),
('8BB1ECF1D6ABF41978A26A734CC81280872A7C30', 'BAYERN 3', 'radio', b'1', b'1', 'http://sundtek.de/picons/?g=BAYERN 3', 'http://localhost:22000/stream/BAYERN 3'),
('8C9B1F2813DD9C390A2688CFB479FFF7AB23E517', 'CNN (eng)', 'television', b'1', b'1', 'http://sundtek.de/picons/?g=CNN %28eng%29', 'http://localhost:22000/stream/CNN %28eng%29'),
('8E892F6D6303514E16C3AA1120CB36D169A7E7AD', 'TLC', 'television', b'1', b'1', 'http://sundtek.de/picons/?g=TLC', 'http://localhost:22000/stream/TLC'),
('8EDD9ECBEC5259B2FBC390DC731592D5913B8055', 'ZDF', 'television', b'1', b'1', 'http://sundtek.de/picons/?g=ZDF', 'http://localhost:22000/stream/ZDF'),
('915FDA982E0AD6C234C6756639A3B518BD4A9C72', 'SWR Aktuell', 'radio', b'1', b'1', 'http://sundtek.de/picons/?g=SWR Aktuell', 'http://localhost:22000/stream/SWR Aktuell'),
('91DB310B55D63D27482571A1EB3285C30A663D27', 'NDR 1 Nieders. HAN', 'radio', b'1', b'1', 'http://sundtek.de/picons/?g=NDR 1 Nieders. HAN', 'http://localhost:22000/stream/NDR 1 Nieders. HAN'),
('92333AC588ACD9EA60139D118C37EABA9E321BCD', 'sonnenklar.TV HD', 'television', b'1', b'1', 'http://sundtek.de/picons/?g=sonnenklar.TV HD', 'http://localhost:22000/stream/sonnenklar.TV HD'),
('929DF42A4380786A1840DFE46C8E57760E3691DE', 'hr-fernsehen', 'television', b'1', b'1', 'http://sundtek.de/picons/?g=hr-fernsehen', 'http://localhost:22000/stream/hr-fernsehen'),
('98737822967C3A26B453B2CF5E6B1E5530E1E433', 'ARD-alpha', 'television', b'1', b'1', 'http://sundtek.de/picons/?g=ARD-alpha', 'http://localhost:22000/stream/ARD-alpha'),
('98C127BBC66ACE925F26AE00CD7C89443CF3F233', 'antenne Saar', 'radio', b'1', b'1', 'http://sundtek.de/picons/?g=antenne Saar', 'http://localhost:22000/stream/antenne Saar'),
('9AC8BE212E54365191C949EF3325FCFBFC848266', 'sixx', 'television', b'1', b'1', 'http://sundtek.de/picons/?g=sixx', 'http://localhost:22000/stream/sixx'),
('9B6BD2AB044D61108875697A32AC586407339B88', 'Bayern 1', 'radio', b'1', b'1', 'http://sundtek.de/picons/?g=Bayern 1', 'http://localhost:22000/stream/Bayern 1'),
('9B99336171AC8CDE0F770D965FF1F0375F5E4B1B', 'tagesschau24', 'television', b'1', b'1', 'http://sundtek.de/picons/?g=tagesschau24', 'http://localhost:22000/stream/tagesschau24'),
('9CD913D0044E4925915FCD367A2E9C2616F745D4', 'WDR 2 Rheinland', 'radio', b'1', b'1', 'http://sundtek.de/picons/?g=WDR 2 Rheinland', 'http://localhost:22000/stream/WDR 2 Rheinland'),
('9D95415A8AEE8918A210E0A14AC2FC3CD8959100', 'Dokumente und Debatten', 'radio', b'1', b'1', 'http://sundtek.de/picons/?g=Dokumente und Debatten', 'http://localhost:22000/stream/Dokumente und Debatten'),
('A29D8149F0F6F36EC702269E383061FC44358BF3', 'YOU FM', 'radio', b'1', b'1', 'http://sundtek.de/picons/?g=YOU FM', 'http://localhost:22000/stream/YOU FM'),
('A61129C7BC10FF407CA95B436067004FB9D6FE3A', 'NDR Kultur', 'radio', b'1', b'1', 'http://sundtek.de/picons/?g=NDR Kultur', 'http://localhost:22000/stream/NDR Kultur'),
('A7FA3A205E04159A7B7444023C248E36097D06BF', 'SWR Fernsehen RP', 'television', b'1', b'1', 'http://sundtek.de/picons/?g=SWR Fernsehen RP', 'http://localhost:22000/stream/SWR Fernsehen RP'),
('A9DE1964737E2C353B8A9C97A16AD704ECF28CDA', 'MDR Thüringen', 'television', b'1', b'1', 'http://sundtek.de/picons/?g=MDR Thüringen', 'http://localhost:22000/stream/MDR Thüringen'),
('AA8A1FA97DC2135C72B32ACB6DE02923A688E094', 'B5 aktuell', 'radio', b'1', b'1', 'http://sundtek.de/picons/?g=B5 aktuell', 'http://localhost:22000/stream/B5 aktuell'),
('AD8DEA1B196747BCA1210C32BD08AB07B38EA483', 'Disney Channel', 'television', b'1', b'1', 'http://sundtek.de/picons/?g=Disney Channel', 'http://localhost:22000/stream/Disney Channel'),
('AE71D27E2C02B8DA3CCAF878B3ECC4EE7323F7EF', 'MDR SPUTNIK', 'radio', b'1', b'1', 'http://sundtek.de/picons/?g=MDR SPUTNIK', 'http://localhost:22000/stream/MDR SPUTNIK'),
('AE99289BC1D22DD401BB89B962FA45E16EE0D368', 'Nick', 'television', b'1', b'1', 'http://sundtek.de/picons/?g=Nick', 'http://localhost:22000/stream/Nick'),
('AED12BF7DDAE69FCA15391DD6B990CDDEC155CCF', 'SWR BW HD', 'television', b'1', b'1', 'http://sundtek.de/picons/?g=SWR BW HD', 'http://localhost:22000/stream/SWR BW HD'),
('AF0E0BBF64D73520A287B9A7BC55B1E6BD92D76E', 'SWR2', 'radio', b'1', b'1', 'http://sundtek.de/picons/?g=SWR2', 'http://localhost:22000/stream/SWR2'),
('AF3B087D7604D0857AF258175DA2CF3824891AEC', 'Bremen Next', 'radio', b'1', b'1', 'http://sundtek.de/picons/?g=Bremen Next', 'http://localhost:22000/stream/Bremen Next'),
('B4E3416D0980854B74C9B086B72A370DE9823E90', 'SWR1 BW', 'radio', b'1', b'1', 'http://sundtek.de/picons/?g=SWR1 BW', 'http://localhost:22000/stream/SWR1 BW'),
('B50216B56CDCC3E088B928447393B0FF251BFBC6', 'Dlf Nova', 'radio', b'1', b'1', 'http://sundtek.de/picons/?g=Dlf Nova', 'http://localhost:22000/stream/Dlf Nova'),
('B7E43BDDE11CDD9338A45252E53142B4E261D522', 'zdf_neo HD', 'television', b'1', b'1', 'http://sundtek.de/picons/?g=zdf_neo HD', 'http://localhost:22000/stream/zdf_neo HD'),
('B9A9A7F8B974CC463802436E0E85C37F748E6F0A', 'QVC', 'television', b'1', b'1', 'http://sundtek.de/picons/?g=QVC', 'http://localhost:22000/stream/QVC'),
('BB1FE812F05F589FA541D786FCD5B2AFF3D8659C', 'COSMO', 'radio', b'1', b'1', 'http://sundtek.de/picons/?g=COSMO', 'http://localhost:22000/stream/COSMO'),
('BB498A85FF5A67FB426BF5C967FE096B04D76966', 'WDR 4', 'radio', b'1', b'1', 'http://sundtek.de/picons/?g=WDR 4', 'http://localhost:22000/stream/WDR 4'),
('BD90F90233FBF5185AEA754028FE50E75FA671C6', 'SONLife (eng)', 'television', b'1', b'1', 'http://sundtek.de/picons/?g=SONLife %28eng%29', 'http://localhost:22000/stream/SONLife %28eng%29'),
('BF5841D99EFD1248E4AC5567BD9A7CBDB41BD5DD', 'ERF Plus', 'radio', b'1', b'1', 'http://sundtek.de/picons/?g=ERF Plus', 'http://localhost:22000/stream/ERF Plus'),
('C17C036A7AB2C31695524D617CA4C2F5257F3F24', 'radioeins', 'radio', b'1', b'1', 'http://sundtek.de/picons/?g=radioeins', 'http://localhost:22000/stream/radioeins'),
('C446278CC019182938CC5FD80A31EBF0EEB7D5EB', 'DMAX', 'television', b'1', b'1', 'http://sundtek.de/picons/?g=DMAX', 'http://localhost:22000/stream/DMAX'),
('C44EF439342971979AE39C152A24CC5D762925AB', 'SWR4 BW', 'radio', b'1', b'1', 'http://sundtek.de/picons/?g=SWR4 BW', 'http://localhost:22000/stream/SWR4 BW'),
('C54A5FB0E45E90BF5B6C1CC22CA3AE816872D7B2', 'Bremen Zwei', 'radio', b'1', b'1', 'http://sundtek.de/picons/?g=Bremen Zwei', 'http://localhost:22000/stream/Bremen Zwei'),
('C55648DE6DD3440D26F79BB9D874EEFEE70D1909', 'SWR RP HD', 'television', b'1', b'1', 'http://sundtek.de/picons/?g=SWR RP HD', 'http://localhost:22000/stream/SWR RP HD'),
('C5F9AA31A4FBC2EEB71DD17F4CECFEB79ACDA6B3', 'RTLplus', 'television', b'1', b'1', 'http://sundtek.de/picons/?g=RTLplus', 'http://localhost:22000/stream/RTLplus'),
('C68450A2C90D79B5210B81F5D99D5E8C21AE7C3B', 'WELT', 'television', b'1', b'1', 'http://sundtek.de/picons/?g=WELT', 'http://localhost:22000/stream/WELT'),
('C741508CABB4F180C9D484A57F604C7773DB5CC4', 'rbb 88.8', 'radio', b'1', b'1', 'http://sundtek.de/picons/?g=rbb 88.8', 'http://localhost:22000/stream/rbb 88.8'),
('C78F7694DB35325DDDE938F8B7C1C1E0D3B0B755', 'ServusTV', 'television', b'1', b'1', 'http://sundtek.de/picons/?g=ServusTV', 'http://localhost:22000/stream/ServusTV'),
('CAE98A91D9FCD7D96342643E05CDFE4F561082D0', 'WDR 3', 'radio', b'1', b'1', 'http://sundtek.de/picons/?g=WDR 3', 'http://localhost:22000/stream/WDR 3'),
('CCA4633D52C52725D122098CC7F83F5D2E8A193E', 'UnserDing', 'radio', b'1', b'1', 'http://sundtek.de/picons/?g=UnserDing', 'http://localhost:22000/stream/UnserDing'),
('CDDFF79B1D14446C6778960C38ABD966F2A06950', 'sunshine live', 'radio', b'1', b'1', 'http://sundtek.de/picons/?g=sunshine live', 'http://localhost:22000/stream/sunshine live'),
('CE6F42FF769927AF396AC587268B70A5EEE65F9E', 'kabel eins Doku', 'television', b'1', b'1', 'http://sundtek.de/picons/?g=kabel eins Doku', 'http://localhost:22000/stream/kabel eins Doku'),
('CF066D43D87C20CCFF3612C9E9994142D00291F8', 'ZDFinfo HD', 'television', b'1', b'1', 'http://sundtek.de/picons/?g=ZDFinfo HD', 'http://localhost:22000/stream/ZDFinfo HD'),
('CF0DCA6ABCB7B4365A9DC4DE2FB53270E6B9DEB3', 'NITRO', 'television', b'1', b'1', 'http://sundtek.de/picons/?g=NITRO', 'http://localhost:22000/stream/NITRO'),
('D02145D30DB4F2A03855FC06A984294DE2A4D003', 'BAYERN plus', 'radio', b'1', b'1', 'http://sundtek.de/picons/?g=BAYERN plus', 'http://localhost:22000/stream/BAYERN plus'),
('D165FA4244B1B475917E45B6B71334268F0DED9D', 'WDR 5', 'radio', b'1', b'1', 'http://sundtek.de/picons/?g=WDR 5', 'http://localhost:22000/stream/WDR 5'),
('D1833BC14B34F5E49AB81C493B728E0775CFECCC', 'Al Jazeera Int (eng)', 'television', b'1', b'1', 'http://sundtek.de/picons/?g=Al Jazeera Int %28eng%29', 'http://localhost:22000/stream/Al Jazeera Int %28eng%29'),
('D26F1C182A84FA90A2F140068E32766FFC5945C5', '1LIVE diGGi', 'radio', b'1', b'1', 'http://sundtek.de/picons/?g=1LIVE diGGi', 'http://localhost:22000/stream/1LIVE diGGi'),
('D2F4EF2284ABA459DEFE670BFA15E3B916A5A31C', 'hr4', 'radio', b'1', b'1', 'http://sundtek.de/picons/?g=hr4', 'http://localhost:22000/stream/hr4'),
('D31E4B26F2B5B27961AC30777DD9542AE9AF07BD', 'zdf_neo', 'television', b'1', b'1', 'http://sundtek.de/picons/?g=zdf_neo', 'http://localhost:22000/stream/zdf_neo'),
('D46470B2B78C6DF3BD256C6CBA08E30C1010F852', 'Fritz', 'radio', b'1', b'1', 'http://sundtek.de/picons/?g=Fritz', 'http://localhost:22000/stream/Fritz'),
('D56CBC4E2599D6E8B74308BB28C0C50BCFB5AC22', 'MEGA Radio', 'radio', b'1', b'1', 'http://sundtek.de/picons/?g=MEGA Radio', 'http://localhost:22000/stream/MEGA Radio'),
('D631D91B4B33D4CB87F761A4C72C04974BBA7D1A', 'MDR Sachsen HD', 'television', b'1', b'1', 'http://sundtek.de/picons/?g=MDR Sachsen HD', 'http://localhost:22000/stream/MDR Sachsen HD'),
('DBF59891E934132327621D657B7E0A20E4E915B4', 'NDR Info NDS', 'radio', b'1', b'1', 'http://sundtek.de/picons/?g=NDR Info NDS', 'http://localhost:22000/stream/NDR Info NDS'),
('DCB4C3405843354ADE647B82956FB410A99CEABA', 'rbb Berlin', 'television', b'1', b'1', 'http://sundtek.de/picons/?g=rbb Berlin', 'http://localhost:22000/stream/rbb Berlin'),
('DCCAA9D199756933A392970E7C61D10B64A29A58', 'WDR 2 Ostwestfalen-Lippe', 'radio', b'1', b'1', 'http://sundtek.de/picons/?g=WDR 2 Ostwestfalen-Lippe', 'http://localhost:22000/stream/WDR 2 Ostwestfalen-Lippe'),
('DDD872A49F3ACF46C5A1166E0468541951978D0A', 'Antenne Brandenburg', 'radio', b'1', b'1', 'http://sundtek.de/picons/?g=Antenne Brandenburg', 'http://localhost:22000/stream/Antenne Brandenburg'),
('DF1DE33833665337236EB41C8563B35D5921E7C1', 'RTL II', 'television', b'1', b'1', 'http://sundtek.de/picons/?g=RTL II', 'http://localhost:22000/stream/RTL II'),
('E12EE1635661034CB2DD1CFB7773C80ADE86BFA1', 'KiKA HD', 'television', b'1', b'1', 'http://sundtek.de/picons/?g=KiKA HD', 'http://localhost:22000/stream/KiKA HD'),
('E2EA4790731B67892CE0C7EC8B5AFED40A55A19F', 'B5 plus', 'radio', b'1', b'1', 'http://sundtek.de/picons/?g=B5 plus', 'http://localhost:22000/stream/B5 plus'),
('E5497891734D822C7A7202B53F79F9289F6D3144', 'allgäu.tv HD', 'television', b'1', b'1', 'http://sundtek.de/picons/?g=allgäu.tv HD', 'http://localhost:22000/stream/allgäu.tv HD'),
('E9B0A9C8E911A99436A90DDC174661E5F75AAC28', 'NDR Info Spez.', 'radio', b'1', b'1', 'http://sundtek.de/picons/?g=NDR Info Spez.', 'http://localhost:22000/stream/NDR Info Spez.'),
('E9CDB2BC0462B870B9EE4032828A74763ED0305B', 'rbbKultur', 'radio', b'1', b'1', 'http://sundtek.de/picons/?g=rbbKultur', 'http://localhost:22000/stream/rbbKultur'),
('E9FC54765B59107798FB1D6D6AEB7ECD1D047CF4', 'Sky Sport News', 'television', b'1', b'1', 'http://sundtek.de/picons/?g=Sky %C2%86Sport%C2%87 %C2%86News%C2%87', 'http://localhost:22000/stream/Sky %C2%86Sport%C2%87 %C2%86News%C2%87'),
('EAFB5B8FBA39111C8BA75491D05C084670664148', 'ONE HD', 'television', b'1', b'1', 'http://sundtek.de/picons/?g=ONE HD', 'http://localhost:22000/stream/ONE HD'),
('EB9C3A4DCF6679F2DB76AB5643572E3C5E4B46EB', 'QVC HD', 'television', b'1', b'1', 'http://sundtek.de/picons/?g=QVC HD', 'http://localhost:22000/stream/QVC HD'),
('EBD524810C9433F04DA08CC35B5CB5099548B0F6', 'SUPER RTL', 'television', b'1', b'1', 'http://sundtek.de/picons/?g=SUPER RTL', 'http://localhost:22000/stream/SUPER RTL'),
('EDBEF4B924A2E2C20E7DEC42FFD3032099AF0AD2', 'phoenix', 'television', b'1', b'1', 'http://sundtek.de/picons/?g=phoenix', 'http://localhost:22000/stream/phoenix'),
('EF37066216CB17D21DA33D52807158C4361C50AA', 'Channel21', 'television', b'1', b'1', 'http://sundtek.de/picons/?g=Channel21', 'http://localhost:22000/stream/Channel21'),
('EF5ED7D3300183FEC7E0A729DF51818B0654BF28', 'n-tv', 'television', b'1', b'1', 'http://sundtek.de/picons/?g=n-tv', 'http://localhost:22000/stream/n-tv'),
('F07C20750BCA0CCC531FAF665BCB289C55966C4D', 'BR Heimat', 'radio', b'1', b'1', 'http://sundtek.de/picons/?g=BR Heimat', 'http://localhost:22000/stream/BR Heimat'),
('F181130B522F643B4C20CB8BF55AB5E94DE4C351', 'ZDFinfo', 'television', b'1', b'1', 'http://sundtek.de/picons/?g=ZDFinfo', 'http://localhost:22000/stream/ZDFinfo'),
('F1916B924B19D717C83C09CBA3E5210E4322FECF', 'NDR1 Welle Nord KI', 'radio', b'1', b'1', 'http://sundtek.de/picons/?g=NDR1 Welle Nord KI', 'http://localhost:22000/stream/NDR1 Welle Nord KI'),
('F50D70DD12EE5398789F2F778384654481D1867B', 'MDR AKTUELL', 'radio', b'1', b'1', 'http://sundtek.de/picons/?g=MDR AKTUELL', 'http://localhost:22000/stream/MDR AKTUELL'),
('FA8191EABDB42FFB22FAA883103474F6F60CC00D', 'PULS', 'radio', b'1', b'1', 'http://sundtek.de/picons/?g=PULS', 'http://localhost:22000/stream/PULS'),
('FBEF072D9F841E58BE46199F1AD6AEA940CCCE2B', 'Dlf', 'radio', b'1', b'1', 'http://sundtek.de/picons/?g=Dlf', 'http://localhost:22000/stream/Dlf'),
('FC0B071E19217C790B924C700A78F8086F9FB7D7', 'SR 3 Saarlandwelle', 'radio', b'1', b'1', 'http://sundtek.de/picons/?g=SR 3 Saarlandwelle', 'http://localhost:22000/stream/SR 3 Saarlandwelle'),
('FC4BFC059C17FD74CA3F562FBE5C3AED5F2FBF1D', 'hr-iNFO', 'radio', b'1', b'1', 'http://sundtek.de/picons/?g=hr-iNFO', 'http://localhost:22000/stream/hr-iNFO'),
('FC4C90F7B554DD4E484794BAEEDA1D6870EDAD58', 'Die Maus', 'radio', b'1', b'1', 'http://sundtek.de/picons/?g=Die Maus', 'http://localhost:22000/stream/Die Maus'),
('FDCBC89C55834C5F3D5432C613476C975D1C6BC9', 'NDR Plus', 'radio', b'1', b'1', 'http://sundtek.de/picons/?g=NDR Plus', 'http://localhost:22000/stream/NDR Plus'),
('FEF52228C229C5BFBD888E855BC1ECDA6B5C61D2', 'VOX', 'television', b'1', b'1', 'http://sundtek.de/picons/?g=VOX', 'http://localhost:22000/stream/VOX');


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
