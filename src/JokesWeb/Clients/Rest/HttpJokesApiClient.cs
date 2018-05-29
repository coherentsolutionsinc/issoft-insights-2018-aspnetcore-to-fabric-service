using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using JokesApiContracts.Domain.Model;

using Newtonsoft.Json;

namespace JokesWeb.Clients.Rest
{
    public class HttpJokesApiClient : IJokesApiClient
    {
        private const string BASE_URI = "http://localhost:59689";

        public async Task<IEnumerable<JokesLanguageModel>> GetLanguagesAsync(
            CancellationToken cancellationToken)
        {
            var client = new HttpClient();

            var response = await client.GetAsync($"{BASE_URI}/api/jokes/languages", cancellationToken);

            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            var serializer = JsonSerializer.Create();
            using (var reader = new StringReader(json))
            {
                using (var jsonReader = new JsonTextReader(reader))
                {
                    return serializer.Deserialize<JokesLanguageModel[]>(jsonReader);
                }
            }
        }

        public async Task<IEnumerable<JokeModel>> GetJokesAsync(
            string language,
            string category,
            CancellationToken cancellationToken)
        {
            var client = new HttpClient();

            var response = await client.GetAsync($"{BASE_URI}/api/jokes/{language}/{category}", cancellationToken);

            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            var serializer = JsonSerializer.Create();
            using (var reader = new StringReader(json))
            {
                using (var jsonReader = new JsonTextReader(reader))
                {
                    return serializer.Deserialize<JokeModel[]>(jsonReader);
                }
            }
        }

        public async Task ImportJokesAsync(
            IEnumerable<JokeImportModel> importJokes,
            CancellationToken cancellationToken)
        {
            var client = new HttpClient();

            var serializer = JsonSerializer.Create();

            var builder = new StringBuilder();

            using (var writer = new StringWriter(builder))
            {
                serializer.Serialize(writer, importJokes);
            }

            var response = await client.PostAsync(
                $"{BASE_URI}/api/jokes/import",
                new StringContent(builder.ToString(), Encoding.UTF8, "application/json"),
                cancellationToken);

            response.EnsureSuccessStatusCode();
        }
    }
}