
using System.Text.Json;
using Infrastructure.Data.Models;

namespace Infrastructure.Data.Interceptors;

internal sealed class EventsPublisherSaveChangesInterceptor(TimeProvider time, [FromKeyedServices("Snowflake")] IPrimitiveTypeIdGenerator<long> idGen) : SaveChangesInterceptor
{
    private static readonly JsonSerializerOptions _serializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };
    public async override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken ct = default)
    {
        if (eventData.Context is not null)
        {


            var outboxMessages = eventData.Context.ChangeTracker
                .Entries<IAggregateRoot>()
                .SelectMany(entry =>
                {
                    var aggregate = entry.Entity;

                    var messages = aggregate.DomainEvents.Select(ev =>
                        new OutboxMessage
                        {
                            Id = idGen.Generate(),
                            Type = ev.GetType().AssemblyQualifiedName!,
                            Content = JsonSerializer.Serialize(ev, ev.GetType(), _serializerOptions),
                            OccurredOnUtc = time.GetUtcNow().UtcDateTime
                        })
                        .ToList();

                    aggregate.ClearDomainEvents();
                    return messages;
                })
                .ToList();

            if(outboxMessages.Count > 0)
                eventData.Context.AddRange(outboxMessages);

        }

        return await base.SavingChangesAsync(eventData, result, ct);
    }

    public async override ValueTask<int> SavedChangesAsync(SaveChangesCompletedEventData eventData, int result, CancellationToken ct = default)
    {

        return await base.SavedChangesAsync(eventData, result, ct);
    }
}
