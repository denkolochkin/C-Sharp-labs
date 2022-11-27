using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PickyBrideProblem.Entity

{
    [Table("contender")]
    public class Contender
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
        public string Lastname { get; set; }

        public int Quality { get; set; }

        public int DateNumber { get; set; }

        public int AttemptId { get; set; }

    }
}

