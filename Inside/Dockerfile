# syntax=docker/dockerfile:1
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
ENV ASPNETCORE_ENVIRONMENT=Development


FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Inside.csproj", "./"]
RUN dotnet restore "Inside.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "Inside.csproj" -c Release -o /app/build


FROM build AS publish
RUN dotnet publish "Inside.csproj" -c Release -o /app/publish

# Build runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
WORKDIR /root
COPY ./Secrets/Inside.pfx /root/.aspnet/https/
WORKDIR /app
ENTRYPOINT ["dotnet", "Inside.dll"]