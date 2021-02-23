using EasyCaching.Core;
using RouletteMisiv.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RouletteMisiv.Infrastructure.Repositories
{
    public class RouletteRepository : IRouletteRepository
    {
        private IEasyCachingProvider _cachingProvider;
        private const string REDIS_KEY = "ROULETE_KEY";
        public RouletteRepository(IEasyCachingProvider provider)
        {
            _cachingProvider = provider ?? throw new ArgumentNullException(nameof(provider));
        }
        public List<Roulette> GetAll()
        {
            var rouletes = _cachingProvider.GetByPrefix<Roulette>(REDIS_KEY);
            if (rouletes.Values.Count == 0)
            {
                return new List<Roulette>();
            }

            return new List<Roulette>(rouletes.Select(x => x.Value.Value));
        }
        public Roulette GetById(string Id)
        {
            var item = _cachingProvider.Get<Roulette>($"{REDIS_KEY}{Id}");
            if (!item.HasValue)
            {
                return null;
            }

            return item.Value;
        }
        public Roulette Save(Roulette roulette)
        {
            _cachingProvider.Set($"{REDIS_KEY}{roulette.Id}", roulette, TimeSpan.FromDays(10));

            return roulette;
        }
        public Roulette Update(string Id, Roulette roulette)
        {
            roulette.Id = Id;

            return Save(roulette);
        }
    }
}
