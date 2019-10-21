using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ScoreService.Extenstion;

namespace ScoreService.Controllers
{
    /// <summary>
    ///     Базовый контроллер для всех контроллеров MVC проекта.
    /// </summary>
  //  [Authorize("BackOffice")]
    public class ControllerBase : Controller
    {
        /// <summary>
        ///     Вернуть результат автоматического выбора представления: полное представление для обычного запроса,
        ///     или частичное представление для AJAX-запроса.
        /// </summary>
        /// <returns>
        ///     Экземпляр <see cref="Microsoft.AspNetCore.Mvc.ViewResult" /> или
        ///     <see cref="Microsoft.AspNetCore.Mvc.PartialViewResult" />.
        /// </returns>
        protected IActionResult AutoView()
        {
            if (this.Request.IsAjaxRequest()) return PartialView();
            return View();
        }

        /// <summary>
        ///     Вернуть результат автоматического выбора представления: полное представление для обычного запроса,
        ///     или частичное представление для AJAX-запроса.
        /// </summary>
        /// <param name="viewName">Наименование представления.</param>
        /// <returns>
        ///     Экземпляр <see cref="Microsoft.AspNetCore.Mvc.ViewResult" /> или
        ///     <see cref="Microsoft.AspNetCore.Mvc.PartialViewResult" />.
        /// </returns>
        protected IActionResult AutoView(
            string viewName)
        {
            if (this.Request.IsAjaxRequest()) return PartialView(viewName);
            return View(viewName);
        }

        /// <summary>
        ///     Вернуть результат автоматического выбора представления: полное представление для обычного запроса,
        ///     или частичное представление для AJAX-запроса.
        /// </summary>
        /// <param name="model">Экземпляр модели представления.</param>
        /// <returns>
        ///     Экземпляр <see cref="Microsoft.AspNetCore.Mvc.ViewResult" /> или
        ///     <see cref="Microsoft.AspNetCore.Mvc.PartialViewResult" />.
        /// </returns>

        protected IActionResult AutoView(
            object model)
        {
            if (this.Request.IsAjaxRequest()) return PartialView(model);
            return View(model);
        }

        /// <summary>
        ///     Вернуть результат автоматического выбора представления: полное представление для обычного запроса,
        ///     или частичное представление для AJAX-запроса.
        /// </summary>
        /// <param name="viewName">Наименование представления.</param>
        /// <param name="model">Экземпляр модели представления.</param>
        /// <returns>
        ///     Экземпляр <see cref="Microsoft.AspNetCore.Mvc.ViewResult" /> или
        ///     <see cref="Microsoft.AspNetCore.Mvc.PartialViewResult" />.
        /// </returns>
        protected IActionResult AutoView(
            string viewName,
            object model)
        {
            if (this.Request.IsAjaxRequest()) return PartialView(viewName, model);
            return View(viewName, model);
        }
    }
}