using MongoDB.Bson;

namespace APIMOVIES.Models
{
    public class Users
    {
        public ObjectId _id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public List<long> playlists { get; set; }
        public long userId { get; set; }
        public List<long> friends { get; set; }
        public List<long> curtidas { get; set; }

    }
}
