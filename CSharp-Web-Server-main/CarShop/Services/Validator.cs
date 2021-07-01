using CarShop.Models.Cars;
using CarShop.Models.Issue;
using CarShop.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CarShop.Services
{
    class Validator : IValidator
    {
        public ICollection<string> ValidateRegistration(RegisterUserFormModel user)
        {
            var errors = new List<string>();

            if (user.Username == null || user.Username.Length < 4 || user.Username.Length > 20)
            {
                errors.Add($"Username '{user.Username}' is not valid. It must be between {4} and {20} characters long.");
            }

            if (user.Email == null || !Regex.IsMatch(user.Email, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"))
            {
                errors.Add($"Email '{user.Email}' is not a valid e-mail address.");
            }

            if (user.Password == null || user.Password.Length < 5 || user.Password.Length > 20)
            {
                errors.Add($"The provided password is not valid. It must be between {5} and {20} characters long.");
            }

            if (user.Password != null && user.Password.Any(x => x == ' '))
            {
                errors.Add($"The provided password cannot contain whitespaces.");
            }

            if (user.Password != user.ConfirmPassword)
            {
                errors.Add("Password and its confirmation are different.");
            }

            if (user.UserType != "Client" && user.UserType != "Mechanic")
            {
                errors.Add($"The user can be either a 'Client' or a 'Mechanic'.");
            }

            return errors;
        }

        public ICollection<string> ValidateCar(AddCarFormModel car)
        {
            var errors = new List<string>();

            if (car.Model == null || car.Model.Length < 5 || car.Model.Length > 20)
            {
                errors.Add($"Model '{car.Model}' is not valid. It must be between {5} and {20} characters long.");
            }

            if (car.Year < 1900 || car.Year > 2100)
            {
                errors.Add($"Year '{car.Year}' is not valid. It must be between {1900} and {2100}.");
            }

            if (car.Image == null || !Uri.IsWellFormedUriString(car.Image, UriKind.Absolute))
            {
                errors.Add($"Image '{car.Image}' is not valid. It must be a valid URL.");
            }

            if (car.PlateNumber == null || !Regex.IsMatch(car.PlateNumber, @"[A-Z]{2}[0-9]{4}[A-Z]{2}"))
            {
                errors.Add($"Plate number '{car.PlateNumber}' is not valid. It should be in 'XX0000XX' format.");
            }

            return errors;
        }

        public ICollection<string> AddIssueFormModel(AddIssueDespription model)
        {
            var errors = new List<string>();

            if (model.CarId == null)
            {
                errors.Add($"CarId '{model.CarId}' is not valid");
            }
            if (model.Description.Length < 5)
            {
                errors.Add($"Model '{model.Description}' is not valid. It must be more than {5} characters long.");
            }

            return errors;
        }
    }
}