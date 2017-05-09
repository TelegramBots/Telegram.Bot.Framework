#!/usr/bin/env bash

rootdir="`pwd`"
# echo "Working dir => `pwd`"

#exit if any command fails
set -e


echo "## Start test.."

dotnet test "$rootdir/test/NetTelegram.Bot.Framework.Tests/NetTelegram.Bot.Framework.Tests.csproj" --configuration Release --list-tests
dotnet test "$rootdir/test/NetTelegram.Bot.Framework.Tests/NetTelegram.Bot.Framework.Tests.csproj" --configuration Release --no-build

echo "#> test completed"
