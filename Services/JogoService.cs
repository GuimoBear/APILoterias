using MegaResult.Entities;
using MegaResult.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace MegaResult.Services
{
    public class JogoService
    {
        private readonly ConcursoRepository repo;

        public JogoService(ConcursoRepository repo)
        {
            this.repo = repo;
        }

        public ResultadoChecagem Checar(IEnumerable<IEnumerable<int>> jogos)
        {
            if (ReferenceEquals(jogos, null) || jogos.Count() == 0)
                throw new System.Exception("Deve ser informado algum jogo");
            var jogosNaoNulos = jogos.Where(jogo => !ReferenceEquals(jogo, null));
            if (jogosNaoNulos.Count() == 0)
                throw new System.Exception("Deve ser informado ao menos um jogo dentro da lista");
            var jogosValidos = jogosNaoNulos.Select(jogo => new List<int>(jogo.Where(numero => numero >= 0 && numero <= 60))).Where(jogo => jogo.Count() >= 6);
            if (jogosNaoNulos.Count() == 0)
                throw new System.Exception("Não foi informado nenhum jogo com mais de seis numeros");
            return new ResultadoChecagem(jogosValidos, repo.Current);
        }

        public ResultadoChecagem Checar(IEnumerable<IEnumerable<int>> jogos, int concurso)
        {
            var resultadoConcurso = repo.Consultar(concurso);
            if (ReferenceEquals(jogos, null) || jogos.Count() == 0)
                throw new System.Exception("Deve ser informado algum jogo");
            var jogosNaoNulos = jogos.Where(jogo => !ReferenceEquals(jogo, null));
            if (jogosNaoNulos.Count() == 0)
                throw new System.Exception("Deve ser informado ao menos um jogo dentro da lista");
            var jogosValidos = jogosNaoNulos.Select(jogo => new List<int>(jogo.Where(numero => numero >= 0 && numero <= 60))).Where(jogo => jogo.Count() >= 6);
            if (jogosNaoNulos.Count() == 0)
                throw new System.Exception("Não foi informado nenhum jogo com mais de seis numeros");
            return new ResultadoChecagem(jogosValidos, resultadoConcurso);
        }
    }
}
