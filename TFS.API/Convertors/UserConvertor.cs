using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TFS.API.Data;

namespace TFS.API.Convertors
{
    public static class UserConvertor
    {
        public static UserDto Convert(this User entity)
        {
            if(entity == null)
            {
                return null;
            }

            var result = new UserDto();

            result.Guid = entity.Guid;
            result.FirstName  = entity.FirstName;
            result.LastName  = entity.LastName;
            result.Email  = entity.Email;
            result.Password  = entity.Password;

            return result;
        }

        public static User Convert(this UserDto dto)
        {
            if(dto == null)
            {
                return null;
            }

            var result = new User();

            result.Guid = dto.Guid;
            result.FirstName  = dto.FirstName;
            result.LastName  = dto.LastName;
            result.Email  = dto.Email;
            result.Password  = dto.Password;

            return result;
        }
    }
}
