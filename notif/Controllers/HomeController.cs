using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using PusherServer;
using System.Threading.Tasks;
using System.Net;

namespace notif.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Views Dot Net | A pusher - .Net Tutorial";

            ViewData["CurrentTime"] = DateTime.Now.ToString();

            var visitors = 0;

            if(System.IO.File.Exists("visitors.txt"))
            {
                string noOfVisitors = System.IO.File.ReadAllText("visitors.txt");
                visitors = Int32.Parse(noOfVisitors);
            }
            

            ++visitors;

            var visit_text = (visitors == 1) ? " view" : " views";
            System.IO.File.WriteAllText("visitors.txt", visitors.ToString());

            ViewData["visitors"] = visitors;
            ViewData["visitors_txt"] = visit_text;


            return View();
        }

        public async Task<ActionResult> UpdateVisits()
        {
            string noOfVisitors = System.IO.File.ReadAllText("visitors.txt");
            int visits = Int32.Parse(noOfVisitors);
            var visit_text = (visits == 1) ? " view" : " views";

            var options = new PusherOptions { Encrypted = true };

            var pusher = new Pusher(
              "344451",
              "913866e45b4b1e9daf1a",
              "6342ac2220f239d94d94",
              options);

            var result = await pusher.TriggerAsync(
            "general",
            "newVisit",
                new { visits = visits.ToString(), message = visit_text } );

            return new HttpStatusCodeResult((int)HttpStatusCode.OK);
        }
    }
}
