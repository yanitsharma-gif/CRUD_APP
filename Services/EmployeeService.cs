using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Practice_3._0;
using Practice_3._0.Models;

public class EmployeeService
{
    private readonly IMongoCollection<Employee> _employees;

    public EmployeeService(
        IOptions<MongoDbSettings> settings)
    {
        var mongoClient =
            new MongoClient(
                settings.Value.ConnectionString);

        var database =
            mongoClient.GetDatabase(
                settings.Value.DatabaseName);

        _employees =
            database.GetCollection<Employee>(
                settings.Value.EmployeesCollectionName);
    }

    public async Task<List<Employee>> GetAllAsync()
    {
        return await _employees
            .Find(_ => true)
            .ToListAsync();
    }

    public async Task<Employee?> GetByIdAsync(string id)
    {
        return await _employees
            .Find(x => x.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task CreateAsync(Employee employee)
    {
        await _employees.InsertOneAsync(employee);
    }

    public async Task UpdateAsync(
        string id,
        Employee employee)
    {
        await _employees.ReplaceOneAsync(
            x => x.Id == id,
            employee);
    }

    public async Task DeleteAsync(string id)
    {
        await _employees.DeleteOneAsync(
            x => x.Id == id);
    }
}