IF OBJECT_ID( 'spQueryProducts' ) IS NULL
	EXEC ('CREATE PROCEDURE spQueryProducts AS SET NOCOUNT ON;');
GO

ALTER PROCEDURE spQueryProducts (
	@Id INT = -1
)
AS
/*								
** Name:						spQueryProducts
** Purpose:						
** Fields:						
** Dependencies:				
** Creation Date:				06/Marzo/2025
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
	Name VARCHAR(85),
	Price DECIMAL(18, 2),
	CategoryName VARCHAR(45),
	ImageUrl VARCHAR(1024)
);

SET NOCOUNT ON
	BEGIN TRY

		IF @Id > 0 AND NOT EXISTS (SELECT 1 FROM Product WHERE Id = @Id)
			BEGIN
				INSERT INTO #Response (IsSuccess, Message)
				SELECT 0, 'Doesn''t exists an product with this id';

				GOTO _END;
			END;

		INSERT INTO #Response (
			IsSuccess, Message, Id,
			Name, Price, CategoryName,
			ImageUrl
		) 
		SELECT 1, '', P.Id,
			P.Name, P.Price, C.Name,
			P.ImageUrl
		FROM dbo.Product P
			INNER JOIN dbo.Category C ON C.Id = P.CategoryId
		WHERE @Id IN (-1, P.Id);

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