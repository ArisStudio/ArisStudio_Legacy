# 贡献新内容

1. fork 本仓库
2. 创建一个本地克隆副本
3. 自行下载 [spine-unity 3.8 for Unity 2017.1-2020.3](http://esotericsoftware.com/spine-unity-download/#spine-unity-unitypackages)，并将 `Spine` 添加到项目中。

```mermaid
flowchart TD

MainCamera[Main Camera -100]
debug[Debug -90]
settings[Settings -80]

fg[fa:fa-image Foreground Image -60]
md[fa:fa-image Midground Image -20]
bg[fa:fa-image Background Image 20]

char([fa:fa-user Character 0])

fe[/Foreground Effect -10/]
be[/Background Effect 10/]

btn{{Button -40}}
dialog[[Dialogue -30]]
components[[Components -50]]

subgraph front
fg --- components --- btn --- dialog --- md --- fe
end

MainCamera --- debug --- settings --- front --- char --- back

subgraph back
be --- bg
end
```
