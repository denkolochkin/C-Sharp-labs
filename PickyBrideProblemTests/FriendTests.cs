namespace PickyBrideProblemTests;

using NUnit.Framework.Internal;
using PickyBrideProblem.Entity;
using PickyBrideProblem.Service;

[TestFixture]
public class FriendTests
{
    private Friend _friend;

    [SetUp]
    public void SetUp()
    {
        _friend = new();
    }

    [Test]
    public void ContendersComparingTest()
    {
        Contender best = new Contender()
        {
            Name = "test",
            Lastname = "test",
            Quality = 30
        };

        Contender worse = new Contender()
        {
            Name = "test",
            Lastname = "test",
            Quality = 2
        };

        _friend.ProcessedContenders.Add(best);
        _friend.ProcessedContenders.Add(worse);

        Contender winner = _friend.Compare(best, worse);

        Assert.That(winner.Quality.Equals(30));
    }

    [Test]
    public void ComparingErrorTest()
    {
        Contender first = new Contender()
        {
            Name = "test",
            Lastname = "test",
            Quality = 30
        };

        Contender second = new Contender()
        {
            Name = "test",
            Lastname = "test",
            Quality = 2
        };

        Assert.Throws<System.Exception>(() =>
        {
            _friend.Compare(first, second);
        });

        _friend.ProcessedContenders.Add(first);

        Assert.Throws<System.Exception>(() =>
        {
            _friend.Compare(first, second);
        });


    }
}
