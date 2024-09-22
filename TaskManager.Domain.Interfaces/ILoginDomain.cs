using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Entities.Users;

namespace TaskManager.Domain.Interfaces
{
    public interface ILoginDomain
    {
        Task<UserResponse?> Login(UserRequest request);
    }
}
