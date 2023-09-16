function Update-Path {
    param(
        [Parameter(Mandatory)]
        $Directory
    )
    Get-ChildItem –Path $Directory –Recurse -Filter *.csproj |
        Foreach-Object {
            Write-Host $_.FullName
            Invoke-Expression -Command "upgrade-assistant upgrade " + $_.FullName + " --operation Inplace --targetFramework net6.0  --non-interactive"
        }
}