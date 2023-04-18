---
sidebar_position: 4
---

# 声音命令

- 声音命令用于控制声音播放。
- 此命令可以 `bgm`，`sfx` 开头，也可**省略**，及直接以 nameId 开头
- 音效默认播放一次，背景音乐默认循环播放
- 目前只有简单命令控制，若有复杂需求建议使用 pr 等软件进行后期处理
- 各平台 Unity 支持音频格式
  - Windows: .wav, .ogg

## 播放

> `{nameId} play`

```txt
声音 play
```

## 暂停

> `{nameId} pause`

```txt
声音 pause
```

## 结束

> `{nameId} stop`

```txt
声音 stop
```

## 音量

> `{nameId} v(volume) {value}`

- value 为音量值，范围 0-1

```txt
声音 v 0.5
```

## 音量渐变

> `{nameId} fade {value} {time}`

- value 为音量值，范围 0-1, 默认为 `0`
- time 为渐变时间，单位为秒。默认为 `1` 秒

```txt
声音 fade
声音 fade 0.5
声音 fade 0.5 1
```

## 循环

> `{nameId} loop`

```txt
声音 loop
```

## 播放一次

> `{nameId} once`

```txt
声音 once
```
