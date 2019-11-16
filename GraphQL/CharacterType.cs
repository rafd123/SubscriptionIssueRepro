using HotChocolate.Types;
using SubscriptionIssueRepro.Data;

namespace SubscriptionIssueRepro.GraphQL
{
    public class CharacterType : ObjectType<Character>
    {
        protected override void Configure(IObjectTypeDescriptor<Character> descriptor)
        {
            descriptor.Field(x => x.Id);
            descriptor.Field(x => x.Name);
        }
    }
}
