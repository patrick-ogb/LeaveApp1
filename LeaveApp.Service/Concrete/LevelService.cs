using LeaveApp.Core.Entities;
using LeaveApp.Service.Abstract;
using LeaveApp.Service.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveApp.Service.Concrete
{
    public class LevelService : ILevelService
    {
        private readonly AppDbContext context;

        public LevelService(AppDbContext context)
        {
            this.context = context;
        }
        public async Task<Level> AddLevel(Level level)
        {
            context.Levels.Add(level);
            await context.SaveChangesAsync();
            return level;
        }

        public async Task<Level> DeleteLevel(int Id)
        {
            var level = context.Levels.FirstOrDefault(emp => emp.Id == Id);
            context.Levels.Remove(level);
            await context.SaveChangesAsync();
            return level;
        }

        public async Task<List<Level>> GetLevels()
        {
            return await context.Levels.ToListAsync();
        }

        public async Task<Level> GetLevel(int Id)
        {
            return await context.Levels.FirstOrDefaultAsync(lvl => lvl.Id == Id);

        }

        public async Task<Level> UpdateLevel(Level levelChange)
        {
            context.Levels.Update(levelChange);
            await context.SaveChangesAsync();
            return levelChange;
        }
    }
}
