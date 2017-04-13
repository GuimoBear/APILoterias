using Newtonsoft.Json;

namespace MegaResult.Entities
{
    public class Premiacao
    {
        public int Ganhadores { get; protected set; }
        public double Valor { get; protected set; }

        [JsonConstructor]
        protected Premiacao() { }

        public Premiacao(int Ganhadores, double Valor)
        {
            this.Ganhadores = Ganhadores;
            this.Valor = Valor;
        }
    }
}
