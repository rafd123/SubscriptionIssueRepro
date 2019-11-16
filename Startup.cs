using HotChocolate;
using HotChocolate.AspNetCore;
using HotChocolate.Subscriptions;
using HotChocolate.Types;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SubscriptionIssueRepro.Data;
using SubscriptionIssueRepro.GraphQL;

namespace SubscriptionIssueRepro
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IRepository, Repository>();
            services.AddDataLoaderRegistry();
            services.AddInMemorySubscriptionProvider();
            services.AddGraphQL(sp => SchemaBuilder.New()
                .ModifyOptions(x => x.DefaultBindingBehavior = BindingBehavior.Explicit)
                .AddQueryType<QueryType>()
                .AddMutationType<MutationType>()
                .AddSubscriptionType<SubscriptionType>()
                .Create());
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseWebSockets();
            app.UseGraphQL();
            app.UsePlayground();
        }
    }
}
