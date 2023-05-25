using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPS.API.Commons
{
    public class GetBaseUrl
    {
        private readonly IHttpContextAccessor _context;
        private static string urlBase = "";

        public GetBaseUrl(IHttpContextAccessor context)
        {
            _context = context;
            urlBase = _context.HttpContext.Request.ToString();
        }

        public static string BaseUrl()
        {
            // Now that you have the request you can select what you need from it.
            return urlBase;
        }
    }
}
