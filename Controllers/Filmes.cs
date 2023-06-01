using APIMOVIES.Models;
using APIMOVIES.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace APIMOVIES.Controllers
{
    [ApiController]
    [Route("Filmes")]
    public class Filmes : Controller
    {
        [HttpGet]
        [Route("get-all-filmes")]
        [SwaggerOperation(
        Summary = "Pega todos os filmes da nossa base",
        Description = @"Ao pedir um filme pela requisição, recupera-se todos os filmes da base.")]
        [SwaggerResponse(200, "Sucesso ao criar ao cadastrar o filme.")]
        [SwaggerResponse(500, "Erro na API")]
        public async Task<List<FilmesBanco>> getAllFilmes()
        {
            MongoDBService client = new MongoDBService();
            List<FilmesBanco> filmes = await client.GetAllFilmes();

            return filmes;
        }
        [HttpGet]
        [Route("get-filme")]
        [SwaggerOperation(
        Summary = "Pega todos os filmes da nossa base",
        Description = @"Ao pedir um filme pela requisição, recupera-se todos os filmes da base.")]
        [SwaggerResponse(200, "Sucesso ao criar ao cadastrar o filme.")]
        [SwaggerResponse(500, "Erro na API")]
        public async Task<FilmesBanco> getFilme([FromQuery] long codFilme)
        {
            MongoDBService client = new MongoDBService();
            FilmesBanco filmes = await client.GetFilme(codFilme);

            return filmes;
        }
        [HttpPost]
        [Route("get-filme-lista")]
        [SwaggerOperation(
        Summary = "Pega todos os filmes da nossa base",
        Description = @"Ao pedir um filme pela requisição, recupera-se todos os filmes da base.")]
        [SwaggerResponse(200, "Sucesso ao criar ao cadastrar o filme.")]
        [SwaggerResponse(500, "Erro na API")]
        public async Task<List<FilmesBanco>> getListaFilmes([FromBody] string lista)
        {
            MongoDBService client = new MongoDBService();
            string[] listaFilmesFormatada = lista.Split(";");
            FilmesBanco filme = new FilmesBanco();
            List<FilmesBanco> ListaFilmes = new List<FilmesBanco>();
            foreach(string codFilme in listaFilmesFormatada)
            {
                filme = await client.GetFilme(int.Parse(codFilme));
                ListaFilmes.Add(filme);
            }

            return ListaFilmes;
        }
        [HttpPatch]
        [Route("att-all-filme")]
        [SwaggerOperation(
        Summary = "Pega todos os filmes da nossa base",
        Description = @"Ao pedir um filme pela requisição, recupera-se todos os filmes da base.")]
        [SwaggerResponse(200, "Sucesso ao criar ao cadastrar o filme.")]
        [SwaggerResponse(500, "Erro na API")]
        public async Task<int> attAllFilmes()
        {
            MongoDBService client = new MongoDBService();
            int filmes = await client.AtualizaStatus();

            return filmes;
        }
        [HttpPost]
        [Route("avaliar-filme")]
        [SwaggerOperation(
        Summary = "Pega todos os filmes da nossa base",
        Description = @"Ao pedir um filme pela requisição, recupera-se todos os filmes da base.")]
        [SwaggerResponse(200, "Sucesso ao criar ao cadastrar o filme.")]
        [SwaggerResponse(500, "Erro na API")]
        public async void AvaliarFilme([FromQuery] double nota, long codFilme)
        {
            MongoDBService client = new MongoDBService();
            FilmesBanco filme = await client.GetFilme(codFilme);
            var quantVotos = filme.vote_count;
            var mediaVotos = filme.vote_average;
            var notaFinalVotos = (double)(((double)mediaVotos * quantVotos)+ (double)nota)/(quantVotos+1);
            await client.avaliarFilme(notaFinalVotos, codFilme, quantVotos + 1);
        }
    }
}
