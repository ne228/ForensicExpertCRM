using ForensicExpertCRM_Web.Data.Domain;
using ForensicExpertCRM_Web.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using ForensicExpertCRM_Web.Models.Expert;
using ForensicExpertCRM_Web;

public class ExpertRepository
{
    private readonly ApplicationDbContext _dbContext;
    private UserManager<MyUser> _userManager;
    private RoleManager<IdentityRole> _roleManager;

    public ExpertRepository(ApplicationDbContext dbContext, RoleManager<IdentityRole> roleManager, UserManager<MyUser> userManager)
    {
        _dbContext = dbContext;
        _roleManager = roleManager;
        _userManager = userManager;
    }

    public async Task<IEnumerable<Expert>> GetAllExpertsAsync()
    {
        return await _dbContext.Experts.ToListAsync();
    }

    public async Task<Expert> GetExpertByIdAsync(int expertId)
    {
        return await _dbContext.Experts.FindAsync(expertId);
    }

    public async Task AddExpertAsync(Expert expert, string login, string pass)
    {
        // Конвертация из модели в сущность
        //var entityExpert = expert.ToEntity();

        //entityExpert.TypesExpertise = null;
        //var expertManagment = expert.ExpertManagments.FirstOrDefault(x => x.IsSelected);

        var tempTypeExpertise = expert.TypesExpertise;
        expert.TypesExpertise = null;

        var ExpertManagment = _dbContext.ExpertManagments.Include(x => x.Experts).FirstOrDefault(x => x.Id == expert.ExpertManagment.Id);

        // Добавленее эксперта в expertManagment
        ExpertManagment.Experts.Add(expert);

        var res = await _userManager.CreateUserWithoutEmailConfirm(_roleManager, login, pass, "expert", expert) as Expert;
        if (res == null) throw new Exception("Ошибка при создании пользователся");

        //res = res as Expert;
        

        var user = _dbContext.Experts
            .Include(x => x.TypesExpertise)
            .FirstOrDefault(x => x.Id == res.Id);


        user.TypesExpertise.AddRange(tempTypeExpertise);

        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateExpertAsync(Expert expert)
    {
        _dbContext.Experts.Update(expert);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteExpertAsync(int expertId)
    {
        var expertToDelete = await GetExpertByIdAsync(expertId);
        _dbContext.Experts.Remove(expertToDelete);
        await _dbContext.SaveChangesAsync();
    }
}
