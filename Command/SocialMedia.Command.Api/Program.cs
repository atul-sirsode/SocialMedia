
using Confluent.Kafka;
using MongoDB.Bson.Serialization;
using SocialMedia.Command.Api.Commands;
using SocialMedia.Command.Api.Commands.Handlers;
using SocialMedia.Command.Domain.Aggregates;
using SocialMedia.Command.Infrastructure.Config;
using SocialMedia.Command.Infrastructure.Dispatchers;
using SocialMedia.Command.Infrastructure.Handlers;
using SocialMedia.Command.Infrastructure.KafkaProducers;
using SocialMedia.Command.Infrastructure.Repository;
using SocialMedia.Command.Infrastructure.Stores;
using SocialMedia.Common.Events;
using SocialMedia.Core.Domain;
using SocialMedia.Core.Events;
using SocialMedia.Core.Handlers;
using SocialMedia.Core.Infrastructure;
using SocialMedia.Core.KafkaProducers;

namespace SocialMedia.Command.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            {
                BsonClassMap.RegisterClassMap<BaseEvent>();
                BsonClassMap.RegisterClassMap<PostCreatedEvent>();
                BsonClassMap.RegisterClassMap<MessageUpdatedEvent>();
                BsonClassMap.RegisterClassMap<PostLikedEvent>();
                BsonClassMap.RegisterClassMap<PostRemovedEvent>();
                BsonClassMap.RegisterClassMap<CommendAddedEvent>();
                BsonClassMap.RegisterClassMap<CommentUpdatedEvent>();
                BsonClassMap.RegisterClassMap<CommentRemoveEvent>();
                

                // Add services to the container.
                builder.Services.Configure<MongoDbConfig>(builder.Configuration.GetSection(nameof(MongoDbConfig)));
                builder.Services.Configure<ProducerConfig>(builder.Configuration.GetSection(nameof(ProducerConfig)));

                builder.Services.AddScoped<IEventStoreRepository, EventStoreRepository>();
                builder.Services.AddScoped<IEventStore, EventStore>();
                builder.Services.AddScoped<IEventSourceHandler<PostAggregate>, EventSourceHandler>();
                builder.Services.AddScoped<IEventProducer, EventProducers>();
                builder.Services.AddScoped<ICommandHandler, CommandHandler>();

                //register command handlers
                var commandHandler = builder.Services.BuildServiceProvider().GetService<ICommandHandler>();
                var dispatcher = new CommandDispatcher();
                dispatcher.RegisterHandler<NewPostCommand>(commandHandler.HandleAsync);
                dispatcher.RegisterHandler<EditPostCommand>(commandHandler.HandleAsync);
                dispatcher.RegisterHandler<LikePostCommand>(commandHandler.HandleAsync);
                dispatcher.RegisterHandler<AddCommentCommand>(commandHandler.HandleAsync);
                dispatcher.RegisterHandler<EditCommentCommand>(commandHandler.HandleAsync);
                dispatcher.RegisterHandler<RemoveCommentCommand>(commandHandler.HandleAsync);
                dispatcher.RegisterHandler<RemovePostCommand>(commandHandler.HandleAsync);
                builder.Services.AddSingleton<ICommandDispatcher>(_ => dispatcher);



                builder.Services.AddControllers();
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