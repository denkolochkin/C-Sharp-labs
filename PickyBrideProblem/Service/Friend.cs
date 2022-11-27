using System;
using log4net;
using PickyBrideProblem.Entity;
using PickyBrideProblem.Service;

namespace PickyBrideProblem.Service
{
    public class Friend
    {
        public List<Contender> ProcessedContenders = new();

        public Contender Compare(Contender first, Contender second)
        {
            if (ProcessedContenders.Contains(first) && ProcessedContenders.Contains(second))
            {
                return first.Quality > second.Quality ? first : second;
            }
            throw new Exception("Someone hasn't dated!");
        }
    }
}

