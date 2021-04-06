#!/usr/bin/env bash

set -e

dotnet restore
dotnet tool restore
dotnet paket restore

dotnet build

rm -rf app/public/
mkdir -p app/public/

dotnet fsi ./BuildTyres.fsx

(cd ./app

dotnet femto

dotnet fable .

yarn install --pure-lockfile
NODE_ENVIRONMENT=production yarn webpack)
