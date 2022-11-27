namespace PickyBrideProblemTests;

using PickyBrideProblem.Entity;
using PickyBrideProblem.Service;

[TestFixture]
public class GeneratorTest
{

    private ContenderGenerator _contenderGenerator;

    [SetUp]
    public void SetUp()
    {
        _contenderGenerator = new();
    }

    [Test]
    public void ContendersUniqueNamesTest()
    {
        List<Contender> contenders = _contenderGenerator.GenerateContenders();

        var groups = contenders.GroupBy(s => s.Name + s.Lastname);
        foreach (var group in groups)
        {
            Assert.That(group.Count().Equals(1));
        }
    }
}
