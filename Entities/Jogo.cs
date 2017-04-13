using MegaResult.Enums;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace MegaResult.Entities
{
    public sealed class Jogo
    {
        public IEnumerable<NumeroJogo> Numeros { get; private set; }
        public int Acertos { get; private set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public PremioEnum? Ganhou { get; private set; }

        [JsonConstructor]
        private Jogo() { }

        public Jogo(IEnumerable<int> jogo, int[] resultadoSorteio)
        {
            Numeros = jogo.Select(n => new NumeroJogo(n, resultadoSorteio));
            Acertos = Numeros.Count(n => n.Acertou);
            switch(Acertos)
            {
                case 4:
                    Ganhou = PremioEnum.Quadra;
                    break;
                case 5:
                    Ganhou = PremioEnum.Quina;
                    break;
                case 6:
                    Ganhou = PremioEnum.Sena;
                    break;
                default:
                    Ganhou = null;
                    break;
            }
        }
    }
}
