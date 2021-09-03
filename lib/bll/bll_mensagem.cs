using lib.dal;
using lib.dto;
using lib.lib.dal;
using System;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace lib.bll
{
    public class bll_mensagem
    {
        dal_mensagem dal;
        public bll_mensagem()
        {
            this.dal = new dal_mensagem();
        }

        private void bll_valida_mensagem(dto_mensagem adt)
        {
            if (adt is null)
                throw new Exception("Mensagem inválida");

            if ((adt?.mensagem?.Length ?? 0) < 5)
                throw new Exception("Mensagem muito pequena");

            if ((adt?.usr ?? null) == null)
                throw new Exception("remetente inválidao");

            if ((adt?.usr?.Email?.Length ?? 0) == 0 || !(adt?.usr?.Email?.Contains('@') ?? false))
                throw new Exception("Email do remetente inválido");

            if ((adt?.usr.Nome?.Length ?? 0) == 0)
                throw new Exception("Nome do remetente inválido");

            if ((adt?.usr?.Telefone?.Length ?? 0) == 0)
                throw new Exception("Telefone do remetente inválido");
        }

        public void bll_envia_mensagem(dto_mensagem adt, dal_conexao acn = null)
        {
            bll_valida_mensagem(adt);
            //bll_envia_mensagem_automatica(adt);

            adt.codigo = dal.dal_max_cod(acn).codigo;

            bll_auth auth = new bll_auth();
            adt.usr = auth.bll_login(adt.usr);

            dal.dal_inc(adt, acn);
        }

        private void bll_envia_mensagem_automatica(dto_mensagem adt)
        {
            SmtpClient smtp = new SmtpClient("my.smtp.exampleserver.net");

            smtp.UseDefaultCredentials = false;
            NetworkCredential basicAuthenticationInfo = new
            NetworkCredential("username", "password");
            smtp.Credentials = basicAuthenticationInfo;

            // add from,to mailaddresses
            MailAddress de = new MailAddress("test@example.com", "TestFromName");
            MailAddress para = new MailAddress("test2@example.com", "TestToName");
            MailMessage email = new MailMessage(de, para);

            // add ReplyTo
            MailAddress replyTo = new MailAddress("reply@example.com");
            email.ReplyToList.Add(replyTo);

            // set subject and encoding 
            email.Subject = "Test message";
            email.SubjectEncoding = Encoding.UTF8;

            // set body-message and encoding
            email.Body = "<b>Test Mail</b><br>using <b>HTML</b>.";
            email.BodyEncoding = Encoding.UTF8;
            // text or html
            email.IsBodyHtml = true;

            smtp.Send(email);
        }
    }
}
