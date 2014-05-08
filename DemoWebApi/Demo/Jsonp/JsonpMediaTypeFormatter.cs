using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;

namespace DemoWebApi.Demo.Jsonp
{
    public class JsonpMediaTypeFormatter : JsonMediaTypeFormatter
    {
        private string callback;

        public JsonpMediaTypeFormatter()
        {
            this.SupportedMediaTypes.Add(JsonMediaTypeFormatter.DefaultMediaType);
            this.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/javascript"));

            this.MediaTypeMappings.Add(new UriPathExtensionMapping("jsonp", JsonMediaTypeFormatter.DefaultMediaType));
        }

        public string Callback
        {
            get { return this.callback ?? "callback"; }
            set { this.callback = value; }
        }

        public override Task WriteToStreamAsync(Type type, object value, Stream writeStream, HttpContent content, TransportContext transportContext)
        {
            string recall;

            if (IsJsonp(out recall))
            {
                return Task.Factory.StartNew(() =>
                {
                    var writer = new StreamWriter(writeStream);
                    writer.Write(recall + "(");
                    writer.Flush();

                    base.WriteToStreamAsync(type, value, writeStream, content, transportContext).Wait();

                    writer.Write(")");
                    writer.Flush();
                });
            }
            else
            {
                return base.WriteToStreamAsync(type, value, writeStream, content, transportContext);
            }
        }

        private bool IsJsonp(out string rappel)
        {
            rappel = null;

            if (HttpContext.Current.Request.HttpMethod != "GET")
            {
                return false;
            }

            rappel = HttpContext.Current.Request.QueryString[this.Callback];

            return !string.IsNullOrEmpty(rappel);
        }
    }
}