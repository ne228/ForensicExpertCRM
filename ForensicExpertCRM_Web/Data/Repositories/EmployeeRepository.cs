using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ForensicExpertCRM_Web;
using ForensicExpertCRM_Web.Data;
using ForensicExpertCRM_Web.Data.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

public class EmployeeRepository
{
    private readonly ApplicationDbContext _context;
    private UserManager<MyUser> _userManager;
    private RoleManager<IdentityRole> _roleManager;

    public EmployeeRepository(ApplicationDbContext context, RoleManager<IdentityRole> roleManager, UserManager<MyUser> userManager)
    {
        _context = context;
        _roleManager = roleManager;
        _userManager = userManager;
    }

    // Создание сотрудника
    public async Task<Employee> CreateAsync(Employee employee, string login, string pass)
    {        
        if (employee.EmployeeManagment!=null)
            employee.EmployeeManagmentId = employee.EmployeeManagment.Id;

        var employeeManagment = _context.EmployeeManagments.Include(x => x.Employees).FirstOrDefault(x => x.Id == employee.EmployeeManagmentId);

        employeeManagment.Employees.Add(employee);

        //_context.ExpertManagments.FirstOrDefault();
        var user = await _userManager.CreateUserWithoutEmailConfirm(_roleManager, login, pass, "employee", employee as Employee);

        if (user == null) new Exception("Ошибка при создании пользователся");

        await _context.SaveChangesAsync();

        return employee;
    }

    // Получение списка всех сотрудников
    public async Task<List<Employee>> GetAllAsync()
    {
        return await _context.Employees.ToListAsync();
    }

    // Получение сотрудника по идентификатору
    public async Task<Employee> GetByIdAsync(int id)
    {
        return await _context.Employees.FindAsync(id);
    }

    // Обновление сотрудника
    public async Task<Employee> UpdateAsync(Employee employee)
    {
        _context.Entry(employee).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return employee;
    }

    // Удаление сотрудника
    public async Task DeleteAsync(int id)
    {
        var employee = await _context.Employees.FindAsync(id);
        _context.Employees.Remove(employee);
        await _context.SaveChangesAsync();
    }
}
