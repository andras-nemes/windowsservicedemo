@ECHO OFF

echo Uninstalling HttpJobRunner service...
echo -----------------------------------------------
C:\Windows\Microsoft.NET\Framework\v4.0.30319\InstallUtil /u "C:\Program Files (x86)\WindowsServiceDemo\Demo.WindowsService.exe"
echo -----------------------------------------------
echo Done.
pause