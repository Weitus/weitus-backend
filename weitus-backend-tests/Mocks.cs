using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

namespace weitus_backend_tests;

class MockConfiguration : IConfiguration
{
    public string this[string key]
    {
        get
        {
            switch (key)
            {
                case "Jwt:Issuer":
                    return "http://localhost:5000";
                case "Jwt:Audience":
                    return "http://localhost:5000";
                case "Jwt:Key":
                    return "0123456789ABCDEF0123456789ABCDEF";
                case "Chat:Secret":
                    return "0123456789ABCDEF0123456789ABCDEF";
                case "Encryption:Key":
                    return "0123456789ABCDEF0123456789ABCDEF";
                case "Encryption:IV":
                    return "0123456789ABCDEF0123456789ABCDEF";
                default:
                    return "";
            }
        }
        set => throw new System.NotImplementedException();
    }

    public IEnumerable<IConfigurationSection> GetChildren()
    {
        throw new System.NotImplementedException();
    }

    public IChangeToken GetReloadToken()
    {
        throw new System.NotImplementedException();
    }

    public IConfigurationSection GetSection(string key)
    {
        throw new System.NotImplementedException();
    }
}
