using System;
namespace lib.dto
{
    public class dto_mensagem
    {
        public Int64 codigo { get; set; }
        public dto_usuario usr { get; set; }
        public string mensagem { get; set; }
        public bool respondida { get; set; }
    }
}
