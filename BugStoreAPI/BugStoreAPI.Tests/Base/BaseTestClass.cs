using System;
using System.Collections.Generic;
using System.Text;

namespace BugStoreAPI.Tests.Base
{
    public abstract class BaseTestClass : IDisposable
    {
        protected string ServiceAddress = "http://localhost:59609/api/";
        protected string RootAddress = String.Empty;
        public virtual void Dispose()
        {
        }
    }
}
