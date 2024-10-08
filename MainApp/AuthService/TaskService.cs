using MainApi.Logger;
using MainApp.ViewModels;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace MainApp.AuthService
{
    public class TaskService
    {
        private readonly HttpClient _httpClient;

        public TaskService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Метод для создания новой задачи
        public async Task<TaskItem> CreateTaskAsync(TaskItem taskItem, string authToken)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);

                var response = await _httpClient.PostAsJsonAsync("https://localhost:7282/api/task", taskItem);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<TaskItem>();
                }
                else
                {
                    LoggerConfig.LogInformation("Ошибка из при отправке запроса  " + response.Headers);

                }
            }
            catch (Exception ex)
            {

                LoggerConfig.LogInformation("Ошибка из сОЗДАНИЯ Задачи " + ex.Message);
            }
           

            return null;
        }

        // Метод для получения списка задач
        public async Task<IEnumerable<TaskItem>> GetTasksAsync(string authToken)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);

            var response = await _httpClient.GetAsync("https://localhost:7282/api/task");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<IEnumerable<TaskItem>>();
            }

            return null;
        }


        public async Task<TaskItem> UpdateTaskAsync(int id, TaskItem taskItem, string authorizationHeader)
        {
            var json = JsonConvert.SerializeObject(taskItem);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", authorizationHeader);

            var response = await _httpClient.PutAsync($"https://localhost:7282/api/task/{id}", content);
            if (response.IsSuccessStatusCode)
            {
                return taskItem;
            }

            return null;
        }

        public async Task<bool> DeleteTaskAsync(int id, string authorizationHeader)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", authorizationHeader);

            var response = await _httpClient.DeleteAsync($"https://localhost:7282/api/task/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}

