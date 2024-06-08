INCLUDE ../Base.ink
INCLUDE ../Utils/Inventory.ink
INCLUDE ../Utils/Sounds.ink
INCLUDE ../Utils/PlayerPrefs.ink
INCLUDE ../Utils/AI.ink
INCLUDE ../Utils/SceneManagement.ink
INCLUDE ../Utils/Methods.ink

VAR game_theme = "Пиратство"
~AskPlayer("game_theme")
~PlayMusic("mashinarium")
# {background} : {EarthBG}
Укажи тему игры, например, "Пиратство", или "Поступление на работу в IT компанию ВК".

VAR narrator_system = ""
~narrator_system = "Ты - рассказчик увлекательных историй в ролевой игре. Тема игры: {game_theme}. Ты виртуозно приспосабливаешься и взаимодействуешь с решениями и действиями игрока. Ты создаешь цепочку реакций, которая может вывести игру на неожиданный путь или раскрыть новую сторону истории. Не бойся конкретики. ОТВЕЧАЙ НА ВОПРОСЫ ОЧЕНЬ КОРОТКО."

VAR prompt = "Скажи кем является главный герой и придумай историю."//"Придумай историю в указанном сеттинге и скажи кем является главный герой."

VAR story = ""
VAR new_story = ""
VAR story_intro = ""
~AIAnswer("new_story", "{narrator_system}", "{prompt}", 200)


VAR choices = ""
VAR player_choice = ""
VAR background_descr = ""
Начинаем!
-> dungeon
=== dungeon ===
# {speaker} : {None}
# barrier : new_story
Развиваем историю...
{TURNS_SINCE(->dungeon) == 0 : 
    ~story_intro = new_story
}
~AIGenerateText("background_descr", "Дай короткое (одно предложение) визуальное описание фона на котором происходят события: {new_story}", 200)
~AIAnswer("choices", "{narrator_system}", "История: \"{story}. {new_story}\". Дай игроку 2 различных побудительных варианта действий на основе истории. Перечисли их парой слов, пронумеровав.", 250)
# barrier : story, background_descr
Анализируем происходящее...
~AIChangeBackground("DungeonBG", "{background_descr}", 1024, 576)
# barrier : choices
Смотрим в будущее...
VAR choice1 = ""
VAR choice2 = ""
~choice1 = GetChoice("{choices}", 1)
~choice2 = GetChoice("{choices}", 2)
{new_story}
+ [{choice1}]
    ~player_choice = GetChoice("{choices}", 1)
+ [{choice2}]
    ~player_choice = GetChoice("{choices}", 2)
+ [Свой вариант]
    ~AskPlayer("player_choice")
    Укажи свой вариант.
-  
~AIAnswer("story", "Перескажи кратко (не более трех предложений) историю.", "{story}. {new_story}", 200)
# barrier : story
Загрузка...
~AIAnswer("new_story", "{narrator_system}", "История: \"{story}\". Действие героя: \"{player_choice}\". Продолжи приключение. Плохие действия должны приводить к плохим последствиям. Разумные действия приводят к хорошим последствиям.", 200)
->dungeon
->END