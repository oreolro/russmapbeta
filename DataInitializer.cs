using UnityEngine;
using System.Collections.Generic;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class DataInitializer
{
#if UNITY_EDITOR
    [MenuItem("🗺️ RUSSKY MAP / Сгенерировать базу данных")]
    public static void GenerateRusskyMapData()
    {
        string folderPath = "Assets/RusskyMapData";
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        CreateLocation(1, 43.049722, 131.881389, 50f,
            "Форт Поспелова (Укрепление №4)", 
            "Fort Pospelov (Fortress №4)", 
            "波斯佩洛夫堡垒 (4号防御工事)",
            "Построен в 1900–1904 гг. на Поспеловском хребте (150 м). Защищал пролив Босфор Восточный от японского флота. В советские годы — секретный склад боеприпасов ТОФ, благодаря чему объект идеально сохранился до наших дней.",
            "Built in 1900–1904 on the Pospelov Ridge (150m). Protected the Eastern Bosphorus Strait from the Japanese fleet. During the Soviet era, it served as a secret Pacific Fleet ammunition depot, which perfectly preserved the site.",
            "建于1900-1904年间，位于波斯佩洛夫山脊（海拔150米）。曾在日俄战争期间用于防御日本舰队突破东博斯普鲁斯海峡。苏联时期作为太平洋舰队的秘密弹药库，得益于此，该建筑完好地保存至今。",
            "Найдите казематированный кофр и разверните в орудийных двориках AR-модели 9-фунтовых пушек образца 1867 года.",
            "Find the casemated caponier and deploy AR models of 9-pound cannons (1867 model) in the gun yards.",
            "找到带有掩体的工事，并在炮兵阵地中部署1867年型9磅大炮的AR模型。");

        CreateLocation(2, 43.064167, 131.890556, 40f,
            "Новосильцевская батарея", 
            "Novosiltsevskaya Battery", 
            "诺沃西尔采夫角炮台",
            "Построена в бетоне в 1897–1903 гг. под руководством инженера Эрнеста Маака прямо под современным Русским мостом. Ее задача — перекрывать огнем самый узкий участок пролива. Цемент для стройки везли морем из Новороссийска.",
            "Built in concrete in 1897–1903 under engineer Ernest Maack, right under the modern Russian Bridge. Its tactical mission was to control the narrowest part of the strait. Cement was delivered by sea from Novorossiysk.",
            "于1897-1903年间在工程师埃尔内斯特·马克的指导下修筑为混凝土结构，正位于现代俄罗斯大桥下方。其任务是用火力封锁海峡最窄的水域。当时修筑所用的水泥是专门通过海运从新罗西斯克运来的。",
            "Зарядите виртуальным снарядом весом 41.4 кг знаменитую 6-дюймовую пушку Канэ и наведите ее на цель в проливе.",
            "Load the famous 6-inch Canet gun with a virtual 41.4 kg shell and aim it at a target in the strait.",
            "为著名的6英寸戛纳大炮装填41.4公斤的虚拟炮弹，并瞄准海峡水域中的目标。");

        CreateLocation(3, 42.981389, 131.914444, 60f,
            "Форт №11 (Князя Святослава Игоревича)", 
            "Fort №11 (Prince Svyatoslav)", 
            "11号堡垒 (斯维亚托斯拉夫·伊戈列维奇大公堡)",
            "Шедевр мировой фортификации 'Проекта 1910 года'. Главный инженер Алексей Шошин спроектировал его так, чтобы он полностью сливался с рельефом. Строительство было остановлено революцией 1917 года.",
            "A masterpiece of world fortification from the '1910 Project'. Chief engineer Alexey Shoshin designed it to blend into the terrain. Construction was halted by the 1917 Revolution.",
            "“1910年计划”的世界筑垒杰作。总工程师阿列克谢·肖申将其设计为完全与山体地形融为一体。1917年的十月革命导致该工程在完成90%后被迫停工。",
            "Исследуйте бетонные потерны и найдите скрытые AR-чертежи и клейма царских военных инженеров.",
            "Explore the concrete tunnels and find hidden AR blueprints and stamps of tsarist military engineers.",
            "探索混凝土坑道，寻找沙皇俄国军事工程师隐藏的AR图纸和印章。");

        CreateLocation(4, 42.996111, 131.931944, 60f,
            "Форт №12 (Князя Владимира Великого)", 
            "Fort №12 (Prince Vladimir)", 
            "12号堡垒 (弗拉基米尔大公堡)",
            "Построен в форме 'бабочки' на горе Ахлестышева. В условиях Первой мировой войны финансирование урезали. Самый мистический элемент форта — подземный бетонный киот, где молились солдаты перед уходом на фронт.",
            "Built in a 'butterfly' shape on Mount Akhlestyshev. Financing was cut due to WWI. The most mystical element is an underground concrete shrine where soldiers prayed before leaving for the front.",
            "位于阿赫列斯特舍夫山上，平面呈“蝴蝶”形。在一战爆发的背景下资金被削减。堡垒中最神秘的元素是地下混凝土神龛，士兵们在开赴前线前曾在此向大天使米哈伊尔祈祷。",
            "Наведите камеру на пустой бетонный киот, чтобы зажечь виртуальные свечи и проявить икону Архангела Михаила.",
            "Aim your camera at the empty concrete shrine to light virtual candles and reveal the icon of Archangel Michael.",
            "将摄像头对准空无一物的混凝土神龛，点燃虚拟蜡烛，使大天使米哈伊尔的圣像显现。");

        CreateLocation(5, 42.984167, 131.893333, 50f,
            "Ворошиловская батарея (Береговая батарея №26)", 
            "Voroshilov Battery", 
            "伏罗希洛夫炮台 (第26海岸炮台)",
            "Построена в 1931–1934 гг. Вооружена двумя трехорудийными башнями калибра 305 мм с линкора 'Полтава'. Под землей скрыты три этажа казематов, уходящих в скалу. Батарея закрыла Владивосток от нападения Японии.",
            "Built in 1931–1934. Armed with two triple 305-mm gun turrets from the battleship 'Poltava'. Three floors of casemates are hidden underground. The battery successfully deterred Japanese attacks.",
            "建于1931-1934年，以应对日本建立伪满洲国的威胁。装备了两座来自“波尔塔瓦”号战列舰的305毫米三联装主炮塔。地下深挖了五层楼深的掩体（每座炮塔地下有3层）。它的存在完全震慑了日本舰队。",
            "Включите рентген-режим AR, чтобы заглянуть на 3 этажа под землю и увидеть механизмы подачи снарядов.",
            "Turn on AR X-ray mode to look 3 floors underground and see the shell loading mechanisms.",
            "开启AR透视模式，透视地下3层结构，观察炮弹和发射药从地下仓库升至炮塔的机械运作。");

        CreateLocation(6, 42.943111, 131.871222, 100f,
            "Мыс Тобизина", 
            "Cape Tobizin", 
            "托比济纳海角",
            "Самый южный мыс острова, сложенный из обрывистых слоистых скал юрского периода, напоминающих плиты. Известен своим старым навигационным маяком и дикими лисами, которые часто выходят к туристам.",
            "The southernmost cape of the island, formed by steep layered Jurassic rocks resembling stone plates. Famous for its old navigation beacon and wild foxes that frequently meet tourists.",
            "俄罗斯岛最南端的海角，由侏罗纪时期的陡峭分层岩石构成，形似巨大的石板。这里因古老的导航灯塔以及经常向游客讨要食物的野生狐狸而闻名。",
            "Найдите цифровую AR-лису на тропе и ответьте на ее исторические вопросы, чтобы пройти дальше.",
            "Find the digital AR fox on the trail and answer its historical trivia questions to proceed.",
            "在步道上找到数字AR狐狸，回答它提出的历史问题以继续前进。");

        CreateLocation(7, 42.940278, 131.895556, 100f,
            "Мыс Вятлина", 
            "Cape Vyatlin", 
            "维亚特利纳海角",
            "Каменистый мыс с крутыми обрывами. Популярен благодаря уникальному арт-объекту под открытым небом: туристы и местные жители строят на галечном пляже сотни многоярусных каменных башен (каирнов).",
            "A rocky cape with steep cliffs. Popular due to a unique open-air art project: tourists and locals build hundreds of multi-tiered stone towers (cairns) on the pebble beach.",
            "地势险峻的岩石海角。因独特的海滩露天艺术景观而闻名：游客 and 当地居民在卵石滩上用石头垒起数百座多层的石塔（凯恩石堆）。",
            "Сложите на пляже свою собственную виртуальную каменную башню и закрепите ее по GPS для других игроков.",
            "Build your own virtual stone tower on the beach and lock it via GPS for other players to see.",
            "在海滩上垒起你自己的虚拟石塔，并通过GPS为其他游戏玩家进行数字标记。");

        CreateLocation(8, 43.024444, 131.894444, 200f,
            "Кампус ДВФУ и бухта Аякс (манз. Вай-Хай-Вай)", 
            "FEFU Campus & Ayax Bay", 
            "远东联邦大学校园与阿雅克斯湾 (外海卫)",
            "Построен к саммиту АТЭС-2012. Набережная бухты Аякс — центр студенческой жизни. В XIX веке китайские отходники (манзы) добывали здесь трепанга и называли бухту 'Вай-Хай-Вай'.",
            "Built for the APEC 2012 summit. The Ayax Bay waterfront is the heart of student life. In the XIX century, Chinese settlers (manzas) harvested sea cucumbers here and called the bay 'Wai-Hai-Wai'.",
            "为2012年APEC峰会而建。阿雅克斯湾的海滨长廊是校园生活的核心。19世纪时，当地的中国居民（满族/大头人）在此捕捞海参，并将该海湾称为“外海卫”。",
            "Найдите следы старинной китайской фанзы и расшифруйте маньчжурский медицинский рецепт в AR.",
            "Find traces of an old Chinese house (fanza) and decode a Manchu medical recipe in AR.",
            "寻找古老中国房屋（方子）的遗迹，并在AR中破译一份满族的传统药方。");

        CreateLocation(9, 43.018333, 131.926389, 150f,
            "Приморский океанариум", 
            "Primorsky Aquarium", 
            "滨海边疆区海洋馆",
            "Научно-образовательный комплекс на полуострове Житкова. Главное здание выполнено в форме гигантской раковины двустворчатого моллюска и вмещает более 130 аквариумов с фауной всех океанов.",
            "A scientific and educational complex on the Zhitkov Peninsula. The main building is shaped like a giant bivalve shell and houses over 130 aquariums with fauna from all world oceans.",
            "位于日特科夫半岛上的大型科学教育综合体。主建筑外观设计为巨大的双壳贝壳形状，内部拥有130多个水族箱，展示着来自世界各大洋的海洋生物。",
            "Соберите интерактивную карту миграции китов вокруг острова Русский в режиме дополненной реальности.",
            "Assemble an interactive whale migration map around Russky Island in augmented reality mode.",
            "在增强现实模式下，拼凑出一幅围绕俄罗斯岛的鲸鱼迁徙互动地图。");

        CreateLocation(10, 43.063889, 131.904444, 300f,
            "Русский мост", 
            "Russky Bridge", 
            "俄罗斯大桥",
            "Самый длинный вантовый мост в мире (центральный пролет — 1104 м, высота пилонов — 324 м). Открыт в 2012 году. Стал визитной карточкой Владивостока и изображен на купюре номиналом 2000 рублей.",
            "The longest cable-stayed bridge in the world (central span — 1104m, pylon height — 324m). Opened in 2012. It became Vladivostok's trademark and is featured on the 2000-ruble banknote.",
            "世界上中央跨度最长的斜拉桥（中心跨长1104米，主塔高324米）。于2012年通车，现已成为符拉迪沃斯托克的城市名片，并被印在2000卢布面额的纸币上。",
            "Узнайте историю кругосветного путешествия 800 петроградских детей, которых эвакуировали через этот пролив в 1920 году.",
            "Learn the story of the round-the-world journey of 800 Petrograd children evacuated through this strait in 1920.",
            "了解1920年通过该海峡疏散的800名彼得格勒儿童进行环球旅行的历史故事。");

AssetDatabase.SaveAssets();
AssetDatabase.Refresh();
Debug.Log("🎉 База данных острова Русский успешно сгенерирована через верхнее меню!");
}
private static void CreateLocation(int id, double lat, double lon, float radius,
string nameRU, string nameEN, string nameZH,
string histRU, string histEN, string histZH,
string qRU, string qEN, string qZH)
{
HistoricalLocation asset = ScriptableObject.CreateInstance<HistoricalLocation>();
asset.locationId = id;
asset.latitude = lat;
asset.longitude = lon;
asset.triggerRadiusMeter = radius;
asset.nameRU = nameRU;
asset.nameEN = nameEN;
asset.nameZH = nameZH;
asset.historyRU = histRU;
asset.historyEN = histEN;
asset.historyZH = histZH;
asset.questTaskRU = qRU;
asset.questTaskEN = qEN;
asset.questTaskZH = qZH;
string assetPath = $"Assets/RusskyMapData/Location_{id}.asset";
AssetDatabase.CreateAsset(asset, assetPath);
}
#endif
}