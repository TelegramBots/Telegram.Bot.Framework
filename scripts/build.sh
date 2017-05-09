#!/usr/bin/env bash

rootdir="`pwd`"
# echo "Working dir => `pwd`"

#exit if any command fails
set -e


echo "## Start restore.."

cd "$rootdir"
dotnet restore

echo "#> Restore completed"


echo "## Start build.."

cd "$rootdir"
dotnet build

echo "#> Build completed"
