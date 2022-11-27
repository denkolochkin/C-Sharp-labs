using System;
namespace PickyBrideProblem.Dto
{
    public record ContenderDto
    {
        public ContenderDto(Guid id, int quality, string? firstName, string? lastName)
        {
            Id = id;
            Quality = quality;
            FirstName = firstName;
            LastName = lastName;
        }

        public Guid Id { get; }
        public int Quality { get; }
        public string? FirstName { get; }
        public string? LastName { get; }
    }
}

