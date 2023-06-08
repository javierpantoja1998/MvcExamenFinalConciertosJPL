using Microsoft.AspNetCore.Mvc;
using MvcExamenFinalConciertosJPL.Models;
using MvcExamenFinalConciertosJPL.Services;

namespace MvcExamenFinalConciertosJPL.Controllers
{
    public class EventosController : Controller
    {
        private ServicesEventos service;
        private ServiceStoragesS3 serviceStorages1;
        private string BucketUrl;

        public EventosController(ServicesEventos service, IConfiguration configuration, ServiceStoragesS3 serviceStorages, KeysModel model)
        {
            this.service = service;
            this.BucketUrl = model.BucketUrl;
            this.serviceStorages1 = serviceStorages;
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
        public async Task<IActionResult> Create(Evento evento, IFormFile file)
        {
            using (Stream stream = file.OpenReadStream())
            {
                await this.serviceStorages1.UploadFileAsync(file.FileName, stream);
            }
            await this.service.CreateEventoAsync(evento.Nombre, evento.Artista,evento.IdCategoria, file.FileName);
            return RedirectToAction("Index");
        }
    }
}
