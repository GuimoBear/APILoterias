using Newtonsoft.Json;
using System;

namespace MegaResult.Entities
{
    public sealed class NumeroJogo
    {
        public int Numero { get; private set; }
        public bool Acertou { get; private set; }

        [JsonConstructor]
        private NumeroJogo() { }

        public NumeroJogo(int numero, int[] resultadoSorteio)
        {
            Numero = numero;
            Acertou = !ReferenceEquals(resultadoSorteio, null) && Array.IndexOf<int>(resultadoSorteio, numero) > -1;
        }
    }
}
