# Data Sync Functionality

## Migration For ChangeTracker

> You need to install below package in `DataSync.Common` Project. After that you can run migration 

```bash
Microsoft.EntityFrameworkCore.Tools
```

> If require then Add this package too in the startup project. `DataSync.DBChangeEmitter`

```bash
dotnet add package Microsoft.EntityFrameworkCore.Design

```
# Code First Approach


## Migration In Visual Studio.
> Note : Please select `DataSync.Common` project in Package Manager Console then run below command.
### Step 1
```bash
Add-Migration DataSyncChangeTracker -Context DataSyncDbContext -OutputDir ".\Migrations\SQL"
```

### Step 2
```bash
Update-Database
```





## Enable Change Tracker in Database

### Step 1: Enable Change tracking in DATABASE

```bash 
ALTER DATABASE STORE
SET CHANGE_TRACKING = ON 
(CHANGE_RETENTION = 2 DAYS, AUTO_CLEANUP = ON);
```


### Step 2: Enable Change tracking on Table 

```bash
ALTER TABLE Customers
ENABLE CHANGE_TRACKING;
```


#### Command to check changes on table 

> Note: lastChangeVersion is an Interger value form where you want changes.

```bash
SELECT * FROM CHANGETABLE(CHANGES Customers, lastChangeVersion) AS CT;
```