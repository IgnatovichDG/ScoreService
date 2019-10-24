using Microsoft.AspNetCore.Http;

namespace ScoreService.ViewModel
{
    public class DataLoadFileModel
    {
        public FileLoadType FileType { get; set; }

        public IFormFile File { get; set; }

        public string ProtectionString { get; set; }
    }
}