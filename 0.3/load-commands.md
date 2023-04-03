# 加载命令 | load commands

- 素材资源需要 **先加载到 as 中才能使用**。
- 加载命令用于加载素材资源。需要写到 **脚本的最前面**。

## 加载 spr 角色 | load spr

> `load spr {nameId} {sprName}`

```txt
load spr 日步美 hihumi_spr
```

## 以通信状态加载 spr 角色 | load spr in communication state

> `load spr_c {nameId} {sprName}`

```txt
load spr_c 日步美 hihumi_spr
```

## 加载自定义 spr 角色

> `load spr {nameId} {scale} {idle} {sprName} {imageList}`

```txt
load spr anyone 1 Idle_01 anyone_spr anyone_spr.png,anyone_spr2.png
```

## 以通信状态加载自定义 spr 角色

> `load spr_c {nameId} {scale} {idle} {sprName} {imageList}`

```txt
load spr_c anyone 1 Idle_01 anyone_spr anyone_spr.png,anyone_spr2.png
```

## 加载 背景图片 | load background image

> `load bg {nameId} {bgName}`

```txt
load bg 教室 BG_ClassRoom2.jpg
```

## 加载 场景图片 | load scenario image

> `load si {nameId} {coverName}`

```txt
load si 光之剑 popup11.PNG
```

## 加载 背景音乐 | load background music

> `load bgm {nameId} {bgmName}`

```txt
load bgm 像素时光 Theme_64.ogg
```

## 加载 背景音效 | Load Sound Effect

> `load sfx {nameId} {seName}`

```txt
load sfx 胜利音效 SE_RetroSuccess_01.wav
```
