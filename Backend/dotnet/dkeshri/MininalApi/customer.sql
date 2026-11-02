SET XACT_ABORT ON;

IF EXISTS (SELECT 1 FROM dbo.Customers)
    THROW 50001, 'Customers must be empty before loading customer.sql.', 1;

BEGIN TRY
    BEGIN TRANSACTION;

    SET IDENTITY_INSERT dbo.Customers ON;

    ;WITH Digits AS
    (
        SELECT n
        FROM (VALUES (0), (1), (2), (3), (4), (5), (6), (7), (8), (9)) AS d(n)
    ),
    CustomerNumbers AS
    (
        SELECT TOP (40000)
            ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS Id
        FROM Digits AS a
        CROSS JOIN Digits AS b
        CROSS JOIN Digits AS c
        CROSS JOIN Digits AS d
        CROSS JOIN Digits AS e
    )
    INSERT INTO dbo.Customers (Id, Name, Email, Phone)
    SELECT
        Id,
        CONCAT('Customer ', RIGHT('00000' + CONVERT(varchar(5), Id), 5)),
        CONCAT('customer', RIGHT('00000' + CONVERT(varchar(5), Id), 5), '@example.test'),
        CONCAT('555-', RIGHT('0000000' + CONVERT(varchar(7), Id), 7))
    FROM CustomerNumbers;

    SET IDENTITY_INSERT dbo.Customers OFF;

    COMMIT TRANSACTION;
END TRY
BEGIN CATCH
    SET IDENTITY_INSERT dbo.Customers OFF;
    IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
    THROW;
END CATCH;
