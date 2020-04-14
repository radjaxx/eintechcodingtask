using System;
using core.Interfaces;

namespace core.Common
{
    public class ApplicationConfiguration : IApplicationConfiguration
    {
        public string DbConnectionString { get; set; }
    }
}
