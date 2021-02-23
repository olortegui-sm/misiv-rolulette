using RouletteMisiv.Infrastructure.Exceptions;
using RouletteMisiv.Infrastructure.Repositories;
using RouletteMisiv.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static RouletteMisiv.Infrastructure.Exceptions.CustomException;

namespace RouletteMisiv.Infrastructure.Services
{
    public class RouletteService : IRouletteService
    {
        IRouletteRepository _rouletteRepository;
        public RouletteService(IRouletteRepository rouletteRepository)
        {
            _rouletteRepository = rouletteRepository ?? throw new ArgumentNullException(nameof(rouletteRepository));
        }

        public Roulette create()
        {
            Roulette roulette = new Roulette()
            {
                Id = Guid.NewGuid().ToString(),
                IsOpen = false,
                Open = null,
                Close = null
            };
            _rouletteRepository.Save(roulette);

            return roulette;
        }

        public Roulette Find(string Id)
        {
            return _rouletteRepository.GetById(Id);
        }

        public Roulette Open(string Id)
        {
            Roulette roulette = _rouletteRepository.GetById(Id);
            if (roulette == null)
            {
                throw new CustomException(400, new ServiceResponse
                {
                    Code = 0,
                    Message = "No se encontró la ruleta."
                });
            }
            if (roulette.Open != null)
            {
                throw new CustomException(400, new ServiceResponse
                {
                    Code = 0,
                    Message = "Ruleta ya está aperturada."
                });
            }
            roulette.Open = DateTime.UtcNow.AddHours(-5);
            roulette.IsOpen = true;

            return _rouletteRepository.Update(Id, roulette);
        }

        public Roulette Close(string Id)
        {
            Roulette roulette = _rouletteRepository.GetById(Id);
            if (roulette == null)
            {
                throw new CustomException(400, new ServiceResponse
                {
                    Code = 0,
                    Message = "No se encontró la ruleta."
                });
            }
            if (roulette.Close != null)
            {
                throw new CustomException(400, new ServiceResponse
                {
                    Code = 0,
                    Message = "Ruleta ya está cerrada."
                });
            }
            Random randon = new Random();
            var winner = randon.Next(0, 36);
            foreach(var item in roulette.numberBets)
            {
                if (item.Number == winner && !item.IsColor && !string.IsNullOrEmpty(item.Player))
                {
                    item.IsWinningNumber = true;
                    item.TotalWon = item.AmountBet * 5;
                }else if (item.Number == winner && item.IsColor && !string.IsNullOrEmpty(item.Player))
                {
                    item.IsWinningNumber = true;
                    item.TotalWon = item.AmountBet * 1.8;
                }
                else if (item.Number == winner && string.IsNullOrEmpty(item.Player))
                {
                    item.IsWinningNumber = true;
                }
            }
            roulette.Close = DateTime.UtcNow.AddHours(-5);
            roulette.IsOpen = false;

            return _rouletteRepository.Update(Id, roulette);
        }

        public Roulette Bet(Bet request, string UserId)
        {
            if (request.money > 10000 || request.money < 1)
            {
                throw new CustomException(400, new ServiceResponse
                {
                    Code = 0,
                    Message = "Debe ingresar un monto permitido"
                });
            }
            Roulette roulette = _rouletteRepository.GetById(request.RouletteId);
            if (roulette == null)
            {
                throw new CustomException(400, new ServiceResponse
                {
                    Code = 0,
                    Message = "No se encontró la ruleta."
                });
            }
            if (roulette.IsOpen == false)
            {
                throw new CustomException(400, new ServiceResponse
                {
                    Code = 0,
                    Message = "La ruleta está cerrada."
                });
            }
            double value = 0d;
            foreach (var item in roulette.numberBets)
            {
                if (item.Number == request.position)
                {
                    item.Player = UserId;
                    item.AmountBet = value + request.money;
                    item.IsColor = request.IsColor;
                }
            }

            return _rouletteRepository.Update(roulette.Id, roulette);
        }

        public List<Roulette> GetAll()
        {
            return _rouletteRepository.GetAll();
        }
    }
}
