using System;
using System.Globalization;
using FluentValidation;

namespace FintechApp;

public record TransactionRequest(string CardPAN, string ExpiryDate, decimal Amount, string PIN, string Email);


public class TransactiobnRequestValidator: AbstractValidator<TransactionRequest>
{
    public TransactiobnRequestValidator()
    {
        RuleFor(x=>x.Amount).NotEmpty();
        RuleFor(x=>x.Email).NotEmpty().EmailAddress();
        RuleFor(x=>x.CardPAN).CreditCard();
        RuleFor(x=> x.ExpiryDate).Custom((x,context)=>{

            CultureInfo ci = new CultureInfo(CultureInfo.CurrentCulture.LCID);
            ci.Calendar.TwoDigitYearMax = 2099; 
            DateTime dt;
            if(!DateTime.TryParseExact(x, "MM/yy", ci, DateTimeStyles.None, out dt))
            {
                context.AddFailure("ExpiryDate invalid");
            }
        });
         RuleFor(x=> x.PIN).Custom((x,context)=>{

            int pin = -1;

            if(!int.TryParse(x, out pin)){
                context.AddFailure("PIN is invalid");
            }

            if(pin.ToString().Length != 4){
                 context.AddFailure("PIN must be 4 digit");
            }
        });
        RuleFor(x=>x.Amount).Custom((x, context)=>
        {
            decimal value = -1;
            if(!decimal.TryParse(x.ToString(),  out value)){
                context.AddFailure("Amount is not a valid amount");
            }
        });
        
    }
}