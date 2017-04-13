using System.Collections.Generic;

namespace MegaResult.Entities
{
    public class UF
    {
        public string Descricao { get; }
        public string Sigla { get; }
        public IEnumerable<Cidade> Cidades { get; }

        public UF(string descricao, string sigla, IEnumerable<Cidade> cidades)
        {
            Descricao = descricao;
            Sigla = sigla;
            Cidades = cidades;
        }
    }
}
