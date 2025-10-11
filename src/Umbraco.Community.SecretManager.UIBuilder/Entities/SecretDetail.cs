namespace Umbraco.Community.SecretManager.UIBuilder.Entities;

public class SecretDetail
{
    public required string Name { get; set; }
    public DateTime? ExpirationDate { get; set; }
    public required string ExpirationPreview { get; set; }
    public required string CreatedOn { get; set; }
    public required string RecoveryLevel { get; set; }
    public required string Tags { get; set; }
}
