using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using TechExpress.Repository.Contexts;
using TechExpress.Repository.Models;

namespace TechExpress.Service.Initializers;

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
            ImageUrl = "https://nguyencongpc.vn/media/product/26728-khung-pc-1.jpg",
            Description = "Máy tính chơi game",
            IsDeleted = false,
        };
        var officePc = new Category
        {
            Id = Guid.NewGuid(),
            Name = "PC văn phòng",
            ImageUrl = "https://product.hstatic.net/200000420363/product/van_phong_5fc79072051c4e8090dd4a093db39624_master.png",
            ParentCategoryId = null,
            Description = "Máy tính văn phòng",
            IsDeleted = false,
        };
        var workstationPc = new Category
        {
            Id = Guid.NewGuid(),
            Name = "PC Workstation",
            ParentCategoryId = null,
            ImageUrl = "https://minhancomputercdn.com/media/product/4863_pc_gaming_i7_10700__gtx_1650__ram_16gb_ssd_250gb.jpg",
            Description = "Máy tính trạm, server",
            IsDeleted = false,
        };
        var laptop = new Category
        {
            Id = Guid.NewGuid(),
            Name = "Máy tính xách tay",
            ImageUrl = "https://www.tncstore.vn/media/product/250-8011-rix-g15-g513rm-hq055w-r7-6800h_080ec72734354979939d6d686704b6a7_master_e71b36a5698447f98ff737a2cc12f.jpg",
            ParentCategoryId = null,
            Description = "Máy tính xách tay chuyên chơi game",
            IsDeleted = false,
        };
        
        var pcComponentsId = Guid.NewGuid();
        var pcComponents = new Category
        {
            Id = pcComponentsId,
            Name = "Linh kiện PC",
            ParentCategoryId = null,
            ImageUrl = "https://dhlend.com/wp-content/uploads/2023/02/bo-may-tinh-lien-chieu.png",
            Description = "Danh mục chứa các linh kiện PC",
            IsDeleted = false,
        };
        var cpu = new Category
        {
            Id = Guid.NewGuid(),
            Name = "CPU",
            ParentCategoryId = pcComponentsId,
            ImageUrl = "https://cdn2.cellphones.com.vn/x/media/catalog/product/c/p/cpu-intel-core-i5-12400f.jpg",
            Description = "Vi xử lý",
            IsDeleted = false,
        };
        var motherboard = new Category
        {
            Id = Guid.NewGuid(),
            Name = "Bo mạch chủ",
            ParentCategoryId = pcComponentsId,
            ImageUrl = "https://cdn2.cellphones.com.vn/x/media/catalog/product/m/a/mainboard-msi-pro-b760m-a-wifi-ddr4_4_.png",
            Description = "Bo mạch chủ",
            IsDeleted = false,
        };
        var ram = new Category
        {
            Id = Guid.NewGuid(),
            Name = "RAM",
            ParentCategoryId = pcComponentsId,
            ImageUrl = "https://bizweb.dktcdn.net/thumb/1024x1024/100/329/122/products/ct8g4dfs832a-02-dc6ccda4-987e-4264-8082-b66aba784d5d-a5f5aff6-308f-4e80-b16c-a00c4fd9c3d9-bd02382e-bc03-48f2-997b-9a3846e5cd76.jpg?v=1605350684943",
            Description = "Bộ nhớ trong",
            IsDeleted = false,
        };
        var gpu = new Category
        {
            Id = Guid.NewGuid(),
            Name = "Card đồ họa",
            ParentCategoryId = pcComponentsId,
            ImageUrl = "https://product.hstatic.net/1000333506/product/_geforce_rtx_4060_ti_8gb_gddr6_57e9ce678ee84d7d9437b79419c9c2f2_grande_401866c2af9e45e0aecf0bdb9f41c1ad.jpg",
            Description = "Card đồ họa rời",
            IsDeleted = false,
        };
        var psu = new Category
        {
            Id = Guid.NewGuid(),
            Name = "Nguồn máy tính",
            ParentCategoryId = pcComponentsId,
            ImageUrl = "https://www.lifewire.com/thmb/Gb9Gx1CUa04i_-cq35BvBTEPdL0=/1500x0/filters:no_upscale():max_bytes(150000):strip_icc()/power-supply-5aba984fba617700376b877f.PNG",
            Description = "Bộ nguồn cấp điện",
            IsDeleted = false,
        };
        var storage = new Category
        {
            Id = Guid.NewGuid(),
            Name = "Ổ cứng",
            ParentCategoryId = pcComponentsId,
            ImageUrl = "https://hopquan.vn/wp-content/uploads/2020/08/c-ng-hdd-seagate-1tb-sata3-7200rpm.jpg",
            Description = "SSD và HDD",
            IsDeleted = false,
        };
        var pcCase = new Category
        {
            Id = Guid.NewGuid(),
            Name = "Vỏ máy tính",
            ParentCategoryId = pcComponentsId,
            ImageUrl = "https://bizweb.dktcdn.net/thumb/1024x1024/100/329/122/products/case-may-tinh-deepcool-cc360-argb-r-cc360-bkapm3-g-1-05.jpg?v=1743639700297",
            Description = "Thùng máy tính",
            IsDeleted = false,
        };
        var cpuCooler = new Category
        {
            Id = Guid.NewGuid(),
            Name = "Tản nhiệt CPU",
            ParentCategoryId = pcComponentsId,
            ImageUrl = "https://product.hstatic.net/200000420363/product/tan-nhiet-cpu-jonsbo-cr-1000-evo-rgb_fc65c36d565c461ab80cc7ddc2964c14_large.jpg",
            Description = "Tản nhiệt khí và tản nhiệt nước",
            IsDeleted = false,
        };

        var accessoriesId = Guid.NewGuid();
        var accessories = new Category
        {
            Id = accessoriesId,
            Name = "Phụ kiện",
            ParentCategoryId = null,
            ImageUrl = "https://tinhocthanhkhang.vn/media/category/cat_icon_35.png",
            Description = "Phụ kiện máy tính",
            IsDeleted = false,
        };
        var keyboard = new Category
        {
            Id = Guid.NewGuid(),
            Name = "Bàn phím",
            ParentCategoryId = accessoriesId,
            ImageUrl = "https://owlgaming.vn/wp-content/uploads/2023/06/ban-phim-co-cidoo-abm098-tri-mode.jpg",
            Description = "Bàn phím cơ, bàn phím membrane",
            IsDeleted = false,
        };
        var mouse = new Category
        {
            Id = Guid.NewGuid(),
            Name = "Chuột",
            ParentCategoryId = accessoriesId,
            ImageUrl = "https://bizweb.dktcdn.net/100/461/733/products/m100-3.jpg?v=1716781677577",
            Description = "Chuột gaming, chuột văn phòng",
            IsDeleted = false,
        };
        var headset = new Category
        {
            Id = Guid.NewGuid(),
            Name = "Tai nghe",
            ParentCategoryId = accessoriesId,
            ImageUrl = "https://www.anphatpc.com.vn/media/news/2509_Top10tainghepcgaming_2.jpg",
            Description = "Tai nghe gaming, tai nghe âm thanh",
            IsDeleted = false,
        };
        var monitor = new Category
        {
            Id = Guid.NewGuid(),
            Name = "Màn hình",
            ParentCategoryId = accessoriesId,
            ImageUrl = "https://dellonline.vn/wp-content/uploads/2024/10/Dell-P2424HT-.png",
            Description = "Màn hình máy tính",
            IsDeleted = false,
        };
        var webcam = new Category
        {
            Id = Guid.NewGuid(),
            Name = "Webcam",
            ParentCategoryId = accessoriesId,
            ImageUrl = "https://viethansecurity.com/media/product/7400_webcam_cho_may_tinh_dahua_uc325.jpg",
            Description = "Webcam họp trực tuyến, livestream",
            IsDeleted = false,
        };
        var speaker = new Category
        {
            Id = Guid.NewGuid(),
            Name = "Loa",
            ParentCategoryId = accessoriesId,
            ImageUrl = "https://nguyengialaptop.com/wp-content/uploads/2023/07/Loa-May-tinh-Rapoo-A80-1.jpg",
            Description = "Loa máy tính",
            IsDeleted = false,
        };
        var mousepad = new Category
        {
            Id = Guid.NewGuid(),
            Name = "Lót chuột",
            ParentCategoryId = accessoriesId,
            ImageUrl = "https://www.tnc.com.vn/uploads/product/03_2017/padmouse_x3.jpg",
            Description = "Lót chuột gaming, lót chuột văn phòng",
            IsDeleted = false,
        };
        var networkDevice = new Category
        {
            Id = Guid.NewGuid(),
            Name = "Thiết bị mạng",
            ParentCategoryId = accessoriesId,
            ImageUrl = "https://maipu.vn/uploads/images/san-pham/switch-maipu/is170/thiet-bi-mang-modem.jpg",
            Description = "Router, card mạng, USB wifi",
            IsDeleted = false,
        };
        var externalStorage = new Category
        {
            Id = Guid.NewGuid(),
            Name = "Ổ cứng di động",
            ParentCategoryId = accessoriesId,
            ImageUrl = "https://www.phucanh.vn/media/product/18929_western_element_2tb_2_5_inch__1_.jpg",
            Description = "Ổ cứng gắn ngoài, USB, box ổ cứng",
            IsDeleted = false,
        };
        var ups = new Category
        {
            Id = Guid.NewGuid(),
            Name = "Bộ lưu điện",
            ParentCategoryId = accessoriesId,
            ImageUrl = "https://bizweb.dktcdn.net/100/082/878/products/55847-bo-luu-dien-ups-hikvision-ds-ups2000.jpg?v=1725783805207",
            Description = "UPS bảo vệ thiết bị",
            IsDeleted = false,
        };
        var caseFan = new Category
        {
            Id = Guid.NewGuid(),
            Name = "Quạt case",
            ParentCategoryId = accessoriesId,
            ImageUrl = "https://product.hstatic.net/200000522285/product/upload_33b3aa0e39814ad2b9e9172515c369c4.jpg",
            Description = "Quạt tản nhiệt thùng máy, LED RGB",
            IsDeleted = false,
        };
        var cable = new Category
        {
            Id = Guid.NewGuid(),
            Name = "Cáp & Đầu chuyển",
            ParentCategoryId = accessoriesId,
            ImageUrl = "https://gucongnghe.com/wp-content/uploads/2020/02/Day-cap-HDMI-4K-Amazon-Basics.jpg",
            Description = "Cáp HDMI, DisplayPort, USB-C, ổ cắm điện",
            IsDeleted = false,
        };
        var gamingChair = new Category
        {
            Id = Guid.NewGuid(),
            Name = "Ghế gaming",
            ParentCategoryId = accessoriesId,
            ImageUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRUi8dpUhAhq2ldO38YTGsDolvgPmU-EvGJEA&s",
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
