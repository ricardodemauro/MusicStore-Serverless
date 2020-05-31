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
    public static class FunctionGenre
    {
        [FunctionName("FunctionGenreCreate")]
        public static async Task<IActionResult> Create(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "genre")] Genre data,
            ILogger log,
            CancellationToken cancellationToken = default)
        {
            log.LogInformation("Start of {method}", nameof(Create));

            var collection = MongoCollectionFactory.Get<Genre>(Constants.GENRE);
            await collection.InsertOneAsync(data);

            log.LogInformation("End of {method}", nameof(Create));
            return new OkObjectResult(data);
        }

        [FunctionName("FunctionGenreUpdate")]
        public static async Task<IActionResult> Update(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "genre/{genreId}")] Genre data,
            string genreId,
            ILogger log,
            CancellationToken cancellationToken = default)
        {
            log.LogInformation("Start of {method}", nameof(Update));

            data.Id = ObjectId.Parse(genreId);

            var collection = MongoCollectionFactory.Get<Genre>(Constants.GENRE);

            await collection.ReplaceOneAsync(u => u.Id.Equals(genreId), data, cancellationToken: cancellationToken);

            log.LogInformation("End of {method}", nameof(Update));
            return new OkObjectResult(data);
        }

        [FunctionName("FunctionGenreList")]
        public static async Task<IActionResult> List(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "genre")] HttpRequest request,
            ILogger log,
            CancellationToken cancellationToken = default)
        {
            log.LogInformation("Start of {method}", nameof(List));

            var collection = MongoCollectionFactory.Get<Genre>(Constants.GENRE);

            var result = await collection.FindAsync(FilterDefinition<Genre>.Empty, cancellationToken: cancellationToken);
            var list = await result.ToListAsync(cancellationToken);

            log.LogInformation("End of {method}", nameof(List));
            return new OkObjectResult(list);
        }
    }
}
