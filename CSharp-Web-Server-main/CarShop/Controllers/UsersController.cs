using CarShop.Data;
using CarShop.Data.Models;
using CarShop.Models.Users;
using CarShop.Services;
using MyWebServer.Controllers;
using MyWebServer.Http;
using System.Linq;

namespace CarShop.Controllers
{
    public class UsersController : Controller
    {
        private readonly IValidator validator;
        private readonly CarShopDbContext data;
        private readonly IPasswordHasher passwordHasher;

        public UsersController(IValidator validator, CarShopDbContext data, IPasswordHasher passwordHasher)
        {
            this.validator = validator;
            this.data = data;
            this.passwordHasher = passwordHasher;
        }
        public HttpResponse Register() => View();

        [HttpPost]
        public HttpResponse Register(RegisterUserFormModel model)
        {
            var modelErrors = this.validator.ValidateRegistration(model);

            if (this.data.Users.Any(u => u.Username == model.Username))
            {
                modelErrors.Add($"User with '{model.Username}' username already exists.");
            }

            if (this.data.Users.Any(u => u.Email == model.Email))
            {
                modelErrors.Add($"User with '{model.Email}' e-mail already exists.");
            }

            if (modelErrors.Any())
            {
                return Error(modelErrors);
            }

            var user = new User
            {
                Username = model.Username,
                Password = this.passwordHasher.HashPassword(model.Password),
                Email = model.Email,
                IsMechanic = model.UserType == "Mechanic"
            };

            data.Users.Add(user);
            data.SaveChanges();


            return Redirect("/Users/Login");
        }

        public HttpResponse Login() => View();

        [HttpPost]
        public HttpResponse Login(LoginFormModel model)
        {
            var hashedPassword = this.passwordHasher.HashPassword(model.Password);

            var userId = this.data
                .Users
                .Where(x => x.Username == model.Username && x.Password == hashedPassword)
                .Select(c => c.Id)
                .FirstOrDefault();

            if (userId == null)
            {
                return Error("Username and password combination is not valid");
            }

            this.SignIn(userId);

            return Redirect("/Cars/All");
        }

        [Authorize]
        public HttpResponse Logout()
        {
            this.SignOut();

            return Redirect("/");
        }
    }
}
