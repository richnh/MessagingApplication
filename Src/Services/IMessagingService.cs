using EmailServerService.Responses;
using System.Collections.Generic;

namespace EmailServerService.Services
{
    public interface IMessagingService<T, Y> 
        where T : class
        where Y : class
    {
        Y CreateMessage(string to, T model);

        ValidationResponse SendMessage(List<string> recipients, T model);
    }
}
