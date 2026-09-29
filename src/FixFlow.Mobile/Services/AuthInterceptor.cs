using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;

namespace FixFlow.Mobile.Services
{
    public class AuthInterceptor : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = await SecureStorage.Default.GetAsync("jwt_token");

            if (!string.IsNullOrEmpty(token))
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            
            return await base.SendAsync(request, cancellationToken);
        }
    }
}
