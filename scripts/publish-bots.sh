#!/usr/bin/env bash
set -e

current_dir="`pwd`"
root_dir="${current_dir}/.."

declare -a sample_projects=("SampleEchoBot" "SampleGames")

for project in "${sample_projects[@]}"
do
    echo; echo "@> Build and publish project ${project}"; echo;
    
    cd "${root_dir}/sample/${project}" &&
        rm -rf bin/publish/ &&
        dotnet publish -c Release -o bin/publish/ &&
        cp -v "${current_dir}/Dockerfile" Dockerfile
done

echo; echo "@> Copy nginx Dockerfile to its context"; echo;

cd "${current_dir}" &&
    cp -v nginx.Dockerfile nginx/Dockerfile

echo; echo "@> Build docker compose"; echo;

cd "${current_dir}" &&
    docker-compose build

echo; echo "@> Remove copied Dockerfiles"; echo;

find "${root_dir}/sample" -type f -name Dockerfile -print0 | xargs -0 rm -v
rm -v "${current_dir}/nginx/Dockerfile"

echo; echo "@> Restart and update containers"; echo;

cd "${current_dir}" &&
    docker-compose down &&
    docker-compose up -d
