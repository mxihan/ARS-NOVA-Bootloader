Stop-Process -Name preboot -ErrorAction SilentlyContinue
Get-Process -Name slidershim -ErrorAction SilentlyContinue | Stop-Process -ErrorAction Stop
Get-Process | Where-Object {$_.MainWindowTitle -eq "Sequenzia - [InPrivate]"} | Stop-Process -ErrorAction SilentlyContinue
Sleep -Seconds 5
Start-Job -ScriptBlock {
    While ((Get-Process -Name preboot -ErrorAction SilentlyContinue).Length -eq 0) { Sleep -Seconds 1 }
    While ((Select-Window -ProcessName preboot -ErrorAction SilentlyContinue).Length -gt 0) {
        While ((Select-Window -ActiveWindow).ProcessName -eq "preboot") { Sleep -Seconds 5 }
        Select-Window -ProcessName allsbootsimulator | Set-WindowActive
    }
}
Start-Process -Wait -FilePath C:\SEGA\system\preboot\preboot.exe