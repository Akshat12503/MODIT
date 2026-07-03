# Use the official .NET 10 SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app

# Copy the project files and build
COPY . ./
RUN dotnet restore
RUN dotnet publish -c Release -o out

# Use the lighter runtime image for the final deployment
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app/out .

# IMPORTANT: Render dynamically assigns a port via the PORT env variable
ENV ASPNETCORE_URLS=http://+:8080 
EXPOSE 8080

# Replace 'modit-backend.dll' with the actual name of your compiled .dll file
ENTRYPOINT ["dotnet", "modit-backend.dll"]