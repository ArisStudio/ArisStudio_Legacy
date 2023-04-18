---
sidebar_position: 3
---

# character commands

- Character commands are used to control character behavior
- This command can start with `char`, `spr`, and can be **omitted**, and directly start with nameId

## show/hide with gradient highlight

> `{nameId} show/hide`

```txt
hihumi show
hihumi hide
```

## appear/disappear directly

> `{nameId} appear/disappear`

- The old command `showD`/`hideD` is still available, but not recommended

```txt
hihumi appear
hihumi disappear

// legacy
hihumi showD
hihumi hideD
```

## highlight

> `{nameId} hl(highlight) {highlight} {time}`

- time is the time of the highlight, in seconds. The default is `0` seconds

```txt
hihumi hl 1
hihumi hl 0.5 1
```

## fade

- keep the keyword, currently no effect

## state

- state preview: how to preview character state

> `{nameId} state {state}`

```txt
hihumi state 03
```

## skin

- skin preview: how to preview character skin

> `{nameId} skin {skin}`

```txt
hihumi skin idle
```

## emotion

- emotion preview: [emotion list](/docs/preview/emotion-list)

> `{nameId} emo/emotion {emotion}`

```txt
hihumi emo action
```

## set character animation

> `{nameId} anim/animation {animation}`

```txt
hihumi anim down
```

## position

### set x/y/z coordinate separately

> `{nameId} x/y/z {value}`

```txt
hihumi x/y/z 5
```

### set x/y coordinate

> `{nameId} p(pos,position) {x} {y}`

```txt
hihumi pos -5 5
```

## character move

### move on x/y axis

> `{nameId} xm(move,move_x)/ym(move_y) {value} {time}`

- time is the time of the move, in seconds. The default is `0.5` seconds
- The old command `moveX`/`moveY` is still available, but not recommended

```txt
hihumi xm/ym 5
hihumi xm/ym 5 1

// legacy
hihumi moveX/moveY 5
```

### move on x and y axis

> `{nameId} pm(move_pos,move_position) {x} {y} {time}`

- time is the time of the move, in seconds. The default is `0.5` seconds

```txt
hihumi pm 5 5
hihumi pm 5 5 1
```

## shake

### shake on x/y axis

> `{nameId} xs(shake_x)/ys(shake_y) {strength} {time}`

- strength is the strength of the shake
- time is the time of the shake, in seconds. The default is `0.5` seconds
- The old command `shakeX`/`shakeY` is still available, but not recommended

```txt
hihumi xs/ys 0.2
hihumi xs/ys 0.2 1

// legacy
hihumi shakeX/shakeY 0.2
```

### shake randomly

> `{nameId} shake {strength} {time}`

- strength is the strength of the shake
- time is the time of the shake, in seconds. The default is `0.5` seconds

```txt
hihumi shake 0.2
hihumi shake 0.2 1
```

## scale

### scale on x and y axis

> `{nameId} scale {value} {time}`

- value is the scale value
- time is the scale time, in seconds. The default is `0` seconds

```txt
hihumi scale 0.5
hihumi scale 0.5 1
```

### common scale close/back

> `{nameId} close/back`

```txt
hihumi close
hihumi back
```
