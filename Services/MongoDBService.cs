using APIMOVIES.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Security.Cryptography;
using static MongoDB.Driver.WriteConcern;

namespace APIMOVIES.Services
{
    public class MongoDBService
    {
        private readonly IMongoCollection<FilmesBanco> _Filmes;
        private readonly IMongoCollection<Users> _Users;

        public MongoDBService()
        {
            MongoClient mongoClient = new MongoClient(Environment.GetEnvironmentVariable("MONGODB_URI"));

            _Filmes = mongoClient.GetDatabase(Environment.GetEnvironmentVariable("MONGODB_DBNAME")).GetCollection<FilmesBanco>("filmes");
            _Users = mongoClient.GetDatabase(Environment.GetEnvironmentVariable("MONGODB_DBNAME")).GetCollection<Users>("profiles");
        }

        public async Task<ObjectId?> PostRequisicaoAsync(FilmesBanco filme)
        {
            var existe = await _Filmes.Find(x => x.id == filme.id).FirstOrDefaultAsync();
            if (existe == null)
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
        public async Task<FilmesBanco> GetFilme(long codFilme) =>
            await _Filmes.Find(x => x.id == codFilme).FirstOrDefaultAsync();

        public async Task<int> AtualizaStatus()
        {

            var filter = Builders<FilmesBanco>.Filter.Ne(filme=> filme.vote_count,1);
            var update = Builders<FilmesBanco>.Update.Set(filme => filme.vote_average, 0);
            await _Filmes.UpdateManyAsync(filter, update);
            return 1;
        }
        public async Task<Boolean> avaliarFilme(double nota,long codFilme,long quantNotas)
        {
            var filter = Builders<FilmesBanco>.Filter.Eq(filme => filme.id,codFilme);
            var update = Builders<FilmesBanco>.Update.Set(filme => filme.vote_average, nota);
            var update2 = Builders<FilmesBanco>.Update.Set(filme => filme.vote_count, quantNotas);
            await _Filmes.UpdateManyAsync(filter, update);
            await _Filmes.UpdateManyAsync(filter, update2);

            return true;
        }

        public async Task<long?> logar(string user, string password)
        {
            var usuario = await _Users.Find(x => x.email == user).FirstOrDefaultAsync();

            if(usuario.password == password){
                return usuario.userId;
            }
            return null;
        }
        public async Task<long?> registrar(string user, string password,string username)
        {
            var usuario = await _Users.Find(x => x.email == user).FirstOrDefaultAsync();

            if (usuario == null)
            {
                var ultimoUsuario = await _Users.Find(_ => true).ToListAsync();
                var idAtual = ultimoUsuario[ultimoUsuario.Count - 1].userId + 1;
                Users usuarioCriado = new Users();
                usuarioCriado.userId = idAtual;
                usuarioCriado.password = password;
                usuarioCriado.email = user;
                usuarioCriado.name = username;
                await _Users.InsertOneAsync(usuarioCriado);
                return idAtual;

            }

            return null;
        }
        public async Task<int> FavoritarFilme(long userId, long codFilme)
        {
            var userBuscado = await _Users.Find(user => user.userId == userId).FirstOrDefaultAsync();
            var playlistAtual = userBuscado.playlists;
            playlistAtual.Add(codFilme);
            var filter = Builders<Users>.Filter.Eq(user => user.userId, userId);
            var update = Builders<Users>.Update.Set(user => user.playlists, playlistAtual);
            await _Users.UpdateOneAsync(filter, update);

            return 1;
        }
        public async Task<Users> GetProfile(string idProfile)
        {
            return await _Users.Find(x => x.userId.ToString() == idProfile).FirstOrDefaultAsync();
        }
    }
}
