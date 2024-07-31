using MyFriends3._5.Data;

public class GenericCrud
{
    private readonly MyFriends3_5Context _context;

    public GenericCrud(MyFriends3_5Context ctx)
    {
        _context = ctx;
    }

    // Generic method to add an entity to the database
    public async Task<int> Add<TModel>(TModel model) where TModel : class
    {
        _context.Set<TModel>().Add(model);
        return await _context.SaveChangesAsync();
    }

    // Generic method to retrieve an entity by primary key
    public async Task<TModel?> Get<TModel>(params object[] keyValues) where TModel : class
    {
        return await _context.Set<TModel>().FindAsync(keyValues);
    }

    // Generic method to update an entity in the database
    public async Task<int> Update<TModel>(TModel model) where TModel : class
    {
        _context.Set<TModel>().Update(model);
        return await _context.SaveChangesAsync();
    }

    // Generic method to delete an entity from the database
    public async Task<int> Delete<TModel>(TModel model) where TModel : class
    {
        _context.Set<TModel>().Remove(model);
        return await _context.SaveChangesAsync();
    }

    // Generic method to retrieve all entities of a type
    public async Task<IQueryable<TModel>> GetAll<TModel>() where TModel : class
    {
        return _context.Set<TModel>();
    }
}
