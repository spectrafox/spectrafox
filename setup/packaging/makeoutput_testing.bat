@echo off
echo ###############################################
echo #
echo # installing new output as development release
echo #
del ..\spectrafox-testing.exe
del ..\spectrafox-testing_lowpriv.exe
copy ..\output\spectrafox-stable.exe ..\spectrafox-testing.exe
copy ..\output\spectrafox-stable_lowpriv.exe ..\spectrafox-testing_lowpriv.exe
echo #
echo # FINISHED!
echo ##############################################
pause