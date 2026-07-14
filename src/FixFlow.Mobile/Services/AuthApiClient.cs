using FixFlow.Shared.Dtos.Auth.Login;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;

namespace FixFlow.Mobile.Services
{
    public class AuthApiClient
    {
        private readonly HttpClient _httpClient;

        private const string TokenKey = "jwt_token";
        private const string TenantKey = "tenant_id";

        public AuthApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;

            _httpClient.DefaultRequestHeaders.Add("X-DevTunnel-Skip", "true");
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<LoginResponseDto?> LoginAsync(string email, string password)
        {
            try
            {
                var request = new LoginRequestDto { Email = email, Password = password };

                var response = await _httpClient.PostAsJsonAsync("/api/Auth/login", request);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<LoginResponseDto>();

                    if (result != null && !string.IsNullOrWhiteSpace(result.Token))
                    {
                        // guardamos el estado en el hardware seguro del dispositivo
                        await SecureStorage.Default.SetAsync(TokenKey, result.Token);
                        await SecureStorage.Default.SetAsync(TenantKey, result.TenantId);
                        
                        // adjuntar el token a este cliente para futuras peticiones a endpoints seguros
                        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result.Token);
                        return result;
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error de conexion: {ex.Message}");
                return null;
            }
        }
        
        // limpiamos credenciales y destruimos la sesion http
        public void Logout()
        {
            SecureStorage.Default.Remove(TokenKey);
            SecureStorage.Default.Remove(TenantKey);
            _httpClient.DefaultRequestHeaders.Authorization = null;
        }
        
        // restauramos sesion si el usuario ya estaba logeado
        public async Task InitializeAuthAsync()
        {
            var token = await SecureStorage.Default.GetAsync(TokenKey);
        
            if (!string.IsNullOrWhiteSpace(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = 
                    new AuthenticationHeaderValue("Bearer", token);
            }
        }
    }
}
