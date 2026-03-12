using System;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using System.Windows.Forms;
using System.IO;

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
            var siteId = string.IsNullOrEmpty(EmbedConfigProvider.Current.SiteIdentifier) ? "" : EmbedConfigProvider.Current.SiteIdentifier;

            // Prepare embed generation payload
            var embedDetails = new
            {
                email = EmbedConfigProvider.Current.UserEmail,
                serverurl = EmbedConfigProvider.Current.ServerUrl,
                siteidentifier = siteId,
                embedsecret = EmbedConfigProvider.Current.EmbedSecret,
                dashboard = new { id = EmbedConfigProvider.Current.DashboardId }
            };

            string accessToken = null;

            using (var client = new HttpClient())
            {
                // POST to BoldBI embed authorize endpoint to get access token
                var requestUrl = EmbedConfigProvider.Current.ServerUrl.TrimEnd('/') + "/api/" + siteId + "/embed/authorize";
                var jsonPayload = JsonConvert.SerializeObject(embedDetails);
                var httpContent = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                var result = client.PostAsync(requestUrl, httpContent).Result;
                var resultContent = result.Content.ReadAsStringAsync().Result;

                // Try to extract access token from response
                try
                {
                    dynamic tokenResp = JsonConvert.DeserializeObject<dynamic>(resultContent);
                    if (tokenResp != null)
                    {
                        if (tokenResp.Data != null && tokenResp.Data.access_token != null)
                            accessToken = (string)tokenResp.Data.access_token;
                        else if (tokenResp.access_token != null)
                            accessToken = (string)tokenResp.access_token;
                        else if (tokenResp.data != null && tokenResp.data.access_token != null)
                            accessToken = (string)tokenResp.data.access_token;
                    }
                }
                catch { /* ignore parse errors */ }

                if (string.IsNullOrEmpty(accessToken))
                {
                    // Fallback: use raw response if token extraction failed
                    accessToken = resultContent;
                }

                // Build HTML embedding page and inject embedToken
                var htmlString = new StringBuilder();
                var serverUrlForJs = EmbedConfigProvider.Current.ServerUrl.TrimEnd('/') + "/" + EmbedConfigProvider.Current.SiteIdentifier;
                var environment = EmbedConfigProvider.Current.Environment;

                var cssPath = System.AppDomain.CurrentDomain.BaseDirectory.Replace("bin\\x64\\Debug\\", "") + "content\\chromium.css";
                htmlString.Append("<!DOCTYPE html><html><head><link rel='stylesheet' href='" + cssPath + "'/><script type='text/javascript' src='https://cdnjs.cloudflare.com/ajax/libs/jquery/3.6.0/jquery.min.js'></script><script src='https://cdn.polyfill.io/v2/polyfill.min.js'></script><script type='text/javascript' src='https://cdn.boldbi.com/embedded-sdk/latest/boldbi-embed.js'></script>");

                // Use JsonConvert.ToString(accessToken) to ensure the token is properly escaped as a JS string literal
                htmlString.Append("<script type='text/javascript'>$(document).ready(function() {this.dashboard = BoldBI.create({ serverUrl:'" + serverUrlForJs + "', dashboardId:'" + EmbedConfigProvider.Current.DashboardId + "', embedContainerId: 'dashboard', embedToken: " + JsonConvert.ToString(accessToken) + ", environment:'" + environment + "', width: window.innerWidth - 20 + 'px', height: window.innerHeight - 20 + 'px' "  + "}); this.dashboard.loadDashboard(); });</script></head><body style='background-color: white'><div id ='viewer-section' style='background-color: white'><div id ='dashboard'></div></div></body></html>");

                string filePath = AppDomain.CurrentDomain.BaseDirectory + "EmbedWrapper.html";
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
                using (FileStream fs = new FileStream(filePath, FileMode.Create))
                using (StreamWriter wr = new StreamWriter(fs, Encoding.UTF8))
                {
                    wr.Write(htmlString.ToString());
                }
                Url = filePath;
            }
        }
    }
}
