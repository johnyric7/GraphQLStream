using GraphQLStream.Repositories;
using GraphQLStream.Schema.Queries;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;
using StackExchange.Redis;
using GraphQLStream.Schema.Subscriptions;
using GraphQLStream.Schema.Mutations;

namespace GraphQLDemo.API
{
    public class Startup
    {
        private readonly IConfiguration configuration;

        // Constructor to inject IConfiguration
        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IMongoClient>(sp =>
                new MongoClient(configuration.GetConnectionString("MongoDbConnection")));

            services.AddScoped<IMongoDbRepository, MongoDbRepository>();

            string redisConnectionString = configuration.GetConnectionString("RedisConnection");

            var redisConfig = ConnectionMultiplexer.Connect(redisConnectionString);

            services.AddSingleton<IConnectionMultiplexer>(redisConfig);

            services.AddGraphQLServer()
                .AddRedisSubscriptions(sp => sp.GetRequiredService<IConnectionMultiplexer>())
                .AddType<UserLocationType>()
                .AddQueryType<Query>()
                .AddMutationType<Mutation>()
                .AddSubscriptionType<Subscription>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseWebSockets();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGraphQL();
            });
        }
    }
}