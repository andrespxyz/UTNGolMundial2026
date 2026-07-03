using Microsoft.AspNetCore.Mvc;
using PublicFrontend.Services;

namespace PublicFrontend.Controllers
{
    public class RankingController : Controller
    {
        private readonly UTNGolCoinClientService _golcoin;

        public RankingController(UTNGolCoinClientService golcoin)
        {
            _golcoin = golcoin;
        }

        public async Task<IActionResult> Index()
        {
            var ranking = await _golcoin.GetRankingAsync();
            return View(ranking);
        }
    }
}