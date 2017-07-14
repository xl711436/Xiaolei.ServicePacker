@echo on
set servicename=MyService
set exefileename=Xiaolei.ServicePacker.exe
net stop %servicename%
sc delete %servicename%

set p=%~dp0
set p=%p:\=/%
sc create %servicename% binPath= %p%%exefileename%
sc config %servicename% start= auto type= share

net start %servicename%
@echo 回车则关闭窗口
pause
