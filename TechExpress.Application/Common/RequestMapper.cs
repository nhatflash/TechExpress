using TechExpress.Application.Dtos.Requests;
using TechExpress.Repository.CustomExceptions;
using TechExpress.Service.Commands;

namespace TechExpress.Application.Common
{
    public class RequestMapper
    {

        public static List<CreateProductSpecValueCommand> MapToCreateProductSpecValueCommandsFromRequests(List<CreateProductSpecValueRequest> requests)
        {
            List<CreateProductSpecValueCommand> commands = [];
            HashSet<Guid> specIds = [];
            foreach (var request in requests)
            {
                if (specIds.Contains(request.SpecDefinitionId))
                {
                    throw new BadRequestException($"Thông số trùng lặp khi gửi yêu cầu {request.SpecDefinitionId}");
                }
                var command = new CreateProductSpecValueCommand 
                { 
                    SpecDefinitionId = request.SpecDefinitionId,
                    Value = request.Value.Trim(),
                };
                commands.Add(command);
                specIds.Add(request.SpecDefinitionId);
            }
            return commands;
        }
    }
}
