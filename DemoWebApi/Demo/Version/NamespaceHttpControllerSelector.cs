using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using System.Web.Http.Routing;

namespace DemoWebApi.Demo.Version
{
    public class NamespaceHttpControllerSelector : IHttpControllerSelector
    {
        
        private readonly HttpConfiguration configuration;

        private readonly Lazy<Dictionary<string, HttpControllerDescriptor>> controleurs;

        private readonly HashSet<string> doublons;

        public NamespaceHttpControllerSelector(HttpConfiguration config)
        {
            this.configuration = config;
            this.doublons = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            this.controleurs =
                new Lazy<Dictionary<string, HttpControllerDescriptor>>(this.InitializeControllerDictionary);
        }

        public IDictionary<string, HttpControllerDescriptor> GetControllerMapping()
        {
            return this.controleurs.Value;
        }

        public HttpControllerDescriptor SelectController(HttpRequestMessage request)
        {
            IHttpRouteData routeData = request.GetRouteData();
            if (routeData == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            var version = GetRouteVariable<string>(routeData, "version");
            if (version == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            var controllerName = GetRouteVariable<string>(routeData, "controller");
            if (controllerName == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            // Find controller
            var key =
                String.Format(CultureInfo.InvariantCulture, "{0}.{1}", version, controllerName)
                      .ToUpperInvariant();

            HttpControllerDescriptor controllerDescriptor;
            if (this.controleurs.Value.TryGetValue(key, out controllerDescriptor))
            {
                return controllerDescriptor;
            }

            if (this.doublons.Contains(key))
            {
                throw new HttpResponseException(
                    request.CreateErrorResponse(
                        HttpStatusCode.InternalServerError, "Too many controllers for this request"));
            }

            throw new HttpResponseException(HttpStatusCode.NotFound);
        }

        private static T GetRouteVariable<T>(IHttpRouteData routeData, string name)
        {
            object result;
            if (routeData.Values.TryGetValue(name, out result))
            {
                return (T)result;
            }

            return default(T);
        }

        private Dictionary<string, HttpControllerDescriptor> InitializeControllerDictionary()
        {
            var dictionary = new Dictionary<string, HttpControllerDescriptor>(StringComparer.OrdinalIgnoreCase);

            // Créer une table de lookup où la clé est "namespace.controller". La valeur de "namespace" est le dernier
            // segment du nom complet du namespace. Par exemple, 
            // DemoWebApi.Controllers.V1.BoardsController => "V1.Boards" 
            var assembliesResolver = this.configuration.Services.GetAssembliesResolver();
            var controllersResolver =
                this.configuration.Services.GetHttpControllerTypeResolver();

            var typeControleurs = controllersResolver.GetControllerTypes(assembliesResolver);

            foreach (var t in typeControleurs)
            {
                if (t.Namespace == null)
                {
                    continue;
                }

                var segments = t.Namespace.Split(Type.Delimiter);

                // Pour la clé du dictionnaire, enlever "Controller" du nom du type.
                // Cela correspond au comportement de DefaultHttpControllerSelector
                var nomControleur =
                    t.Name.Remove(t.Name.Length - DefaultHttpControllerSelector.ControllerSuffix.Length);

                var cle =
                    String.Format(
                        CultureInfo.InvariantCulture,
                        "{0}.{1}",
                        segments[segments.Length - 1],
                        nomControleur).ToUpperInvariant();

                // Vérifier les doublons.
                if (dictionary.Keys.Contains(cle))
                {
                    this.doublons.Add(cle);
                }
                else
                {
                    dictionary[cle] = new HttpControllerDescriptor(this.configuration, t.Name, t);
                }
            }

            // Retirer les doublons du dictionnaire, parce que ceux-ci créent des correspondances ambigues.
            foreach (var s in this.doublons)
            {
                dictionary.Remove(s);
            }

            return dictionary;
        }
    }
}