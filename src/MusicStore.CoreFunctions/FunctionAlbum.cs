using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using MusicStore.Domain.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MusicStore.CoreFunctions
{
    public class FunctionAlbum
    {
        static async Task<Genre> GetGenreByName(string genreName, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(genreName))
                throw new ArgumentNullException(nameof(genreName));

            var collection = MongoCollectionFactory.Get<Genre>(Constants.GENRE);

            FindOptions<Genre, Genre> opts = new FindOptions<Genre, Genre>()
            {
                AllowPartialResults = false,
                BatchSize = 1,
            };

            var genre = await collection.FindAsync(x => x.Name == genreName, opts, cancellationToken: cancellationToken);

            return await genre.FirstOrDefaultAsync(cancellationToken);
        }

        static async Task<Artist> GetArtistByName(string artistName, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(artistName))
                throw new ArgumentNullException(nameof(artistName));

            var collection = MongoCollectionFactory.Get<Artist>(Constants.ARTIST);

            FindOptions<Artist, Artist> opts = new FindOptions<Artist, Artist>()
            {
                AllowPartialResults = false,
                BatchSize = 1,
            };

            var album = await collection.FindAsync(x => x.Name == artistName, opts, cancellationToken: cancellationToken);

            return await album.FirstOrDefaultAsync(cancellationToken);
        }

        [FunctionName("FunctionAlbumCreateBatch")]
        public static async Task<IActionResult> CreateBatch(
           [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "album/batch")] Album[] dataColl,
           ILogger log,
           CancellationToken cancellationToken = default)
        {
            log.LogInformation("Start of {method}", nameof(Create));

            foreach (var item in dataColl)
            {
                await Create(item, log, cancellationToken);
            }

            log.LogInformation("End of {method}", nameof(Create));
            return new OkObjectResult(dataColl);
        }

        [FunctionName("FunctionAlbumCreate")]
        public static async Task<IActionResult> Create(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "album")] Album data,
            ILogger log,
            CancellationToken cancellationToken = default)
        {
            log.LogInformation("Start of {method}", nameof(Create));

            var artist = string.IsNullOrEmpty(data.Artist?.Name) && string.IsNullOrEmpty(data.ArtistName)
                ? default
                : await GetArtistByName(data.Artist?.Name ?? data.ArtistName);

            var genre = string.IsNullOrEmpty(data.Genre?.Name) && string.IsNullOrEmpty(data.GenreName)
                ? default
                : await GetGenreByName(data.Genre?.Name ?? data.GenreName);

            data.Artist = artist;
            data.Genre = genre;

            var collection = MongoCollectionFactory.Get<Album>(Constants.ALBUM);
            await collection.InsertOneAsync(data);

            log.LogInformation("End of {method}", nameof(Create));
            return new OkObjectResult(data);
        }

        //[FunctionName("FunctionAlbumUpdate")]
        //public static async Task<IActionResult> Update(
        //    [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "album/{albumId}")] Album data,
        //    string albumId,
        //    ILogger log,
        //    CancellationToken cancellationToken = default)
        //{
        //    log.LogInformation("Start of {method}", nameof(Update));

        //    data.Id = ObjectId.Parse(albumId);

        //    var collection = MongoCollectionFactory.Get<Album>(Constants.ARTIST);

        //    await collection.ReplaceOneAsync(u => u.Id.Equals(albumId), data, cancellationToken: cancellationToken);

        //    log.LogInformation("End of {method}", nameof(Update));
        //    return new OkObjectResult(data);
        //}

        [FunctionName("FunctionAlbumList")]
        public static async Task<IActionResult> List(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "album")] HttpRequest request,
            ILogger log,
            CancellationToken cancellationToken = default)
        {
            log.LogInformation("Start of {method}", nameof(List));

            var collection = MongoCollectionFactory.Get<Album>(Constants.ALBUM);

            var result = await collection.FindAsync(FilterDefinition<Album>.Empty, cancellationToken: cancellationToken);
            var list = await result.ToListAsync(cancellationToken);

            log.LogInformation("End of {method}", nameof(List));
            return new OkObjectResult(list);
        }
    }
}
