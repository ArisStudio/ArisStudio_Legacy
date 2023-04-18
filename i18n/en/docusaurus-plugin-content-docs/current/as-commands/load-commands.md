---
sidebar_position: 1
---

# load commands

- **Resources must be loaded into ArisStudio before they can be used**.
- Load commands are used to load resources. It is recommended to write them at the **beginning of the script**.

## load character

- spine character resources need to be placed in the `/data/character/spr` folder
- png character resources need to be placed in the `/data/character/png` folder

### load spr character(default/communication state)

> `load spr/sprc(spr_c) {nameId} {sprName}`

```txt
load spr hihumi hihumi_spr
load sprc hihumi hihumi_spr
```

#### load custom spr character(default/communication state)

> `load spr/sprc(spr_c) {nameId} {scale} {idle} {sprName} {imageList}`

```txt
load spr anyone 1 Idle_01 anyone_spr anyone_spr.png,anyone_spr2.png
```

### load png

- To be improved

## load foreground/midground/background image

> `load fg/mg/bg {nameId} {bgName}`

- Foreground image files need to be placed in the `/data/image/foreground` folder
- Midground image files need to be placed in the `/data/image/midground` folder
- Background image files need to be placed in the `/data/image/background` folder

```txt
load fg 幕布 black.png
load md 光之剑 popup11.PNG
load bg 教室 BG_ClassRoom2.jpg
```

## load sound effect/background music

> `load sfx/bgm {nameId} {bgmName}`

- Sound effect files need to be placed in the `/data/audio/sfx` folder
- Background music files need to be placed in the `/data/audio/bgm` folder
- Sound effects are played once by default, and background music is played in a loop by default

```txt
load sfx 胜利音效 SE_RetroSuccess_01.wav
load bgm 像素时光 Theme_64.ogg
```
