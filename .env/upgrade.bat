for f in **/*.csproj; do
upgrade-assistant upgrade "$f" --operation Inplace --targetFramework net6.0  --non-interactive
done