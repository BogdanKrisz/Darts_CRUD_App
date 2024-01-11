using EUDBLD_HFT_2023241.Logic;
using EUDBLD_HFT_2023241.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EUDBLD_HFT_2023241.Endpoint.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrizesController : ControllerBase
    {
        IPrizeLogic logic;

        public PrizesController(IPrizeLogic logic)
        {
            this.logic = logic;
        }

        [HttpGet]
        public IEnumerable<Prizes> ReadAll()
        {
            return this.logic.ReadAll();
        }

        [HttpGet("{id}")]
        public Prizes Read(int id)
        {
            return this.logic.Read(id);
        }

        [HttpPost]
        public void Create([FromBody] Prizes value)
        {
            this.logic.Create(value);
        }

        [HttpPut("{id}")]
        public void Update([FromBody] Prizes value)
        {
            this.logic.Update(value);
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            this.logic.Delete(id);
        }
    }
}
