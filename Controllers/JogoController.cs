using MegaResult.Entities;
using MegaResult.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace MegaResult.Controllers
{
    [Route("api/[controller]")]
    public class JogoController : Controller
    {
        private readonly JogoService service;

        public JogoController(JogoService service)
        {
            this.service = service;
        }

        [HttpPost, Route("checar")]
        public ResultadoChecagem Checar([FromQuery] int? concurso, [FromBody] IEnumerable<IEnumerable<int>> jogos)
        {
            if(!concurso.HasValue)
                return service.Checar(jogos);
            return service.Checar(jogos, concurso.Value);
        }
    }
}
