using lib.dto;
using System;
using lib.str;

namespace lib.bll
{
    /// <summary>Classe responsável pela lógica do e negócios (BLL - Business Logic Layer)</summary>
    public class bll_Curricullum 
    {
        /// <summary>Método de validação do objeto dto_curriculo </summary>
        private void bll_valida(dto_curriculo adt)
        {
            if (adt is null)
                throw new Exception("Dados inválidos");

            if (string.IsNullOrEmpty(adt.Nome))
                throw new Exception("Nome vazio");

            if (((adt?.Telefone?.Length ?? 0) == 0) && (adt?.Email?.Length ?? 0) == 0)
                throw new Exception("Contatos inválidos");
        }

        /// <summary>Lógica de criação</summary>
        public void bll_criaCurriculo(dto_curriculo adt)
        {
            bll_valida(adt);
            str_lib.criaPDF(str_lib.geraHtml(adt), adt.Nome);
        }

        /// <summary>Lógica que retorna a view do currículo (Não cria o PDF do mesmo)</summary>
        public string bll_retornaHtml(dto_curriculo adt)
        {
            bll_valida(adt);
            return str_lib.geraHtml(adt);
        }
    }
}
