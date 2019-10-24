using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ClosedXML.Excel;
using Microsoft.EntityFrameworkCore;
using ScoreService.Dto;
using ScoreService.Entities;
using ScoreService.Infrastructure;

namespace ScoreService.Services
{
    public interface IExportServie
    {
        Task<FileExportDto> GetReportAsync();
    }

    public class ExportService : IExportServie
    {
        private readonly SSDbContext _dbContext;

        public ExportService(SSDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<FileExportDto> GetReportAsync()
        {
            var workbook = new XLWorkbook();

            var worksheet = workbook.Worksheets.Add("Отчёт по оценкам.");

            var data = await _dbContext.Set<TeamEntity>()
                .Where(p => p.IsScored)
                .Select(p => new
                {
                    UserId = p.User.Id,
                    UserName = p.User.Login,
                    TeamName = p.Name,
                    Scores = p.Score.Select(t => new
                    {
                        t.Category.Id,
                        t.Category.Name,
                        Value = t.Score
                    }).OrderBy(t => t.Id).ToList()
                }).OrderBy(p => p.UserId).ToListAsync();

            #region Make Header

            worksheet.Cell(1, 1).Value = "Имя пользователя";
            StyleHeaderCell(worksheet.Cell(1, 1));
            worksheet.Cell(1, 2).Value = "Название команды";
            StyleHeaderCell(worksheet.Cell(1, 2));
            var headerIndex = 3;
            foreach (var score in data.First().Scores)
            {
                worksheet.Cell(1, headerIndex).Value = score.Name;
                StyleHeaderCell(worksheet.Cell(1, headerIndex));
                headerIndex++;
            }
            #endregion

            var row = 2;
            foreach (var record in data)
            {
                worksheet.Cell(row, 1).Value = record.UserName;
                StyleCell(worksheet.Cell(row, 1));
                worksheet.Cell(row, 2).Value = record.TeamName;
                StyleCell(worksheet.Cell(row, 2));         
                var contentIndex = 3;
                foreach (var score in record.Scores)
                {
                    worksheet.Cell(row, contentIndex).Value = score.Value;
                    StyleCell(worksheet.Cell(row, contentIndex));
                    contentIndex++;
                }

                row++;
            }

            worksheet.Columns().AdjustToContents();

            using (var ms = new MemoryStream())
            {
                workbook.SaveAs(ms);
                ms.Seek(0, SeekOrigin.Begin);

                var content = ms.ToArray();
                return new FileExportDto()
                {
                    Name = $"ScoreReport-{DateTime.Now}",
                    Content = content
                };
            }


        }

        private void StyleHeaderCell(IXLCell cell)
        {
            cell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            cell.Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            cell.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            cell.Style.Fill.BackgroundColor = XLColor.Gray;
            cell.Style.Font.Bold = true;
        }

        private void StyleCell(IXLCell cell)
        {
            cell.Style.Border.SetOutsideBorder(XLBorderStyleValues.Hair);
            cell.Style.Border.SetOutsideBorderColor(XLColor.Gray);
            cell.Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            cell.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        }
    }
}