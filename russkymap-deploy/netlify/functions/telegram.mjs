/* Серверная функция Telegram-бота RusskyMap.
   Токен — в переменной окружения TELEGRAM_BOT_TOKEN (Netlify → Environment variables → Production).

   Умеет:
   - /start — приветствие на языке пользователя + постоянная кнопка-приложение;
   - /id    — код входа для клиентов Telegram, которые не отдают профиль приложению
              (Telegram Web и подобные). Код кладётся в таблицу tg_link (Supabase),
              живёт 10 минут; сам код отправляется отдельным сообщением в моноширинном
              блоке — в Telegram он копируется одним тапом.

   Прогрев от «холодного старта» делает само приложение при открытии
   (тихий GET-пинг) — расписания Netlify на бесплатном тарифе не работают,
   поэтому от них отказались. */

const TOKEN = process.env.TELEGRAM_BOT_TOKEN || "";

const SUPABASE_URL = "https://iabjijogvkydhhmpnkhm.supabase.co";
const SUPABASE_ANON = "sb_publishable_iwI_U1sCMiOrbKZR3KR14w_6V1hwD9A";

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
      "Жми кнопку ниже — и в путь!\n\n" +
      "🔑 Если в приложении ты виден как «Турист» и не можешь войти " +
      "(такое бывает в Telegram Web на компьютере) — отправь мне команду /id: " +
      "я пришлю код из 6 символов, введёшь его в приложении и профиль подтвердится.",
    button: "🗺 Открыть гид",
    idIntro: "🔑 Твой код входа — ниже. Нажми на него, чтобы скопировать:",
    idOutro:
      "Открой приложение → Профиль → введи код.\n" +
      "Код живёт 10 минут и привязан к твоему аккаунту. Никому его не показывай.",
    hint: "Я пока понимаю немного: /start — приветствие, /id — код входа в приложение. 😉",
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
      "Tap the button below and let's go!\n\n" +
      "🔑 If the app shows you as “Tourist” and won't let you in " +
      "(this happens in Telegram Web on desktop) — send me /id: " +
      "I'll reply with a 6-character code, enter it in the app and your profile is verified.",
    button: "🗺 Open the guide",
    idIntro: "🔑 Your login code is below. Tap it to copy:",
    idOutro:
      "Open the app → Profile → enter the code.\n" +
      "The code lives 10 minutes and is bound to your account. Keep it secret.",
    hint: "I only know a few tricks so far: /start for hello, /id for a login code. 😉",
  },
  zh: {
    welcome:
      "🌊 你好！欢迎来到 RusskyMap。\n\n" +
      "我们是一个学生项目，把俄罗斯岛变成一本鲜活的历史书：" +
      "符拉迪沃斯托克要塞的13个地标——海岸炮台、堡垒与海岬——配有地图、路线、AR照片和探险任务。\n\n" +
      "🥾 用脚步丈量海岛，比刷手机有趣多了。\n" +
      "🗣 俄语、英语、中文——一起探索，没有语言障碍。\n" +
      "🏆 打卡地点、赚取积分、冲上共享排行榜。\n\n" +
      "点击下方按钮，出发吧！\n\n" +
      "🔑 如果应用里显示为“游客”而无法进入" +
      "（在电脑网页版 Telegram 中会出现这种情况）——请发送 /id：" +
      "我会回复一个6位验证码，在应用中输入即可验证身份。",
    button: "🗺 打开向导",
    idIntro: "🔑 您的验证码如下，点击即可复制：",
    idOutro: "打开应用 → 个人资料 → 输入验证码。\n验证码10分钟内有效，仅限您的账号使用，请勿告诉他人。",
    hint: "我目前只会几招：/start 打招呼，/id 获取登录验证码。😉",
  },
};

function pickMessages(langCode) {
  const lc = (langCode || "").toLowerCase();
  if (lc.startsWith("zh")) return MESSAGES.zh;
  if (lc.startsWith("ru")) return MESSAGES.ru;
  return MESSAGES.en;
}

function makeCode() {
  /* без легко путаемых символов (0/O, 1/I/L) */
  const abc = "ABCDEFGHJKMNPQRSTUVWXYZ23456789";
  let s = "";
  for (let i = 0; i < 6; i++) s += abc[Math.floor(Math.random() * abc.length)];
  return s;
}

async function saveLinkCode(code, userId, name, username) {
  const res = await fetch(`${SUPABASE_URL}/rest/v1/tg_link`, {
    method: "POST",
    headers: {
      apikey: SUPABASE_ANON,
      Authorization: `Bearer ${SUPABASE_ANON}`,
      "Content-Type": "application/json",
      Prefer: "return=minimal",
    },
    body: JSON.stringify({ code, user_id: String(userId), name, username }),
  });
  return res.ok;
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
    /* keep-alive по расписанию приходит без понятного тела — просто отвечаем ок */
    return new Response("ok", { status: 200 });
  }

  /* адрес сайта — из окружения Netlify или из URL запроса */
  const siteUrl = (process.env.URL || new URL(req.url).origin).replace(/\/$/, "");

  const message = update.message;
  const callback = update.callback_query;
  const from = message?.from || callback?.from;
  const chatId = message?.chat?.id ?? callback?.message?.chat?.id;

  /* служебные вызовы (расписание, callback без чата) — тихо выходим */
  if (chatId == null) {
    return new Response("ok", { status: 200 });
  }

  const m = pickMessages(from?.language_code);

  if (callback) {
    await callTelegram("answerCallbackQuery", { callback_query_id: callback.id });
  }

  const text = typeof message?.text === "string" ? message.text.trim() : "";

  if (text.startsWith("/id")) {
    const code = makeCode();
    const name = [from?.first_name, from?.last_name].filter(Boolean).join(" ") || "—";
    const saved = await saveLinkCode(code, from?.id, name, from?.username || "");
    if (saved) {
      /* 1) подводка  2) сам код моноширинным блоком (копируется одним тапом)  3) инструкция */
      await callTelegram("sendMessage", { chat_id: chatId, text: m.idIntro });
      await callTelegram("sendMessage", {
        chat_id: chatId,
        text: `<code>${code}</code>`,
        parse_mode: "HTML",
      });
      await callTelegram("sendMessage", { chat_id: chatId, text: m.idOutro });
    } else {
      await callTelegram("sendMessage", { chat_id: chatId, text: m.hint });
    }
    return new Response("ok", { status: 200 });
  }

  /* /start (включая системную кнопку START) и первый запуск без текста */
  const isStart = text.startsWith("/start");
  if (isStart || !text) {
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
    return new Response("ok", { status: 200 });
  }

  /* прочие команды — лёгкая подсказка */
  if (text.startsWith("/")) {
    await callTelegram("sendMessage", { chat_id: chatId, text: m.hint });
  }

  return new Response("ok", { status: 200 });
};
