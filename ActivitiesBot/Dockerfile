#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
WORKDIR /app

LABEL org.opencontainers.image.source=https://github.com/Misha-133/Activities-Bot

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["ActivitiesBot/ActivitiesBot.csproj", "ActivitiesBot/"]
RUN dotnet restore "ActivitiesBot/ActivitiesBot.csproj"
COPY . .
WORKDIR "/src/Activities Bot"
RUN dotnet build "ActivitiesBot.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ActivitiesBot.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ActivitiesBot.dll"]
