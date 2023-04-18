---
sidebar_position: 1
---

// To Do

# demo_en.txt

- The following is the **English version** of the demo file, please switch languages for other versions

```txt
load spr hihumi hihumi_spr
load bg ClassRoom2 BG_ClassRoom2.jpg
load Bgm Theme_04 Theme_04.ogg

load end

ClassRoom2 show
banner 'New Product'

label 'Trinity College Classroom'
Theme_04 play

hihumi show
hihumi emo Action
hihumi state 03
txt Hifumi 'Remedial Department' 'Ah, Sensei! You are here!'

hihumi state 00
txt Hifumi 'Remedial Department' 'Ne, just like what I told Sensei on MomoTalk, I just got new peripheral products.'

hihumi state 03
hihumi shakeY 3 -1 1
txt Hifumi 'Remedial Department' 'Look! Ja ja~'

hihumi state 00
hihumi emo Twinkle
hihumi shakeY 8 1 3
txt Hifumi 'Remedial Department' 'It\'s a headrest in the shape of a "wave cat", so soft and fluffy!'

hihumi state 03
txt Hifumi 'Remedial Department' 'As long as you have this, you can take a nap comfortably no matter where you are!'

button '......波浪猫？' 't1'

target t1

hihumi state 02
hihumi shakeX 20 1 6
hihumi emo Q
txt 'hihumi' '补课部' '咦...？老师，难道你不知道波浪猫吗？'

hihumi state 03
hihumi emo Twinkle
hihumi shakeY 8 1 3
txt 'hihumi' '补课部' '波浪猫是在Momo好友中登场的角色，是一双身体长长的猫，也是佩洛洛独一无二的好朋友!'

hihumi state 00
txt 'hihumi' '补课部' '一向充满魅力的舞蹈是他的特色，虽然不如佩洛洛舞，但在Momo好友的发烧友之间还是人气很高。'

hihumi move -5 25
hihumi emo Chat
txt 'hihumi' '补课部' '还有在波浪猫身上的这只小猫头鹰叫做大哥！'

hihumi state 03
hihumi move 0 25
txt 'hihumi' '补课部' '因为只要大哥只要凝视对方的眼睛，就能得知一切真相，所以是考生之间很受欢迎的护身符。'

hihumi state 00
hihumi move 5 25
txt 'hihumi' '补课部' '还有这旁边的短尾袋鼠叫做Mr.尼科莱......'

hihumi state 04
hihumi emo Sweat
hihumi shakeX 20 1 6
txt 'hihumi' '补课部' '哎呀呀，就我自己一个人说个不停。对不起！'

hihumi state 01
hihumi move 0 25
txt 'hihumi' '补课部' '一聊到Momo好友的事，我就不禁嗨了起来......'

button '没关系，很有意思。' 't2'
target t2

hihumi state 99
hihumi emo Idea
txt 'hihumi' '补课部' '虽然只要老师觉得好玩就太好了......'

hihumi state 01
txt 'hihumi' '补课部' '也有很多朋友不喜欢听我聊Momo好友的事。'

hihumi state 04
hihumi shakeX 20 1 6
txt 'hihumi' '补课部' 'Momo好友的全部只有看起来扭曲又奇特的角色......'

hihumi state 02
hihumi emo Q
txt 'hihumi' '补课部' '不过这一点不是很有魅力吗？一点都不平凡，拥有其他角色没有的个性，这一点真的很迷人。'

hihumi state 01
txt 'hihumi' '补课部' '我没有特别之处，平凡得不能再平凡了......所以看到这种个性的角色，总会觉得很憧憬......'

button '我也喜欢平凡的东西' 't3'
target t3

hihumi state 04
hihumi emo Sweat
txt 'hihumi' '补课部' '哈、哈哈......是吗？听你这么说，总觉得好害羞......'

hihumi state 02
txt 'hihumi' '补课部' '对了，老师。你要用用看这个波浪猫头枕吗？'

hihumi state 03
hihumi emo Twinkle
txt 'hihumi' '补课部' '软绵绵的，心情好好~'

hihumi state 01
hihumi hide
wait 1

hihumi close
hihumi show
wait 1

hihumi shakeY 3 -1 1
wait 1

hihumi state 03
hihumi emo Chat
txt 'hihumi' '补课部' '哈哈，好适合你！好像Momo好友的新角色一样！'

hihumi state 00
txt 'hihumi' '补课部' '怎么样？光是挂在脖子上就慢慢有睡意了吧？'

hihumi state 04
txt 'hihumi' '补课部' '...不过这个头枕不能给你。因为我也是好不容易才得到这个东西！'

hihumi state 99
hihumi emo Idea
===

hihumi state 01
hihumi emo Shy
txt 'hihumi' '补课部' '不过有需要的话...可以偶尔借给你。'
```
