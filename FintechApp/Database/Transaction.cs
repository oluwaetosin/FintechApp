public class Transaction
{
    public Guid Id {get; set;}
    public required string MaskedCardPAN {get; set;}
    public required string EncryptedCardPAN {get; set;}
    public required string ExpiryDate {get; set;}
    public required decimal Amount {get; set;}
    public required string EmailAddress {get; set;}
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
    public required string  Status {get; set;}
}