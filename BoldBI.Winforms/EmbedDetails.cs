using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace BoldBI.Winforms
{
    [DataContract]
    internal class EmbedDetails
    {
        [DataMember]
        public string DashboardId { get; set; }

        [DataMember(Name = "ServerUrl")]
        public string ServerUrl { get; set; }

        [DataMember]
        public string UserEmail { get; set; }

        [DataMember]
        public string EmbedSecret { get; set; }

        [DataMember]
        public string EmbedType { get; set; }

        [DataMember]
        public string Environment { get; set; }

        [DataMember]
        public string ExpirationTime { get; set; }

        [DataMember]
        public string SiteIdentifier { get; set; }
    }

    internal static class EmbedConfigProvider
    {
        public static EmbedDetails Current { get; private set; }

        public static void Load(string filePath = null)
        {
            var path = filePath ?? Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "embedConfig.json");
            if (!File.Exists(path))
                throw new FileNotFoundException("embedConfig.json not found", path);

            string json;
            // Read with BOM detection
            using (var sr = new StreamReader(path, Encoding.UTF8, detectEncodingFromByteOrderMarks: true))
            {
                json = sr.ReadToEnd();
            }

            if (string.IsNullOrWhiteSpace(json))
                throw new InvalidOperationException("embedConfig.json is empty.");

            // Strip BOM if present
            if (json.Length > 0 && json[0] == '\uFEFF')
                json = json.Substring(1);

            var ser = new DataContractJsonSerializer(typeof(EmbedDetails));
            using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(json)))
            {
                Current = (EmbedDetails)ser.ReadObject(ms) as EmbedDetails;
            }

            if (Current == null)
                throw new InvalidOperationException("Failed to deserialize embed configuration.");

            if (!string.IsNullOrEmpty(Current.ServerUrl) && !Current.ServerUrl.EndsWith("/"))
                Current.ServerUrl += "/";

            if (Current.SiteIdentifier == null)
                Current.SiteIdentifier = string.Empty;
        }
    }
}
