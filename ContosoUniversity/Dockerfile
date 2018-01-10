FROM microsoft/aspnetcore:2.0-nanoserver-sac2016 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0-nanoserver-sac2016 AS build
ARG connString
WORKDIR /src
COPY ContosoUniversity.CI.sln ./
COPY ContosoUniversity/ContosoUniversity.csproj ContosoUniversity/
COPY ContosoUniversity.IntegrationTests/ContosoUniversity.IntegrationTests.csproj ContosoUniversity.IntegrationTests/
RUN dotnet --info
RUN dotnet restore ContosoUniversity.CI.sln
COPY . .
RUN dotnet build ContosoUniversity.CI.sln -c Release
WORKDIR /src/ContosoUniversity.IntegrationTests
ENV ConnectionStrings:DefaultConnection=${connString}
RUN dotnet xunit -configuration Release -nobuild


FROM build AS publish
WORKDIR /src/ContosoUniversity
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "ContosoUniversity.dll"]

