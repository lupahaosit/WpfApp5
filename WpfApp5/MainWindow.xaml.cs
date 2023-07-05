using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
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

namespace WpfApp5
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ObservableCollection<string> names = new ObservableCollection<string>();
        public MainWindow()
        {
            InitializeComponent();
            HtmCryptoParse cryptoParse = new HtmCryptoParse();
            List<Crypte> cryptes = cryptoParse.Generation();
            CryptoFabric cryptoFabric = new CryptoFabric();
            cryptoFabric.CryptoCheck(cryptes);
            cryptoFabric.PersentDbFill();
            Context context = new Context();
            foreach (var item in context.crypteItems)
            {
                names.Add(item.name);
            }
            BoxWithNames.ItemsSource = names;
            Bot_Logic botLogic = new Bot_Logic();
           
        }
    }
}
