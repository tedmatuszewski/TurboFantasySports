using System;

namespace TFS.API
{
    public class UserDto
    {
        public UserDto()
        {
            this.Guid = Guid.NewGuid();
        }

        public Guid Guid { get; set; }
        
        public string FirstName { get; set; }
        
        public string LastName { get; set; }
        
        public string Email { get; set; }
        
        public string Password { get; set; }

        public string FullName 
        {
            get
            {
                return $"{FirstName} {LastName}";
            }
        }
    }
}