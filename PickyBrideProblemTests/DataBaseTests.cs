namespace PickyBrideProblemTests;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework.Internal;
using PickyBrideProblem.Entity;
using PickyBrideProblem.Service;

[TestFixture]
public class DataBaseTests
{

    private DataBaseContext _dataBaseContext;

    [SetUp]
    public void SetUp()
    {
        _dataBaseContext = new();
    }

    [Test]
    public void DataBaseTest()
    {
        Assert.That(_dataBaseContext.Attempt.Count().Equals(101));
        Assert.That(_dataBaseContext.Attempt.Find(101).Name.Equals("test"));

        List<Contender> cloneContenders = _dataBaseContext.Contender
            .Where(c => c.AttemptId.Equals(101))
            .OrderBy(c => c.DateNumber)
            .ToList();
        List<Contender> originalContenders = _dataBaseContext.Contender
            .Where(c => c.AttemptId.Equals(25))
            .OrderBy(c => c.DateNumber)
            .ToList();

        Assert.That(cloneContenders.Count().Equals(originalContenders.Count));

        for (int i = 0; i < originalContenders.Count(); i++)
        {
            Assert.That(originalContenders[i].DateNumber.Equals(cloneContenders[i].DateNumber));
            Assert.That(originalContenders[i].Name.Equals(cloneContenders[i].Name));
            Assert.That(originalContenders[i].Lastname.Equals(cloneContenders[i].Lastname));
            Assert.That(originalContenders[i].Quality.Equals(cloneContenders[i].Quality));
        }

        Assert.That(_dataBaseContext.Contender.Count() <= 100 * 101);
    }
}
