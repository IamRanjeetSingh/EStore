# build stage with dotnet sdk
FROM mcr.microsoft.com/dotnet/sdk:6.0 as build

# set /src folder as working directory
WORKDIR /src

# copy source code
COPY ./ProductService ./ProductService
COPY ./Common/EStore.Common ./Common/EStore.Common

# build the solution in Release configuration
RUN dotnet build ./ProductService/ProductService.sln --configuration Release

# publish the API project in Release configuration in ./ProductService/ProductService.API/bin/publish/Release folder
RUN dotnet publish ./ProductService/ProductService.API/ProductService.API.csproj --configuration Release --output ./ProductService/ProductService.API/bin/publish/Release

# runtime stage with dotnet aspnet runtime
FROM mcr.microsoft.com/dotnet/aspnet:6.0 as runtime

# set /app folder as working directory
WORKDIR /app

# copy all files and folders from build stage's /src/ProductService/ProductService.API/bin/publish/Release folder to current stage's /app folder
COPY --from=build /src/ProductService/ProductService.API/bin/publish/Release .

# expose port 80 of the container, this will be used for HTTP requests
EXPOSE 80

# expose port 443 of the container, this will be used for HTTPS requests
#For HTTPS - Step-1: uncomment below line to expose container's port 443 for listening for incomming HTTPS requests
#EXPOSE 443

# environment variable Environment is created with a default value Development, at runtime this value can be modified like, -e Environment=Production
ENV Environment=Development

# environment variables for specifying the path and password for HTTPS certificate
# For HTTPS - Step-2: uncomment below environment variables and provide the path and password for HTTPS certificate
#ENV ASPNETCORE_Kestrel__Certificates__Default__Path= 
#ENV ASPNETCORE_Kestrel__Certificates__Default__Password= 

# set dotnet ProductService.API.dll as the entrypoint for the container
# For HTTPS - Step-3: comment below entrypoint because it only sets the http port for the api
ENTRYPOINT dotnet ProductService.API.dll --urls "http://+:80" --environment $Environment
# For HTTPS - Step-4: uncomment below entrypoint to use set both http and https ports for the api
#ENTRYPOINT dotnet ProductService.API.dll --urls "http://+:80;https://+:443" --environment $Environment