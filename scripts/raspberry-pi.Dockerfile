FROM microsoft/dotnet:2-runtime-deps

ARG project
ARG env=Production

ENV PROJECT_NAME ${project}
ENV ASPNETCORE_ENVIRONMENT ${env}
ENV ASPNETCORE_URLS http://+:80
ENV ASPNETCORE_PKG_VERSION 2.0.0

WORKDIR /app
EXPOSE 80
COPY bin/publish .
ENTRYPOINT "/app/${project}"