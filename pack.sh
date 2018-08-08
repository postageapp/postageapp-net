rm -rf nupkgs

cd ./src

find . -name '*.csproj' | xargs -i dotnet pack {} -c Release /p:BuildNumber=$(printf "%07d" $1) --output ../../nupkgs/beta

cd ../

find ./nupkgs/beta -name '*.nupkg' | xargs -i dotnet nuget push {} -s $2
