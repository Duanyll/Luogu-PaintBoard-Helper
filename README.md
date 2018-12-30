# Luogu-PaintBoard-Helper
洛谷冬日滑板辅助脚本

支持多账号使用，添加多个Cookie即可（Cookie必须从画板页面的POST请求中截取）

需要自备.Net Core SDK 2.1,160MB左右

解析Cookie部分使用了来自isGood的代码

## 使用方法

1. 下载并安装.Net Core SDK,然后
```sh
git clone https://github.com/duanyll/luogu-paintboard-helper
cd luogu-paintboard-helper/lgpb
```
2. 编译并运行:dotnet run
3. 图形存在image.png里面,起始坐标(x,y)作为命令行第1,2个参数
4. 输入Cookie，一行一个，空行截止（cookie获得方法：登录洛谷后打开画板页面，按F12查看Network，随便画一个点，点击paint事件，查看Request Headers里面的cookies，复制下来）
5. 断点续传：上一次画到的坐标作为第3,4个参数(可选)（如dotnet run 100 100 1 3从1,3开始画）
