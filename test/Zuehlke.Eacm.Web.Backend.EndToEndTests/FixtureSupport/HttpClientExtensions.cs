using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Zuehlke.Eacm.Web.Backend.Diagnostics;

namespace Zuehlke.Eacm.Web.Backend.EndToEndTests.FixtureSupport
{
    public static class HttpClientExtensions
    {
        public static async Task<HttpResponseMessage> PostAsJsonAsync<T>(this HttpClient httpClient, string requestUrl, T value)
        {
            httpClient.ArgumentNotNull(nameof(httpClient));

            return await httpClient.PostAsync(requestUrl, new JsonContent(value));
        }

        public static async Task<T> ReadAsAsync<T>(this HttpResponseMessage response)
        {
            response.ArgumentNotNull(nameof(response));

            var responseText = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(responseText);
        }
    }
}