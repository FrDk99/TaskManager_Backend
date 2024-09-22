using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Entities.Users;

namespace TaskManager.Repository.Interfaces
{
    public interface ILoginRepository
    {
        Task<UserResponse?> Login(UserRequest usuario);
    }
}
