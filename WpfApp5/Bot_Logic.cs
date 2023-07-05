using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace WpfApp5
{
    internal class Bot_Logic
    {
        Context context = new Context();
        public Bot_Logic() 
        {
            
            var botClient = new TelegramBotClient("6379278902:AAGPVLMeVETQULyCEMADXQy8QJMxZINafgw");

            botClient.StartReceiving(updateHandler: UpdateHandler, pollingErrorHandler: PollingErrorHandler);
        }

       

        private Task UpdateHandler(ITelegramBotClient client, Update update, CancellationToken token)
        {
            var mes = update.Message.Text;
            if (context.users.Where(x => x.chatId == update.Message.Chat.Id).Count() == 0)
            {
                context.users.Add(new TgUser()
                {
                    name = update.Message.Chat.FirstName,
                    chatId = update.Message.Chat.Id
                });
                context.SaveChanges();
            }
            switch (mes)
            {
                case "/start":
                    client.SendTextMessageAsync(update.Message.Chat.Id, text: $"hello world {update.Message.Chat.FirstName}");
                    break;

            }
            return Task.CompletedTask;
        }
        private async Task PollingErrorHandler(ITelegramBotClient client, Exception exception, CancellationToken token)
        {




            throw new NotImplementedException();
        }
    }
}
