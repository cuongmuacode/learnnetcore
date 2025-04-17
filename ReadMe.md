dotnet ef migrations add InitLearnNetCore --startup-project .\LearnNetCore.Api\LearnNetCore.Api.csproj --project .\LearnNetCore.EntityFrameworkCore\LearnNetCore.EntityFrameworkCore.csproj --context LearnNetCore.EntityFrameworkCore.ApplicationDbContext

dotnet ef database update --startup-project .\LearnNetCore.Api\LearnNetCore.Api.csproj --project .\LearnNetCore.EntityFrameworkCore\LearnNetCore.EntityFrameworkCore.csproj --context LearnNetCore.EntityFrameworkCore.ApplicationDbContext

dotnet ef mgirations script --startup-project .\LearnNetCore.Api\LearnNetCore.Api.csproj --project .\LearnNetCore.EntityFrameworkCore\LearnNetCore.EntityFrameworkCore.csproj --context LearnNetCore.EntityFrameworkCore.ApplicationDbContext


dotnet ef migrations add Identity --startup-project .\LearnNetCore.Api\LearnNetCore.Api.csproj --project .\LearnNetCore.EntityFrameworkCore\LearnNetCore.EntityFrameworkCore.csproj --context LearnNetCore.EntityFrameworkCore.ApplicationDbContext



dotnet ef migrations add Identity --startup-project .\LearnNetCore.Api\LearnNetCore.Api.csproj --project .\LearnNetCore.Postgresql.Db\LearnNetCore.Postgresql.Db.csproj --context LearnNetCore.EntityFrameworkCore.ApplicationDbContext

dotnet ef database update --startup-project .\LearnNetCore.Api\LearnNetCore.Api.csproj --project .\LearnNetCore.Postgresql.Db\LearnNetCore.Postgresql.Db.csproj --context LearnNetCore.EntityFrameworkCore.ApplicationDbContext
