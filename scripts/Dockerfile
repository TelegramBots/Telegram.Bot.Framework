FROM microsoft/aspnetcore:1.1

ARG project
ARG env=Production

ENV PROJECT_NAME ${project}
ENV ASPNETCORE_ENVIRONMENT ${env}

WORKDIR /app
EXPOSE 80
COPY bin/publish .
ENTRYPOINT dotnet "${PROJECT_NAME}.dll"