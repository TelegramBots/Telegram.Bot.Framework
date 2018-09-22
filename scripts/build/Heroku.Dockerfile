FROM microsoft/dotnet:2.1.4-aspnetcore-runtime

COPY app /app
WORKDIR /app/

CMD ASPNETCORE_URLS=http://+:${PORT:-80} dotnet Quickstart.AspNetCore.dll