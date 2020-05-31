using MongoDB.Bson;

namespace MusicStore.Domain.Models
{
    public class Album
    {
        public ObjectId Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public string AlbumArtUrl { get; set; }

        public virtual Genre Genre { get; set; }

        public string GenreName { get; set; }

        public virtual Artist Artist { get; set; }

        public string ArtistName { get; set; }
    }
}
