# 文本命令 | text commands

- 文本命令用于控制文本行为。

## 显示文本 | show text

> `text show`

```txt
text show
```

## 隐藏文本 | hide text

> `text hide`

```txt
text hide
```

## 设置打字机效果时间间隔 | set typewriter effect interval

- 本命令设置的是打字机效果的时间间隔，单位为秒。
- 本命令设置的时间间隔会影响到 `t/txt` 命令的打字机效果。

> `text interval {interval}`

```txt
text interval 0.02
```

## 设置文本内容 | set text content

- 本命令自带显示文本的功能，不需要额外的 `text show` 命令。
- 本命令自带断点，即文本显示完毕后，会暂停并等待点击事件。

> `t/txt {name} {group} {content}`

```txt
txt 日步美 补课部 是〈波浪猫〉造型的头枕，好软好蓬松啊！
```

## 设置文本内容并高亮指定人物 | set text content and highlight character

- 本命令显示文本的功能，不需要额外的 `text show` 命令。
- 本命令自带断点，即文本显示完毕后，会暂停并等待点击事件。
- th - text highlight

> `th {nameId} {name} {group} {content}`

```txt
th 日步美 "日步美" "补课部" "是〈波浪猫〉造型的头枕，好软好蓬松啊！"
```

## 设置文本内容并不带断点 | set text content without breakpoint

- 本命令自带显示文本的功能，不需要额外的 `text show` 命令。
- tc - text continue

> `tc {name} {group} {content}`

```txt
tc "日步美" "补课部" "是〈波浪猫〉造型的头枕，好软好蓬松啊！"
```

## 设置文本内容并高亮指定人物，不带断点 | set text content and highlight character without breakpoint

- 本命令自带显示文本的功能，不需要额外的 `text show` 命令。
- thc - text highlight continue

> `thc {nameId} {name} {group} {content}`

```txt
thc 日步美 "日步美" "补课部" "是〈波浪猫〉造型的头枕，好软好蓬松啊！"
```
