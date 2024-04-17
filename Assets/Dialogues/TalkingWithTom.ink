INCLUDE settings.ink
-> start_scene

=== start_scene === 
    # {background} : {Сanteen}
    # {reset_characters} : {None}
    # {speaker} : {None}
    Ты заходишь в столовую.
    # AI : DESCRIBE
    # max_tokens : 200
    Опиши в двух коротких предложениях просторную столовую, заполненную шумными людьми.
    # {reset_characters} : Том
    Недалеко от себя ты обнаруживаешь Тома.
    -> start_choices
=== start_choices === 
     + [Поговорить с Томом]->talk_with_tom
     + [Делать всякое]
    # AI : DESCRIBE
    # max_tokens : 150
    Главный герой игры делает что-то в столовой. Придумай и расскажи в двух коротких предложениях.
    ->start_choices
    + [Уйти]->END

=== talk_with_tom
    # {speaker} : Том
    # AI : TALK
    # system : {Tom_description} Ты раздаешь людям кофе. Ты дашь персонажу кофе, но только если он прямо попросит тебя дать ему кофе. Иначе ничего не говори ему о кофе. 
    # max_tokens : 250
    # may_recieve_items : кофе
    Привет!
    # {speaker} : {None}
    Ты заканчиваешь диалог с Томом
    ->start_choices