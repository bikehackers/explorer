#!/usr/bin/env bash

dotnet restore
dotnet tool restore
dotnet paket restore

dotnet fsi ./BuildTyres.fsx

cd ./app

yarn webpack -c Release

cd ./public

git init
git remote add origin git@github.com:bikehackers/bikehackers.github.io.git

git add .
git commit -m 'Deploy'
git pull --set-upstream origin master
git push --force
