VAR characterName = ""
VAR characterExpression = 1
первый диалог. 
~characterName="Clara"
хмхмхм
~characterName="John"
охохох
~characterName=""
Вопрос?
 + [Ответ1] -> answer1
 + [Ответ2] -> answer2
 
=== answer1 ===
~characterName="Clara"
ответик
~characterName="John"
хахаха
~characterName="Clara"
лооол
->ending

=== answer2 ===
~characterName="Clara"
ответик тожк
~characterName="John"
ухухуху
~characterName="Clara"
->ending


=== ending ===
~characterName="Clara"
я еще раз спрашиваю?
 + [Ответ1] -> answer
 + [Ответ2] -> answer
 
=== answer ===
ок
->END