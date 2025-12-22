FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src
COPY ["src/ANLASH.Web.Host/ANLASH.Web.Host.csproj", "src/ANLASH.Web.Host/"]
COPY ["src/ANLASH.Web.Core/ANLASH.Web.Core.csproj", "src/ANLASH.Web.Core/"]
COPY ["src/ANLASH.Application/ANLASH.Application.csproj", "src/ANLASH.Application/"]
COPY ["src/ANLASH.Core/ANLASH.Core.csproj", "src/ANLASH.Core/"]
COPY ["src/ANLASH.EntityFrameworkCore/ANLASH.EntityFrameworkCore.csproj", "src/ANLASH.EntityFrameworkCore/"]
WORKDIR "/src/src/ANLASH.Web.Host"
RUN dotnet restore 

WORKDIR /src
COPY ["src/ANLASH.Web.Host", "src/ANLASH.Web.Host"]
COPY ["src/ANLASH.Web.Core", "src/ANLASH.Web.Core"]
COPY ["src/ANLASH.Application", "src/ANLASH.Application"]
COPY ["src/ANLASH.Core", "src/ANLASH.Core"]
COPY ["src/ANLASH.EntityFrameworkCore", "src/ANLASH.EntityFrameworkCore"]
WORKDIR "/src/src/ANLASH.Web.Host"
RUN dotnet publish -c Release -o /publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0
EXPOSE 80
WORKDIR /app
COPY --from=build /publish .
ENTRYPOINT ["dotnet", "ANLASH.Web.Host.dll"]
