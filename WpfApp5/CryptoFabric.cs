using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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
                    var x = dbCrypte.First().name;
                    var z = cryptes.First().name;
                    var persents = context.persents;
                    var y = x == z;
                    //Добавляет крипту в БД, если таковой нет
                    if (dbCrypte.Where(n => n.name.Equals(item.name)).Count() == 0)
                    {

                        Console.WriteLine(dbCrypte.Contains(item));
                        dbCrypte.Add(item);
                        cryptoHistory.Add(new CrypteHistory() { name = item.name, lastValue = item.value });
                       
                    
                        
                    }
                    else
                    {
                        cryptoHistory.Where(n => n.name.Contains(item.name)).First().lastValue = dbCrypte.Where(n => n.name.Contains(item.name)).First().value;
                        dbCrypte.Where(n => n.name.Contains(item.name)).First().value = item.value;
                    }
                    if (persents.Where(n => n.name == item.name).Count() == 0)
                    {
                        context.persents.Add(new Persents() { globalChangePersent = 0, lastChangePersent = 0, name = item.name });
                    }
                    else
                    {
                        cryptoHistory.Where(n => n.name.Contains(item.name)).First().lastValue = dbCrypte.Where(n => n.name.Contains(item.name)).First().value;
                        dbCrypte.Where(n => n.name.Contains(item.name)).First().value = item.value;
                    }
                }
               
               
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
        //функция записи данных о процентных изменениях курсов криптовалют
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
