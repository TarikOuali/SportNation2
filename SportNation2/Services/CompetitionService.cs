using SportNation2.Data;
using SportNation2.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

[assembly:InternalsVisibleTo("SportNation2.Tests")]
namespace SportNation2.Services
{
    public class CompetitionService : ICompetitionService
    {
        private readonly AppDbContext _dbContext;

        public CompetitionService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> CreateCompetitionAsync(int sportId, string name, DateTime startDate)
        {
            if(!CreateCompetitionDataIsValid(sportId, name, startDate))
            {
                throw new InvalidDataException("Données invalides");
            }

            var item = new Competition()
            {
                Date = startDate,
                Name = name,
                SportId = sportId                
            };

            var entry = await _dbContext.AddAsync(item);
            await _dbContext.SaveChangesAsync();
            return entry.Entity.Id;
        }

        internal bool CreateCompetitionDataIsValid(int sportId, string name, DateTime startDate)
        {
            //valider id du sport
            if(sportId <= 0)
            {
                return false;
            }
            if(!_dbContext.Sports.Any(s => s.Id == sportId))
            {
                return false;
            }

            //valider name
            if(string.IsNullOrWhiteSpace(name))
            {
                return false;
            }
            //valioder startdate
            if(startDate < DateTime.Now.AddDays(1))
            {
                return false;
            }

            return true;

        }

        public Task CreateCompetitionEvent(int competitionId, CompetitionEventArguments args)
        {
            throw new NotImplementedException();
        }
    }
}
