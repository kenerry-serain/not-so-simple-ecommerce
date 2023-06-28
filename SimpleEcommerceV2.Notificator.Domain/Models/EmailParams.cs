using SimpleEcommerceV2.Notificator.Domain.Enums;

namespace SimpleEcommerceV2.Notificator.Domain.Models;

public class EmailParams
{
    public EmailParams
    (
        string fromAddress, 
        ICollection<string> toAddresses, 
        ICollection<string> ccAddresses,
        ICollection<string> bccAddresses,
        string subject, 
        string templateName
    )
    {
        FromAddress = fromAddress;
        ToAddresses = toAddresses;
        CcAddresses = ccAddresses;
        BccAddresses = bccAddresses;
        Subject = subject;
        TemplateName = templateName;
    }

    public string FromAddress { get; set; }
    public string TemplateName { get; set; }
    public ICollection<string> ToAddresses { get; set; }
    public ICollection<string> CcAddresses { get; set; }
    public ICollection<string> BccAddresses { get; set; }
    public string Subject { get; set; }
    public string BodyContent { get; set; }
    public BodyContentType BodyContentType { get; set; }
}
