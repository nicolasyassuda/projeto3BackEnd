using APIMOVIES.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Security.Cryptography;

namespace APIMOVIES.Services
{
    public class MongoDBService
    {
        private readonly IMongoCollection<FilmesBanco> _Filmes;

        public MongoDBService()
        {
            MongoClient mongoClient = new MongoClient(Environment.GetEnvironmentVariable("MONGODB_URI"));

            _Filmes = mongoClient.GetDatabase(Environment.GetEnvironmentVariable("MONGODB_DBNAME")).GetCollection<FilmesBanco>("filmes");
        }

        public async Task<ObjectId?> PostRequisicaoAsync(FilmesBanco filme)
        {
            var existe = await _Filmes.Find(x => x.id == filme.id).FirstOrDefaultAsync();
            if(existe == null)
            {
                await _Filmes.InsertOneAsync(filme);
                return filme._id;
            }
            
            return null;
        }

        public async Task<FilmesBanco?> GetRequisicaoAsync(ObjectId id) =>
            await _Filmes.Find(x => x._id == id).FirstOrDefaultAsync();

        public async Task<List<FilmesBanco>?> GetAllFilmes() =>
            await _Filmes.Find(_ => true).ToListAsync();
    }
}
