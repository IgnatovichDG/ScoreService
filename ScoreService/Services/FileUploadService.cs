using System;
using System.IO;
using System.Threading.Tasks;
using ClosedXML.Excel;
using Microsoft.EntityFrameworkCore;
using ScoreService.Dto;
using ScoreService.Entities;
using ScoreService.Infrastructure;
using ScoreService.ViewModel;

namespace ScoreService.Services
{
    public interface IFileUploadService
    {
        Task UploadAsync(FileLoadType fileLoadType, byte[] content);
    }

    public class FileUploadService : IFileUploadService
    {
        private readonly IUserService _userService;
        private readonly SSDbContext _context;
        

        public FileUploadService(IUserService userService, SSDbContext context)
        {
            _userService = userService;
            _context = context;
        }

        public async Task UploadAsync(FileLoadType fileLoadType, byte[] content)
        {
            switch (fileLoadType)
            {
                case FileLoadType.Users:
                    await UploadUsersAsync(content);
                    break;
                case FileLoadType.UserTeam:
                    await UploadTeamsAsync(content);
                    break;
                case FileLoadType.Categories:
                    await UploadCategoriesAsync(content);
                    break;                   
                default:
                    throw new ArgumentOutOfRangeException(nameof(fileLoadType), fileLoadType, null);
            }
        }

        private async Task UploadCategoriesAsync(byte[] content)
        {
            using (var ms = new MemoryStream(content))
            {
                var worsheet = new XLWorkbook(ms).Worksheet(1);
                foreach (var row in worsheet.RowsUsed())
                {
                    if (row.RowNumber() == 1)
                        continue;
                    var categoryDto = new ScoreCategoryDto()
                    {
                        Name = row.Cell(1).GetValue<string>(),
                        Alias = row.Cell(2).GetValue<string>(),
                        Kind = (ScoreCategoryKind)row.Cell(3).GetValue<int>(),
                        IsDeleted = row.Cell(4).GetValue<string>() == "Y"
                    };

                    var categoryEntity = await _context.Set<ScoreCategoryEntity>().FirstOrDefaultAsync(p => p.Alias == categoryDto.Alias);
                    if (categoryEntity != null)
                    {
                        if (categoryDto.IsDeleted)
                        {
                            _context.Remove(categoryEntity);
                            await _context.SaveChangesAsync();
                        }
                        else
                        {
                            categoryEntity.Kind = categoryDto.Kind;
                            await _context.SaveChangesAsync();
                        }
                    }
                    else
                    {
                        await _context.AddAsync(new ScoreCategoryEntity()
                        {
                            Name = categoryDto.Name,
                            Alias = categoryDto.Alias,
                            Kind = categoryDto.Kind                           
                        });
                        await _context.SaveChangesAsync();
                    }
                }
            }
        }

        private async Task UploadUsersAsync(byte[] content)
        {
            using (var ms = new MemoryStream(content))
            {
                var worsheet = new XLWorkbook(ms).Worksheet(1);
                foreach (var row in worsheet.RowsUsed())
                {
                    if(row.RowNumber() == 1)
                        continue;                    
                    var userDto = new UserUploadDto()
                    {
                        Login = row.Cell(1).GetValue<string>(),
                        Password = row.Cell(2).GetValue<string>(),
                        IsDeleted = row.Cell(3).GetValue<string>() == "Y"
                    };

                    var userEntity = await _context.Set<UserEntity>().FirstOrDefaultAsync(p => p.Login == userDto.Login);
                    if (userEntity != null)
                    {
                        if (userDto.IsDeleted)
                        {
                            _context.Remove(userEntity);
                            await _context.SaveChangesAsync();
                        }                            
                        else
                        {
                            await _userService.ChangePassword(userEntity.Login, userDto.Password);                 
                        }
                    }
                    else
                    {
                        await _userService.SaveUser(userDto.Login, userDto.Password);
                    }
                }
            }
        }

        private async Task UploadTeamsAsync(byte[] content)
        {
            using (var ms = new MemoryStream(content))
            {
                var worsheet = new XLWorkbook(ms).Worksheet(1);
                foreach (var row in worsheet.RowsUsed())
                {
                    if (row.RowNumber() == 1)
                        continue;
                    var teamDto = new TeamUploadDto()
                    {
                        Login = row.Cell(1).GetValue<string>(),
                        TeamName = row.Cell(2).GetValue<string>(),
                        IsDeleted = row.Cell(3).GetValue<string>() == "Y"
                    };

                    var userEntity = await _context.Set<UserEntity>().FirstOrDefaultAsync(p => p.Login == teamDto.Login);
                    if (userEntity == null)
                        continue;
                    var teamEntity = await _context.Set<TeamEntity>().FirstOrDefaultAsync(p => p.Name == teamDto.TeamName && p.User.Login == userEntity.Login);
                    if (teamEntity != null)
                    {
                        if (teamDto.IsDeleted)
                        {
                            _context.Remove(teamEntity);
                            await _context.SaveChangesAsync();
                        }
                    }
                    else
                    {
                        await _context.AddAsync(new TeamEntity()
                        {
                            Name = teamDto.TeamName,
                            User = userEntity,
                        });
                        await _context.SaveChangesAsync();
                    }
                }
            }
        }
    }
}