using System.ComponentModel.DataAnnotations;
using System.Security;
using ServiceBusLifeboat.Cli.Extensions;

namespace ServiceBusLifeboat.Cli.PromptModels;

public sealed class CreateConnectionPromptModel
{
    internal SecureString GetSecureString() => InsecureString.ConvertToSecureString();

    [Required(ErrorMessage = "The connection-string field is required")]
    [MaxLength(500)]
    [Display(Name = "set connection-string")]
    [DataType(DataType.Password)]
    public string InsecureString { private get; init; }
}