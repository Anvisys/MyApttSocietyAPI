using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace MyApttSocietyAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "DefaultApiWithAction",
                routeTemplate: "api/{controller}/{Action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.EnableCors();

            var json = config.Formatters.JsonFormatter;
            //json.UseDataContractJsonSerializer = true;
            json.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.All;
            json.SerializerSettings.DateFormatHandling = Newtonsoft.Json.DateFormatHandling.IsoDateFormat;
            //json.SerializerSettings.DateFormatString = DateTimeKind.Utc;
            //json.SerializerSettings.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Local;

            config.Formatters.Remove(config.Formatters.XmlFormatter);
        }
    }
}
