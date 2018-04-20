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
using Microsoft.Reporting.WebForms;
using System.IO;

namespace RacePuntos.Controllers
{
    public class personasController : Controller
    {
        private RacePuntosEntities db = new RacePuntosEntities();

        // GET: personas
        public async Task<ActionResult> Index()
        {
            if (Session["USUARIO_LOGUEADO"] != null)
            {
                var personas = db.personas.Include(p => p.cargos);
                List<sp_RepUsuarios_Result> lstUsu = db.sp_RepUsuarios().ToList();
                Table ta = new Table();
                string TableUsuarios = UsuariosT(ta, lstUsu);
                ViewData["_TableUsuarios"] = TableUsuarios;
                return View(await personas.ToListAsync());
            }
            else
            {
                Response.Redirect("/Personas/Login");
                return null;
            }
        }

        // GET: personas/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (Session["USUARIO_LOGUEADO"] != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                personas personas = await db.personas.FindAsync(id);
                if (personas == null)
                {
                    return HttpNotFound();
                }
                return View(personas);
            }
            else
            {
                Response.Redirect("/Personas/Login");
                return null;
            }
        }

        // GET: personas/Create
        public ActionResult Create()
        {
            if (Session["USUARIO_LOGUEADO"] != null)
            {
                List<UsuariosVehiculo1_Result> lstUsuarios = db.UsuariosVehiculo1().ToList();
                List<MarcasVehiculo_Result> lstMarca = db.MarcasVehiculo().ToList();
                Table ta = new Table();
                string TableUsuarios = UsuariosVehiculo(ta, lstUsuarios);
                string TableMarca = MarcaVehiculo(ta, lstMarca);
                ViewData["_SelectUsua"] = TableUsuarios;
                ViewData["_SelectMarc"] = TableMarca;
                return View();
            }
            else
            {
                Response.Redirect("/Personas/Login");
                return null;
            }
        }

        public string UsuariosVehiculo(Table ta, List<UsuariosVehiculo1_Result> lsUsrVehiculo)
        {
            string ListUsu = "<select class='selectpicker' name='documento_usuario' id='documento_usuario' data-live-search='true' title='[SELECCIONE]'>";
            for (int i = 0; i < lsUsrVehiculo.Count; i++)
            {
                ListUsu += "<option value='" + lsUsrVehiculo[i].documento + "'>" + lsUsrVehiculo[i].Nombres + "</option>";
            }
            ListUsu += "</select>";
            return ListUsu;
        }

        public string MarcaVehiculo(Table ta, List<MarcasVehiculo_Result> lsMarca)
        {
            string ListMarc = "<select class='selectpicker' name='marca_vehiculo' id='marca_vehiculo' data-live-search='true' title='[SELECCIONE]'>";
            for (int i = 0; i < lsMarca.Count; i++)
            {
                ListMarc += "<option value='" + lsMarca[i].codigo + "'>" + lsMarca[i].marca + "</option>";
            }
            ListMarc += "</select>";
            return ListMarc;
        }

        public string UsuariosT(Table ta, List<sp_RepUsuarios_Result> lsUsr)
        {
            string ListUsr = "";
            foreach (var item in lsUsr)
            {
                ListUsr += "<tr>";
                ListUsr += "<td>" + item.documento + " </td>";
                ListUsr += "<td>" + item.nombres + " " + item.apellidos + "  </td>";
                ListUsr += " <td>" + item.marca + "  </td>";
                ListUsr += "<td>" + item.placa + "  </td>";
                ListUsr += " <td>" + item.puntos_redimidos + "  </td>";
                ListUsr += "  <td>" + item.puntos_acumulados + "  </td>";
                ListUsr += " </tr>";
            }
            return ListUsr;
        }

        // POST: personas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(string tipo_documento, string documento, string contrasena, string rol, string cargo, string id_usuario_creacion, string nombres, string apellidos, string fecha_nacimiento, string direccion, string numero_celular, string correoElectronico)
        {
            if (Session["USUARIO_LOGUEADO"] != null)
            {
                TempData["Mensaje"] = "";
                if (ModelState.IsValid)
                {
                    int usr = db.personas.Where(c => c.documento == documento).Count();
                    if (usr == 0)
                    {
                        rol = (rol == "") ? "USUARIO" : rol;
                        db.registro_persona(tipo_documento, documento, contrasena, rol, cargo, id_usuario_creacion, nombres, apellidos, fecha_nacimiento, direccion, numero_celular, correoElectronico).ToString();
                        TempData["Mensaje"] = "0~Usuario Creado con exito";
                    }
                    else
                    {
                        TempData["Mensaje"] = "1~Usuario ya existe, verifique";
                    }

                    //db.personas.Add(personas);
                    //await db.SaveChangesAsync();
                    Response.Redirect("/Personas/Create");
                    return null;
                }

                ViewBag.cargo = new SelectList(db.cargos, "id_cargo", "nombre_cargo", cargo);
                Response.Redirect("/Personas/Create");
                return null;
            }
            else
            {
                Response.Redirect("/Personas/Login");
                return null;
            }
        }

        // GET: personas/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (Session["USUARIO_LOGUEADO"] != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                personas personas = await db.personas.FindAsync(id);
                if (personas == null)
                {
                    return HttpNotFound();
                }
                ViewBag.cargo = new SelectList(db.cargos, "id_cargo", "nombre_cargo", personas.cargo);
                return View(personas);
            }
            else
            {
                Response.Redirect("/Personas/Login");
                return null;
            }
        }

        // POST: personas/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "tipo_documento,documento,contrasena,rol,cargo,id_usuario_creacion,nombres,apellidos,fecha_nacimiento,direccion,numero_celular,correoElectronico")] personas personas)
        {
            if (Session["USUARIO_LOGUEADO"] != null)
            {
                if (ModelState.IsValid)
                {
                    db.Entry(personas).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                ViewBag.cargo = new SelectList(db.cargos, "id_cargo", "nombre_cargo", personas.cargo);
                return View(personas);
            }
            else
            {
                Response.Redirect("/Personas/Login");
                return null;
            }
        }

        // GET: personas/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (Session["USUARIO_LOGUEADO"] != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                personas personas = await db.personas.FindAsync(id);
                if (personas == null)
                {
                    return HttpNotFound();
                }
                return View(personas);
            }
            else
            {
                Response.Redirect("/Personas/Login");
                return null;
            }
        }

        // POST: personas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            if (Session["USUARIO_LOGUEADO"] != null)
            {
                personas personas = await db.personas.FindAsync(id);
                db.personas.Remove(personas);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            else
            {
                Response.Redirect("/Personas/Login");
                return null;
            }
        }

        // GET: personas/servicios
        public async Task<ActionResult> Servicios()
        {
            if (Session["USUARIO_LOGUEADO"] != null)
            {
                return View();
            }
            else
            {
                Response.Redirect("/Personas/Login");
                return null;
            }
        }

        //
        // GET: /personas/Login
        [AllowAnonymous]
        public ActionResult Login()
        {
            if (Session["USUARIO_LOGUEADO"] == null)
            {
                return View();
            }
            else
            {
                Response.Redirect("/Home/Index");
                return null;
            }
        }

        //
        // POST: /personas/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string documento, string contrasena)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    Response.Redirect("Login");
                    return null;
                }
                TempData["Error"] = "";
                var ValidateLogin = db.logeo_persona(documento.ToString(), contrasena.ToString()).Count();


                if (ValidateLogin > 0)
                {
                    personas User = new personas
                    {
                        documento = documento,
                        nombres = db.personas.Where(c => c.documento == documento).First().nombres,
                        apellidos = db.personas.Where(c => c.documento == documento).First().apellidos
                    };
                    Session["USUARIO_LOGUEADO"] = User;

                    Session["DOCUMENTO"] = db.personas.Where(c => c.documento == documento).First().documento;
                    Session["NOMBRES"] = db.personas.Where(c => c.documento == documento).First().nombres;
                    Session["APELLIDOS"] = db.personas.Where(c => c.documento == documento).First().apellidos;
                    Session["ROL"] = db.personas.Where(c => c.documento == documento).First().rol;
                    Session["CARGO"] = db.personas.Where(c => c.documento == documento).First().cargo;

                    Response.Redirect("/Home/Index");
                    return null;
                }
                else
                {
                    TempData["Error"] = "Datos incorrectos, verifique.";
                    Response.Redirect("~/Personas/Login");
                    return null;
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                db.Dispose();
            }
            return null;

        }

        //
        // GET: /personas/Logout
        [AllowAnonymous]
        public ActionResult Logout()
        {
            Session["USUARIO_LOGUEADO"] = null;
            Response.Redirect("~/Personas/Login");
            return null;
        }

        public ActionResult Report(string id, string rdlc, string NameDataSet)
        {
            LocalReport lr = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/ReportViewer"), rdlc+".rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("Index");
            }
            List<sp_RepUsuarios_Result> cm = new List<sp_RepUsuarios_Result>();
            using (RacePuntosEntities dc = new RacePuntosEntities())
            {
                cm = dc.sp_RepUsuarios().ToList();
            }
            ReportDataSource rd = new ReportDataSource(NameDataSet, cm);
            lr.DataSources.Add(rd);
            string reportType = id;
            string mimeType;
            string encoding;
            string fileNameExtension;



            string deviceInfo =

            "<DeviceInfo>" +
            "  <OutputFormat>" + id + "</OutputFormat>" +
            "  <PageWidth>8.5in</PageWidth>" +
            "  <PageHeight>11in</PageHeight>" +
            "  <MarginTop>0.5in</MarginTop>" +
            "  <MarginLeft>1in</MarginLeft>" +
            "  <MarginRight>1in</MarginRight>" +
            "  <MarginBottom>0.5in</MarginBottom>" +
            "</DeviceInfo>";

            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;

            renderedBytes = lr.Render(
                reportType,
                deviceInfo,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);
            return File(renderedBytes, mimeType);

        }
    }
}
