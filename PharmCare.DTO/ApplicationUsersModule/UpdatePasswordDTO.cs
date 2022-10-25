

namespace PharmCare.DTO.ApplicationUsersModule
{
    public class UpdatePasswordDTO
    {

        public string CurrentPassword { get; set; }
        public string ConfirmPassword { get; set; }
        public string NewPassword { get; set; }

    }
}
