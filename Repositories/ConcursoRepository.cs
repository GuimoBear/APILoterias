using AngleSharp.Parser.Html;
using MegaResult.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;

namespace MegaResult.Repositories
{
    public class ConcursoRepository
    {
        private static IDictionary<int, Concurso> concursos;
        private static Concurso current;
        private static readonly CookieAwareWebClient _client;
        private static readonly HtmlParser _parser;
        private static Timer atualizador;

        static ConcursoRepository()
        {
            _client = new CookieAwareWebClient(new CookieContainer());
            _client.Headers[HttpRequestHeader.UserAgent] = "Mozilla/4.0 (compatible; Synapse)";
            _client.Headers[HttpRequestHeader.KeepAlive] = "300";
            concursos = new Dictionary<int, Concurso>();
            _parser = new HtmlParser();
            Atualizar(null);
            concursos.Add(current.Numero, current);
            atualizador = new Timer(Atualizar, null, TimeSpan.FromMinutes(10), TimeSpan.FromMinutes(10));
        }

        private static void Atualizar(object state) => current = new Concurso(_parser.Parse(_client.DownloadString("http://loterias.caixa.gov.br/wps/portal/loterias/landing/megasena/")));

        private readonly CookieAwareWebClient client;
        private readonly HtmlParser parser;

        public ConcursoRepository(CookieAwareWebClient client, HtmlParser parser)
        {
            this.client = client;
            this.parser = parser;
        }

        public Concurso Current => current;

        public Concurso Consultar(int numero)
        {
            if (!concursos.ContainsKey(numero))
            {
                var document = parser.Parse(client.DownloadString("http://loterias.caixa.gov.br/wps/portal/loterias/landing/megasena/"));
                var url = document.QuerySelector("base").GetAttribute("href");
                var action = document.Body.QuerySelector("form[name=\"buscaForm\"]").GetAttribute("action");
                System.Collections.Specialized.NameValueCollection formData = new System.Collections.Specialized.NameValueCollection();
                formData["concurso"] = numero.ToString();
                byte[] responseBytes = client.UploadValues($"{url}{action}", "POST", formData);
                var concurso = new Concurso(parser.Parse(Encoding.UTF8.GetString(responseBytes)));
                concursos.Add(numero, concurso);
            }
            return concursos[numero];
        }

        public static void Clear()
        {
            atualizador.Dispose();
            _client.Dispose();
        }
    }
}
