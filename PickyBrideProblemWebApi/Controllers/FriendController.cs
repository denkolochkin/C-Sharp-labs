using Microsoft.AspNetCore.Mvc;

using PickyBrideProblem.Service;
using PickyBrideProblem.Entity;
using PickyBrideProblem.Dto;

namespace PickyBrideProblemWebApi.Controllers;

public class FriendController : ControllerBase
{

    public static Friend _friend = new();

    [HttpPost("friend/{attempt}/compare")]
    public ContenderDto Compare(int attempt, [FromBody] ComparingDto dto)
    {
        DataBaseContext context = HallController.Context;

        Contender firstContender = context.Contender
            .Where(c => c.AttemptId == attempt && c.Name == dto.FirstFirstName && c.Lastname == dto.FirstLastName)
            .First();
        Contender secondContender = context.Contender
            .Where(c => c.AttemptId == attempt && c.Name == dto.SecondFirstName && c.Lastname == dto.SecondLastName)
            .First();

        Contender winner = _friend.Compare(firstContender, secondContender);

        return new ContenderDto(winner.Id, winner.Quality, winner.Name, winner.Lastname);
    }
}