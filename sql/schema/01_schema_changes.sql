use TaskTrackerDev

BEGIN TRY
    BEGIN TRANSACTION
    
    --drop table TTR_TASKS
    CREATE TABLE TTR_TASKS (
        TSK_ID int IDENTITY(1,1) PRIMARY KEY,
        TSK_CODE varchar(20),
        TSK_DESC varchar(max),
        TSK_ACTIVE bit
    )
    insert into TTR_TASKS values ('task1', 'tester task', 1)

	COMMIT TRANSACTION

PRINT 'Database update completed successfully.'
END TRY

BEGIN CATCH
	PRINT '
	!! Database update failed: ' + ERROR_MESSAGE()
	ROLLBACK TRANSACTION
	PRINT '
	Rolled Back'
END CATCH