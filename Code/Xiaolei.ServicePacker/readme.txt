说明：
1.本程序用于将某文件(exe jar bat dll) 包装成 windows服务
2. windows服务的名称 在  InstallService.bat 文件的第二行 和 UninstallService.bat 文件的第二行  和  Xiaolei.ServicePacker.exe.config 文件中的  MyServiceName 配置项中设置， 三者必须保持一致
3. Xiaolei.ServicePacker.exe.config  文件中 对启动服务和结束服务的操作进行配置，支持两种模式：
	1)  当 StartCmdFileName 和  StopCmdFileName 进行了配置时， 启动和结束服务时，分别执行这两个脚本文件
	2)  当 StartCmdFileName 和  StopCmdFileName 没有 配置时  启动服务时执行 ExecuteFileName ，并以StartPara 作为参数， 结束服务时 当 StopPara 配置了时 
	    执行 ExecuteFileName ，并以StopPara 作为参数 ，当StopPara 没有配置时，结束 KillProcessName 名称的所有进程


测试
1.将tomcat 部署为服务， 试验执行cmd
2.将自己的测试程序部署为服务，测试 开始参数和结束进程
3.将nginx部署为服务，测试 开始参数和结束参数




