IF OBJECT_ID( 'spSaveCoupon' ) IS NULL
	EXEC ('CREATE PROCEDURE spSaveCoupon AS SET NOCOUNT ON;');
GO

ALTER PROCEDURE spSaveCoupon (
	@Id INT,
	@Code VARCHAR(45),
	@DisccountAmount FLOAT,
	@MinAmount FLOAT,
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
					MinAmount = @MinAmount
				WHERE Id = @Id;
			END;
		ELSE
			BEGIN
				INSERT INTO dbo.Coupon (
					Code, DisccountAmount, MinAmount
				) VALUES (@Code, @DisccountAmount, @MinAmount);
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