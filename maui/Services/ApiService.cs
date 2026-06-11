using BuechereiApp.Models;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;

namespace BuechereiApp.Services
{
    public class ApiService
    {
        private readonly HttpClient _http;
        private const string BaseUrl = "http://localhost:8000";

        public ApiService()
        {
            _http = new HttpClient();
        }

        public async Task<List<Buch>> GetBuecher()
        {
            return await _http.GetFromJsonAsync<List<Buch>>($"{BaseUrl}/buecher");
        }
        public async Task<Buch> BuchHinzufuegen(string titel, string autor, string isbn, int menge)
        {
            var url = $"{BaseUrl}/buecher?titel={titel}&autor={autor}&isbn={isbn}&menge={menge}";
            var response = await _http.PostAsync(url, null);
            return await response.Content.ReadFromJsonAsync<Buch>();
        }

        public async Task<List<Mitglied>> GetMitglieder()
        {
            return await _http.GetFromJsonAsync<List<Mitglied>>($"{BaseUrl}/mitglieder");
        }

        public async Task<Mitglied> MitgliedHinzufuegen(string name, string email)
        {
            var url = $"{BaseUrl}/mitglieder?name={name}&email={email}";
            var response = await _http.PostAsync(url, null);
            return await response.Content.ReadFromJsonAsync<Mitglied>();
        }



    }
}
