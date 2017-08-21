#!/usr/bin/env bash

current_dir="`pwd`"
root_dir="$current_dir/.."

echo; echo "Building 'Sample Echo Bot' for Raspberry Pi..."; echo;

docker_file="Dockerfile.RaspberryPi"
cd "$root_dir/sample/SampleEchoBot" &&
    dotnet publish -o bin/publish -c Release -r debian.8-arm &&
    cp "$current_dir/$docker_file" . &&
    docker build --tag sample-echo-bot --file "$docker_file" --build-arg project2=SampleEchoBot . &&
    rm "$docker_file"


echo; echo "Building 'Sample Games Bot'..."; echo;

docker_file="Dockerfile"
cd "$root_dir/sample/SampleGames" &&
    dotnet publish -o bin/publish -c Release &&
    cp "$current_dir/$docker_file" . &&
    docker build --tag sample-games-bot --file "$docker_file" --build-arg project=SampleGames . &&
    rm "$docker_file"
