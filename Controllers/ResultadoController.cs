using Microsoft.AspNetCore.Mvc;
using MegaResult.Entities;
using MegaResult.Repositories;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace MegaResult.Controllers
{
    [Route("api/[controller]")]
    public class ResultadoController : Controller
    {
        private readonly ConcursoRepository repo;

        public ResultadoController(ConcursoRepository repo)
        {
            this.repo = repo;
        }

        // GET: api/resultado
        [HttpGet]
        public Concurso Get([FromQuery] int? concurso)
        {
            if(concurso.HasValue)
                return repo.Consultar(concurso.Value);
            return repo.Current;
        }
    }
}
