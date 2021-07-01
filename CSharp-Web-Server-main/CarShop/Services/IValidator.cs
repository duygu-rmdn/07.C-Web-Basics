using CarShop.Models.Cars;
using CarShop.Models.Issue;
using CarShop.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarShop.Services
{
    public interface IValidator
    {
        ICollection<string> ValidateRegistration(RegisterUserFormModel model);

        ICollection<string> ValidateCar(AddCarFormModel model);

        ICollection<string> AddIssueFormModel(AddIssueDespription model);
    }
}
