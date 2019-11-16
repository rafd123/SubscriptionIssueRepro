using HotChocolate.Resolvers;
using HotChocolate.Types;
using SubscriptionIssueRepro.Data;

namespace SubscriptionIssueRepro.GraphQL
{
    public class QueryType : ObjectType
    {
        protected override void Configure(IObjectTypeDescriptor descriptor)
        {
            descriptor
                .Field("character")
                .Type<CharacterType>()
                .Argument("id", x => x.Type<NonNullType<IdType>>())
                .Resolver((context, cancellationToken) =>
                {
                    var store = context.Service<IRepository>();
                    return context
                        .BatchDataLoader<int, Character>(nameof(store.GetCharactersById), store.GetCharactersById)
                        .LoadAsync(context.Argument<int>("id"), cancellationToken);
                });
        }
    }
}
