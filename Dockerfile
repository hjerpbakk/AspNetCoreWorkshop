FROM microsoft/dotnet:2.1-sdk-alpine AS builder
WORKDIR /source

COPY ./*.csproj ./
RUN dotnet restore

COPY ./ ./
RUN dotnet publish "./Workshop.csproj" --output "./dist" --configuration Release --no-restore

FROM microsoft/dotnet:2.1-aspnetcore-runtime-alpine
WORKDIR /app
COPY --from=builder /source/dist .
EXPOSE 80
ENTRYPOINT ["dotnet", "Workshop.dll"]