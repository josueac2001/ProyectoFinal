using SchoolPublications.DAL.Entities;
using SchoolPublications.Enums;
using SchoolPublications.Helpers;
using Microsoft.EntityFrameworkCore;

namespace SchoolPublications.DAL
{
        public class SeederDb
        {
            private readonly DatabaseContext _context;
            private readonly IUserHelper _userHelper;
            private readonly IAzureBlobHelper _azureBlobHelper;

            public SeederDb(DatabaseContext context, IUserHelper userHelper, IAzureBlobHelper azureBlobHelper)
            {
                _context = context;
                _userHelper = userHelper;
                _azureBlobHelper = azureBlobHelper;
            }

            public async Task SeederAsync()
            {
                await _context.Database.EnsureCreatedAsync();
                await PopulateRolesAsync();
                await PopulateUserAsync("Steve", "Jobs", "admin@gmail.com", "3002323232","102030", "SteveJobs.png", UserType.Admin);
                await PopulateUserAsync("Bill", "Gates", "bill_gates_admin@yopmail.com", "4005656656", "405060", "BillGates.png", UserType.User);

                await _context.SaveChangesAsync();

            }

            private async Task PopulateCategoriesAsync()
            {
                if (!_context.Categories.Any())
                {
                    _context.Categories.Add(new Category { Name = "Tecnología", Description = "Elementos tech", CreatedDate = DateTime.Now });
                    _context.Categories.Add(new Category { Name = "Implementos de Aseo", Description = "Detergente, jabón, etc.", CreatedDate = DateTime.Now });
                    _context.Categories.Add(new Category { Name = "Ropa interior", Description = "Tanguitas, narizonas", CreatedDate = DateTime.Now });
                    _context.Categories.Add(new Category { Name = "Gamers", Description = "PS5, XBOX SERIES", CreatedDate = DateTime.Now });
                    _context.Categories.Add(new Category { Name = "Mascotas", Description = "Concentrado, jabón para pulgas.", CreatedDate = DateTime.Now });
                }
            }

            private async Task PopulateRolesAsync()
            {
                await _userHelper.CheckRoleAsync(UserType.Admin.ToString());
                await _userHelper.CheckRoleAsync(UserType.User.ToString());
             //   await _userHelper.CheckRoleAsync(UserType.Student.ToString());
            }

            private async Task PopulateUserAsync(
                string firstName,
                string lastName,
                string email,
                string phone,
                string document,
                string image,
                UserType userType)
            {
                User user = await _userHelper.GetUserAsync(email);

                if (user == null)
                {
                    Guid imageId = await _azureBlobHelper.UploadAzureBlobAsync
                        ($"{Environment.CurrentDirectory}\\wwwroot\\images\\users\\{image}", "users");

                    user = new User
                    {
                        AdmissionDate = DateTime.Now,
                        FirstName = firstName,
                        LastName = lastName,
                        Email = email,
                        UserName = email,
                        PhoneNumber = phone,
                        Document = document,
                        UserType = userType,
                        ImageId = imageId
                    };

                    await _userHelper.AddUserAsync(user, "123456");
                    await _userHelper.AddUserToRoleAsync(user, userType.ToString());
                }
            }  
        }
    }
