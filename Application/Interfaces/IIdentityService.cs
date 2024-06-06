using Domain.BusinessModels;

namespace Application.Interfaces
{
    public interface IIdentityService
    {
        public bool RegisterNewAdmin(AppUser input);

        public bool RegisterNewModerator(AppUser input);

        public string Login(AppUser input);
    }
}
