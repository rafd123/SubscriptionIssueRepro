namespace SubscriptionIssueRepro.Data
{
    public class Character
    {
        public Character(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public int Id { get; }
        public string Name { get; }
    }
}
