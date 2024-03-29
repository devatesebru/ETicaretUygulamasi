﻿
using ETicaretAPI.Application.Abstractions;
using ETicaretAPI.Application.Abstractions.Storage;
using ETicaretAPI.Infrastructure.Enums;
using ETicaretAPI.Infrastructure.Services;
using ETicaretAPI.Infrastructure.Services.Storage;
using ETicaretAPI.Infrastructure.Services.Storage.Local;
using Microsoft.AspNetCore.Mvc.TagHelpers.Cache;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Infrastructure
{
    public  static class ServiceRegistration
    {
        public static void AddInfrastructureServices(this IServiceCollection servisCollection)
        {
            servisCollection.AddScoped<IStorageService, StorageService>();
            servisCollection.AddScoped<ITokenHandler, TokenHandler>();

        }

        public static void AddStorage<T>(this IServiceCollection serviceCollection) where T : Storage, IStorage
        {
            serviceCollection.AddScoped<IStorage, T>();
        }

        public static void AddStorage(this IServiceCollection serviceCollection , StorageType storageType) 
        {
            switch(storageType){

                case StorageType.Local:
                    serviceCollection.AddScoped<IStorage, LocalStorage>();
                break;

                case StorageType.Azure:

                    break;

                case StorageType.AWS:

                    break;
                default:
                    serviceCollection.AddScoped<IStorage, LocalStorage>();
                    break;
            }
            //serviceCollection.AddScoped<IStorage, T>();
        }
    }
}
