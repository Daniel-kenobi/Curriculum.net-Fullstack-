using System;
using System.Collections.Generic;
using  lib.dto;

/// CLASSE PARA MAPEAMENTO DOS ERROS GENÉRICOS NO SWAGGER

namespace lib.Retornos
{
    /// <summary>Classe de erro genérico (400 - BAD REQUEST)</summary>
    public class ErroGenerico
    {
        public string ErrorMessage { get; set; }
        public ErroGenerico(string ErrorMessage)
        {
            this.ErrorMessage = ErrorMessage;
        }
    }

    /// <summary>Classe de lista de erros (400 - BAD REQUEST) )</summary>
    public class ListError
    {
        public IEnumerable<string> Erros { get; private set; }

        public ListError(IEnumerable<string> Erros)
        {
            this.Erros = Erros;
        }
    }

    /// <summary>Classe de token (201 - CREATED - TOKEN))</summary>
    public class Token
    {
        public string token{ get; set; }
        public string Description { get; set; }
        public Token(string Description)
        {
            this.Description = Description;
        }
    }

    /// <summary>Classe de sucesso (200 - OK)</summary>
    public class Sucess
    {
        public dto_curriculo Model { get; set; }
    }
}
