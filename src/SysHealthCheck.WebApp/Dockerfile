#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["src/SysHealthCheck.WebApp/SysHealthCheck.WebApp.csproj", "src/SysHealthCheck.WebApp/"]
RUN dotnet restore "src/SysHealthCheck.WebApp/SysHealthCheck.WebApp.csproj"
COPY . .
WORKDIR "/src/src/SysHealthCheck.WebApp"
RUN dotnet build "SysHealthCheck.WebApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SysHealthCheck.WebApp.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SysHealthCheck.WebApp.dll"]