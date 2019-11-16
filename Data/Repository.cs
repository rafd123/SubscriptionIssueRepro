using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace SubscriptionIssueRepro.Data
{
    public interface IRepository
    {
        Character UpdateCharacterName(int characterId, string name);

        Task<IReadOnlyDictionary<int, Character>> GetCharactersById(IReadOnlyList<int> showIds);
    }

    public class Repository : IRepository
    {
        private readonly object _syncRoot = new object();

        private readonly Dictionary<int, Character> _characters = new[]
            {
                new Character(id: 1, name: "Rick")
            }
            .ToDictionary(x => x.Id);


        public Character UpdateCharacterName(int characterId, string name)
        {
            lock (_syncRoot)
            {
                if(!_characters.TryGetValue(characterId, out var character))
                {
                    throw new InvalidOperationException("Character not found");
                }
                
                character = new Character(
                    id: character.Id,
                    name: name);

                _characters[character.Id] = character;

                return character;
            }
        }

        public Task<IReadOnlyDictionary<int, Character>> GetCharactersById(IReadOnlyList<int> ids)
        {
            lock (_syncRoot)
            {
                var characters = ids
                    .Where(id => _characters.ContainsKey(id))
                    .Select(id => _characters[id])
                    .ToDictionary(x => x.Id);

                return Task.FromResult(
                    (IReadOnlyDictionary<int, Character>)
                    new ReadOnlyDictionary<int, Character>(characters));
            }
        }
    }
}
