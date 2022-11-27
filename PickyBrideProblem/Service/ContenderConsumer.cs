using System;
using System.Text.Json;
using MassTransit;

using PickyBrideProblem.Dto;

namespace PickyBrideProblem.Service
{
    public class ContenderConsumer : IConsumer<ContenderDto>
    {
        public static ContenderDto _currentContender;

        public async Task Consume(ConsumeContext<ContenderDto> context)
        {
            var message = context.Message;
            _currentContender = new ContenderDto(message.Id, message.Quality, message.FirstName, message.LastName);

            var jsonMessage = JsonSerializer.Serialize(message);
            Console.WriteLine($"Contender message: {jsonMessage}");
        }

        public static Task<ContenderDto> GetNext()
        {
            return Task.FromResult(_currentContender);
        }
    }
}

