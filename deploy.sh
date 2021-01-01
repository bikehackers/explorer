#!/usr/bin/env bash

dotnet restore
dotnet tool restore
dotnet paket restore

cd ./app

# git config --global user.email 'bikehackers@users.noreply.github.com'
# git config --global user.name bikehackers

pwd
git clone --branch master git@github.com:bikehackers/bikehackers.github.io.git ./public
# git clone --branch master "ssh://$GITHUB_TOKEN@github.com/bikehackers/bikehackers.github.io.git" ./public  || exit 1

cd ./public

git remote -v
git fetch origin

rm -r *

cd ../..
dotnet fsi ./BuildTyres.fsx || exit 1

cd ./app
pwd

yarn install --pure-lockfile
NODE_ENVIRONMENT=production yarn webpack || exit 1

cd ./public

pwd
git status
git remote -v

git add . && git commit -m 'Deploy' && git push
