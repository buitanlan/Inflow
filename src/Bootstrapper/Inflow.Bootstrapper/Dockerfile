FROM mcr.microsoft.com/dotnet/aspnet:8.0-preview AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0-preview AS build
WORKDIR /src
COPY ["src/Boostrapper/Inflow.Bootstrapper/Inflow.Bootstrapper.csproj", "src/Boostrapper/Inflow.Bootstrapper/"]
COPY ["src/Modules/Customers/Inflow.Modules.Customers.Api/Inflow.Modules.Customers.Api.csproj", "src/Modules/Customers/Inflow.Modules.Customers.Api/"]
COPY ["src/Modules/Customers/Inflow.Modules.Customers.Core/Inflow.Modules.Customers.Core.csproj", "src/Modules/Customers/Inflow.Modules.Customers.Core/"]
RUN dotnet restore "src/Bootstrapper/Inflow.Bootstrapper/Inflow.Bootstrapper.csproj"
COPY . .
WORKDIR "/src/src/Boostrapper/Inflow.Bootstrapper"
RUN dotnet build "Inflow.Bootstrapper.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Inflow.Bootstrapper.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Inflow.Bootstrapper.dll"]
