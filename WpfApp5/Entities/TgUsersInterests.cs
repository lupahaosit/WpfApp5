﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp5
{
    internal class TgUsersInterests
    {
        public int Id { get; set; }

        public long chatId { get; set; }
        public string nameOfInterest { get; set; }

        public TgUsersInterests(long chatId, string nameOfInterest)
        {
            this.chatId = chatId;
            this.nameOfInterest = nameOfInterest;
        }
    }
}
