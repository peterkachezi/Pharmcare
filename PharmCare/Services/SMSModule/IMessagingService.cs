
using PharmCare.DTO.ApplicationUsersModule;
using System.Threading.Tasks;

namespace PharmCare.Services.SMSModule
{
    public interface IMessagingService
    {
        Task<ApplicationUserDTO> usersAccount(ApplicationUserDTO applicationUserDTO);
        Task<ResetPasswordDTO> PasswordResetEmailNotification(ResetPasswordDTO resetPasswordDTO);

    }
}