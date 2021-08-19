using System;

namespace lib.dto
{
    [Serializable]
    public class dto_municipio
    {
        public string codigo { get; set; }
        public string nome { get; set; }
        public string uf_sigla { get; set; }
    }
}
