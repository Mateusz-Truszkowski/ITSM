using ITSM.Tests.TestUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITSM.Tests.Integration
{
    [CollectionDefinition("IntegrationTests", DisableParallelization = true)]
    public class IntegrationTestsCollection : ICollectionFixture<MyWebApplicationFactory>
    {
    }
}
