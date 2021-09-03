using System;
using lib.dal;
using lib.dto;
using lib.str;

namespace lib.lib.dal
{
    public class dal_mensagem
    {
        public dto_mensagem dal_max_cod(dal_conexao acn = null)
        {
            dal_conexao dalcon = acn ?? new dal_conexao();
            dalcon.OpenConn(acn == null);

            string vsql = string.Empty;

            try
            {
                vsql = "SELECT MAX(codigo) AS codigo FROM mensagem";


                dalcon.CreateCommand(vsql);
                dalcon.ExecReader();

                dto_mensagem rst = new dto_mensagem();

                if (dalcon.Reader.Read())
                    rst.codigo = str_lib.CampoInt64(dalcon.Reader, "codigo") + 1;

                dalcon.CloseReader();
                dalcon.CloseConn(acn == null);

                return rst;
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public void dal_inc(dto_mensagem adt, dal_conexao acn = null)
        {
            dal_conexao dalcon = acn ?? new dal_conexao();
            dalcon.OpenConn(acn == null);

            string vsql = string.Empty;

            try
            {
                vsql =
                    "INSERT INTO mensagem (" +
                    "   codigo, usr_codigo, mensagem, respondida" +
                    ") VALUES (" +
                    "   ?codigo, ?usr_codigo, ?mensagem, ?respondida" +
                    ")";

                dalcon.CreateCommand(vsql);

                dal_inc_param(adt, dalcon);

                 dalcon.ExecSQL();

                dalcon.CloseConn(true, acn == null);
            }
            catch (Exception ex)
            {
                dalcon.CloseConn(false, acn == null);
                throw;
            }
        }

        private void dal_inc_param(dto_mensagem adt, dal_conexao dalcon)
        {
            dalcon.AddParams("codigo", adt?.codigo);
            dalcon.AddParams("usr_codigo", adt?.usr?.ID);
            dalcon.AddParams("mensagem", adt?.mensagem);
            dalcon.AddParams("respondida", adt.respondida ? "S" : "N");
        }
    }
}
