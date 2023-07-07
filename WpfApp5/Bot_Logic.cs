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
    
        enum CallBackQuearyRequist
        {
            add,
            remove,
            subscribes
        }
        string requist;
        public static InlineKeyboardMarkup CreateTextButtonsList()
        {
            Context context = new Context();
            var list = context.crypteItems.ToList();
            InlineKeyboardButton[][] buttons = new InlineKeyboardButton[list.Count-1][];
            int count = 0;
            for (int i = 1; i < context.crypteItems.Count() ; i ++)
            {
                buttons[count] = new[]
                {
                            InlineKeyboardButton.WithCallbackData(list[i].name, list[i].name)
                };
                count++;
            }

            return buttons;
        }

        Context context = new Context();
        public Bot_Logic() 
        {
            
            var botClient = new TelegramBotClient("6379278902:AAGPVLMeVETQULyCEMADXQy8QJMxZINafgw");

            botClient.StartReceiving(updateHandler: UpdateHandler, pollingErrorHandler: PollingErrorHandler);
        }

       

        private Task UpdateHandler(ITelegramBotClient client, Update update, CancellationToken token)
        {
           
            var mes = "s";
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

                        client.SendTextMessageAsync(update.Message.Chat.Id, text: $"hello world {update.Message.Chat.FirstName}\n" +
                            $"выберите интересующие вас крипто валюты, они будут добавлены в ваш список отслеживаемых.\n" +
                            $"Что бы снова вызвать это меню, введите 'Крипта'\n" +
                            $"что бы просмотреть подписки введите 'подписки'\n" +
                            $"Что бы удалить определенную крипту из интересов введите 'удалить'", replyMarkup: CreateTextButtonsList());
                        break;
                    case "крипта":
                        requist = CallBackQuearyRequist.add.ToString();
                        break;
                    case "подписки":
                        requist = CallBackQuearyRequist.subscribes.ToString();
                        break;
                    case "удалить":
                        requist = CallBackQuearyRequist.remove.ToString();
                        break;

                }
            }
            else
            {
                switch (requist)
                {
                    case "add":
                        requist = CallBackQuearyRequist.add.ToString();
                        break;
                    case "subscribes":
                        requist = CallBackQuearyRequist.subscribes.ToString();
                        break;
                    case "remove":
                        requist = CallBackQuearyRequist.remove.ToString();
                        break;

                }
            }
            
           
           

            return Task.CompletedTask;
        }
        public static InlineKeyboardButton[][] sendKeyboard()
        {
            InlineKeyboardButton[][] buttons = new InlineKeyboardButton[1][];


            return new InlineKeyboardButton[1][];
        }
        private async Task PollingErrorHandler(ITelegramBotClient client, Exception exception, CancellationToken token)
        {




            throw new NotImplementedException();
        }
    }
}
