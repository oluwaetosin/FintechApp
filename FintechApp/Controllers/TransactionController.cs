using FintechApp.Extensions;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FintechApp.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class TransactionController : Controller
    {
        private ILogger<TransactionController> _logger;
        private readonly IConfiguration _configuration;
        private readonly ITransactionRepository<Transaction> _transactionRepo;
        

        public TransactionController(ILogger<TransactionController> logger, 
        IConfiguration configuration, 
        ITransactionRepository<Transaction> transaction)
        {
            _logger = logger;
            _configuration = configuration;
            _transactionRepo = transaction;
            
        }
        /// <summary>
        /// Process Transaction
        /// </summary>
        /// <param name="request">Request to Process</param>
        /// <returns>success message</returns>
        [HttpPost("process")]
        [ProducesResponseType(typeof(string), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails),StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ProcessTransaction([FromBody] TransactionRequest request, 
        IValidator<TransactionRequest> validator)
        {
             

            string encryptionKey = _configuration.GetValue<string>("EncryptionKey")!;

            string iv  = _configuration.GetValue<string>("EncryptionIV")!;
             
            
            var validationResults = await validator.ValidateAsync(request);

            if (!validationResults.IsValid)
            {

                return ValidationProblem(validationResults.ToModelStateDictionary());

            }

            var transaction = new Transaction {
                Amount = request.Amount,
                MaskedCardPAN =  $"************{request.CardPAN[^4..]}", 
                ExpiryDate = AESEncryptionService.Encrypt(request.ExpiryDate, 
                Convert.FromBase64String(encryptionKey),
                Convert.FromBase64String(iv)
                ),
                EncryptedCardPAN = AESEncryptionService.Encrypt(request.CardPAN, 
                Convert.FromBase64String(encryptionKey),
                Convert.FromBase64String(iv)
                ),
                Status = "Successful",
                EmailAddress = request.Email
                
                
            };
            await _transactionRepo.Create(transaction);
          ;

          var info = new { Amount = request.Amount,
                MaskedCardPAN =  transaction.MaskedCardPAN, 
                ExpiryDate = transaction.ExpiryDate,
                EncryptedCardPAN = transaction.EncryptedCardPAN,
                Status = "Successful",
                Email = transaction.EmailAddress,
                id = transaction.Id,
                Date = transaction.CreatedDate
          };

            
             
            _logger.LogInformation(info.ToString());

            return Ok(transaction.Id);
        }
    
    
    }
}
