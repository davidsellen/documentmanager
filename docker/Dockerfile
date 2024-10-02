# Use the official .NET SDK image to build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the .csproj file and restore as distinct layers
COPY ../src/DocumentApi/DocumentApi.csproj DocumentApi/
RUN dotnet restore "DocumentApi/DocumentApi.csproj"

# Copy the entire source code
COPY ../src/DocumentApi/ DocumentApi/
WORKDIR "/src/DocumentApi"
RUN dotnet build "DocumentApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DocumentApi.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "DocumentApi.dll"]