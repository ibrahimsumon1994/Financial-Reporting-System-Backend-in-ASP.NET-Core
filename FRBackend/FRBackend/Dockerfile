#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
RUN sed -i 's/MinProtocol = TLSv1.2/MinProtocol = TLSv1/g' /etc/ssl/openssl.cnf
RUN sed -i 's/MinProtocol = TLSv1.2/MinProtocol = TLSv1/g' /usr/lib/ssl/openssl.cnf
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["FRBackend/FRBackend.csproj", "FRBackend/"]
COPY ["FREntities/FREntities.csproj", "FREntities/"]
RUN dotnet restore "FRBackend/FRBackend.csproj"
COPY . .
WORKDIR "/src/FRBackend"
RUN dotnet build "FRBackend.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FRBackend.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FRBackend.dll"]