#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443


FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Services.GageRR.WebApp/Services.GageRR.WebApp.csproj", "Services.GageRR.WebApp/"]
COPY ["Services.GageRR.Core/Services.GageRR.Core.csproj", "Services.GageRR.Core/"]
RUN dotnet restore "Services.GageRR.WebApp/Services.GageRR.WebApp.csproj"
COPY . .
WORKDIR "/src/Services.GageRR.WebApp"
# Typescript 빌드를 위해 nodejs를 설치한다.
RUN apt-get update && apt-get install -y curl
RUN curl -sL https://deb.nodesource.com/setup_14.x | bash -
RUN apt-get install -y nodejs
RUN npm install

RUN dotnet build "Services.GageRR.WebApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Services.GageRR.WebApp.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Services.GageRR.WebApp.dll"]