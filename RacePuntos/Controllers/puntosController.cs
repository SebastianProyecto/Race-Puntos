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
using System.Web.UI.WebControls;

namespace RacePuntos.Controllers {
	public class puntosController : Controller {
		private RacePuntosEntities db = new RacePuntosEntities();

		// GET: puntos
		public async Task<ActionResult> Index() {
            List<sp_RepHistAdqPuntos_Result> lstUsu = db.sp_RepHistAdqPuntos().ToList();
            Table ta = new Table();
            string TableHistorial1 = TableHistorial1T(ta, lstUsu);
            ViewData["_TableHistorial1"] = TableHistorial1;

            return View();
		}

        public string TableHistorial1T(Table ta, List<sp_RepHistAdqPuntos_Result> lsHistory1)
        {
            string ListUsr = "";
            foreach (var item in lsHistory1)
            {
                ListUsr += "<tr>";
                ListUsr += "<td>" + item.documento + " </td>";
                ListUsr += "<td>" + item.nombres + " " + item.apellidos + "  </td>";
                ListUsr += "  <td>" + item.puntos_acumulados + "  </td>";
                ListUsr += " </tr>";
            }
            return ListUsr;
        }


        // GET: puntos
        public async Task<ActionResult> Index2() {
			var puntos = db.puntos.Include(p => p.personas);
			return View(await puntos.ToListAsync());
		}

		// GET: puntos/Details/5
		public async Task<ActionResult> Details(int? id) {
			if(id == null) {
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			puntos puntos = await db.puntos.FindAsync(id);
			if(puntos == null) {
				return HttpNotFound();
			}
			return View(puntos);
		}

		// GET: puntos/Create
		public ActionResult Create() {
			ViewBag.id_usuario_puntos = new SelectList(db.personas, "documento", "tipo_documento");
			return View();
		}

		// POST: puntos/Create
		// Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
		// más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Create([Bind(Include = "id_puntos_actuales,id_usuario_puntos,id_usuario_plataforma,puntos_acumulados")] puntos puntos) {
			if(ModelState.IsValid) {
				db.puntos.Add(puntos);
				await db.SaveChangesAsync();
				return RedirectToAction("Index");
			}

			ViewBag.id_usuario_puntos = new SelectList(db.personas, "documento", "tipo_documento", puntos.id_usuario_puntos);
			return View(puntos);
		}

		// GET: puntos/Edit/5
		public async Task<ActionResult> Edit(int? id) {
			if(id == null) {
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			puntos puntos = await db.puntos.FindAsync(id);
			if(puntos == null) {
				return HttpNotFound();
			}
			ViewBag.id_usuario_puntos = new SelectList(db.personas, "documento", "tipo_documento", puntos.id_usuario_puntos);
			return View(puntos);
		}

		// POST: puntos/Edit/5
		// Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
		// más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Edit([Bind(Include = "id_puntos_actuales,id_usuario_puntos,id_usuario_plataforma,puntos_acumulados")] puntos puntos) {
			if(ModelState.IsValid) {
				db.Entry(puntos).State = EntityState.Modified;
				await db.SaveChangesAsync();
				return RedirectToAction("Index");
			}
			ViewBag.id_usuario_puntos = new SelectList(db.personas, "documento", "tipo_documento", puntos.id_usuario_puntos);
			return View(puntos);
		}

		// GET: puntos/Delete/5
		public async Task<ActionResult> Delete(int? id) {
			if(id == null) {
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			puntos puntos = await db.puntos.FindAsync(id);
			if(puntos == null) {
				return HttpNotFound();
			}
			return View(puntos);
		}

		// POST: puntos/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> DeleteConfirmed(int id) {
			puntos puntos = await db.puntos.FindAsync(id);
			db.puntos.Remove(puntos);
			await db.SaveChangesAsync();
			return RedirectToAction("Index");
		}

		protected override void Dispose(bool disposing) {
			if(disposing) {
				db.Dispose();
			}
			base.Dispose(disposing);
		}
	}
}
