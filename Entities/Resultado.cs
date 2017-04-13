using MegaResult.Enums;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace MegaResult.Entities
{
    public sealed class ResultadoChecagem
    {
        public int[] Resultado { get; private set; }
        public IEnumerable<Jogo> Jogos { get; private set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public double? Valor { get; private set; }

        [JsonConstructor]
        private ResultadoChecagem() { }

        public ResultadoChecagem(IEnumerable<IEnumerable<int>> jogos, Concurso resultado)
        {
            Resultado = resultado.Resultado;
            Jogos = jogos.Select(jogo => new Jogo(jogo, resultado.Resultado));
            double valorPremio = 0;
            Jogos.ToList().ForEach(res =>
            {
                if (res.Ganhou.HasValue)
                {
                    switch (res.Ganhou.Value)
                    {
                        case PremioEnum.Quadra:
                            if (!ReferenceEquals(resultado.Quadra, null))
                                valorPremio += resultado.Quadra.Valor;
                            break;
                        case PremioEnum.Quina:
                            if (!ReferenceEquals(resultado.Quina, null))
                                valorPremio += resultado.Quina.Valor;
                            break;
                        case PremioEnum.Sena:
                            if(!ReferenceEquals(resultado.Sena, null))
                                valorPremio += resultado.Sena.Valor;
                            break;
                    }
                }
            });
            if (valorPremio > 0)
                Valor = valorPremio;
        }
    }
}
