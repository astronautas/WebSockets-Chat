using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace backend.Interfaces
{
    /* Router
     * All callbacks take JSON parameters as single argument.
     * All callbacks return JSON string as single return value.
     */

    interface IRoute
    {
        void Route(string action, string parameters);
        void Route(string action, string parameters, Action<string> routeCallback);
        void AddRoute(string route, Func<string, string> callback);
        void AddAsyncRoute(string route, Func<string, string> callback);
    }
}
