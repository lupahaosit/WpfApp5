using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp5
{
    internal class Persents : CrypteBase
    {
        public int Id { get; set; }
        public double globalChangePersent { get; set; }

        public double lastChangePersent { get; set; }
    }
}
