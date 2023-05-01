# 文本命令

- 文本命令用于控制文本行为。

## 隐藏文本

> `text hide`

```txt
text hide
```

## 设置打字机效果时间间隔

- 本命令设置的是打字机效果的时间间隔，单位为秒。

> `text interval {interval}`

```txt
text interval 0.02
```

## 隐藏所有文本框

> `text hide`

```txt
text hide
```

## 默认文本框

### 设置文本内容

- 本命令自带断点，即文本显示完毕后，会暂停并等待点击事件。

> `t/txt {content}`

> `t/txt {name} {content}`

> `t/txt {name} {group} {content}`

```txt
txt 日步美 补课部 "是〈波浪猫〉造型的头枕，好软好蓬松啊！"
```

### 设置文本内容并不带断点

- tc - text continue

> `tc {name} {group} {content}`

```txt
tc "日步美" "补课部" "是〈波浪猫〉造型的头枕，好软好蓬松啊！"
```

### 设置文本内容并高亮指定人物

- 本命令显示文本的功能，不需要额外的 `text show` 命令。
- 本命令自带断点，即文本显示完毕后，会暂停并等待点击事件。
- th - text highlight

> `th {nameId} {content}`

> `th {nameId} {name} {content}`

> `th {nameId} {name} {group} {content}`

```txt
th 日步美 "日步美" "补课部" "是〈波浪猫〉造型的头枕，好软好蓬松啊！"
```

## 设置文本内容并高亮指定人物，不带断点

- thc - text highlight continue

> `thc {nameId} {content}`

> `thc {nameId} {name} {content}`

> `thc {nameId} {name} {group} {content}`

```txt
thc 日步美 "日步美" "补课部" "是〈波浪猫〉造型的头枕，好软好蓬松啊！"
```

## 中部文本框

### 设置文本内容

- 本命令自带断点，即文本显示完毕后，会暂停并等待点击事件。

> `mt(middle_text) {content}`

```
mt "这是在中间的文字"
```

### 设置文本内容并不带断点

- mtc - middle text continue

> `mtc {content}`

```
mtc "这是在中间的文字"
```

## 底部文本框

### 设置文本内容

- 本命令自带断点，即文本显示完毕后，会暂停并等待点击事件。

> `bt(bottom_text) {content}`

```
bt "这是在底部的文字"
```

### 设置文本内容并不带断点

- btc - bottom text continue

> `btc {content}`

```
btc "这是在底部的文字"
```

---

## 小标题 | label

- 本命令用于设置小标题，一般用于说明地点。

> `label {text}`

```txt
label 教室
```

## 横幅 | banner

- 本命令用于设置开始横幅。

> `banner {text}`

```txt
banner 一天的开始
```

- 带有两行文本的横幅

> `banner {text} {text}`

```txt
banner 副标题 主标题
```

- 带有三行文本的横幅(暂不可用)

> `banner {text} {text} {text}`

```txt
banner 副标题 小标题 主标题
```
