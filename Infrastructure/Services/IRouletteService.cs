using RouletteMisiv.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RouletteMisiv.Infrastructure.Services
{
    public interface IRouletteService
    {
        Roulette create();
        Roulette Open(string Id);
        Roulette Find(string Id);
        Roulette Close(string Id);
        Roulette Bet(Bet request, string UserId);
        List<Roulette> GetAll();
    }
}
