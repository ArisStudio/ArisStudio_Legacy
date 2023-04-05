# 特殊命令 | special commands

- 特殊命令用于控制 AriStudio 的行为。

## 注释 | comment

- 注释用于注释，不会被执行。
- 注释需要单独一行。

> 以 `//` 开头即可

```txt
// 这是注释
```

## 断点 | breakpoint

- 断点会暂停等待鼠标点击事件，并在点击后继续执行。

> 以 `=` 开头即可

```txt
===
```

## 等待 | wait

- 等待指定时间后继续执行。
- time 为等待时间，单位为秒。

> `wait {time}`

```txt
wait 1
```

- time 可以省略，默认为 1 秒。

```txt
wait
```

## 标记 | target

- 标记用来记录所在位置，可以用于跳转。

> `target {targetName}`

```txt
target 第一个分支
```

## 跳转 | jump

- 跳转到指定标记处。

> `jump {targetName}`

```txt
jump 第一个分支
```

## 设置自动播放速度 | set auto play speed

- 设置自动播放速度，单位为秒。

> `auto {time}`

```txt
auto 2.5
```

## 切换脚本 | switch script

- 切换脚本。

> `switch {txtFileName}`

```txt
switch demo.txt
```
