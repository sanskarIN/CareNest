$ErrorActionPreference = "Stop"
dotnet format --verify-no-changes CareNest.sln
dotnet build src/CareNest.Domain/CareNest.Domain.csproj -c Release
dotnet build src/CareNest.Application/CareNest.Application.csproj -c Release
dotnet build src/CareNest.Infrastructure/CareNest.Infrastructure.csproj -c Release
dotnet test tests/CareNest.UnitTests/CareNest.UnitTests.csproj -c Release --no-restore
dotnet test tests/CareNest.IntegrationTests/CareNest.IntegrationTests.csproj -c Release --no-restore
dotnet test tests/CareNest.UiTests/CareNest.UiTests.csproj -c Release --no-restore
