using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Net.Mail;

namespace RacePuntos.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
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

        public ActionResult QuienesSomos()
        {
            if (Session["USUARIO_LOGUEADO"] != null)
            {
                ViewBag.Message = "Your application description page.";
                return View();
            }
            else
            {
                Response.Redirect("/Personas/Login");
                return null;
            }
        }

        public ActionResult Puntos()
        {
            if (Session["USUARIO_LOGUEADO"] != null)
            {
                ViewBag.Message = "Your application description page.";
                return View();
            }
            else
            {
                Response.Redirect("/Personas/Login");
                return null;
            }
        }

        public ActionResult Gestion()
        {
            if (Session["USUARIO_LOGUEADO"] != null)
            {
                ViewBag.Message = "Your application description page.";
                return View();
            }
            else
            {
                Response.Redirect("/Personas/Login");
                return null;
            }
        }

        public ActionResult Ayuda()
        {
            if (Session["USUARIO_LOGUEADO"] != null)
            {
                ViewBag.Message = "Your application description page.";
                return View();
            }
            else
            {
                Response.Redirect("/Personas/Login");
                return null;
            }
        }

        public ActionResult Contact()
        {
            if (Session["USUARIO_LOGUEADO"] != null)
            {
                ViewBag.Message = "Your application description page.";
                return View();
            }
            else
            {
                Response.Redirect("/Personas/Login");
                return null;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Email(string nombre, string correo, string inquietud)
        {
            var fromAddress = new MailAddress(correo, nombre);
            var toAddress = new MailAddress("RacePuntos@gmail.com", "RacePuntos");
            string fromPassword = "Proyecto2018";
            string subject = "Contactenos - " + nombre;
            string body = inquietud;

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("RacePuntos@gmail.com", fromPassword)
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })
            {
                smtp.Send(message);
            }

            Response.Redirect("Index");
            return null;
        }
    }
}