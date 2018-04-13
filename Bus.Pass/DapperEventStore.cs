using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using Bus.Common;
using Dapper;
using Microsoft.Extensions.Logging;

namespace Bus.Pass
{
    public class DapperEventStore : IEventStore
    {
        private readonly string connectionString;
        private readonly ILogger logger;
        private readonly IMessageSerializer serializer;

        public DapperEventStore(string connectionString, ILogger<DapperEventStore> logger, IMessageSerializer serializer)
        {
            this.logger = logger;
            this.serializer = serializer;
            this.connectionString = connectionString;
            logger.LogInformation($"DapperEventStore构造函数调用完成。Hash Code：{this.GetHashCode()}.");
            logger.LogInformation($"Serializer序列化工具是。Hash Code：{this.GetHashCode()}.");
        }

        public async Task SaveEventAsync<TEvent>(TEvent @event) where TEvent : IEvent
        {

            const string sql = @"INSERT INTO [dbo].[Events] 
                                ([EventId], [EventPayload], [EventTimestamp]) 
                                VALUES 
                                (@eventId, @eventPayload, @eventTimestamp)";
            using (var connection = new SqlConnection(this.connectionString))
            {
             
                await connection.ExecuteAsync(sql, new
                {
                    eventId = @event.Id,
                    eventPayload = serializer.SerializeObject(@event),
                    eventTimestamp = @event.Timestamp
                });
            }
        }
        private bool disposedValue = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    this.logger.LogInformation($"DapperEventStore已经被Dispose。Hash Code:{this.GetHashCode()}.");
                }

                disposedValue = true;
            }
        }

        public void Dispose() => Dispose(true);

    }
}
