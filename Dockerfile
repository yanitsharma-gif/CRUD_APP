# ---- Build stage ----
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy csproj first (from inside the Practice folder)
COPY Practice/*.csproj ./Practice/
RUN dotnet restore ./Practice/Practice.csproj

# Copy everything else and publish
COPY . .
RUN dotnet publish ./Practice/Practice.csproj -c Release -o /app/publish

# ---- Runtime stage ----
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://0.0.0.0:5000
EXPOSE 5000

ENTRYPOINT ["dotnet", "Practice.dll"]