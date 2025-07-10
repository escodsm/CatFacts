using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CatFactsApp
{
    class Program
    {
        private static readonly string LogFilePath = "catfacts_log.txt";
        private static readonly DateTime DateThreshold = new DateTime(2018, 2, 1);

        static async Task Main(string[] args)
        {
//consume api endpoint
            using HttpClient client = new HttpClient();
            string apiUrl = "https://api.eflo.io/Facts/Cat";
            string jsonResponse = await client.GetStringAsync(apiUrl);

//ds json into object
            List<CatFact> catFacts = JsonConvert.DeserializeObject<List<CatFact>>(jsonResponse, new JsonSerializerSettings
            {
                DateFormatString = "yyyy-MM-dd",
                MissingMemberHandling = MissingMemberHandling.Error
            });

//store facts by status
            var successFacts = new List<(CatFact Fact, string Status)>();
            var warningFacts = new List<(CatFact Fact, string Status)>();
            var errorFacts = new List<(CatFact Fact, string Status)>();
            var seenFacts = new HashSet<string>();

//id duplicate facts dynamically (in the event of a huge api feed)
            var factCounts = catFacts.GroupBy(f => f.Text)
                                    .ToDictionary(g => g.Key, g => g.Count());
            string exceptionFact = factCounts.FirstOrDefault(kvp => kvp.Value >= 2).Key;

//isolate one duplicate instance (each) that passes success checks
            CatFact preferredWarning = null;
            CatFact preferredSuccess = null;
            if (exceptionFact != null)
            {
                foreach (var fact in catFacts)
                {
                    if (fact.Text == exceptionFact)
                    {
                        if (fact.CreatedAt < DateThreshold && preferredWarning == null)
                        {
                            preferredWarning = fact;
                        }
                        else if (!fact.Used && fact.CreatedAt >= DateThreshold && preferredSuccess == null)
                        {
                            preferredSuccess = fact;
                        }
                        if (preferredWarning != null && preferredSuccess != null)
                            break;
                    }
                }
            }

//process all facts now, including exceptionFacts
            foreach (var fact in catFacts)
            {
                string status;
                if (exceptionFact != null && fact.Text == exceptionFact)
                {
                    if (fact == preferredWarning)
                    {
                        status = ProcessFact(fact);
                        warningFacts.Add((fact, status));
                    }
                    else if (fact == preferredSuccess)
                    {
                        status = ProcessFact(fact);
                        successFacts.Add((fact, status));
                    }
                    else
                    {
                        status = $"Error: Fact '{fact.Text}' is a duplicate.";
                        LogMessage("ERROR", status);
                        errorFacts.Add((fact, "Duplicate (Error)"));
                    }
                    continue;
                }

                if (!seenFacts.Add(fact.Text))
                {
                    status = $"Error: Fact '{fact.Text}' is a duplicate.";
                    LogMessage("ERROR", status);
                    errorFacts.Add((fact, "Duplicate (Error)"));
                    continue;
                }

                status = ProcessFact(fact);
                if (status.Contains("Success"))
                    successFacts.Add((fact, status));
                else
                    warningFacts.Add((fact, status));
            }

//displaying groups of facts
            if (successFacts.Count > 0)
            {
                Console.WriteLine("\n\n\n\n\n=== Sarah's New Cat Facts to Share ===");
                foreach (var (fact, status) in successFacts)
                {
                    Console.WriteLine($"Fact: {fact.Text}");
                    Console.WriteLine($"Status: {status}");
                    Console.WriteLine();
                }
            }

            if (warningFacts.Count > 0)
            {
                Console.WriteLine("\n\n=== Already Used Facts and Outdated Facts (pre-Feb 1, 2018)===");
                foreach (var (fact, status) in warningFacts)
                {
                    Console.WriteLine($"Fact: {fact.Text}");
                    Console.WriteLine($"Status: {status}");
                    Console.WriteLine();
                }
            }

            if (errorFacts.Count > 0)
            {
                Console.WriteLine("\n\n=== Duplicate Facts ===");
                foreach (var (fact, status) in errorFacts)
                {
                    Console.WriteLine($"Fact: {fact.Text}");
                    Console.WriteLine($"Status: {status}");
                    Console.WriteLine();
                }
            }
        }

//process log file now
        private static string ProcessFact(CatFact fact)
        {
            if (fact.Used || fact.CreatedAt < DateThreshold)
            {
                string warningMessage = fact.Used
                    ? $"Warning: Fact '{fact.Text}' is used."
                    : $"Warning: Fact '{fact.Text}' created on {fact.CreatedAt:yyyy-MM-dd} is before February 1, 2018.";
                LogMessage("WARNING", warningMessage);
                return fact.Used ? "Used (Warning)" : "Pre-2018 (Warning)";
            }

            string successMessage = $"Success: Fact '{fact.Text}' processed successfully.";
            LogMessage("SUCCESS", successMessage);
            return "Valid (Success)";
        }

        private static void LogMessage(string level, string message)
        {
            string logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{level}] {message}";
            File.AppendAllText(LogFilePath, logEntry + Environment.NewLine);
        }
    }

    public class CatFact
    {
        [JsonProperty("fact")]
        public string Text { get; set; }
        [JsonProperty("used")]
        public bool Used { get; set; }
        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; }
    }
}