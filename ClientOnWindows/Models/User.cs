
using System.ComponentModel.DataAnnotations.Schema;

namespace ClientOnWindows.Models
{
    public class User
    {

        public int Id { get;  set; }
        public string UserName { get;  set; }
        public string Password { get;  set; }
        [NotMapped]
        public string RePassword { get;  set; }
        public string FullName { get;  set; }
        public string FName { get;  set; }

        public string ProfilePhoto { get;  set; }
        [NotMapped]
        //public IFormFile? ProfilePhotoFile { get; set; }

        public int RoleId { get; set; }
        //public Role Role { get; set; }

        public User()
        {
            
        }

        public User(string userName, string password, string fullName, string fName, string profilePhoto
           , int roleId)
        {
            UserName = userName;
            Password = password;
            FullName = fullName;
            FName = fName;
            ProfilePhoto = profilePhoto;
            RoleId = roleId;
        }





        public void Edit(string userName, string password, string fullName, string fName, string profilePhoto, int roleId)
        {
            UserName = userName;
            FullName = fullName;
            FName = fName;

            //RoleId = roleId;

            if (!string.IsNullOrWhiteSpace(profilePhoto))
                ProfilePhoto = profilePhoto;
            if (!string.IsNullOrWhiteSpace(password))
                Password = password;
        }

    }
}
