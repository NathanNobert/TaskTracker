IF EXISTS (SELECT name FROM sysobjects 
		WHERE name = 'TTR_INSERT_USER' AND type = 'P')
DROP PROCEDURE TTR_INSERT_USER
GO

/*==============================================================================
TTR_INSERT_USER

DESCRIPTION: 
Stored Proc to insert a row into the TTR_USERS table.

DATE CREATED:2021.05.10
AUTHOR:nnobert

INPUT:
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

CREATE PROCEDURE TTR_INSERT_USER
	@p_usr_email				VARCHAR(50) 	= NULL,
	@p_usr_lan_name				VARCHAR(8)		= NULL,
	@p_usr_fname				VARCHAR(50)		= NULL,
	@p_usr_lname                VARCHAR(50)     = Null,
	@p_usr_password             VARCHAR(20)     = Null,
	@p_usr_admin                BIT             = Null,
	@p_retval                   INT = 0 OUT
AS
DECLARE @l_error INT

BEGIN TRANSACTION

INSERT INTO TTR_USER (
	USR_EMAIL,
	USR_LAN_NAME,
	USR_FNAME,
	USR_LNAME,
    USR_PASSWORD,
    USR_ADMIN
)
VALUES (
	@p_usr_email						,
	@p_usr_lan_name		                ,
	@p_usr_fname						,
	@p_usr_lname                        ,
    @p_usr_password                     ,
    @p_usr_admin
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
