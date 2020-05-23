FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /app

COPY Template.sln .

COPY Template.Data/Template.Data.csproj Template.Data/
COPY Template.Services/Template.Services.csproj Template.Services/
COPY Template.Bot/Template.Bot.csproj Template.Bot/

RUN dotnet restore 

COPY . .

RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/core/runtime:3.1 AS runtime
WORKDIR /app
COPY --from=build /app/out ./

ENTRYPOINT ["dotnet", "Template.Bot.dll"]