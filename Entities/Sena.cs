using Newtonsoft.Json;
using System.Collections.Generic;

namespace MegaResult.Entities
{
    public class Sena : Premiacao
    {
        public IEnumerable<PremiacaoPorLocal> Premios { get; protected set; }

        [JsonConstructor]
        protected Sena() : base() { }

        public Sena(IEnumerable<PremiacaoPorLocal> ganhadores, int Ganhadores, double Valor) : base(Ganhadores, Valor)
        {
            Premios = ganhadores;
        }
    }
}
