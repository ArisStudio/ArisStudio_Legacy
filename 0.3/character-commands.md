# 角色命令 | character commands

- 角色命令用于控制角色行为。
- 此命令可以 `spr`/`char` 开头，也可**省略**，及直接以 nameId 开头。

## 带有渐变效果的显示角色 | show character with fade effect

> `{nameId} show`

```txt
日步美 show
```

## 带有渐变效果的隐藏角色 | hide character with fade effect

> `{nameId} hide`

```txt
日步美 hide
```

## 角色直接出现 | show character appear directly

> `{nameId} appear`

```txt
日步美 appear
```

## 角色直接消失 | hide character disappear directly

> `{nameId} disappear`

```txt
日步美 disappear
```

## 设置角色高亮 | set character highlight

> `{nameId} hl/highlight {highlight}`

```txt
日步美 hl 1
```

## 设置角色线性渐变到指定高亮 | set character linearly to specified highlight

- time 为渐变时间，单位为秒。

> `{nameId} hl/highlight {highlight} linear {time}`

```txt
日步美 hl 1 linear 1
```

- time 可以省略，默认为 0.5 秒。

```txt
日步美 hl 1 linear
```

## 设置角色位置 | set character position

> `{nameId} pos/position {x} {y}`

```txt
日步美 pos 0 0
```

## 设置角色移动到指定位置 | set character move to specified position

- time 为移动时间，单位为秒。

> `{nameId} move {x} {y} {time}`

```txt
日步美 move 0 0 1
```

- time 可以省略，默认为 0.5 秒。

```txt
日步美 move 0 0
```

## 设置角色 z 轴 | set character z-axis

> `{nameId} z {z}`

```txt
日步美 z 1
```

## 设置角色抖动 | set character shake

- 待定

## 设置角色缩放 | set character scale

> `{nameId} scale {scale}`

```txt
日步美 scale 1.5
```

## 设置角色靠近 | set character close

> `{nameId} close`

```txt
日步美 close
```

## 设置角色远离 | set character back

> `{nameId} back`

```txt
日步美 back
```

## 设置角色状态 | set character state

- 状态预览：[如何预览角色状态](/how-to-preview-character-state.md)

> `{nameId} state {state}`

```txt
日步美 state 03
```

## 设置角色表情 | set character emotion

- 表情预览：[表情列表](/emotion-list.md)

> `{nameId} emo/emotion {emotion}`

```txt
日步美 emo action
```

## 设置角色动画 | set character animation

> `{nameId} anim/animation {animation}`

```txt
日步美 anim down
```
