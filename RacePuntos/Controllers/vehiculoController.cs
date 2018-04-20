using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RacePuntos.Datos;

namespace RacePuntos.Controllers
{
    public class vehiculoController : Controller
    {
        private RacePuntosEntities db = new RacePuntosEntities();

        // GET: vehiculo
        public async Task<ActionResult> Index()
        {
            var vehiculo = db.vehiculo.Include(v => v.detalle_vehiculos).Include(v => v.personas);
            return View(await vehiculo.ToListAsync());
        }

        // GET: vehiculo/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            vehiculo vehiculo = await db.vehiculo.FindAsync(id);
            if (vehiculo == null)
            {
                return HttpNotFound();
            }
            return View(vehiculo);
        }

        // GET: vehiculo/Create
        public ActionResult Create()
        {
            ViewBag.codigo_vehiculo = new SelectList(db.detalle_vehiculos, "codigo", "marca");
            ViewBag.documento_usuario = new SelectList(db.personas, "documento", "tipo_documento");
            return View();
        }

        // POST: vehiculo/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(string documento_usuario, string placa_vehiculo_cliente, string marca_vehiculo, string referencia_vehiculo, string cilindraje_vehiculo)
        {
            if (ModelState.IsValid)
            {
                int result = db.vehiculo.Where(c => c.documento_usuario == documento_usuario && c.placa_vehiculo_cliente == placa_vehiculo_cliente ).Count();
                if (result == 0)
                {
                    db.spInsertVehiculo(documento_usuario, placa_vehiculo_cliente, marca_vehiculo, referencia_vehiculo, cilindraje_vehiculo).ToString();
                    TempData["Mensaje"] = "0~Vehiculo Creado con exito";
                }
                else
                {
                    TempData["Mensaje"] = "1~Vehiculo ya existe, verifique";
                }

                //db.personas.Add(personas);
                //await db.SaveChangesAsync();
                Response.Redirect("/Personas/Create");
                return null;
            }
            return null;
        }

        // GET: vehiculo/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            vehiculo vehiculo = await db.vehiculo.FindAsync(id);
            if (vehiculo == null)
            {
                return HttpNotFound();
            }
            ViewBag.codigo_vehiculo = new SelectList(db.detalle_vehiculos, "codigo", "marca", vehiculo.codigo_vehiculo);
            ViewBag.documento_usuario = new SelectList(db.personas, "documento", "tipo_documento", vehiculo.documento_usuario);
            return View(vehiculo);
        }

        // POST: vehiculo/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "documento_usuario,placa_vehiculo_cliente,codigo_vehiculo,marca_vehiculo,referencia_vehiculo,cilindraje_vehiculo,id_vehiculo")] vehiculo vehiculo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(vehiculo).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.codigo_vehiculo = new SelectList(db.detalle_vehiculos, "codigo", "marca", vehiculo.codigo_vehiculo);
            ViewBag.documento_usuario = new SelectList(db.personas, "documento", "tipo_documento", vehiculo.documento_usuario);
            return View(vehiculo);
        }

        // GET: vehiculo/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            vehiculo vehiculo = await db.vehiculo.FindAsync(id);
            if (vehiculo == null)
            {
                return HttpNotFound();
            }
            return View(vehiculo);
        }

        // POST: vehiculo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            vehiculo vehiculo = await db.vehiculo.FindAsync(id);
            db.vehiculo.Remove(vehiculo);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
