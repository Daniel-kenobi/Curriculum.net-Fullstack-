using lib.str;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web;

namespace lib.sistema
{
    public delegate void Evento<T>(T ojbect);

    public static class fncComuns
    {
        [DllImport("user32.dll")]
        static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        static extern bool AttachThreadInput(uint idAttach, uint idAttachTo, bool fAttach);

        [DllImport("user32.dll")]
        static extern uint GetWindowThreadProcessId(IntPtr hWnd, IntPtr ProcessId);

        [DllImport("kernel32.dll")]
        private static extern uint GetCurrentThread();

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool BringWindowToTop(IntPtr hWnd);

        public const int SW_RESTORE = 9;
        private const uint WS_MINIMIZE = 0x20000000;
        private const int SW_SHOW = 5;

        public static T fnc_converter_json<T>(string json) where T : class
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        public static double fnc_formatar_decimais(double pValor, int pCasas)
        {
            return Math.Round(pValor, pCasas);
        }

        public static string GetQueryString(object obj)
        {
            var properties = from p in obj.GetType().GetProperties()
                             where p.GetValue(obj, null) != null
                             select p.Name + "=" + HttpUtility.UrlEncode(p.GetValue(obj, null).ToString());

            return String.Join("&", properties.ToArray());
        }

        public static IEnumerable<List<T>> SplitList<T>(List<T> locations, int nSize = 20)
        {
            for (int i = 0; i < locations.Count; i += nSize)
            {
                yield return locations.GetRange(i, Math.Min(nSize, locations.Count - i));
            }
        }

        public static List<string> SplitString(string str_split, int nSize = 20)
        {
            List<string> rst = new List<string>();

            string str = str_split;

            while (str.Length > 0)
            {
                rst.Add(str_lib.StrCopy(str, 1, nSize));

                if (str.Length > nSize)
                {
                    str = str.Remove(nSize - 1);
                }
                else
                {
                    str = "";
                }
            }

            return rst;
        }

        public static bool validarCpfCnpj(string valor)
        {
            if (valor.Length > 11)
                return ValidarCNPJ(valor);
            else
                return validarCPF(valor);
        }

        private static bool ValidarCNPJ(string cnpj)
        {
            int[] multiplicador1 = new int[12] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

            int soma;
            int resto;

            string digito;
            string tempCnpj;

            cnpj = cnpj.Trim();

            cnpj = cnpj.Replace(".", "").Replace("-", "").Replace("/", "");

            if (cnpj.Length != 14)
                return false;

            for (int i = 0; i < 10; i++)
            {
                if (cnpj == i.ToString().PadLeft(14, i.ToString()[0]))
                    return false;
            }

            tempCnpj = cnpj.Substring(0, 12);
            soma = 0;
            for (int i = 0; i < 12; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador1[i];
            resto = (soma % 11);
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = resto.ToString();
            tempCnpj = tempCnpj + digito;
            soma = 0;
            for (int i = 0; i < 13; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador2[i];
            resto = (soma % 11);
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = digito + resto.ToString();

            return cnpj.EndsWith(digito);
        }

        private static bool validarCPF(string cpf)
        {
            int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            string tempCpf;
            string digito;

            int soma;
            int resto;

            cpf = cpf.Trim();
            cpf = cpf.Replace(".", "").Replace("-", "");

            if (cpf.Length != 11)
                return false;

            for (int i = 0; i < 10; i++)
            {
                if (cpf == i.ToString().PadLeft(11, i.ToString()[0]))
                    return false;
            }

            tempCpf = cpf.Substring(0, 9);
            soma = 0;

            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];
            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = resto.ToString();
            tempCpf = tempCpf + digito;
            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];
            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = digito + resto.ToString();

            return cpf.EndsWith(digito);
        }

        public static byte[] ObjectToByteArray(Object obj)
        {
            if (obj == null)
                return null;

            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }

        public static T ByteArrayToObject<T>(byte[] arrBytes) where T : class
        {
            if ((arrBytes?.Length ?? 0) == 0)
                return null;

            MemoryStream memStream = new MemoryStream();
            BinaryFormatter binForm = new BinaryFormatter();
            memStream.Write(arrBytes, 0, arrBytes.Length);
            memStream.Seek(0, SeekOrigin.Begin);
            T obj = (T)binForm.Deserialize(memStream);

            return obj;
        }

        public static byte[] Compactar(byte[] buffer)
        {
            if ((buffer?.Length ?? 0) == 0)
                return buffer;

            MemoryStream ms = new MemoryStream();
            using (GZipStream zip = new GZipStream(ms, CompressionMode.Compress, true))
            {
                zip.Write(buffer, 0, buffer.Length);
            }
            ms.Position = 0;

            byte[] compressed = new byte[ms.Length];
            ms.Read(compressed, 0, compressed.Length);

            byte[] gzBuffer = new byte[compressed.Length + 4];

            System.Buffer.BlockCopy(compressed, 0, gzBuffer, 4, compressed.Length);

            System.Buffer.BlockCopy(BitConverter.GetBytes(buffer.Length), 0, gzBuffer, 0, 4);

            return gzBuffer;
        }

        public static byte[] Descompactar(byte[] gzBuffer)
        {
            if ((gzBuffer?.Length ?? 0) == 0)
                return gzBuffer;

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
                return buffer;
            }
        }

        public static void FocusWindow(IntPtr focusOnWindowHandle)
        {
            int style = GetWindowLong(focusOnWindowHandle, -16);

            // Minimize and restore to be able to make it active.
            if ((style & WS_MINIMIZE) == WS_MINIMIZE)
            {
                ShowWindow(focusOnWindowHandle, SW_RESTORE);
            }

            uint currentlyFocusedWindowProcessId = GetWindowThreadProcessId(GetForegroundWindow(), IntPtr.Zero);
            uint appThread = GetCurrentThread();

            if (currentlyFocusedWindowProcessId != appThread)
            {
                AttachThreadInput(currentlyFocusedWindowProcessId, appThread, true);
                BringWindowToTop(focusOnWindowHandle);
                ShowWindow(focusOnWindowHandle, SW_SHOW);
                AttachThreadInput(currentlyFocusedWindowProcessId, appThread, false);
            }
            else
            {
                BringWindowToTop(focusOnWindowHandle);
                ShowWindow(focusOnWindowHandle, SW_SHOW);
            }
        }

        public static DateTime GetNetworkTime()
        {

            uint SwapEndianness(ulong x)
            {
                return (uint)(((x & 0x000000ff) << 24) +
                               ((x & 0x0000ff00) << 8) +
                               ((x & 0x00ff0000) >> 8) +
                               ((x & 0xff000000) >> 24));
            }

            try
            {
                //Servidor nacional para melhor latência
                const string ntpServer = "a.ntp.br";

                // Tamanho da mensagem NTP - 16 bytes (RFC 2030)
                var ntpData = new byte[48];

                //Indicador de Leap (ver RFC), Versão e Modo
                ntpData[0] = 0x1B; //LI = 0 (sem warnings), VN = 3 (IPv4 apenas), Mode = 3 (modo cliente)

                var addresses = Dns.GetHostEntry(ntpServer).AddressList;

                //123 é a porta padrão do NTP
                var ipEndPoint = new IPEndPoint(addresses[0], 123);
                //NTP usa UDP
                var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                socket.Connect(ipEndPoint);

                //Caso NTP esteja bloqueado, ao menos nao trava o app
                socket.ReceiveTimeout = 5000;

                socket.Send(ntpData);
                socket.Receive(ntpData);
                socket.Close();

                //Offset para chegar no campo "Transmit Timestamp" (que é
                //o do momento da saída do servidor, em formato 64-bit timestamp
                const byte serverReplyTime = 40;

                //Pegando os segundos
                ulong intPart = BitConverter.ToUInt32(ntpData, serverReplyTime);

                //e a fração de segundos
                ulong fractPart = BitConverter.ToUInt32(ntpData, serverReplyTime + 4);

                //Passando de big-endian pra little-endian
                intPart = SwapEndianness(intPart);
                fractPart = SwapEndianness(fractPart);

                var milliseconds = (intPart * 1000) + ((fractPart * 1000) / 0x100000000L);

                //Tempo em **UTC**
                var networkDateTime = (new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc)).AddMilliseconds((long)milliseconds);

                return networkDateTime.ToLocalTime();
            }
            catch
            {
                return DateTime.Now;
            }
        }

        public static bool IsPis(string pis)
        {
            int[] multiplicador = new int[10] { 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int soma;
            int resto;

            if (pis.Trim().Length != 11)
                return false;

            pis = pis.Trim();
            pis = pis.Replace("-", "").Replace(".", "").PadLeft(11, '0');

            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += int.Parse(pis[i].ToString()) * multiplicador[i];

            resto = soma % 11;

            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            return pis.EndsWith(resto.ToString());
        }
    }
}
