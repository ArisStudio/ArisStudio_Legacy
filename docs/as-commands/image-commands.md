---
sidebar_position: 2
---

# 图片命令

- 图片命令用于控制图片行为
- 此命令可以 `bg`，`mg`, `fg`, `image` 开头，也可**省略**，及直接以 nameId 开头

## 带有渐变高亮的 显示/隐藏

> `{nameId} show/hide`

```txt
图片 show
图片 hide
```

## 直接 出现/消失

> `{nameId} appear/disappear`

- 旧命令 `showD`/`hideD` 仍然可用，但不推荐使用

```txt
图片 appear
图片 disappear
```

## 透明度变化

> `{nameId} fade {a} {time}`

- a 为透明度，范围为 `0` ~ `1`
- time 为渐变时间，单位为秒。默认为 `0` 秒

```txt
图片 fade 0.5
图片 fade 0.5 1
```

## 位置

### 单独设置 x/y/z 坐标

> `{nameId} x/y/z {value}`

```txt
图片 x/y/z 5
```

### 设置 x/y 坐标

> `{nameId} p(pos,position) {x} {y}`

```txt
图片 pos -5 5
```

## 移动

### x/y 轴移动

> `{nameId} xm(move,move_x)/ym(move_y) {value} {time}`

- time 为移动时间，单位为秒。默认为 `0.5` 秒
- 旧命令 `moveX`/`moveY` 仍然可用，但不推荐使用

```txt
图片 xm/ym 5
图片 xm/ym 5 1

// legacy
图片 moveX/moveY 5
```

### 在 x 和 y 轴移动

> `{nameId} pm(move_pos,move_position) {x} {y} {time}`

- time 为移动时间，单位为秒。默认为 `0.5` 秒

```txt
图片 pm 5 5
图片 pm 5 5 1
```

## 抖动

### x/y 轴抖动

> `{nameId} xs(shake_x)/ys(shake_y) {strength} {time} {vibrato}`

- strength 为抖动强度。
- time 为抖动时间，单位为秒。默认为 `0.5` 秒
- vibrato 为抖动频率。默认为 `6`
- 旧命令 `shakeX`/`shakeY` 仍然可用，但不推荐使用

```txt
图片 xs/ys 0.2
图片 xs/ys 0.2 1
图片 xs/ys 0.2 1 10

// legacy
图片 shakeX/shakeY 0.2
```

### 随机方向抖动

> `{nameId} shake {strength} {time} {vibrato}`

- strength 为抖动强度
- time 为抖动时间，单位为秒。默认为 `0.5` 秒
- vibrato 为抖动频率。默认为 `6`

```txt
图片 shake 0.2
图片 shake 0.2 1
图片 shake 0.2 1 10
```

## 缩放

### 在 x 和 y 轴同时缩放

> `{nameId} scale {value} {time}`

- value 为缩放值
- time 为缩放时间，单位为秒。默认为 `0` 秒

```txt
图片 scale 0.5
图片 scale 0.5 1
```
