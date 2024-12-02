using System;
using FluentValidation;

namespace FintechApp.dtos;

public class NewUser
{
    public required string Email {get; set;}
    public required string Firstname {get;set;}
    public required string Lastname {get; set;}
    public required string Password {get;set;}
 
}


public class NewUserRequestValidator: AbstractValidator<NewUser>{
      
      public NewUserRequestValidator()
      {
        RuleFor(x=>x.Firstname).NotEmpty().MinimumLength(3);
        RuleFor(x=>x.Lastname).NotEmpty().MinimumLength(3);
        RuleFor(x=>x.Email).EmailAddress();
        RuleFor(x=> x.Password).NotEmpty().WithMessage("Your password cannot be empty")
                    .MinimumLength(8).WithMessage("Your password length must be at least 8.")
                    .MaximumLength(16).WithMessage("Your password length must not exceed 16.")
                    .Matches(@"[A-Z]+").WithMessage("Your password must contain at least one uppercase letter.")
                    .Matches(@"[a-z]+").WithMessage("Your password must contain at least one lowercase letter.")
                    .Matches(@"[0-9]+").WithMessage("Your password must contain at least one number.")
                    .Matches(@"[\!\?\*\.]+").WithMessage("Your password must contain at least one (!? *.).");
        
      
      }
}