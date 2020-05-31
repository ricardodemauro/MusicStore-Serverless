using MongoDB.Bson;

namespace MusicStore.Domain.Models
{
    public class Artist
    {
        public ObjectId Id { get; set; }

        public string Name { get; set; }
    }
}
