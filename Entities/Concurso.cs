using AngleSharp.Dom;
using AngleSharp.Dom.Html;
using Newtonsoft.Json;
using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace MegaResult.Entities
{
    public class Concurso
    {
        public int Numero { get; protected set; }
        public bool Acumulou { get; protected set; }
        public DateTime Data { get; protected set; }
        public DateTime Proximo { get; protected set; }
        public double Valor { get; protected set; }
        public string Local { get; protected set; }
        public int[] Resultado { get; protected set; }
        public Sena Sena { get; protected set; }
        public Premiacao Quina { get; protected set; }
        public Premiacao Quadra { get; protected set; }

        [JsonConstructor]
        protected Concurso() { }

        public Concurso(IHtmlDocument document)
        {
            var strConcurso = document.Body.QuerySelector("#resultados>div.content-section.section-text.with-box.no-margin-bottom>div>h2>span").TextContent.Trim();
            Numero = numeroFromString(strConcurso);
            Acumulou = acumulouFromElement(document.Body.QuerySelector("div.resultado-loteria h3.epsilon"));
            Data = dataFromString(strConcurso);
            Proximo = proximoFromElement(document.Body.QuerySelector("div.resultado-loteria div.next-prize>p:nth-child(1)"));
            Valor = valorFromElement(document.Body.QuerySelector("div.resultado-loteria div.next-prize>p.value"));
            Local = document.Body.QuerySelector("div.resultado-loteria p.description").TextContent
                .Replace("\n", " ")
                .Replace("\t", "")
                .Replace("Sorteio realizado no", "")
                .Replace("Sorteio realizado em", "")
                .Trim();
            Resultado = document.Body.QuerySelector("ul.numbers.mega-sena").ChildNodes.Where(li => !string.IsNullOrWhiteSpace(li.TextContent.Trim())).Select(li => Convert.ToInt32(li.TextContent.Trim())).ToArray();
            Sena = senaFromDocument(document);
            Quina = premiacaoFromElement(document.Body.QuerySelector("#resultados>div.content-section.section-text.with-box.column-right.no-margin-top>div.related-box.gray-text.no-margin p.description:nth-child(3)"));
            Quadra = premiacaoFromElement(document.Body.QuerySelector("#resultados>div.content-section.section-text.with-box.column-right.no-margin-top>div.related-box.gray-text.no-margin p.description:nth-child(4)"));
        }
        private int numeroFromString(string dados)
        {
            Regex r = new Regex(@"Concurso (?<Numero>\d+) \((\d+\/\d+\/\d+)\)");
            var match = r.Match(dados);
            if (match.Success)
                return Convert.ToInt32(match.Groups["Numero"].Value);
            return 0;
        }
        private DateTime dataFromString(string dados)
        {
            Regex r = new Regex(@"Concurso (\d+) \((?<Data>\d+\/\d+\/\d+)\)");
            var match = r.Match(dados);
            if (match.Success)
                return DateTime.Parse(match.Groups["Data"].Value, new CultureInfo("pt-BR"));
            return default(DateTime);
        }
        private bool acumulouFromElement(IElement element)
        {
            if (ReferenceEquals(element, null)) return false;
            if (string.IsNullOrWhiteSpace(element.TextContent)) return false;
            return element.TextContent.ToLower().Contains("acumulou");
        }
        private DateTime proximoFromElement(IElement element)
        {
            if (ReferenceEquals(element, null)) return default(DateTime);
            if (string.IsNullOrWhiteSpace(element.TextContent)) return default(DateTime);
            Regex r = new Regex(@"(?<Data>\d+\/\d+\/\d+)");
            var match = r.Match(element.TextContent.Trim());
            if (match.Success)
                return DateTime.Parse(match.Groups["Data"].Value, new CultureInfo("pt-BR"));
            return default(DateTime);
        }
        private double valorFromElement(IElement element)
        {
            if (ReferenceEquals(element, null)) return default(double);
            if (string.IsNullOrWhiteSpace(element.TextContent)) return default(double);
            Regex r = new Regex(@"R\$ (?<Valor>[\d\.\,]+)");
            var match = r.Match(element.TextContent.Trim());
            if (!match.Success) return default(double);
            return Convert.ToDouble(match.Groups["Valor"].Value.Replace(".", ""));
        }
        private Sena senaFromDocument(IHtmlDocument document)
        {
            var premiacao = premiacaoFromElement(document.Body.QuerySelector("#resultados>div.content-section.section-text.with-box.column-right.no-margin-top>div.related-box.gray-text.no-margin p.description:nth-child(2)"));
            if (!ReferenceEquals(premiacao, null))
            {
                var ganhadores = document.Body.QuerySelectorAll("#resultados>div.content-section.section-text.with-box.column-right.no-margin-top>div.related-box.gray-text.no-margin p.description:nth-child(n+5)")
                                              .Select(n => new PremiacaoPorLocal(n.QuerySelector("strong").TextContent.Replace("\n", "").Replace("\t", ""), n.TextContent.Replace("\n", "").Replace("\t", "")))
                                              .ToList();
                if (ganhadores.Count > 0)
                    return new Sena(ganhadores, premiacao.Ganhadores, premiacao.Valor);
            }
            return default(Sena);
        }
        private Premiacao premiacaoFromElement(IElement element)
        {
            if (ReferenceEquals(element, null) || string.IsNullOrWhiteSpace(element.TextContent)) return default(Premiacao);
            Regex r = new Regex(@"(?<Ganhadores>\d+) aposta(s|) ganhadora(s|), R\$ (?<Valor>[\d\.\,]+)");
            var match = r.Match(element.TextContent.Trim());
            if (!match.Success) return default(Premiacao);
            var ganhadores = Convert.ToInt32(match.Groups["Ganhadores"].Value);
            var strValor = match.Groups["Valor"].Value.Replace(".", "");
            var valor = Convert.ToDouble(strValor);
            return new Premiacao(ganhadores, valor);
        }
    }
}
