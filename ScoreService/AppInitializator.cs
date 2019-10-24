using System;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.EntityFrameworkCore;
using ScoreService.Infrastructure;

namespace ScoreService
{

    public interface IAppInitializator
    {
        Task PrepareAsync();
    }

    public class AppInitializator : IAppInitializator
    {
        private readonly SSDbContext _context;

        public AppInitializator(SSDbContext context)
        {
            _context = context;
        }

        public async Task PrepareAsync()
        {
            var masterDbContext = _context;
            var isMasterDbInMemory = masterDbContext.Database.IsInMemory();
            
            if (isMasterDbInMemory)
            {
               Console.WriteLine("Используется база данных в памяти приложения (InMemory)");
                var dbCreated = await masterDbContext.Database.EnsureCreatedAsync();
                if (!dbCreated)
                {
                    Console.WriteLine("Не удалось создать и настроить базу данных в памяти");
                }
            }
            await masterDbContext.Database.MigrateAsync();
            Console.WriteLine("Миграции сделаль");
            Console.WriteLine();
          

        }
    }
}