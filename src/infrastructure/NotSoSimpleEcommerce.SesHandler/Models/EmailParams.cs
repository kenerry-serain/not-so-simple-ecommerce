namespace NotSoSimpleEcommerce.SesHandler.Models;

public record EmailParams
(
    string FromAddress,
    ICollection<string> ToAddresses,
    string Subject,
    string TemplateName,
    string TemplateData,
    ICollection<string>? CcAddresses = default,
    ICollection<string>? BccAddresses = default
);
