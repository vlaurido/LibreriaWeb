using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LibreriaWeb.Data;
using LibreriaWeb.Models;
using static System.Reflection.Metadata.BlobBuilder;

namespace LibreriaWeb.Controllers
{
    public class LibrosController : Controller
    {
        private readonly AppDbContext _context;

        public LibrosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Libros
        public async Task<IActionResult> Index(string searchString, int page = 1)
        {
            int pageSize = 5;

            var libros = _context.Libros
                        .Include(l => l.Autor)
                        .AsQueryable();

            if (!String.IsNullOrEmpty(searchString))
            {
                libros = libros.Where(b => b.Titulo.Contains(searchString));
            }

            libros = libros.OrderBy(l => l.Titulo); // Orden ascendente

            int totalLibros = await libros.CountAsync();

            var libros2 = await libros
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewData["CurrentPage"] = page;
            ViewData["TotalPages"] = (int)Math.Ceiling(totalLibros / (double)pageSize);
            ViewData["CurrentFilter"] = searchString;

            return View(libros2);
        }

        // GET: Libros/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var libro = await _context.Libros
                .Include(l => l.Autor)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (libro == null)
            {
                return NotFound();
            }

            return View(libro);
        }

        // GET: Libros/Create
        public IActionResult Create()
        {
            ViewData["AutorId"] = new SelectList(_context.Autores, "Id", "Nombre");
            return View();
        }

        // POST: Libros/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Libro libro)
        {
            if (ModelState.IsValid)
            {
                if (libro.ImagenArchivo != null && libro.ImagenArchivo.Length > 0)
                {
                    var nombreArchivo = Guid.NewGuid().ToString() + Path.GetExtension(libro.ImagenArchivo.FileName);
                    var ruta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/imagenes", nombreArchivo);

                    using (var stream = new FileStream(ruta, FileMode.Create))
                    {
                        await libro.ImagenArchivo.CopyToAsync(stream);
                    }

                    libro.ImagenRuta = "/imagenes/" + nombreArchivo;
                }

                _context.Add(libro);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AutorId"] = new SelectList(_context.Autores, "Id", "Nombre", libro.AutorId);
            return View(libro);
        }

        // GET: Libros/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var libro = await _context.Libros.FindAsync(id);
            if (libro == null)
            {
                return NotFound();
            }
            ViewData["AutorId"] = new SelectList(_context.Autores, "Id", "Nombre", libro.AutorId);
            return View(libro);
        }

        // POST: Libros/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Libro libro)
        {
            if (id != libro.Id)
            {
                return NotFound();
            }

            // Recuperar libro anterior de la base de datos
            var libroExistente = await _context.Libros.AsNoTracking().FirstOrDefaultAsync(l => l.Id == id);
            if (libroExistente == null)
                return NotFound();


            if (ModelState.IsValid)
            {
                try
                {
                    if (libro.ImagenArchivo != null && libro.ImagenArchivo.Length > 0)
                    {
                        // Eliminar la imagen anterior del servidor
                        if (!string.IsNullOrEmpty(libroExistente.ImagenRuta))
                        {
                            var rutaAnterior = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", libroExistente.ImagenRuta.TrimStart('/'));
                            if (System.IO.File.Exists(rutaAnterior))
                            {
                                System.IO.File.Delete(rutaAnterior);
                            }
                        }

                        //Guardar nueva imagen
                        var nombreArchivo = Guid.NewGuid().ToString() + Path.GetExtension(libro.ImagenArchivo.FileName);
                        var ruta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/imagenes", nombreArchivo);

                        using (var stream = new FileStream(ruta, FileMode.Create))
                        {
                            await libro.ImagenArchivo.CopyToAsync(stream);
                        }

                        libro.ImagenRuta = "/imagenes/" + nombreArchivo;
                    }
                    else
                    {
                        // Recuperar la imagen anterior de la base de datos
                        libro.ImagenRuta = libroExistente.ImagenRuta;
                    }

                    _context.Update(libro);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LibroExists(libro.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["AutorId"] = new SelectList(_context.Autores, "Id", "Nombre", libro.AutorId);
            return View(libro);
        }

        // GET: Libros/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var libro = await _context.Libros
                .Include(l => l.Autor)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (libro == null)
            {
                return NotFound();
            }

            return View(libro);
        }

        // POST: Libros/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var libro = await _context.Libros.FindAsync(id);
            if (libro != null)
            {
                // Eliminar la imagen del servidor
                if (!string.IsNullOrEmpty(libro.ImagenRuta))
                {
                    // Obtener la ruta física en el servidor
                    var rutaCompleta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", libro.ImagenRuta.TrimStart('/'));

                    if (System.IO.File.Exists(rutaCompleta))
                    {
                        System.IO.File.Delete(rutaCompleta);
                    }
                }
                _context.Libros.Remove(libro);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LibroExists(int id)
        {
            return _context.Libros.Any(e => e.Id == id);
        }
    }
}
