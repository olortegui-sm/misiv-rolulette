using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RouletteMisiv.Model
{
    public class Roulette
    {
        public string Id { get; set; }

        public bool IsOpen { get; set; } = false;

        public DateTime? Open { get; set; }

        public DateTime? Close { get; set; }
        public List<NumberBet> numberBets { get; set; }

        public Roulette()
        {
            this.Start();
        }
        private void Start()
        {
            numberBets = new List<NumberBet>();
            for (int i = 0; i < 37; i++)
            {
                numberBets.Add(new NumberBet()
                {
                    Number = i,
                    AmountBet = null,
                    Player = null,
                    TotalWon = null
                });
            }
        }
    }
    public class NumberBet
    {
        public int Number { get; set; }
        public string Player { get; set; }
        public double? AmountBet { get; set; }
        public double? TotalWon { get; set; }
        public bool IsWinningNumber { get; set; }
        public bool IsColor { get; set; }
    }
}
