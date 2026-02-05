using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Business.Interfaces
{
    public interface ICurrentUserService
    {
        int GetUserId();
        bool IsAdmin();
        bool IsAuthenticated();
    }
}
