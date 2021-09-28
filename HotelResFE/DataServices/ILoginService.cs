using HotelResFE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace HotelResFE.DataServices
{
    public interface ILoginService
    {
        public Task<string> LoginAsync(LoginCreds creds);
    }
}
