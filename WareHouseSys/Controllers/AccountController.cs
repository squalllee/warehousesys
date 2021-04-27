using WareHouseSys.Models;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Security;

namespace WareHouseSys.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> Login(string token)
        {
            //token = "eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiJ9.eyJpc3MiOiJ3d3cudGNtZXRyby50dzo4MDgwIiwiYXVkIjoid3d3LnRjbWV0cm8udHciLCJpYXQiOjE1NDg5OTE1MjIsImV4cCI6MTU0ODk5MTgyMiwic2lkIjoiMSIsInVzZXJuYW1lIjoiMTEwMTI4In0.lkIi79wNexICCVdUu5erqSMdnagkanAOYty_8J2JfNHe5JTkDIHksbu8E3dyabWPo0ritA4gzxxfsiWahMalzv_fhMRvM_qqXEFzj8_SyBruf2732-RMwgY1UHyvbDjTJvOZOm_KF_LMsh0S9XBWe_0M9nbLH6YWGaEpk1SiZYplQS-CsDeWx08M46J204luGo9xYc3-mTaRpCPxzD5nDleQfyqGU7qQEkQZwRZHwbG4Usg0A8gXjjD7jc5nxzuXiji5ydsNJj6YuPzj03hPEhBL126oQkmpiPKeDVW-Q0oESsXjf-wuZCOluhiuvaw6TY6v5D34zjIECGE-CMv2OQ";
            if (token != null)
            {
                //string publicAndPrivateKey = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAwVSVGU5wylmw9r2sgB97r+k8D5i7FlHeYgSezIpxzFor896a8a027rVdOJmaGExDpLK9LVzBE1KfhMMawpzY4XATW4D1EDO1MBpGz54vj37VWGImeAGJwkh1fjbJTd5UUBWfUhuCtkI45SuWwQ878PR8irVwTEgZOkVo9wF8sp56zSPLk2jLAZKYIwHJr6/Wek4Hu5qXBhFIpkDIbF9PbOv74s8Nb8zLkYdI+c2L5b1+cJ8X5hvczOZU1Opw/mc5E5e3M8mzMEozRLcK5NVYN37sxNtl8dDLOAhBjWo5CRqYB5sdQ0gm1w/hwAzNwnnppWiA73CPqtB+mSN8nIKRCwIDAQAB";
                string SSOURL = WebConfigurationManager.AppSettings["SSOURL"];
                var buffer = Encoding.UTF8.GetBytes(token);
                var byteContent = new ByteArrayContent(buffer);
                HttpClient client = new HttpClient();
                //client.BaseAddress = new Uri("http://www.tcmetro.tw:8080/eipplus/");
                client.BaseAddress = new Uri(SSOURL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("text/html"));
                HttpResponseMessage response = await client.PostAsync(
                    "systoken/verify.php", byteContent);

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    if(json.IndexOf("JWT") > -1)
                    {
                        return View();
                    }
                    else
                    {
                        JWTModel jWTModel = JsonConvert.DeserializeObject<JWTModel>(json);//反序列化
                        DateTime dateTime = DateTime.Now.AddMinutes(30.0);
                        FormsAuthentication.SetAuthCookie(jWTModel.username, false);
                        string value = FormsAuthentication.Encrypt(new FormsAuthenticationTicket(1, jWTModel.username, DateTime.Now, dateTime, true, jWTModel.username, FormsAuthentication.FormsCookiePath));
                        Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, value)
                        {
                            Expires = dateTime,
                            Domain = "www.tcmetro.tw"

                        });
                        
                        return RedirectToAction("Index", "Home");
                    }
                    
                }
                return View();
            }
            else
            {
                return View();
            }
            
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (Membership.ValidateUser(model.ID, model.Password))
            {
                DateTime dateTime = DateTime.Now;
                dateTime = ((!model.RememberMe) ? dateTime.AddMinutes(30.0) : dateTime.AddMonths(1));
                string value = FormsAuthentication.Encrypt(new FormsAuthenticationTicket(1, model.ID, DateTime.Now, dateTime, true, model.ID, FormsAuthentication.FormsCookiePath));
                Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, value)
                {
                    Expires = dateTime
                });

                if (returnUrl != null)
                    return Redirect(returnUrl);
                else
                    return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("", "無法登入，請重新輸入正確的帳號密碼");
            }
            return View(model);
        }


        [HttpGet]
        public ActionResult Logout()
        {
            string ID = HttpContext.User.Identity.Name;

            FormsAuthentication.SignOut();
            Session.Abandon();
            HttpCookie httpCookie = new HttpCookie(FormsAuthentication.FormsCookieName, string.Empty);
            DateTime now = DateTime.Now;
            httpCookie.Expires = now.AddYears(-1);
            HttpCookie cookie = httpCookie;
            base.Response.Cookies.Add(cookie);
            HttpCookie httpCookie2 = new HttpCookie("ASP.NET_SessionId", string.Empty);
            now = DateTime.Now;
            httpCookie2.Expires = now.AddYears(-1);
            HttpCookie cookie2 = httpCookie2;
            Response.Cookies.Add(cookie2);

            return RedirectToAction("Login", "Account");

        }
    }
}