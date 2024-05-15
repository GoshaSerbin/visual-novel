INCLUDE ../Base.ink
INCLUDE ../Utils/AI.ink
INCLUDE ../Utils/Sounds.ink
INCLUDE ../Utils/SceneManagement.ink
INCLUDE ../Utils/PlayerPrefs.ink
-> start_scene

=== start_scene === 
VAR location_description = "Ну и место..." // описание покажется, если не успеет загрузиться ai ответ
~ AIGenerateText("location_description", "Площадка лагеря повстанцев находится среди леса за бетонными стенами. Это отдельно стоящее просторное здание, в котором есть тир и манекены для тренировки ближнего боя. Несколько людей упражняются в тире. Продолжи описание двумя короткими предложениями.", 250)
VAR do_something = "Делать нечего..."
~ AIGenerateText("do_something", "Опиши как главный герой что-то делает на тренировочной площадке в двух коротких предложениях.", 250)
    # {background} : {BarracksBG}
    # {reset_characters} : {None}
    # {speaker} : {None}
    Ты заходишь на площадку лагеря повстанцев.
    Площадка лагеря повстанцев находится среди леса за бетонными стенами.
    {location_description}
    
    # {reset_characters} : Джон
    На тренировочной площадке ошивается Джон
    -> start_choices
=== start_choices === 
     + [Поговорить с Джоном]->talk_with_john
     + [Делать всякое]
        {do_something}
        ~ AIGenerateText("do_something", "Опиши как главный герой что-то делает на тренировочной площадке в двух коротких предложениях.", 250)
    ->start_choices
    + [Уйти]->END

=== talk_with_john
    ~AITalk("{john_description} Тебя зовут Джон.", 250)
    # {speaker} : Джон
    О чем хочешь поговорить?
    До скорого!
    # {speaker} : {None}
    Ты заканчиваешь диалог с Джоном
    ->start_choices
    