using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TFourMusic.Models;

namespace TFourMusic.Data
{
    public class MusicContext: DbContext
    {
        public MusicContext(DbContextOptions<MusicContext> options):base (options)
        {

        }
        public DbSet<quangcaoModel> quangcaoModels { get; set; }
        public DbSet<baihatModel> baihatModels { get; set; }
    }
}
