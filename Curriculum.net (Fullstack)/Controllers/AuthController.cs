using Microsoft.AspNetCore.Mvc;
using System;
using lib.bll;
using lib.dto;
using Swashbuckle.AspNetCore.Annotations;
using lib.Retornos;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using lib.Filters;
using Microsoft.AspNetCore.Authorization;

namespace Curriculum.net.Controllers
{
    // ASSINANDO AS RESPOSTAS, 200 - OK, 400 - PARAMETROS OBRIGATÓRIOS FALTANTES, 
    [SwaggerResponse(statusCode: 200, "Token gerado com sucesso!", Type = typeof(Token))]
    [SwaggerResponse(statusCode: 201, "Curriculo criado com sucesso!", Type = typeof(Sucess))]
    [SwaggerResponse(statusCode: 400, "Parametros faltantes!", Type = typeof(ListError))]
    [SwaggerResponse(statusCode: 401, "Usuário não autorizado!", Type = typeof(ErroGenerico))]
    [SwaggerResponse(statusCode: 500, "Erro interno do servidor!", Type = typeof(ErroGenerico))]

    ///<summary>pre-fixo de rota (v1/api)</summary>
    [Route("v1/api/auth")] // configuração de ROTA, setei p prefixo da API para v1/
    public class AuthController : Controller
    {
        bll_auth bll;

        ///<summary>Construtor da classe</summary>
        public AuthController()
        {
            bll ??= new bll_auth();

            if (bll is null)
                throw new Exception("Erro interno");
        }

        ///<summary>Rota para gerar um novo currículo (v1/api/inc - POST)</summary>
        [HttpPost("login")] // v1/api/inc - POST - cria um novo curriculo
        [FilterRequest]  // ASSINO A CHAMADA DO FILTRO NESSA ROTA
        public IActionResult login([FromBody] dto_usuario adt)
        {
            try
            {
                var rst = bll.bll_login(adt);

                if ((rst?.ID ?? 0) > 0)
                    rst.token = token(rst).token;

                return ((rst?.ID ?? 0) != 0) ? Ok(rst) : NotFound("Usuário ou senha inválidos");
            }
            catch (Exception ex)
            {
                return BadRequest(ex?.InnerException?.Message ?? ex?.Message);
            }
        }

        ///<summary>Rota para obter um token com base no nome do dono do currículo (v1/api/Token - POST)</summary>
        [HttpPost("cadastrar")]
        public IActionResult cadastro([FromBody] dto_usuario adt)
        {
            try
            {
                bll.bll_cadastro(adt);
                adt.Senha = string.Empty;

                return Created("cadastrar", adt);
            }
            catch (Exception ex)
            {
                return BadRequest(ex?.InnerException?.Message ?? ex?.Message);
            }
        }

        [HttpPost("atualizar")]
        public IActionResult atualizar([FromBody] dto_usuario adt)
        {
            try
            {
                bll.bll_atualizar(adt);

                return Ok(adt);
            }
            catch (Exception ex)
            {
                return BadRequest(ex?.InnerException?.Message ?? ex?.Message);
            }
        }

        public dto_usuario token([FromBody] dto_usuario adt)
        {
            var secret = Encoding.ASCII.GetBytes("aa790cbf9d02deb783286181e4c5dd5");
            var symetricSecurityKey = new SymmetricSecurityKey(secret);
            var securityTokenDescriptor = new SecurityTokenDescriptor
            {
                /* DE FORMA RESUMIDA O CLAIM É UM ATRIBUTO, QUE TAMBÉM PODE SER HERDADO DA CLASSE PASSADA NO JSON
                 * PODE SER UM ATRIBUTO DO OBJETO, COMO NOME, EMAIL, E-mail, Telefone e etc */
                Subject = new ClaimsIdentity(new Claim[]
                {
                        new Claim(ClaimTypes.Email, adt.Email)
                }),
                Expires = DateTime.UtcNow.AddMinutes(20),
                SigningCredentials = new SigningCredentials(symetricSecurityKey, SecurityAlgorithms.HmacSha256Signature)
            };
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var tokenGenerated = jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);
            var token = jwtSecurityTokenHandler.WriteToken(tokenGenerated);

            return new dto_usuario { token = token };
        }
    }
}
