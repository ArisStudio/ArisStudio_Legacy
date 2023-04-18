---
sidebar_position: 3
---

# 角色命令

- 角色命令用于控制角色行为
- 此命令可以 `char`，`spr` 开头，也可**省略**，及直接以 nameId 开头

## 带有渐变高亮的 显示/隐藏

> `{nameId} show/hide`

```txt
日步美 show
日步美 hide
```

## 直接 出现/消失

> `{nameId} appear/disappear`

- 旧命令 `showD`/`hideD` 仍然可用，但不推荐使用

```txt
日步美 appear
日步美 disappear

// legacy
日步美 showD
日步美 hideD
```

## 高亮

> `{nameId} hl(highlight) {highlight} {time}`

- time 为渐变时间，单位为秒。默认为 `0` 秒

```txt
日步美 hl 1
日步美 hl 0.5 1
```

## 透明度变化

- 保留关键字，目前无效果

## 状态

- 状态预览：如何预览角色状态

> `{nameId} state {state}`

```txt
日步美 state 03
```

## 皮肤

- 皮肤预览：如何预览角色皮肤

> `{nameId} skin {skin}`

```txt
日步美 skin idle
```

## 表情

- 表情预览：[表情列表](/docs/reference/emotion-list)

> `{nameId} emo/emotion {emotion}`

```txt
日步美 emo action
```

## 设置角色动画

> `{nameId} anim/animation {animation}`

```txt
日步美 anim down
```

## 位置

### 单独设置 x/y/z 坐标

> `{nameId} x/y/z {value}`

```txt
日步美 x/y/z 5
```

### 设置 x/y 坐标

> `{nameId} p(pos,position) {x} {y}`

```txt
日步美 pos -5 5
```

## 移动

### x/y 轴移动

> `{nameId} xm(move,move_x)/ym(move_y) {value} {time}`

- time 为移动时间，单位为秒。默认为 `0.5` 秒
- 旧命令 `moveX`/`moveY` 仍然可用，但不推荐使用

```txt
日步美 xm/ym 5
日步美 xm/ym 5 1

// legacy
日步美 moveX/moveY 5
```

### 在 x 和 y 轴移动

> `{nameId} pm(move_pos,move_position) {x} {y} {time}`

- time 为移动时间，单位为秒。默认为 `0.5` 秒

```txt
日步美 pm 5 5
日步美 pm 5 5 1
```

## 抖动

### x/y 轴抖动

> `{nameId} xs(shake_x)/ys(shake_y) {strength} {time}`

- strength 为抖动强度。
- time 为抖动时间，单位为秒。默认为 `0.5` 秒
- 旧命令 `shakeX`/`shakeY` 仍然可用，但不推荐使用

```txt
日步美 xs/ys 0.2
日步美 xs/ys 0.2 1

// legacy
日步美 shakeX/shakeY 0.2
```

### 随机方向抖动

> `{nameId} shake {strength} {time}`

- strength 为抖动强度
- time 为抖动时间，单位为秒。默认为 `0.5` 秒

```txt
日步美 shake 0.2
日步美 shake 0.2 1
```

## 缩放

### 在 x 和 y 轴同时缩放

> `{nameId} scale {value} {time}`

- value 为缩放值
- time 为缩放时间，单位为秒。默认为 `0` 秒

```txt
日步美 scale 0.5
日步美 scale 0.5 1
```

### 常用缩放 靠近/返回

> `{nameId} close/back`

```txt
日步美 close
日步美 back
```
