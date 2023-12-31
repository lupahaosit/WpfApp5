﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace WpfApp5
{
    internal static class StaticFunctions
    {
        public static ITelegramBotClient staticClient;
        public static bool CryptoContains(this DbSet<Crypte> cryptes, Crypte crypte)
        {
            if (cryptes == null)
            foreach (var item in cryptes)
            {
                if (item.name.Contains(crypte.name) && item.shortName.Contains(crypte.shortName))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
