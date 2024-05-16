using System.Security.Claims;

namespace ClientOnWindows.Models
{
    public class LoginResult
    {
        public bool Success { get; set; }
        public string TokenString { get; set; }
        public List<ClaimModel> claims { get; set; } = new List<ClaimModel>();

        public LoginResult()
        {
            
        }

    }
}
