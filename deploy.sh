#!/usr/bin/env bash

dotnet restore
dotnet tool restore
dotnet paket restore

cd ./app

# git clone git@github.com:bikehackers/bikehackers.github.io.git ./public
git clone --branch master "https://${{ secrets.GITHUB_TOKEN }}@github.com/bikehackers/bikehackers.github.io.git" ./public

cd ./public

git fetch origin

rm -r *

cd ../..

dotnet fsi ./BuildTyres.fsx

cd ./app

yarn install --pure-lockfile
NODE_ENVIRONMENT=production yarn webpack

cd ./public

pwd
git status
git remote -v

git add .
git commit -m 'Deploy'
git push
