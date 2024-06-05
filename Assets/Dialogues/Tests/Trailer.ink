INCLUDE ../Base.ink
INCLUDE ../Utils/Inventory.ink
INCLUDE ../Utils/Sounds.ink
INCLUDE ../Utils/PlayerPrefs.ink
INCLUDE ../Utils/AI.ink
INCLUDE ../Utils/SceneManagement.ink
INCLUDE ../Utils/Methods.ink


# reset_characters : Джон
# speaker : John
Держи
~RemoveFromInventory("Жетон", 1)
~AddToInventory("Жетон", 1)
~RemoveFromInventory("Кастет", 2)
~AddToInventory("Кастет", 2)
~RemoveFromInventory("Таблетки от головы", 2)
~AddToInventory("Таблетки от головы", 2)
~RemoveFromInventory("Кола", 3)
~AddToInventory("Кола", 3)
~RemoveFromInventory("Броня", 1)
~AddToInventory("Броня", 1)
~RemoveFromInventory("Артефакт", 2)
~AddToInventory("Артефакт", 2)
~RemoveFromInventory("Фрагмент карты", 4)
~AddToInventory("Фрагмент карты", 4)
~RemoveFromInventory("Фонарик", 1)
~AddToInventory("Фонарик", 1)
~RemoveFromInventory("Крепкий кофе", 2)
~AddToInventory("Крепкий кофе", 2)
~RemoveFromInventory("Суп из столовой", 1)
~AddToInventory("Суп из столовой", 1)
# background : InfirmaryBG
 {" "}
# background : CampBG
 {" "}
# background : BarracksBG
 {" "}
# background : CanteenBG
 {" "}
# background : CommanderOfficeBG
 {" "}
# background : HousesBG
 {" "}
# background : ConcertBG
 {" "}
# background :  LibraryBG
 {" "}
# background : HellBG
 {" "}
# background : qprcmqpwoc
 {" "}
~AIChangeBackground("DungeonBG", "Задний фон для игры, спокойный лес ночью", 1024, 576)
 {" "}
~AIChangeBackground("DungeonBG", "Задний фон для игры, спокойный лес днем", 1024, 576)
 {" "}
~AIChangeBackground("DungeonBG", "Задний фон для игры, луна над лесом ночью", 1024, 576)
 {" "}
 
// ~AIChangeBackground("DungeonBG", "Заброшенный склад", 1024, 576)
// ~RemoveFromInventory("Артефакт", 10)
// Ты заходишь в заброшенный склад.
// VAR choice = "Бездействовать"
// ~AskPlayer("choice")
// # {reset_characters} : ,
// Что будешь делать?
// VAR response = ""
// ~AIGenerateText("response", "Действие игрока в заброшенном складе : {choice}. Если игрок обыскивает склад, то опиши в одном предложении как он находит таинственный артефакт", 130)
// # barrier : response
// Игрок действует...
// {response}
// ~AddToInventory("Артефакт", 1)
// Ты забираешь таинственный артефакт себе.
// Нужно показать его библиотекарю.

// # background : {BlackBack}
// ...
// ...
// # background : {LibraryBG}
// # reset_characters: Оливия, Библиотекарь
// Ты заходишь в библиотеку
// +[Поговорить с Оливией]
// +[Поговорить с библиотекарем]
// +[Уйти]
// -
// # speaker : Библиотекарь
// Добрый день. Что-то хотел?
// +[Поговорить]
// +[Дать артефакт]
// +[Уйти]
// -
// ~RemoveFromInventory("Артефакт", 1)
// # speaker : 
// Ты передаешь таинственный артефакт.
// # speaker : Библиотекарь
// Кажется эта вещь очень древняя.
// Мне понадобится время, чтобы расшифровать все написанное!
// ~AddToInventory("Броня", 1)
// Спасибо за сотрудничество, держи свою награду!
// # speaker : 
// ...
// VAR char_description = ""
// ~ char_description = "{npc_description}. Тебя зовут Билл. Ты любишь котлетки с пюрешкой. Любимый жанр музыки - пост-хардкор."
// ~AITalk("{char_description} Сейчас ты в столовой и  собираешься плотно поесть.", 250)
// # {speaker} : Билл
// Приветствую!
// Пока!


// ~AIChangeBackground("DungeonBG", "улица ночью", 1024, 576)
// # {background} : JapLandscapes123123
// # reset_characters: Клара
// Ты выходишь на улицу.
// +[Поговорить]
// +[Уйти]
// -
// VAR char_description = ""
// ~ char_description = "{npc_description}. Ты дружелюбная девушка. Любишь флиртовать."
// ~AITalk("{char_description} Тебя зовут Клара. Ты находишься на улице Youtopia. Ночью тут всегда горят огни. Ты пришла сюда закупиться продовольствием и скоро уходишь.", 250)
// # {speaker} : Клара
// Привет! Ты что-то хотел?
// ~AIChangeBackground("DungeonBG", "задний фон для игры, столовая.", 1024, 576)
// # {reset_characters} : , Билл
// Ты заходишь в столовую
// +[Поговорить]
// +[Уйти]
// -
// VAR char_description = ""
// ~ char_description = "{npc_description}. Тебя зовут Билл. Ты любишь котлетки с пюрешкой. Любимый жанр музыки - пост-хардкор."
// ~AITalk("{char_description} Сейчас ты в столовой и  собираешься плотно поесть.", 250)
// # {speaker} : Билл
// Приветствую!
// Пока!
->END