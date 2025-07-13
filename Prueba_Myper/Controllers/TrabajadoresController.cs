using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Prueba_Myper.Models;

namespace Prueba_Myper.Controllers
{
    public class TrabajadoresController : Controller
    {
        //public IActionResult Index()
        //{
        //    return View();
        //}
        private readonly ApplicationDbContext _context;

        public TrabajadoresController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> Index(string sexoFiltro)
        {
            //var trabajadores1 = await _context.Trabajadores.ToListAsync();

            var trabajadores = await _context.TrabajadorListado
                .FromSqlRaw("EXEC sp_ListarTrabajadores")
                .ToListAsync();

            if (!string.IsNullOrEmpty(sexoFiltro))
            {
                trabajadores = trabajadores.Where(x => x.Sexo == sexoFiltro).ToList();
            }

            ViewBag.Departamentos = await _context.Departamentos.ToListAsync();
            ViewBag.Provincias = await _context.Provincias.ToListAsync();
            ViewBag.Distritos = await _context.Distritos.ToListAsync();

            ViewBag.SexoFiltro = sexoFiltro;
            return View(trabajadores);
        }

        [HttpGet]
        public async Task<IActionResult> Crear()
        {
            ViewBag.Departamentos = await _context.Departamentos.ToListAsync();
            return PartialView("_FormModal", new Trabajador());
        }

        [HttpPost]
        public async Task<IActionResult> Crear(Trabajador model)
        {
            if (ModelState.IsValid)
            {
                _context.Trabajadores.Add(model);
                await _context.SaveChangesAsync();
                return Json(new { success = true });
            }
            ViewBag.Departamentos = await _context.Departamentos.ToListAsync();
            ViewBag.Provincias = model.IdDepartamento.HasValue ? await _context.Provincias.Where(p => p.IdDepartamento == model.IdDepartamento).ToListAsync() : new List<Provincia>();
            ViewBag.Distritos = model.IdProvincia.HasValue ? await _context.Distritos.Where(d => d.IdProvincia == model.IdProvincia).ToListAsync() : new List<Distrito>();
            return PartialView("_FormModal", model);
        }

        //public async Task<IActionResult> Index(string sexoFiltro)
        //{
        //    //var trabajadores1 = await _context.Trabajadores.ToListAsync();

        //    var trabajadores = await _context.TrabajadorListado
        //        .FromSqlRaw("EXEC sp_ListarTrabajadores")
        //        .ToListAsync();

        //    if (!string.IsNullOrEmpty(sexoFiltro))
        //    {
        //        trabajadores = trabajadores.Where(x => x.Sexo == sexoFiltro).ToList();
        //    }

        //    ViewBag.Departamentos = await _context.Departamentos.ToListAsync();
        //    ViewBag.Provincias = await _context.Provincias.ToListAsync();
        //    ViewBag.Distritos = await _context.Distritos.ToListAsync();

        //    ViewBag.SexoFiltro = sexoFiltro;
        //    return View(trabajadores);
        //}

        //[HttpGet]
        //public async Task<IActionResult> Crear()
        //{
        //    ViewBag.Departamentos = await _context.Departamentos.ToListAsync();
        //    return PartialView("_FormModal", new Trabajador());
        //}

        //[HttpPost]
        //public async Task<IActionResult> Crear(Trabajador model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Trabajadores.Add(model);
        //        await _context.SaveChangesAsync();
        //        return Json(new { success = true });
        //    }
        //    ViewBag.Departamentos = await _context.Departamentos.ToListAsync();
        //    ViewBag.Provincias = model.IdDepartamento.HasValue ? await _context.Provincias.Where(p => p.IdDepartamento == model.IdDepartamento).ToListAsync() : new List<Provincia>();
        //    ViewBag.Distritos = model.IdProvincia.HasValue ? await _context.Distritos.Where(d => d.IdProvincia == model.IdProvincia).ToListAsync() : new List<Distrito>();
        //    return PartialView("_FormModal", model);
        //}

        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {

            var trabajador = await _context.Trabajadores.FindAsync(id);
            ViewBag.Departamentos = await _context.Departamentos.ToListAsync();
            ViewBag.Provincias = await _context.Provincias.Where(p => p.IdDepartamento == trabajador.IdDepartamento).ToListAsync();
            ViewBag.Distritos = await _context.Distritos.Where(d => d.IdProvincia == trabajador.IdProvincia).ToListAsync();
            return PartialView("_FormModal", trabajador);

        }

        [HttpPost]
        public async Task<IActionResult> Editar(Trabajador model)
        {
            //if (ModelState.IsValid)
            //{
                _context.Update(model);
                await _context.SaveChangesAsync();
                return Json(new { success = true });
            //}
            ViewBag.Departamentos = await _context.Departamentos.ToListAsync();
            ViewBag.Provincias = model.IdDepartamento.HasValue ? await _context.Provincias.Where(p => p.IdDepartamento == model.IdDepartamento).ToListAsync() : new List<Provincia>();
            ViewBag.Distritos = model.IdProvincia.HasValue ? await _context.Distritos.Where(d => d.IdProvincia == model.IdProvincia).ToListAsync() : new List<Distrito>();
            //return PartialView("_FormModal", model);
            var trabajadores = _context.TrabajadorListado
                .FromSqlRaw("EXEC sp_ListarTrabajadores")
                .ToListAsync();
            //return PartialView("_TablaParcial", trabajadores);
            return RedirectToAction("Index", trabajadores);
        }

        [HttpGet]
        public async Task<IActionResult> Eliminar(int id)
        {
            var trabajador = await _context.Trabajadores.FindAsync(id);
            _context.Trabajadores.Remove(trabajador);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", trabajador);
            //return PartialView("_TablaParcial", trabajador);
        }

        [HttpGet]
        public async Task<IActionResult> ProvinciasPorDepartamento(int idDepartamento)
        {
            var provincias = await _context.Provincias.Where(p => p.IdDepartamento == idDepartamento).ToListAsync();
            return Json(provincias);
        }

        [HttpGet]
        public async Task<IActionResult> DistritosPorProvincia(int idProvincia)
        {
            var distritos = await _context.Distritos.Where(d => d.IdProvincia == idProvincia).ToListAsync();
            return Json(distritos);
        }

        public IActionResult TablaParcial()
        {
            var trabajadores = _context.TrabajadorListado
                .FromSqlRaw("EXEC sp_ListarTrabajadores")
                .ToList(); 

            return PartialView("_TablaParcial", trabajadores);
        }
    }
}
