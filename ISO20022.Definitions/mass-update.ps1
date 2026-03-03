$files = Get-ChildItem .\*\*.ps1
for ($i=0; $i -lt $files.Count; $i++) {
   
    Write-Host $files[$i].Name
    
    Set-Location -Path $files[$i].DirectoryName

    $subdirs = Get-ChildItem -Path $files[$i].DirectoryName -Recurse -Directory -Force -ErrorAction SilentlyContinue | Select-Object FullName

    for ($y=0; $y -lt $subdirs.Count; $y++) {
        Remove-Item $subdirs[$y].FullName -Recurse -Force
    }

    start-process -wait powershell $files[$i]
}