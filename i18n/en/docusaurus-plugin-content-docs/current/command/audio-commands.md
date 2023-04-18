# 声音命令 | audio commands

- 目前只有简单命令控制，若有复杂需求请使用 pr 等软件进行后期处理。
- 各平台 Unity 支持音频格式
  - Windows: .wav, .ogg

## 设置 | set

> `bgm/sfx set {nameId}`

```txt
bgm set 背景音乐
sfx set 音效
```

## 播放 | play

> `bgm/sfx play`

```txt
bgm/sfx play
```

## 暂停 | pause

> `bgm/sfx pause`

```txt
bgm/sfx pause
```

## 结束 | stop

> `bgm/sfx stop`

```txt
bgm/sfx stop
```

## 设置音量 | set volume

- 音量范围 0-1

> `bgm/sfx v/volume {volume}`

```txt
bgm/sfx v 0.5
```

## 设置循环 | set loop

> `bgm/sfx loop`

```txt
bgm/sfx loop
```

## 设置播放一次 | set play once

> `bgm/sfx once`

```txt
bgm/sfx once
```
