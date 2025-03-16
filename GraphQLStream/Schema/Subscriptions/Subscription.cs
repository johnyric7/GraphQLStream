using GraphQLStream.Schema.Mutations;
using HotChocolate;
using HotChocolate.Types;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace GraphQLStream.Schema.Subscriptions
{
    public class Subscription
    {
        [Subscribe]
        public UserLocationResult UserCreated([EventMessage] UserLocationResult user) => user;

        // Method that handles the subscription of updates for a particular user
        [SubscribeAndResolve]
        public async IAsyncEnumerable<UserLocationResult> UserLocationUpdated(Guid userId, [Service] IConnectionMultiplexer topicEventReceiver)
        {
            // Generate a unique topic name for this user
            string topicName = $"{userId}_{nameof(Subscription.UserLocationUpdated)}";

            // Get the Redis subscriber instance
            var subscriber = topicEventReceiver.GetSubscriber();

            // Subscribe to the Redis channel for the given topic name
            var channel = await subscriber.SubscribeAsync(topicName);

            // Listen for messages and yield them as UserLocationResult
            await foreach (var message in channel)
            {
                // Deserialize the message to a UserLocationResult object
                var userLocationResult = JsonSerializer.Deserialize<UserLocationResult>(message.Message);

                if (userLocationResult != null)
                {
                    // Yield the UserLocationResult to the subscription client
                    yield return userLocationResult;
                }
            }
        }
    }
}