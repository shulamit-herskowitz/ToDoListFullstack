-- ===================================
-- TodoDB Database Creation Script
-- Run this in MySQL Workbench or command line
-- ===================================

-- Create the database if it doesn't exist
CREATE DATABASE IF NOT EXISTS ToDoDB
CHARACTER SET utf8mb4
COLLATE utf8mb4_unicode_ci;

-- Use the database
USE ToDoDB;

-- Create the Items table
CREATE TABLE IF NOT EXISTS Items (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Name VARCHAR(255) NOT NULL,
    IsComplete TINYINT(1) NOT NULL DEFAULT 0,
    INDEX idx_name (Name),
    INDEX idx_complete (IsComplete)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Optional: Insert some sample data
INSERT INTO Items (Name, IsComplete) VALUES
('Learn ASP.NET Core', 0),
('Build TodoAPI', 0),
('Deploy to production', 0);

-- Verify the table was created
SHOW TABLES;

-- Show table structure
DESCRIBE Items;

-- Show sample data
SELECT * FROM Items;
