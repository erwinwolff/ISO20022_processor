$files = Get-ChildItem .\*.xsd
for ($i=0; $i -lt $files.Count; $i++) {
    $dirname = $files[$i].Name -replace ".xsd", ""
    $prepare = $dirname -replace "acmt.", ""
    $prepare = $prepare -replace '\.', "_"
    $namespace = "ISO20022.Acmt_$prepare"

    Write-Host "Processing to $dirname with namespace $namespace"

    New-Item -Path $dirname -Type Directory
    ."C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.8.1 Tools\xsd.exe" /c $files[$i].FullName /out:$dirname /namespace:$namespace /nologo
}