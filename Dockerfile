FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["sample/Quickstart.AspNetCore/Quickstart.AspNetCore.csproj", "sample/Quickstart.AspNetCore/"]
RUN dotnet restore "sample/Quickstart.AspNetCore/Quickstart.AspNetCore.csproj"
COPY . .
WORKDIR "/src/sample/Quickstart.AspNetCore"
RUN dotnet build "Quickstart.AspNetCore.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Quickstart.AspNetCore.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
CMD ASPNETCORE_URLS=http://+:${PORT:-80} dotnet Quickstart.AspNetCore.dll