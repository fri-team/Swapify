using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using BlazorClient.Services.API;

namespace BlazorClient.Services
{
    public interface ISwapifyAPI
    {
        IUserEndpoints User { get; }
        IStudentEndpoints Student { get; }
    }
}
