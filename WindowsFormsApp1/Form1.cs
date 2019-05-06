using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http;
using System.Net;
using System.Text.RegularExpressions;


namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private CookieContainer _container;
        private string site_Key = "6LdkSWIUAAAAAEIjAxvA_JtVSO2fz0Rx6dNluiPm";
        public string captcha_key = "f44b6238f45d3e8cc9dad97784b62fe9";
        public string pageUrl = "https://store.paniniamerica.net";
        public Form1()
        {
            InitializeComponent();
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage responseMessage = new HttpResponseMessage();
            string strContent = "";
            try
            {
                _container = new CookieContainer();
                var handler = new HttpClientHandler()
                {
                    AllowAutoRedirect = true
                };
                handler.CookieContainer = _container;
                httpClient = new HttpClient(handler);
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8");
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Language", "en-US,en;q=0.9,ko;q=0.8");
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/70.0.3538.77 Safari/537.36");
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Upgrade-Insecure-Requests", "1");
                string strUrl = "https://store.paniniamerica.net/customer/account/login/";
                responseMessage = httpClient.GetAsync(strUrl).Result;
                responseMessage.EnsureSuccessStatusCode();
                strContent = responseMessage.Content.ReadAsStringAsync().Result;

                var responseCookies = _container.GetCookies(new Uri(strUrl)).Cast<System.Net.Cookie>();
                _container.Add(new System.Net.Cookie("sucuri_cloudproxy_uuid_5bb645a2b", "d15135cac53fc94fb68951532544c0cf") { Domain = new Uri(strUrl).Host});


                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Cache-Control", "max-age=0");
                httpClient.DefaultRequestHeaders.Referrer = new Uri(strUrl);
                responseMessage = httpClient.GetAsync(strUrl).Result;
                responseMessage.EnsureSuccessStatusCode();
                strContent = responseMessage.Content.ReadAsStringAsync().Result;

                string strPattern = "name=\"form_key\" value=\"(?<val>[^\"]*)\"";
                Match math = Regex.Match(strContent, strPattern);
                string strFromKey = math.Groups["val"].Value;

                Dictionary<string, string> dicparm = new Dictionary<string, string>();
                dicparm.Add("form_key", strFromKey);
                dicparm.Add("login[username]", "dearmitt66@yahoo.com");
                dicparm.Add("login[password]", "Panini3366");
                dicparm.Add("send", "");

                KeyValuePair<string, string>[] keypairs = new KeyValuePair<string, string>[dicparm.Keys.Count];
                for (int i = 0; i < dicparm.Keys.Count; i++)
                {
                    keypairs[i] = new KeyValuePair<string, string>(dicparm.Keys.ElementAt(i), dicparm[dicparm.Keys.ElementAt(i)]);
                }

                ServicePointManager.DefaultConnectionLimit = 2;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

                strUrl = "https://store.paniniamerica.net/customer/account/loginPost/";
                responseMessage = httpClient.PostAsync(strUrl, new FormUrlEncodedContent(keypairs)).Result;
                responseMessage.EnsureSuccessStatusCode();

                strUrl = "https://store.paniniamerica.net/customer/account/";
                responseMessage = httpClient.GetAsync(strUrl).Result;
                responseMessage.EnsureSuccessStatusCode();
                strContent = responseMessage.Content.ReadAsStringAsync().Result;

                strUrl = "https://store.paniniamerica.net/cards/paninieternal/2018/tristan-thompson-2018-panini-eternal-2017-nba-finals-memorabilia-card-pe-f9-base-1-25-jersey-june-1.html";
                responseMessage = httpClient.GetAsync(strUrl).Result;
                responseMessage.EnsureSuccessStatusCode();
                strContent = responseMessage.Content.ReadAsStringAsync().Result;

                strPattern = "action=\"(?<val>[^\"]*)\" method=\"post\" id=\"product_addtocart_form\"";
                math = Regex.Match(strContent, strPattern);

                strUrl = math.Groups["val"].Value;

                string captcha_response = "";

                int nCount = 0;
                while (nCount < 5)
                {
                    captcha_response = trySolvingCaptcha();
                    if (captcha_response == "")
                    {
                        nCount = nCount + 1;
                    }
                    else
                    {
                        break;
                    }
                }

                if (captcha_response == "")
                    return;

                dicparm.Clear();
                dicparm.Add("qty", "1");
                dicparm.Add("g-recaptcha-response", captcha_response);

                keypairs = null;
                keypairs = new KeyValuePair<string, string>[dicparm.Keys.Count];
                for (int i = 0; i < dicparm.Keys.Count; i++)
                {
                    keypairs[i] = new KeyValuePair<string, string>(dicparm.Keys.ElementAt(i), dicparm[dicparm.Keys.ElementAt(i)]);
                }
                
                responseMessage = httpClient.PostAsync(strUrl, new FormUrlEncodedContent(keypairs)).Result;
                responseMessage.EnsureSuccessStatusCode();

                strUrl = "https://store.paniniamerica.net/checkout/cart/";
                responseMessage = httpClient.GetAsync(strUrl).Result;
                responseMessage.EnsureSuccessStatusCode();
                strContent = responseMessage.Content.ReadAsStringAsync().Result;

                strUrl = "https://store.paniniamerica.net/checkout/onepage/";
                responseMessage = httpClient.GetAsync(strUrl).Result;
                responseMessage.EnsureSuccessStatusCode();
                strContent = responseMessage.Content.ReadAsStringAsync().Result;

                strPattern = "name=\"billing[address_id]\" value=\"(?<val>[^\"]*)\"";
                math = Regex.Match(strContent, strPattern);

                string billingAddress_id = math.Groups["val"].Value;

                dicparm.Clear();
                dicparm.Add("billing_address_id", "");
                dicparm.Add("billing[address_id]", billingAddress_id);
                dicparm.Add("billing[firstname]", "Robert");
                dicparm.Add("billing[lastname]", "DeArmitt");
                dicparm.Add("billing[company]", "");
                dicparm.Add("billing[street][]", "5416 60th Avenue Ct W");
                dicparm.Add("billing[street][]", "");
                dicparm.Add("billing[city]", "University Place");
                dicparm.Add("billing[region_id]", "62");
                dicparm.Add("billing[region]", "");
                dicparm.Add("billing[postcode]", "98467");
                dicparm.Add("billing[country_id]", "US");
                dicparm.Add("billing[telephone]", "2539617951");
                dicparm.Add("billing[use_for_shipping]", "1");

                strUrl = "https://store.paniniamerica.net/checkout/onepage/saveBilling/";
                keypairs = null;
                keypairs = new KeyValuePair<string, string>[dicparm.Keys.Count];
                for (int i = 0; i < dicparm.Keys.Count; i++)
                {
                    keypairs[i] = new KeyValuePair<string, string>(dicparm.Keys.ElementAt(i), dicparm[dicparm.Keys.ElementAt(i)]);
                }

                responseMessage = httpClient.PostAsync(strUrl, new FormUrlEncodedContent(keypairs)).Result;
                responseMessage.EnsureSuccessStatusCode();


            }
            catch(Exception ex)
            {

            }
        }

        private string trySolvingCaptcha()
        {
            string verifyCode = "";
            try
            {
                string sendUrl = string.Format("http://2captcha.com/in.php?key={0}&method=userrecaptcha&googlekey={1}&pageurl={2}", captcha_key, site_Key, pageUrl);
                HttpClient httpClient = new HttpClient();
                HttpResponseMessage responseMessageMain = httpClient.GetAsync(sendUrl).Result;
                responseMessageMain.EnsureSuccessStatusCode();


                string sendUrlString = responseMessageMain.Content.ReadAsStringAsync().Result;
                if (string.IsNullOrEmpty(sendUrlString))
                    return verifyCode;


                if (!sendUrlString.Contains("OK|"))
                    return verifyCode;


                string captchaId = sendUrlString.Replace("OK|", string.Empty);
                if (string.IsNullOrEmpty(captchaId))
                    return verifyCode;


                string verifyUrl = string.Format("http://2captcha.com/res.php?key={0}&action=get&id={1}", captcha_key, captchaId);


                int requestCount = 0;
                while (requestCount < 20)
                {
                    System.Threading.Thread.Sleep(5000);
                    requestCount++;


                    HttpResponseMessage responseMessageVerify = httpClient.GetAsync(verifyUrl).Result;
                    responseMessageVerify.EnsureSuccessStatusCode();


                    string verifyUrlString = responseMessageVerify.Content.ReadAsStringAsync().Result;
                    if (string.IsNullOrEmpty(verifyUrlString))
                        continue;


                    if (!verifyUrlString.Contains("OK|"))
                        continue;


                    verifyCode = verifyUrlString.Replace("OK|", string.Empty);
                    break;
                }


                return verifyCode;
            }
            catch (Exception e)
            {
                return verifyCode;
            }
        }
    }
}
