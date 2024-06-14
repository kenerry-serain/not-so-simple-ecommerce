using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using NotSoSimpleEcommerce.Main.Domain.Models;

namespace NotSoSimpleEcommerce.Main.Controllers
{
    [ApiController]
    //[Authorize]
    [Route("api/report")]
    public class ReportController : ControllerBase
    {

        [HttpGet("order")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> GetAllAsync
        (
            CancellationToken cancellationToken, 
            [FromServices] IConfiguration configuration
        )
        {
            var settings = MongoClientSettings.FromUrl(new MongoUrl(configuration.GetConnectionString("Mongo")));
            settings.SslSettings = new SslSettings
            {
                ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true
            };

            var client = new MongoClient(settings);

            var database = client.GetDatabase(configuration.GetValue<string>("Mongo:Database"));

            //TDO testar entidade e tb allow hostname invalid na connection string e trech acima
            var collection = database.GetCollection<ReportEntity>(configuration.GetValue<string>("Mongo:Report_Collection"));
            var report = await collection.Find(_ => true).ToListAsync(cancellationToken);

            if (!report.Any())
                return NoContent();

            return Ok(report);
        }
    }
}
