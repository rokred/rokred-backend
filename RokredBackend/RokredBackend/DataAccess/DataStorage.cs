using System;
using Microsoft.EntityFrameworkCore;
using RokredBackend.Models;

namespace RokredBackend.DataAccess
{
    public class DataStorage : DbContext
    {
        private static bool _created = false;
        public DataStorage()
        {
            if (!_created)
            {
                _created = true;
                Database.EnsureDeleted();
                Database.EnsureCreated();
            }
        }

        public DbSet<Opinion> Opinions { get; set; }

        public DbSet<Subject> Subjects { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(@"Data Source=rokred.db");
        }
    }
}