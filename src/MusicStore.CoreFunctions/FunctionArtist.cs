using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using MusicStore.Domain.Models;
using System.Threading;
using System.Threading.Tasks;

namespace MusicStore.CoreFunctions
{
    public static class FunctionArtist
    {
        [FunctionName("FunctionArtistCreateBatch")]
        public static async Task<IActionResult> CreateBatch(
           [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "artist/batch")] Artist[] dataColl,
           ILogger log,
           CancellationToken cancellationToken = default)
        {
            log.LogInformation("Start of {method}", nameof(Create));

            var collection = MongoCollectionFactory.Get<Artist>(Constants.ARTIST);
            await collection.InsertManyAsync(dataColl);

            log.LogInformation("End of {method}", nameof(Create));
            return new OkObjectResult(dataColl);
        }

        [FunctionName("FunctionArtistCreate")]
        public static async Task<IActionResult> Create(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "artist")] Artist data,
            ILogger log,
            CancellationToken cancellationToken = default)
        {
            log.LogInformation("Start of {method}", nameof(Create));

            var collection = MongoCollectionFactory.Get<Artist>(Constants.ARTIST);
            await collection.InsertOneAsync(data);

            log.LogInformation("End of {method}", nameof(Create));
            return new OkObjectResult(data);
        }

        [FunctionName("FunctionArtistUpdate")]
        public static async Task<IActionResult> Update(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "artist/{artistId}")] Artist data,
            string artistId,
            ILogger log,
            CancellationToken cancellationToken = default)
        {
            log.LogInformation("Start of {method}", nameof(Update));

            data.Id = ObjectId.Parse(artistId);

            var collection = MongoCollectionFactory.Get<Artist>(Constants.ARTIST);

            await collection.ReplaceOneAsync(u => u.Id.Equals(artistId), data, cancellationToken: cancellationToken);

            log.LogInformation("End of {method}", nameof(Update));
            return new OkObjectResult(data);
        }

        [FunctionName("FunctionArtistList")]
        public static async Task<IActionResult> List(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "artist")] HttpRequest request,
            ILogger log,
            CancellationToken cancellationToken = default)
        {
            log.LogInformation("Start of {method}", nameof(List));

            var collection = MongoCollectionFactory.Get<Artist>(Constants.ARTIST);

            var result = await collection.FindAsync(FilterDefinition<Artist>.Empty, cancellationToken: cancellationToken);
            var list = await result.ToListAsync(cancellationToken);

            log.LogInformation("End of {method}", nameof(List));
            return new OkObjectResult(list);
        }
    }
}
