using RouletteMisiv.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RouletteMisiv.Infrastructure.Repositories
{
    public interface IRouletteRepository
    {
        List<Roulette> GetAll();
        Roulette GetById(string Id);
        Roulette Save(Roulette roulette);
        Roulette Update(string Id, Roulette roulette);
    }
}
