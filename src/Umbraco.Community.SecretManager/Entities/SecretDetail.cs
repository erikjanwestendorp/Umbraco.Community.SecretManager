namespace Umbraco.Community.SecretManager.Entities;

public class SecretDetail
{
    public required string Name { get; set; }
    public required string Expire { get; set; }
    public required string CreatedOn { get; set; }
    public required string RecoveryLevel { get; set; }
    public required string Tags { get; set; }
}
