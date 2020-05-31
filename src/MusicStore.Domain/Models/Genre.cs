using MongoDB.Bson;

namespace MusicStore.Domain.Models
{
    public class Genre
    {
        public ObjectId Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}
