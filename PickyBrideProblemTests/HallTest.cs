namespace PickyBrideProblemTests;

using PickyBrideProblem.Entity;
using PickyBrideProblem.Service;

[TestFixture]
public class HallTest
{

    private Hall _hall;


    [SetUp]
    public void SetUp()
    {
        _hall = new();
        ContenderGenerator contenderGenerator = new();
        _hall.contenders = contenderGenerator.GenerateContenders();
    }

    [Test]
    public void GetNextAndLastContenderErrorTest()
    {
        for (int i = 0; i < 100; i++)
        {
            Contender contender = _hall.GetNextContender();
            Assert.IsFalse(_hall.contenders.Contains(contender));
        }
        Assert.Throws<System.ArgumentOutOfRangeException>(() =>
        {
            _hall.GetNextContender();
        });
    }
}
