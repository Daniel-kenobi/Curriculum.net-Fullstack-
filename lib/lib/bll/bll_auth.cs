using lib.dal;
using lib.dto;
using lib.lib.dal;
using System.Text;
using System.Security.Cryptography;
using System;

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

        private void fnc_valida_email(dto_usuario adt)
        {
            if ((dal.dal_login(new dto_usuario { Email = adt?.Email ?? string.Empty })?.ID ?? 0) > 0)
                throw new Exception("Usuário já cadastrado");
        }

        public void bll_cadastro(dto_usuario adt, dal_conexao acn = null)
        {
            fnc_valida_email(adt);

            adt.Senha = bll_senha_hash(adt.Senha);
            adt.ID = dal.dal_max_cod(acn).ID;
            adt.dt_cadastro = DateTime.Now;

            dal.dal_cadastro(adt, acn);
        }

        public void bll_atualizar(dto_usuario adt, dal_conexao acn = null)
        {
            fnc_valida_email(adt);
            dal.dal_atualizar(adt, acn);
        }
    }
}
