namespace SharedTrip.Servises
{
    using SharedTrip.Models.Trips;
    using SharedTrip.Models.Users;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using static Data.DataConstants;

    public class Validator : IValidator
    {

        public ICollection<string> ValidateUser(RegisterUserFormModel model)
        {
            var errors = new List<string>();

            if (model.Username.Length < UsernameMinLength || model.Username.Length > UsernameMaxLength)
            {
                errors.Add($"Username '{model.Username}' is not valid. It must be between {UsernameMinLength} and {UsernameMaxLength} characters long.");
            }

            if (!Regex.IsMatch(model.Email, UserEmailRegularExpression))
            {
                errors.Add($"Email {model.Email} is not a valid e-mail address.");
            }

            if (model.Password.Length < PasswordMinLength || model.Password.Length > PasswordMaxLength)
            {
                errors.Add($"The provided password is not valid. It must be between {PasswordMinLength} and {PasswordMaxLength} characters long.");
            }

            if (model.Password.Any(x => x == ' '))
            {
                errors.Add($"The provided password cannot contain whitespaces.");
            }

            if (model.Password != model.ConfirmPassword)
            {
                errors.Add($"Password and its confirmation are different.");
            }

            return errors;
        }
        public ICollection<string> ValidateTrip(AddTripFormModel model)
        {
            var errors = new List<string>();

            if (model.StartPoint == null)
            {
                errors.Add($"StartPoint '{model.StartPoint}' is not valid.");
            }

            if (model.EndPoint == null)
            {
                errors.Add($"EndPoint '{model.EndPoint}' is not valid.");
            }

            if (!Uri.IsWellFormedUriString(model.ImagePath, UriKind.Absolute))
            {
                errors.Add($"Image '{model.ImagePath}' is not valid. It must be a valid URL.");
            }

            if (model.Seats < SeatsMinValue || model.Seats > SeatsMaxValue)
            {
                errors.Add($"Seats '{model.Seats}' is not valid. It must be between {SeatsMinValue} and {SeatsMaxValue}");
            }

            if (model.Description == null || model.Description.Length > DescriptionMaxLength)
            {
                errors.Add($"Description '{model.Description}' is not valid. It must be less than {DescriptionMaxLength}");
            }

            return errors;
        }
    }
}