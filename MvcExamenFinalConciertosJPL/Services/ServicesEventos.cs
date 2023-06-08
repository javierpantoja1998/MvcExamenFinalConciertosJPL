using MvcExamenFinalConciertosJPL.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace MvcExamenFinalConciertosJPL.Services
{
    public class ServicesEventos
    {

        private MediaTypeWithQualityHeaderValue Header;
        private string UrlApi;

        public ServicesEventos(IConfiguration configuration)
        {
            this.Header =
                new MediaTypeWithQualityHeaderValue("application/json");
            this.UrlApi =
                configuration.GetValue<string>("ApiUrls:ApiConciertos");
        }

        private async Task<T> CallApiAsync<T>(string request)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                string url = this.UrlApi + request;
                HttpResponseMessage response =
                    await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    T data = await response.Content.ReadAsAsync<T>();
                    return data;
                }
                else
                {
                    return default(T);
                }
            }
        }

        public async Task<List<Evento>> GetEventosAsync()
        {
            string request = "api/Eventos/GetEventosList";
            List<Evento> eventos = await this.CallApiAsync<List<Evento>>(request);
            return eventos;
        }

        public async Task<List<CategoriaEvento>> GetCategoriasAsync()
        {
            string request = "api/Eventos/GetCategorias";
            List<CategoriaEvento> categorias = await this.CallApiAsync<List<CategoriaEvento>>(request);
            return categorias;
        }

        public async Task<Evento> FindEventoAsync(int id)
        {
            string request = "api/Eventos/" + id;
            Evento evento = await this.CallApiAsync<Evento>(request);
            return evento;
        }

        public async Task CreateEventoAsync(string nombre, string artista, int idcategoria, string imagen)
        {
            using (HttpClient client = new HttpClient())
            {
                string request = "api/Eventos";
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);

                Evento evento = new Evento
                {
                    Nombre = nombre,
                    Artista = artista,
                    IdCategoria = idcategoria,
                    Imagen = imagen
                };
                string jsonEvento = JsonConvert.SerializeObject(evento);
                StringContent content =
                    new StringContent(jsonEvento, Encoding.UTF8, "application/json");
                HttpResponseMessage response =
                    await client.PostAsync(this.UrlApi + request, content);
            }
        }
    }
}
