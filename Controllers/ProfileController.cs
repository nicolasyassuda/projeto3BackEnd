using APIMOVIES.Models;
using APIMOVIES.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace APIMOVIES.Controllers
{
    [ApiController]
    [Route("profile")]
    public class ProfileController : Controller
    {
        [HttpGet]
        [Route("getprofile")]
        [SwaggerOperation(
        Summary = "Cadastra o filme na nossa Base de dados",
        Description = @"Ao pedir um filme pela requisição verifica-se se o filme existe na base para cadastrar.")]
        [SwaggerResponse(200, "Sucesso ao criar ao cadastrar o filme.")]
        [SwaggerResponse(500, "Erro na API")]
        public async Task<Users> getProfile([FromQuery]string idProfile)
        {
            Users profile = new Users();
            MongoDBService client = new MongoDBService();
            profile = await client.GetProfile(idProfile);
            profile.password = "**********";
            return profile;
        }
        [HttpPost]
        [Route("favoritar")]
        [SwaggerOperation(
        Summary = "Cadastra o filme na nossa Base de dados",
        Description = @"Ao pedir um filme pela requisição verifica-se se o filme existe na base para cadastrar.")]
        [SwaggerResponse(200, "Sucesso ao criar ao cadastrar o filme.")]
        [SwaggerResponse(500, "Erro na API")]
        public async void favoritaFilme([FromBody] FavoritaInput favorito)
        {
            Users profile = new Users();
            MongoDBService client = new MongoDBService();
            await client.FavoritarFilme(favorito.UserId,favorito.codFilme);

        }
    }
}
