---
sidebar_position: 1
---

# 加载命令 | load commands

- 素材资源需要 **先加载到 ArisStudio 中才能使用**。
- 加载命令用于加载素材资源。建议写到 **脚本的最前面**。

## 加载角色 | load character

### 加载 spr 角色 | load spr

> `load spr {nameId} {sprName}`

```txt
load spr 日步美 hihumi_spr
```

### 以通信状态加载 spr 角色 | load spr in communication state

> `load sprc/spr_c {nameId} {sprName}`

```txt
load spr_c 日步美 hihumi_spr
```

### 加载自定义 spr 角色

> `load spr {nameId} {scale} {idle} {sprName} {imageList}`

```txt
load spr anyone 1 Idle_01 anyone_spr anyone_spr.png,anyone_spr2.png
```

### 以通信状态加载自定义 spr 角色

> `load sprc/spr_c {nameId} {scale} {idle} {sprName} {imageList}`

```txt
load spr_c anyone 1 Idle_01 anyone_spr anyone_spr.png,anyone_spr2.png
```

## 加载 前景/中景/背景图片 | load foreground/midground/background image

> `load fg/mg/bg {nameId} {bgName}`

```txt
load fg 幕布 black.png
load md 光之剑 popup11.PNG
load bg 教室 BG_ClassRoom2.jpg
```

## 加载 音效/背景音乐 | load sound effect/background music

> `load sfx/bgm {nameId} {bgmName}`

```txt
load sfx 胜利音效 SE_RetroSuccess_01.wav
load bgm 像素时光 Theme_64.ogg
```
