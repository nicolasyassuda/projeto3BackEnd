using APIMOVIES.Models;
using APIMOVIES.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Runtime.InteropServices;

namespace APIMOVIES.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : Controller
    {
        [HttpPost]
        [Route("logar")]
        [SwaggerOperation(
        Summary = "Cadastra o filme na nossa Base de dados",
        Description = @"Ao pedir um filme pela requisição verifica-se se o filme existe na base para cadastrar.")]
        [SwaggerResponse(200, "Sucesso ao criar ao cadastrar o filme.")]
        [SwaggerResponse(500, "Erro na API")]
        public async Task<ObjectResult> Logar([FromBody] UserInput user)
        {
            MongoDBService client = new MongoDBService();

            long? resultado = await client.logar(user.user, user.password);
            if (resultado != null)
            {
                return Ok(resultado);
            }
            else
            {
                return Unauthorized("Login ou senha estao erradas");
            }

        }
        [HttpPost]
        [Route("registrar")]
        [SwaggerOperation(
        Summary = "Cadastra o filme na nossa Base de dados",
        Description = @"Ao pedir um filme pela requisição verifica-se se o filme existe na base para cadastrar.")]
        [SwaggerResponse(200, "Sucesso ao criar ao cadastrar o filme.")]
        [SwaggerResponse(500, "Erro na API")]
        public async Task<ObjectResult> Registrar([FromBody] UserInput user)
        {
            MongoDBService client = new MongoDBService();
            if(user.username== null)
            {
                return BadRequest("Error falta username");
            }
            long? resultado = await client.registrar(user.user, user.password,user.username);
            if (resultado != null)
            {
                return Ok(resultado);
            }
            else
            {
                return Unauthorized("Login ou senha estao erradas");
            }

        }
    }
}
