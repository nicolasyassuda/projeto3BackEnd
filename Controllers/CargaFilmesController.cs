using APIMOVIES.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using MongoDB.Bson;
using Newtonsoft.Json.Linq;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Http.Headers;
using System.Text.Json;
using APIMOVIES.Services;
using static System.Net.WebRequestMethods;
using Microsoft.AspNetCore.Http.HttpResults;

namespace APIMOVIES.Controllers
{
    [ApiController]
    [Route("CargaFilmes")]
    public class CargaFilmesController : Controller
    {
        

        [HttpPost]
        [Route("requisicao")]
        [SwaggerOperation(
        Summary = "Cadastra o filme na nossa Base de dados",
        Description = @"Ao pedir um filme pela requisição verifica-se se o filme existe na base para cadastrar.")]
        [SwaggerResponse(200, "Sucesso ao criar ao cadastrar o filme.")]
        [SwaggerResponse(500, "Erro na API")]
        public async Task<ObjectResult> SalvarFilmes([FromBody] string nomeFilme)
        {

            int resultado = await VerificarFilmes(nomeFilme);
            if (resultado == 200)
            {
                return Ok("Filmes foram Adicionados");
            }
            else if (resultado==404)
            {
                return NotFound("Filme requisitado não foi encontrado em nossa base.");
            }
            else if (resultado == 403)
            {
                return BadRequest("Porfavor seja mais especifico no titulo, muitos titulos com a pesquisa foram encontrados.");
            }
            else
            {
                return BadRequest("Algo na requisição efetuada deu erro");
            }
            
        }

        async Task<int> VerificarFilmes(string nomeFilme)
        {
            string Token = Environment.GetEnvironmentVariable("TOKEN");
            if (Token == null)
            {
                Token = "eyJhbGciOiJIUzI1NiJ9.eyJhdWQiOiI3OGFiMGY2MjgzMTZhNTU3MmY2NmIyNWMzZjY4NGQwNCIsInN1YiI6IjY0NWIxMTk0MWI3MGFlMDE0NWVlYjFmNyIsInNjb3BlcyI6WyJhcGlfcmVhZCJdLCJ2ZXJzaW9uIjoxfQ.qAEZ8u8C08cAedTLjTwW0Akh7EpkRcxlHZ89tjolgq0";
            }
            var query = new Dictionary<string, string>()
            {
                ["query"] = nomeFilme,
                ["include_adult"] = "false",
                ["language"] = "pt-BR",
                ["page"] = "1"
            };
            var query2 = new Dictionary<string, string>()
            {

                ["language"] = "pt-BR"

            };
            try { 
                var url = QueryHelpers.AddQueryString("https://api.themoviedb.org/3/search/movie", query);
                var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
                var requisicao = await client.GetAsync(url);
                var json = await requisicao.Content.ReadAsStringAsync();
                var result = JObject.Parse(json).SelectToken("results").ToList();
                List<JToken> result2 = new();
                string StringJson;
                string urlTrailer;
                MongoDBService banco = new MongoDBService();
                FilmesBanco filme;
                Youtube keyYoutube;
                if (result.Count > 10)
                {
                    return 403;
                }
                else if (result.Count != 0)
                {
                        for (int i = 0; i < result.Count; i++)
                        {

                            filme = result[i].ToObject<FilmesBanco>();

                            urlTrailer = QueryHelpers.AddQueryString("https://api.themoviedb.org/3/movie/" + filme.id + "/videos", query2);
                            requisicao = await client.GetAsync(urlTrailer);
                            json = await requisicao.Content.ReadAsStringAsync();
                            result2 = JObject.Parse(json).SelectToken("results").ToList();
                            if (result2.Count != 0)
                            {
                                keyYoutube = result2[0].ToObject<Youtube>();
                                filme.keyYoutube = keyYoutube;
                            }
                            filme.vote_count = 0;
                            filme.vote_average= 0;
                            await banco.PostRequisicaoAsync(filme);

                        }
                        return 200;
                }
                else
                {
                    return 404;
                }
            
            }
            catch (Exception err)
            {
                return 400;
            }
        }
    }
}
