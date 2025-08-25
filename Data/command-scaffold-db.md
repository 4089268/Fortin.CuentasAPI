Command to scaffold the db:

```Powershell
dotnet ef dbcontext scaffold "Name=ConnectionStrings:Arquos" Microsoft.EntityFrameworkCore.SqlServer --output-dir Models/ArquosEntities --context-dir Data --namespace Arquos --table Padron.vw_Cat_Padron --force
```