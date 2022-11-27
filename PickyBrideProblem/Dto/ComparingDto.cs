using System;
namespace PickyBrideProblem.Dto
{
    public record ComparingDto
    (
        string? FirstFirstName = null,
        string? FirstLastName = null,
        string? SecondFirstName = null,
        string? SecondLastName = null
    );
}

