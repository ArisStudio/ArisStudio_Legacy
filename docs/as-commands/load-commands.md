---
sidebar_position: 1
---

# 加载命令 | load commands

- 素材资源需要 **先加载到 ArisStudio 中才能使用**。
- 加载命令用于加载素材资源。建议写到 **脚本的最前面**。

## 加载角色 | load character

- spine 角色素材需要放在 `/data/character/spr` 文件夹中
- png 角色素材需要放在 `/data/character/png` 文件夹中

### 加载 spr 角色(默认/通信状态) | load spr character(default/communication state)

> `load spr/sprc(spr_c) {nameId} {sprName}`

```txt
load spr 日步美 hihumi_spr
load sprc 日步美 hihumi_spr
```

#### 加载自定义 spr 角色(默认/通信状态) | load custom spr character(default/communication state)

> `load spr/sprc(spr_c) {nameId} {scale} {idle} {sprName} {imageList}`

```txt
load spr anyone 1 Idle_01 anyone_spr anyone_spr.png,anyone_spr2.png
```

### 加载 png 角色 | load png

- 待完善

## 加载 前景/中景/背景 图片 | load foreground/midground/background image

> `load fg/mg/bg {nameId} {bgName}`

- 前景图片文件需要放在 `/data/image/foreground` 文件夹中
- 中景图片文件需要放在 `/data/image/midground` 文件夹中
- 背景图片文件需要放在 `/data/image/background` 文件夹中

```txt
load fg 幕布 black.png
load md 光之剑 popup11.PNG
load bg 教室 BG_ClassRoom2.jpg
```

## 加载 音效/背景音乐 | load sound effect/background music

> `load sfx/bgm {nameId} {bgmName}`

- 音效文件需要放在 `/data/audio/sfx` 文件夹中
- 背景音乐文件需要放在 `/data/audio/bgm` 文件夹中
- 音效默认播放一次，背景音乐默认循环播放

```txt
load sfx 胜利音效 SE_RetroSuccess_01.wav
load bgm 像素时光 Theme_64.ogg
```
