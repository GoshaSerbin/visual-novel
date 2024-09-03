# О проекте
Визуальная новелла с элементами RPG в сеттинге киберпанка с генерацией контента с помощью ИИ. Для ознакомления с проектом посмотрите карточку проекта, презентацию и видео.

Играть [здесь](https://laplas-games-inc.itch.io/apocalypse-code)

![Скриншот](https://img.itch.zone/aW1hZ2UvMjY0Mjk0OC8xNjQ4NDg1MS5wbmc=/original/imY08M.png)


# ink API
Мы предоставляем возможность задействовать искусственный интеллект в ваших играх. Делаем это с помощью интеграции функционала ИИ в Ink через наше API. Общую информацию об ink и его возможностях можно почитать [здесь](https://www.inklestudios.com/ink/).

### Подготовка
Откройте проект в Unity. Продублируйте (Ctrl+D) сцену "DemoScene" в папке Scenes. Зайдите в нее. Найдите на сцене объект SceneContext. В инспекторе найдите компонент DialogueInstaller у этого объекта. В поле Ink Json вставьте ваш Ink файл.
Готово!
Как добавить новый Ink файл? В папке Dialogues/Stories кликните ПКМ → Create → Ink.
Готово!

### Персонажи
Персонажи должны храниться в директории **"Resources/Characters"**. Для создания персонажа в этой директории нужно нажать ПКМ и выбрать Create → CharacterSO. Заполните информацию о персонаже. 
Готово!
Персонаж может иметь несколько имен. При написании ink файла для указания автора фразы добавьте тег:
```Ink
# speaker : Том
Привет!
Что делаешь сегодня?
```
Если спикер для фразы не указан, то им будем спикер предыдущей фразы. Поэтому тег имеет смысл указывать только если спикер поменялся.

#### Отображение персонажей на сцене
Отображаться могут не более двух персонажей (один -- слева, другой -- справа). Для обновления персонажей на сцене используйте тег
```Ink
# reset_characters : Том, Джон
Том посмотрел на Джона
```
В примере выше Том будет стоять слева, Джон -- справа.
Можно поместить одного персонажа слева:
```Ink
reset_characters : Джон
```
Можно поместить одного персонажа справа:
```Ink
reset_characters : , Джон
```
Если нужно убрать всех персонажей:
```Ink
reset_characters :
```

**Важно**. Указывать хештег имеет смысл только когда персонажи на сцене должны обновиться.

#### Указать эмоцию персонажа
```Ink
# emotion : 0
```
Эмоция -- это число, позиция спрайта в списке спрайтов персонажа. Если у вашего персонажа только один спрайт, то данный тег не нужен. Данный тег сохраняется, т.е. если для какой-то реплики он не указан, то берется с предыдущей реплики.

### Задний фон
```Ink
# background : BackgroundName
```
Указывать этот тег нужно только когда задний фон должен поменяться.
Про использование здесь ИИ будет далее.

### Управление инвентарем
Инвентарь хранит предметы . Содержание инвентаря сохраняется между сценами и между сеансами игры. Все возможные предметы расположены в директории **"Resources/Items"**. Для создания предмета действия аналогичные созданию персонажей: ПКМ → Create → ItemSO. Заполните информацию о предмете. В дальнейшем обращайтесь к предмету через его имя (не имя файла, а имя, указанное в поле `name` предмета).
Для управления инвентарем есть 3 функции.
#### Добавление в инвентарь
```Ink
AddToInventory(name, count)
```
Добавляет вещь с названием `name` в количестве `count` в инвентарь. Функция возвращает количество предметов, которые не удалось положить в инвентарь (из-за нехватки места). Чаще всего это 0 (в случае успеха). **Рекомендую** исходить из того что вещи в инвентарь всегда поместятся и не проверять результат.

#### Удаление из инвентаря
```Ink
RemoveFromInventory(name, count)
```
Аналогично, только убирает из инвентаря. Можно удалять больше предметов, чем есть на самом деле. Функция возращает сколько предметов не удалось забрать (т.е. в случае успеха 0).
Пример использования:
```Ink
~reminder = RemoveFromInventory("кофе", 3)
Ты должен мне еще {reminder} кофе.
```
#### Проверка предметов в инвентаре
```Ink
HowManyItems(name)
```
Возвращает количество предметов с именем `name` в инвентаре.
Пример:
```Ink
~coffee_num = HowManyItems("кофе")
{~coffee_num == 0 : У тебя совсем нет кофе. | Одолжи кофе, приятель.} 
```

### PlayerPrefs
PlayerPrefs нужны для долгосрочного сохранения информации, например, имени игрока, прогресса в игре, или любой информация, которая может понадобиться в долгой перспективе. Эти данные сохраняются между сценами и при повторном запуске игры. Синтаксис аналогичен тому что [есть](https://docs.unity3d.com/ScriptReference/PlayerPrefs.html) в Unity. 
#### Установка значений
```Ink
SetInt(name, value)
SetFloat(name, value)
SetString(name, value)
```
Аргументы: `name` -- имя(для дальнейшего обращения к сохраненному значению). `value` -- сохраняемое значение.
Пример:
```Ink
Какая твоя любимая еда?
    + [макарошки]
        ~SetString("favorite_food", "макарошки")
    + [пюрешка]
        ~SetString("favorite_food", "пюрешка")
```

##### Получение значений
```Ink
GetInt(name, default_value)
GetFloat(name, default_value)
GetString(name, default_value)
```
Аргументы: `name` -- имя, из которого берем значение. `default_value` -- значение на случай, если в запрашиваемой переменной ничего нет.
Пример:
```Ink
Твое любимое блюдо - {~GetString("favorite_food", "котлетки?")}
```

### Проигрывание звуков
```EXTERNAL ~PlaySound(name)```

Работа со звуками вероятно поменяется в дальнешем.

## AI функциональность
Виновник торжества. С помощью этих команд можно генерировать контент в runtime, создавая множество игровых механик. В данный момент есть генерация текста и изображений. Для текста используется GPT от OpenAI, для изображений -- kandinsky от Сбера. Обращение к конкретным моделям происходит на сервере, поэтому потенциально можно использовать любые (в том числе свои) нейросети, не меняя ink-файл или код в Unity.
Генерация контента может занять некоторое время. Для текста чаще всего это не критично, что не скажешь про изображения. Для минимизации времени ожидания наш API предлагает разнести моменты времени начала генерации контента и его использования. Вызывать функции для генерации стоит заранее, как только по ходу истории становится понятно, что данный контент будет необходим. Заранее генерировать абсолютно весь контент тоже вряд ли имеет смысл: возможно игрок не пойдет в ту или иную локацию, или не выберет ту или иную ветку диалога.
### Философия
На мой взгляд концепция бесшовной интеграции ИИ в утилиты для создания сюжета имеет большой потенциал. Думаю стоит ожидать в дальнейшем аналогичных решений. Почему я так думаю? Ink -- по сути инструмент для создания графа истории/сюжета. Граф этот можно построить сколь угодно сложным и запутанным -- инструменты Ink позволяют делать очень много вещей. В то же время, ИИ -- это то что может наполнять этот граф неограниченным контентом. В реальном времени, прямо в процессе игры. При удобной интеграции такого инструмента можно очень сильно обогатить сюжет игры. 
Приведу пример. Можно начать с того что в принципе не использовать ИИ...:flushed: Это будет обычная ВН, понятно, идем дальше. Генерировать контент можно для описания персонажей, место, событий и т.д. На каком-то этапе, предполагаю, ни один человек не догадается что имеет дело с генерацием контента с помощью ИИ. Уже неплохо. Всяко лучше имеющихся в Ink инструментов для вариативности (в основном - рандомизация), т.к. контент заранее не прописан, а значит не ограничен.
Но :open_mouth: можно пойти еще дальше, и тут становится интересно.
Сгенерированный в какой-то момент времени контент можно использовать как угодно, поскольку он записывается в обычные переменные. Его можно использовать для определения хода развития событий. Его даже можно использовать чтобы генерировать новый контент, понимаете? Например AI Dungeon в таком ключе у нас работает из коробки. Мы генерируем короткую предысторию, генерируем к ней варианты ответа и картинку. Затем на основе текущей истории и выборе игрока генерируем новое продолжение, снова генерируем к нему ответы и так по циклу. Граф у такого сюжета примитивный, в Ink это он будет выглядет очень просто. А контента в этом графе будет бесконечно много. Вот такие пироги.

### Общая информация
ИИ начнет генерировать контент в тот момент, когда данные функции вызываются. Поскольку генерация контента может занять какое-то время (особенно для картинок), то вызывать эти функции (кроме AITalk) лучше заранее, чтобы гарантировать что ответ будет точно сгенерирован. Для тектового контента ответ записывается в переменную. Если ИИ не успеет вписать в переменную ответ, то в ней будет храниться исходное значение (логично). Это поведение можно использовать как fallback режим.
Есть также глобальная перменная
```Ink
VAR ai_temperature = 1.2
```
Отвечает за температуру при генерации текста. В целом не вижу смысла ее менять. Но если хочется, то можно менять от реплики к реплике.

### AIAnswer
```Ink
AIAnswer(varName, system, question, max_tokens)
```
Генерирует текст. 
`varName` -- имя переменной, куда записать ответ. Важное отличие от PlayerPrefs: переменная с таким именем обязана существовать. Иначе Ink редактор не скомпилирует ваш файл. 
`system` -- системный промт для ИИ. Определяет поведение ИИ.
`question` -- вопрос для ИИ с позиции user
`max_tokens` -- максимальное число токенов, после которых ответ обрубается. Для OpenAI токены с кириллическими буквами короче чем с латинскими. Насколько я понимаю, 1 токен обычно примерно 1-3 символа. Могу ошибаться. Настраивать лучше эмпирически.

Функция может использоваться для ответов NPC на вопрос. В системе указываем описание персонажа. Можно попросить их что-нибудь описать и потом использовать это в качестве их реплики.

### AIGenerateText
```Ink
AIGenerateText(varName, prompt, max_tokens)
```
На самом деле то же самом что и `AIAnswer`, только нет `system`. Можно использовать для генерации любого контента. Описание местности, случайные события и т.д.

### AIGenerateImage
```Ink
AIGenerateImage(varName, prompt, w, h)
```
Генерирует изображения. 
`w`-- ширина в пикселях
`h` -- высота в пикселях
`prompt` -- описание. 
Сгенерированная картинка сохраняется на компьютер с соответсвующим именем (.jpg добавлять не надо). Потом к ней можно обращаться когда угодно, в том числе при следующем запуске игры. В текущий момент это Kandinsky. 
Рекомендации: разрешение бэкгранда в нашей игре 1280x720. В этом же разрешении по словам разрабом кандинского он лучше картинки делает. Эмпирически выявлено, что для более прикольной картинки лучше добавлять в начало промпта "Задний фон для игры". Но возможно вы найдете более подходящие промты.
Есть также глобальная переменная
```Ink
VAR ai_style = "DEFAULT"
```
Она определяет в каком стиле кандински делает изображения https://cdn.fusionbrain.ai/static/styles/api
Если надо поменять на другой стиль, то можно менять эту переменную в ран-тайме

При обновлении заднего фона указанное в теге имя фона сначала ищется в директории со сгенерированными изображениями. Если находит - значит используется оно. Если нет - то ищется в отдельном массиве у объекта DisplayBackground. Там можно указывать fallback бекграунд для каждого названия на случай отказа ИИ.

## AITalk
```Ink
EXTERNAL AITalk(system, max_tokens)
```
Наконец.
При вызове этой функции сразу включается режим разговора. В `system` указывается с кем говорите (чаще всего описание персонажа, но возможно можно найти нестандартные применения). `max_tokens` можно считать что ограничение на одну итерацию. Имеется в виду что в процессе диалога сохраняется в том числе несколько предыдущих пар (вопрос, ответ). Вот на каждую такую пару будет отводиться указанные max_tokens. Настраивается эмпирически.
Специально для этой функции введены несколько тегов. Их надо (если надо) указывать после вызова функции.
#### Получение предметов
```Ink
# may_receive_items : Деньги, Таблетки от головы
```
Через запятую перечисляются предметы, которые нпс может передать игроку. Даст или не даст -- определяется тоже ии с помощью анализа диалога.
#### Влияние на сюжет
```Ink
# may_affect : "Героя пригласили на концерт" => can_go_to_concert
# may_affect : "Персонаж обиделся" => Женщина_в_форме_рассержена
```
Данный хэштег может быть прописан несколько раз для одного диалога. С помощью него прописывается на какие события может повлиять диалог.  Сначала указывается какое событие может произойти, потом через `=>` указывается имя переменной, куда записать результат (переменная должна существовать, чтобы потом к ней можно было обратиться). Анализ производит тоже ии на основе диалога. Если событие произошло, то в переменную запишется 1. Иначе значение переменной не изменится. После завершения диалога эти переменные можно испольозвать как угодно для определения сюжета игры.
### Общие рекомендации
1. Если для фразы одновременно есть и функция и тег, то сначала надо написать функцию, а ниже теги к фразе. Иначе это превратится в две фразы, одна из которых будет пустая (на ней только вызовется функция). И скорее всего это не ожидаемое поведение.
2. Пробелы между запятыми и двоеточием необязательны.
3. Названия тегов лучше оформлять в фигурные скобки \{ \} для дополнительной проверки ink-редактора на предмет опечаток.
4. Про составление промтов для npc. Мне кажется удобно хранить для каждого персонажа общий большой промт, а по мере сюжета в разных сценах дописывать в него информацию, актуальную для конкретной сцены. Примеры можно посмотреть в уже имеющихся файлах
5. Сохраняй изображения в папке Images для структурированности
6. НИКОГДА...НИКОГДА...Не называй имена файлов кириллическими буквами.

Have fun!

# Вопросы?
@GoshaSerbin, @MiraiRatchet в Telegram





