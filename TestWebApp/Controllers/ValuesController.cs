using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace TestWebApp.Controllers
{
	public class ValuesController : ApiController
    {
        public string Post([FromBody]string value)
        {
            return AsyncHelper.RunSync(PostAsync);
        }

        private async Task<string> PostAsync()
        {
            await Task.Delay(10000);
            return "Hello World";
        }
    }
}
