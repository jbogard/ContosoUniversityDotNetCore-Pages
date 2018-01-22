FROM microsoft/aspnetcore-build:2.0.5-2.1.4-nanoserver-sac2016
WORKDIR /src
COPY ContosoUniversity.CI.sln ./
COPY ContosoUniversity/ContosoUniversity.csproj ContosoUniversity/
COPY ContosoUniversity.IntegrationTests/ContosoUniversity.IntegrationTests.csproj ContosoUniversity.IntegrationTests/
RUN dotnet restore 
COPY . .

ENTRYPOINT ["powershell"]