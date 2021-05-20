IF EXISTS (SELECT name FROM sysobjects 
		WHERE name = 'TTR_INSERT_TASK' AND type = 'P')
DROP PROCEDURE TTR_INSERT_TASK
GO

/*==============================================================================
TTR_INSERT_TASK

DESCRIPTION: 
Stored Proc to insert a row into the TTR_TASK table.

DATE CREATED:2021.05.16
AUTHOR:rforbes

INPUT:
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

CREATE PROCEDURE TTR_INSERT_TASK
	@p_tsk_code			VARCHAR(20)		    = NULL,
	@p_tsk_desc			VARCHAR(max)		= NULL,
	@p_tsk_active       bit                 = Null,
	@p_retval           INT = 0 OUT
AS
DECLARE @l_error INT

BEGIN TRANSACTION

INSERT INTO TTR_TASK(
	TSK_CODE,
	TSK_DESC,
	TSK_ACTIVE
)
VALUES (
	@p_tsk_code	  ,
	@p_tsk_desc	  ,
	@p_tsk_active			
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
