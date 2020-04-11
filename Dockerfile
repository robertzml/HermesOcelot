#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app

ENV ASPNETCORE_URLS http://+:5000
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["HermesOcelot/HermesOcelot.csproj", "HermesOcelot/"]
RUN dotnet restore "HermesOcelot/HermesOcelot.csproj"
COPY . .
WORKDIR "/src/HermesOcelot"
RUN dotnet build "HermesOcelot.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HermesOcelot.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HermesOcelot.dll"]