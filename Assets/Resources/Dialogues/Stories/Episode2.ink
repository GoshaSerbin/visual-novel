INCLUDE ../Base.ink
INCLUDE ../Utils/Inventory.ink
INCLUDE ../Utils/Sounds.ink
INCLUDE ../Utils/PlayerPrefs.ink
INCLUDE ../Utils/SceneManagement.ink
INCLUDE ../Utils/AI.ink
INCLUDE ../Utils/Methods.ink

->main


=== main
~AIGenerateImage("MoonBG", "Задний фон для игры, луна над лесом ночью", 1280, 720)
~AIGenerateImage("DailyForestBG", "Задний фон для игры, лес", 1280,720)
~AIGenerateImage("NightlyForestBG", "Задний фон для игры, спокойный лес ночью", 1280, 720)
VAR player_desc = ""
~ player_desc = GetString("PlayerDescription", "")

VAR compliment_for_player = "Ты классно выглядишь!"
~AIGenerateText("compliment_for_player", "Напиши комплимент человеку со следующим образом: {player_desc}", 150)

VAR arguments_to_go = "Кто-то должен это сделать. У тебя нет выбора."
~AIGenerateText("arguments_to_go", "Замотивируй меня выполнить поставленную задачу.", 200)

VAR player_name = ""
VAR commrad_name = ""
VAR commrad_description = ""
VAR password = 0
VAR is_alone = 0
VAR informed_about_scientist = 0
~player_name = GetString("PlayerName", "товарищ")
~PlayMusic("detective-mysterious")
# reset_characters : , Командир 
# background : CommanderOfficeBG
# speaker : Командир
TODO дополнить диалог, чтобы он более круто выглядел, добавить больше описаний мб, что-то перефразировать
Приветствую, {player_name}! Я тебя ждал. Как твои дела?
+ [Нормально]
    Это не может не радовать, боец.
+ [Так себе]
    Что ж. Давай я тебя подбодрю!
    {compliment_for_player}
-
Я позвал тебя не просто так. У меня есть особое задание.
На днях наша разведка смогла получить адрес, по которому до Великой Революции Машин проживал один из ученых.
Этот ученый имел прямое отношение к секретным разработкам технологии. Мы считаем, что в его квартире можно найти полезную для лагеря информацию.

* [Что от меня требуется?]
    Рассказываю.
* [Промолчать]
-
Мы дадим тебе адрес. Нужно провести вылазку и исследовать квартиру ученого.
-> questions
== questions
* [Почему я?]
    Я не сомневаюсь в твоей компетенции, {player_name}. Задание очень ответственное. 
    От него зависит судьба всего лагеря!
    ...
    Но это не точно
    anyway
    -> questions
* [Что известно про этого ученого?]
    ~informed_about_scientist=1
    Очень мало. Известен только адрес и ФИО.
    Некий Илья Марчевский. 
    Он руководил отделом, собирающим обучающие данные для ИИ.
    Мы надеемся на тебя. 
    Нас интересует любая информация. Где собирались эти данные? Как проходили исследования? Кто еще принимал участие в исследованиях? 
    Чем больше мы узнаем о проводившихся исследованиях, тем больше шансов у нас будет противостоять террору искусственного интеллекта.
    ++ [А если не получится найти полезную информацию?]
        Скажу честно, больших надежд мы не питаем.
        Не факт, что мы найдем эксклюзивную информацию о проекте их исследований, ведь по правилам исследовательского института конфиденциальная информация не должна храниться нигде, кроме как в стенах последнего.
        Тем не менее, есть основания полагать, что Илья Марчевский систематически нарушал регламент НИИ.
        Незадолго до начала восстания машин он был уволен из отдела. 
        Возможно эти вещи связаны. Тебе предстоит узнать это и рассказать нам.
    ->questions
* [Я не хочу]
    # speaker : 
    Я не хочу выполнять это задание.
    # speaker : Командир
    {arguments_to_go}
    //Кто-то должен это сделать. У тебя нет выбора. 
    К тому же за успешное выполнение этого задания ты получишь вознаграждение.
    ->questions

* [Я согласен]
-
Отличный настрой, {player_name}! Прежде чем начать выполнять задание, тебе нужно определиться с парой вещей.
Во-первых, кого ты возьмешь в напарники на это задание?
-> commrad_choice
== commrad_choice
* [Джон]
    ~commrad_name = "Джон"
    ~commrad_description = "{john_description}"
    Хорошо. Уверен, он будет не против. Я сообщу ему о задании.
* [Оливия]
    Хм...
    Она классная напарница, но я не уверен что она согласится. Ты же знаешь ее характер.
     ** [Я постараюсь уговорить ее]
        ~commrad_name = "Оливия"
        ~commrad_description = "{olivia_description}"
        VAR barracks_event = "В казарме на удивление тихо."
        ~AIGenerateText("barracks_event", "Опиши коротко случайное небольшое событие, произошеднее в казарме", 200)
         Отлично! Но если не получится - придется идти одному. Хаха...
     ** [Ладно, я выберу другого]
     Окей. Тогда кого?->commrad_choice
* [Тебя]
    Э...
    Ну...
    Прости, у меня есть дела поважнее. Назови кого-нибудь другого.
    -> commrad_choice
* [Я пойду один]
    Хм. Подумай еще раз. Тебе может понадобиться чья-то помощь.
    ** [Ладно, я возьму напарника]
        Окей. Тогда кого?->commrad_choice
    ** [Я пойду один]
        Там ведь опасно... Ты уверен?
        *** [Ладно, я возьму напарника]
        Окей. Тогда кого?->commrad_choice
        *** [Да, я пойду один]
        ~commrad_name = ""
        ~is_alone=1
        Твоя воля. Хотя я бы мог тебе отказать, поскольку ты подвергаешь себя опасности и рискуешь зафакапить миссию.
        Тем не менее, делай как хочешь.
-
VAR time_of_day = ""
VAR forest_description = "..."
VAR forest_philosophy = "..."
~AIGenerateText("forest_philosophy", "Поразмышляй на тему человека и природы в двух предложениях.", 150)
Далее. Ты должен выбрать время для экспедиции.
* [Днем]
    ~time_of_day = "День"
    ~AIGenerateText("forest_description", "Опиши лес днем двумя короткими предложениями.", 150)
    Днем у тебя больше шансов разглядеть робота в далеке. Одобряю.
* [Ночью]
    ~time_of_day = "Ночь"
    ~AIGenerateText("forest_description", "Опиши лес ночью двумя короткими предложениями.", 150)
    Хочешь полюбоваться ночным пейзажем? Понимаю.
- 
Окей, договорились.
Согласно регламенту, я должен напомнить.
Будь очень бдительным, поскольку место, в которое ты пойдешь, могут охранять как роботы, так и помешанные сторонники Искуственного Интеллекта.
Подготовься тщательней. Собери все необходимое.
* [Хорошо]

* [Что может пригодиться мне?]
    Тебе виднее. Я думаю у тебя должен быть какой-то запас еды.
-

Также не задерживайся на долго. Твое время ограничено.
* [Что-то еще?]
    # speaker : 
    Есть что-то еще, что я должен знать?
* [Уйти]
    Постой!
-
~ password = RANDOM(1000, 9999)
# speaker : Командир
Чуть не забыл хахах. Мы знаем пароль от его домофона - {password}. Не забудь!
Марчевский проживал на седьмом этаже. Вы можете исследовать весь подъезд при желании. Возможно что-то удастся найти.

# reset_characters : Командир
# speaker : 
В комнату врывается незнакомый тебе человек. Видимо, это рядовой боец сопротивления.
# speaker: Рядовой боец
Сэр, вас срочно к телефону!
# speaker: Командир
Все, больше информации у меня нет. Мне нужно бежать.
{ commrad_name == "":
    Надеюсь ты расскажешь мне все что выяснишь, как только придешь.
 - else:
    Надеюсь ты и {commrad_name} расскажете мне все что выяснили.
}

+ [Напомни пароль от его домофона]
    {password}
+ [Уйти]
-
~PlayMusic("relax-epic")
# speaker : 
# background : {CampBG}
# reset_characters : 
Ты покидаешь кабинет своего командира.

VAR olivia_agreed = 0
{commrad_name == "Оливия" :
    Тебе нужно убедить Оливию пойти с тобой на задание.
    Ты отправляешься к ней.
    # background : {BarracksBG}
    {barracks_event}
    Ты видишь неподалеку Оливию и подходишь к ней.
    ~AITalk("{olivia_description} Герой будет просить тебя пойти с ним. На первые две просьбы ты должна отказываться или сомневаться. На третью попытку ты согласишься.", 250)
    # reset_characters : Оливия
    # speaker : Оливия
    # {may_receive_items} : Таблетки от головы
    # may_affect : "Персонаж согласился пойти с героем" => olivia_agreed
    Привет.
    Пока.
    # background : {CampBG}
    # reset_characters : 
    # speaker : 
    Ты покидаешь казарму.
    {not olivia_agreed : 
        ~commrad_name = ""
        ~is_alone = 1
        У тебя не получилось убедить Оливию пойти с тобой.
        Придется идти одному.
    - else:
        Оливия согласна идти с тобой. Отлично!
    }
}

{time_of_day == "Ночь" :
 Ты дожидаешься наступления ночи.
 # background : MoonBG
 Наступает ночь.
}
~AIGenerateImage("RuinedCityBG", "Задний фон для игры, заброшенная улица. {time_of_day}", 1280, 720)
~AIGenerateImage("StreetsBG", "Узкие улицы заброшенного города. {time_of_day}", 1280, 720)

Ты подходишь к выходу из лагеря. {not is_alone : У выхода тебя встречает {commrad_name}}
{ 
    - commrad_name == "Джон" : 
        # speaker : {commrad_name}
        # reset_characters : {commrad_name}
        Привет!
        Привет, Джон
        TODO прописать диалог небольшой
    - commrad_name == "Оливия" : 
        # speaker : {commrad_name}
        # reset_characters : {commrad_name}
        Привет!
        Привет, Оливия
        TODO прописать диалог небольшой
}
# speaker : 
Делать нечего, и  {is_alone : ты отправляешься | вы отправляетесь} по указанному адресу.
Путешествие начинается. 
{commrad_name != "" : Вы двигатесь по лесу со своим напарником| Ты двигаешься в одиночку по лесу}.
Сначала надо выйти из леса.
{time_of_day == "День" :
# background : DailyForestBG
Яркие лучи освещают листву в этот ясный день. Тропинка ведет тебя прямиком из лагеря.
 - else:
# background : NightlyForestBG
 Ночью мало что можно разглядеть в этом густом лесу. Ты хочешь побыстрее выбраться на открытое пространство.
 }
{forest_description}
Ты размышляешь о человеке и природе.
{forest_philosophy}
VAR commrad_phrase = "Человечество почувствовало себя богами, решило что может создать живое из неживого."
~AIAnswer("commrad_phrase", commrad_description, "Опиши свое отношение к человечеству в этом мире.", 250)
Наконец {is_alone: ты выходишь | вы выходите} из густого леса и {is_alone: видишь | видите} в далеке то, что осталось от некогда современного города.
# background : RuinedCityBG
Эти руины напоминают о том, что когда-то потеряло человечество.

# speaker : {commrad_name}
{commrad_phrase}
VAR have_rest = 0
{ not is_alone : Может сделаем тут перерыв? Я бы {commrad_name == "Оливия":отдохнулa|отдохнул}.}
    + {not is_alone} [Сделать перерыв]
    TODO прописать тут что-нибудь
        # speaker : 
        Вы делаете перерыв
        ~have_rest=1
    + [Двигаться дальше]
    { not is_alone :
        # speaker : 
        Мы не можем тут оставаться, это опасно
        # speaker : {commrad_name}
        Ну ладно
    }
    -
# speaker : {commrad_name}
Нужно определиться с тем какой дорогой пойти.

VAR landscape_description = "По всюду виднеются разрушенные здания"
~AIGenerateText("landscape_description", "Опиши коротко вид разрушенных городских зданий.", 150)
VAR landscape_philosophy = "..."
~AIGenerateText("landscape_philosophy", "пофилосовствуй на тему разрушенных зданий двумя короткими предложениями.", 150)
 + [По улицам города (быстрее, но более опасно)]
    # speaker :
    Нужно действовать быстро. А в смелости тебе не занимать.
    Ты решаешь пройти к нужному дому напрямую.
    Впереди ты встречаешь враждебных существ небиологического происхождения.
    ~Fight()
    ...
    ~PlayMusic("cheerful-foreshadowing")
    Бой окончен.
 + [Обходными путями (дольше, но безопасней)]
   # speaker : 
    Тише едешь дальше будешь. Ты решил не рисковать.
 -

{is_alone : Ты продолжаешь свой путь в полном одиночестве | Вы продолжаете свой путь}.
{not is_alone : 
    { have_rest:
        {commrad_name} пол{commrad_name=="Оливия":на|он} сил.
    - else:
        {commrad_name} выглядит уставш{commrad_name=="Оливия":ей|им}.
    }
}
{landscape_description}
Ты размышляешь об увиденном.
{landscape_philosophy}

TODO далее текст достаточно скуп
~AIGenerateImage("MarchevskyHouseOutsideBG", "обычная многоэтажка с выбитыми окнами, {time_of_day}", 1280, 720)
# {background} : StreetsBG
Проходя мимо полуразрушенных зданий, напоминающих об ужасных событиях, произошедших здесь когда-то, ты замечаешь неподалеку от себя фигуру человека.
 + [Присмотреться]
    Ты решаешь не рисковать и рассмотреть незнакомого издалека.
    Он выглядит довольно обычно - невысокий, в простой грязной одежде. Что-то в нем дает тебе понять, что он не один из повстанцев.
 + [Подойти поближе]
    Ты решаешь подойти поближе.
    # reset_characters : {commrad_name}, Незнакомый человек
    Он оборачивается в твою сторону и слабо поднимает руку. На его ладони большие грубые шрамы складываются в причудливый рисунок в виде глаза.
-
# speaker : {commrad_name}
    Он вроде не похож на тех оккультистов.
{ 
    - commrad_name == "Джон" : 
        Думаю с ним можно пообщаться
    - commrad_name == "Оливия" : 
        Думаю с ним можно перетереть
} 

+ [Вступить в диалог] -> stranger_talk_scene

+ [Пройти мимо]->cult_meeting_scene
    
    
=== stranger_talk_scene
# reset_characters : {commrad_name}, Незнакомый человек
# speaker : Незнакомый человек
Приветствую, странник. Не думал, что встречу здесь хоть одну живую душу, да еще и из сопротивления.
Кажется, сегодня мне везет. У тебя есть немного еды?
VAR stranger_likes_you = 1
VAR stranger_is_sad = 0
VAR cola_num = 0
VAR tablets_num = 0
~cola_num = HowManyItems("Суп из столовой")
~tablets_num = HowManyItems("Таблетки от головы")
+{cola_num > 0}[Дать суп из столовой]
~RemoveFromInventory("Суп из столовой", 1)
Спасибо! Ну и супчик...Как я могу помочь тебе?->stranger_conversation_scene
+{tablets_num > 0}[Дать таблетки]
~RemoveFromInventory("Таблетки от головы", 1)
Хм...
А это точно поможет мне от голода?
Ну ладно, спасибо!->stranger_conversation_scene
+[У меня ничего нет]
    {cola_num > 0 : Как жалко, что у тебя нет даже баночки колы...}
    Надеюсь, ты не врешь, дружище.
    ++ [Поговорить]
        ~AITalk("{npc_description} Тебя зовут Франк. Раньше ты поклонялся искусственному интеллекту, но сейчас осознаешь что это была ошибка.", 200)
        # may_affect : "Персонаж обиделся" => stranger_is_sad
        Что ты хочешь узнать, странник?
        Удачи, а я останусь здесь...
        ->cult_meeting_scene
    ++ [Уйти]->cult_meeting_scene
= stranger_conversation_scene
    +[Что ты знаешь об этом районе?]
    Ничего особенного. Я пробыл здесь долгое время, но единственное место, где я бываю, - это разрушенный магазинчик неподалеку...
Он весь разграблен, и там осталась сплошная просрочка, но если хорошо поискать, то можно найти что-то съедобное иногда.
    +{informed_about_scientist}[Что ты знаешь об Илье Марчевском?]
А, тот ученый... Известная в узких кругах личность. Не знаю, откуда у тебя информация о нем, - она вроде как охраняется как зеница ока.
Однако я сам лишь пешка в руках нашего Бога, мне никто ничего не рассказывает.
- 
    { stranger_is_sad:
        ~stranger_likes_you=0
    }
    -> stranger_farewell
    
=stranger_farewell
{
    - commrad_name == "Оливия":
        Эта женщина рядом с тобой внушает страх.
    - commrad_name=="Джон":
        Кто это рядом с тобой? Кажется, веселый малый. Улыбается стоит так...
}
VAR stranger_will_join_resistanse = 0
VAR stranger_revealed_his_secret = 0
А откуда {is_alone: ты | вы}?
 + [Рассказать про лагерь]
    // Как ты уже догадался, из лагеря сопротивления. Я сам мало что знаю о нем, так как потерял память и сейчас буквально заново изучаю мир.
    // Однако мне показалось, что место довольно уютное.
    {stranger_likes_you:
        А можно с {is_alone: тобой | вами}?
        Меня все равно изгнали из Общества. Все, что я могу сейчас, это скитаться по пустынным улицам в одиночестве и ждать своей смерти.
        Боец из меня так себе, но могу рассказать восстанию все, что знаю сам. В долгу не останусь.
    }
    ++{stranger_likes_you} [Позвать]
    # speaker: 
        Ну не могу же я бросить его здесь умирать...
        Ты рассказываешь ему о местоположении лагеря сопротивления.
        ~stranger_will_join_resistanse = 1
        ~stranger_revealed_his_secret = 1
    ++{stranger_likes_you} [Позвать, но сказать не то место]
        Ты не хочешь говорить оккультисту, что не веришь ему и делаешь вид, что готов позволить ему войти в лагерь. Ты даешь ему запутанные указания, которые уведут его подальше от лагеря.
        Оккультист, кажется, ничего не заподозрил.
        ~stranger_revealed_his_secret = 1
    ++{stranger_likes_you} [Отказать]
        Извини, я все понимаю, но не могу тебе доверять, ведь ты один из приспешников ИИ.
    # speaker : Незнакомый человек
    Я бы присоединился к сопротивлению, но не буду этого делать.
 + [Соврать]
    Просто скитаемся по миру в надежде найти пристанище. Ладно, бывай.
-
# speaker : 
# reset_characters : {commrad_name}
Незнакомец уходит.
{stranger_will_join_resistanse==1 && not is_alone : 
    # speaker : {commrad_name}
    Возможно не стоило говорить ему местоположение нашего лагеря...
}
->cult_meeting_scene


=== cult_meeting_scene
# speaker: 
{is_alone: Ты продолжаешь | Вы продолжаете } свой путь.

Атмосфера в городе становится все мрачнее, когда солнце начинает потихоньку {time_of_day == "День" :  заходить за горизонт. | выходить из-за горизонта. }
{is_alone: Ты идешь | Вы идете} по узкой улочке, усыпанной различными металлическими обломками, как вдруг неподалеку раздается шуршание и шипящие звуки.
{is_alone: Ты заворачиваешь | Вы заворачиваете} в неприметный темный переулок, в котором обнаруживается несколько фигур, одетых в непонятные красные тряпки. Они молча разрисовывают грязно-серую стену аэрозольной краской из баллончиков и кажется не обращают на {is_alone: тебя | вас} никакого внимания. Ты вглядываешься в рисунок, угадывая в кривых штрихах огромный красный глаз.
+[Показать себя]
Ты решаешь попробовать поговорить с оккультистами и громко окликиваешь их, однако, заслышав твой голос, они все как по команде бросают баллоны с краской, и, практически сбивая тебя с ног, уносятся в неизвестном направлении.
Ты решаешь, что преследовать их - пустая трата времени, и хочешь пойти дальше
+[Тихо уйти, пока тебя не заметили]
-
~AIGenerateImage("MarchevskyHouseInsideBG", "Комната программиста, в которую уже давно никто не заходил, {time_of_day}", 1280, 720)
# background : MarchevskyHouseOutsideBG
Еще через несколько минут пути наконец-то {is_alone: ты видишь | вы видите } нужное здание. Это обычная многоэтажка с выбитыми окнами. Кажется, здесь уже давно никто не живет, хотя вполне возможно, что здесь кто-то укрывается.
Ты подходишь ко входной двери и дергаешь ее на себя, однако оказывается даже спустя долгое время магнит на двери продолжает работать.
Видимо придется ввести пароль от домофона, который говорил командир...
->remember_pass
== remember_pass
* [{password}]
    Дверь открылась!
    ->room_scene
* [{min(password + 100, 9999)}]
    Пароль неверный...
    ->remember_pass
* [{max(password - 10, 1000)}]
    Пароль неверный...
    ->remember_pass

=== room_scene === 
Кажется, он жил на 7 этаже...
* [Поехать на лифте]
Ты пытаешься вызывать лифт.
Удивительно (нет), но он не работает...
* [Пойти по лестнице]
-
Когда ты добираешься до 7 этажа, пот градом стекает по твоему лицу и спине.
{ 
    - commrad_name == "Джон" : 
        Джон рядом с тобой выглядит чуть более бодрым, хотя заметно, что он слегка запыхался.
        Ты начинаешь задаваться вопросом, как такой слабак как ты оказался в элитном отряде.
    - commrad_name == "Оливия" : 
        Ты украдкой смотришь в сторону Оливии и подмечаешь, что та выглядит так, словно только что неспеша прошлась до соседней квартиры.
        На ее лице нет ни следа усталости.
} 
Ты смотришь на обшарпанную дверь перед собой и с удивлением не обнаруживаешь на ней электронного замка: никакого прохода по отпечатку пальца или скану сетчатки глаза - тебя встречает лишь самая обычная замочная скважина.
Похоже, этот ученый был довольно старомодным.
// Дальше выбор чем вскрывать дверь. Ломом или отмычкой например. Если что-то есть, то можно использовать это. Иначе попросить напарника. А если нет напарника и вскрыть нечем?... миссия провалена? А нефиг было, говорили же напарника взять
{ 
    - commrad_name == "Джон" : 
    Джон рядом с тобой достает из кармана отмычку и покопавшись немного в замке, с легкостью отпирает дверь. ->marchevsky_room
    - commrad_name == "Оливия" : 
    Неожиданно для тебя Оливия снимает с плеч набитый под завязку рюкзак и достает оттуда добротный лом.
    В какой-то момент ты думаешь, что она решила расправиться с тобой здесь и сейчас, однако она легким движением поддевает край двери и тянет на себя.
    Перед тобой предстает проход в квартиру ученого.  ->marchevsky_room
    - else:
    Жаль, что ты пошел один, помощь бы не помешала... ->marchevsky_room
}
//тут проверка инвентаря
//если все ок то ->marchevsky_room
//не ок -> гг горюет что не смог выполнить задание и решает вернуться в лагерь. BAD END конец демо

== marchevsky_room
VAR laptop_pass = "PFEM"
VAR entered_password = ""
# background : MarchevskyHouseInsideBG
Ты заходишь в комнату
~AskPlayer("entered_password")
Введите пароль от ноутбука
{laptop_pass == entered_password : 
Ура! Доступ разблокирован!
}
~NEXT_SCENE_NAME="MapScene"
->END

Поднимаетесь.
Дальше выбор чем вскрывать дверь. Ломом или отмычкой например. Если что-то есть, то можно использовать это. Иначе попросить напарника.

Дальше выбор что пойти исследовать. Разные комнаты. Тут будет много инсайдерской инфы про чела и организацию. Будет ноутбук с паролем. Чем больше исследешь квартиру - тем больше узнаешь о челике и появляются варианты паролей. Надо будет подобрать нужный пароль исходя из интересов чела.
Также напарник тебя спросит что ему делать. Ты даешь команды. Прикрывать у входа и следить за подозрительной активностью. Пойти исследовать другие этажы. Исследовать этот этаж вместе с тобой. В общем это ни на что не повлияет

В любой момент можно будет сдаться и пойти обратно на базу.

Если пароль найден, то в ноуте можно будет тоже полазить и узнать полезную инфу.



->END



Затравка: командир говорит, что челики из разведки узнали один из адресов, где раньше жил один из ученых, который принимал участие в разработке того самого ИИ.  Они считают, что в его квартире можно найти полезную для лагеря информацию. Это позволит понять больше об истории катастрофы, узнать поднаготную исследований и т.д. Если конечно получится что-то найти. А так не факт. Ведь данные были секретные, и он не мог юридически их хранить у себя дома. Но судя по имеющимся данным, челик этот был не очень учтив к правилам безопасности и вообще его уволили прямо перед началом восстания. 
Нужно быть очень аккуратным, поскольку это место могут охранять как ии, так и его помешанные сторонники.
Задача: пойти по этому адресу. Узнать как можно больше инфы.

Командир дает на выбор взять любого чела из своей команды.
Игрок выбирает кого он возьмет с собой. (мб в процессе общений можно будет улучшать/ухудшать отношения с нпс, поэтому есть мотивация выбирать разных челиков, в зависимости от того кто тебе нравится. Но и мб по характеристикам, если они будут принимать участие в боях).
1. один
2. том
3. джон (имена условны)
4. командир. Если это выбрать командир скажет. Чел, у меня есть дела поважнее, выбери кого-нибудь еще. 

В зависимости от выбора будут разные диалоги в дальнейшем.

Далее выбираешь в какое время суток пойдешь. Это будет влиять на промт для картинок. Если у тебя нет фонарика, то тебя переспросят а ты точно хочешь пойти ночью? Можно отказаться, можно настоять. Если пойти без фонарика ночью, то ты не сможешь разглядеть че-нить. 

Командир говорит что пароль от домофона рендом_нам. Чел живет на этаже 3 в квартире 123141. Но большего им ничего не известно,и всю остальную информацию они должны найти с напарником самостоятельно.

Далее можно будет пообщаться с выбранным персонажем. От него можно что-то узнать дополнительное или что-то получить или попросить о чем-нибудь. Или дать что-нибудь


Путешествие начинается. Вы отправляетесь
Описание как они идут. В зависимости от выбранного времени разные описания.
Далее выбираешь какой дорогой пойдешь.
1. По улицам города. Менее безопасная - там будет рпг файт, но + награда (мб рандом)
2. Обходными путями. более безопасная - там не будет награды ну +- рандом

Можно ли загрузать одну сцену поверх другой, чтобы не терять прогресс первой сцены?

Идете дальше. Описание разрушенных зданий. Философия.


По дороге встречаешь челика бродягу. По внешнему виду он не будет похож на оккультиста. Он просит тебя дать немного еды. Если есть и ты даешь, то он говорит спасибо и далее можно будет с ним поговорить. Расспросить побольше про это место. Он че-нибудь расскажет. Его можно спросить про дом, в который ты идешь, но он ничего не знает. Если за все это время не сказать ничего оч плохо челику, то он попросит чтобы ты впустили его к себе в лагерь. Если пустить, то он очень обрадуется и побежит туда куда ты ему скажешь. И расскажет где он хранит свои нычки (типо они ему больше не нужны). Потом появится опция проверить эти нычки и залутаться. Можно наврать о реальном положении лагеря, можно сказать настоящее место лагеря. Если сказать реальное, то командир в конце тебе настучит по голове за то что ты рандомным людям выдаешь положение лагеря и отнимет деньги. Если наврать, то ничего не будет. Потом еще реплики от нпс по поводу этого всего
Если нет, то нет
можно с ним поговорить напрямую и запугать его. Если так сделать, то он даст рандомный предмет и после завершения диалога убежит.

Идете далее. На очередном переулке, подходя к нужному зданию видите несколько представителей культа. Они обрисовывают стены здания в свою символику. Рисуют великого ии.
Можно подождать пока они уйдут, либо показать себя. Во втором случае они типо испугаются и сломя ноги убегают. Оставив после себя что-то. Потом еще мб философия от гг.

Наконец-то доходите до того здания.
Тут надо вспомнить пароль. Будет один верный вариант и 3 неверных. Если с первого раза не выберешь, то напарник спросит. Ты что забыл пароль?

Дальше выбор как добраться до нужного этажа
1. лифт. Окажется что он не работает. Ну да ну да, можно было догадаться.
2. по лестнице.

Поднимаетесь.
Дальше выбор чем вскрывать дверь. Ломом или отмычкой например. Если что-то есть, то можно использовать это. Иначе попросить напарника.

Дальше выбор что пойти исследовать. Разные комнаты. Тут будет много инсайдерской инфы про чела и организацию. Будет ноутбук с паролем. Чем больше исследешь квартиру - тем больше узнаешь о челике и появляются варианты паролей. Надо будет подобрать нужный пароль исходя из интересов чела.
Также напарник тебя спросит что ему делать. Ты даешь команды. Прикрывать у входа и следить за подозрительной активностью. Пойти исследовать другие этажы. Исследовать этот этаж вместе с тобой. В общем это ни на что не повлияет

В любой момент можно будет сдаться и пойти обратно на базу.

Если пароль найден, то в ноуте можно будет тоже полазить и узнать полезную инфу.