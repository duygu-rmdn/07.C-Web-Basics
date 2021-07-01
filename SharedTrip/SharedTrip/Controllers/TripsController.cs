namespace SharedTrip.Controllers
{
    using MyWebServer.Controllers;
    using MyWebServer.Http;
    using SharedTrip.Data;
    using SharedTrip.Models;
    using SharedTrip.Models.Trips;
    using SharedTrip.Servises;
    using System;
    using System.Linq;

    public class TripsController : Controller
    {
        private readonly IValidator validator;
        private readonly ApplicationDbContext data;

        public TripsController(
            IValidator validator,
            ApplicationDbContext data)
        {
            this.validator = validator;
            this.data = data;
        }

        public HttpResponse All()
        {
            var tripsQuery = this.data
                .Trips
                .AsQueryable();

            var trips = tripsQuery
                .Select(t => new TripsListingViewModel
                {
                    Id = t.Id,
                    StartPoint = t.StartPoint,
                    EndPoint = t.EndPoint,
                    DepartureTime = t.DepartureTime,
                    Seats = t.Seats,
                    Description = t.Description
                }).ToList();

            return View(trips);
        }

        public HttpResponse Add() => View();

        [HttpPost]
        public HttpResponse Add(AddTripFormModel model)
        {
            var errors = this.validator.ValidateTrip(model).ToList();

            if (errors.Any())
            {
                //return Error(errors);
                return Redirect("/Trips/Add");
            }

            var trip = new Trip
            {
                StartPoint = model.StartPoint,
                EndPoint = model.EndPoint,
                DepartureTime = DateTime.Parse(model.DepartureTime),
                ImagePath = model.ImagePath,
                Seats = model.Seats,
                Description = model.Description
            };

            data.Trips.Add(trip);
            data.SaveChanges();

            return Redirect("/Trips/All");
        }
        [Authorize]
        public HttpResponse Details()
        {
            var tripsQuery = this.data
                .Trips
                .AsQueryable();

            var trip = tripsQuery
                .Select(t => new AddTripFormModel
                {
                    Id = t.Id,
                    StartPoint = t.StartPoint,
                    EndPoint = t.EndPoint,
                    ImagePath = t.ImagePath,
                    DepartureTime = t.DepartureTime.ToString("dd.MM.yyyy HH:mm"),
                    Seats = t.Seats,
                    Description = t.Description
                })
                .FirstOrDefault();

            if (trip == null)
            {
                return Redirect("/Trips/Details");
            }

            return View(trip);
        }

        [Authorize]
        public HttpResponse AddUserToTrip(string tripId)
        {
            var currTrip = data.Trips.Where(x => x.Id == tripId).FirstOrDefault();

            var user = data.Users.Where(x => x.Id == this.User.Id).FirstOrDefault();

            if (currTrip == null || user == null)
            {
                return Redirect("Trips/Details"); 
            }

            var userTrip = new UserTrip
            {
                Trip = currTrip,
                User = user
            };

            if (data.UsersTrips.Where(x => x.UserId == user.Id && x.Trip.Id == currTrip.Id).Any())
            {
                return Redirect($"/Trips/Details?tripId={currTrip.Id}");
            }

            currTrip.UserTrips.Add(userTrip);

            user.UserTrips.Add(userTrip);

            data.UsersTrips.Add(userTrip);

            data.SaveChanges();

            return Redirect("/Trips/All");
        }
    }
}