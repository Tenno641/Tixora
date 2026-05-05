using Dapper;
using Events.Application.Common;
using Events.Domain.Categories;

namespace Events.Infrastructure.Persistence.Repositories;

public class CategoryRepository: ICategoryRepository
{
    private readonly EventsDbContext _context;
    private readonly IDbConnectionFactory _dbConnectionFactory;
    
    public CategoryRepository(EventsDbContext context, IDbConnectionFactory dbConnectionFactory)
    {
        _context = context;
        _dbConnectionFactory = dbConnectionFactory;
    }

    public void Insert(Category category)
    {
        _context.Add(category);
    }
    
    public async Task<Category?> GetByIdAsync(Guid id)
    {
        await using var connection = await _dbConnectionFactory.OpenConnectionAsync();

        string sql = """
                     SELECT *
                     FROM events."Categories" as c
                     WHERE c."Id" = @Id
                     """;

        Category category = await connection.QuerySingleAsync<Category>(sql, new { Id = id });
        
        return category;
    }
    
    public void Update(Category category)
    {
        _context.Categories.Update(category);
    }
}