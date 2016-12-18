using backend.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace chat_server
{
    class Router : IRoute
    {
        private static readonly Router _instance = new Router();

        public static Router Instance
        {
            get
            {
                return _instance;
            }
        }

        private Router()
        {

        }

        private Dictionary<string, Func<string, string>> _routes = new Dictionary<string, Func<string, string>>() { };
        private Dictionary<string, Func<string, string>> _async_routes = new Dictionary<string, Func<string, string>>() { };

        public void AddRoute(string route, Func<string, string> callback)
        {
            _routes.Add(route, callback);
        }

        public void AddAsyncRoute(string route, Func<string, string> callback)
        {
            _async_routes.Add(route, callback);
        }

        public void Route(string route, string parameters)
        {
            if (_routes.ContainsKey(route))
            {
                _routes[route](parameters);
            }
            else
            {
                throw new Exception();
            }
        }

        public async void Route(string route, string parameters, Action<string> routeCallback)
        {
            if (_async_routes.ContainsKey(route))
            {
                var data = await Task.Run(() => {
                    return _async_routes[route](parameters);
                });

                routeCallback(data);
            }
            else
            {
                throw new Exception();
            }
        }
    }
}
