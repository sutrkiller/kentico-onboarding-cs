using System;
using System.Threading.Tasks;
using ItemsListApp.Services.Time;
using NUnit.Framework;

namespace ItemsListApp.Services.UnitTests.Time
{
    [TestFixture]
    public class DateTimeServiceUnitTests
    {
        private DateTimeService _dateTimeService;

        [SetUp]
        public void SetUp()
        {
            _dateTimeService = new DateTimeService();
        }

        [Test]
        public async Task GetCurrentDateAsync_NotReturnsDefaultDate()
        {
            var date = await _dateTimeService.GetCurrentDateAsync();

            Assert.That(date, Is.Not.EqualTo(default(DateTime)));
        }

        [Test]
        public async Task GetCurrentDateAsync_ReturnsCurrentDate()
        {
            var dateBefore = DateTime.Now;
            var currentDate = await _dateTimeService.GetCurrentDateAsync();
            var dateAfter = DateTime.Now;

            Assert.That(currentDate, Is.GreaterThanOrEqualTo(dateBefore).And.LessThanOrEqualTo(dateAfter));
        }
    }
}
