namespace PickyBrideProblemTests;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.Extensions.DependencyInjection;

using System.Threading;

using PickyBrideProblem.Entity;
using PickyBrideProblem.Service;
using PickyBrideProblem.Dto;

[TestFixture]
public class PrincessTests
{

    private Princess _princess;

    private Hall _hall;

    private Friend _friend;

    private ContenderGenerator _generator;

    private DataBaseContext _context;

    [SetUp]
    public void SetUp()
    {
        _hall = new();
        _friend = new();
        _generator = new();
        _context = new();
        ILogger<ApplicationLifetime> logger = NullLogger<ApplicationLifetime>.Instance;
        ApplicationLifetime lifetime = new(logger);
        _princess = new(lifetime, _hall, _friend, _generator, _context);
        _hall.contenders = _generator.GenerateContenders();
    }

    [Test]
    public void StrategyTest()
    {
        int datesCount = 0;

        List<Contender> firstContenders = new();
        PrincessAnswer currentAnswer = new();
        Contender currentContender = new();

        for (int i = 0; i < 100; i++)
        {
            currentContender = _hall.GetNextContender();
            currentAnswer = _princess.DateContender(currentContender, _friend);
            datesCount++;
            if (datesCount < (100 / Math.E))
            {
                firstContenders.Add(currentContender);
            }
            else
            {
                Contender bestOfFirst = FindBestOfFirst(firstContenders);
                if (currentAnswer.Answer.Equals("Yes") && datesCount < 100)
                {
                    Console.WriteLine("Winner test");

                    Assert.That(currentAnswer.Quality.Equals(currentContender.Quality));
                    Assert.That(bestOfFirst.Quality < currentAnswer.Quality);
                }
            }
            if (currentAnswer.Answer.Equals("Yes"))
            {
                break;
            }
        }


        if (datesCount.Equals(100))
        {
            Console.WriteLine("Last contender test");

            Assert.That(currentAnswer.Answer.Equals("Yes"));
            Assert.That(currentAnswer.Quality.Equals(currentContender.Quality));
        }

    }

    private Contender FindBestOfFirst(List<Contender> FirstContenders)
    {
        Contender bestContenter = FirstContenders.First();
        for (int i = 1; i < FirstContenders.Count; i++)
        {
            if (!bestContenter.Equals(_friend.Compare(bestContenter, FirstContenders[i])))
            {
                bestContenter = FirstContenders[i];
            }
        }
        return bestContenter;
    }
}
