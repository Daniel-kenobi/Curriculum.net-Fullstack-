using SelectPdf;
using lib.dto;
using System.Text;
using lib;
using System;
using System.IO;
using System.Collections.Generic;
using System.Data.Common;
using System.IO.Compression;
using System.ComponentModel;


namespace lib.str
{
    /// <summary>Classe de tratamento de string </summary>
    public static class str_lib
    {
        ///<summary>Método que puxa os dados de endereço com bas eno CEP</summary>
        public static dto_endereco RetornaCep(string cep)
        {
            if ((cep?.Length ?? 0) == 0)
                return new dto_endereco();

            return wsexec.execGetWs<dto_endereco>($"http://viacep.com.br/ws/", TimeSpan.FromSeconds(30), $"{cep}/json") ?? new dto_endereco();
        }

        ///<summary>Método que cria o código HTML do curriculo</summary>
        public static string geraHtml(dto_curriculo infos)
        {
            StringBuilder sb = new StringBuilder();

            infos.Endereco = RetornaCep(infos.Endereco.cep);

            sb.Append("<!DOCTYPE html>");
            sb.Append("<html>");
            sb.Append("<head>");

            sb.Append("<meta charset=\"UTF-8\"/>");
            sb.Append("<style> body { font-family: Arial, Helvetica, sans-serif; margin: 30px 80px 70px 80px} </style>");

            sb.Append("</head>");
            sb.Append("<body>");

            sb.Append($"<h1 style=\"color:black; text-align: center\"> {infos.Nome} </h1>");

            sb.Append($"<p style=\"font-size: 16px; text-align: center\"><b> {infos.Email} • {infos.Telefone} ");

            if (!string.IsNullOrEmpty(infos.Endereco.cep))
                sb.Append($"• {infos.Endereco.Localidade} - {infos.Endereco.UF} ");

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
                    sb.Append($"<p style=\"color: darkgray;\"> {i.Descricao}");
                }
            }

            sb.Append("</body>");
            sb.Append("<footer style=\"color: black; font-size: 12px; text-align: center\"><p>Agradeço o retorno positívo</p></footer>");
            sb.Append("</html>");

            return sb.ToString();
        }

        ///<summary>étodo que cria o PDF com base no HTML recebido</summary>
        public static byte[] criaPDF(string html, string nome)
        {
            HtmlToPdf converter = new HtmlToPdf();

            string dir_arqs = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "arquivos");
            string arq_path = Path.Combine(dir_arqs, $"{DateTime.Now.ToString("dd-MM-yyyy-HH-mm")}-{nome}-Curricullum.pdf");

            if (!Directory.Exists(dir_arqs))
                Directory.CreateDirectory(dir_arqs);

            PdfDocument document = converter.ConvertHtmlString(html);
            document.Save(arq_path);
            document.Close();

            byte[] arquivo = File.ReadAllBytes(arq_path);

            /*if (File.Exists(arq_path))
                File.Delete(arq_path);*/

            return arquivo ?? null;
        }

        public static string CodPaisBrasil()
        {
            return "1058";
        }

        public static string NomePaisBrasil()
        {
            return "Brasil";
        }

        public static List<string> lst_uf()
        {
            List<string> ret = new List<string>();
            ret.Add("AC");
            ret.Add("AL");
            ret.Add("AP");
            ret.Add("AM");
            ret.Add("BA");
            ret.Add("CE");
            ret.Add("DF");
            ret.Add("ES");
            ret.Add("GO");
            ret.Add("MA");
            ret.Add("MT");
            ret.Add("MS");
            ret.Add("MG");
            ret.Add("PR");
            ret.Add("PB");
            ret.Add("PA");
            ret.Add("PE");
            ret.Add("PI");
            ret.Add("RJ");
            ret.Add("RN");
            ret.Add("RS");
            ret.Add("RO");
            ret.Add("RR");
            ret.Add("SC");
            ret.Add("SE");
            ret.Add("SP");
            ret.Add("TO");
            return ret;
        }

        public static string StrCopy(string texto, int Index, int Count)
        {
            if ((texto?.Length ?? 0) == 0)
                return string.Empty;

            int idx = Index - 1;
            int cnt = Count;
            string ret = "";

            for (int x = 0; x <= (texto.Length - 1); x++)
            {
                if ((x >= idx) && (cnt > 0))
                {
                    ret = ret + texto[x];
                    cnt--;
                }
            }

            return ret;
        }

        public static double CampoDouble(DbDataReader reader, string coluna)
        {
            try
            {

                return (!reader.IsDBNull(reader.GetOrdinal(coluna))) ?
                            Convert.ToDouble(reader[coluna]) : 0;
            }
            catch
            {
                return 0;
            }
        }

        public static double? CampoDoubleNullable(DbDataReader reader, string coluna)
        {
            try
            {
                return (!reader.IsDBNull(reader.GetOrdinal(coluna))) ?
                            (double?)(reader[coluna]) : null;
            }
            catch
            {
                return null;
            }
        }

        public static float CampoFloat(DbDataReader reader, string coluna)
        {
            try
            {
                return (!reader.IsDBNull(reader.GetOrdinal(coluna))) ?
                            Convert.ToSingle(reader[coluna]) : 0;
            }
            catch
            {
                return 0;
            }
        }

        public static float? CampoFloatNullable(DbDataReader reader, string coluna)
        {
            try
            {
                return (!reader.IsDBNull(reader.GetOrdinal(coluna))) ?
                            (float?)(reader[coluna]) : null;
            }
            catch
            {
                return null;
            }
        }

        public static decimal CampoDecimal(DbDataReader reader, string coluna)
        {
            try
            {

                return (!reader.IsDBNull(reader.GetOrdinal(coluna))) ?
                            Convert.ToDecimal(reader[coluna]) : 0;
            }
            catch
            {
                return 0;
            }
        }

        public static decimal? CampoDecimalNullable(DbDataReader reader, string coluna)
        {
            try
            {
                return (!reader.IsDBNull(reader.GetOrdinal(coluna))) ?
                            (decimal?)(reader[coluna]) : null;
            }
            catch
            {
                return null;
            }
        }

        public static int CampoInt32(DbDataReader reader, string coluna)
        {
            try
            {

                return (!reader.IsDBNull(reader.GetOrdinal(coluna))) ?
                            Convert.ToInt32(reader[coluna]) : 0;
            }
            catch
            {
                return 0;
            }
        }

        public static int? CampoInt32Nullable(DbDataReader reader, string coluna)
        {
            try
            {
                return (!reader.IsDBNull(reader.GetOrdinal(coluna))) ?
                            (Int32?)(reader[coluna]) : null;
            }
            catch
            {
                return null;
            }
        }

        public static Int64 CampoInt64(DbDataReader reader, string coluna)
        {
            try
            {

                return (!reader.IsDBNull(reader.GetOrdinal(coluna))) ?
                            Convert.ToInt64(reader[coluna]) : 0;
            }
            catch
            {
                return 0;
            }
        }

        public static Int64? CampoInt64Nullable(DbDataReader reader, string coluna)
        {
            try
            {
                return (!reader.IsDBNull(reader.GetOrdinal(coluna))) ?
                            (Int64?)(reader[coluna]) : null;
            }
            catch
            {
                return null;
            }
        }

        public static DateTime CampoDateTime(DbDataReader reader, string coluna)
        {
            try
            {
                return (!reader.IsDBNull(reader.GetOrdinal(coluna))) ?
                            DateTime.Parse(reader[coluna].ToString()) : DateTime.MinValue;
            }
            catch
            {
                return DateTime.MinValue;
            }
        }

        public static DateTime? CampoDateTimeNullable(DbDataReader reader, string coluna)
        {
            try
            {
                DateTime? rst = (!reader.IsDBNull(reader.GetOrdinal(coluna))) ?
                            DateTime.Parse(reader[coluna].ToString()) : DateTime.MinValue;

                return (rst == DateTime.MinValue) ? null : rst;

            }
            catch
            {
                return null;
            }
        }

        public static string CampoString(DbDataReader reader, string coluna)
        {
            try
            {

                return (!reader.IsDBNull(reader.GetOrdinal(coluna))) ?
                            reader[coluna].ToString() : String.Empty;
            }
            catch
            {
                return String.Empty;
            }
        }

        public static bool CampoBool(DbDataReader reader, string coluna)
        {
            try
            {
                return CampoString(reader, coluna).ToUpper() == "S";
            }
            catch
            {
                return false;
            }
        }

        public static byte[] CampoBlob(DbDataReader reader, string coluna)
        {
            try
            {
                byte[] rst = null;

                if (!reader.IsDBNull(reader.GetOrdinal(coluna)))
                {
                    rst = new byte[(reader.GetBytes(reader.GetOrdinal(coluna), 0, null, 0, int.MaxValue))];
                    reader.GetBytes(reader.GetOrdinal(coluna), 0, rst, 0, rst.Length);
                }

                return rst;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static string strUrlApi(string urlBase, string urlApi)
        {
            if (urlApi.Trim().Length == 0)
                return string.Empty;

            if (str_lib.StrCopy(urlApi, 1, 4).ToLower() == "http")
                return urlApi;

            string barra = "/";

            if (str_lib.StrCopy(urlBase, 1, 4).ToLower() != "http")
                urlBase = "http://" + urlBase;

            if (str_lib.StrCopy(urlBase, urlBase.Length, 1) == "/")
                barra = "";

            if ((str_lib.StrCopy(urlApi, 1, 4).ToLower() != "http") && (str_lib.StrCopy(urlApi, 1, 1) == "/"))
                barra = "";

            return (str_lib.StrCopy(urlApi, 1, 4).ToLower() != "http") ? urlBase + barra + urlApi : string.Empty;
        }

        public static string fnc_retornarNumeros(string entrada)
        {
            if (string.IsNullOrEmpty(entrada))
                return "";

            StringBuilder saida = new StringBuilder(entrada.Length);

            foreach (char c in entrada)
                if (char.IsDigit(c))
                    saida.Append(c);

            return saida.ToString();
        }

        public static string nomeUF(int cUF)
        {
            string rst = "";

            try
            {
                switch (cUF)
                {
                    case 11: rst = "RO"; break;
                    case 12: rst = "AC"; break;
                    case 13: rst = "AM"; break;
                    case 14: rst = "RR"; break;
                    case 15: rst = "PA"; break;
                    case 16: rst = "AP"; break;
                    case 17: rst = "TO"; break;

                    case 21: rst = "MA"; break;
                    case 22: rst = "PI"; break;
                    case 23: rst = "CE"; break;
                    case 24: rst = "RN"; break;
                    case 25: rst = "PB"; break;
                    case 26: rst = "PE"; break;
                    case 27: rst = "AL"; break;
                    case 28: rst = "SE"; break;
                    case 29: rst = "BA"; break;

                    case 31: rst = "MG"; break;
                    case 32: rst = "ES"; break;
                    case 33: rst = "RJ"; break;
                    case 35: rst = "SP"; break;

                    case 41: rst = "PR"; break;
                    case 42: rst = "SC"; break;
                    case 43: rst = "RS"; break;

                    case 50: rst = "MS"; break;
                    case 51: rst = "MT"; break;
                    case 52: rst = "GO"; break;
                    case 53: rst = "DF"; break;

                    default: rst = ""; break;
                }

            }
            catch
            {
                return "";
            }

            return rst;

        }

        public static int codUF(string UF)
        {
            int rst = 0;

            try
            {
                switch (UF)
                {
                    case "RO": rst = 11; break;
                    case "AC": rst = 12; break;
                    case "AM": rst = 13; break;
                    case "RR": rst = 14; break;
                    case "PA": rst = 15; break;
                    case "AP": rst = 16; break;
                    case "TO": rst = 17; break;

                    case "MA": rst = 21; break;
                    case "PI": rst = 22; break;
                    case "CE": rst = 23; break;
                    case "RN": rst = 24; break;
                    case "PB": rst = 25; break;
                    case "PE": rst = 26; break;
                    case "AL": rst = 27; break;
                    case "SE": rst = 28; break;
                    case "BA": rst = 29; break;

                    case "MG": rst = 31; break;
                    case "ES": rst = 32; break;
                    case "RJ": rst = 33; break;
                    case "SP": rst = 35; break;

                    case "PR": rst = 41; break;
                    case "SC": rst = 42; break;
                    case "RS": rst = 43; break;

                    case "MS": rst = 50; break;
                    case "MT": rst = 51; break;
                    case "GO": rst = 52; break;
                    case "DF": rst = 53; break;

                    default: rst = 99; break;
                }

            }
            catch
            {
                return 0;
            }

            return rst;

        }

        public static byte[] strParaByteArray(string texto, char separador = ' ')
        {

            string[] lstStr = texto.Split(separador);

            byte[] buf = new byte[lstStr.Length];

            for (int x = 0; x < lstStr.Length; x++)
            {
                buf[x] = Convert.ToByte(lstStr[x]);
            }

            return buf;

        }

        public static string compactarStr(string text)
        {
            if ((text?.Length ?? 0) == 0)
                return text;

            byte[] buffer = Encoding.UTF8.GetBytes(text);
            MemoryStream ms = new MemoryStream();
            using (GZipStream zip = new GZipStream(ms, CompressionMode.Compress, true))
            {
                zip.Write(buffer, 0, buffer.Length);
            }

            ms.Position = 0;
            MemoryStream outStream = new MemoryStream();

            byte[] compressed = new byte[ms.Length];
            ms.Read(compressed, 0, compressed.Length);

            byte[] gzBuffer = new byte[compressed.Length + 4];
            System.Buffer.BlockCopy(compressed, 0, gzBuffer, 4, compressed.Length);
            System.Buffer.BlockCopy(BitConverter.GetBytes(buffer.Length), 0, gzBuffer, 0, 4);
            return Convert.ToBase64String(gzBuffer);
        }

        public static string descompactarStr(string compressedText)
        {
            if ((compressedText?.Length ?? 0) == 0)
                return compressedText;


            byte[] gzBuffer = Convert.FromBase64String(compressedText);

            using (MemoryStream ms = new MemoryStream())
            {
                int msgLength = BitConverter.ToInt32(gzBuffer, 0);
                ms.Write(gzBuffer, 4, gzBuffer.Length - 4);

                byte[] buffer = new byte[msgLength];

                ms.Position = 0;
                using (GZipStream zip = new GZipStream(ms, CompressionMode.Decompress))
                {
                    zip.Read(buffer, 0, buffer.Length);
                }
                return Encoding.UTF8.GetString(buffer);
            }
        }

        public static string espacoStrCentralizar(string str, int length, char character = ' ')
        {
            return str.PadLeft(((length - str.Length) / 2) + str.Length, character);//.PadRight(length, character);
        }


        public static string formatarCpfCnpj(string strCpfCnpj)
        {
            string texto = strCpfCnpj.Trim().Replace("/", "").Replace(".", "");

            if (texto.Length <= 11)
            {
                MaskedTextProvider mtpCpf = new MaskedTextProvider(@"000\.000\.000-00");
                mtpCpf.Set(zerosEsquerda(texto, 11));
                return mtpCpf.ToString();
            }
            else
            {
                MaskedTextProvider mtpCnpj = new MaskedTextProvider(@"00\.000\.000\/0000-00");
                mtpCnpj.Set(zerosEsquerda(texto, 14));
                return mtpCnpj.ToString();
            }
        }

        public static string removeMascara(string MaskedString)
        {
            if (string.IsNullOrEmpty(MaskedString))
                return string.Empty;

            return MaskedString.Replace(".", "").Replace(",", "").Replace("(", "").Replace(")", "")
                .Replace("/", "").Replace("_", "").Replace("-", "").Replace(":", "").Replace("#", "").Trim();
        }

        public static void validaEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);

                if (addr.Address != email)
                    throw new Exception("E-mail inválido!");
            }
            catch
            {
                throw;
            }
        }

        public static string zerosEsquerda(string strString, int intTamanho)
        {
            string strResult = "";
            for (int intCont = 1; intCont <= (intTamanho - strString.Length); intCont++)
            {
                strResult += "0";
            }
            return strResult + strString;
        }

        public static string formatarChaveNfe(string sChave)
        {
            string chaveAcesso = sChave;
            string novaChave = string.Empty;
            int contaChaveAcesso = 0;

            foreach (char c in chaveAcesso)
            {
                contaChaveAcesso++;
                novaChave += c;

                if (contaChaveAcesso == 4)
                {
                    novaChave += " ";
                    contaChaveAcesso = 0;
                }
            }
            return novaChave;
        }

        public static string fnc_extenso(decimal valor)
        {
            if (valor <= 0 | valor >= 1000000000000000)
                return "Valor não suportado pelo sistema.";
            else
            {
                string strValor = valor.ToString("000000000000000.00");
                string valor_por_extenso = string.Empty;

                for (int i = 0; i <= 15; i += 3)
                {
                    valor_por_extenso += escreva_parte(Convert.ToDecimal(strValor.Substring(i, 3)));
                    if (i == 0 & valor_por_extenso != string.Empty)
                    {
                        if (Convert.ToInt32(strValor.Substring(0, 3)) == 1)
                            valor_por_extenso += " TRILHÃO" + ((Convert.ToDecimal(strValor.Substring(3, 12)) > 0) ? " E " : string.Empty);
                        else if (Convert.ToInt32(strValor.Substring(0, 3)) > 1)
                            valor_por_extenso += " TRILHÕES" + ((Convert.ToDecimal(strValor.Substring(3, 12)) > 0) ? " E " : string.Empty);
                    }
                    else if (i == 3 & valor_por_extenso != string.Empty)
                    {
                        if (Convert.ToInt32(strValor.Substring(3, 3)) == 1)
                            valor_por_extenso += " BILHÃO" + ((Convert.ToDecimal(strValor.Substring(6, 9)) > 0) ? " E " : string.Empty);
                        else if (Convert.ToInt32(strValor.Substring(3, 3)) > 1)
                            valor_por_extenso += " BILHÕES" + ((Convert.ToDecimal(strValor.Substring(6, 9)) > 0) ? " E " : string.Empty);
                    }
                    else if (i == 6 & valor_por_extenso != string.Empty)
                    {
                        if (Convert.ToInt32(strValor.Substring(6, 3)) == 1)
                            valor_por_extenso += " MILHÃO" + ((Convert.ToDecimal(strValor.Substring(9, 6)) > 0) ? " E " : string.Empty);
                        else if (Convert.ToInt32(strValor.Substring(6, 3)) > 1)
                            valor_por_extenso += " MILHÕES" + ((Convert.ToDecimal(strValor.Substring(9, 6)) > 0) ? " E " : string.Empty);
                    }
                    else if (i == 9 & valor_por_extenso != string.Empty)
                        if (Convert.ToInt32(strValor.Substring(9, 3)) > 0)
                            valor_por_extenso += " MIL" + ((Convert.ToDecimal(strValor.Substring(12, 3)) > 0) ? " " : string.Empty);

                    if (i == 12)
                    {
                        if (valor_por_extenso.Length > 8)
                            if (valor_por_extenso.Substring(valor_por_extenso.Length - 6, 6) == "BILHÃO" | valor_por_extenso.Substring(valor_por_extenso.Length - 6, 6) == "MILHÃO")
                                valor_por_extenso += " DE";
                            else
                                if (valor_por_extenso.Substring(valor_por_extenso.Length - 7, 7) == "BILHÕES" | valor_por_extenso.Substring(valor_por_extenso.Length - 7, 7) == "MILHÕES" | valor_por_extenso.Substring(valor_por_extenso.Length - 8, 7) == "TRILHÕES")
                                valor_por_extenso += " DE";
                            else
                                    if (valor_por_extenso.Substring(valor_por_extenso.Length - 8, 8) == "TRILHÕES")
                                valor_por_extenso += " DE";

                        if (Convert.ToInt64(strValor.Substring(0, 15)) == 1)
                            valor_por_extenso += " REAL";
                        else if (Convert.ToInt64(strValor.Substring(0, 15)) > 1)
                            valor_por_extenso += " REAIS";

                        if (Convert.ToInt32(strValor.Substring(16, 2)) > 0 && valor_por_extenso != string.Empty)
                            valor_por_extenso += " E ";
                    }

                    if (i == 15)
                        if (Convert.ToInt32(strValor.Substring(16, 2)) == 1)
                            valor_por_extenso += " CENTAVO";
                        else if (Convert.ToInt32(strValor.Substring(16, 2)) > 1)
                            valor_por_extenso += " CENTAVOS";
                }
                return valor_por_extenso;
            }
        }

        private static string escreva_parte(decimal valor)
        {
            if (valor <= 0)
                return string.Empty;
            else
            {
                string montagem = string.Empty;
                if (valor > 0 & valor < 1)
                {
                    valor *= 100;
                }
                string strValor = valor.ToString("000");
                int a = Convert.ToInt32(strValor.Substring(0, 1));
                int b = Convert.ToInt32(strValor.Substring(1, 1));
                int c = Convert.ToInt32(strValor.Substring(2, 1));

                if (a == 1) montagem += (b + c == 0) ? "CEM" : "CENTO";
                else if (a == 2) montagem += "DUZENTOS";
                else if (a == 3) montagem += "TREZENTOS";
                else if (a == 4) montagem += "QUATROCENTOS";
                else if (a == 5) montagem += "QUINHENTOS";
                else if (a == 6) montagem += "SEISCENTOS";
                else if (a == 7) montagem += "SETECENTOS";
                else if (a == 8) montagem += "OITOCENTOS";
                else if (a == 9) montagem += "NOVECENTOS";

                if (b == 1)
                {
                    if (c == 0) montagem += ((a > 0) ? " E " : string.Empty) + "DEZ";
                    else if (c == 1) montagem += ((a > 0) ? " E " : string.Empty) + "ONZE";
                    else if (c == 2) montagem += ((a > 0) ? " E " : string.Empty) + "DOZE";
                    else if (c == 3) montagem += ((a > 0) ? " E " : string.Empty) + "TREZE";
                    else if (c == 4) montagem += ((a > 0) ? " E " : string.Empty) + "QUATORZE";
                    else if (c == 5) montagem += ((a > 0) ? " E " : string.Empty) + "QUINZE";
                    else if (c == 6) montagem += ((a > 0) ? " E " : string.Empty) + "DEZESSEIS";
                    else if (c == 7) montagem += ((a > 0) ? " E " : string.Empty) + "DEZESSETE";
                    else if (c == 8) montagem += ((a > 0) ? " E " : string.Empty) + "DEZOITO";
                    else if (c == 9) montagem += ((a > 0) ? " E " : string.Empty) + "DEZENOVE";
                }
                else if (b == 2) montagem += ((a > 0) ? " E " : string.Empty) + "VINTE";
                else if (b == 3) montagem += ((a > 0) ? " E " : string.Empty) + "TRINTA";
                else if (b == 4) montagem += ((a > 0) ? " E " : string.Empty) + "QUARENTA";
                else if (b == 5) montagem += ((a > 0) ? " E " : string.Empty) + "CINQUENTA";
                else if (b == 6) montagem += ((a > 0) ? " E " : string.Empty) + "SESSENTA";
                else if (b == 7) montagem += ((a > 0) ? " E " : string.Empty) + "SETENTA";
                else if (b == 8) montagem += ((a > 0) ? " E " : string.Empty) + "OITENTA";
                else if (b == 9) montagem += ((a > 0) ? " E " : string.Empty) + "NOVENTA";

                if (strValor.Substring(1, 1) != "1" & c != 0 & montagem != string.Empty) montagem += " E ";

                if (strValor.Substring(1, 1) != "1")
                    if (c == 1) montagem += "UM";
                    else if (c == 2) montagem += "DOIS";
                    else if (c == 3) montagem += "TRÊS";
                    else if (c == 4) montagem += "QUATRO";
                    else if (c == 5) montagem += "CINCO";
                    else if (c == 6) montagem += "SEIS";
                    else if (c == 7) montagem += "SETE";
                    else if (c == 8) montagem += "OITO";
                    else if (c == 9) montagem += "NOVE";

                return montagem;
            }
        }

        public static string formatarTel(string tel)
        {
            string texto = tel.Trim().Replace("(", "").Replace(")", "").Replace("-", "");

            if ((tel?.Length ?? 0) == 0)
                return tel;

            if (texto.Length == 10)
            {
                MaskedTextProvider mtpFone = new MaskedTextProvider("(00) 0000-0000");
                mtpFone.Set(zerosEsquerda(texto, 10));
                return mtpFone.ToString();
            }
            else
            {
                MaskedTextProvider mtpCel = new MaskedTextProvider("(00) 00000-0000");
                mtpCel.Set(zerosEsquerda(texto, 11));
                return mtpCel.ToString();
            }
        }

        public static string formatarCep(string cep)
        {
            string texto = cep.Trim().Replace("-", "");

            if ((cep?.Length ?? 0) == 0)
                return cep;

            MaskedTextProvider mtpCep = new MaskedTextProvider("00000-000");
            mtpCep.Set(zerosEsquerda(texto, 7));
            return mtpCep.ToString();
        }

        public static string RemoverAcentuacao(string txt)
        {
            string[] comAcento = new string[] { "ç", "Ç", "á", "é", "í", "ó", "ú", "ý", "Á", "É", "Í", "Ó", "Ú", "Ý", "à", "è", "ì", "ò", "ù",
                                                "À", "È", "Ì", "Ò", "Ù", "ã", "õ", "ñ", "ä", "ë", "ï", "ö", "ü", "ÿ", "Ä", "Ë", "Ï", "Ö", "Ü",
                                                "Ã", "Õ", "Ñ", "â", "ê", "î", "ô", "û", "Â", "Ê", "Î", "Ô", "Û" };

            string[] semAcento = new string[]  { "c", "C", "a", "e", "i", "o", "u", "y", "A", "E", "I", "O", "U", "Y", "a", "e", "i", "o", "u",
                                                 "A", "E", "I", "O", "U", "a", "o", "n", "a", "e", "i", "o", "u", "y", "A", "E", "I", "O", "U",
                                                 "A", "O", "N", "a", "e", "i", "o", "u", "A", "E", "I", "O", "U" };

            for (int i = 0; i < comAcento.Length; i++)
                txt = txt.Replace(comAcento[i], semAcento[i]);

            return txt;
        }

        public static string RemoverCaracteresEspeciais(string txt, bool removerEspacos = false, bool removerAcentuacao = false)
        {
            if (removerAcentuacao)
                txt = RemoverAcentuacao(txt);

            if (removerEspacos)
                txt = System.Text.RegularExpressions.Regex.Replace(txt, @"[^0-9a-zA-ZéúíóáÉÚÍÓÁèùìòàÈÙÌÒÀõãñÕÃÑêûîôâÊÛÎÔÂëÿüïöäËYÜÏÖÄçÇ]+?", string.Empty);
            else
                txt = System.Text.RegularExpressions.Regex.Replace(txt, @"[^0-9a-zA-ZéúíóáÉÚÍÓÁèùìòàÈÙÌÒÀõãñÕÃÑêûîôâÊÛÎÔÂëÿüïöäËYÜÏÖÄçÇ\s]+?", string.Empty);

            return txt;
        }
    }
}

