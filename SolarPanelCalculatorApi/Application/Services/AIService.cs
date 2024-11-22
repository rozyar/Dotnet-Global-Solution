using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SolarPanelCalculatorApi.Application.Services
{
    public class AIService : IAIService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public AIService(IConfiguration configuration)
        {
            _apiKey = configuration["OpenAI:ApiKey"];
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://api.openai.com/v1/");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
        }

        public async Task<string> CalculateSolarPanels(double totalConsumption, int sunlightHours)
        {
            var prompt = $"Dada uma demanda diária total de energia de {totalConsumption} kWh, " +
                         $"calcule o número de painéis solares de 250W necessários para suprir essa demanda. " +
                         $"Considere que a localização recebe em média {sunlightHours} horas de sol por dia. " +
                         "Forneça a resposta exatamente no seguinte formato: " +
                         $"'Você precisará de X painéis solares de 250W para suprir seu consumo diário de {totalConsumption} kWh'. " +
                         "Apenas substitua 'X' pelo número calculado. Não forneça explicações adicionais.";

            var requestBody = new
            {
                model = "gpt-3.5-turbo",
                messages = new[]
                {
            new { role = "user", content = prompt }
             },
                max_tokens = 150,
                temperature = 0.0
            };

            var jsonRequestBody = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(jsonRequestBody, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("chat/completions", content);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Falha ao obter resposta da OpenAI: {errorContent}");
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();
            using var document = JsonDocument.Parse(jsonResponse);
            var completionText = document.RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString();

            return completionText.Trim();
        }
    }
}
