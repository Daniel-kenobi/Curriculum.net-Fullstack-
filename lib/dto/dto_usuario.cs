using System;

namespace lib.dto
{
    [Serializable]
    public class dto_usuario
    {
        public Int64 ID { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public string Telefone { get; set; }
        public string Instagram { get; set; }
        public string Linkedin { get; set; }
        public string Github { get; set; }
        public DateTime dt_cadastro { get; set; }
        public byte[] img_perfil { get; set; }
        public string token { get; set; }
    }
}
