INCLUDE ../Base.ink
INCLUDE ../Utils/Inventory.ink
INCLUDE ../Utils/Sounds.ink
INCLUDE ../Utils/PlayerPrefs.ink
INCLUDE ../Utils/AI.ink
INCLUDE ../Utils/SceneManagement.ink
INCLUDE ../Utils/Methods.ink

~ai_temperature=1.3

VAR narrator_system = "Ты проводишь важные опросы, которые помогут определить и установить мнение большинства. Ты должен давать два варианта ответа на вопрос \"Что бы вы выбрали?\", нумеруя их. Пример опроса:\n1.Ваш шанс победить в любой игре минимум 75%. 2. Вы можете замедлять время до 0,5 и ускорять до х2.\n Еще пример: 1. Низкую девушку. 2. Высокую девушку."

VAR prompt = "Придумай опрос."

VAR choices = ""
VAR player_choice = ""
-> dungeon
=== dungeon ===
~AIAnswer("choices", "{narrator_system}", "{prompt}", 200)
# {background} : {BlackBack}
# {speaker} : {None}
# barrier : choices
Составляем опрос...
VAR choice1 = ""
VAR choice2 = ""
~choice1 = GetChoice("{choices}", 1)
~choice2 = GetChoice("{choices}", 2)
Что бы вы выбрали?
+ [{choice1}]
    ~player_choice = GetChoice("{choices}", 1)
+ [{choice2}]
    ~player_choice = GetChoice("{choices}", 2)
-
~AIChangeBackground("DungeonBG", "{player_choice}", 1024, 576)
->dungeon
->END