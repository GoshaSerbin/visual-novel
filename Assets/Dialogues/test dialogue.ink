CONST DEFAULT = 0
CONST ANGRY = 1
CONST SAD = 2

VAR characterExpression = DEFAULT
VAR characterName = ""
Первый диалог
Какое-то описание 

~characterName = "Clara"
~characterExpression = DEFAULT
Ну дарова
бла бла бла

~characterName="John"
~characterExpression = ANGRY
че ты несешь???
епта

~characterName=""
Что делать?
 + [дать леща] -> answer1
 + [сломать колени] -> answer2
 + [поговорить с ии] -> answer3
 
=== answer1 ===
дает леща
~characterName="Clara"
~characterExpression = DEFAULT
ответик
~characterName="John"
хахаха
~characterName="Clara"
лооол
->ending

=== answer2 ===
ломает колени
~characterName="Clara"
ответик тожк
~characterName="John"
ухухуху
~characterName="Clara"
->ending

=== answer3 ===
#AI_TALK
~characterName="John"
Че надо?
ну чтош, хорошо поболтали, малец
А теперь проваливай
 + [ок] -> ending
->ending

=== ending ===
~characterName="Clara"
я еще раз спрашиваю?
 + [Ответ1] -> answer
 + [Ответ2] -> answer
 
=== answer ===
ок
->END