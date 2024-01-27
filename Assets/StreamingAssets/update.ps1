if (Test-Path -Path "C:\SEGA\update\system_update.ps1" -ErrorAction SilentlyContinue) {
    Start-Transcript -Path S:\system.update.log.txt -Append | Out-Null
    . C:\SEGA\update\system_update.ps1
    Sleep -Seconds 5
    Remove-Item -Path "C:\SEGA\update\*" -Force -Recurse -Confirm:$false -ErrorAction Continue
    Set-Content -Encoding utf8 -Value "Flush Disk Cache" -Path "C:\SEGA\system\platform_update\preboot_Data\StreamingAssets\install.txt" -ErrorAction SilentlyContinue
    Write-VolumeCache C -ErrorAction SilentlyContinue
    Write-VolumeCache S -ErrorAction SilentlyContinue
    Stop-Transcript -ErrorAction SilentlyContinue
    Set-Content -Encoding utf8 -Value "Reboot System" -Path "C:\SEGA\system\platform_update\preboot_Data\StreamingAssets\install.txt" -ErrorAction SilentlyContinue
    Sleep -Seconds 2
	shutdown /r /f /t 2
	Sleep -Seconds 10
}
Stop-Process -Name preboot -ErrorAction SilentlyContinue
# SIG # Begin signature block
# MIIGEgYJKoZIhvcNAQcCoIIGAzCCBf8CAQExCzAJBgUrDgMCGgUAMGkGCisGAQQB
# gjcCAQSgWzBZMDQGCisGAQQBgjcCAR4wJgIDAQAABBAfzDtgWUsITrck0sYpfvNR
# AgEAAgEAAgEAAgEAAgEAMCEwCQYFKw4DAhoFAAQU/hXWNYnLGDzK463mOPPQjXPe
# F0OgggOCMIIDfjCCAmagAwIBAgIQJlq0EDKWmKtOwveGVRLWsTANBgkqhkiG9w0B
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
# BgkqhkiG9w0BCQQxFgQUDye7ILr5WQPljYA+jqV9UYhJnIkwDQYJKoZIhvcNAQEB
# BQAEggEATaOsUYiyMwnzzPTM0+WfPg8aGQdC8hlkvxg2+/GR1IolZPkbD0PodLi2
# jR0UuBOs+NTHe59htS1ww7ja0X/VyC4lVNH/kc4XUpEdTcm+qt/qjnIk317x51nu
# /I5fSOzaMyPdzlBAnqO95e/0FjOIlQ7tD0W+iAOENdni9eGLQdssFOK2dhq0tN+x
# uXnZnp0eMVTpPhugHdAnAUT2riWVKe2665DjZGsPs7NN7J96WqOB0gHpzDAZxnTQ
# IKG8qocNwoiMmIlNr9INi/LHtaHCW/k8IVoKWpYQtQiKaOORr2lkm8Nm8R7CjsD5
# MUA/HkKl9cQ9lGYQToEYoBzosEd4og==
# SIG # End signature block
