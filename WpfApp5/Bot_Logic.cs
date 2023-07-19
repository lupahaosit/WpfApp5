using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace WpfApp5
{
    internal class Bot_Logic
    {
        Context context = new Context();
        string mes = "s";
        string requist;
        enum CallBackQuearyRequist
        {
            add,
            remove,
            subsribes
        }
        public Bot_Logic()
        {

            var botClient = new TelegramBotClient("6379278902:AAGPVLMeVETQULyCEMADXQy8QJMxZINafgw");
            StaticFunctions.staticClient = botClient;
            Thread thread = new Thread(IntervalUpdate);
            thread.Start();
            botClient.StartReceiving(updateHandler: UpdateHandler, pollingErrorHandler: PollingErrorHandler);
        }


        public static InlineKeyboardMarkup CreateTextButtonsList(List<string> items, string workMode)
        {

            InlineKeyboardButton[][] buttons = new InlineKeyboardButton[items.Count][];
            int count = 0;
            switch (workMode)
            {
                case "add":
                    for (int i = 0; i < items.Count(); i++)
                    {
                        buttons[count] = new[]
                        {
                            InlineKeyboardButton.WithCallbackData(items[i], items[i])
                        };
                        count++;
                    }
                    return buttons;
                case "subsribes":
                    for (int i = 0; i < items.Count(); i++)
                    {
                        buttons[count] = new[]
                        {
                            InlineKeyboardButton.WithCallbackData(items[i], items[i])
                        };
                        count++;
                    }
                    return buttons;
                case "remove":

                    for (int i = 0; i < items.Count(); i++)
                    {
                        buttons[count] = new[]
                        {
                            InlineKeyboardButton.WithCallbackData(items[i], items[i])
                        };
                        count++;
                    }
                    return buttons;
                default:
                    break;


            }

            return buttons;

        }





        private async Task UpdateHandler(ITelegramBotClient client, Update update, CancellationToken token)
        {



            if (update.Type != UpdateType.CallbackQuery)
            {
                mes = update.Message.Text;

                if (context.users.Where(x => x.chatId == update.Message.Chat.Id).Count() == 0)
                {
                    context.users.Add(new TgUser()
                    {
                        name = update.Message.Chat.FirstName,
                        chatId = update.Message.Chat.Id
                    });
                    context.SaveChanges();
                }
                switch (mes.ToLower())
                {
                    case "/start":

                        requist = CallBackQuearyRequist.add.ToString();
                        await client.SendTextMessageAsync(update.Message.Chat.Id, text: $"hello world {update.Message.Chat.FirstName}\n" +
                             $"выберите интересующие вас крипто валюты, они будут добавлены в ваш список отслеживаемых.\n" +
                             $"Что бы снова вызвать это меню, введите 'Крипта'\n" +
                             $"что бы просмотреть подписки введите 'подписки'\n" +
                             $"Что бы удалить определенную крипту из интересов введите 'удалить'", replyMarkup: CreateTextButtonsList(context.crypteItems.Select(n => n.name).ToList(), requist));
                        context.crypteItems.Select(n => n).ToList();
                        break;
                    case "крипта":
                        var temp = context.crypteItems.Select(n => n.name).ToList();
                        requist = CallBackQuearyRequist.add.ToString();
                        await client.SendTextMessageAsync(update.Message.Chat.Id, text: "Ваши подписки", replyMarkup: CreateTextButtonsList(temp, requist));

                        break;
                    case "подписки":
                        requist = CallBackQuearyRequist.subsribes.ToString();
                        var tempString = context.interests.Where(n => n.chatId == update.Message.Chat.Id).Select(n => n.nameOfInterest).ToList();
                        if (tempString.Count != 0)
                        {
                            await client.SendTextMessageAsync(update.Message.Chat.Id, text: "Ваши подписки", replyMarkup: CreateTextButtonsList(tempString, requist));
                            break;
                        }
                        await client.SendTextMessageAsync(update.Message.Chat.Id, text: "У вас пока нет подписок");

                        break;
                    case "удалить":
                        tempString = context.interests.Where(n => n.chatId == update.Message.Chat.Id).Select(n => n.nameOfInterest).ToList();

                        requist = CallBackQuearyRequist.remove.ToString();
                        if (tempString.Count != 0)
                        {
                            await client.SendTextMessageAsync(update.Message.Chat.Id, text: "Выберите, что желаете удалить из подписок", replyMarkup: CreateTextButtonsList(tempString, requist));
                            break;
                        }
                        await client.SendTextMessageAsync(update.Message.Chat.Id, text: "У вас пока нет подписок");



                        break;
                    default:
                        await client.SendTextMessageAsync(update.Message.Chat.Id, text: "неизвестная команда \n" +
                              "Что бы снова вызвать это меню, введите 'Крипта'\n" +
                              "что бы просмотреть подписки введите 'подписки'\n" +
                              "Что бы удалить определенную крипту из интересов введите 'удалить'");
                        break;

                }
            }
            else
            {
                if (requist == null)
                {
                    requist = "add";
                }
                switch (requist)
                {
                    case "add":
                        await client.SendTextMessageAsync(update.CallbackQuery.From.Id, text: $" add hello world");
                        var items = context.interests.Where(z => z.chatId == update.CallbackQuery.From.Id).ToList();
                        foreach (var item in items)
                        {
                            if (item.chatId == update.CallbackQuery.From.Id && item.nameOfInterest == update.CallbackQuery.Data)
                            {
                                await client.SendTextMessageAsync(update.CallbackQuery.From.Id, text: $"уже в списке подписок");
                                return;
                            }
                        }
                        context.interests.Add(new TgUsersInterests(update.CallbackQuery.From.Id, update.CallbackQuery.Data));

                        context.SaveChanges();
                        break;
                    case "remove":
                        string text = update.CallbackQuery.Data.ToString();
                        var x = context.interests.Where(z => z.chatId == update.CallbackQuery.From.Id).ToList();
                        foreach (var item in context.interests)
                        {
                            if (item.chatId == update.CallbackQuery.From.Id && item.nameOfInterest == update.CallbackQuery.Data)
                            {
                                context.interests.Remove(item);
                                break;
                            }
                        }
                        await client.SendTextMessageAsync(update.CallbackQuery.From.Id, text: $"Удалено");
                        context.SaveChanges();
                        break;

                }
            }





        }

        private async Task PollingErrorHandler(ITelegramBotClient client, Exception exception, CancellationToken token)
        {




            throw new NotImplementedException();
        }

        public async void IntervalUpdate()
        {
            StringBuilder builder = new StringBuilder();
            while (true)
            {

                CryptoFabric cryptoFabric = new CryptoFabric();
                cryptoFabric.PersentChangeFill();
                Context context = new Context();
                var persets = context.persents;
                var interest = context.interests;
                var temp = interest.Join(context.persents,
                                             x => x.nameOfInterest,
                                             y => y.name,
                                             (x, y) => new
                                             {
                                                 x.chatId,
                                                 x.nameOfInterest,
                                                 y.lastChangePersent,
                                                 y.globalChangePersent
                                             }).ToList();

                foreach (var item in temp)
                {
                    await StaticFunctions.staticClient.SendTextMessageAsync(item.chatId, text: $"Криптовалюта: {item.nameOfInterest}\n" +
                                                                                               $"Изменила своё значение с последней проверки на {item.lastChangePersent}\n" +
                                                                                               $"глобальное изменение с начала проекта составляет: {item.globalChangePersent}");
                }
                Thread.Sleep(10000);

            }
        }
    }
}
