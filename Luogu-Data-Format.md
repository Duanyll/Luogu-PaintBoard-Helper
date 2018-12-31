# 洛谷数据格式

需要的cookie:`__client_id`和`_uid`

## 发送绘图请求

向`https://luogu.org/paintboard/paint`发送`POST`请求,表单是`x={X}&y={Y}&color={Color}`

需要处理的回应:`没有登录`,`操作过于频繁`

## 获取画板

向`https://luogu.org/paintboard/board`发送`GET`请求,返回表示画板的ascii矩阵,行是翻转的X坐标,列是Y坐标,每个单元格按照32进制解释颜色值

## 颜色表

```js
["rgb(0, 0, 0)", "rgb(255, 255, 255)", "rgb(170, 170, 170)", "rgb(85, 85, 85)", "rgb(254, 211, 199)", "rgb(255, 196, 206)", "rgb(250, 172, 142)", "rgb(255, 139, 131)", "rgb(244, 67, 54)", "rgb(233, 30, 99)", "rgb(226, 102, 158)", "rgb(156, 39, 176)", "rgb(103, 58, 183)", "rgb(63, 81, 181)", "rgb(0, 70, 112)", "rgb(5, 113, 151)", "rgb(33, 150, 243)", "rgb(0, 188, 212)", "rgb(59, 229, 219)", "rgb(151, 253, 220)", "rgb(22, 115, 0)", "rgb(55, 169, 60)", "rgb(137, 230, 66)", "rgb(215, 255, 7)", "rgb(255, 246, 209)", "rgb(248, 203, 140)", "rgb(255, 235, 59)", "rgb(255, 193, 7)", "rgb(255, 152, 0)", "rgb(255, 87, 34)", "rgb(184, 63, 39)", "rgb(121, 85, 72)"]
```

## 画板动态更新

与`wss://ws.luogu.org/ws`建立WebSocket连接,返回格式如下(其实发送的是json)

```yaml
color: 28
type: "paintboard_update"
x: 748
y: 133
_channel: "paintboard"
_channel_param: ""
_ws_type: "server_broadcast"
```

连接时发送的数据(json)

```yaml
channel: "paintboard"
channel_param: ""
type: "join_channel"
```