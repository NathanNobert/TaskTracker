IF EXISTS (SELECT name FROM sysobjects 
		WHERE name = 'TTR_INSERT_USERS' AND type = 'P')
DROP PROCEDURE TTR_INSERT_USERS
GO

/*==============================================================================
TTR_INSERT_USERS

DESCRIPTION: 
Stored Proc to insert a row into the TTR_USERS table.

DATE CREATED:2021.05.10
AUTHOR:nnobert

INPUT:
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

CREATE PROCEDURE TTR_INSERT_USERS
	@p_tus_email				VARCHAR(50) 	= NULL,
	@p_tus_lan_name				VARCHAR(8)		= NULL,
	@p_tus_fname				VARCHAR(50)		= NULL,
	@p_tus_lname                VARCHAR(50)     = Null,
	@p_tus_password             VARCHAR(20)     = Null,
	@p_tus_admin                BIT             = Null,
	@p_retval                   INT = 0 OUT
AS
DECLARE @l_error INT

BEGIN TRANSACTION

INSERT INTO TTR_USERS (
	TUS_EMAIL,
	TUS_LAN_NAME,
	TUS_FNAME,
	TUS_LNAME,
    TUS_PASSWORD,
    TUS_ADMIN
)
VALUES (
	@p_tus_email						,
	@p_tus_lan_name		                ,
	@p_tus_fname						,
	@p_tus_lname                        ,
    @p_tus_password                     ,
    @p_tus_admin
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
