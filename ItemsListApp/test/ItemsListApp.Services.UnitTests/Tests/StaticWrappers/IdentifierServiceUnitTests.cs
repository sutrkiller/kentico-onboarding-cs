using System;
using System.Threading.Tasks;
using ItemsListApp.Services.StaticWrappers;
using NUnit.Framework;

namespace ItemsListApp.Services.UnitTests.Tests.StaticWrappers
{
    [TestFixture]
    public class IdentifierServiceUnitTests
    {
        private IdentifierService _identifier;

        [SetUp]
        public void SetUp()
        {
            _identifier = new IdentifierService();
        }

        [Test]
        public async Task GenerateIdAsync_NotReturnEmptyGuid()
        {
            var newId = await _identifier.GenerateIdAsync();

            Assert.That(newId, Is.Not.EqualTo(Guid.Empty));
        }

        [Test]
        public async Task GenerateIdAsync_TwoConsequentCalls_ReturnUniqueIds()
        {
            var newId = await _identifier.GenerateIdAsync();
            var newId2 = await _identifier.GenerateIdAsync();

            Assert.That(newId, Is.Not.EqualTo(newId2));
        }
    }
}