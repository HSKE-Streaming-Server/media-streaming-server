# Dockerfile for establishing our server application in Debug mode

# Establish the normal core sdk as a build environment
FROM mcr.microsoft.com/dotnet/core/sdk AS build-env
WORKDIR /app

# Copy all csproj files and restore 
COPY API/*.csproj ./API/
COPY API/Properties/* ./API/Properties/
COPY Data/*.csproj ./Data/
COPY MediaInput/*.csproj ./MediaInput/
COPY Transcoder/*.csproj ./Transcoder/
RUN dotnet restore API/API.csproj

# Copy the remaining files and build
COPY . ./
RUN dotnet publish API/API.csproj -c Debug -o out 

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
RUN apt-get update && apt-get -y install curl xz-utils
WORKDIR /ffmpeg
RUN curl https://johnvansickle.com/ffmpeg/builds/ffmpeg-git-amd64-static.tar.xz -o ffmpeg.tar.xz
RUN tar -xvf ffmpeg.tar.xz --wildcards '*ffmpeg' -O > /usr/local/bin/ffmpeg
RUN chmod +x /usr/local/bin/ffmpeg
WORKDIR /app
ENV ASPNETCORE_ENVIRONMENT=Development
ENV ASPNETCORE_URLS=http://0.0.0.0:5000;http://[::]:5000
# Copy compiled files from the build environment into this context
COPY --from=build-env /app/out .
# Copy configuration files from ./docker/api
COPY docker/api/. .
EXPOSE 5000/tcp
EXPOSE 5000/udp
ENTRYPOINT ["dotnet", "API.dll"]
