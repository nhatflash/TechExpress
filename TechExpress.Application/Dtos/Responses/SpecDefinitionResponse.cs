using System;
using TechExpress.Repository.Enums;

namespace TechExpress.Application.Dtos.Responses;

public record SpecDefinitionResponse(
    Guid Id,
    string Code,
    string Name,
    Guid CategoryId,
    string CategoryName,
    string Unit,
    SpecAcceptValueType AcceptValueType,
    string Description,
    bool IsRequired,
    bool IsDeleted,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt
);
