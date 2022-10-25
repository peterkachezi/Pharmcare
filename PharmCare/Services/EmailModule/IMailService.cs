
using PharmCare.DTO.ApplicationUsersModule;

namespace PharmCare.Services.EmailModule
{
    public interface IMailService
    {

        bool PasswordResetEmailNotification(ResetPasswordDTO resetPasswordDTO);
        bool AccountEmailNotification(ApplicationUserDTO applicationUserDTO);
  
    }
}