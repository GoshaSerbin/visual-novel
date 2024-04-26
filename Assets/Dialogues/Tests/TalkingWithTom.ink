INCLUDE ../Utils/AI.ink
INCLUDE ../Base.ink
-> start_scene

=== start_scene === 
VAR location_description = "Ну и место..." // описание покажется, если не успеет загрузиться ai ответ
~ AIGenerateText("location_description", "Опиши в двух коротких предложениях просторную столовую, заполненную шумными людьми.", 150)
VAR do_something = "Делать нечего..."
~ AIGenerateText("do_something", "Главный герой игры делает что-то в столовой. Придумай и расскажи в двух коротких предложениях.", 150)
    # {background} : {Сanteen}
    # {reset_characters} : {None}
    # {speaker} : {None}
    Ты заходишь в столовую.
    # AI : DESCRIBE
    {location_description}
    # {reset_characters} : Том
    Недалеко от себя ты обнаруживаешь Тома.
    -> start_choices
=== start_choices === 
     + [Поговорить с Томом]->talk_with_tom
     + [Делать всякое]
    {do_something}
    ~ AIGenerateText("do_something", "Главный герой игры делает что-то в столовой. Придумай и расскажи в двух коротких предложениях.", 150)
    ->start_choices
    + [Уйти]->END

=== talk_with_tom
    ~AITalk("{Tom_description} Ты раздаешь людям кофе. Ты дашь персонажу кофе, но только если он прямо попросит тебя дать ему кофе. Иначе ничего не говори ему о кофе.", 250)
    # {speaker} : Том
    # may_recieve_items : кофе
    Привет!
    Увидимся!
    # {speaker} : {None}
    Ты заканчиваешь диалог с Томом
    ->start_choices