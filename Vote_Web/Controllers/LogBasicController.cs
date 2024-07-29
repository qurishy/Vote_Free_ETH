using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Nethereum.Web3;
using Vote_Web.Models;

namespace Vote_Web.Controllers
{
    public class LogBasicController : Controller
    {
        private readonly Web3 _web3;

        public LogBasicController(IOptions<Web3Config> cond)
        {
            _web3 =new Web3(cond.Value.EthereumNodeUrl);

        }
        public async Task<IActionResult> Index()
        {

            var blocknumber = _web3.Eth.Blocks.GetBlockNumber.SendRequestAsync().Result;

           ViewBag.message = blocknumber;

            return View();
        }



        public IActionResult Login()
        {
            var metamask = new MetaMask();

            return View(metamask);
        }



        [HttpPost]
        public IActionResult Login(MetaMask metamask)
        {
            if (!ModelState.IsValid)
            {

                string signature = metamask.Signature;
                string address = metamask.Address;
                    // Process the data and return a response
                    return Json(new { message = "Data received successfully!" });
                
            }
            else
            {
                return View(metamask);


            }

        }
    }
}
