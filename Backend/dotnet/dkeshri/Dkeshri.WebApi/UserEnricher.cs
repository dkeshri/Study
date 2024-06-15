using Serilog.Core;
using Serilog.Events;

namespace Dkeshri.WebApi
{
    public class UserEnricher : ILogEventEnricher
    {
        private readonly string _userName;
        private readonly int _userId;

        public UserEnricher(string userName, int userId)
        {
            _userName = userName;
            _userId = userId;
        }

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("UserName", _userName));
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("UserId", _userId));
        }
    }
}
