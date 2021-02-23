using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RouletteMisiv.Model
{
    public class Bet
    {
        public string RouletteId { get; set; }

        [Range(0.1d, maximum: 10000)]
        public double money { get; set; }

        [Range(0, 37)]
        public int position { get; set; }

        public bool IsColor { get; set; }
    }
}
