using System.Collections.Generic;
using System.Web.Http;
using IglaClub.ObjectModel;

namespace IglaClub.Web.Controllers
{
    public class GAController : ApiController
    {
        // GET api/default1
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/default1/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/default1
        public void Post([FromBody]string value)
        {
        }

        // PUT api/default1/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/default1/5
        public void Delete(int id)
        {
        }

        public string ActiveUsers()
        {
            return GaService.GetGAData();

        }
    }
}
