using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

Random r = new Random();

using var cts = new CancellationTokenSource();
var bot = new TelegramBotClient("7945123194:AAEYbVWJOgkT9HuofFZRvVdb75XfD12p2nU", cancellationToken: cts.Token);
var me = await bot.GetMeAsync();
bot.OnError += OnError;
bot.OnMessage += OnMessage;
bot.OnUpdate += OnUpdate;


Console.WriteLine($"@{me.Username} is running... Press Enter to terminate");
Console.ReadLine();
cts.Cancel(); // stop the bot

// method to handle errors in polling or in your OnMessage/OnUpdate code
async Task OnError(Exception exception, HandleErrorSource source)
{
    Console.WriteLine(exception); // just dump the exception to the console
}

// method that handle messages received by the bot:
async Task OnMessage(Message msg, UpdateType type)
{
    if (msg.Text == "/start")
    {
        await bot.SendTextMessageAsync(msg.Chat,
            $"Привет, {msg.From.FirstName}! Я здесь, чтобы дать вам совет, как оправдаться перед друзьями за то, что не успели досмотреть сериал.");
        await bot.SendTextMessageAsync(msg.Chat, "Кого будем винить?",
            replyMarkup: new InlineKeyboardMarkup()
            .AddButton("Сериал", "1")
            .AddNewRow()
            .AddButton("Ближайшее окружение", "2")
            .AddNewRow()
            .AddButton("Вселенную в целом", "3")
            .AddNewRow()
            .AddButton("Случайный совет, одна штука", Convert.ToString(r.Next(1,4))));
    }
}

// method that handle other types of updates received by the bot:
async Task OnUpdate(Update update)
{
    if (update is { CallbackQuery: { } query }) // non-null CallbackQuery
    {
        var chat = query.Message.Chat;
        switch (query.Data)
        {
            case "1":
                {
                    await bot.AnswerCallbackQueryAsync(query.Id);
                    await bot.SendTextMessageAsync(
                                chat.Id,
                                "Скажите, что сериал слишком длинный. Что чем дальше, тем сюжет становится скучнее. " +
                                "Что начало было захватывающим, но продолжение разочаровало. " +
                                "Выпустите погулять своего внутреннего критика! " +
                                "Залейте комнату слезами ностальгии, и пусть количество уступит место качеству!");

                    await bot.SendVideoAsync(chat.Id, "https://telegrambots.github.io/book/docs/video-hawk.mp4");
                    return;
                }
            case "2":
                {
                    await bot.AnswerCallbackQueryAsync(query.Id);
                    await bot.SendTextMessageAsync(
                                chat.Id,
                                "Расскажите о том, как нелегко вам живётся. " +
                                "Что жена пилит, дети просят сделать за них домашку, начальник оставляет на работе до утра. " +
                                "Никто вас не понимает, кроме друзей, и что с этим поделать? " +
                                "Остаётся только досматривать сериал вместе)");
                    await bot.SendStickerAsync(chat.Id, "https://telegrambots.github.io/book/docs/sticker-dali.webp");
                    return;
                }
            case "3":
                {
                    await bot.AnswerCallbackQueryAsync(query.Id);
                    await bot.SendTextMessageAsync(
                                chat.Id,
                                "Сама вселенная и господь Бог против ваших планов! " +
                                "Чаинки и кофейная гуща собираются в жуткие узоры. " +
                                "Гороскопы и гадалки хором твердят бросить глупую затею. " +
                                "Как только вы садитесь, отрубает свет во всём городе, и молния ударяет прямо в окно. " +
                                "Но вы герой! Вы смогли отвоевать у мира целых... Ну сколько вы там посмотрели? " +
                                "Это и скажите друзьям.");
                    await bot.SendPhotoAsync(chat.Id, "https://i.pinimg.com/originals/ef/7b/97/ef7b9724ad06cd6dfce92193e95a5caa.jpg");
                    return;
                }
        }
    }
}
