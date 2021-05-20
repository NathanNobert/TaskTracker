IF EXISTS (SELECT name FROM sysobjects 
		WHERE name = 'TTR_UPDATE_TASK' AND type = 'P')
DROP PROCEDURE TTR_UPDATE_TASK
GO

/*==============================================================================
TTR_UPDATE_TASK

DESCRIPTION: 
Stored Proc to update a row in the TTR_TASK table.

DATE CREATED:2021.05.16
AUTHOR:rforbes

INPUT:
@p_tsk_id     					INT
@p_tsk_code     				VARCHAR(20)
@p_tsk_desc		            	VARCHAR(max)
@p_tsk_active  					bit

OUTPUT:
@p_retval                           INT

RETURNS:
@l_error
--------------------------------------------------------------------------------
Change History
--------------------------------------------------------------------------------
==============================================================================*/
CREATE PROCEDURE TTR_UPDATE_TASK
	@p_tsk_id			INT				    = NULL,
	@p_tsk_code			VARCHAR(20)		    = NULL,
	@p_tsk_desc			VARCHAR(max)		= NULL,
	@p_tsk_active       bit                 = Null,
	@p_retval           INT = 0 OUT
AS

DECLARE @l_error INT

BEGIN TRANSACTION

UPDATE TTR_TASK SET
	TSK_CODE   = @p_tsk_code,
	TSK_DESC   = @p_tsk_desc,
	TSK_ACTIVE = @p_tsk_active

WHERE TSK_ID = @p_tsk_id

SET @l_error = @@ERROR
IF @l_error != 0 GOTO errorhandler

SET @p_retval = @p_tsk_id

COMMIT TRANSACTION
RETURN @l_error

errorhandler:
IF @@TRANCOUNT > 0
	ROLLBACK TRANSACTION
RETURN @l_error
GO

