using Microsoft.AspNet.Identity;
using QPC.Api.Identity;
using QPC.Core.Repositories;
using QPC.DataAccess;
using QPC.Web.Helpers;
using System;
using System.Web.Http;
using Unity;
using Unity.Injection;
using Unity.Lifetime;

namespace QPC.Api
{
    public static class UnityConfig
    {
        public static UnityContainer Container { get; private set; }

        public static void RegisterComponents()
        {
            var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService


            container.RegisterType<IUnitOfWork, UnitOfWork>(new HierarchicalLifetimeManager(), new InjectionConstructor());
            container.RegisterType<QualityControlFactory, QualityControlFactory>(new HierarchicalLifetimeManager(), new InjectionConstructor());
            container.RegisterType<IUserStore<IdentityUser, Guid>, UserStore>(new TransientLifetimeManager());
            container.RegisterType<RoleStore>(new TransientLifetimeManager());

            
            GlobalConfiguration.Configuration.DependencyResolver = new Unity.WebApi.UnityDependencyResolver(container);

            //Container is stored in order to retrive instance of objects 
            // In sections where DI is not configured or needed but instances 
            // of objects that are registered in the container. 
            Container = container;
        }
    }
}