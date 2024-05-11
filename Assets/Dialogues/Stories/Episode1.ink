INCLUDE ../Base.ink
INCLUDE ../Utils/AI.ink
INCLUDE ../Utils/Inventory.ink
INCLUDE ../Utils/Sounds.ink
INCLUDE ../Utils/SceneManagement.ink
INCLUDE ../Utils/PlayerPrefs.ink

~doctor_description = "{doctor_description} Ты общаешься с главным героем, которого доставили сюда после боя. Большего ты не знаешь." 

~Повар_раздачи_description = "{Повар_раздачи_description} Ты не знаешь ни имени главного героя, ни чем конкретно он занимается." 

~ olivia_description = "{olivia_description} Главного героя знает только в лицо и считает, что тот довольно слаб, чтобы состоять в элитном отряде."

-> part1

=== part1 ===
# {background} : {BlackBack}
# {reset_characters} : {None}
# {speaker} : {None}
Ты просыпаешься, медленно открывая глаза, и ощущаешь легкое покалывание в голове.
~SetInt("SavedStoryProgress", 1)

Твоя рука автоматически тянется к месту удара, и ты аккуратно касаешься затылка, резко ощущая пронзившую голову боль.
Осторожно поднимаясь, ты садишься на кровати, пытаясь сморгнуть накатившую разом усталость и оглядываешь комнату.

# {background} : InfirmaryBG
# {reset_characters} : {None}, {Незнакомец}
Неподалеку от тебя сидит незнакомый человек и непринужденно перелистывает страницу какой-то книги.
Ты хочешь уже открыть рот и подать голос, но незнакомец вдруг захлопывает книгу, все так же не поднимая на тебя взгляда.

VAR location_description = "Ну и место..."
~ AIGenerateText("location_description", "Опиши лазарет, в котором очнулся главный герой. В помещении только два человека: ты сидишь на кровати и незнакомец сидит за столом неподалеку от тебя. Лазарет похож на большую палатку. Напиши 2 небольших предложения, без действий.", 100)
VAR player_descr = ""
~player_descr = GetString("PlayerDescription", "Молодой человек")
VAR comment_on_player = "Ну и вид у тебя..."
~ AIGenerateText("comment_on_player", "Напиши язвительный комментарий на человека с таким образом \"{player_descr}\"", 100)
# {speaker} : {Незнакомец}
# {emotion} : {DEFAULT}
Проснулся? Отлично.
# {speaker} : {None}
Ты не знаешь, что ответить на такое сухое приветствие и ждешь продолжения фразы, однако его не следует.
Незнакомец откладывает книгу и наконец поднимает свой взгляд на тебя, словно ожидая от тебя каких-то действий. Повисает гнетущее молчание.
Ты все еще не понимаешь, что происходит и где ты находишься, а потому осматриваешь помещение, стараясь игнорировать остаточную пульсирующую боль в затылке.

{location_description}

До этого молчавший незнакомец тяжело вздыхает.

# {speaker} : {Незнакомец}
{comment_on_player}
Ты так и будешь тут сидеть?
Если ничего не болит, то поднимай свою задницу с кровати и проваливай.
* [Кто ты такой?]
* [Где я?]
-Совсем головой ударился или прикалываешься?

# {speaker} : {None}
Он усмехается и пристально смотрит на меня, но кажется уловив твой полный недоумения взгляд становится серьезнее.

# {speaker} : {Незнакомец}
Черт тебя побери, ты реально ничего не помнишь?

# {speaker} : {None}
Ты пытаешься вспомнить хоть что-то - хотя бы свое имя, но боль в затылке лишь усиливается.
Ты поверженно мотаешь головой.

# {speaker} : {Незнакомец}
Ты в лазарете нашего лагеря. Я тут вроде как врач, но толку от меня немного. Я бы может и рад помочь, но умею оказывать только первую помощь.

# {speaker} : {None}
Он небрежно кивает на аптечку в углу комнаты.

# {speaker} : {Врач}
Нормального оборудования у нас нет, а ребята, которые хоть что-то умеют отправились с твоим отрядом на миссию.

# {speaker} : {None}
Отрядом? Лагеря? Ты пытаешься вспомнить хоть что-нибудь из этого, но твои мысли абсолютно пусты.
* [Кто я?]
- Наконец ты задаешь самый главный вопрос, мучающий тебя последние пять минут. Кажется, что собственное имя вертится на языке, но при попытке вспомнить ты встречаешь лишь пустоту.

# {speaker} : {Врач}
Без понятия.

# {speaker} : {None}
Он пожимает плечами и снова открывает книгу, кажется, уже потеряв интерес к разговору с тобой.

# {speaker} : {Врач}
Тебя притащил какой-то парень из твоего отряда и тут же убежал.
Можешь расспросить обо всем своих ребят, когда они вернутся. Я ничего не знаю о тебе и мало что знаю о ситуации снаружи.
А пока, если хорошо себя чувствуешь, можешь пройтись по лагерю.
Может знакомые места или люди помогут что-то вспомнить.

# {speaker} : {None}
Кажется на этом ваш увлекательный диалог заканчивается.

-> free_time_infirmary

=== free_time_infirmary ===
VAR describe_yourself = "Ну врач я..."
~ AIAnswer("describe_yourself", doctor_description, "Расскажи о себе", 150)
VAR what_happened = "Слушай, мне некогда..."
~ AIAnswer("what_happened", doctor_description, "Можешь примерно описать положение дел? Я не знаю ничего про этот лагерь", 150)
# {speaker} : {None}
+ [Поговорить с врачом]-> doctor
+ [Идти в лагерь]
    VAR camp_description = "Ну и лагерь..."
    ~ AIGenerateText("camp_description", "Опиши главную площадку лагеря повстанцев, который находится среди леса за бетонными стенами. На площадке находится толпа шумных людей, которые идут по своим делам. От площадки отходят дороги в разные части лагеря: на тренировочную площадку, в казармы и в столовую.", 150)
    VAR training_ground_description = "Ну и казарма..."
    ~ AIGenerateText("training_ground_description", "Площадка лагеря повстанцев находится среди леса за бетонными стенами. Это отдельно стоящее просторное здание, в котором есть тир и манекены для тренировки ближнего боя. Несколько людей упражняются в тире. Продолжи описание двумя короткими предложениями.", 250)
    VAR canteen_description = "Столовая большая..."
    ~ AIGenerateText("canteen_description", "Опиши просторную столовую, заполненную шумными людьми", 200)
    VAR houses_description = "Небольшие жилые дома..."
    ~ AIGenerateText("houses_description", "Опиши кратко небольшие жилые дома в 2 этажа снаружи. Они выглядят довольно мрачно.", 200)
    # {speaker} : {Врач}
    Постой...
    Забыл отдать тебе кое-что.
    Это нашли рядом с тобой, когда ты был в отключке.
    ~AddToInventory("Жетон", 1)
    # {speaker} : {None}
    {Врач} дает тебе непонятный предмет, о котором ты ничего не знаешь.
    # {speaker} : {None}
    Ты последний раз бросаешь взгляд на недовольного врача и приоткрываешь плотную занавеску, явно служащую здесь в качестве двери.
    # {background} : CampBG
    # {reset_characters} : {None}
    Перед тобой открывается вид на гигантскую площадь, полностью заполненную снующими туда-сюда людьми.
    {camp_description}-> look_around_camp

= doctor
# {speaker} : {Врач}
Чего тебе опять?
 * [Расскажи о себе]
    # speaker : {Врач}
    {describe_yourself}
    -> doctor
 * [Что происходит?]
    # {speaker} : {None}
    Можешь примерно описать положение дел? Я не знаю ничего про этот лагерь
    # speaker : {Врач}
    {what_happened}
    -> doctor
 + [Задать другой вопрос]
 ~AITalk("{doctor_description} Если игрок попросит, ты можешь дать ему таблетки.", 150)
    # {may_receive_items} : Деньги, Таблетки от головы
    # speaker : {Врач}
    Да?
    -> doctor
 + [Выход]
    -> free_time_infirmary
 
 
=== look_around_camp ===
# background : CampBG
# {reset_characters} : {None}
Ты выходишь на развилку.
* [Тренировочная площадка]-> training_ground
* [Столовая]-> canteen
* [Жилая часть]-> houses
* -> 
    Кажется, в этом лагере ты пока не встретил ни одного знакомого лица.
    Ты слышишь вдалеке какие-то странные звуки. 
    -> part2
 
=== training_ground ===
# background : BarracksBG
{training_ground_description}
VAR good_morning = "Добрый"
~AIAnswer("good_morning", john_description, "Добрый день", 200)
Ты несколько секунд наблюдаешь за тем, как парочка молодых ребят с переменным успехом стреляет по мишеням в конце коридора и неуверенно осматриваешься.
Никто из находящихся в комнате людей не кажется знакомым, пока ты не натыкаешься взглядом на двоих людей в военной форме, сидящих в позе лотоса на полу в углу комнаты.
# {reset_characters} : {Мужчина_в_форме}, {Женщина_в_форме}
Они сосредоточенно смотрят на веера игральных карт в своих руках.
Что-то в их внешнем виде побуждает тебя подойти к ним и прервать их увлекательную партию. Возможно, это твой мозг подсказывает, что они могут иметь хоть какую-то информацию о тебе.
-> trainers

= trainers
* [Добрый день]
    {good_morning}
    -> trainers
+ [Другой вопрос]
    # speaker : {None} 
    ++ [Мужчина в форме]
         ~AITalk("{john_description}", 150)
        # {speaker} : {Мужчина_в_форме}
        О чем хочешь поговорить?
        До скорого!
        -> trainers
    ++ [Женщина в форме]
    ~AITalk("{olivia_description}", 150)
    # {speaker} : {Женщина_в_форме}
        Что?
        Удачи.
        ->trainers
+ [Уйти]
# {speaker} : {None}
- Разговор окончен. Ты решаешь вернуться на главную площадь лагеря.
->look_around_camp

=== canteen ===
# background : СanteenBG 
{canteen_description}
Ты решаешь подойти к человеку, который видел тебя до этого с наибольшей вероятностью -- в дальнем углу помещения находится что-то вроде импровизированного прилавка.
Рядом с прилавком молодая девушка, на вид ученица старшей школы, что-то насвистывая себе под нос, наливает в большую тарелку странно выглядящую субстанцию.
Заметив твое приближение, она лучезарно улыбается, с грохотом бросает половник в гигантскую дымящуюся кастрюлю и полностью переключает внимание на тебя.
# {reset_characters} : {Повар_раздачи}
# {speaker} : {Повар_раздачи}
Привет! Что нового? О, твой отряд уже вернулся с миссии? А я не слышала. Как поход? Есть хорошие новости? Плохие новости? А у нас суп на обед.
# {speaker} : {None}
Она заваливает тебя таким количеством слов, что ты можешь только беспомощно наблюдать за тем, как вопросы вылетают из ее рта пулеметной очередью. Ты решаешь ответить хоть на что-то, заметив ее выжидающий взгляд.
# {speaker} : {None}
* [Особо ничего нового]
    Девушка демонстративно разочарованно вздыхает. 
    # {speaker} : {Повар_раздачи}
    Скучный ты какой-то. Тут вообще поговорить не с кем, все приходят чисто поесть, начинаешь с ними общаться, а они говорят, мол, извини, я занят, ну и все. А шеф вообще когда готовит, делает вид, что он глухонемой. Говорит, от моей болтовни у него зелень вянет. 
* [Суп... здорово]
# {speaker} : {Повар_раздачи}
Да это шеф все новые рецепты пробует. Сегодня вот решил добавить какой-то новый ингредиент, выглядит дико, но на вкус вполне ничего.
# {speaker} : {None}
Ты недоверчиво окидываешь взглядом непонятную жижу в миске и решаешь не испытывать судьбу. Однако девушка продолжает, не обращая внимания на заминку.
# {speaker} : {Повар_раздачи}
Он в следующий раз овощное рагу пообещал приготовить. Хорошо хоть сейчас урожая хоть отбавляй. Хоть какая-то помощь от ИИ.
* [Я пойду, пожалуй]
Ты решаешь не выслушивать бесконечный поток сознания девушки, а потому резко разворачиваешься и, прежде чем она успевает открыть рот, поспешно идешь к выходку из здания. 
    ->look_around_camp
- # {speaker} : {None}
Ты не знаешь, что еще сказать, а потому решаешь перевести тему.
+ [Поговорить]
 ~AITalk("{Повар_раздачи_description}", 150)
 Да-да? Я всегда готова поболтать! На любые темы!
 Ну ладно.. снова я одна.
 + [Уйти]
-  # {speaker} : {Повар_раздачи}
    Стой! Хотя бы возьми с собой.
    # {speaker} : {None}
    Она отворачивается от тебя и шуршит за стойкой, через десять секунд поворачиваясь к тебе с контейнером, полным неаппетитной жижи.
    ~AddToInventory("Суп из столовой", 1)
    Ты решаешь не сопротивляться и взять его с собой и возвращаешься на главную площадь лагеря.
->look_around_camp

=== houses ===
# background : BarracksBG 
{houses_description}
# {reset_characters} : {Комендант}
Ты хочешь зайти и поспрашивать у людей, знает ли кто-нибудь, где здесь твоя комната, но пожилая женщина, спокойно сидевшая рядом со входом, внезапно материализуется прямо перед тобой.
# {speaker} : {Комендант}
Вход только по пропускам!
# {speaker} : {None}
Ее громкий голос неожиданно пугает тебя и ты ощупываешь карманы, но находишь только пустоту.
*[Потерял]
*[Кажется, я оставил его в комнате]
*[Он где-то есть у меня, просто найти не могу]
-# {speaker} : {Комендант}
Не волнует, вернешься, когда сделаешь новый пропуск.
# {speaker} : {None}
Ты решаешь не спорить с этой пугающей женщиной, но пока не уходишь, надеясь выудить из нее больше информации.
+ [Поговорить]
# {speaker} : {Комендант}
 ~AITalk("{Комендант_description}", 150)
 Чего тебе? По графику сейчас не свободное время.
 Иди занимайся.
 + [Уйти]
 # {speaker} : {None}
- Ты решаешь вернуться на главную площадь лагеря.
->look_around_camp

=== part2 ===
TODO После этого он заканчивает блуждать по лагерю и понимает, что в казармах да и вообще везде начинается какая-то возня. Кто-то орет, что прибыли челы с миссии и наш гг срывается конечно к главным воротам как и многие остальные встречать элитный отряд с миссии. Самым первым заходит относительно молодой чел (лет 30+), видит гг и такой типа оооо здарова чел давно не виделись как себя чувствуешь жаль что ты пропустил миссию, там такоеееее былоооо. и увлекает его за собой вместе с этой странной компанией. они все идут в столовку по жрать. гг пока особо слова не давали, поэтому он в шоках, его сажают вместе с командиром за один стол еще с каими-то двумя ребятами. стооловая тут же наполняется необычным шумом. Еда в тарелке выглядит лучше чем утром. Гг из разговоров вокруг пытается понять че ваще происходит и о чем все говорят. Командир наконец говорит что гг необычно тихий и даже не спрашивает что с миссией и интересуется здоровьем гг, а то его здорово приложило головой о землю и тут гг такой а я ниче не помню и вас тоже не помню… все за столом вахуи и думают, что он шутит, но нет. Они хотят снова потащить его к врачу чтобы он пояснил ситуацию, но он им поясняет что чел из лазарета ниче не знает, а пара врачей вернувшихся с миссии очень занята операцией. Тогда они идут в казармы (наканецта), и командир рассказывает челиксу минимально историю и пытается пробудить в нем воспоминания показывая ему предметы, но тот нифига не помнит вообще продолжением квеста будет сходить к врачу адекватному. Врач проведет исследование и скажет блин жесть ваще ниче сделать не могу, придется тебе просто ждать, пока воспоминания сами не вернутся. далее думаю у нас фри тайм просто чтобы мы познакомились с персами на локациях (гг типа пытается вспомнить что-то). Можно выдать ему здесь парочку побочных квестов.
// -> END