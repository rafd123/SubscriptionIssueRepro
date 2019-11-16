using HotChocolate.Subscriptions;
using HotChocolate.Types;

namespace SubscriptionIssueRepro.GraphQL
{
    public class SubscriptionType : ObjectType
    {
        protected override void Configure(IObjectTypeDescriptor descriptor)
        {
            descriptor
                .Field("characterUpdated")
                .Type<NonNullType<CharacterUpdatedType>>()
                .Argument("id", x => x.Type<NonNullType<IdType>>())
                .Resolver(context => context
                    .CustomProperty<CharacterUpdatedType.Message>(typeof(IEventMessage).FullName)
                    .Payload);
        }
    }
}