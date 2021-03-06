using HotelResFE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace HotelResFE.DataServices
{
    public interface IUserService
    {
        public Task<string> LoginAsync(LoginCreds creds);
        public Task<LoginCreds> RegisterNewUserAsync(User user);
        public Task<User> GetUserAsync();
        public Task<HttpStatusCode?> DeleteUserAsync();
        public Task EditUserAsync(User user);
        public void LogOut();
    }
}
