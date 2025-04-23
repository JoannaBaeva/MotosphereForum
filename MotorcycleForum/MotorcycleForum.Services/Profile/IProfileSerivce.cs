using MotorcycleForum.Services.Models.Profile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorcycleForum.Services.Profile
{
    public interface IProfileService
    {
        Task<ProfileViewModel?> GetProfileDetailsAsync(Guid id);
    }

}
