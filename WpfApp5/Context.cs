using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp5
{
    internal class Context : DbContext
    {
        public DbSet<CrypteHistory> crypteHistories { get; set; }
        public DbSet<Crypte> crypteItems { get; set; }
        public DbSet<Persents> persents { get; set; }

        public DbSet<TgUser> users { get; set; }

        public DbSet<TgUsersInterests> interests { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseNpgsql("Server=localhost;Port=5432;Database=Crypto;Username=postgres;Password=IGOR2002vlad");
        }
    }
}
