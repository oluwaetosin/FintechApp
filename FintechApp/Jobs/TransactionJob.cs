using Coravel.Invocable;
using Microsoft.EntityFrameworkCore;

class TransactionJob: IInvocable
{
    private readonly AppDbContext _context;
    public TransactionJob(AppDbContext context)
    {
        _context = context;
    }
    public async Task Invoke()
    {
        Console.WriteLine("invoking job");
       var data =  await _context.Transactions.ToListAsync();

       foreach (var item in data)
       {
            await _context.TransactionArchives.AddAsync(new TransactionArchive{
                    Amount = item.Amount,
                    EmailAddress = item.EmailAddress,
                    EncryptedCardPAN = item.EncryptedCardPAN,
                    ExpiryDate = item.ExpiryDate,
                    MaskedCardPAN = item.MaskedCardPAN,
                    Status = item.Status,
                    ArchivedAt = DateTime.UtcNow,
                    CreatedDate = item.CreatedDate,
                    UpdatedDate = item.UpdatedDate,
                    Id = item.Id
            });


       }

       _context.Transactions.RemoveRange(data);

       await _context.SaveChangesAsync();
    }
}