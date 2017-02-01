using Autofac;
using MovieStore.BLL.BusinessService;
using MovieStore.BLL.BusinessServiceInterfaces;
using MovieStore.BLL.Repositories;
using MovieStore.Service.Interfaces;
using MovieStore.Service.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MovieStore.BLL.AutofacDI
{
    public class ContainerConfig
    {
       public static IContainer BuildContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<MovieBS>().As<IMovieBS>();
            builder.RegisterType<MovieRepository>().As<IMovieRepository>();
            builder.RegisterType<MovieService>().As<IMovieService>();
            
            return builder.Build();
        }
    }
}