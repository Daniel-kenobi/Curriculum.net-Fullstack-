using lib.dal;
using lib.dto;
using lib.lib.dal;
using System.Text;
using System.Security.Cryptography;

namespace lib.bll
{
    public class bll_auth
    {
        dal_auth dal;

        public bll_auth()
        {
            dal = new dal_auth();
        }

        public string bll_senha_hash(string senha)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.ASCII.GetBytes(senha);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }

        public dto_usuario bll_login(dto_usuario adt, dal_conexao acn = null)
        {
            adt.Senha = bll_senha_hash(adt.Senha);
            return dal.dal_login(adt, acn);
        }

        public void bll_cadastro(dto_usuario adt, dal_conexao acn = null)
        {
            adt.Senha = bll_senha_hash(adt.Senha);
            dal.dal_cadastro(adt, acn);
        }
    }
}
