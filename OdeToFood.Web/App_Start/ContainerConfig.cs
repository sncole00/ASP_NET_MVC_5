using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using OdeToFood.Data.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace OdeToFood.Web
{
    public class ContainerConfig
    {
        internal static void RegisterContainer(HttpConfiguration httpConfiguration)
        {
            var builder = new ContainerBuilder();

            builder.RegisterControllers(typeof(MvcApplication).Assembly);
            builder.RegisterApiControllers(typeof(MvcApplication).Assembly);

            // The following statement was appropriate for in-memory data.  However, when we graduate to
            // an SQL database, we need some changes.
            //builder.RegisterType<InMemoryRestaurantData>().As<IRestaurantData>().SingleInstance();

            // The following statements - configuration for SQL database - replace the in-memory option above.  Refer
            // to the section titled "Configuring a DbContext".
            builder.RegisterType<SqlRestaurantData>().As<IRestaurantData>().InstancePerRequest();
            builder.RegisterType<OdeToFoodDbContext>().InstancePerRequest();

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            // We added this statement for the purpose of providing an API Controller (to deliver XML or JSON
            // instead of HTML); refer to the section titled "MVC and API Controllers")/
            httpConfiguration.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
    }
}