@echo on
set servicename=MyService 
net stop %servicename%
sc delete %servicename%
@echo 回车则关闭窗口
pause

 
 