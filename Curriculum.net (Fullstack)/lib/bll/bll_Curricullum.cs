using lib.Interfaces;
using  lib.dto;
using System;
using lib.str;

namespace lib.bll
{
    /// <summary>Classe responsável pela lógica do e negócios (BLL - Business Logic Layer)</summary>
    public class bll_Curricullum : IBllInterface<dto_curriculo>
    {
        /// <summary>Método de validação do objeto dto_curriculo </summary>
        public void bll_vld(dto_curriculo adt)
        {
            if (adt == null)
                throw new Exception("Erro interno");

            if (string.IsNullOrEmpty(adt.Nome))
                throw new Exception("Nome vazio");

            if (string.IsNullOrEmpty(adt.Telefone) && string.IsNullOrEmpty(adt.Email))
                throw new Exception("Contatos inválidos");
        }

        /// <summary>Lógica de criação</summary>
        public bool bll_criaCurriculum(dto_curriculo adt)
        {
            try
            {
                bll_vld(adt);
                str_lib.criaPDF(str_lib.criaHTML(adt), adt.Nome);
                return true;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>Lógica que retorna a view do currículo (Não cria o PDF do mesmo)</summary>
        public string bll_retornaHTMLCurricullum(dto_curriculo adt)
        {
            try
            {
                bll_vld(adt);
                return str_lib.criaHTML(adt);
            }
            catch
            {
                throw;
            }
        }
    }
}
