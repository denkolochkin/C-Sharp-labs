using Microsoft.AspNetCore.Mvc;
using MassTransit;

using PickyBrideProblem.Service;
using PickyBrideProblem.Entity;
using PickyBrideProblem.Dto;

namespace PickyBrideProblemWebApi.Controllers;

public class HallController : ControllerBase
{
    private readonly IPublishEndpoint _publishEndpoint;

    public static DataBaseContext Context = new();

    private static int CurrentAttempt = 0;

    private static int DatesCount = 1;

    private static List<ContenderDto> CurrentList = new();

    public HallController(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    [HttpGet("hall/{attempt}/next")]
    public void GetNext(int attempt)
    {
        if (CurrentAttempt == 0 || CurrentAttempt != attempt)
        {
            Attempt newAttempt = new Attempt
            {
                Id = attempt,
                Name = Faker.StringFaker.Alpha(15),
                contenderList = new()
            };
            Context.Attempt.Add(newAttempt);
            Context.SaveChanges();

            CurrentAttempt = attempt;
            DatesCount = 1;
            ContenderGenerator generator = new();
            CurrentList = generator.GenerateContenders();
            FriendController._friend.ProcessedContenders = new();
        }

        ContenderDto contenderDto = CurrentList[new Random().Next(CurrentList.Count)];
        CurrentList.Remove(contenderDto);

        Contender contender = new Contender
        {
            Id = contenderDto.Id,
            Name = contenderDto.FirstName,
            Lastname = contenderDto.LastName,
            Quality = contenderDto.Quality,
            DateNumber = DatesCount
        };

        Context.Contender.Add(contender);
        Context.Attempt.Find(attempt).contenderList.Add(contender);
        Context.SaveChanges();

        DatesCount++;

        FriendController._friend.ProcessedContenders.Add(contender);

        _publishEndpoint.Publish<ContenderDto>(contenderDto);
    }

    [HttpPost("hall/{attempt}/reset")]
    public ContenderDto Reset(int attempt)
    {
        DataBaseContext context = new();
        Attempt? currentAttempt = context.Attempt.Find(attempt);

        if (currentAttempt == null)
        {
            throw new Exception("Attempt #" + attempt + " not found");
        }

        Contender contender = context.Contender
            .Where(c => c.AttemptId.Equals(currentAttempt.Id))
            .OrderBy(c => c.DateNumber)
            .Last();

        return new ContenderDto(contender.Id, contender.Quality, contender.Name, contender.Lastname);
    }

    [HttpPost("hall/{attempt}/select/{date}")]
    public int GetQuality(int attempt, int date)
    {
        DataBaseContext context = new();
        Attempt? currentAttempt = context.Attempt.Find(attempt);


        if (currentAttempt == null)
        {
            throw new Exception("Attempt #" + attempt + " not found");
        }

        Contender contender = context.Contender
            .Where(c => c.AttemptId.Equals(currentAttempt.Id))
            .Where(c => c.DateNumber == date)
            .First();

        return contender.Quality;
    }
}




