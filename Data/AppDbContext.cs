using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiCrud.Api.Estudantes;
using Microsoft.EntityFrameworkCore;

namespace ApiCrud.Api.Data
{
    
    public class AppDbContext : DbContext
    {
       public DbSet<Estudante> Estudantes {get; set;}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            optionsBuilder.UseSqlite("Data Source=BancoApi.sqlite");

            optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information);

            optionsBuilder.EnableSensitiveDataLogging();
            
            base.OnConfiguring(optionsBuilder);
                    
        }

    }
    
        


    
        
    
}