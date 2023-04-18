---
sidebar_position: 4
---

# audio commands

- audio commands are used to control the playback of audio.
- This command can start with `bgm`, `sfx`, or **omit** and start directly with nameId
- Sound effects are played once by default, and background music is played in a loop by default
- Currently, only simple command control is supported. If you have complex requirements, it is recommended to use pr and other software for post-processing.
- Unity supports the following audio formats for each platform:
  - Windows: .wav, .ogg

## play

> `{nameId} play`

```txt
audio play
```

## pause

> `{nameId} pause`

```txt
audio pause
```

## stop

> `{nameId} stop`

```txt
audio stop
```

## volume

> `{nameId} v(volume) {value}`

- value is the volume value, ranging from 0-1

```txt
audio v 0.5
```

## fade

> `{nameId} fade {value} {time}`

- value is the volume value, ranging from 0-1, default is `0`
- time is the fade time, in seconds. The default is `1` second

```txt
audio fade
audio fade 0.5
audio fade 0.5 1
```

## loop

> `{nameId} loop`

```txt
audio loop
```

## once

> `{nameId} once`

```txt
audio once
```
