using TechExpress.Repository;
using TechExpress.Repository.CustomExceptions;
using TechExpress.Repository.Enums;
using TechExpress.Repository.Models;
using TechExpress.Service.Utils;

namespace TechExpress.Service.Services;

public class SpecDefinitionService
{
    private readonly UnitOfWork _unitOfWork;

    public SpecDefinitionService(UnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<SpecDefinition> HandleCreateAsync(
        string code,
        string name,
        Guid categoryId,
        string unit,
        SpecAcceptValueType acceptValueType,
        string description,
        bool isRequired)
    {
        if (!await _unitOfWork.SpecDefinitionRepository.CategoryExistsAsync(categoryId))
        {
            throw new NotFoundException("Danh mục không tồn tại.");
        }

        if (await _unitOfWork.SpecDefinitionRepository.ExistsByCodeAsync(code.Trim()))
        {
            throw new BadRequestException("Code định nghĩa thông số đã tồn tại.");
        }

        if (await _unitOfWork.SpecDefinitionRepository.ExistsByNameAsync(name))
        {
            throw new BadRequestException("Tên định nghĩa thông số đã tồn tại.");
        }

        var specDefinition = new SpecDefinition
        {
            Id = Guid.NewGuid(),
            Code = code.Trim(),
            Name = name.Trim(),
            CategoryId = categoryId,
            Unit = unit.Trim(),
            AcceptValueType = acceptValueType,
            Description = description.Trim(),
            IsRequired = isRequired,
            IsDeleted = false,
            UpdatedAt = DateTimeOffset.Now
        };

        await _unitOfWork.SpecDefinitionRepository.AddAsync(specDefinition);
        await _unitOfWork.SaveChangesAsync();

        return specDefinition;
    }

    public async Task<Pagination<SpecDefinition>> HandleGetPagedAsync(
        int pageNumber = 1,
        int pageSize = 20,
        string? searchName = null,
        DateTimeOffset? createdFrom = null,
        DateTimeOffset? createdTo = null)
    {
        if (pageNumber < 1) pageNumber = 1;
        if (pageSize < 1 || pageSize > 100) pageSize = 20;

        var (items, totalCount) = await _unitOfWork.SpecDefinitionRepository.GetPagedAsync(
            pageNumber,
            pageSize,
            searchName,
            createdFrom,
            createdTo);

        return new Pagination<SpecDefinition>
        {
            Items = items,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }

    public async Task<SpecDefinition> HandleGetByIdAsync(Guid id)
    {
        var specDefinition = await _unitOfWork.SpecDefinitionRepository.FindByIdAsync(id)
            ?? throw new NotFoundException("Định nghĩa thông số không tồn tại.");

        return specDefinition;
    }

    public async Task<SpecDefinition> HandleUpdateAsync(
        Guid id,
        string? code,
        string? name,
        Guid? categoryId,
        string? unit,
        SpecAcceptValueType? acceptValueType,
        string? description,
        bool? isRequired)
    {
        var specDefinition = await _unitOfWork.SpecDefinitionRepository.FindByIdWithTrackingAsync(id)
            ?? throw new NotFoundException("Định nghĩa thông số không tồn tại.");

        if (specDefinition.IsDeleted)
        {
            throw new BadRequestException("Định nghĩa thông số đã bị xóa.");
        }

        if (!string.IsNullOrWhiteSpace(code))
        {
            if (await _unitOfWork.SpecDefinitionRepository.ExistsByCodeExcludingIdAsync(code.Trim(), id))
            {
                throw new BadRequestException("Code định nghĩa thông số đã tồn tại.");
            }
            specDefinition.Code = code.Trim();
        }

        if (!string.IsNullOrWhiteSpace(name))
        {
            if (await _unitOfWork.SpecDefinitionRepository.ExistsByNameExcludingIdAsync(name.Trim(), id))
            {
                throw new BadRequestException("Tên định nghĩa thông số đã tồn tại.");
            }
            specDefinition.Name = name.Trim();
        }

        if (categoryId.HasValue)
        {
            if (!await _unitOfWork.SpecDefinitionRepository.CategoryExistsAsync(categoryId.Value))
            {
                throw new NotFoundException("Danh mục không tồn tại.");
            }
            specDefinition.CategoryId = categoryId.Value;
        }

        if (!string.IsNullOrWhiteSpace(unit))
        {
            specDefinition.Unit = unit.Trim();
        }

        if (acceptValueType.HasValue)
        {
            specDefinition.AcceptValueType = acceptValueType.Value;
        }

        if (!string.IsNullOrWhiteSpace(description))
        {
            specDefinition.Description = description.Trim();
        }

        if (isRequired.HasValue)
        {
            specDefinition.IsRequired = isRequired.Value;
        }

        specDefinition.UpdatedAt = DateTimeOffset.Now;

        await _unitOfWork.SaveChangesAsync();

        return specDefinition;
    }

    public async Task HandleDeleteAsync(Guid id)
    {
        var specDefinition = await _unitOfWork.SpecDefinitionRepository.FindByIdWithTrackingAsync(id)
            ?? throw new NotFoundException("Định nghĩa thông số không tồn tại.");

        var hasRelatedData = await _unitOfWork.SpecDefinitionRepository.HasRelatedProductSpecValuesAsync(id);

        if (hasRelatedData)
        {
            if (specDefinition.IsDeleted)
            {
                throw new BadRequestException("Định nghĩa thông số đã bị xóa.");
            }

            specDefinition.IsDeleted = true;
            specDefinition.UpdatedAt = DateTimeOffset.Now;
        }
        else
        {
            _unitOfWork.SpecDefinitionRepository.Remove(specDefinition);
        }

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<Pagination<SpecDefinition>> HandleFindSpecDefinitionsOnCategoryIdAsync(Guid categoryId,
        int pageNumber = 1)
    {
        if (pageNumber < 1) pageNumber = 1;
        (List<SpecDefinition> specs, int totalCount) =
            await _unitOfWork.SpecDefinitionRepository.FindByCategoryIdWithPagingAsync(categoryId, pageNumber);

        return new Pagination<SpecDefinition>
        {
            Items = specs,
            PageNumber = pageNumber,
            PageSize = 20,
            TotalCount = totalCount
        };
    }
}
