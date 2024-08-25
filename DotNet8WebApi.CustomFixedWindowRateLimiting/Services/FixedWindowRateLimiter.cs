namespace DotNet8WebApi.CustomFixedWindowRateLimiting.Services
{
    public class FixedWindowRateLimiter
    {
        private readonly int _limit;
        private readonly TimeSpan _windowSize;
        private readonly ConcurrentDictionary<string, (int count, DateTime windowStart)> _clients;

        public FixedWindowRateLimiter(int limit, TimeSpan windowSize)
        {
            _limit = limit;
            _windowSize = windowSize;
            _clients = new ConcurrentDictionary<string, (int, DateTime)>();
        }

        public bool IsRequestAllowed(string clientId)
        {
            var currentTime = DateTime.UtcNow;

            var clientData = _clients.GetOrAdd(clientId, (0, currentTime));

            if (currentTime >= clientData.windowStart + _windowSize)
            {
                // New window, reset the counter
                clientData = (0, currentTime);
            }

            if (clientData.count < _limit)
            {
                // Allow the request and increment the counter
                _clients[clientId] = (clientData.count + 1, clientData.windowStart);
                return true;
            }

            // Request limit exceeded
            return false;
        }
    }
}
