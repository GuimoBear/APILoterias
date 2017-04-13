using MegaResult.Repositories;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace MegaResult.Entities
{
    public class PremiacaoPorLocal
    {
        public UF Local { get; protected set; }
        public int Ganhadores { get; protected set; }

        [JsonConstructor]
        protected PremiacaoPorLocal() { }

        public PremiacaoPorLocal(string uf_cidade, string mensagem)
        {
            if (!string.IsNullOrWhiteSpace(uf_cidade))
            {
                var str = uf_cidade.Split('-').ToList();
                var UF = str.Last().Trim();
                str.RemoveAt(str.Count - 1);
                var Cidade = string.Join("-", str).Trim();
                Local = UFRepository.Current.GetUFECidade(UF, Cidade);
            }
            if (!string.IsNullOrWhiteSpace(mensagem))
            {
                Regex r = new Regex(@"(?<Ganhadores>\d+) aposta(s|) ganh(ou|aram) o prêmio para (?<Acertos>\d+) acertos");
                var match = r.Match(mensagem.Trim().Replace("\n", " ").Replace("\t", ""));
                if (match.Success)
                    Ganhadores = Convert.ToInt32(match.Groups["Ganhadores"].Value);
            }
        }
    }
}
