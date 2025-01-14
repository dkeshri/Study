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
