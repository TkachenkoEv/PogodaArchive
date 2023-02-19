using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PogodaArchive.Models;

namespace PogodaArchive.Data
{
    public class PogodaArchiveContext : DbContext
    {
        public PogodaArchiveContext (DbContextOptions<PogodaArchiveContext> options)
            : base(options)
        {
        }

        public DbSet<PogodaArchive.Models.PogodaModel> PogodaModel { get; set; } = default!;

        public DbSet<PogodaArchive.Models.YearMonthModel> YearMonthModel { get; set; }
    }
}
