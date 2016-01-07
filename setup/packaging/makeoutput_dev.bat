@echo off
echo ##########################################
echo #
echo # installing new output as stable release
echo #
del ..\spectrafox-dev.exe
del ..\spectrafox-dev_lowpriv.exe
copy ..\output\spectrafox-stable.exe ..\spectrafox-dev.exe
copy ..\output\spectrafox-stable_lowpriv.exe ..\spectrafox-dev_lowpriv.exe
echo #
echo # FINISHED!
echo ##########################################
pause