using MainApi.Logger;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Newtonsoft.Json;
using System.Text;
using System.Text.Json.Serialization;

namespace MainApp.AuthService
{
    public class AuthServices
    {
        private readonly HttpClient _httpClient;
        public AuthServices(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> LoginAsync(string Email, string password)
        {
            var loginmodel = new { Email = Email, Password = password };
            var json = JsonConvert.SerializeObject(loginmodel);

            var content = new StringContent(json,  Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("https://localhost:7137/api/Auth/login", content);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var token = JsonConvert.DeserializeObject<TokenModel>(result);
                return token?.Token;
            }

            return null;

        }

        public async Task<bool> RegisterAsync(string email, string password, string confirmPassword)
        {
            try
            {
                var json = JsonConvert.SerializeObject(new { Email = email, Password = password, ConfirmPassword = confirmPassword });
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("http://localhost:5074/api/Auth/register", content);
                LoggerConfig.LogInformation($"Ответ от AuthApi: {response}");

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    LoggerConfig.LogInformation($"ОШИБКА в content: {errorContent}");
                }

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                LoggerConfig.LogInformation(ex.Message);
            }
            finally
            {
                LoggerConfig.CloseLogger();
            }
            return false;
        }
    }

   
}
