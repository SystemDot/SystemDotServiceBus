using System;
using System.Collections.Generic;
using SystemDot.Environment;

namespace SystemDot.Messaging.Specifications
{
    public class TestApplication : IApplication
    {
        public IEnumerable<Type> GetAllTypesInAllAssembliesThatImplement<T>()
        {
            yield break;
        }
    }
}