using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using CodeShare.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using shortid;
using shortid.Configuration;

namespace CodeShare.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMemoryCache _memoryCache;

        public HomeController (ILogger<HomeController> logger, IMemoryCache _memoryCache)
        {
            _logger = logger;
            this._memoryCache = _memoryCache;
        }

        [Route ("")]
        [Route ("/g/{**path}")]
        public IActionResult Index ([FromRoute] string path)
        {
            if (!string.IsNullOrWhiteSpace (path) && _memoryCache.TryGetValue (path, out EditorModel data))
            {
                return View (data);
            }

            return View (new EditorModel ());
        }

        [Route ("Share")]
        public IActionResult Share ([FromForm] EditorModel model)
        {
            model.Code = HttpUtility.JavaScriptStringEncode (model.Code, false);
            var url = ShortId.Generate (new GenerationOptions () { UseSpecialCharacters = false, Length = 8 });
            _memoryCache.Set (url, model, TimeSpan.FromDays (1));
            return Ok (url);
        }

        [ResponseCache (Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error ()
        {
            return View (new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
