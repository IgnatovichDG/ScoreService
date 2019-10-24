using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ScoreService.Services;
using ScoreService.ViewModel;

namespace ScoreService.Controllers
{
    [AllowAnonymous]
    [Route("import")]
    public class ImportController : ControllerBase
    {
        private IFileUploadService _fileUploadService;

        public ImportController(IFileUploadService fileUploadService)
        {
            _fileUploadService = fileUploadService;
        }

        [HttpGet("upload")]
        public async Task<IActionResult> Upload(FileLoadType type)
        {
            return AutoView(new DataLoadFileModel()
            {
                FileType = type
            });
        }

        [HttpGet("getReport")]
        public Task<IActionResult> GetReport()
        {
            return null;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload(DataLoadFileModel model)
        {
            var ct = HttpContext.RequestAborted;
            if (model.ProtectionString != "AlexeyKrasava")
            {
                ModelState.AddModelError(nameof(model.ProtectionString), "Код защиты введён неверно");
            }
            if (model.File.ContentType != "application/vnd.ms-excel"
                && model.File.ContentType != "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                ModelState.AddModelError(nameof(model.File), "Недопустимый формат файла.");
            }
            if (model.FileType == FileLoadType.Unknown)
            {
                ModelState.AddModelError(nameof(model.FileType), "Выбран неизвестный тип загружаемого файла.");
            }
            if (ModelState.IsValid)
            {

                try
                {
                    byte[] fileContent;
                    using (var ms = new MemoryStream())
                    {
                        await model.File.CopyToAsync(ms, ct);
                        fileContent = ms.ToArray();
                    }
                    await _fileUploadService.UploadAsync(model.FileType, fileContent);
                    var result = new { status = "Данные успешно загружены!" };
                    return Ok(result);
                }
                catch (Exception)
                {
                    ModelState.AddModelError(nameof(model.File), "При попытке обработать файл что-то пошло не так. Проверьте содержание файла и попробуйте загрузить его снова.");
                    var result = new { status = "Что-то пошло не так!" };
                    return Ok(result);
                }
            }

            return AutoView(model);
        }

    }
}