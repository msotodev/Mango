IF NOT EXISTS (SELECT NAME FROM SYS.DATABASES WHERE NAME = 'MangoCoupon')
	BEGIN
		CREATE DATABASE MangoCoupon;
	END;
GO

USE MangoCoupon;

--> Tables

IF NOT EXISTS (SELECT  1 FROM SYS.TABLES WHERE NAME = 'Coupon')
	BEGIN

		CREATE TABLE Coupon (
			Id INT NOT NULL IDENTITY (1, 1),
			Code VARCHAR(45) NOT NULL
			CONSTRAINT DF_Coupon_Code DEFAULT(''),
			DisccountAmount FLOAT NOT NULL
			CONSTRAINT DF_Coupon_DisccountAmount DEFAULT(0),
			MinAmount FLOAT NOT NULL
			CONSTRAINT DF_Coupon_MinAmount DEFAULT(0),
			Created DATETIME NOT NULL
			CONSTRAINT DF_Coupon_Created DEFAULT('1900-01-01 00:00:00.0'),
			CreatedBy INT NOT NULL
			CONSTRAINT DF_Coupon_CategoryId DEFAULT(-1),
			Updated DATETIME NOT NULL
			CONSTRAINT DF_Coupon_Updated DEFAULT('1900-01-01 00:00:00.0'),
			UpdatedBy INT NOT NULL
			CONSTRAINT DF_Coupon_UpdatedBy DEFAULT(-1)
		);

	END;
GO

--> Store Procedures

IF OBJECT_ID( 'spDeleteCoupon' ) IS NULL
	EXEC ('CREATE PROCEDURE spDeleteCoupon AS SET NOCOUNT ON;');
GO

ALTER PROCEDURE spDeleteCoupon (
	@Id INT,
	@UserId INT,
	@ReturnsObject BIT = 1
)
AS
/*								
** Name:						spDeleteCoupon
** Purpose:						
** Returns:						Si @ReturnsObject
**									1 - Returns an object response (Result, Message, Error)
**									0 - Returns the error
**								
** Date of Creation:			12/February/2025
** Author of Creation:			MSoto
** Date of Modification:		
** Author of Modification:		
** Revision:					0
*/								
BEGIN

DECLARE @Trancount INT = -1,
		@Error INT = -1;

DECLARE @Result AS TABLE (
	Result BIT,
	Message VARCHAR(500),
	Error INT DEFAULT(-1)
);

SET NOCOUNT ON
	BEGIN TRY

		SET @Trancount = @@Trancount;

		IF @Trancount > 0
			SAVE TRANSACTION NewCoupon;
		ELSE
			BEGIN TRANSACTION

		DELETE FROM dbo.Coupon
		WHERE Id = @Id;

		GOTO _FinTran;
		_RollBack:

		IF @Trancount = 0
			ROLLBACK TRANSACTION;
		ELSE IF @Trancount <> -1 AND XACT_STATE() <> -1
			ROLLBACK TRANSACTION NewCoupon;

		IF EXISTS (SELECT 1 FROM @Result)
			BEGIN
				DELETE @Result;
			END;

		GOTO _Fin;
		_FinTran:

		IF @Trancount = 0
			COMMIT TRANSACTION;

	END TRY
	BEGIN CATCH

		SET @Error = @@ERROR;
		PRINT CONCAT('[', ERROR_PROCEDURE(), ' : ', ERROR_MESSAGE(), ' : ', @Error, ']');

		INSERT INTO @Result (Result, Message, Error)
		VALUES (0, ERROR_MESSAGE(), @Error);

		IF @Trancount = 0
			ROLLBACK TRANSACTION
		ELSE IF @Trancount <> -1
			IF XACT_STATE() <> -1
				ROLLBACK TRANSACTION NewCoupon;

	END CATCH

	_Fin:
	SET NOCOUNT OFF;

	IF (@ReturnsObject = 1)
		BEGIN
			IF NOT EXISTS (SELECT 1 FROM @Result)
				BEGIN
					INSERT INTO @Result (
						Result, Message
					)
					SELECT 1, '';
				END;
			
			SELECT * FROM @Result;
		END;
	ELSE RETURN @Error;
END;

GO

IF OBJECT_ID( 'spQueryCoupons' ) IS NULL
	EXEC ('CREATE PROCEDURE spQueryCoupons AS SET NOCOUNT ON;');
GO

ALTER PROCEDURE spQueryCoupons (
	@Id INT = -1
)
AS
/*								
** Name:						spQueryCoupons
** Purpose:						
** Fields:						
** Dependencies:				
** Creation Date:				11/Febrero/2025
** Creation Author:				MSoto
** Modification Date:			
** Modification Author:			
** Revision:					0
*/								
BEGIN

IF EXISTS (
		SELECT 1
			FROM SYS.SYSOBJECTS
		WHERE ID = OBJECT_ID('dbo.#Response')
			AND TYPE = 'U'
	)
	BEGIN
		DROP TABLE #Response;
	END;

CREATE TABLE #Response (
	IsSuccess BIT,
	Message VARCHAR(500),
	Id INT,
	Code VARCHAR(45),
	DisccountAmount FLOAT,
	MinAmount FLOAT
);

SET NOCOUNT ON
	BEGIN TRY

		IF @Id > 0 AND NOT EXISTS (SELECT 1 FROM Coupon WHERE Id = @Id)
			BEGIN
				INSERT INTO #Response (IsSuccess, Message)
				SELECT 0, 'Doesn''t exists an coupon with this id';

				GOTO _END;
			END;

		INSERT INTO #Response (
			IsSuccess, Message, Id,
			Code, DisccountAmount, MinAmount
		) 
		SELECT 1, '', Id,
			Code, DisccountAmount, MinAmount
		FROM dbo.Coupon
		WHERE @Id IN (-1, Id);

		IF NOT EXISTS (SELECT 1 FROM #Response)
			BEGIN
				INSERT INTO #Response (IsSuccess, Message)
				SELECT 0, 'No se encontraron registros.';
			END;

	END TRY
	BEGIN CATCH

		INSERT INTO #Response (IsSuccess, Message)
		SELECT 0, CONCAT(ERROR_PROCEDURE(), ' : ', ERROR_MESSAGE(), ' - ', ERROR_LINE());
		PRINT CONCAT(ERROR_PROCEDURE(), ' : ', ERROR_MESSAGE(), ' - ', ERROR_LINE());

	END CATCH

	_END:

	SELECT * FROM #Response;

	IF EXISTS (
			SELECT 1
				FROM SYS.SYSOBJECTS
			WHERE ID = OBJECT_ID('Tempdb.dbo.#Response')
				AND TYPE = 'U'
		)
		BEGIN
			DROP TABLE #Response;
		END;

END;

GO

IF OBJECT_ID( 'spQueryCouponsByCode' ) IS NULL
	EXEC ('CREATE PROCEDURE spQueryCouponsByCode AS SET NOCOUNT ON;');
GO

ALTER PROCEDURE spQueryCouponsByCode (
	@Code VARCHAR(45) = ''
)
AS
/*								
** Name:						spQueryCouponsByCode
** Purpose:						
** Fields:						
** Dependencies:				
** Creation Date:				11/Febrero/2025
** Creation Author:				MSoto
** Modification Date:			
** Modification Author:			
** Revision:					0
*/								
BEGIN

IF EXISTS (
		SELECT 1
			FROM SYS.SYSOBJECTS
		WHERE ID = OBJECT_ID('dbo.#Response')
			AND TYPE = 'U'
	)
	BEGIN
		DROP TABLE #Response;
	END;

CREATE TABLE #Response (
	IsSuccess BIT,
	Message VARCHAR(500),
	Id INT,
	Code VARCHAR(45),
	DisccountAmount FLOAT,
	MinAmount FLOAT
);

SET NOCOUNT ON
	BEGIN TRY

		IF @Code <> '' AND NOT EXISTS (SELECT 1 FROM Coupon WHERE Code = @Code)
			BEGIN
				INSERT INTO #Response (IsSuccess, Message)
				SELECT 0, 'Doesn''t exists an coupon with this code';

				GOTO _END;
			END;

		INSERT INTO #Response (
			IsSuccess, Message, Id,
			Code, DisccountAmount, MinAmount
		) 
		SELECT 1, '', Id,
			Code, DisccountAmount, MinAmount
		FROM dbo.Coupon
		WHERE @Code IN ('', Code);

		IF NOT EXISTS (SELECT 1 FROM #Response)
			BEGIN
				INSERT INTO #Response (IsSuccess, Message)
				SELECT 0, 'No se encontraron registros.';
			END;

	END TRY
	BEGIN CATCH

		INSERT INTO #Response (IsSuccess, Message)
		SELECT 0, CONCAT(ERROR_PROCEDURE(), ' : ', ERROR_MESSAGE(), ' - ', ERROR_LINE());
		PRINT CONCAT(ERROR_PROCEDURE(), ' : ', ERROR_MESSAGE(), ' - ', ERROR_LINE());

	END CATCH

	_END:

	SELECT * FROM #Response;

	IF EXISTS (
			SELECT 1
				FROM SYS.SYSOBJECTS
			WHERE ID = OBJECT_ID('Tempdb.dbo.#Response')
				AND TYPE = 'U'
		)
		BEGIN
			DROP TABLE #Response;
		END;

END;

GO

IF OBJECT_ID( 'spSaveCoupon' ) IS NULL
	EXEC ('CREATE PROCEDURE spSaveCoupon AS SET NOCOUNT ON;');
GO

ALTER PROCEDURE spSaveCoupon (
	@Id INT,
	@Code VARCHAR(45),
	@DisccountAmount FLOAT,
	@MinAmount FLOAT,
	@UserId INT,
	@ReturnsObject BIT = 1
)
AS
/*								
** Name:						spSaveCoupon
** Purpose:						
** Returns:						Si @ReturnsObject
**									1 - Returns an object response (Result, Message, Error)
**									0 - Returns the error
**								
** Date of Creation:			12/February/2025
** Author of Creation:			MSoto
** Date of Modification:		
** Author of Modification:		
** Revision:					0
*/								
BEGIN

DECLARE @Trancount INT = -1,
		@Error INT = -1;

DECLARE @Result AS TABLE (
	Result BIT,
	Message VARCHAR(500),
	Id INT,
	Code VARCHAR(45),
	DisccountAmount FLOAT,
	MinAmount FLOAT,
	Error INT DEFAULT(-1)
);

SET NOCOUNT ON
	BEGIN TRY

		SET @Trancount = @@Trancount;

		IF @Trancount > 0
			SAVE TRANSACTION NewCoupon;
		ELSE
			BEGIN TRANSACTION

		IF @Id > 0
			BEGIN
				UPDATE dbo.Coupon SET
					Code = @Code,
					DisccountAmount = @DisccountAmount,
					MinAmount = @MinAmount,
					Updated = GETDATE(),
					UpdatedBy = @UserId
				WHERE Id = @Id;
			END;
		ELSE
			BEGIN
				INSERT INTO dbo.Coupon (
					Code, DisccountAmount, MinAmount,
					Created, CreatedBy, Updated,
					UpdatedBy
				) VALUES (
					@Code, @DisccountAmount, @MinAmount,
					GETDATE(), @UserId, GETDATE(),
					@UserId
				);
			END;

		GOTO _FinTran;
		_RollBack:

		IF @Trancount = 0
			ROLLBACK TRANSACTION;
		ELSE IF @Trancount <> -1 AND XACT_STATE() <> -1
			ROLLBACK TRANSACTION NewCoupon;

		IF EXISTS (SELECT 1 FROM @Result)
			BEGIN
				DELETE @Result;
			END;

		GOTO _Fin;
		_FinTran:

		IF @Trancount = 0
			COMMIT TRANSACTION;

	END TRY
	BEGIN CATCH

		SET @Error = @@ERROR;
		PRINT CONCAT('[', ERROR_PROCEDURE(), ' : ', ERROR_MESSAGE(), ' : ', @Error, ']');

		INSERT INTO @Result (Result, Message, Error)
		VALUES (0, ERROR_MESSAGE(), @Error);

		IF @Trancount = 0
			ROLLBACK TRANSACTION
		ELSE IF @Trancount <> -1
			IF XACT_STATE() <> -1
				ROLLBACK TRANSACTION NewCoupon;

	END CATCH

	_Fin:
	SET NOCOUNT OFF;

	IF (@ReturnsObject = 1)
		BEGIN
			IF NOT EXISTS (SELECT 1 FROM @Result)
				BEGIN
					INSERT INTO @Result (
						Result, Message, Id,
						Code, DisccountAmount, MinAmount
					)
					SELECT 1, '', Id, Code, DisccountAmount, MinAmount
						FROM dbo.Coupon
					WHERE Id IN (@Id, SCOPE_IDENTITY());
				END;
			
			SELECT * FROM @Result;
		END;
	ELSE RETURN @Error;
END;

GO