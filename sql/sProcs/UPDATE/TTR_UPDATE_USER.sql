IF EXISTS (SELECT name FROM sysobjects 
		WHERE name = 'TTR_UPDATE_USER' AND type = 'P')
DROP PROCEDURE TTR_UPDATE_USER
GO

/*==============================================================================
TTR_UPDATE_USER

DESCRIPTION: 
Stored Proc to update a row in the TTR_USER table.

DATE CREATED:2021.05.10
AUTHOR:nnobert

INPUT:
@p_usr_id							INT  
@p_usr_email						VARCHAR(50)
@p_usr_lan_name     				VARCHAR(8)
@p_usr_fname						VARCHAR(50)
@p_usr_lname    					VARCHAR(50)
@p_usr_password                     VARCHAR(20)
@p_usr_admin                        VARCHAR(1)

OUTPUT: 
@p_retval                           INT

RETURNS:
@l_error
--------------------------------------------------------------------------------
Change History
--------------------------------------------------------------------------------
==============================================================================*/

CREATE PROCEDURE TTR_UPDATE_USER
	@p_usr_id					INT  			= NULL,
	@p_usr_email				VARCHAR(50) 	= NULL,
	@p_usr_lan_name				VARCHAR(8)		= NULL,
	@p_usr_fname				VARCHAR(50)		= NULL,
	@p_usr_lname                VARCHAR(50)     = Null,
	@p_usr_password             VARCHAR(20)     = Null,
	@p_usr_admin                BIT             = Null,
	@p_retval INT = 0 OUT
AS

DECLARE @l_error INT

BEGIN TRANSACTION

UPDATE TTR_USER SET
	USR_EMAIL = @p_usr_email,
	USR_LAN_NAME = @p_usr_lan_name,
	USR_FNAME = @p_usr_fname,
	USR_LNAME = @p_usr_lname,
	USR_PASSWORD = @p_usr_password,
	USR_ADMIN = @p_usr_admin

WHERE USR_ID = @p_usr_id

SET @l_error = @@ERROR
IF @l_error != 0 GOTO errorhandler

SET @p_retval = @p_usr_id

COMMIT TRANSACTION
RETURN @l_error

errorhandler:
IF @@TRANCOUNT > 0
	ROLLBACK TRANSACTION
RETURN @l_error
GO

