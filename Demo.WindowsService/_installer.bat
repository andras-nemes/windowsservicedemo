@ECHO OFF

echo Installing HttpJobRunner service...
echo -----------------------------------------------
C:\Windows\Microsoft.NET\Framework\v4.0.30319\InstallUtil /i "C:\Program Files (x86)\WindowsServiceDemo\Demo.WindowsService.exe"
C:\Windows\System32\sc.exe failure "HttpJobRunner" reset= 0 actions= restart/1000
echo -----------------------------------------------
echo Done.
pause