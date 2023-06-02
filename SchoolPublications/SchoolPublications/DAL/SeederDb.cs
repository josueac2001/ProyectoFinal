using Microsoft.EntityFrameworkCore;
using SchoolPublications.DAL.Entities;
using SchoolPublications.Enums;
using SchoolPublications.Helpers;

namespace SchoolPublications.DAL
{
    public class SeederDb
    {
        private readonly DatabaseContext _context;          //Accesos y conexiones
        private readonly IUserHelper _userHelper;
        //private readonly IAzureBlobHelper _azureBlobHelper;

        public SeederDb(DatabaseContext context, IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;
            //_azureBlobHelper = azureBlobHelper;
        }

        public async Task SeederAsync()
        {
            await _context.Database.EnsureCreatedAsync();       //Patrón de diseño para crear la base de datos
            await PopulateCategoriesAsync();
            await PopulatePublicationsAsync();
            await PopulateRolesAsync();
            await PopulateUserAsync("Leo", "Dicrapio", "leo.di@example.com", "3052356508", "ABC123", "Leo.png", UserType.Admin);
            await PopulateUserAsync("Kendall", "Jenner", "kendall.jenner@example.com", "3184985640", "abc123", "Kendall.png", UserType.User);

            await _context.SaveChangesAsync();

        } 

        private async Task PopulateCategoriesAsync()
        {
            if (!_context.Categories.Any())
            {
                _context.Categories.Add(new Category { Name = "Novela", Description = "Historia ficticia" });
                _context.Categories.Add(new Category { Name = "Terror", Description = "Miedo, angustia o suspenso" });
                _context.Categories.Add(new Category { Name = "Acción", Description = "Combate, persecuciones y situaciones de alto riesgo" });
                _context.Categories.Add(new Category { Name = "Drama", Description = "Conflictos emocionales y personales" });
            }
        }

        private async Task PopulatePublicationsAsync()
        {
            if (!_context.Publications.Any())
            {
                await AddPublicationAsync("Cien años de soledad", "sssssss",  new List<string>() { "Novela", "Drama" }, new List<string>() { "Cien_Anios.png" });
                await AddPublicationAsync("El resplandor", "dfgdfg",  new List<string>() { "Terror" }, new List<string>() { "El_Resplandor.png" });
                await AddPublicationAsync("Misión Imposible: Fallout", "dfgdfg",  new List<string>() { "Acción" }, new List<string>() { "MI_Fallout.png"});
                await AddPublicationAsync("La lista de Schindler", "dfgdfg",  new List<string>() { "Drama" }, new List<string>() { "La_lista.png" });
            }
        }
        private async Task AddPublicationAsync(string title, string description, List<string> categories, List<string> images)
        {
            Publication publication = new()
            {
                Title = title,
                Description = description,
                PublicationDate = DateTime.Now,
                PublicationCategories = new List<PublicationCategory>(),
                PublicationImages = new List<PublicationImage>()
            };

            foreach (string category in categories)
            {
                publication.PublicationCategories.Add(new PublicationCategory { Category = await _context.Categories.FirstOrDefaultAsync(c => c.Name == category) });
            }

            //foreach (string image in images)
            //{
            //    Guid imageId = await _azureBlobHelper.UploadAzureBlobAsync($"{Environment.CurrentDirectory}\\wwwroot\\images\\publications\\{image}", "publications");
            //    publication.PublicationImages.Add(new PublicationImage { ImageId = imageId });
            //}

            _context.Publications.Add(publication);
            await _context.SaveChangesAsync();
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

            //if (user == null)
            //{
            //    Guid imageId = await _azureBlobHelper.UploadAzureBlobAsync
            //        ($"{Environment.CurrentDirectory}\\wwwroot\\images\\users\\{image}", "users");

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
                    //ImageId = imageId
                };

                await _userHelper.AddUserAsync(user, "123456");
                await _userHelper.AddUserToRoleAsync(user, userType.ToString());
            }
        }
    }

