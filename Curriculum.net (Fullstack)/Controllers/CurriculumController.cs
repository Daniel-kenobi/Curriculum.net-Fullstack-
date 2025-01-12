﻿using Microsoft.AspNetCore.Mvc;
using System;
using lib.bll;
using lib.dto;
using Swashbuckle.AspNetCore.Annotations;
using lib.Retornos;

namespace Curriculum.net.Controllers
{
    // ASSINANDO AS RESPOSTAS, 200 - OK, 400 - PARAMETROS OBRIGATÓRIOS FALTANTES, 
    [SwaggerResponse(statusCode: 200, "Token gerado com sucesso!", Type = typeof(Token))]
    [SwaggerResponse(statusCode: 201, "Curriculo criado com sucesso!", Type = typeof(Sucess))]
    [SwaggerResponse(statusCode: 400, "Parametros faltantes!", Type = typeof(ListError))]
    [SwaggerResponse(statusCode: 401, "Usuário não autorizado!", Type = typeof(ErroGenerico))]
    [SwaggerResponse(statusCode: 500, "Erro interno do servidor!", Type = typeof(ErroGenerico))]

    ///<summary>pre-fixo de rota (v1/api)</summary>
    [Route("v1/api")] // configuração de ROTA, setei p prefixo da API para v1/
    public class CurriculumController : Controller
    {
        bll_Curricullum bll;

        ///<summary>Construtor da classe</summary>
        public CurriculumController()
        {
            bll ??= new bll_Curricullum();

            if (bll is null)
                throw new Exception("Erro interno");
        }

        ///<summary>Rota para gerar um novo currículo (v1/api/inc - POST)</summary>
        [HttpPost("inc")] // v1/api/inc - POST - cria um novo curriculo
                          //  [FilterRequest]  // ASSINO A CHAMADA DO FILTRO NESSA ROTA
                          // [Authorize] // Requisito autorização por token
        public IActionResult criar([FromBody] dto_curriculo adt)
        {
            try
            {
                byte[] fileBytes = bll.bll_criaCurriculo(adt);
                return File(fileBytes, "application/pdf", "Curricullum");
            }
            catch (Exception ex)
            {
                return BadRequest(ex?.InnerException?.Message ?? ex.Message);
            }
        }
    }
}
