using System;
using FluentValidation;

namespace FintechApp.dtos;

public class UserLogin
{
    public required string Email {get; set;} 
    public required string Password {get;set;}
 
}


public class UserLoginValidator: AbstractValidator<UserLogin>{
      
      public UserLoginValidator()
      {
        
        RuleFor(x=>x.Email).EmailAddress();
        RuleFor(x=> x.Password).NotEmpty();
        
      
      }
}