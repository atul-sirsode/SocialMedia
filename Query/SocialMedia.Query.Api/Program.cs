
using Confluent.Kafka;
using Microsoft.EntityFrameworkCore;
using SocialMedia.Core.Consumers;
using SocialMedia.Core.Infrastructure;
using SocialMedia.Query.Api.Queries;
using SocialMedia.Query.Api.Queries.Handlers;
using SocialMedia.Query.Domain.Entities;
using SocialMedia.Query.Domain.Repositories;
using SocialMedia.Query.Infrastructure.Consumers;
using SocialMedia.Query.Infrastructure.DataContext;
using SocialMedia.Query.Infrastructure.Dispatcher;
using SocialMedia.Query.Infrastructure.Handlers;
using SocialMedia.Query.Infrastructure.Repositories;
using EventHandler = SocialMedia.Query.Infrastructure.Handlers.EventHandler;

namespace SocialMedia.Query.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            {
                void ConfigureDbContext(DbContextOptionsBuilder options)
                {
                    options
                        .UseLazyLoadingProxies()
                        .UseSqlServer(builder.Configuration.GetConnectionString("SocialMediaSqlDb"));
                }

                // Add services to the container.
                builder.Services.AddDbContext<DatabaseContext>((Action<DbContextOptionsBuilder>)ConfigureDbContext);
                builder.Services.AddSingleton(new DatabaseContextFactory(ConfigureDbContext));
                builder.Services.AddScoped<IPostRepository, PostRepository>();
                builder.Services.AddScoped<ICommentRepository, CommentRepository>();
                builder.Services.AddScoped<IQueryHandler, QueryHandler>();
                builder.Services.AddScoped<IEventHandler, EventHandler>();
                builder.Services.Configure<ConsumerConfig>(builder.Configuration.GetSection(nameof(ConsumerConfig)));
                builder.Services.AddScoped<IEventConsumer, EventConsumer>();

                //register query handlers
                var queryHandler = builder.Services.BuildServiceProvider().GetRequiredService<IQueryHandler>();
                var dispatcher = new QueryDispatcher();
                dispatcher.RegisterHandler<FindAllPostQuery>(queryHandler.HandleAsync);
                dispatcher.RegisterHandler<FindPostByAuthorQuery>(queryHandler.HandleAsync);
                dispatcher.RegisterHandler<FindPostByIdQuery>(queryHandler.HandleAsync);
                dispatcher.RegisterHandler<FindPostWithCommentsQuery>(queryHandler.HandleAsync);
                dispatcher.RegisterHandler<FindPostWithLikesQuery>(queryHandler.HandleAsync);
                builder.Services.AddSingleton<IQueryDispatcher<PostEntity>>(_ => dispatcher);

                if (builder.Environment.IsDevelopment())
                {
                    //Create Database and table
                    var dbContext = builder.Services.BuildServiceProvider().GetService<DatabaseContext>();
                    dbContext?.Database.EnsureCreated();
                }



                builder.Services.AddControllers();
                builder.Services.AddHostedService<ConsumerHostedService>();
                // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
                builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddSwaggerGen();

            }

            var app = builder.Build();
            {
                // Configure the HTTP request pipeline.
                if (app.Environment.IsDevelopment())
                {
                    app.UseSwagger();
                    app.UseSwaggerUI();
                }

                app.UseHttpsRedirection();

                app.UseAuthorization();


                app.MapControllers();
            }


            app.Run();
        }
    }
}