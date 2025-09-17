using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginService.LoginService
{
    public interface ILoginService
    {
        public Guid UserId { get; }
        public string UserName { get; }
    }
}
