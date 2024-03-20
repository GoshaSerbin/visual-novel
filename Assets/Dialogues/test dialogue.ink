VAR characterName = "Bill"
VAR characterExpression = 1
первый диалог. 
~characterName="Bill"
хмхмхм
~characterName="John"
охохох
~characterName=""
Вопрос?
 + [Ответ1] -> answer1
 + [Ответ2] -> answer2
 
=== answer1 ===
~characterName="Bill"
ответик
~characterName="John"
хахаха
~characterName="Bill"
лооол
->ending

=== answer2 ===
~characterName="Bill"
ответик тожк
~characterName="John"
ухухуху
~characterName="Bill"
->ending


=== ending ===
я еще раз спрашиваю?
 + [Ответ1] -> answer
 + [Ответ2] -> answer
 
=== answer ===
ок
->END