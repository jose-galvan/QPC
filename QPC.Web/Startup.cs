﻿using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;
using QPC.Web.Identity;
using System;

[assembly: OwinStartupAttribute(typeof(QPC.Web.Startup))]
namespace QPC.Web
{
    public partial class Startup
    {

        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
