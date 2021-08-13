using SelectPdf;
using  lib.dto;
using System.Text;
using lib;
using System;

namespace lib.str
{
    /// <summary>Classe de tratamento de string </summary>
    public static class str_lib
    {
        ///<summary>Método que puxa os dados de endereço com bas eno CEP</summary>
        public static dto_cep RetornaCep(string cep)
        {
            if (string.IsNullOrEmpty(cep))
                return new dto_cep();

            dto_cep dto = wsexec.execGetWs<dto_cep>($"http://viacep.com.br/ws/", TimeSpan.FromSeconds(30),
                $"{cep}/json") ?? new dto_cep();

            return dto;
        }

        ///<summary>Método que cria o código HTML do curriculo</summary>
        public static string criaHTML(dto_curriculo infos)
        {
            StringBuilder sb = new StringBuilder();

            infos.cep = RetornaCep(infos.cep.cep);

            sb.Append("<!DOCTYPE html>");
            sb.Append("<html>");
            sb.Append("<head>");

            sb.Append("<meta charset=\"UTF-8\"/>");
            sb.Append("<style> body { font-family: Arial, Helvetica, sans-serif; margin: 30px 80px 70px 80px} </style>");

            sb.Append("</head>");
            sb.Append("<body>");

            sb.Append($"<h1 style=\"color:black; text-align: center\"> {infos.Nome} </h1>");

            sb.Append($"<p style=\"font-size: 16px; text-align: center\"><b> {infos.Email} • {infos.telefone} ");

            if (!string.IsNullOrEmpty(infos.cep.cep))
                sb.Append($"• {infos.cep.localidade} - {infos.cep.uf} ");

            if (!string.IsNullOrEmpty(infos.Github) && !string.Equals(infos.Github, "string"))
                sb.Append($"• Github: {infos.Github} ");

            if (!string.IsNullOrEmpty(infos.Linkedin) && !string.Equals(infos.Linkedin, "string"))
                sb.Append($"• Linkedin: {infos.Linkedin} ");

            if (!string.IsNullOrEmpty(infos.Instagram) && !string.Equals(infos.Instagram, "string"))
                sb.Append($"• Instagram: {infos.Instagram} ");

            sb.Append("</b></p>");

            
                if (string.Equals(infos.FraseMotivacional, "string") || string.IsNullOrEmpty(infos.FraseMotivacional))
                {
                    sb.Append("<p style = \"color: darkgray; font-size: 14px; text-align: center\"> Me encantaria encontrar uma vaga para essa empresa que é uma instituição que admiro tanto. Além disso, acredito que minha desenvoltura ");
                    sb.Append("natural com pessoas, ótima comunicação, e jeito cuidadoso se provarão muito úteis. Gostaria de poder falar sobre como posso contribuir para");
                    sb.Append(" essa empresa, e contando com experiência para o meu aperfeiçoamento pessoal, já agradeço pela futura resposta positiva!</p>");
                }
                else
                    sb.Append($"<p style = \"color: darkgray; font-size: 14px; text-align: center\">{infos.FraseMotivacional}</p>");

            if (infos.lst_infos_academicas.Count > 0)
            {
                sb.Append("<h2 style=\"color: steelblue;\"> Formação Acadêmica </h2>");

                foreach (var i in infos.lst_infos_academicas)
                {
                    sb.Append($"<h3 style=\"color: black;\"> {i.Nome_instituicao} • {i.TipoCurso}</h3>");
                    sb.Append($"<p style=\"color: darkgray;\">Concluído em {i.DataConclusao.ToString("dd/MM/yyyy")}</p>");
                    sb.Append($"<p style=\"color: darkgray;\"> {i.Descricao_aprendizado}</p>");
                }
            }

            if (infos.lst_Historico_Profissional.Count > 0)
            {
                sb.Append("<h2 style=\"color: steelblue;\"> Histórico Profissional </h2>");
                foreach (var i in infos.lst_Historico_Profissional)
                {
                    sb.Append($"<h3 style=\"color: black;\"> {i.Nome_instituicao} • {i.Cargo}</h3>");
                    sb.Append($"<p style=\"color: darkgray;\"> Entrada: {i.DataInicio.ToString("dd/MM/yyyy")} - Saída: {i.DataSaida.ToString("dd/MM/yyyy")}</p>");
                    sb.Append($"<p style=\"color: darkgray;\"> {i.Descricao_cargo}</p>");
                }

            }

            if (infos.lst_soft_skills.Count > 0)
            {
                sb.Append("<h2 style=\"color: steelblue;\"> Habilidades </h2>");

                foreach (var i in infos.lst_soft_skills)
                {
                    sb.Append($"<h3 style=\"color: black;\"> {i.Nome}</h3>");
                    sb.Append($"<p style=\"color: darkgray;\"> {i.descricao}");
                }
            }

            sb.Append("</body>");
            sb.Append("<footerp style=\"color: black; font-size: 12px; text-align: center\"><p>Agradeço o retorno positívo</p></footer>");
            sb.Append("</html>");

            return sb.ToString();
        }

        ///<summary>étodo que cria o PDF com base no HTML recebido</summary>
        public static string criaPDF(string HTML)
        {
            HtmlToPdf converter = new HtmlToPdf();
            PdfDocument document = converter.ConvertHtmlString(HTML);
            document.Save($"Curricullum.pdf");
            document.Close();
            return HTML;
        }
    }
}

