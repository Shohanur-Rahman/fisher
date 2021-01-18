using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Core.ViewService.Models;
using Services.Domain;

namespace Services.Domain.Models.User.EditorModel
{
    public class UserProfileModel : BaseEditorModel
    {

        public PersonalInformation PersonalInfo { get; set; }
        public UserPassword UserPassword { get; set; }
       
    }
}
