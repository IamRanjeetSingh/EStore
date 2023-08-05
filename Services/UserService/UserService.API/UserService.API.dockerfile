FROM mcr.microsoft.com/dotnet/sdk:6.0 as build

WORKDIR /src

COPY ./UserService ./UserService
COPY ./Common/EStore.Common ./Common/EStore.Common

RUN dotnet build ./UserService/UserService.sln --configuration Release

RUN dotnet publish ./UserService/UserService.API/UserService.API.csproj --configuration Release --output ./UserService/UserService.API/bin/publish/Release

FROM mcr.microsoft.com/dotnet/aspnet:6.0 as runtime

WORKDIR /app

COPY --from=build /src/UserService/UserService.API/bin/publish/Release .

EXPOSE 80

#For HTTPS - Step-1: uncomment below line to expose container's port 443 for listening for incomming HTTPS requests
#EXPOSE 443

ENV Environment=Development

# environment variables for specifying the path and password for HTTPS certificate
# For HTTPS - Step-2: uncomment below environment variables and provide the path and password for HTTPS certificate
#ENV ASPNETCORE_Kestrel__Certificates__Default__Path=
#ENV ASPNETCORE_Kestrel__Certificates__Default__Password=

# For HTTPS - Step-3: comment below entrypoint because it only sets the http port for the api
ENTRYPOINT dotnet UserService.API.dll --urls "http://+:80" --environment=$Environment

# For HTTPS - Step-4: uncomment below entrypoint to use set both http and https ports for the api
#ENTRYPOINT dotnet UserService.API.dll --urls "http://+:80;https://+:443" --environment $Environment