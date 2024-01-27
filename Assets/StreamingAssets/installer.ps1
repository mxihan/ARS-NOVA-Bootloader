Start-Transcript -Path S:\system.installer.log.txt -Append | Out-Null

# Wait for USB Connected
While ((Get-Volume -FileSystemLabel SOS_INS -ErrorAction SilentlyContinue | Format-List).Length -lt 1) {
    Sleep -Seconds 1
}

$password = "BLUE_STEEL_PLATFORM_FOR_SGX"
$letter = (Get-Volume -FileSystemLabel SOS_INS).DriveLetter
$src = "${letter}:\BLUE_STEEL\"

# Platform Update Pack
if (Test-Path -Path "${src}SYSTEM_*.pack") {
    Set-Content -Encoding utf8 -Value "Reading Platform Image..." -Path "C:\SEGA\system\preboot\preboot_Data\StreamingAssets\install.txt"
    Set-Content -Encoding utf8 -Value "Reading Platform Image..." -Path "C:\SEGA\system\base_boot\preboot_Data\StreamingAssets\install.txt"
    Sleep -Seconds 1
    $i = 0
    if (Test-Path -Path "C:\SEGA\update\") {
        Remove-Item -Path "C:\SEGA\update\*" -Force -Recurse -Confirm:$false
    } else {
        New-Item -ItemType Directory -Path "C:\SEGA\update" -Force -Confirm:$false
    }
    Get-ChildItem -Path "${src}SYSTEM_*.pack" | ForEach-Object {
        $i++
        Set-Content -Encoding utf8 -Value "Installing Platform Image... (${i}/$((Get-ChildItem -Path "${src}SYSTEM_*.pack").Count))" -Path "C:\SEGA\system\preboot\preboot_Data\StreamingAssets\install.txt"
        Set-Content -Encoding utf8 -Value "Installing Platform Image... (${i}/$((Get-ChildItem -Path "${src}SYSTEM_*.pack").Count))" -Path "C:\SEGA\system\base_boot\preboot_Data\StreamingAssets\install.txt"
        &'C:\Program Files\7-Zip\7z.exe' x -aoa -p"${password}" -oC:\SEGA\update "${_}"
        if (Test-Path -Path "C:\SEGA\update\post_update.ps1" -ErrorAction SilentlyContinue) {
            Set-Content -Encoding utf8 -Value "Run Pre-Update... (${i}/$((Get-ChildItem -Path "${src}SYSTEM_*.pack").Count))" -Path "C:\SEGA\system\preboot\preboot_Data\StreamingAssets\install.txt"
            Set-Content -Encoding utf8 -Value "Run Pre-Update... (${i}/$((Get-ChildItem -Path "${src}SYSTEM_*.pack").Count))" -Path "C:\SEGA\system\base_boot\preboot_Data\StreamingAssets\install.txt"
            . C:\SEGA\update\post_update.ps1
            Sleep -Seconds 1
        }
    }

    $letter = (Get-Volume -FileSystemLabel SOS_INS).DriveLetter
    if (Test-Path -Path "${letter}:\BLUE_STEEL\INSTALL\" -ErrorAction SilentlyContinue) {
        $i = 0
        Get-ChildItem -Path "${letter}:\BLUE_STEEL\INSTALL\*" | ForEach-Object {
            $i++
            Set-Content -Encoding utf8 -Value "Copy Install Files... (${i}/$((Get-ChildItem -Path "${letter}:\BLUE_STEEL\INSTALL\*").Count))" -Path "C:\SEGA\system\preboot\preboot_Data\StreamingAssets\install.txt"
            Set-Content -Encoding utf8 -Value "Copy Install Files... (${i}/$((Get-ChildItem -Path "${letter}:\BLUE_STEEL\INSTALL\*").Count))" -Path "C:\SEGA\system\base_boot\preboot_Data\StreamingAssets\install.txt"
            Copy-Item -Path "${letter}:\BLUE_STEEL\INSTALL\*" -Destination "C:\SEGA\system\remote_update\" -ErrorAction SilentlyContinue
        }
        
        if ($i -gt 0) {
    	    Set-Content -Encoding utf8 -Value "Disconnect Update USB!" -Path "C:\SEGA\system\preboot\preboot_Data\StreamingAssets\install.txt"
    	    Set-Content -Encoding utf8 -Value "Disconnect Update USB!" -Path "C:\SEGA\system\base_boot\preboot_Data\StreamingAssets\install.txt"
            While ((Get-Volume -FileSystemLabel SOS_INS -ErrorAction SilentlyContinue | Format-List).Length -gt 0) {
                Sleep -Seconds 1
            }
	    }
    }

    if (Test-Path -Path "C:\SEGA\update\system_update.ps1" -ErrorAction SilentlyContinue) {
        Set-Content -Encoding utf8 -Value "Reboot System" -Path "C:\SEGA\system\preboot\preboot_Data\StreamingAssets\install.txt"
        Set-Content -Encoding utf8 -Value "Reboot System" -Path "C:\SEGA\system\base_boot\preboot_Data\StreamingAssets\install.txt"
        Sleep -Seconds 5
    }
    New-Item -ItemType Directory -Path "C:\SEGA\update" -Force -Confirm:$false
    New-Item -ItemType File -Path "C:\SEGA\update\WRITE_ENABLE"           
    shutdown /r /t 2 /f /c "Pending Platform Update"
    Start-Sleep -Seconds 30
}
Stop-Process -Name preboot -ErrorAction SilentlyContinue

Stop-Transcript
# SIG # Begin signature block
# MIIGEgYJKoZIhvcNAQcCoIIGAzCCBf8CAQExCzAJBgUrDgMCGgUAMGkGCisGAQQB
# gjcCAQSgWzBZMDQGCisGAQQBgjcCAR4wJgIDAQAABBAfzDtgWUsITrck0sYpfvNR
# AgEAAgEAAgEAAgEAAgEAMCEwCQYFKw4DAhoFAAQU1o9noN5KX8hD1ytHJzUipp7v
# RMagggOCMIIDfjCCAmagAwIBAgIQJlq0EDKWmKtOwveGVRLWsTANBgkqhkiG9w0B
# AQUFADBFMUMwQQYDVQQDDDpDb2RlIFNpZ25pbmcgLSBBY2FkZW15IENpdHkgUmVz
# ZWFyY2ggUC5TLlIuIChmb3IgTWlzc2xlc3MpMB4XDTIzMTIyOTIzMTMzNVoXDTMw
# MTIyNDA1MDAwMFowRTFDMEEGA1UEAww6Q29kZSBTaWduaW5nIC0gQWNhZGVteSBD
# aXR5IFJlc2VhcmNoIFAuUy5SLiAoZm9yIE1pc3NsZXNzKTCCASIwDQYJKoZIhvcN
# AQEBBQADggEPADCCAQoCggEBANqtipcPEhVWQAUz+KVOBm806ZX0LVp/DV/AW2yJ
# VlBcmT4WP8cIEIay4jU3QZCoVYztQnxI6VUgXsxrpgVfdmWv7Mi1T0yESaicB56k
# c+E+SuJ5QPJiNEOom1cFhpriafjIwjcXazBP1RfqzqP7yfEbN3CxSp4jpRHCfIbq
# agYyVjDqMnyk4iXh2oOY19OHCmHqKCZ0jRlDLpU2RCVMEV0pNewq7O2wn745NxF2
# cm4FP4CU48Zav2LJDwlI2ZA0j5xVJKnwLhRhde0A+N6oFG5GWP709lW9A2EY4tIV
# GKX+FH6BwnXCAedWoiHMa55m0u1KGfxUJc1wC6fnFzEa5mECAwEAAaNqMGgwDgYD
# VR0PAQH/BAQDAgeAMBMGA1UdJQQMMAoGCCsGAQUFBwMDMCIGA1UdEQQbMBmCF2Js
# dWUtc3RlZWwubWlzc2xlc3MubmV0MB0GA1UdDgQWBBSQo+sgAwlIIYWIsEVsvXgF
# dUTz0DANBgkqhkiG9w0BAQUFAAOCAQEAC8jrcbhyQLn2ddfFn2cRk4ONXdp7EDbE
# Eqr+OifivDuUwK5hV2ds9ygbvcuYK2hv1wrixTVElIvQ40qXzSPtbSwlQ86OCGWc
# hrnnI04iAMKFq8m3rxVrePe0rGwJk/NcIXORRQbU8H3yI2UEMAOqCXr8CGcJyxer
# n9jLCxIBQXf8nJ9GU7GydDn/ODFdqCKUhbPAlMCQC4kdQMLPc+6XYnlQ6ex2qSPq
# MT5Josy660b+bUb+PrvhOEG5TH2MP+SCq9hQJZ3viv/ciG1c5x6WnW2HU6WM7XC1
# HKt1v5NZCaCwDD1n0v4RqIODI0Qk9eqmD+45rkrQdZHRZuhwgmBASTGCAfowggH2
# AgEBMFkwRTFDMEEGA1UEAww6Q29kZSBTaWduaW5nIC0gQWNhZGVteSBDaXR5IFJl
# c2VhcmNoIFAuUy5SLiAoZm9yIE1pc3NsZXNzKQIQJlq0EDKWmKtOwveGVRLWsTAJ
# BgUrDgMCGgUAoHgwGAYKKwYBBAGCNwIBDDEKMAigAoAAoQKAADAZBgkqhkiG9w0B
# CQMxDAYKKwYBBAGCNwIBBDAcBgorBgEEAYI3AgELMQ4wDAYKKwYBBAGCNwIBFTAj
# BgkqhkiG9w0BCQQxFgQUsaRtmWmpssI6jH9vyOJgw16HX2IwDQYJKoZIhvcNAQEB
# BQAEggEAkMCgJR+/FJfwzJ4nAzN8/b8h8NQII/cZ1qconKUirEyRSxbj+hwmneaR
# DVbUv3pk/OKTLY4GNJ0en7rb/vZsptfqXYeZwSwmBdn2IEf1A05Pq2TKogP0A2Ob
# DA0VNMAwwOIRdRL5PFWP0y1QelU9SE7MrHR62/WiUeD22HACDQXC/4Sf5v/FoLoA
# onv3YXVTw7X31FPMFDke2Ww2xrHe2Yb06yyvxsCEvVuazxkEF05GVpxlHpDs0Rbz
# RoGDhD6KNOyWQooUQACPe5MXZft2tjsv+Z2ukSuoEEJ4200RH8voaBUw5PssgonG
# 6nfMh5+k2LzFIcLKAWLOB2pTV+EFPQ==
# SIG # End signature block
