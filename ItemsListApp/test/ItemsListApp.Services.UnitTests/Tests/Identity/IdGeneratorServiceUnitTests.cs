using System;
using System.Threading.Tasks;
using ItemsListApp.Services.Identity;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace ItemsListApp.Services.UnitTests.Tests.Identity
{
    [TestFixture()]
    public class IdGeneratorServiceUnitTests
    {
        private IdGeneratorService _idGenerator;

        [SetUp]
        public void SetUp()
        {
            _idGenerator = new IdGeneratorService();
        }

        [Test]
        public async Task GenerateIdAsync_NotReturnEmptyGuid()
        {
            var newId = await _idGenerator.GenerateIdAsync();

            Assert.That(newId, Is.Not.EqualTo(Guid.Empty));
        }

        [Test]
        public async Task GenerateIdAsync_TwoConsequentCalls_ReturnUniqueIds()
        {
            var newId = await _idGenerator.GenerateIdAsync();
            var newId2 = await _idGenerator.GenerateIdAsync();

            Assert.That(newId, Is.Not.EqualTo(newId2));
        }
    }
}