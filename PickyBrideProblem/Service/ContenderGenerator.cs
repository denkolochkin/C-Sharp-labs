using System.Configuration;
using Faker;
using PickyBrideProblem.Dto;

namespace PickyBrideProblem.Service
{
    public class ContenderGenerator
    {

        private readonly int ContendersCount = 100;

        public List<ContenderDto> GenerateContenders()
        {
            List<ContenderDto> contenders = new();

            for (int i = 0; i < ContendersCount; i++)
            {
                ContenderDto contender = new ContenderDto(
                    new Guid(), i + 1, NameFaker.MaleFirstName(), NameFaker.LastName()
                    );
                contenders.Add(contender);
            }

            return contenders;
        }
    }
}

