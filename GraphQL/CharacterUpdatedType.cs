using System.Globalization;
using HotChocolate.Language;
using HotChocolate.Resolvers;
using HotChocolate.Subscriptions;
using HotChocolate.Types;
using SubscriptionIssueRepro.Data;

namespace SubscriptionIssueRepro.GraphQL
{
    public class CharacterUpdatedType : ObjectType<CharacterUpdatedType.Payload>
    {
        protected override void Configure(IObjectTypeDescriptor<Payload> descriptor)
        {
            descriptor
                .Field("character")
                .Type<NonNullType<CharacterType>>()
                .Resolver((context, cancellationToken) =>
                {
                    var store = context.Service<IRepository>();
                    return context
                        .BatchDataLoader<int, Character>(nameof(store.GetCharactersById), store.GetCharactersById)
                        .LoadAsync(context.Parent<Payload>().Id, cancellationToken);
                });
        }

        public class Payload
        {
            public Payload(int id)
            {
                Id = id;
            }

            public int Id { get; }
        }

        public class Message : EventMessage
        {
            public Message(int id)
                : base(CreateEventDescription(id), new Payload(id))
            {
            }

            private static IEventDescription CreateEventDescription(int id)
            {
                return new EventDescription("characterUpdated",
                    new ArgumentNode("id", new StringValueNode(id.ToString(CultureInfo.InvariantCulture))));
            }
        }
    }
}
