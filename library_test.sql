-- phpMyAdmin SQL Dump
-- version 4.6.5.2
-- https://www.phpmyadmin.net/
--
-- Host: localhost:8889
-- Generation Time: Aug 25, 2017 at 01:31 AM
-- Server version: 5.6.35
-- PHP Version: 7.0.15

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `library_test`
--

-- --------------------------------------------------------

--
-- Table structure for table `authors`
--

CREATE TABLE `authors` (
  `id` bigint(20) UNSIGNED NOT NULL,
  `name` varchar(255) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Dumping data for table `authors`
--

INSERT INTO `authors` (`id`, `name`) VALUES
(214, 'JK Rowling');

-- --------------------------------------------------------

--
-- Table structure for table `authors_books`
--

CREATE TABLE `authors_books` (
  `id` bigint(20) UNSIGNED NOT NULL,
  `author_id` int(11) DEFAULT NULL,
  `book_id` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Dumping data for table `authors_books`
--

INSERT INTO `authors_books` (`id`, `author_id`, `book_id`) VALUES
(1, 11, 30),
(2, 17, 36),
(3, 23, 42),
(4, 29, 48),
(5, 35, 54),
(6, 41, 60),
(7, 47, 66),
(8, 53, 72),
(9, 59, 73),
(10, 60, 79),
(11, 66, 80),
(12, 67, 86),
(13, 73, 87),
(14, 74, 93),
(15, 80, 94),
(16, 81, 100),
(17, 87, 101),
(18, 88, 107),
(19, 94, 108),
(20, 95, 114),
(21, 101, 115),
(22, 102, 121),
(23, 108, 122),
(24, 109, 128),
(25, 115, 129),
(26, 116, 135),
(27, 122, 136),
(28, 123, 142),
(29, 129, 143),
(30, 130, 149),
(31, 136, 1),
(32, 137, 7),
(33, 143, 9),
(34, 144, 15),
(35, 150, 17),
(36, 151, 23),
(37, 157, 25),
(38, 158, 31),
(39, 164, 33),
(40, 165, 39),
(41, 171, 41),
(42, 172, 47),
(43, 178, 49),
(44, 179, 55),
(45, 185, 57),
(46, 186, 63),
(47, 192, 65),
(48, 193, 71),
(49, 199, 73),
(50, 200, 79),
(51, 206, 81),
(52, 207, 87),
(53, 213, 89),
(54, 214, 95);

-- --------------------------------------------------------

--
-- Table structure for table `books`
--

CREATE TABLE `books` (
  `id` bigint(20) UNSIGNED NOT NULL,
  `title` varchar(255) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Table structure for table `copies`
--

CREATE TABLE `copies` (
  `id` bigint(20) UNSIGNED NOT NULL,
  `book_id` int(11) DEFAULT NULL,
  `available` tinyint(1) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Dumping data for table `copies`
--

INSERT INTO `copies` (`id`, `book_id`, `available`) VALUES
(111, 1, 0);

-- --------------------------------------------------------

--
-- Table structure for table `copies_patrons`
--

CREATE TABLE `copies_patrons` (
  `id` bigint(20) UNSIGNED NOT NULL,
  `copy_id` int(11) DEFAULT NULL,
  `patron_id` int(11) DEFAULT NULL,
  `due` datetime DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Dumping data for table `copies_patrons`
--

INSERT INTO `copies_patrons` (`id`, `copy_id`, `patron_id`, `due`) VALUES
(1, 7, 31, '2017-09-06 15:12:22'),
(2, 11, 37, '2017-09-06 15:14:36'),
(3, 15, 43, '2017-09-06 15:16:21'),
(4, 19, 49, '2017-09-06 15:19:19'),
(5, 23, 55, '2017-09-06 15:20:04'),
(6, 27, 61, '2017-09-06 15:23:45'),
(7, 31, 6, '2017-09-07 08:39:43'),
(8, 37, 12, '2017-09-07 08:40:11'),
(9, 43, 18, '2017-09-07 08:41:57'),
(10, 49, 24, '2017-09-07 08:44:18'),
(11, 57, 30, '2017-09-07 08:46:29'),
(12, 65, 36, '2017-09-07 08:46:41'),
(13, 73, 42, '2017-09-07 08:47:27'),
(14, 81, 48, '2017-09-07 08:48:11'),
(15, 89, 54, '2017-09-07 08:48:20'),
(16, 97, 60, '2017-09-07 08:48:29'),
(17, 105, 66, '2017-09-07 08:48:46'),
(18, 111, 72, '2017-09-07 08:49:31');

-- --------------------------------------------------------

--
-- Table structure for table `patrons`
--

CREATE TABLE `patrons` (
  `id` bigint(20) UNSIGNED NOT NULL,
  `name` varchar(255) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Indexes for dumped tables
--

--
-- Indexes for table `authors`
--
ALTER TABLE `authors`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `id` (`id`);

--
-- Indexes for table `authors_books`
--
ALTER TABLE `authors_books`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `id` (`id`);

--
-- Indexes for table `books`
--
ALTER TABLE `books`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `id` (`id`);

--
-- Indexes for table `copies`
--
ALTER TABLE `copies`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `id` (`id`);

--
-- Indexes for table `copies_patrons`
--
ALTER TABLE `copies_patrons`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `id` (`id`);

--
-- Indexes for table `patrons`
--
ALTER TABLE `patrons`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `id` (`id`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `authors`
--
ALTER TABLE `authors`
  MODIFY `id` bigint(20) UNSIGNED NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=215;
--
-- AUTO_INCREMENT for table `authors_books`
--
ALTER TABLE `authors_books`
  MODIFY `id` bigint(20) UNSIGNED NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=55;
--
-- AUTO_INCREMENT for table `books`
--
ALTER TABLE `books`
  MODIFY `id` bigint(20) UNSIGNED NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=97;
--
-- AUTO_INCREMENT for table `copies`
--
ALTER TABLE `copies`
  MODIFY `id` bigint(20) UNSIGNED NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=112;
--
-- AUTO_INCREMENT for table `copies_patrons`
--
ALTER TABLE `copies_patrons`
  MODIFY `id` bigint(20) UNSIGNED NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=19;
--
-- AUTO_INCREMENT for table `patrons`
--
ALTER TABLE `patrons`
  MODIFY `id` bigint(20) UNSIGNED NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=73;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
