FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5000
EXPOSE 5001
ENV ASPNETCORE_URLS="http://+:5000;http://+:5001"

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["UI/BankingApp.UI.csproj", "UI/"]
COPY ["../Application/BankingApp.Application.csproj", "Application/"]
COPY ["../Infrastructure/BankingApp.Infrastructure.csproj", "Infrastructure/"]
COPY ["../Domain/BankingApp.Domain.csproj", "Domain/"]
RUN dotnet restore "UI/BankingApp.UI.csproj"
RUN dotnet restore "Application/BankingApp.Application.csproj"
RUN dotnet restore "Infrastructure/BankingApp.Infrastructure.csproj"
RUN dotnet restore "Domain/BankingApp.Domain.csproj"
COPY . .
WORKDIR "/src/UI"
RUN dotnet build "BankingApp.UI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BankingApp.UI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BankingApp.UI.dll"]
