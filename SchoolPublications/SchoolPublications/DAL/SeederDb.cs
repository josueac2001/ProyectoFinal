using SchoolPublications.DAL.Entities;
using SchoolPublications.Enums;
using SchoolPublications.Helpers;

namespace SchoolPublications.DAL
{
        public class SeederDb
        {
            private readonly DatabaseContext _context;          //Accesos y conexiones
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
                await _context.Database.EnsureCreatedAsync();       //Patrón de diseño para crear la base de datos
                await PopulateCategoriesAsync();
                await PopulatePublicationsAsync();
                await PopulateRolesAsync();
                await PopulateUserAsync("Steve", "Jobs", "admin@gmail.com", "3002323232","102030", "SteveJobs.png", UserType.Admin);
                await PopulateUserAsync("Bill", "Gates", "bill_gates_admin@yopmail.com", "4005656656", "405060", "BillGates.png", UserType.User);

                await _context.SaveChangesAsync();

            }

            private async Task PopulateCategoriesAsync()
            {
                if (!_context.Categories.Any())
                {
                    _context.Categories.Add(new Category { Name = "Romance", Description = "Elementos tech"});
                    _context.Categories.Add(new Category { Name = "Acción", Description = "Detergente, jabón, etc." });
                    _context.Categories.Add(new Category { Name = "Ropa interior", Description = "Tanguitas, narizonas" });
                    _context.Categories.Add(new Category { Name = "Gamers", Description = "PS5, XBOX SERIES" });
                    _context.Categories.Add(new Category { Name = "Mascotas", Description = "Concentrado, jabón para pulgas." });
                }
            }

            private async Task PopulatePublicationsAsync()
            {
                if (!_context.Publications.Any())
                {
                _context.Publications.Add(new Publication
                    {
                        new Publication
                        {
                            Title = "Publicación 1",
                            Content = "Contenido de la publicación 1",
                            CreatedDate = DateTime.Now,
                            Comments = new List<Comment>
                            {
                                new Comment { Text = "¡Gran publicación!", CreatedDate = DateTime.Now },
                                new Comment { Text = "Me encanta este contenido.", CreatedDate = DateTime.Now }
                            }
                        },
                        new Publication
                        {
                            Title = "Publicación 2",
                            Content = "Contenido de la publicación 2",
                            CreatedDate = DateTime.Now,
                            Comments = new List<Comment>
                            {
                                new Comment { Text = "Muy interesante.", CreatedDate = DateTime.Now }
                            }
                        },
                        new Publication
                        {
                            Title = "Publicación 3",
                            Content = "Contenido de la publicación 3",
                            CreatedDate = DateTime.Now,
                            Comments = new List<Comment>
                            {
                                new Comment { Text = "Grandioso, es el mejor contenido que he leído.", CreatedDate = DateTime.Now }
                                new Comment { Text = "Que agradable texto.", CreatedDate = DateTime.Now }
                                new Comment { Text = "Potaxio.", CreatedDate = DateTime.Now }
                            }
                        }
                    });

                    await _context.Publications.AddRangeAsync(publications);
                    await _context.SaveChangesAsync();
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
                        //AdmissionDate = DateTime.Now,
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
