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