using HotChocolate.Subscriptions;
using HotChocolate.Types;
using SubscriptionIssueRepro.Data;

namespace SubscriptionIssueRepro.GraphQL
{
    public class MutationType : ObjectType
    {
        protected override void Configure(IObjectTypeDescriptor descriptor)
        {
            descriptor
                .Field("updateCharacterName")
                .Argument("id", x => x.Type<NonNullType<IdType>>())
                .Argument("name", x => x.Type<NonNullType<StringType>>())
                .Resolver(async (context, _) =>
                {
                    var character = context.Service<IRepository>()
                        .UpdateCharacterName(
                            context.Argument<int>("id"),
                            context.Argument<string>("name"));

                    await context.Service<IEventSender>().SendAsync(new CharacterUpdatedType.Message(character.Id));

                    return character;
                });
        }
    }
}
