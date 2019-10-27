using System;
using System.Collections.Generic;
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
    public class ExportService : IExportServie
    {
        private readonly SSDbContext _dbContext;

        public ExportService(SSDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public FileExportDto GetResults(byte[] content)
        {
            using (var ms = new MemoryStream(content))
            {
                var worsheet = new XLWorkbook(ms).Worksheet(1);
                var data = worsheet.RowsUsed().Select(row => (row.Cell(1).GetValue<string>(), row.Cell(2).GetValue<double>())).ToList();
            

                var result = new Dictionary<string, List<double>>();

                foreach (var tuple in data)
                {
                    if (result.ContainsKey(tuple.Item1)) 
                        result[tuple.Item1].Add(tuple.Item2);
                    else
                    {
                        result.Add(tuple.Item1, new List<double>(){tuple.Item2});
                    }
                }


                var resultWorkbook = new XLWorkbook();
                var resultWorksheet = resultWorkbook.Worksheets.Add("Отчёт по оценкам.");


                var rowIndex = 1;
                foreach (var record in result)
                {
                    resultWorksheet.Cell(rowIndex, 1).Value = record.Key;
                    resultWorksheet.Cell(rowIndex, 2).Value = record.Value.Average();
                    rowIndex++;
                }

                resultWorksheet.Columns().AdjustToContents();

                using (var ms2 = new MemoryStream())
                {
                    resultWorkbook.SaveAs(ms2);
                    ms2.Seek(0, SeekOrigin.Begin);

                    var resultContent = ms2.ToArray();
                    return new FileExportDto()
                    {
                        Name = $"ScoreTotal-{DateTime.Now}",
                        Content = resultContent
                    };
                }
            }
        }

        public async Task<FileExportDto> GetReportAsync()
        {
            var workbook = new XLWorkbook();

            var worksheet = workbook.Worksheets.Add("Отчёт по оценкам.");

            var data = await _dbContext.Set<UserTeamRelation>()
                .Where(p=>p.IsScored)
                .Select(p => new
                {
                    UserId = p.User.Id,
                    TeamAddress= p.Team.Address,
                    UserName = p.User.Login,
                    TeamName = p.Team.Name,
                    Scores = p.Team.Score.Select(t => new
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
            worksheet.Cell(1, 3).Value = "Адрес команды";
            StyleHeaderCell(worksheet.Cell(1, 3));
            var headerIndex = 4;
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
                worksheet.Cell(row, 3).Value = record.TeamAddress;
                StyleCell(worksheet.Cell(row, 3));
                var contentIndex = 4;
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

        public async Task<FileExportDto> GetUserTeamsBinds()
        {
            var workbook = new XLWorkbook();

            var worksheet = workbook.Worksheets.Add("Связка юзер - команда");

            var data =  await _dbContext.Set<UserTeamRelation>().Select(p => new {p.User.Login, p.Team.Name, p.Team.Address}).ToListAsync();

            #region Make Header

            worksheet.Cell(1, 1).Value = "Имя пользователя";
            StyleHeaderCell(worksheet.Cell(1, 1));
            worksheet.Cell(1, 2).Value = "Адрес команды";
            StyleHeaderCell(worksheet.Cell(1, 2));
            worksheet.Cell(1, 3).Value = "Название команды";
            StyleHeaderCell(worksheet.Cell(1, 3));
            #endregion

            var row = 2;
            foreach (var record in data)
            {
                worksheet.Cell(row, 1).Value = record.Login;
                StyleCell(worksheet.Cell(row, 1));
                worksheet.Cell(row, 2).Value = record.Address;
                StyleCell(worksheet.Cell(row, 2));
                worksheet.Cell(row, 3).Value = record.Name;
                StyleCell(worksheet.Cell(row, 3));
            
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
                    Name = $"UserTeam-{DateTime.Now}",
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