using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp5
{
    internal class CryptoFabric
    {
        List<string> crypteNames = new List<string>();
        //Принимает список из первых 10-и криптовалют рынка, проценты отсутсвуют
        public void CryptoCheck(List<Crypte> cryptes)
        {
            
            Context context = new Context();

            var dbCrypte = context.crypteItems;
            
            var cryptoHistory = context.crypteHistories;

            //Проверка на присутсвие элементов в БД

            if (context.crypteItems.Count() != 0)
            {

                foreach (var item in cryptes)
                {
                    //Добавляет крипту в БД, если таковой нет
                    if (!dbCrypte.CryptoContains(item))
                    {
                        Console.WriteLine(dbCrypte.Contains(item));
                        dbCrypte.Add(item);
                        cryptoHistory.Add(new CrypteHistory() { name = item.name, lastValue = item.value });
                        while (dbCrypte.Count() >= 10)
                        {
                            dbCrypte.Remove(dbCrypte.OrderBy(x => x.Id).Last());
                            cryptoHistory.Remove(cryptoHistory.OrderBy(x => x.Id).Last());
                        }
                    }
                    else
                    {
                        cryptoHistory.Where(n => n.name.Contains(item.name)).First().lastValue = dbCrypte.Where(n => n.name.Contains(item.name)).First().value;
                        dbCrypte.Where(n => n.name.Contains(item.name)).First().value = item.value;
                    }
                }
                context.SaveChanges();
            }
            //Заполнение пустой БД
            else
            {
                foreach (var item in cryptes)
                {
                    dbCrypte.Add(item);
                    

                }
                context.SaveChanges();
                var db = dbCrypte.Select(n => n).ToArray();
           
                for (int i = db.Length - 1; i >= 0; i--)
                {
                    cryptoHistory.Add(new CrypteHistory()
                    {
                        name = db[i].name,
                        lastValue = db[i].value,
                    });
                }
                context.SaveChanges();

            }
            context.SaveChanges();

        }

        public void PersentDbFill()
        {

            Context context1 = new Context();
            var crypte = context1.crypteItems.ToArray();
            var persents = context1.persents.ToArray();
            if (persents.Count() == 0)
            {
                foreach (var persent in crypte)
                {
                    context1.persents.Add(new Persents() { name = persent.name, globalChangePersent = 0, lastChangePersent = 0 });
                }
                context1.SaveChanges();
            }
            
            PersentChangeFill();
            context1.SaveChanges();

        }

        public void PersentChangeFill()
        {
            Context context1 = new Context();
            var crypte = context1.crypteItems.OrderBy(x => x.Id).ToArray();
            var crypteLastCheck = context1.crypteHistories.OrderBy(x => x.Id).ToArray();
            var persents = context1.persents.OrderBy(x => x.Id).ToArray();
            double statPersent = 0;
            for (int i = 0; i < 10; i++)
            {
                statPersent = 1 - (crypte.OrderBy(x => x.Id).ToArray()[i].value / crypteLastCheck.OrderBy(x => x.Id).ToArray()[i].lastValue);

                context1.persents.ToArray()[i].globalChangePersent += Math.Round(statPersent, 4);
                context1.persents.ToArray()[i].lastChangePersent = Math.Round(statPersent, 4);

            }

            context1.SaveChanges();
        }
    }
}
