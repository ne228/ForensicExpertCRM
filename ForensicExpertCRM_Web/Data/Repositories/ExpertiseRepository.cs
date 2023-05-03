using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ForensicExpertCRM_Web.Data.Domain;
using ForensicExpertCRM_Web.Data;
using Microsoft.EntityFrameworkCore;


namespace YourApplicationName.Repositories
{
    public class ExpertiseRepository 
    {
        private readonly ApplicationDbContext _dbContext;

        public ExpertiseRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Expertise> GetExpertiseByIdAsync(int id)
        {
            return await _dbContext.Expertises.FindAsync(id);
        }

        public async Task<List<Expertise>> GetExpertisesAsync()
        {
            return await _dbContext.Expertises.ToListAsync();
        }

        public async Task<int> AddExpertiseAsync(Expertise expertise)
        {
            _dbContext.Expertises.Add(expertise);
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<int> UpdateExpertiseAsync(Expertise expertise)
        {
            _dbContext.Expertises.Update(expertise);
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<int> DeleteExpertiseAsync(Expertise expertise)
        {
            _dbContext.Expertises.Remove(expertise);
            return await _dbContext.SaveChangesAsync();
        }
    }
}
