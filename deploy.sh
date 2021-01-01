#!/usr/bin/env bash

pwd

dotnet restore
dotnet tool restore
dotnet paket restore

mkdir -p ./app/public

dotnet fsi ./BuildTyres.fsx

cd ./app

git config --global user.email 'bikehackers@users.noreply.github.com'
git config --global user.name bikehackers

# git clone git@github.com:bikehackers/bikehackers.github.io.git ./public
git clone --branch master "ssh://$GITHUB_TOKEN@github.com/bikehackers/bikehackers.github.io.git" ./public

cd ./public

git fetch origin

rm -r *

cd ..
pwd

yarn install --pure-lockfile
NODE_ENVIRONMENT=production yarn webpack

cd ./public

pwd
git status
git remote -v

git add . && git commit -m 'Deploy' && git push
