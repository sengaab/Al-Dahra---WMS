using whm.Models;

namespace whm.DTOs
{
    public class AuthRegisterDTO
    {

        public Guid Id { get; set; }
        public string FullName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

       
    }
}
