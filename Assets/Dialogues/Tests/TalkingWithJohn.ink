INCLUDE ../Base.ink
INCLUDE ../Utils/AI.ink
-> start_scene

=== start_scene === 
    # {background} : {Barracks}
    # {reset_characters} : {None}
    # {speaker} : {None}
    Ты заходишь на площадку лагеря повстанцев.
    # AI : DESCRIBE
    # max_tokens : 250
    Площадка лагеря повстанцев находится среди леса за бетонными стенами. Это отдельно стоящее просторное здание, в котором есть тир и манекены для тренировки ближнего боя. Несколько людей упражняются в тире. Продолжи описание двумя короткими предложениями.
    # {reset_characters} : Джон
    На тренировочной площадке ошивается Джон
    -> start_choices
=== start_choices === 
     + [Поговорить с Джоном]->talk_with_john
     + [Делать всякое]
    # AI : DESCRIBE
    # max_tokens : 100
    Опиши как главный герой что-то делает на тренировочной площадке в двух коротких предложениях.
    ->start_choices
    + [Уйти]->END

=== talk_with_john
    # {speaker} : Джон
    # AI : TALK
    # system : {Мужчина_в_форме_description} Тебя зовут Джон.
    # max_tokens : 200
    О чем хочешь поговорить?
    # {speaker} : {None}
    Ты заканчиваешь диалог с Джоном
    ->start_choices
    