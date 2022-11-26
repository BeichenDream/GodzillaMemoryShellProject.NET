# GodzillaMemoryShellProject.NET

## 使用场景

当目标存在反序列化漏洞或其它漏洞，我们可以通过加载此项目的类来获得无文件的内存Shell

### 如何使用
  *  普通的ASP.NET 网站(存在aspx/asmx/ashx/soap/...)
  
     [VirtualPathProviderMemoryShell](https://github.com/BeichenDream/GodzillaMemoryShellProject.NET/blob/main/GodzillaMemoryShellProject/VirtualPathProviderMemoryShell.cs)

  *  ASP.NET MVC网站
  
     [HttpWebRouteMemoryShell](https://github.com/BeichenDream/GodzillaMemoryShellProject.NET/blob/main/GodzillaMemoryShellProject/HttpWebRouteMemoryShell.cs)  

  *  System权限(HttpListener复用适用于所有类型)
  
     [HttpListenerMemoryShell](https://github.com/BeichenDream/GodzillaMemoryShellProject.NET/blob/main/GodzillaMemoryShellProject/HttpListenerMemoryShell.cs)    

  
## Web Demo

![image](https://user-images.githubusercontent.com/43266206/204101323-59a677aa-2231-4a57-92c5-83f35a4967e9.png)

## InjectVirtualPathProviderMemoryShell

连接时URL填写目标已存在的可执行脚本，如aspx/asmx/ashx/soap

![image](https://user-images.githubusercontent.com/43266206/204101485-e697e5dc-a759-4107-a28a-da42232f3c51.png)

## InjectHttpListenerMemoryShell

连接时URL填写httpListenerURL，如http://127.0.0.1:80/godzilla/、http://127.0.0.1:80/ews/soap/

![image](https://user-images.githubusercontent.com/43266206/204101496-ac0fc2fe-56e7-4819-81ac-d70c0edc7bdd.png)

## InjectHttpWebRouteMemoryShell

连接时URL填写目标已存在的路由,如http://localhost/Home/About

![image](https://user-images.githubusercontent.com/43266206/204101512-70c58998-0f38-4994-9a1f-22f1c9464d11.png)
