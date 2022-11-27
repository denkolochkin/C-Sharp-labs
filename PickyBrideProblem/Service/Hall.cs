using PickyBrideProblem.Entity;
using PickyBrideProblem.Dto;

using System.Configuration;

namespace PickyBrideProblem.Service
{
    public class Hall 
    {
        /// <summary>
        /// Заполняется ContenderGenerator и присваивается перед началом оды.
        /// </summary>
        public List<ContenderDto> contenders = new();

        public ContenderDto GetNextContender()
        {
            ContenderDto contender = contenders[new Random().Next(contenders.Count)];
            contenders.Remove(contender);
            return contender;
        }
    }
}

