/* Серверная функция Telegram-бота RusskyMap.
   Хранит токен в переменной окружения TELEGRAM_BOT_TOKEN (Netlify → Environment variables).
   Отвечает на /start приветствием на языке пользователя и вешает постоянную
   кнопку-приложение, открывающую Mini App по адресу сайта. */

const TOKEN = process.env.TELEGRAM_BOT_TOKEN || "";

const MESSAGES = {
  ru: {
    welcome:
      "🌊 Привет! Ты в RusskyMap.\n\n" +
      "Мы — студенческий проект, который превращает остров Русский в живую историю: " +
      "13 точек Владивостокской крепости, береговые батареи, форты и мысы — с картой, " +
      "маршрутами, AR-фото и квестами.\n\n" +
      "🥾 Прогулки вместо сидения дома — наш любимый вид спорта.\n" +
      "🗣 Русский, English, 中文 — гуляем вместе без языковых барьеров.\n" +
      "🏆 Проходи точки, собирай очки и следи за общим рейтингом.\n\n" +
      "Жми кнопку ниже — и в путь!",
    button: "🗺 Открыть гид",
  },
  en: {
    welcome:
      "🌊 Hey! Welcome to RusskyMap.\n\n" +
      "We are a student project that turns Russky Island into a living history book: " +
      "13 landmarks of the Vladivostok Fortress — coastal batteries, forts and capes — " +
      "with a map, routes, AR photos and quests.\n\n" +
      "🥾 Walking beats scrolling — that's our kind of sport.\n" +
      "🗣 Русский, English, 中文 — we explore together, no language barriers.\n" +
      "🏆 Clear locations, earn points and climb the shared leaderboard.\n\n" +
      "Tap the button below and let's go!",
    button: "🗺 Open the guide",
  },
  zh: {
    welcome:
      "🌊 你好！欢迎来到 RusskyMap。\n\n" +
      "我们是一个学生项目，把俄罗斯岛变成一本鲜活的历史书：" +
      "符拉迪沃斯托克要塞的13个地标——海岸炮台、堡垒与海岬——配有地图、路线、AR照片和探险任务。\n\n" +
      "🥾 用脚步丈量海岛，比刷手机有趣多了。\n" +
      "🗣 俄语、英语、中文——一起探索，没有语言障碍。\n" +
      "🏆 打卡地点、赚取积分、冲上共享排行榜。\n\n" +
      "点击下方按钮，出发吧！",
    button: "🗺 打开向导",
  },
};

function pickMessages(langCode) {
  const lc = (langCode || "").toLowerCase();
  if (lc.startsWith("zh")) return MESSAGES.zh;
  if (lc.startsWith("ru")) return MESSAGES.ru;
  return MESSAGES.en;
}

async function callTelegram(method, body) {
  const res = await fetch(`https://api.telegram.org/bot${TOKEN}/${method}`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(body),
  });
  return res.json();
}

export default async (req) => {
  if (req.method !== "POST") {
    return new Response("RusskyMap bot is alive", { status: 200 });
  }
  if (!TOKEN) {
    return new Response("missing TELEGRAM_BOT_TOKEN", { status: 200 });
  }

  let update;
  try {
    update = await req.json();
  } catch {
    return new Response("ok", { status: 200 });
  }

  /* адрес сайта — из окружения Netlify или из URL запроса */
  const siteUrl = (process.env.URL || new URL(req.url).origin).replace(/\/$/, "");

  const message = update.message;
  const callback = update.callback_query;
  const from = message?.from || callback?.from;
  const chatId = message?.chat?.id ?? callback?.message?.chat?.id;

  if (callback) {
    /* просто подтверждаем нажатие, чтобы у пользователя не крутились «часики» */
    await callTelegram("answerCallbackQuery", { callback_query_id: callback.id });
  }

  /* отвечаем на /start (включая системную кнопку START) и на первый запуск */
  const isStart =
    !!message &&
    (typeof message.text === "string" ? message.text.trim().startsWith("/start") : false);

  if (chatId != null && (isStart || !message?.text)) {
    const m = pickMessages(from?.language_code);
    await callTelegram("sendMessage", {
      chat_id: chatId,
      text: m.welcome,
      reply_markup: {
        keyboard: [
          [
            {
              text: m.button,
              web_app: { url: siteUrl },
            },
          ],
        ],
        resize_keyboard: true,
      },
    });
  }

  return new Response("ok", { status: 200 });
};
