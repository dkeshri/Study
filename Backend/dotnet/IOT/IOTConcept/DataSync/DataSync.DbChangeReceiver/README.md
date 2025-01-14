# Data-Sync-Receiver

This will subscribe to` rabbitMq` Queue. on message received, Apply changes to `MSSQL` Database

# Installation Steps

## Pre-requisite

> Note: Before running image need some steps to perform

### Step 1: Enable Change tracking on Database

> If not enabled please run below command.

```sql
ALTER DATABASE YourDatabaseName
SET CHANGE_TRACKING = ON 
(CHANGE_RETENTION = 2 DAYS, AUTO_CLEANUP = ON);
```
