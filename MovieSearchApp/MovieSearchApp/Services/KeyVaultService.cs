using MovieSearchApp.Models.KeyVaultService;
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
        private KeyVaultModel _keys;

        public KeyVaultService(IRestService restService)
        {
            _restService = restService;


        }




        public async Task<KeyVaultModel> GetKeysAsync()
        {
            if(_keys == null)
            {
                var result = await _restService.GetAsync<KeyVaultModel>($"http://movieappfunction.azurewebsites.net/api/Function1?code=Feka5i8Z/anvdUQMErpUggXazQaJcMlPGIu2nkJin8CizYT7MEw/0w==");

                if (result.status == ResultStatus.Success)
                {
                    _keys = result.payload;
                }
            }
            return _keys;

            //return result.payload;
        }

    }
}
