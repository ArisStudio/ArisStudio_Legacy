# 脚本命令

- 命令用来控制播放逻辑
- 一行为一句命令
- 断点间的命令会同时执行
- 空行会忽略
- /Data/0Txt/demo.txt 是演示文件，可以参考

目前连接关键字为 `&`
请不要在其他位置使用此关键字

# 特殊

## 注释

- 命令: 以 `//` 开头
- 表示改行为注释行
- 示例: `//这一行是命令`

## 断点

- 命令: 以 `=` 开头
- 设置一个断点，在此之前到上一个断点间的命令会同时执行，鼠标点击才会继续执行。
- 示例: `====`

## 跳转 Jump

- 命令: `Jump&{要跳转行数}`
- 跳转到指定行
- 示例: `Jump&14`

# UI 前景

## 小标题

- 命令: `Label&{内容}`
- 播放器左上角通常用来显示地名
- 示例: `Label&三一学院教室`

## 横幅

- 命令: `Banner&{内容}`
- 播放开始时的横幅
- 示例: `Banner&新产品`

## 选择按钮

- 命令:
  - 一个按钮 `Button&{内容}&{要跳转行数}`
  - 两个按钮 `Button&{第一个按钮内容}&{第一个按钮要跳转行数}&{第二个按钮内容}&{第二个按钮要跳转行数}`
  - 三个按钮 `Button&{第一个按钮内容}&{第一个按钮要跳转行数}&{第二个按钮内容}&{第二个按钮要跳转行数}&{第三个按钮内容}&{第三个按钮要跳转行数}`
- 示例: `Button&First Button&32&Seconde Button&14`
- **此命令自带一次断点**

## 文本框

- 设置文本框内容

  - 命令: `Txt&{学生名}&{社团名}&{内容}`
  - 示例: `Txt&日步美&补课部&是<波浪猫>造型的头枕，好软好蓬松啊！`
  - 此命令自带显示文本框效果
  - **此命令自带一次断点**

- 隐藏文本框
  - 命令: `TxtHide`
  - 示例: `TxtHide`

# 背景

## 背景图片

- 加载

  - 命令: `LoadBg&{nameId}&{图片名}`
  - 示例: `LoadBg&ClassRoom&BG_ClassRoom2.jpg`
  - `nameId` 为自定义唯一识别 Id，为后续设置使用
  - 背景文件请放在 `\Data\Image\Background` 文件夹中
  - 图片名 **包括后缀**

- 设置

  - 命令: `SetBg&{nameId}`
  - 示例: `SetBg&ClassRoom`
  - `nameId` 为加载时自定义的唯一识别 Id
  - 此命令自带显示背景效果

- 隐藏
  - 命令: `BgHide`
  - 示例: `BgHide`

## 背景音乐

- 加载

  - 命令: `LoadBgm&{nameId}&{音乐名}`
  - 示例: `LoadBgm&Theme_04&Theme_04.ogg`
  - `nameId` 为自定义唯一识别 Id，为后续设置使用
  - 背景文件请放在 `\Data\Bgm` 文件夹中
  - 音乐名 **包括后缀**
  - 仅支持 ogg,wav 格式

- 设置
  - 命令: `SetBgm&{nameId}`
  - 示例: `SetBgm&Theme_04`
  - `nameId` 为加载时自定义的唯一识别 Id

# 人物

## 加载

- 命令: `LoadSpr&{nameId}&{人物文件名}`
- 示例: `LoadSpr&hihumi&hihumi_spr`
- `nameId` 为自定义唯一识别 Id，为后续设置使用
- 人物文件请放在 `\Data\Spr` 文件夹中
- 人物文件名 **不包括后缀** 以 `_spr` 结尾

## 显示

- 出现
  - 命令: `SprShow&{nameId}`
  - 示例: `SprShow&hihumi`
- 隐藏
  - 命令: `SprHide&{nameId}`
  - 示例: `SprHide&hihumi`
- 高亮效果
  - 命令: `SprHL&{nameId}&{程度0.0~1.0}`
  - 示例: `SprHL&hihumi&0.5`

## 面部表情状态

- 命令: `SprState&{nameId}&{状态名}`
- 示例: `SprState&hihumi&03`
- 如何查看状态名与面部表情对应关系？
  - 以 `/Data` 为根目录开启 web 服务
  - 比如用 vscode 的 Live Server 插件

## 表情

- 命令: `SprEmo&{nameId}&{表情名}`
- 示例: `SprEmo&hihumi&Action`
- 表情名与实际效果对应关系
  - 表格后补
  - 可以先播放 `/Data/0Txt/Test.txt` 查看

## 位置与移动

X 轴从左到右建议 -10，-5，0，5，10 这五个位置

- 设置位置
  - 命令: `SprX&{nameId}&{X坐标}`
  - 示例: `SprX&hihumi&-5`
- 移动到指定位置
  - 命令: `SprMove&{nameId}&{目标X坐标}&{速度}`
  - 示例: `SprMove&hihumi&10&25`
- X 轴抖动
  - 命令: `SprShakeX&{速度}&{幅度}&{周期}`
  - 示例: `SprShakeX&hihumi&20&1&6`
  - 抖动为 Sin 函数
- Y 轴抖动
  - 命令: `SprShakeY&{速度}&{幅度}&{周期}`
  - 示例: `SprShakeY&hihumi&3&-1&1`
  - 抖动为 Sin 函数
- 靠近
  - 命令: `SprClose&{nameId}`
  - 示例: `SprClose&hihumi`
  - 与隐藏出现一起用可实现走到脸前效果
- 后退到默认位置
  - 命令: `SprBack&{nameId}`
  - 示例: `SprBack&hihumi`
