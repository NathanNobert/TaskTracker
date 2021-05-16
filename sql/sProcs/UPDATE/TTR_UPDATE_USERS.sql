IF EXISTS (SELECT name FROM sysobjects 
		WHERE name = 'TTR_UPDATE_USERS' AND type = 'P')
DROP PROCEDURE TTR_UPDATE_USERS
GO

/*==============================================================================
TTR_UPDATE_USERS

DESCRIPTION: 
Stored Proc to update a row in the TTR_USERS table.

DATE CREATED:2021.05.10
AUTHOR:nnobert

INPUT:
@p_tus_id							INT  
@p_tus_email						VARCHAR(50)
@p_tus_lan_name     				VARCHAR(8)
@p_tus_fname						VARCHAR(50)
@p_tus_lname    					VARCHAR(50)
@p_tus_password                     VARCHAR(20)
@p_tus_admin                        VARCHAR(1)

OUTPUT: 
@p_retval                           INT

RETURNS:
@l_error
--------------------------------------------------------------------------------
Change History
--------------------------------------------------------------------------------
==============================================================================*/
CREATE PROCEDURE TTR_UPDATE_USERS
	@p_tus_id					INT  			= NULL,
	@p_tus_email				VARCHAR(50) 	= NULL,
	@p_tus_lan_name				VARCHAR(8)		= NULL,
	@p_tus_fname				VARCHAR(50)		= NULL,
	@p_tus_lname                VARCHAR(50)     = Null,
	@p_tus_password             VARCHAR(20)     = Null,
	@p_tus_admin                BIT             = Null,
	@p_retval INT = 0 OUT
AS

DECLARE @l_error INT

BEGIN TRANSACTION

UPDATE TTR_USERS SET
	TUS_EMAIL = @p_tus_email,
	TUS_LAN_NAME = @p_tus_lan_name,
	TUS_FNAME = @p_tus_fname,
	TUS_LNAME = @p_tus_lname,
	TUS_PASSWORD = @p_tus_password,
	TUS_ADMIN = @p_tus_admin

WHERE TUS_ID = @p_tus_id

SET @l_error = @@ERROR
IF @l_error != 0 GOTO errorhandler

SET @p_retval = @p_tus_id

COMMIT TRANSACTION
RETURN @l_error

errorhandler:
IF @@TRANCOUNT > 0
	ROLLBACK TRANSACTION
RETURN @l_error
GO

