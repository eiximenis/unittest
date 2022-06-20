#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Bank3.FraudApi/Bank3.FraudApi.csproj", "Bank3.FraudApi/"]
RUN dotnet restore "Bank3.FraudApi/Bank3.FraudApi.csproj"
COPY . .
WORKDIR "/src/Bank3.FraudApi"
RUN dotnet build "Bank3.FraudApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Bank3.FraudApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Bank3.FraudApi.dll"]