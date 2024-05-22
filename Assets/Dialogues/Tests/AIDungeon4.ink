INCLUDE ../Base.ink
INCLUDE ../Utils/Inventory.ink
INCLUDE ../Utils/Sounds.ink
INCLUDE ../Utils/PlayerPrefs.ink
INCLUDE ../Utils/AI.ink

EXTERNAL GetChoice(choices, num)

VAR narrator_system = "Ты - рассказчик увлекательных историй в ролевой игре. Тема игры: \"Поступление на работу в IT компанию ВК\". Ты виртуозно приспосабливаешься и взаимодействуешь с решениями и действиями игрока. Ты создаешь цепочку реакций, которая может вывести игру на совершенно неожиданный путь или раскрыть новую сторону истории. ОТВЕЧАЙ НА ВОПРОСЫ ОЧЕНЬ КОРОТКО."

VAR prompt = "Скажи кем является главный герой и придумай историю."//"Придумай историю в указанном сеттинге и скажи кем является главный герой."

VAR story = ""
VAR new_story = ""
~AIAnswer("new_story", "{narrator_system}", "{prompt}", 200)

VAR choices = ""
VAR player_choice = ""
VAR background_descr = ""
# {background} : {EarthBG}
Начинаем!
-> dungeon
=== dungeon ===
# {speaker} : {None}
# barrier : new_story
Развиваем историю...
~AIGenerateText("background_descr", "Дай короткое (одно предложение) визуальное описание фона на котором происходят события: {new_story}", 200)
~AIAnswer("choices", "{narrator_system}", "Дай игроку 2 альтернативных варианта действий. Перечисли их парой слов, пронумеровав. История: \"{story}. {new_story}\".", 250)
# barrier : story, background_descr
Анализируем происходящее...
~AIChangeBackground("DungeonBG", "{background_descr}", 1024, 576)
# barrier : choices
Смотрим в будущее...
{new_story}
+ [{GetChoice("{choices}", 1)}]
    ~player_choice = GetChoice("{choices}", 1)
+ [{GetChoice("{choices}", 2)}]
    ~player_choice = GetChoice("{choices}", 2)
-  
~AIAnswer("story", "Перескажи кратко (не более трех предложений) историю.", "{story}. {new_story}", 250)
# barrier : story
Загрузка...
~AIAnswer("new_story", "{narrator_system}", "История: \"{story}\". Действие игрока: \"{player_choice}\". Продолжи приключение.", 200)
->dungeon
->END