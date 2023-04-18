# 演示文本 | demo.txt

- 以下为**中文版**的演示文件

```txt
load spr 日步美 hihumi_spr
load bg 教室 BG_ClassRoom2.jpg
load bgm 背景音乐 Theme_04.ogg

load end

教室 show
banner '新产品'

label '三一学院教室'
背景音乐 play

日步美 show
日步美 emo Action
日步美 state 03
txt '日步美' '补课部' '啊，老师！你来了啊！'

日步美 state 00
txt '日步美' '补课部' '嘿嘿，就跟我在MomoTalk上和你说的一样，我刚拿到新的周边商品呢。'

日步美 state 03
日步美 shakeY 3 -1 1
txt '日步美' '补课部' '请看！锵~'

日步美 state 00
日步美 emo Twinkle
日步美 shakeY 8 1 3
txt '日步美' '补课部' '是〈波浪猫〉造型的头枕，好软好蓬松啊！'

日步美 state 03
txt '日步美' '补课部' '只要有了这个，不管在哪里都能舒服的睡午觉！'

button '......波浪猫？' 't1'

target t1

日步美 state 02
日步美 shakeX 20 1 6
日步美 emo Q
txt '日步美' '补课部' '咦...？老师，难道你不知道波浪猫吗？'

日步美 state 03
日步美 emo Twinkle
日步美 shakeY 8 1 3
txt '日步美' '补课部' '波浪猫是在Momo好友中登场的角色，是一双身体长长的猫，也是佩洛洛独一无二的好朋友!'

日步美 state 00
txt '日步美' '补课部' '一向充满魅力的舞蹈是他的特色，虽然不如佩洛洛舞，但在Momo好友的发烧友之间还是人气很高。'

日步美 move -5 25
日步美 emo Chat
txt '日步美' '补课部' '还有在波浪猫身上的这只小猫头鹰叫做大哥！'

日步美 state 03
日步美 move 0 25
txt '日步美' '补课部' '因为只要大哥只要凝视对方的眼睛，就能得知一切真相，所以是考生之间很受欢迎的护身符。'

日步美 state 00
日步美 move 5 25
txt '日步美' '补课部' '还有这旁边的短尾袋鼠叫做Mr.尼科莱......'

日步美 state 04
日步美 emo Sweat
日步美 shakeX 20 1 6
txt '日步美' '补课部' '哎呀呀，就我自己一个人说个不停。对不起！'

日步美 state 01
日步美 move 0 25
txt '日步美' '补课部' '一聊到Momo好友的事，我就不禁嗨了起来......'

button '没关系，很有意思。' 't2'
target t2

日步美 state 99
日步美 emo Idea
txt '日步美' '补课部' '虽然只要老师觉得好玩就太好了......'

日步美 state 01
txt '日步美' '补课部' '也有很多朋友不喜欢听我聊Momo好友的事。'

日步美 state 04
日步美 shakeX 20 1 6
txt '日步美' '补课部' 'Momo好友的全部只有看起来扭曲又奇特的角色......'

日步美 state 02
日步美 emo Q
txt '日步美' '补课部' '不过这一点不是很有魅力吗？一点都不平凡，拥有其他角色没有的个性，这一点真的很迷人。'

日步美 state 01
txt '日步美' '补课部' '我没有特别之处，平凡得不能再平凡了......所以看到这种个性的角色，总会觉得很憧憬......'

button '我也喜欢平凡的东西' 't3'
target t3

日步美 state 04
日步美 emo Sweat
txt '日步美' '补课部' '哈、哈哈......是吗？听你这么说，总觉得好害羞......'

日步美 state 02
txt '日步美' '补课部' '对了，老师。你要用用看这个波浪猫头枕吗？'

日步美 state 03
日步美 emo Twinkle
txt '日步美' '补课部' '软绵绵的，心情好好~'

日步美 state 01
日步美 hide
wait 1

日步美 close
日步美 show
wait 1

日步美 shakeY 3 -1 1
wait 1

日步美 state 03
日步美 emo Chat
txt '日步美' '补课部' '哈哈，好适合你！好像Momo好友的新角色一样！'

日步美 state 00
txt '日步美' '补课部' '怎么样？光是挂在脖子上就慢慢有睡意了吧？'

日步美 state 04
txt '日步美' '补课部' '...不过这个头枕不能给你。因为我也是好不容易才得到这个东西！'

日步美 state 99
日步美 emo Idea
===

日步美 state 01
日步美 emo Shy
txt '日步美' '补课部' '不过有需要的话...可以偶尔借给你。'
```
