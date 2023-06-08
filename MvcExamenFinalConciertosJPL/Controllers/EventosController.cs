using Microsoft.AspNetCore.Mvc;
using MvcExamenFinalConciertosJPL.Models;
using MvcExamenFinalConciertosJPL.Services;

namespace MvcExamenFinalConciertosJPL.Controllers
{
    public class EventosController : Controller
    {
        private ServicesEventos service;
        private string BucketUrl;

        public EventosController(ServicesEventos service, IConfiguration configuration)
        {
            this.service = service;
            this.BucketUrl = configuration.GetValue<string>("AWS:BucketUrl");
        }

        public async Task<IActionResult> Index()
        {
            List<Evento> eventos = await this.service.GetEventosAsync();
            ViewData["URLIMAGEN"] = this.BucketUrl;
            return View(eventos);
        }

        public async Task<IActionResult> FindEventoxCategoria(int id)
        {
            Evento evento = await this.service.FindEventoAsync(id);
            return View(evento);
        }

        public async Task<IActionResult> Categorias()
        {
            List<CategoriaEvento> categorias = await this.service.GetCategoriasAsync();
            return View(categorias);
        }

        public async Task<IActionResult> Create()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Evento evento)
        {
            await this.service.CreateEvento(evento);
            return RedirectToAction("Index");
        }
    }
}
