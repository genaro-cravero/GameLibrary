FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY GameLibrary.csproj GameLibrary.csproj
COPY GameLibrary.API/GameLibrary.API.csproj GameLibrary.API/GameLibrary.API.csproj

RUN dotnet restore "GameLibrary.API/GameLibrary.API.csproj"

COPY . .

WORKDIR "/src/GameLibrary.API"
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "GameLibrary.API.dll"]