
using Microsoft.EntityFrameworkCore;

class TransactionRepository : ITransactionRepository<Transaction>
{
    private readonly AppDbContext _context;
    public TransactionRepository(AppDbContext context){
        _context = context;
    }
    public async Task<Guid> Create(Transaction entity)
    {
      await  _context.Transactions.AddAsync(entity);
      _context.SaveChanges();

      return entity.Id;
    }

    public void Delete(Transaction entity)
    {
        throw new NotImplementedException();
    }

    public  async Task<IEnumerable<Transaction>> GetAll()
    {
       return await  _context.Transactions.ToArrayAsync();
    }

    public Transaction? GetById(int id)
    {
        throw new NotImplementedException();
    }

    public void Update(Transaction entity)
    {
        throw new NotImplementedException();
    }
}