IF EXISTS (SELECT name FROM sysobjects 
		WHERE name = 'TTR_INSERT_TASKS' AND type = 'P')
DROP PROCEDURE TTR_INSERT_TASKS
GO

/*==============================================================================
TTR_INSERT_TASKS

DESCRIPTION: 
Stored Proc to insert a row into the TTR_USERS table.

DATE CREATED:2021.05.16
AUTHOR:rforbes

INPUT:
@p_tts_code     				VARCHAR(20)
@p_tts_desc		            	VARCHAR(max)
@p_tts_active  					bit

OUTPUT:
@p_retval                           INT

RETURNS:
@l_error
--------------------------------------------------------------------------------
Change History
--------------------------------------------------------------------------------
==============================================================================*/

CREATE PROCEDURE TTR_INSERT_TASKS
	@p_tts_code			VARCHAR(20)		    = NULL,
	@p_tts_desc			VARCHAR(max)		= NULL,
	@p_tts_active       bit                 = Null,
	@p_retval           INT = 0 OUT
AS
DECLARE @l_error INT

BEGIN TRANSACTION

INSERT INTO TTR_TASKS(
	TTS_CODE,
	TTS_DESC,
	TTS_ACTIVE
)
VALUES (
	@p_tts_code	  ,
	@p_tts_desc	  ,
	@p_tts_active			
)

SET @l_error = @@ERROR
IF @l_error != 0 GOTO errorhandler

SET @p_retval = @@IDENTITY

COMMIT TRANSACTION
RETURN @l_error

errorhandler:
IF @@TRANCOUNT > 0
	ROLLBACK TRANSACTION
RETURN @l_error
GO
