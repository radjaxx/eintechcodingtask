using System;
using core.Interfaces;

namespace littlecms.Repositories
{
    public interface IUnitOfWorkManager
    {
        IUnitOfWorkScope CreateScope();
    }


    public class UnitOfWorkManager : IUnitOfWorkManager
        {
            private readonly IApplicationConfiguration _config;
            

            public UnitOfWorkManager(IApplicationConfiguration config)
            {
                _config = config; 
            }

            public IUnitOfWorkScope CreateScope()
            {
                return new UnitOfWorkScope(_config);
            }
        }
    
}
