using System.Threading.Tasks;
using System.Web.Http;

namespace Quickstart.AspNet45.Controllers
{
    public class WebhookController : ApiController
    {
        [HttpPost]
        public async Task<IHttpActionResult> Webhook()
        {
            await Task.Delay(1);
            //this.ActionContext.
            return this.BadRequest();
        }
    }
}
