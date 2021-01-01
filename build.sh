#!/usr/bin/env bash

dotnet restore
dotnet tool restore
dotnet paket restore

dotnet build

dotnet fsi ./BuildTyres.fsx

cd ./app

yarn install --pure-lockfile
NODE_ENVIRONMENT=production yarn webpack