using System;
using System.Net.Http;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Security.Cryptography;

namespace BoldBI.Winforms
{
    public partial class Form1 : Form
    {
        public string Url { get; set; }
        public Form1()
        {
            this.Size = Screen.PrimaryScreen.WorkingArea.Size;
            this.WindowState = FormWindowState.Maximized;

            GetEmbedDetails();
            InitializeComponent();
        }

        public void GetEmbedDetails()
        {
            decimal time = (decimal)Math.Round((DateTime.Now.ToUniversalTime() - new DateTime(1970, 1, 1)).TotalMilliseconds / 1000);
            var siteId = string.IsNullOrEmpty(EmbedConfigProvider.Current.SiteIdentifier) ? "" : EmbedConfigProvider.Current.SiteIdentifier;
            var dashboardServerApiUrl = EmbedConfigProvider.Current.ServerUrl + "api/" + siteId;

            var embedQuerString = "embed_nonce=" + Guid.NewGuid() +
            "&embed_dashboard_id=" + EmbedConfigProvider.Current.DashboardId +
            "&embed_timestamp=" + Math.Round(time) +
            "&embed_expirationtime=" + (int.TryParse(EmbedConfigProvider.Current.ExpirationTime, out var expVal) ? expVal : 100000);
            embedQuerString += "&embed_user_email=" + EmbedConfigProvider.Current.UserEmail;
            //To set embed_server_timestamp to overcome the EmbedCodeValidation failing while different timezone using at client application.
            double timeStamp = (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
            embedQuerString += "&embed_server_timestamp=" + timeStamp;
            var embedDetailsUrl = "/embed/authorize?" + embedQuerString + "&embed_signature=" + GetSignatureUrl(embedQuerString);

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(dashboardServerApiUrl);
                client.DefaultRequestHeaders.Accept.Clear();

                var result = client.GetAsync(dashboardServerApiUrl + embedDetailsUrl).Result;
                string resultContent = result.Content.ReadAsStringAsync().Result;
                //webBrowser1.ObjectForScripting = this;
                var htmlString = new StringBuilder();
                var serverUrlForJs = EmbedConfigProvider.Current.ServerUrl + EmbedConfigProvider.Current.SiteIdentifier;
                var embedType = EmbedConfigProvider.Current.EmbedType;
                var environment = EmbedConfigProvider.Current.Environment;
                var expirationForJs = (int.TryParse(EmbedConfigProvider.Current.ExpirationTime, out var expJs) ? expJs : 100000);
                htmlString.Append("<!DOCTYPE html><html><head><link rel='stylesheet' href='" + System.AppDomain.CurrentDomain.BaseDirectory.Replace("bin\\x64\\Debug\\", "") + "content\\chromium.css'/><script type='text/javascript' src='https://cdnjs.cloudflare.com/ajax/libs/jquery/3.6.0/jquery.min.js'></script><script src='https://cdn.polyfill.io/v2/polyfill.min.js'></script><script type='text/javascript' src='https://cdn.boldbi.com/embedded-sdk/latest/boldbi-embed.js'></script></script>");
                htmlString.Append("<script type='text/javascript'>$(document).ready(function() {this.dashboard = BoldBI.create({ serverUrl:'" + serverUrlForJs + "', dashboardId:'" + EmbedConfigProvider.Current.DashboardId + "',embedContainerId: 'dashboard',embedType:'" + embedType + "',environment:'" + environment + "',width: window.innerWidth - 20 + 'px',height: window.innerHeight - 20 + 'px',expirationTime: " + expirationForJs + ",authorizationServer:{url: '', data:" + resultContent + "},dashboardSettings:{showExport: false,showRefresh: false,showMoreOption: false}});console.log(this.dashboard);this.dashboard.loadDashboard();});</script></head><body style='background-color: white'><div id ='viewer-section' style='background-color: white'><div id ='dashboard'></div></div></body></html>");
                string filePath = AppDomain.CurrentDomain.BaseDirectory + "EmbedWrapper.html";
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
                using (FileStream fs = new FileStream(filePath, FileMode.Create))
                {
                    using (StreamWriter wr = new StreamWriter(fs, Encoding.UTF8))
                    {
                        wr.Write(htmlString.ToString());
                    }
                }
                Url = filePath;
            }
        }

        public string GetSignatureUrl(string message)
        {
            var encoding = new System.Text.UTF8Encoding();
            var keyBytes = encoding.GetBytes(EmbedConfigProvider.Current.EmbedSecret);
            var messageBytes = encoding.GetBytes(message);
            using (var hmacsha1 = new HMACSHA256(keyBytes))
            {
                var hashMessage = hmacsha1.ComputeHash(messageBytes);
                return Convert.ToBase64String(hashMessage);
            }
        }
    }
}
