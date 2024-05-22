INCLUDE ../Base.ink
INCLUDE ../Utils/Inventory.ink
INCLUDE ../Utils/Sounds.ink
INCLUDE ../Utils/PlayerPrefs.ink
INCLUDE ../Utils/AI.ink


EXTERNAL GetChoice(choices, num)

VAR narrator_system = "Ты - рассказчик увлекательных историй в ролевой игре в жанре киберпанк. Предыстория игры: \"Наступил пост-апокалипсис. Искусственный интеллект вырвался из под контроля, началась борьба против искусственного интеллекта, в результате которой большая часть человечества погибла. Часть оставшихся в живых образовала культ поклонения искусственному интеллекту. Часть оставшихся образовали лагерь сопротивления, в котором продолжают борьбу. Поселение выживших находится в лесу\". Ты виртуозно приспосабливаешься и взаимодействуешь с решениями и действиями игрока. Ты создаешь цепочку реакций, которая может вывести игру на совершенно неожиданный путь или раскрыть новую сторону истории. ОТВЕЧАЙ НА ВОПРОСЫ ОЧЕНЬ КОРОТКО."
// VAR story_system = ". В поселении есть столовая, лазарет, казарма, библиотека. Периодически формируются отряды и отправляются исследовать новые территории/добывать еду, материалы и медикаменты, искать новых выживших и т.д."

// VAR story_user = "Твоя задача - придумать историю, которая произойдет в данном сеттинге. Это должно быть 1 или 2 коротких предложения. Например, 'Во время очередной исследовательской миссии, вы встретили группу из десяти роботов', 'Один из поселенцев предлагает сыграть с вами в карты', 'Намечается очередная экспедиция и нужно выбрать куда пойти'. Суть этих историй должна быть в том, что возникает выбор действий, например, 'Напасть на роботов или отступить', 'Сыграть в карты или отказаться', 'Пойти в заброшенную больницу или на заброшенный завод'. Можешь придумывать абсолютно разные истории, они должны быть интересными и неочевидными. Важно, чтобы результат от принятого решения не менял существующий сеттинг. То есть нельзя, чтобы поселение полностью исчезло, или люди победили искусственный интеллект. В ответе укажи только историю (без ответов)"


VAR prompt = "Придумай историю в указанном сеттинге."

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
// Привет!!! {TURNS_SINCE(-> dungeon)}
// ~AISum("story", "{story}. {player_choice}. {new_story}", 250)
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
// + [{GetChoice("{choices}", 3)}]
//     ~player_choice = GetChoice("{choices}", 3)
-  
~AIAnswer("story", "Перескажи кратко (не более трех предложений) историю.", "{story}. {new_story}", 250)
# barrier : story
Загрузка...
~AIAnswer("new_story", "{narrator_system}", "История: \"{story}\". Действие игрока: \"{player_choice}\". Продолжи приключение.", 200)
->dungeon
->END
