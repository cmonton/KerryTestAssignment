using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Http.HttpClientLibrary;

namespace Api.AddressApiClient
{
    public class AddressClientFactory
    {
        private readonly IAuthenticationProvider _authenticationProvider;
        private readonly HttpClient _httpClient;

        public AddressClientFactory(HttpClient httpClient)
        {
            _authenticationProvider = new AnonymousAuthenticationProvider();
            _httpClient = httpClient;
        }

        public AddressClient GetClient()
        {
            return new AddressClient(new HttpClientRequestAdapter(_authenticationProvider, httpClient: _httpClient));
        }
    }
}
