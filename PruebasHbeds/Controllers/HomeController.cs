using Microsoft.Ajax.Utilities;
using Newtonsoft.Json.Linq;
using PruebasHbeds.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace PruebasHbeds.Controllers
{
    public class HomeController : Controller
    {
        public async Task<ActionResult> Index()
        {
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            StatusViewModel status = null;
            string url = "https://api.test.hotelbeds.com/hotel-api/1.0/status";
            string key = "sfc88fnezwsnvhpa68r5c68q";
            string secretKey = "kKbAmBtZQ9";
            var utctime = (DateTime.Now.ToUniversalTime() - new DateTime(1970, 1, 1)).TotalSeconds;
            var assemble = key + secretKey + utctime;
            using (SHA256 sha256 = SHA256.Create())
            {
                long ts = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds / 1000;
                var computedHash = sha256.ComputeHash(Encoding.UTF8.GetBytes(key + secretKey + ts));
                string xsignature = BitConverter.ToString(computedHash).Replace("-", "");

                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("Accept", "application/json");
                    client.DefaultRequestHeaders.Add("Api-Key", key);
                    client.DefaultRequestHeaders.Add("X-Signature", xsignature);
                    var responseTask = client.GetAsync(url);
                    responseTask.Wait();
                    if (responseTask.Result.IsSuccessStatusCode)
                    {
                        var response = await responseTask.Result.Content.ReadAsStringAsync();
                        var stat = JObject.Parse(response);
                        status = new StatusViewModel();
                        status.status = (string)stat["status"];
                        status.auditData = (string)stat["auditData"]["timestamp"];
                    }
                }
            }
            return View(status);
        }
        public async Task<ActionResult> HotelList()
        {
            List<HotelViewModel> model = new List<HotelViewModel>();
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            string url = "https://api.test.hotelbeds.com/hotel-content-api/1.0/hotels";
            string key = "sfc88fnezwsnvhpa68r5c68q";
            string secretKey = "kKbAmBtZQ9";
            var utctime = (DateTime.Now.ToUniversalTime() - new DateTime(1970, 1, 1)).TotalSeconds;
            var assemble = key + secretKey + utctime;
            using (SHA256 sha256 = SHA256.Create())
            {
                long ts = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds / 1000;
                var computedHash = sha256.ComputeHash(Encoding.UTF8.GetBytes(key + secretKey + ts));
                string xsignature = BitConverter.ToString(computedHash).Replace("-", "");

                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("Accept", "application/json");
                    client.DefaultRequestHeaders.Add("Api-Key", key);
                    client.DefaultRequestHeaders.Add("X-Signature", xsignature);
                    var builder = new UriBuilder(url);
                    builder.Port = -1;
                    var query = HttpUtility.ParseQueryString(builder.Query);
                    query["fields"] = "all";
                    builder.Query = query.ToString();
                    var responseTask = await client.GetAsync(builder.ToString());
                    if (responseTask.IsSuccessStatusCode)
                    {
                        var response = await responseTask.Content.ReadAsStringAsync();
                        var stat = JObject.Parse(response);
                        var hotels = stat["hotels"];
                        foreach (var obj in hotels)
                        {
                            HotelViewModel hotel = new HotelViewModel()
                            {
                                Code = (int)obj["code"],
                                Name = (string)obj["name"]["content"],
                                CountryCode = (string)obj["countryCode"],
                                Description = (string)obj["description"]["content"],
                                Address = (string)obj["description"]["content"],
                                Email = (string)obj["email"],
                                Web = (string)obj["web"]
                            };
                            model.Add(hotel);
                        }
                    }
                }
            }
            return View(model);
        }
        public async Task<ActionResult> Hotel(int Code)
        {
            HotelViewModel model = null;
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            string url = "https://api.test.hotelbeds.com/hotel-content-api/1.0/hotels/" + Code;
            string key = "sfc88fnezwsnvhpa68r5c68q";
            string secretKey = "kKbAmBtZQ9";
            var utctime = (DateTime.Now.ToUniversalTime() - new DateTime(1970, 1, 1)).TotalSeconds;
            var assemble = key + secretKey + utctime;
            using (SHA256 sha256 = SHA256.Create())
            {
                long ts = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds / 1000;
                var computedHash = sha256.ComputeHash(Encoding.UTF8.GetBytes(key + secretKey + ts));
                string xsignature = BitConverter.ToString(computedHash).Replace("-", "");

                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("Accept", "application/json");
                    client.DefaultRequestHeaders.Add("Api-Key", key);
                    client.DefaultRequestHeaders.Add("X-Signature", xsignature);
                    var builder = new UriBuilder(url);
                    builder.Port = -1;
                    var query = HttpUtility.ParseQueryString(builder.Query);
                    query["fields"] = "all";
                    builder.Query = query.ToString();
                    var responseTask = await client.GetAsync(builder.ToString());
                    if (responseTask.IsSuccessStatusCode)
                    {
                        var response = await responseTask.Content.ReadAsStringAsync();
                        var stat = JObject.Parse(response);
                        var obj = stat["hotel"];
                        model = new HotelViewModel()
                        {
                            Code = (int)obj["code"],
                            Name = (string)obj["name"]["content"],
                            CountryCode = (string)obj["country"]["code"],
                            Description = (string)obj["description"]["content"],
                            Address = (string)obj["address"]["content"],
                            Email = (string)obj["email"],
                            Web = (string)obj["web"]
                        };
                    }
                }
            }
            return View(model);
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}