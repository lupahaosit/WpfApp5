using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Telegram.Bot;

namespace WpfApp5
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>


    public partial class MainWindow : Window
    {
        List<string> names = new List<string>();
        Dictionary<string, double> valuePairs = new Dictionary<string, double>();
        public MainWindow()
        {
            InitializeComponent();
            HtmCryptoParse cryptoParse = new HtmCryptoParse();
            List<Crypte> cryptes = cryptoParse.Generation();
            CryptoFabric cryptoFabric = new CryptoFabric();
            cryptoFabric.CryptoCheck(cryptes);
            cryptoFabric.PersentDbFill();
            Context context = new Context();
            Bot_Logic botLogic = new Bot_Logic();
            foreach (var item in context.crypteItems)
            {
                names.Add(item.name);
            }
            foreach (var item in cryptes)
            {
                valuePairs.Add(item.name, item.value);
            }
            BoxWithNames.ItemsSource = names;
            BoxWithNames.Text = names[0];

            context.Dispose();


        }

        private void BoxWithNames_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var x = Convert.ToString(valuePairs[BoxWithNames.Text]);
                text_box.Text = x;
            }
            catch (KeyNotFoundException)
            {
                text_box.Text = "Error";

            }

        }


    }
}
