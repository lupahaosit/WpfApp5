using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp5
{
    internal class HtmCryptoParse
    {
        public List<Crypte> Generation()
        {
            var url = "https://ru.investing.com/crypto/";
            var web = new HtmlWeb();
            var doc = web.Load(url);
            var cryptoName = doc.DocumentNode.Descendants("td").Where(x => x.Attributes["class"].Value == "left bold elp name cryptoName first js-currency-name").ToList();

            var cryptoTicket = doc.DocumentNode.Descendants("td").Where(x => x.Attributes["class"].Value == "left noWrap elp symb js-currency-symbol").ToList();

            var cryptoPrice = doc.DocumentNode.Descendants("td").Where(x => x.Attributes["class"].Value == "price js-currency-price").ToList();
            var z = 5.3;

            List<Crypte> cryptes = new List<Crypte>();
            for (int i = 0; i < 10; i++)
            {
                cryptes.Add(new Crypte()
                {
                    name = cryptoName[i].InnerText,
                    shortName = cryptoTicket[i].InnerText,
                    value = Convert.ToDouble(cryptoPrice[i].InnerText.Replace(".", "").Replace(',', '.').Replace('.', ','))
                });

            }
            return cryptes;
        }
    }
}
