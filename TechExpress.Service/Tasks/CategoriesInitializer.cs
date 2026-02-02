using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using TechExpress.Repository.Contexts;
using TechExpress.Repository.Models;

namespace TechExpress.Service.Tasks;

public static class CategoriesInitializer
{
    public static async Task Init(ApplicationDbContext context)
    {
        if (await context.Categories.AnyAsync())
        {
            return;
        }
        var gamingPc = new Category
        {
            Id = Guid.NewGuid(),
            Name = "PC Gaming",
            ParentCategoryId = null,
            Description = "Máy tính chơi game",
            IsDeleted = false,
        };
        var officePc = new Category
        {
            Id = Guid.NewGuid(),
            Name = "PC văn phòng",
            ParentCategoryId = null,
            Description = "Máy tính văn phòng",
            IsDeleted = false,
        };
        var workstationPc = new Category
        {
            Id = Guid.NewGuid(),
            Name = "PC Workstation",
            ParentCategoryId = null,
            Description = "Máy tính trạm, server",
            IsDeleted = false,
        };
        var laptop = new Category
        {
            Id = Guid.NewGuid(),
            Name = "Laptop",
            ParentCategoryId = null,
            Description = "Máy tính xách tay",
            IsDeleted = false,
        };
        
        var pcComponentsId = Guid.NewGuid();
        var pcComponents = new Category
        {
            Id = pcComponentsId,
            Name = "Linh kiện PC",
            ParentCategoryId = null,
            Description = "Danh mục chứa các linh kiện PC",
            IsDeleted = false,
        };
        var cpu = new Category
        {
            Id = Guid.NewGuid(),
            Name = "CPU",
            ParentCategoryId = pcComponentsId,
            Description = "Vi xử lý",
            IsDeleted = false,
        };
        var motherboard = new Category
        {
            Id = Guid.NewGuid(),
            Name = "Bo mạch chủ",
            ParentCategoryId = pcComponentsId,
            Description = "Bo mạch chủ",
            IsDeleted = false,
        };
        var ram = new Category
        {
            Id = Guid.NewGuid(),
            Name = "RAM",
            ParentCategoryId = pcComponentsId,
            Description = "Bộ nhớ trong",
            IsDeleted = false,
        };
        var gpu = new Category
        {
            Id = Guid.NewGuid(),
            Name = "Card đồ họa",
            ParentCategoryId = pcComponentsId,
            Description = "Card đồ họa rời",
            IsDeleted = false,
        };
        var psu = new Category
        {
            Id = Guid.NewGuid(),
            Name = "Nguồn máy tính",
            ParentCategoryId = pcComponentsId,
            Description = "Bộ nguồn cấp điện",
            IsDeleted = false,
        };
        var storage = new Category
        {
            Id = Guid.NewGuid(),
            Name = "Ổ cứng",
            ParentCategoryId = pcComponentsId,
            Description = "SSD và HDD",
            IsDeleted = false,
        };
        var pcCase = new Category
        {
            Id = Guid.NewGuid(),
            Name = "Vỏ máy tính",
            ParentCategoryId = pcComponentsId,
            Description = "Thùng máy tính",
            IsDeleted = false,
        };
        var cpuCooler = new Category
        {
            Id = Guid.NewGuid(),
            Name = "Tản nhiệt CPU",
            ParentCategoryId = pcComponentsId,
            Description = "Tản nhiệt khí và tản nhiệt nước",
            IsDeleted = false,
        };

        var accessoriesId = Guid.NewGuid();
        var accessories = new Category
        {
            Id = accessoriesId,
            Name = "Phụ kiện",
            ParentCategoryId = null,
            Description = "Phụ kiện máy tính",
            IsDeleted = false,
        };
        var keyboard = new Category
        {
            Id = Guid.NewGuid(),
            Name = "Bàn phím",
            ParentCategoryId = accessoriesId,
            Description = "Bàn phím cơ, bàn phím membrane",
            IsDeleted = false,
        };
        var mouse = new Category
        {
            Id = Guid.NewGuid(),
            Name = "Chuột",
            ParentCategoryId = accessoriesId,
            Description = "Chuột gaming, chuột văn phòng",
            IsDeleted = false,
        };
        var headset = new Category
        {
            Id = Guid.NewGuid(),
            Name = "Tai nghe",
            ParentCategoryId = accessoriesId,
            Description = "Tai nghe gaming, tai nghe âm thanh",
            IsDeleted = false,
        };
        var monitor = new Category
        {
            Id = Guid.NewGuid(),
            Name = "Màn hình",
            ParentCategoryId = accessoriesId,
            Description = "Màn hình máy tính",
            IsDeleted = false,
        };
        var webcam = new Category
        {
            Id = Guid.NewGuid(),
            Name = "Webcam",
            ParentCategoryId = accessoriesId,
            Description = "Webcam họp trực tuyến, livestream",
            IsDeleted = false,
        };
        var speaker = new Category
        {
            Id = Guid.NewGuid(),
            Name = "Loa",
            ParentCategoryId = accessoriesId,
            Description = "Loa máy tính",
            IsDeleted = false,
        };
        var mousepad = new Category
        {
            Id = Guid.NewGuid(),
            Name = "Lót chuột",
            ParentCategoryId = accessoriesId,
            Description = "Lót chuột gaming, lót chuột văn phòng",
            IsDeleted = false,
        };
        var networkDevice = new Category
        {
            Id = Guid.NewGuid(),
            Name = "Thiết bị mạng",
            ParentCategoryId = accessoriesId,
            Description = "Router, card mạng, USB wifi",
            IsDeleted = false,
        };
        var externalStorage = new Category
        {
            Id = Guid.NewGuid(),
            Name = "Ổ cứng di động",
            ParentCategoryId = accessoriesId,
            Description = "Ổ cứng gắn ngoài, USB, box ổ cứng",
            IsDeleted = false,
        };
        var ups = new Category
        {
            Id = Guid.NewGuid(),
            Name = "Bộ lưu điện",
            ParentCategoryId = accessoriesId,
            Description = "UPS bảo vệ thiết bị",
            IsDeleted = false,
        };
        var caseFan = new Category
        {
            Id = Guid.NewGuid(),
            Name = "Quạt case",
            ParentCategoryId = accessoriesId,
            Description = "Quạt tản nhiệt thùng máy, LED RGB",
            IsDeleted = false,
        };
        var cable = new Category
        {
            Id = Guid.NewGuid(),
            Name = "Cáp & Đầu chuyển",
            ParentCategoryId = accessoriesId,
            Description = "Cáp HDMI, DisplayPort, USB-C, ổ cắm điện",
            IsDeleted = false,
        };
        var gamingChair = new Category
        {
            Id = Guid.NewGuid(),
            Name = "Ghế gaming",
            ParentCategoryId = accessoriesId,
            Description = "Ghế gaming, ghế công thái học",
            IsDeleted = false,
        };

        context.Categories.AddRange(
            gamingPc, officePc, workstationPc, laptop,
            pcComponents, cpu, motherboard, ram, gpu, psu, storage, pcCase, cpuCooler,
            accessories, keyboard, mouse, headset, monitor, webcam, speaker, mousepad,
            networkDevice, externalStorage, ups, caseFan, cable, gamingChair
        );
        await context.SaveChangesAsync();
    }
}
