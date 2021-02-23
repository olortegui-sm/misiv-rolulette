using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RouletteMisiv.Infrastructure.Services;
using RouletteMisiv.Model;
using static RouletteMisiv.Infrastructure.Exceptions.CustomException;

namespace RouletteMisiv.Controllers
{
    [Route("misiv/roulette")]
    [ApiController]
    public class RouletteController : ControllerBase
    {
        IRouletteService _rouletteService;
        public RouletteController(IRouletteService rouletteService)
        {
            _rouletteService = rouletteService ?? throw new ArgumentNullException(nameof(rouletteService));
        }

        [HttpPost]
        [Route("new-rulette")]
        public IActionResult NewRulette()
        {
            Roulette roulette = _rouletteService.create();
            return Ok(roulette);
        }

        [HttpPut("open/{id}")]
        public IActionResult Open([FromRoute(Name = "id")] string id)
        {
            try
            {
                _rouletteService.Open(id);
                return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest(400);
            }
        }

        [HttpPut("close/{id}")]
        public IActionResult Close([FromRoute(Name = "id")] string id)
        {
            try
            {
                Roulette roulette = _rouletteService.Close(id);
                return Ok(roulette);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(405);
            }
        }

        [HttpPost("bet")]
        public IActionResult Bet([FromBody] Bet request, [FromHeader(Name = "user-id")] string userId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ServiceResponse
                {
                    Code = 0,
                    Message = "Debe ingresar los parámtros obligatorios."
                });
            }
            try
            {
                Roulette roulette = _rouletteService.Bet(request, userId);
                return Ok(roulette);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(405);
            }

        }

        [HttpGet]
        [Route("geta-all")]
        public IActionResult GetAll()
        {
            return Ok(_rouletteService.GetAll());
        }
    }
}
