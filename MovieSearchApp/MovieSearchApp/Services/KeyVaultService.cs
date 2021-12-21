using MovieSearchApp.Services.Rest;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MovieSearchApp.Services
{
    class KeyVaultService
    {
        private readonly IRestService _restService;

        public KeyVaultService(IRestService restService)
        {
            _restService = restService;
        }
        public async Task<string> GetKeysAsync()
        {
            var result = await _restService.GetAsync<string>($"http://movieappfunction.azurewebsites.net/api/Function1?code=Feka5i8Z/anvdUQMErpUggXazQaJcMlPGIu2nkJin8CizYT7MEw/0w==");

            return result.payload;
        }

    }
}
