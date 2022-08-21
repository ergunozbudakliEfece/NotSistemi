using NotSistemi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;

namespace NotSistemi.Controllers
{
    public class HomeController : Controller
    {
        [Authorize]
        public ActionResult Index()
        {
            if (Request.Cookies["UserId"] == null)
            {
                FormsAuthentication.SignOut();
                return RedirectToAction("Login");
            }
            ViewBag.UserId = Request.Cookies["UserId"].Value;
            ViewBag.Name = Request.Cookies["Name"].Value;
            ViewBag.Role = Request.Cookies["Role"].Value;
            ViewBag.Tickets = GetTickets();
            return View();
        }
        public List<Ticket> GetTickets()
        {


            var apiUrl = "http://192.168.1.107:83/api/ticket";

            //Connect API
            Uri url = new Uri(apiUrl);
            WebClient client = new WebClient();
            client.Encoding = System.Text.Encoding.UTF8;

            string json = client.DownloadString(url);
            //END

            //JSON Parse START
            JavaScriptSerializer ser = new JavaScriptSerializer();
            List<Ticket> jsonList = ser.Deserialize<List<Ticket>>(json);

            //END

            return jsonList;
        }
        [HttpPost]
        public async Task<ActionResult> CreateTicket(Ticket ticket)
        {


            Ticket tic = new Ticket();
            tic.TICKET_ID = 0;
            tic.USER_NAME = Request.Cookies["Name"].Value;
            tic.DATE = DateTime.Now.ToString("d");
            tic.ISSUE = ticket.ISSUE;
            tic.STATUS = false;
            
            var apiUrl = "http://192.168.1.107:83/api/ticket";


            var httpClient = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, apiUrl)
            {
                Content = new StringContent(new JavaScriptSerializer().Serialize(tic), Encoding.UTF8, "application/json")
            };
            var l = new StringContent(new JavaScriptSerializer().Serialize(tic), Encoding.UTF8, "application/json");
            var response = await httpClient.SendAsync(request);



            return RedirectToAction("Index");

        }
        [HttpPost]
        public async Task<ActionResult> UpdateTicket(Ticket ticket)
        {


            Ticket tic = new Ticket();
            tic.TICKET_ID = ticket.TICKET_ID;
            tic.USER_NAME = Request.Cookies["Name"].Value;
            tic.DATE = DateTime.Now.ToString("d");
            tic.ISSUE = ticket.ISSUE;
            tic.STATUS = ticket.STATUS;
            tic.SOLUTION = ticket.SOLUTION;

            var apiUrl = "http://192.168.1.107:83/api/ticket/"+ticket.TICKET_ID;


            var httpClient = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Put, apiUrl)
            {
                Content = new StringContent(new JavaScriptSerializer().Serialize(tic), Encoding.UTF8, "application/json")
            };
            var l = new StringContent(new JavaScriptSerializer().Serialize(tic), Encoding.UTF8, "application/json");
            var response = await httpClient.SendAsync(request);



            return RedirectToAction("Index");

        }


    }
}