# Luogu-PaintBoard-Helper
洛谷冬日滑板辅助脚本

支持多账号使用，添加多个Cookie即可（Cookie必须从画板页面的POST请求中截取）

需要自备.Net Core SDK 2.1,160MB左右

解析Cookie部分使用了来自isGood的代码

## 使用方法

1. 下载并安装.Net Core SDK
2.
```sh
git clone https://github.com/duanyll/luogu-paintboard-helper
cd luogu-paintboard-helper/lgpb
dotnet run
```
3. 图形在data文件里面，起始坐标写死在代码里了（Bad Practice）
4. 输入Cookie，一行一个，空行截止（cookie获得方法：登录洛谷后打开画板页面，按F12查看Network，随便画一个点，点击paint事件，查看Request Headers里面的cookies，复制下来）
5. 断点续传：命令行参数里面输入上一次画到的坐标（如`dotnet run 1 3`从1,3开始画）
