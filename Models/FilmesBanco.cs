using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace APIMOVIES.Models
{
    public class FilmesBanco
    {
        [BsonId]
        public ObjectId _id { get; set; }
        public bool adult { get; set; }
        public List<int> genre_ids { get; set; }
        public int id { get; set; }
        public string original_language { get; set; }
        public string original_title { get; set; }
        public string overview { get; set;}
        public double popularity { get; set; }
        public string poster_path { get; set; }
        public string release_data { get; set; }
        public string title { get; set; }
        public bool video { get; set; }
        public double vote_average { get; set; }
        public long vote_count { get; set;}
        public Youtube keyYoutube { get; set; }
    }
    public class Youtube
    {
        public string key { get; set; }
    }
    public class FavoritaInput
    {
        public long UserId { get; set; }
        public long codFilme { get; set; }
    }
}
