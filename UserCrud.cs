using Microsoft.EntityFrameworkCore;
using MyFriends3._5.Data;
using MyFriends3._5.Models;
using System.Linq.Expressions;

public class UserCrud
{
    private readonly MyFriends3_5Context _context;

    public UserCrud(MyFriends3_5Context ctx)
    {
        _context = ctx;
    }

    /// <summary>
    /// Generic method to add an entity to the database
    /// returns id of item created or -1
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    public async Task<int> Add(User user)
    {
        _context.User.Add(user);
        int rowsAffected = await _context.SaveChangesAsync();
        return rowsAffected > 0 ? user.Id : -1;
    }

    // Optionally, to leverage IQueryable and defer execution with database-side filtering:
    public async Task<List<User>> FindByAsync(Expression<Func<User, bool>> predicate) =>
        await _context.User.Where(predicate).ToListAsync();

    // FindByAsync(user=>user.FirstName=="DUDU" && user.Phone=="05555")
    // FindByAsync(user=>string.IsullOrempty(user.Email))

    // public bool Foo(User user) => true;

    //public void Test()
    //{
    //    var yy = (User x) => x.FirstName == "";
    //    FindByAsync(yy);
    //    FindByAsync(Foo);
    //}

    // Generic method to update an entity in the database
    public async Task<int> Update(User model)
    {
        _context.User.Update(model);
        return await _context.SaveChangesAsync();
    }

    // Generic method to delete an entity from the database
    public async Task<int> Delete(User model)
    {
        _context.User.Remove(model);
        return await _context.SaveChangesAsync();
    }

    // Generic method to retrieve all entities of a type
    public async Task<List<User>> GetAll() =>
        await _context.User.ToListAsync() ?? new List<User>();

    public async Task<List<User>> GetPaginated(int offset, int limit) =>
        await _context.User
            .Skip(offset)
            .Take(limit)
            .ToListAsync();

    //public async Task<List<User>> GetPaginated2(int offset, int limit)
    //{
    //    return await _context.User
    //    .Skip(offset)
    //    .Take(limit)
    //    .ToListAsync();
    //}

    //public Func<string, string> greet = name =>
    //{
    //    string greeting = $"Hello {name}!";
    //    Console.WriteLine(greeting);
    //    return "dudu";
    //};

    //public Func<string, string> Greet2 = name =>
    //  "dudu";


}
