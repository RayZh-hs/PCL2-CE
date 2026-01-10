Write-Host "Reset language setting for PCL2CE:";
# Reset language setting in config.v1.json
$dest = "$env:APPDATA\PCLCE_Debug", "$env:APPDATA\PCLCE";
foreach ($d in $dest) {
    if (Test-Path $d) {
        $path = "$d\config.v1.json";
        if (Test-Path $path) {
            Write-Host " - Resetting language in $path";
            $json = Get-Content $path | ConvertFrom-Json;
            # If Language property exists, set it to empty string
            if ($json.PSObject.Properties.Name -contains "Language") {
                $json.Language = "";
                $json | ConvertTo-Json -Depth 100 | Set-Content $path;
            } else {
                Write-Host "   - Language property not found, skipping.";
            }
        }
    }
}
Write-Host "Done.";