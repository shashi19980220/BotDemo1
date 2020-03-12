// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
//
// Generated with Bot Builder V4 SDK Template for Visual Studio EchoBot v4.6.2

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Integration.AspNet.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using BotDemo1.Bots;
using BotDemo1.Services;
using Microsoft.Bot.Builder.Azure;
using BotDemo1.Dialogs;

namespace BotDemo1
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // Create the Bot Framework Adapter with error handling enabled.
            services.AddSingleton<IBotFrameworkHttpAdapter, AdapterWithErrorHandler>();

            //luis
            services.AddSingleton<BotServices>();
            //configure state
            ConfigureState(services);
            ConfigureDialogs(services);
            // Create the bot as a transient. In this case the ASP Controller is expecting an IBot.
            services.AddTransient<IBot, DialogBot<MainDialog>>();
        }

        public void ConfigureState(IServiceCollection services)
        {
            //create storage for user and conversation state
            services.AddSingleton<IStorage, MemoryStorage>();
            //replace above line to connect with azure blob
            //var storageAccount = "DefaultEndpointsProtocol=https;AccountName=botdemoblob;AccountKey=gARtdme5xRQQJ2EsU8yiXpPYh0nTZVywfhJIom4gRmjC399/tb+551PPHTow3YB5EVuRCYndK1b5CdZv0JK+8Q==;EndpointSuffix=core.windows.net";
            //var storageContainer = "mystatedata";

            //services.AddSingleton<IStorage>(new AzureBlobStorage(storageAccount, storageContainer));

            //create user state
            services.AddSingleton<UserState>();
            //create the conversation state
            services.AddSingleton<ConversationState>();
            //instance of state service
            services.AddSingleton<BotStateService>();
        }

        public void ConfigureDialogs(IServiceCollection services)
        {
            services.AddSingleton<MainDialog>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseWebSockets();
            //app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
