#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Activities Bot/Activities Bot.csproj", "Activities Bot/"]
RUN dotnet restore "Activities Bot/Activities Bot.csproj"
COPY . .
WORKDIR "/src/Activities Bot"
RUN dotnet build "Activities Bot.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Activities Bot.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Activities Bot.dll"]