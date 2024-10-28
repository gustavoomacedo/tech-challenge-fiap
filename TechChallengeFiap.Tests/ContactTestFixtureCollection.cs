using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechChallengeFiap.Tests
{
    [CollectionDefinition("ContactTestFixtureCollection")]
    public class ContactTestFixtureCollection : ICollectionFixture<ContactFixture>
    {
    }
}
