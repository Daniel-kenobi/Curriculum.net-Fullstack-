using Microsoft.AspNetCore.Mvc;
using System;
using lib.bll;
using lib.dto;
using Swashbuckle.AspNetCore.Annotations;
using lib.Retornos;
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
    [Route("v1/api/mensagem")] // configuração de ROTA, setei o prefixo da API para v1/
    public class MensagemController : Controller
    {
        bll_mensagem bll;

        ///<summary>Construtor da classe</summary>
        public MensagemController()
        {
            bll ??= new bll_mensagem();

            if (bll is null)
                throw new Exception("Erro interno");
        }

        ///<summary>Rota para gerar um novo currículo (v1/api/inc - POST)</summary>
        [HttpPost("enviar")] // v1/api/inc - POST - cria um novo curriculo
        public IActionResult enviar([FromBody] dto_mensagem adt)
        {
            try
            {
                this.bll.bll_envia_mensagem(adt);

                return Ok("Enviada com sucesso");
            }
            catch (Exception ex)
            {
                return BadRequest(ex?.InnerException?.Message ?? ex.Message);
            }
        }
    }
}
