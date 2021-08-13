using System;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace lib
{
    public delegate void execRetornoWs(object ojbect);

    static public class wsexec
    {
        public static T execGetWs<T>(string ApiUrl, TimeSpan Tempo, string parametro = "",
            execRetornoWs execFalhaRetorno = null, string token = "") where T : class
        {
            try
            {
                HttpClient client = new HttpClient();
                client.Timeout = Tempo;
                client.BaseAddress = new Uri($"{ApiUrl}/{parametro}");

                client.DefaultRequestHeaders.Accept.Add(
                   new MediaTypeWithQualityHeaderValue("application/json"));

                if ((token?.Length ?? 0) > 0)
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = client.GetAsync(client.BaseAddress).Result;

                using (HttpContent content = response.Content)
                {
                    var strResult = content.ReadAsStringAsync();

                    if (!response.IsSuccessStatusCode)
                        throw new Exception(response.ReasonPhrase + " - " + strResult.Result);

                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                        return null;

                    var rootobject = JsonConvert.DeserializeObject<T>(strResult.Result);

                    return rootobject;

                }

            }
            catch (Exception ex)
            {
                execFalhaRetorno?.DynamicInvoke(ex?.InnerException?.Message ?? ex.Message);
                return null;
            }

        }

        public static async Task<T> execGetWsTask<T>(string ApiUrl, TimeSpan Tempo, string parametro = "",
            execRetornoWs execFalhaRetorno = null, string token = "") where T : class
        {
            try
            {
                HttpClient client = new HttpClient();
                client.Timeout = Tempo;
                client.BaseAddress = new Uri($"{ApiUrl}/{parametro}");

                client.DefaultRequestHeaders.Accept.Add(
                   new MediaTypeWithQualityHeaderValue("application/json"));

                if ((token?.Length ?? 0) > 0)
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = await client.GetAsync(client.BaseAddress);

                using (HttpContent content = response.Content)
                {
                    var strResult = content.ReadAsStringAsync();

                    if (!response.IsSuccessStatusCode)
                        throw new Exception(response.ReasonPhrase + " - " + strResult.Result);

                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                        return null;

                    var rootobject = JsonConvert.DeserializeObject<T>(strResult.Result);

                    return rootobject;

                }

            }
            catch (Exception ex)
            {
                execFalhaRetorno?.DynamicInvoke(ex?.InnerException?.Message ?? ex.Message);
                return null;
            }

        }

        public static T execPostWs<T>(string ApiUrl, TimeSpan Tempo, object parametro = null,
            execRetornoWs execFalhaRetorno = null, string token = "") where T : class
        {
            try
            {
                HttpClient client = new HttpClient();
                client.Timeout = Tempo;
                client.BaseAddress = new Uri(ApiUrl);
                client.DefaultRequestHeaders.Accept.Add(
                   new MediaTypeWithQualityHeaderValue("application/json"));

                if ((token?.Length ?? 0) > 0)
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var param = JsonConvert.SerializeObject(parametro);
                HttpContent contentPost = new StringContent(param, Encoding.UTF8, "application/json");

                HttpResponseMessage response = client.PostAsync(ApiUrl, contentPost).Result;

                using (HttpContent content = response.Content)
                {
                    var strResult = content.ReadAsStringAsync();

                    if (!response.IsSuccessStatusCode)
                        throw new Exception(strResult.Result);

                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                        return null;

                    var rootobject = JsonConvert.DeserializeObject<T>(strResult.Result);

                    return rootobject;
                }

            }
            catch (Exception ex)
            {
                execFalhaRetorno?.DynamicInvoke(ex?.InnerException?.Message ?? ex.Message);
                return null;
            }
        }

        public static async Task<T> execPostWsTask<T>(string ApiUrl, TimeSpan Tempo, object parametro = null,
            execRetornoWs execFalhaRetorno = null, string token = "") where T : class
        {
            try
            {
                HttpClient client = new HttpClient
                {
                    Timeout = Tempo,
                    BaseAddress = new Uri(ApiUrl)
                };
                client.DefaultRequestHeaders.Accept.Add(
                   new MediaTypeWithQualityHeaderValue("application/json"));

                var param = JsonConvert.SerializeObject(parametro);
                HttpContent contentPost = new StringContent(param, Encoding.UTF8, "application/json");

                if ((token?.Length ?? 0) > 0)
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = null;

                response = await client.PostAsync(ApiUrl, contentPost);

                using (HttpContent content = response.Content)
                {
                    var strResult = content.ReadAsStringAsync();

                    if (!response.IsSuccessStatusCode)
                        throw new Exception(strResult.Result);

                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                        return null;

                    var rootobject = JsonConvert.DeserializeObject<T>(strResult.Result);
                    return rootobject;
                }

            }
            catch (Exception ex)
            {
                execFalhaRetorno?.DynamicInvoke(ex?.InnerException?.Message ?? ex.Message);
                return null;
            }

        }

        public static T execWs<T>(request tp, string ApiUrl, TimeSpan Tempo, object parametro = null,
            execRetornoWs execFalhaRetorno = null, string token = "") where T : class
        {
            try
            {
                string baseAddres = ApiUrl;

                if ((request.Get == tp) || (request.Delete == tp))
                    baseAddres += "/" + (parametro?.ToString() ?? "");

                HttpClient client = new HttpClient
                {
                    Timeout = Tempo,
                    BaseAddress = new Uri(baseAddres)
                };

                client.DefaultRequestHeaders.Accept.Add(
                   new MediaTypeWithQualityHeaderValue("application/json"));

                HttpContent contentPost = null;

                if ((request.Post == tp) || (request.Put == tp))
                {
                    string param;
                    param = JsonConvert.SerializeObject(parametro);
                    contentPost = new StringContent(param, Encoding.UTF8, "application/json");
                }

                if ((token?.Length ?? 0) > 0)
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = null;

                switch (tp)
                {
                    case request.Get:
                        response = client.GetAsync(client.BaseAddress).Result;
                        break;
                    case request.Post:
                        response = client.PostAsync(ApiUrl, contentPost).Result;
                        break;
                    case request.Put:
                        response = client.PutAsync(ApiUrl, contentPost).Result;
                        break;
                    case request.Delete:
                        response = client.DeleteAsync(client.BaseAddress).Result;
                        break;
                }

                using (HttpContent content = response.Content)
                {
                    var strResult = content.ReadAsStringAsync();

                    if (!response.IsSuccessStatusCode)
                        throw new Exception(strResult.Result);

                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                        return null;

                    if ((tp == request.Put) || (tp == request.Delete))
                        return (T)parametro;

                    var rootobject = JsonConvert.DeserializeObject<T>(strResult.Result);
                    return rootobject;
                }
            }
            catch (Exception ex)
            {
                execFalhaRetorno?.DynamicInvoke(ex?.InnerException?.Message ?? ex.Message);
                return null;
            }
        }

        public static async Task<T> execWsTask<T>(request tp, string ApiUrl, TimeSpan Tempo, object parametro = null,
            execRetornoWs execFalhaRetorno = null, string token = "") where T : class
        {
            try
            {
                string baseAddres = ApiUrl;

                if ((request.Get == tp) || (request.Delete == tp))
                    baseAddres += "/" + (parametro?.ToString() ?? "");

                HttpClient client = new HttpClient
                {
                    Timeout = Tempo,
                    BaseAddress = new Uri(baseAddres)
                };

                client.DefaultRequestHeaders.Accept.Add(
                   new MediaTypeWithQualityHeaderValue("application/json"));

                HttpContent contentPost = null;

                if ((request.Post == tp) || (request.Put == tp))
                {
                    var param = JsonConvert.SerializeObject(parametro);
                    contentPost = new StringContent(param, Encoding.UTF8, "application/json");
                }

                if ((token?.Length ?? 0) > 0)
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = null;

                switch (tp)
                {
                    case request.Get:
                        response = await client.GetAsync(client.BaseAddress);
                        break;
                    case request.Post:
                        response = await client.PostAsync(ApiUrl, contentPost);
                        break;
                    case request.Put:
                        response = await client.PutAsync(ApiUrl, contentPost);
                        break;
                    case request.Delete:
                        response = await client.DeleteAsync(client.BaseAddress);
                        break;
                }

                using (HttpContent content = response.Content)
                {
                    var strResult = content.ReadAsStringAsync();

                    if (!response.IsSuccessStatusCode)
                        throw new Exception(strResult.Result);

                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                        return null;

                    if ((tp == request.Put) || (tp == request.Delete))
                        return (T)parametro;

                    var rootobject = JsonConvert.DeserializeObject<T>(strResult.Result);
                    return rootobject;
                }
            }
            catch (Exception ex)
            {
                execFalhaRetorno?.DynamicInvoke(ex?.InnerException?.Message ?? ex.Message);
                return null;
            }
        }
    }

    public enum request
    {
        Get,
        Post,
        Put,
        Delete
    }
}
