@echo off
echo ##########################################
echo #
echo # installing new output as stable release
echo #
del ..\spectrafox-stable.exe
del ..\spectrafox-stable_lowpriv.exe
copy ..\output\spectrafox-stable.exe ..\spectrafox-stable.exe
copy ..\output\spectrafox-stable_lowpriv.exe ..\spectrafox-stable_lowpriv.exe
echo #
echo # FINISHED!
echo ##########################################
pause