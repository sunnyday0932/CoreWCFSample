FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["CoreWCFClient/CoreWCFClient.csproj", "CoreWCFClient/"]
RUN dotnet restore "CoreWCFClient/CoreWCFClient.csproj"
COPY . .
WORKDIR "/src/CoreWCFClient"
RUN dotnet build "CoreWCFClient.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CoreWCFClient.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CoreWCFClient.dll"]
