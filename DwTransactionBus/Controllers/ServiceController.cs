using System;

using Microsoft.AspNetCore.Mvc;

using DwFramework.Core;
using DwFramework.Core.Extensions;
using DwFramework.Core.Plugins;
using DwFramework.WebAPI;
using DwFramework.WebAPI.Plugins;

namespace DwTransactionBus
{
    [ApiController]
    [Route("service")]
    public class ServiceController : Controller
    {
        [HttpGet("get")]
        public IActionResult Get()
        {
            return Ok("ok");
        }
    }
}
