using System;
using Microsoft.EntityFrameworkCore;

namespace FintechApp.Repository;

class UserRepository: IUserRepository<User>
{
    private readonly AppDbContext _context;
    public UserRepository(AppDbContext context){
        _context = context;
    }
    public async Task<Guid> Create(User entity)
    {
      await  _context.Users.AddAsync(entity);
      _context.SaveChanges();

      return entity.Id;
    }
    public async Task<User?> UserExist(string email)
    {
      var user = await  _context.Users.Where(x=>x.Email == email).FirstOrDefaultAsync();

      return user;
       
    }
    public void Delete(User entity)
    {
        throw new NotImplementedException();
    }

    public  async Task<IEnumerable<User>> GetAll()
    {
       return await  _context.Users.ToListAsync();
    }

    public User? GetById(int id)
    {
        throw new NotImplementedException();
    }

    public void Update(User entity)
    {
        throw new NotImplementedException();
    }
}
 