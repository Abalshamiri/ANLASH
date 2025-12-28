-- إضافة CountryName و CityName columns إلى Universities table

USE ANLASHDb;
GO

-- Check if columns don't exist, then add them
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Universities]') AND name = 'CountryName')
BEGIN
    ALTER TABLE [dbo].[Universities]
    ADD [CountryName] NVARCHAR(100) NULL;
    
    PRINT 'CountryName column added successfully';
END
ELSE
    PRINT 'CountryName column already exists';

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Universities]') AND name = 'CityName')
BEGIN
    ALTER TABLE [dbo].[Universities]
    ADD [CityName] NVARCHAR(100) NULL;
    
    PRINT 'CityName column added successfully';
END
ELSE
    PRINT 'CityName column already exists';

GO

-- Verify columns were added
SELECT 
    COLUMN_NAME, 
    DATA_TYPE, 
    CHARACTER_MAXIMUM_LENGTH, 
    IS_NULLABLE
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'Universities' 
  AND COLUMN_NAME IN ('CountryName', 'CityName');
