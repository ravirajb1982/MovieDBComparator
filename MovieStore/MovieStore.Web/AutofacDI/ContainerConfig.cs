using Autofac;
using MovieStore.BLL.BusinessService;
using MovieStore.BLL.BusinessServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MovieStore.Web.AutofacDI
{
    public class ContainerConfig
    {
       public static IContainer BuildContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<MovieBS>().As<IMovieBS>();
            return builder.Build();
        }
    }
}