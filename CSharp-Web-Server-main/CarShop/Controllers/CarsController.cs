using CarShop.Data;
using CarShop.Data.Models;
using CarShop.Models.Cars;
using CarShop.Services;
using MyWebServer.Controllers;
using MyWebServer.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarShop.Controllers
{
    public class CarsController : Controller
    {
        private readonly IValidator validator;
        private readonly IUserService users;
        private readonly CarShopDbContext data;

        public CarsController(
            IValidator validator,
            IUserService users,
            CarShopDbContext data)
        {
            this.validator = validator;
            this.users = users;
            this.data = data;
        }

        [Authorize]
        public HttpResponse All()
        {
            var carsQuery = data
                .Cars
                .AsQueryable();

            if (this.users.IsMechanic(this.User.Id))
            {
                carsQuery = carsQuery
                    .Where(x => x.Issues.Any(c => !c.IsFixed));
            }
            else
            {
                carsQuery = carsQuery
                    .Where(x => x.OwnerId == this.User.Id);
            }

            var cars = carsQuery
                .Select(x => new CarListingViewModel
                {
                    Id = x.Id,
                    Model = x.Model,
                    Year = x.Year,
                    Image = x.PictureUrl,
                    PlateNumber = x.PlateNumber,
                    RemainingIssues = x.Issues.Count(a => !a.IsFixed),
                    FixedIssues = x.Issues.Count(a => a.IsFixed)
                }).ToList();

            return View(cars);
        }

        [Authorize]
        public HttpResponse Add()
        {
            if (this.users.IsMechanic(this.User.Id))
            {
                return Error("Mechanics cannot add cars.");
            }

            return View();
        }

        [Authorize]
        [HttpPost]
        public HttpResponse Add(AddCarFormModel model)
        {
            var modelErrors = this.validator.ValidateCar(model);

            if (modelErrors.Any())
            {
                return Error(modelErrors);
            }

            var car = new Car
            {
                Model = model.Model,
                Year = model.Year,
                PictureUrl = model.Image,
                PlateNumber = model.PlateNumber,
                OwnerId = this.User.Id
            };

            data.Cars.Add(car);

            data.SaveChanges();

            return Redirect("/Cars/All");
        }
    }
}
