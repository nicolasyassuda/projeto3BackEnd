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
        [HttpPost]
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
    }
}
