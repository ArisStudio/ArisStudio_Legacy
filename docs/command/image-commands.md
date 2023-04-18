---
sidebar_position: 2
---

# 图片命令 | image commands

- 图片命令用于控制图片行为。

## 背景图片命令 | background image commands

- 建议背景图片大小：`1280 x 900`，与游戏背景图片大小一致。

### 带有渐变效果的显示图片 | show image with gradient effect

- 透明度从 0 到 1。

> `bg {nameId} show`

```txt
bg 教室 show
```

### 带有渐变效果的隐藏图片 | hide image with gradient effect

- 透明度从 1 到 0。

> `bg {nameId} hide`

```txt
bg 教室 hide
```

### 背景图片直接出现 | background image appear directly

> `bg {nameId} appear`

```txt
bg 教室 appear
```

### 背景图片直接消失 | background image disappear directly

> `bg {nameId} disappear`

```txt
bg 教室 disappear
```

### 设置背景图片透明度 | set background image transparency

> `bg {nameId} a/alpha {alpha}`

```txt
bg 教室 alpha 0.5
```

### 设置背景图片线性渐变到指定透明度 | set background image linearly to specified transparency

- time 为渐变时间，单位为秒。

> `bg {nameId} a/alpha {alpha} linear {time}`

```txt
bg 教室 alpha 0.5 linear 1
```

- time 可以省略，默认为 0.5 秒。

```txt
bg 教室 alpha 0.5 linear
```

### 设置背景图片位置 | set background image position

> `bg {nameId} pos/position {x} {y}`

```txt
bg 教室 pos 0 0
```

### 设置背景图片移动到指定位置 | set background image move to specified position

- time 为移动时间，单位为秒。

> `bg {nameId} move {x} {y} {time}`

```txt
bg 教室 move 0 0 1
```

- time 可以省略，默认为 0.5 秒。

```txt
bg 教室 move 0 0
```

### 设置背景图片抖动 | set background image shake

- 待定

### 设置背景图片缩放 | set background image scale

> `bg {nameId} scale {scale}`

```txt
bg 教室 scale 1.5
```

---

## 场景图片命令 | scenario image commands

- 场景图片在人物前面，可以用于显示道具、物品等。

### 显示场景图片 | show scenario image

> `si {nameId} show`

```txt
si 光之剑 show
```

### 隐藏场景图片 | hide scenario image

> `si {nameId} hide`

```txt
si 光之剑 hide
```

### 设置场景图片位置 | set scenario image position

> `si {nameId} pos/position {x} {y}`

```txt
si 光之剑 pos 0 0
```
