using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoWebApi.Tests.Utils
{
    public class RouteInfo
    {
        public Type Controller { get; set; }

        public string Action { get; set; }

        public IDictionary<string, object> Params { get; set; }
    }
}
