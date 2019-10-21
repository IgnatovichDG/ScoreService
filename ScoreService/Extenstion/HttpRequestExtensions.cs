using System;
using Microsoft.AspNetCore.Http;

namespace ScoreService.Extenstion
{
    public static class HttpRequestExtensions
    {
        /// <summary>
        ///     Определить, выполняется ли запрос со стороны JavaScript из браузера (AJAX-запрос).
        /// </summary>
        /// <param name="request">Экземпляр HTTP-запроса.</param>
        /// <returns>
        ///     True, если запрос выполняется со стороны JavaScript браузера. False, если запрос выполняется самим браузером.
        /// </returns>
        public static bool IsAjaxRequest(this HttpRequest request)
        {
            return string.Equals(request.Query["X-Requested-With"], "XMLHttpRequest", StringComparison.Ordinal)
                   || string.Equals(request.Headers["X-Requested-With"], "XMLHttpRequest", StringComparison.Ordinal);
        }

    }
}