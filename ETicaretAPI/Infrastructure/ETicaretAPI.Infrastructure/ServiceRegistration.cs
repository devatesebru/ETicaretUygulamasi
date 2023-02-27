﻿using ETicaretAPI.Application.Services;
using ETicaretAPI.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Infrastructure
{
    public  static class ServiceRegistration
    {
        public static void AddInfrastructureServices(this IServiceCollection servisCollection)
        {
            servisCollection.AddScoped<IFileService, FileService>();
        }
    }
}
