# LANMaker
WIP MAUI Blazor simple game distribution application for LAN parties.

![LANMaker screenshot](https://drogoganor.net/lan/lanmaker.png)

# Publishing

Followed [this guide](https://docs.microsoft.com/en-us/dotnet/maui/windows/deployment/overview) to generate a MSIX.

PowerShell:

```
New-SelfSignedCertificate -Type Custom `
                          -Subject "CN=drogoganor" `
                          -KeyUsage DigitalSignature `
                          -FriendlyName "LANMaker Certificate" `
                          -CertStoreLocation "Cert:\CurrentUser\My" `
                          -TextExtension @("2.5.29.37={text}1.3.6.1.5.5.7.3.3", "2.5.29.19={text}")
```

Then:

```
$password = ConvertTo-SecureString -String <YOUR PASSWORD> -Force -AsPlainText
Export-PfxCertificate -cert "Cert:\CurrentUser\My\<YOUR CERT KEY>" -FilePath H:\Development\LANMaker\LANMaker.pfx -Password $password
```

Then using VS 2022 Developer Command Prompt:

```
SignTool sign /fd sha256 /a /f H:\Development\LANMaker\LANMaker.pfx /p <YOUR PASSWORD> H:\Development\LANMaker\LANMaker\bin\Release\net6.0-windows10.0.19041.0\win10-x64\AppPackages\LANMaker_1.0.0.1_Test\LANMaker_1.0.0.1_x64.msix
```

https://docs.microsoft.com/en-us/windows/msix/package/sign-app-package-using-signtool

Then from project directory (not solution):

```
dotnet publish -f net6.0-windows10.0.19041.0 -c Release
```

LANMaker.pfx must be registered in the Trusted People store before you can install with the MSIX.