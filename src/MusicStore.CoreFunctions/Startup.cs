using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using MongoDB.Bson.Serialization;
using MusicStore.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


[assembly: FunctionsStartup(typeof(MusicStore.CoreFunctions.Startup))]

namespace MusicStore.CoreFunctions
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            BsonClassMap.RegisterClassMap<Album>(cm =>
            {
                cm.AutoMap();
                cm.MapProperty(x => x.AlbumArtUrl).SetIgnoreIfDefault(true);

                cm.UnmapProperty(x => x.ArtistName);
                cm.UnmapProperty(x => x.GenreName);

                cm.SetIgnoreExtraElements(true);
            });

            BsonClassMap.RegisterClassMap<Genre>(cm =>
            {
                cm.AutoMap();
                cm.MapProperty(x => x.Description).SetIgnoreIfDefault(true);

                cm.SetIgnoreExtraElements(true);
            });

            BsonClassMap.RegisterClassMap<Artist>(cm =>
            {
                cm.AutoMap();
                cm.SetIgnoreExtraElements(true);
            });
        }
    }
}
