# escape=`

# Installer image
FROM microsoft/mssql-server-windows-express AS installer-env

SHELL ["powershell", "-Command", "$ErrorActionPreference = 'Stop'; $ProgressPreference = 'SilentlyContinue';"]

# Build image
FROM microsoft/dotnet:2.0.5-sdk-2.1.4-nanoserver-1709

# Note: Build image's SHELL is the CMD shell (different than the installer image).

# Set up environment
ENV ASPNETCORE_URLS http://+:80
ENV ASPNETCORE_PKG_VERSION 2.0.5

# Install node, bower, and git
COPY --from=installer-env ["nodejs", "C:\\Program Files\\nodejs"]
RUN ""C:\Program Files\nodejs\npm"" install -g gulp bower
COPY --from=installer-env ["git", "C:\\Program Files\\git"]
RUN setx PATH "%PATH%;C:\Program Files\nodejs;C:\Program Files\git\cmd"

# Warmup up NuGet package cache
RUN mkdir C:\warmup
COPY packagescache.csproj C:\warmup\packagescache.csproj
RUN dotnet restore C:\warmup\packagescache.csproj `
        --source https://api.nuget.org/v3/index.json `
        --verbosity quiet `
    && del /F /S /Q C:\warmupUSER ContainerUser