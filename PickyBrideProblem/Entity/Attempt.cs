using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace PickyBrideProblem.Entity
{
    [Table("attempt")]
    public class Attempt
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public List<Contender>? contenderList { get; set; }
    }
}

