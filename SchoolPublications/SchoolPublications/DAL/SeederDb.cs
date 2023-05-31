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
                await PopulateUserAsync("John", "Doe", "john.doe@example.com", "123456789", "123 Main St", "ABC123", "john-doe.png", UserType.User);
                await PopulateUserAsync("Jane", "Smith", "jane.smith@example.com", "987654321", "456 Elm St", "XYZ789", "jane-smith.png", UserType.User);

            await _context.SaveChangesAsync();

            }

            private async Task PopulateCategoriesAsync()
            {
                if (!_context.Categories.Any())
                {
                    _context.Categories.Add(new Category { Name = "Novela", Description = "Historia ficticia" , CreatedDate = DateTime.Now });
                    _context.Categories.Add(new Category { Name = "Terror", Description = "Miedo, angustia o suspenso" , CreatedDate = DateTime.Now });
                    _context.Categories.Add(new Category { Name = "Acción", Description = "Combate, persecuciones y situaciones de alto riesgo" , CreatedDate = DateTime.Now });
                    _context.Categories.Add(new Category { Name = "Drama", Description = "Conflictos emocionales y personales" , CreatedDate = DateTime.Now });
                }
            }

            private async Task PopulatePublicationsAsync()
            {
                if (!_context.Products.Any())
                {
                    await AddPublicationsAsync("Medias Grises", 270000M, 12F, new List<string>() { "Ropa Interior", "Calzado" }, new List<string>() { "Medias1.png" });
                    await AddProductAsync("Medias Negras", 300000M, 12F, new List<string>() { "Ropa Interior", "Calzado" }, new List<string>() { "Medias2.png" });
                    await AddProductAsync("TV Samsung OLED", 5000000M, 12F, new List<string>() { "Tecnología", "Gamers" }, new List<string>() { "TvOled.png", "TvOled2.png" });
                    await AddProductAsync("Play Station 5", 5000000M, 12F, new List<string>() { "Gamers" }, new List<string>() { "PS5.png", "PS52.png" });
                    await AddProductAsync("Bull Dog Francés", 10000000M, 12F, new List<string>() { "Mascotas" }, new List<string>() { "Frenchie1.png", "Frenchie2.png", "Frenchie3.png" });
                    await AddProductAsync("Cepillo de dientes", 5000M, 12F, new List<string>() { "Implementos de Aseo" }, new List<string>() { "CepilloDientes.png" });
                    await AddProductAsync("Crema dental Pro Alivio", 25000M, 12F, new List<string>() { "Implementos de Aseo" }, new List<string>() { "CremaDental1.png", "CremaDental2.png" });
                }
            }
        private async Task PopulatePublicationsAsync()
            {
                if (!_context.Publications.Any())
                {
                    _context.Publications.Add(new Publication
                    {
                        Title = "Cien años de soledad",
                        Content = "La historia épica de la familia Buendía a lo largo de varias generaciones, llena de amor, tragedia y realismo mágico.",
                        CreatedDate = DateTime.Now,
                        Comments = new List<Comment>()
                        {
                            new Comment { Text = "¡Una novela fascinante!", CreatedDate = DateTime.Now },
                            new Comment { Text = "La narrativa es increíble.", CreatedDate = DateTime.Now }
                            new Comment { Text = "No tengo palabras para expresar lo increíble que esta novela.", CreatedDate = DateTime.Now }
                            new Comment { Text = "LMe asombra el caracter de estos personajes", CreatedDate = DateTime.Now }
                                 
                        }
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
