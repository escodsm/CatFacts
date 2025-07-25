using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using CatFactsApp.Models;

namespace CatFactsApp.Services
{
    public class CatFactService
    {
        private readonly HttpClient _client;
        private const string ApiUrl = "https://api.eflo.io/Facts/Cat";
        private const string LogFilePath = "logs.txt";
        private static readonly DateTime CutoffDate = new DateTime(2018, 2, 1);

        public CatFactService(HttpClient client)
        {
            _client = client;
        }

        public async Task<List<CatFact>> FetchAndProcessCatFacts()
        {
            try
            {
                // Fetch facts
                HttpResponseMessage response = await _client.GetAsync(ApiUrl);
                response.EnsureSuccessStatusCode();
                string jsonResponse = await response.Content.ReadAsStringAsync();
                var facts = JsonSerializer.Deserialize<List<CatFact>>(jsonResponse, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

                // Process facts
                foreach (var fact in facts)
                {
                    fact.LogStatus = ProcessCatFact(fact);
                }

                return facts;
            }
            catch (Exception ex)
            {
                string errorMessage = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] ERROR: Failed to fetch or process cat facts: {ex.Message}";
                LogToFile(errorMessage);
                throw; // Re-throw to handle in controller
            }
        }

        private string ProcessCatFact(CatFact fact)
        {
            string logMessage;
            string logLevel;

            if (fact.CreatedAt < CutoffDate)
            {
                logLevel = "ERROR";
                logMessage = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] ERROR: Fact '{fact.Text}' created on {fact.CreatedAt:yyyy-MM-dd} is before February 1, 2018.";
            }
            else if (!fact.Used)
            {
                logLevel = "WARNING";
                logMessage = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] WARNING: Fact '{fact.Text}' has not been used.";
            }
            else
            {
                logLevel = "SUCCESS";
                logMessage = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] SUCCESS: Fact '{fact.Text}' processed successfully.";
            }

            LogToFile(logMessage);
            return logLevel;
        }

        private void LogToFile(string message)
        {
            File.AppendAllText(LogFilePath, message + Environment.NewLine);
        }
    }
}