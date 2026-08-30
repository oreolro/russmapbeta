using System.Collections.Generic;

public static class QuestDatabase
{
    public static List<QuestQuestion> GetQuestionsForLocation(int locationId)
    {
        List<QuestQuestion> list = new List<QuestQuestion>();

        switch (locationId)
        {
            case 1: // Форт Поспелова
                list.Add(new QuestQuestion
                {
                    questionId = 1,
                    questionRU = "Какую функцию выполнял Форт Поспелова в советские годы, благодаря чему идеально сохранился?",
                    questionEN = "What function did Fort Pospelov perform during the Soviet years, which perfectly preserved it?",
                    questionZH = "波斯佩洛夫堡垒在苏联时期承担 letter 什么功能，使其得以完美保存至今？",
                    optionsRU = new string[] { "Секретный склад боеприпасов ТОФ", "Санаторий для офицеров", "Школа радистов", "Подземная тюрьма" },
                    optionsEN = new string[] { "Secret Pacific Fleet ammunition depot", "Sanatorium for officers", "Radio operators school", "Underground prison" },
                    optionsZH = new string[] { "太平洋舰队秘密弹药库", "军官疗养院", "无线电报员学校", "地下监狱" },
                    correctAnswerIndex = 0
                });
                break;

            case 2: // Новосильцевская батарея
                list.Add(new QuestQuestion
                {
                    questionId = 2,
                    questionRU = "Откуда везли цемент морем для строительства Новосильцевской батареи в 1897 году?",
                    questionEN = "Where was the cement delivered from by sea for the construction of the Novosiltsevskaya Battery in 1897?",
                    questionZH = "1897年修筑诺沃西尔采夫角炮台时，所用的水泥是通过海运从哪里运来的？",
                    optionsRU = new string[] { "Из Новороссийска", "Из Санкт-Петербурга", "Из Шанхая", "Из Нагасаки" },
                    optionsEN = new string[] { "From Novorossiysk", "From St. Petersburg", "From Shanghai", "From Nagasaki" },
                    optionsZH = new string[] { "新罗西斯克", "圣彼得堡", "上海", "长崎" },
                    correctAnswerIndex = 0
                });
                break;

            case 3: // Форт №11
                list.Add(new QuestQuestion
                {
                    questionId = 3,
                    questionRU = "Кто был главным инженером Форта №11, спроектировавшим его как шедевр мировой фортификации?",
                    questionEN = "Who was the chief engineer of Fort No. 11, who designed it as a masterpiece of world fortification?",
                    questionZH = "11号堡垒的总工程师是谁？他将其设计为世界筑垒工程的杰作。",
                    optionsRU = new string[] { "Алексей Шошин", "Эрнест Маак", "Константин Поспелов", "Барон Фон Вятлин" },
                    optionsEN = new string[] { "Alexey Shoshin", "Ernest Maack", "Konstantin Pospelov", "Baron Von Vyatlin" },
                    optionsZH = new string[] { "阿列克谢·肖申", "埃尔内斯特·马克", "康斯坦丁·波斯佩洛夫", "冯·维亚特利纳男爵" },
                    correctAnswerIndex = 0
                });
                break;

            case 4: // Форт №12
                list.Add(new QuestQuestion
                {
                    questionId = 4,
                    questionRU = "Какое уникальное подземное бетонное сооружение форта №12 служило местом молитвы солдат?",
                    questionEN = "What unique underground concrete structure of Fort No. 12 served as a place of prayer for soldiers?",
                    questionZH = "12号堡垒中哪一个独特的地下混凝土结构曾作为士兵们祈祷的地方？",
                    optionsRU = new string[] { "Подземный киот", "Орудийный каземат", "Потерна глубокого заложения", "Командный бункер" },
                    optionsEN = new string[] { "Underground concrete shrine", "Gun casemate", "Deep tunnel", "Command bunker" },
                    optionsZH = new string[] { "地下神龛", "火炮掩体", "深埋坑道", "指挥掩体" },
                    correctAnswerIndex = 0
                });
                break;

            case 5: // Ворошиловская батарея
                list.Add(new QuestQuestion
                {
                    questionId = 5,
                    questionRU = "С какого знаменитого корабля были сняты гигантские 305-мм башни для Ворошиловской батареи?",
                    questionEN = "From which famous ship were the giant 305-mm turrets taken for the Voroshilov Battery?",
                    questionZH = "伏罗希洛夫炮台的两座巨型305毫米主炮塔是从哪艘著名的战列舰上拆卸卸装下来的？",
                    optionsRU = new string[] { "Линкор 'Полтава'", "Крейсер 'Варяг'", "Броненосец 'Потёмкин'", "Линкор 'Марат'" },
                    optionsEN = new string[] { "Battleship 'Poltava'", "Cruiser 'Varyag'", "Battleship 'Potemkin'", "Battleship 'Marat'" },
                    optionsZH = new string[] { "“波尔塔瓦”号战列舰", "“瓦良格”号巡洋舰", "“波将金”号战列舰", "“马拉”号战列舰" },
                    correctAnswerIndex = 0
                });
                break;

            case 6: // Мыс Тобизина
                list.Add(new QuestQuestion
                {
                    questionId = 6,
                    questionRU = "Из каких геологических пород сложены обрывистые слоистые скалы мыса Тобизина?",
                    questionEN = "What geological rocks form the steep layered cliffs of Cape Tobizin?",
                    questionZH = "托比济纳海角陡峭的分层岩石属于哪一个地质时期的产物？",
                    optionsRU = new string[] { "Скалы Юрского периода", "Меловые отложения", "Вулканический туф", "Древний гранит" },
                    optionsEN = new string[] { "Jurassic rocks", "Chalk deposits", "Volcanic tuff", "Ancient granite" },
                    optionsZH = new string[] { "侏罗纪时期岩石", "白垩纪沉积物", "火山角砾岩", "古老 granite" },
                    correctAnswerIndex = 0
                });
                break;

            case 7: // Мыс Вятлина
                list.Add(new QuestQuestion
                {
                    questionId = 7,
                    questionRU = "Какой уникальный народный арт-объект под открытым небом создают туристы на галечном пляже мыса Вятлина?",
                    questionEN = "What unique open-air art project do tourists create on the pebble beach of Cape Vyatlin?",
                    questionZH = "游客和当地居民在维亚特利纳海角的卵石滩上创造了哪种独特的露天艺术景观？",
                    optionsRU = new string[] { "Многоярусные каменные башни (каирны)", "Наскальные рисунки", "Деревянные идолы", "Песчаные замки" },
                    optionsEN = new string[] { "Multi-tiered stone towers (cairns)", "Rock paintings", "Wooden idols", "Sandcastles" },
                    optionsZH = new string[] { "多层石塔（凯恩石堆）", "岩画", "木雕神像", "沙雕城堡" },
                    correctAnswerIndex = 0
                });
                break;

            case 8: // Кампус ДВФУ
                list.Add(new QuestQuestion
                {
                    questionId = 8,
                    questionRU = "Как называли китайские отходники (манзы) бухту Аякс в XIX веке, добывая здесь трепанга?",
                    questionEN = "What did Chinese settlers (manzas) call Ayax Bay in the 19th century while harvesting sea cucumbers?",
                    questionZH = "19世纪中国居民（满族/大头人）在此捕捞海参时，将今天的阿雅克斯湾称为什么？",
                    optionsRU = new string[] { "Вай-Хай-Вай", "Хайшэньвай", "Фанза-Бэй", "Манзы-Пролив" },
                    optionsEN = new string[] { "Wai-Hai-Wai", "Haishenwai", "Fanza Bay", "Manza Strait" },
                    optionsZH = new string[] { "外海卫", "海参崴", "方子湾", "蛮子海峡" },
                    correctAnswerIndex = 0
                });
                break;

            case 9: // Приморский океанариум
                list.Add(new QuestQuestion
                {
                    questionId = 9,
                    questionRU = "В форме какого природного объекта выполнено главное здание Приморского океанариума?",
                    questionEN = "The main building of the Primorsky Aquarium is shaped like what natural object?",
                    questionZH = "滨海边疆区海洋馆的主建筑外观采用的是哪种自然生物的外形设计？",
                    optionsRU = new string[] { "Гигантская раковина двустворчатого моллюска", "Волна цунами", "Синий кит", "Коралловый риф" },
                    optionsEN = new string[] { "Giant bivalve mollusk shell", "Tsunami wave", "Blue whale", "Coral reef" },
                    optionsZH = new string[] { "巨大的双壳贝壳形状", "海啸波浪", "蓝鲸", "珊瑚礁" },
                    correctAnswerIndex = 0
                });
                break;

            case 10: // Русский мост
                list.Add(new QuestQuestion
                {
                    questionId = 10,
                    questionRU = "На какой российской денежной купюре изображен Русский мост, открытый в 2012 году?",
                    questionEN = "On which Russian banknote is the Russian Bridge featured, opened in 2012?",
                    questionZH = "2012年通车的俄罗斯大桥作为符拉迪沃斯托克的城市名片，被印在了哪种面额的卢布纸币上？",
                    optionsRU = new string[] { "2000 рублей", "1000 рублей", "5000 рублей", "200 рублей" },
                    optionsEN = new string[] { "2000 rubles", "1000 rubles", "5000 rubles", "200 rubles" },
                    optionsZH = new string[] { "2000 卢布", "1000 卢布", "5000 卢布", "200 卢布" },
                    correctAnswerIndex = 0
                });
                break;
        }

        return list;
    }
}
