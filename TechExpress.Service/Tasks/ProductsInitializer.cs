using System;
using Microsoft.EntityFrameworkCore;
using TechExpress.Repository.Contexts;
using TechExpress.Repository.CustomExceptions;
using TechExpress.Repository.Enums;
using TechExpress.Repository.Models;

namespace TechExpress.Service.Tasks;

public static class ProductsInitializer
{
    public static async Task Init(ApplicationDbContext context)
    {
        if (await context.Products.AnyAsync())
        {
            return;
        }

        var allSpecValues = new List<ProductSpecValue>();
        var allProductImages = new List<ProductImage>();

        // ============= GPU PRODUCTS =============
        await InitGpuProducts(context, allSpecValues, allProductImages);

        // ============= CPU PRODUCTS =============
        await InitCpuProducts(context, allSpecValues, allProductImages);

        // ============= MOTHERBOARD PRODUCTS =============
        await InitMotherboardProducts(context, allSpecValues, allProductImages);

        // ============= RAM PRODUCTS =============
        await InitRamProducts(context, allSpecValues, allProductImages);

        context.ProductSpecValues.AddRange(allSpecValues);
        context.ProductImages.AddRange(allProductImages);

        await context.SaveChangesAsync();
    }

    private static async Task InitGpuProducts(ApplicationDbContext context, List<ProductSpecValue> specValues, List<ProductImage> productImages)
    {
        var gpuCategory = await context.Categories.FirstOrDefaultAsync(c => c.Name == "Card đồ họa")
            ?? throw new NotFoundException("Không tìm thấy danh mục Card đồ họa");

        var gpuSpecs = await context.SpecDefinitions
            .Where(s => s.CategoryId == gpuCategory.Id)
            .ToListAsync();

        // Load all GPU brands
        var asusBrand = await context.Brands.FirstOrDefaultAsync(b => b.Name == "ASUS")
            ?? throw new NotFoundException("Không tìm thấy thương hiệu ASUS");
        var msiBrand = await context.Brands.FirstOrDefaultAsync(b => b.Name == "MSI")
            ?? throw new NotFoundException("Không tìm thấy thương hiệu MSI");
        var gigabyteBrand = await context.Brands.FirstOrDefaultAsync(b => b.Name == "Gigabyte")
            ?? throw new NotFoundException("Không tìm thấy thương hiệu Gigabyte");
        var zotacBrand = await context.Brands.FirstOrDefaultAsync(b => b.Name == "Zotac")
            ?? throw new NotFoundException("Không tìm thấy thương hiệu Zotac");
        var palitBrand = await context.Brands.FirstOrDefaultAsync(b => b.Name == "Palit")
            ?? throw new NotFoundException("Không tìm thấy thương hiệu Palit");
        var galaxBrand = await context.Brands.FirstOrDefaultAsync(b => b.Name == "Galax")
            ?? throw new NotFoundException("Không tìm thấy thương hiệu Galax");
        var inno3dBrand = await context.Brands.FirstOrDefaultAsync(b => b.Name == "INNO3D")
            ?? throw new NotFoundException("Không tìm thấy thương hiệu INNO3D");
        var colorfulBrand = await context.Brands.FirstOrDefaultAsync(b => b.Name == "Colorful")
            ?? throw new NotFoundException("Không tìm thấy thương hiệu Colorful");
        var pnyBrand = await context.Brands.FirstOrDefaultAsync(b => b.Name == "PNY")
            ?? throw new NotFoundException("Không tìm thấy thương hiệu PNY");
        var sapphireBrand = await context.Brands.FirstOrDefaultAsync(b => b.Name == "Sapphire")
            ?? throw new NotFoundException("Không tìm thấy thương hiệu Sapphire");
        var xfxBrand = await context.Brands.FirstOrDefaultAsync(b => b.Name == "XFX")
            ?? throw new NotFoundException("Không tìm thấy thương hiệu XFX");
        var powerColorBrand = await context.Brands.FirstOrDefaultAsync(b => b.Name == "PowerColor")
            ?? throw new NotFoundException("Không tìm thấy thương hiệu PowerColor");

        // ============= NVIDIA RTX 4090 =============

        // ASUS ROG STRIX RTX 4090 OC
        var asus4090Id = Guid.NewGuid();
        context.Products.Add(new Product
        {
            Id = asus4090Id,
            Name = "ASUS ROG STRIX GeForce RTX 4090 OC Edition 24GB",
            Sku = "GPU-RTX4090-ASUS-STRIX-OC",
            CategoryId = gpuCategory.Id,
            BrandId = asusBrand.Id,
            Price = 54990000,
            Stock = 8,
            Description = "Card đồ họa ASUS ROG STRIX RTX 4090 OC Edition với 24GB GDDR6X, kiến trúc Ada Lovelace. Tản nhiệt 3.5 slot với 3 quạt Axial-tech, Aura Sync RGB, GPU Tweak III. Card đồ họa flagship cho gaming 4K và workstation.",
            WarrantyMonth = 36,
            Status = ProductStatus.Available,
        });
        AddGpuSpecs(specValues, gpuSpecs, asus4090Id, vram: 24, tdp: 450, length: 358, pcieSlot: "PCIe 4.0 x16", powerConnector: "1x 16-pin (12VHPWR)");
        productImages.Add(new ProductImage { ProductId = asus4090Id, ImageUrl = "https://dlcdnwebimgs.asus.com/gain/3f0f0f27-8c88-4b44-adb6-df13ca8d9fae/w800" });

        // MSI SUPRIM X RTX 4090
        var msi4090Id = Guid.NewGuid();
        context.Products.Add(new Product
        {
            Id = msi4090Id,
            Name = "MSI GeForce RTX 4090 SUPRIM X 24G",
            Sku = "GPU-RTX4090-MSI-SUPRIM-X",
            CategoryId = gpuCategory.Id,
            BrandId = msiBrand.Id,
            Price = 52990000,
            Stock = 10,
            Description = "Card đồ họa MSI RTX 4090 SUPRIM X với 24GB GDDR6X, thiết kế tản nhiệt TRI FROZR 3S với 3 quạt TORX 5.0. Mystic Light RGB, Zero Frozr technology. Hiệu năng đỉnh cao cho gaming 4K và AI.",
            WarrantyMonth = 36,
            Status = ProductStatus.Available,
        });
        AddGpuSpecs(specValues, gpuSpecs, msi4090Id, vram: 24, tdp: 450, length: 336, pcieSlot: "PCIe 4.0 x16", powerConnector: "1x 16-pin (12VHPWR)");
        productImages.Add(new ProductImage { ProductId = msi4090Id, ImageUrl = "https://asset.msi.com/resize/image/global/product/product_1664184282a71f6a3c6c19a1c1c7e4e3e5c1e5e4.png62405b38c58fe0f07fcef2367d8a9ba1/1024.png" });

        // Gigabyte AORUS Master RTX 4090
        var gigabyte4090Id = Guid.NewGuid();
        context.Products.Add(new Product
        {
            Id = gigabyte4090Id,
            Name = "Gigabyte AORUS GeForce RTX 4090 MASTER 24G",
            Sku = "GPU-RTX4090-GIGABYTE-AORUS-MASTER",
            CategoryId = gpuCategory.Id,
            BrandId = gigabyteBrand.Id,
            Price = 51990000,
            Stock = 12,
            Description = "Card đồ họa Gigabyte AORUS RTX 4090 MASTER với 24GB GDDR6X, hệ thống tản nhiệt WINDFORCE với 3 quạt. LCD Edge View hiển thị thông tin real-time, RGB Fusion 2.0. Card cao cấp cho enthusiast.",
            WarrantyMonth = 36,
            Status = ProductStatus.Available,
        });
        AddGpuSpecs(specValues, gpuSpecs, gigabyte4090Id, vram: 24, tdp: 450, length: 358, pcieSlot: "PCIe 4.0 x16", powerConnector: "1x 16-pin (12VHPWR)");
        productImages.Add(new ProductImage { ProductId = gigabyte4090Id, ImageUrl = "https://www.gigabyte.com/FileUpload/Global/KeyFeature/2238/innergigabyte/images/kf-img.png" });

        // ============= NVIDIA RTX 4080 SUPER =============

        // ASUS TUF Gaming RTX 4080 SUPER
        var asus4080sId = Guid.NewGuid();
        context.Products.Add(new Product
        {
            Id = asus4080sId,
            Name = "ASUS TUF Gaming GeForce RTX 4080 SUPER 16GB OC",
            Sku = "GPU-RTX4080S-ASUS-TUF-OC",
            CategoryId = gpuCategory.Id,
            BrandId = asusBrand.Id,
            Price = 32990000,
            Stock = 15,
            Description = "Card đồ họa ASUS TUF Gaming RTX 4080 SUPER OC với 16GB GDDR6X, thiết kế bền bỉ chuẩn quân sự. Tản nhiệt 3 quạt Axial-tech, dual ball fan bearings. Hiệu năng mạnh mẽ cho gaming 4K.",
            WarrantyMonth = 36,
            Status = ProductStatus.Available,
        });
        AddGpuSpecs(specValues, gpuSpecs, asus4080sId, vram: 16, tdp: 320, length: 348, pcieSlot: "PCIe 4.0 x16", powerConnector: "1x 16-pin (12VHPWR)");
        productImages.Add(new ProductImage { ProductId = asus4080sId, ImageUrl = "https://dlcdnwebimgs.asus.com/gain/e8c8f8e8-8c88-4b44-adb6-df13ca8d9fae/w800" });

        // MSI Gaming X Trio RTX 4080 SUPER
        var msi4080sId = Guid.NewGuid();
        context.Products.Add(new Product
        {
            Id = msi4080sId,
            Name = "MSI GeForce RTX 4080 SUPER 16G GAMING X TRIO",
            Sku = "GPU-RTX4080S-MSI-GAMING-X-TRIO",
            CategoryId = gpuCategory.Id,
            BrandId = msiBrand.Id,
            Price = 31990000,
            Stock = 18,
            Description = "Card đồ họa MSI RTX 4080 SUPER GAMING X TRIO với 16GB GDDR6X, tản nhiệt TRI FROZR 3 với 3 quạt TORX 5.0. Thiết kế đẹp mắt với Mystic Light RGB, Zero Frozr cho hoạt động êm ái.",
            WarrantyMonth = 36,
            Status = ProductStatus.Available,
        });
        AddGpuSpecs(specValues, gpuSpecs, msi4080sId, vram: 16, tdp: 320, length: 337, pcieSlot: "PCIe 4.0 x16", powerConnector: "1x 16-pin (12VHPWR)");
        productImages.Add(new ProductImage { ProductId = msi4080sId, ImageUrl = "https://asset.msi.com/resize/image/global/product/product_1705391282a71f6a3c6c19a1c1c7e4e3e5c1e5e4.png62405b38c58fe0f07fcef2367d8a9ba1/1024.png" });

        // Zotac Trinity RTX 4080 SUPER
        var zotac4080sId = Guid.NewGuid();
        context.Products.Add(new Product
        {
            Id = zotac4080sId,
            Name = "ZOTAC GAMING GeForce RTX 4080 SUPER Trinity Black",
            Sku = "GPU-RTX4080S-ZOTAC-TRINITY",
            CategoryId = gpuCategory.Id,
            BrandId = zotacBrand.Id,
            Price = 29990000,
            Stock = 20,
            Description = "Card đồ họa ZOTAC RTX 4080 SUPER Trinity Black với 16GB GDDR6X, thiết kế IceStorm 3.0 với 3 quạt. SPECTRA 2.0 RGB, FireStorm utility. Giải pháp gaming cao cấp với giá cạnh tranh.",
            WarrantyMonth = 36,
            Status = ProductStatus.Available,
        });
        AddGpuSpecs(specValues, gpuSpecs, zotac4080sId, vram: 16, tdp: 320, length: 306, pcieSlot: "PCIe 4.0 x16", powerConnector: "1x 16-pin (12VHPWR)");
        productImages.Add(new ProductImage { ProductId = zotac4080sId, ImageUrl = "https://www.zotac.com/download/files/styles/w1024/public/product_gallery/graphics_cards/zt-d40810d-10p-image01.jpg" });

        // ============= NVIDIA RTX 4070 Ti SUPER =============

        // Gigabyte Gaming OC RTX 4070 Ti SUPER
        var gigabyte4070tiSId = Guid.NewGuid();
        context.Products.Add(new Product
        {
            Id = gigabyte4070tiSId,
            Name = "Gigabyte GeForce RTX 4070 Ti SUPER GAMING OC 16G",
            Sku = "GPU-RTX4070TIS-GIGABYTE-GAMING-OC",
            CategoryId = gpuCategory.Id,
            BrandId = gigabyteBrand.Id,
            Price = 23990000,
            Stock = 22,
            Description = "Card đồ họa Gigabyte RTX 4070 Ti SUPER GAMING OC với 16GB GDDR6X, hệ thống tản nhiệt WINDFORCE với 3 quạt. RGB Fusion 2.0, ép xung sẵn từ nhà máy. Card tầm trung cao cấp cho gaming 1440p/4K.",
            WarrantyMonth = 36,
            Status = ProductStatus.Available,
        });
        AddGpuSpecs(specValues, gpuSpecs, gigabyte4070tiSId, vram: 16, tdp: 285, length: 329, pcieSlot: "PCIe 4.0 x16", powerConnector: "1x 16-pin (12VHPWR)");
        productImages.Add(new ProductImage { ProductId = gigabyte4070tiSId, ImageUrl = "https://www.gigabyte.com/FileUpload/Global/KeyFeature/2494/innergigabyte/images/kf-img.png" });

        // Palit GameRock RTX 4070 Ti SUPER
        var palit4070tiSId = Guid.NewGuid();
        context.Products.Add(new Product
        {
            Id = palit4070tiSId,
            Name = "Palit GeForce RTX 4070 Ti SUPER GameRock OC 16GB",
            Sku = "GPU-RTX4070TIS-PALIT-GAMEROCK",
            CategoryId = gpuCategory.Id,
            BrandId = palitBrand.Id,
            Price = 22490000,
            Stock = 25,
            Description = "Card đồ họa Palit RTX 4070 Ti SUPER GameRock OC với 16GB GDDR6X, thiết kế tản nhiệt TurboFan 3.0 với 3 quạt. ARGB LED lighting, DrMOS power stages. Hiệu năng cao với mức giá hợp lý.",
            WarrantyMonth = 36,
            Status = ProductStatus.Available,
        });
        AddGpuSpecs(specValues, gpuSpecs, palit4070tiSId, vram: 16, tdp: 285, length: 329, pcieSlot: "PCIe 4.0 x16", powerConnector: "1x 16-pin (12VHPWR)");
        productImages.Add(new ProductImage { ProductId = palit4070tiSId, ImageUrl = "https://www.palit.com/palit/vgapro/img/ne6407ts19t2-1043g.png" });

        // ============= NVIDIA RTX 4070 SUPER =============

        // MSI Ventus 3X RTX 4070 SUPER
        var msi4070sId = Guid.NewGuid();
        context.Products.Add(new Product
        {
            Id = msi4070sId,
            Name = "MSI GeForce RTX 4070 SUPER 12G VENTUS 3X OC",
            Sku = "GPU-RTX4070S-MSI-VENTUS-3X",
            CategoryId = gpuCategory.Id,
            BrandId = msiBrand.Id,
            Price = 16990000,
            Stock = 30,
            Description = "Card đồ họa MSI RTX 4070 SUPER VENTUS 3X OC với 12GB GDDR6X, tản nhiệt 3 quạt TORX 4.0. Thiết kế tinh gọn, hiệu suất ổn định. Lựa chọn tuyệt vời cho gaming 1440p.",
            WarrantyMonth = 36,
            Status = ProductStatus.Available,
        });
        AddGpuSpecs(specValues, gpuSpecs, msi4070sId, vram: 12, tdp: 220, length: 308, pcieSlot: "PCIe 4.0 x16", powerConnector: "1x 16-pin (12VHPWR)");
        productImages.Add(new ProductImage { ProductId = msi4070sId, ImageUrl = "https://asset.msi.com/resize/image/global/product/product_1705391282a71f6a3c6c19a1c1c7e4e3e5c1e5e5.png62405b38c58fe0f07fcef2367d8a9ba1/1024.png" });

        // Galax RTX 4070 SUPER EX Gamer
        var galax4070sId = Guid.NewGuid();
        context.Products.Add(new Product
        {
            Id = galax4070sId,
            Name = "GALAX GeForce RTX 4070 SUPER EX Gamer 12GB",
            Sku = "GPU-RTX4070S-GALAX-EX-GAMER",
            CategoryId = gpuCategory.Id,
            BrandId = galaxBrand.Id,
            Price = 15990000,
            Stock = 28,
            Description = "Card đồ họa GALAX RTX 4070 SUPER EX Gamer với 12GB GDDR6X, thiết kế 2.5 slot với 3 quạt. Infinity LED Edge lighting, Xtreme Tuner Plus software. Card gaming tầm trung với hiệu năng tốt.",
            WarrantyMonth = 36,
            Status = ProductStatus.Available,
        });
        AddGpuSpecs(specValues, gpuSpecs, galax4070sId, vram: 12, tdp: 220, length: 302, pcieSlot: "PCIe 4.0 x16", powerConnector: "1x 16-pin (12VHPWR)");
        productImages.Add(new ProductImage { ProductId = galax4070sId, ImageUrl = "https://www.galax.com/en/graphics-card/40-series/geforce-rtx-4070-super-ex-gamer.html" });

        // ============= NVIDIA RTX 4070 =============

        // INNO3D RTX 4070 TWIN X2 (existing)
        var rtx4070Id = Guid.NewGuid();
        context.Products.Add(new Product
        {
            Id = rtx4070Id,
            Name = "INNO3D GeForce RTX 4070 TWIN X2",
            Sku = "GPU-RTX4070-INNO3D-TWINX2",
            CategoryId = gpuCategory.Id,
            BrandId = inno3dBrand.Id,
            Price = 14990000,
            Stock = 25,
            Description = "Card đồ họa INNO3D GeForce RTX 4070 TWIN X2 với kiến trúc Ada Lovelace, 12GB GDDR6X, hỗ trợ Ray Tracing và DLSS 3.0. Thiết kế tản nhiệt 2 quạt hiệu quả, phù hợp cho gaming 1440p và làm việc sáng tạo.",
            WarrantyMonth = 36,
            Status = ProductStatus.Available,
        });
        AddGpuSpecs(specValues, gpuSpecs, rtx4070Id, vram: 12, tdp: 200, length: 267, pcieSlot: "PCIe 4.0 x16", powerConnector: "1x 8-pin");
        productImages.AddRange(new[]
        {
            new ProductImage { ProductId = rtx4070Id, ImageUrl = "https://inno3d.com/uploads/product/n40702-12d6x-171xxin/n40702-12d6x-171xxin_img_01-e8a4d0c3.webp" },
            new ProductImage { ProductId = rtx4070Id, ImageUrl = "https://inno3d.com/uploads/product/n40702-12d6x-171xxin/n40702-12d6x-171xxin_img_02-d3e76b21.webp" },
        });

        // ASUS Dual RTX 4070
        var asus4070Id = Guid.NewGuid();
        context.Products.Add(new Product
        {
            Id = asus4070Id,
            Name = "ASUS Dual GeForce RTX 4070 OC Edition 12GB",
            Sku = "GPU-RTX4070-ASUS-DUAL-OC",
            CategoryId = gpuCategory.Id,
            BrandId = asusBrand.Id,
            Price = 15490000,
            Stock = 32,
            Description = "Card đồ họa ASUS Dual RTX 4070 OC với 12GB GDDR6X, thiết kế 2.5 slot với 2 quạt Axial-tech. Auto-Extreme Technology, GPU Tweak III. Cân bằng tốt giữa hiệu năng và kích thước.",
            WarrantyMonth = 36,
            Status = ProductStatus.Available,
        });
        AddGpuSpecs(specValues, gpuSpecs, asus4070Id, vram: 12, tdp: 200, length: 267, pcieSlot: "PCIe 4.0 x16", powerConnector: "1x 8-pin");
        productImages.Add(new ProductImage { ProductId = asus4070Id, ImageUrl = "https://dlcdnwebimgs.asus.com/gain/2f0f0f27-8c88-4b44-adb6-df13ca8d9faf/w800" });

        // Colorful iGame RTX 4070 Ultra W DUO OC
        var colorful4070Id = Guid.NewGuid();
        context.Products.Add(new Product
        {
            Id = colorful4070Id,
            Name = "Colorful iGame GeForce RTX 4070 Ultra W DUO OC 12GB",
            Sku = "GPU-RTX4070-COLORFUL-ULTRA-W",
            CategoryId = gpuCategory.Id,
            BrandId = colorfulBrand.Id,
            Price = 14490000,
            Stock = 30,
            Description = "Card đồ họa Colorful iGame RTX 4070 Ultra W DUO OC với 12GB GDDR6X, thiết kế trắng tinh khiết với 2 quạt. iGame Center software, LED lighting effects. Card đồ họa phong cách cho build trắng.",
            WarrantyMonth = 36,
            Status = ProductStatus.Available,
        });
        AddGpuSpecs(specValues, gpuSpecs, colorful4070Id, vram: 12, tdp: 200, length: 275, pcieSlot: "PCIe 4.0 x16", powerConnector: "1x 8-pin");
        productImages.Add(new ProductImage { ProductId = colorful4070Id, ImageUrl = "https://en.colorful.cn/product/GeForce-RTX-4070-Ultra-W-DUO-OC-12GB.html" });

        // ============= NVIDIA RTX 4060 Ti =============

        // Gigabyte EAGLE OC RTX 4060 Ti
        var gigabyte4060tiId = Guid.NewGuid();
        context.Products.Add(new Product
        {
            Id = gigabyte4060tiId,
            Name = "Gigabyte GeForce RTX 4060 Ti EAGLE OC 8G",
            Sku = "GPU-RTX4060TI-GIGABYTE-EAGLE-OC",
            CategoryId = gpuCategory.Id,
            BrandId = gigabyteBrand.Id,
            Price = 11490000,
            Stock = 35,
            Description = "Card đồ họa Gigabyte RTX 4060 Ti EAGLE OC với 8GB GDDR6, hệ thống tản nhiệt WINDFORCE với 2 quạt. Thiết kế compact, ép xung sẵn từ nhà máy. Card gaming 1080p tốt nhất phân khúc.",
            WarrantyMonth = 36,
            Status = ProductStatus.Available,
        });
        AddGpuSpecs(specValues, gpuSpecs, gigabyte4060tiId, vram: 8, tdp: 160, length: 261, pcieSlot: "PCIe 4.0 x16", powerConnector: "1x 8-pin");
        productImages.Add(new ProductImage { ProductId = gigabyte4060tiId, ImageUrl = "https://www.gigabyte.com/FileUpload/Global/KeyFeature/2428/innergigabyte/images/kf-img.png" });

        // PNY VERTO Dual Fan RTX 4060 Ti
        var pny4060tiId = Guid.NewGuid();
        context.Products.Add(new Product
        {
            Id = pny4060tiId,
            Name = "PNY GeForce RTX 4060 Ti 8GB VERTO Dual Fan",
            Sku = "GPU-RTX4060TI-PNY-VERTO",
            CategoryId = gpuCategory.Id,
            BrandId = pnyBrand.Id,
            Price = 10990000,
            Stock = 40,
            Description = "Card đồ họa PNY RTX 4060 Ti VERTO với 8GB GDDR6, thiết kế 2 quạt hiệu quả. EPIC-X RGB lighting, dual fan cooling. Giải pháp gaming tầm trung với giá tốt.",
            WarrantyMonth = 36,
            Status = ProductStatus.Available,
        });
        AddGpuSpecs(specValues, gpuSpecs, pny4060tiId, vram: 8, tdp: 160, length: 250, pcieSlot: "PCIe 4.0 x16", powerConnector: "1x 8-pin");
        productImages.Add(new ProductImage { ProductId = pny4060tiId, ImageUrl = "https://www.pny.com/file%20library/product%20images/graphics%20cards/geforce/rtx-4060-ti-verto.png" });

        // ============= NVIDIA RTX 4060 =============

        // MSI Ventus 2X RTX 4060
        var msi4060Id = Guid.NewGuid();
        context.Products.Add(new Product
        {
            Id = msi4060Id,
            Name = "MSI GeForce RTX 4060 VENTUS 2X BLACK 8G OC",
            Sku = "GPU-RTX4060-MSI-VENTUS-2X",
            CategoryId = gpuCategory.Id,
            BrandId = msiBrand.Id,
            Price = 8490000,
            Stock = 45,
            Description = "Card đồ họa MSI RTX 4060 VENTUS 2X BLACK OC với 8GB GDDR6, tản nhiệt 2 quạt TORX 4.0. Thiết kế nhỏ gọn phù hợp nhiều case, Zero Frozr technology. Card gaming 1080p hiệu quả.",
            WarrantyMonth = 36,
            Status = ProductStatus.Available,
        });
        AddGpuSpecs(specValues, gpuSpecs, msi4060Id, vram: 8, tdp: 115, length: 199, pcieSlot: "PCIe 4.0 x16", powerConnector: "1x 8-pin");
        productImages.Add(new ProductImage { ProductId = msi4060Id, ImageUrl = "https://asset.msi.com/resize/image/global/product/product_1684391282a71f6a3c6c19a1c1c7e4e3e5c1e5e4.png62405b38c58fe0f07fcef2367d8a9ba1/1024.png" });

        // INNO3D RTX 4060 TWIN X2
        var inno3d4060Id = Guid.NewGuid();
        context.Products.Add(new Product
        {
            Id = inno3d4060Id,
            Name = "INNO3D GeForce RTX 4060 TWIN X2 8GB",
            Sku = "GPU-RTX4060-INNO3D-TWINX2",
            CategoryId = gpuCategory.Id,
            BrandId = inno3dBrand.Id,
            Price = 7990000,
            Stock = 50,
            Description = "Card đồ họa INNO3D RTX 4060 TWIN X2 với 8GB GDDR6, thiết kế 2 quạt hiệu quả. Hỗ trợ DLSS 3.0, Ray Tracing. Card gaming entry-level với công nghệ mới nhất.",
            WarrantyMonth = 36,
            Status = ProductStatus.Available,
        });
        AddGpuSpecs(specValues, gpuSpecs, inno3d4060Id, vram: 8, tdp: 115, length: 240, pcieSlot: "PCIe 4.0 x16", powerConnector: "1x 8-pin");
        productImages.Add(new ProductImage { ProductId = inno3d4060Id, ImageUrl = "https://inno3d.com/uploads/product/n40602-08d6-171xxin/n40602-08d6-171xxin_img_01.webp" });

        // ============= NVIDIA RTX 3060 =============

        // Palit Dual RTX 3060
        var palit3060Id = Guid.NewGuid();
        context.Products.Add(new Product
        {
            Id = palit3060Id,
            Name = "Palit GeForce RTX 3060 Dual 12GB",
            Sku = "GPU-RTX3060-PALIT-DUAL",
            CategoryId = gpuCategory.Id,
            BrandId = palitBrand.Id,
            Price = 6490000,
            Stock = 55,
            Description = "Card đồ họa Palit RTX 3060 Dual với 12GB GDDR6, thiết kế 2 quạt TurboFan 2.0. Kiến trúc Ampere, hỗ trợ Ray Tracing và DLSS. Card gaming phổ thông với VRAM lớn.",
            WarrantyMonth = 36,
            Status = ProductStatus.Available,
        });
        AddGpuSpecs(specValues, gpuSpecs, palit3060Id, vram: 12, tdp: 170, length: 245, pcieSlot: "PCIe 4.0 x16", powerConnector: "1x 8-pin");
        productImages.Add(new ProductImage { ProductId = palit3060Id, ImageUrl = "https://www.palit.com/palit/vgapro/img/ne63060019k9-190af.png" });

        // Galax RTX 3060 (1-Click OC)
        var galax3060Id = Guid.NewGuid();
        context.Products.Add(new Product
        {
            Id = galax3060Id,
            Name = "GALAX GeForce RTX 3060 (1-Click OC) 12GB",
            Sku = "GPU-RTX3060-GALAX-1CLICK",
            CategoryId = gpuCategory.Id,
            BrandId = galaxBrand.Id,
            Price = 6290000,
            Stock = 48,
            Description = "Card đồ họa GALAX RTX 3060 (1-Click OC) với 12GB GDDR6, tính năng 1-Click OC để ép xung nhanh chóng. Thiết kế 2 quạt, Xtreme Tuner Plus software. Card gaming giá tốt.",
            WarrantyMonth = 36,
            Status = ProductStatus.Available,
        });
        AddGpuSpecs(specValues, gpuSpecs, galax3060Id, vram: 12, tdp: 170, length: 235, pcieSlot: "PCIe 4.0 x16", powerConnector: "1x 8-pin");
        productImages.Add(new ProductImage { ProductId = galax3060Id, ImageUrl = "https://www.galax.com/en/graphics-card/30-series/geforce-rtx-3060-1-click-oc.html" });

        // ============= AMD RADEON RX 7900 XTX =============

        // Sapphire NITRO+ RX 7900 XTX
        var sapphire7900xtxId = Guid.NewGuid();
        context.Products.Add(new Product
        {
            Id = sapphire7900xtxId,
            Name = "Sapphire NITRO+ AMD Radeon RX 7900 XTX Vapor-X 24GB",
            Sku = "GPU-RX7900XTX-SAPPHIRE-NITRO-VAPORX",
            CategoryId = gpuCategory.Id,
            BrandId = sapphireBrand.Id,
            Price = 32990000,
            Stock = 12,
            Description = "Card đồ họa Sapphire NITRO+ RX 7900 XTX Vapor-X với 24GB GDDR6, hệ thống tản nhiệt Vapor-X cooling với 3 quạt. ARGB lighting, Dual BIOS. Card AMD flagship cho gaming 4K.",
            WarrantyMonth = 36,
            Status = ProductStatus.Available,
        });
        AddGpuSpecs(specValues, gpuSpecs, sapphire7900xtxId, vram: 24, tdp: 355, length: 320, pcieSlot: "PCIe 4.0 x16", powerConnector: "2x 8-pin");
        productImages.Add(new ProductImage { ProductId = sapphire7900xtxId, ImageUrl = "https://www.sapphiretech.com/images/products/7900xtx-nitro-vaporx.png" });

        // XFX MERC 310 RX 7900 XTX
        var xfx7900xtxId = Guid.NewGuid();
        context.Products.Add(new Product
        {
            Id = xfx7900xtxId,
            Name = "XFX Speedster MERC 310 AMD Radeon RX 7900 XTX 24GB",
            Sku = "GPU-RX7900XTX-XFX-MERC310",
            CategoryId = gpuCategory.Id,
            BrandId = xfxBrand.Id,
            Price = 29990000,
            Stock = 15,
            Description = "Card đồ họa XFX Speedster MERC 310 RX 7900 XTX với 24GB GDDR6, hệ thống tản nhiệt 3 quạt hiệu quả. Thiết kế chắc chắn, dual BIOS. Card AMD high-end với giá cạnh tranh.",
            WarrantyMonth = 36,
            Status = ProductStatus.Available,
        });
        AddGpuSpecs(specValues, gpuSpecs, xfx7900xtxId, vram: 24, tdp: 355, length: 344, pcieSlot: "PCIe 4.0 x16", powerConnector: "2x 8-pin");
        productImages.Add(new ProductImage { ProductId = xfx7900xtxId, ImageUrl = "https://www.xfxforce.com/images/products/rx-7900-xtx-merc-310.png" });

        // PowerColor Red Devil RX 7900 XTX
        var powercolor7900xtxId = Guid.NewGuid();
        context.Products.Add(new Product
        {
            Id = powercolor7900xtxId,
            Name = "PowerColor Red Devil AMD Radeon RX 7900 XTX 24GB",
            Sku = "GPU-RX7900XTX-POWERCOLOR-REDDEVIL",
            CategoryId = gpuCategory.Id,
            BrandId = powerColorBrand.Id,
            Price = 31990000,
            Stock = 10,
            Description = "Card đồ họa PowerColor Red Devil RX 7900 XTX với 24GB GDDR6, thiết kế tản nhiệt 3 quạt với Devil Zone RGB. Triple BIOS, DrMOS power stages. Card AMD cao cấp với phong cách độc đáo.",
            WarrantyMonth = 36,
            Status = ProductStatus.Available,
        });
        AddGpuSpecs(specValues, gpuSpecs, powercolor7900xtxId, vram: 24, tdp: 355, length: 332, pcieSlot: "PCIe 4.0 x16", powerConnector: "2x 8-pin");
        productImages.Add(new ProductImage { ProductId = powercolor7900xtxId, ImageUrl = "https://www.powercolor.com/images/products/rx-7900-xtx-red-devil.png" });

        // ============= AMD RADEON RX 7900 XT =============

        // Sapphire PULSE RX 7900 XT
        var sapphire7900xtId = Guid.NewGuid();
        context.Products.Add(new Product
        {
            Id = sapphire7900xtId,
            Name = "Sapphire PULSE AMD Radeon RX 7900 XT 20GB",
            Sku = "GPU-RX7900XT-SAPPHIRE-PULSE",
            CategoryId = gpuCategory.Id,
            BrandId = sapphireBrand.Id,
            Price = 24990000,
            Stock = 18,
            Description = "Card đồ họa Sapphire PULSE RX 7900 XT với 20GB GDDR6, hệ thống tản nhiệt Dual-X với 2 quạt. Thiết kế tinh gọn hơn NITRO+, hiệu năng xuất sắc cho gaming 4K.",
            WarrantyMonth = 36,
            Status = ProductStatus.Available,
        });
        AddGpuSpecs(specValues, gpuSpecs, sapphire7900xtId, vram: 20, tdp: 315, length: 280, pcieSlot: "PCIe 4.0 x16", powerConnector: "2x 8-pin");
        productImages.Add(new ProductImage { ProductId = sapphire7900xtId, ImageUrl = "https://www.sapphiretech.com/images/products/7900xt-pulse.png" });

        // ASUS TUF Gaming RX 7900 XT
        var asus7900xtId = Guid.NewGuid();
        context.Products.Add(new Product
        {
            Id = asus7900xtId,
            Name = "ASUS TUF Gaming Radeon RX 7900 XT OC Edition 20GB",
            Sku = "GPU-RX7900XT-ASUS-TUF-OC",
            CategoryId = gpuCategory.Id,
            BrandId = asusBrand.Id,
            Price = 26990000,
            Stock = 14,
            Description = "Card đồ họa ASUS TUF Gaming RX 7900 XT OC với 20GB GDDR6, thiết kế bền bỉ chuẩn quân sự. Tản nhiệt 3 quạt Axial-tech, GPU Tweak III. Card AMD bền bỉ cho gaming enthusiast.",
            WarrantyMonth = 36,
            Status = ProductStatus.Available,
        });
        AddGpuSpecs(specValues, gpuSpecs, asus7900xtId, vram: 20, tdp: 315, length: 353, pcieSlot: "PCIe 4.0 x16", powerConnector: "2x 8-pin");
        productImages.Add(new ProductImage { ProductId = asus7900xtId, ImageUrl = "https://dlcdnwebimgs.asus.com/gain/4f0f0f27-8c88-4b44-adb6-df13ca8d9fae/w800" });

        // ============= AMD RADEON RX 7800 XT =============

        // PowerColor Red Dragon RX 7800 XT
        var powercolor7800xtId = Guid.NewGuid();
        context.Products.Add(new Product
        {
            Id = powercolor7800xtId,
            Name = "PowerColor Red Dragon AMD Radeon RX 7800 XT 16GB",
            Sku = "GPU-RX7800XT-POWERCOLOR-REDDRAGON",
            CategoryId = gpuCategory.Id,
            BrandId = powerColorBrand.Id,
            Price = 14990000,
            Stock = 25,
            Description = "Card đồ họa PowerColor Red Dragon RX 7800 XT với 16GB GDDR6, thiết kế 3 quạt hiệu quả. Card AMD tầm trung cao cấp, cạnh tranh trực tiếp với RTX 4070.",
            WarrantyMonth = 36,
            Status = ProductStatus.Available,
        });
        AddGpuSpecs(specValues, gpuSpecs, powercolor7800xtId, vram: 16, tdp: 263, length: 305, pcieSlot: "PCIe 4.0 x16", powerConnector: "2x 8-pin");
        productImages.Add(new ProductImage { ProductId = powercolor7800xtId, ImageUrl = "https://www.powercolor.com/images/products/rx-7800-xt-red-dragon.png" });

        // XFX QICK 319 RX 7800 XT
        var xfx7800xtId = Guid.NewGuid();
        context.Products.Add(new Product
        {
            Id = xfx7800xtId,
            Name = "XFX Speedster QICK 319 AMD Radeon RX 7800 XT 16GB",
            Sku = "GPU-RX7800XT-XFX-QICK319",
            CategoryId = gpuCategory.Id,
            BrandId = xfxBrand.Id,
            Price = 13990000,
            Stock = 28,
            Description = "Card đồ họa XFX Speedster QICK 319 RX 7800 XT với 16GB GDDR6, hệ thống tản nhiệt 3 quạt. Thiết kế chắc chắn, hiệu năng tuyệt vời cho gaming 1440p.",
            WarrantyMonth = 36,
            Status = ProductStatus.Available,
        });
        AddGpuSpecs(specValues, gpuSpecs, xfx7800xtId, vram: 16, tdp: 263, length: 322, pcieSlot: "PCIe 4.0 x16", powerConnector: "2x 8-pin");
        productImages.Add(new ProductImage { ProductId = xfx7800xtId, ImageUrl = "https://www.xfxforce.com/images/products/rx-7800-xt-qick-319.png" });

        // Sapphire NITRO+ RX 7800 XT
        var sapphire7800xtId = Guid.NewGuid();
        context.Products.Add(new Product
        {
            Id = sapphire7800xtId,
            Name = "Sapphire NITRO+ AMD Radeon RX 7800 XT 16GB",
            Sku = "GPU-RX7800XT-SAPPHIRE-NITRO",
            CategoryId = gpuCategory.Id,
            BrandId = sapphireBrand.Id,
            Price = 15490000,
            Stock = 22,
            Description = "Card đồ họa Sapphire NITRO+ RX 7800 XT với 16GB GDDR6, hệ thống tản nhiệt Tri-X với 3 quạt. ARGB Fan, Dual BIOS. Card AMD tầm trung cao cấp với build quality tuyệt vời.",
            WarrantyMonth = 36,
            Status = ProductStatus.Available,
        });
        AddGpuSpecs(specValues, gpuSpecs, sapphire7800xtId, vram: 16, tdp: 263, length: 310, pcieSlot: "PCIe 4.0 x16", powerConnector: "2x 8-pin");
        productImages.Add(new ProductImage { ProductId = sapphire7800xtId, ImageUrl = "https://www.sapphiretech.com/images/products/7800xt-nitro.png" });

        // ============= AMD RADEON RX 7700 XT =============

        // Gigabyte Gaming OC RX 7700 XT
        var gigabyte7700xtId = Guid.NewGuid();
        context.Products.Add(new Product
        {
            Id = gigabyte7700xtId,
            Name = "Gigabyte Radeon RX 7700 XT GAMING OC 12G",
            Sku = "GPU-RX7700XT-GIGABYTE-GAMING-OC",
            CategoryId = gpuCategory.Id,
            BrandId = gigabyteBrand.Id,
            Price = 11990000,
            Stock = 30,
            Description = "Card đồ họa Gigabyte RX 7700 XT GAMING OC với 12GB GDDR6, hệ thống tản nhiệt WINDFORCE với 3 quạt. RGB Fusion 2.0, ép xung sẵn. Card AMD tầm trung tốt cho gaming 1440p.",
            WarrantyMonth = 36,
            Status = ProductStatus.Available,
        });
        AddGpuSpecs(specValues, gpuSpecs, gigabyte7700xtId, vram: 12, tdp: 245, length: 302, pcieSlot: "PCIe 4.0 x16", powerConnector: "2x 8-pin");
        productImages.Add(new ProductImage { ProductId = gigabyte7700xtId, ImageUrl = "https://www.gigabyte.com/FileUpload/Global/KeyFeature/2456/innergigabyte/images/kf-img.png" });

        // MSI Gaming X RX 7700 XT
        var msi7700xtId = Guid.NewGuid();
        context.Products.Add(new Product
        {
            Id = msi7700xtId,
            Name = "MSI Radeon RX 7700 XT GAMING X TRIO 12G",
            Sku = "GPU-RX7700XT-MSI-GAMING-X-TRIO",
            CategoryId = gpuCategory.Id,
            BrandId = msiBrand.Id,
            Price = 12490000,
            Stock = 26,
            Description = "Card đồ họa MSI RX 7700 XT GAMING X TRIO với 12GB GDDR6, tản nhiệt TRI FROZR 3 với 3 quạt TORX 5.0. Mystic Light RGB, Zero Frozr. Card AMD gaming với thiết kế cao cấp.",
            WarrantyMonth = 36,
            Status = ProductStatus.Available,
        });
        AddGpuSpecs(specValues, gpuSpecs, msi7700xtId, vram: 12, tdp: 245, length: 325, pcieSlot: "PCIe 4.0 x16", powerConnector: "2x 8-pin");
        productImages.Add(new ProductImage { ProductId = msi7700xtId, ImageUrl = "https://asset.msi.com/resize/image/global/product/product_1693391282a71f6a3c6c19a1c1c7e4e3e5c1e5e4.png62405b38c58fe0f07fcef2367d8a9ba1/1024.png" });

        // ============= AMD RADEON RX 7600 =============

        // Sapphire PULSE RX 7600
        var sapphire7600Id = Guid.NewGuid();
        context.Products.Add(new Product
        {
            Id = sapphire7600Id,
            Name = "Sapphire PULSE AMD Radeon RX 7600 8GB",
            Sku = "GPU-RX7600-SAPPHIRE-PULSE",
            CategoryId = gpuCategory.Id,
            BrandId = sapphireBrand.Id,
            Price = 7490000,
            Stock = 40,
            Description = "Card đồ họa Sapphire PULSE RX 7600 với 8GB GDDR6, hệ thống tản nhiệt Dual-X với 2 quạt. Thiết kế compact, hiệu năng tốt cho gaming 1080p. Card AMD entry-level thế hệ mới.",
            WarrantyMonth = 36,
            Status = ProductStatus.Available,
        });
        AddGpuSpecs(specValues, gpuSpecs, sapphire7600Id, vram: 8, tdp: 165, length: 260, pcieSlot: "PCIe 4.0 x16", powerConnector: "1x 8-pin");
        productImages.Add(new ProductImage { ProductId = sapphire7600Id, ImageUrl = "https://www.sapphiretech.com/images/products/7600-pulse.png" });

        // PowerColor Fighter RX 7600
        var powercolor7600Id = Guid.NewGuid();
        context.Products.Add(new Product
        {
            Id = powercolor7600Id,
            Name = "PowerColor Fighter AMD Radeon RX 7600 8GB",
            Sku = "GPU-RX7600-POWERCOLOR-FIGHTER",
            CategoryId = gpuCategory.Id,
            BrandId = powerColorBrand.Id,
            Price = 6990000,
            Stock = 45,
            Description = "Card đồ họa PowerColor Fighter RX 7600 với 8GB GDDR6, thiết kế 2 quạt hiệu quả. Card AMD giá tốt cho gaming 1080p với kiến trúc RDNA 3 mới nhất.",
            WarrantyMonth = 36,
            Status = ProductStatus.Available,
        });
        AddGpuSpecs(specValues, gpuSpecs, powercolor7600Id, vram: 8, tdp: 165, length: 256, pcieSlot: "PCIe 4.0 x16", powerConnector: "1x 8-pin");
        productImages.Add(new ProductImage { ProductId = powercolor7600Id, ImageUrl = "https://www.powercolor.com/images/products/rx-7600-fighter.png" });

        // XFX SWFT 210 RX 7600
        var xfx7600Id = Guid.NewGuid();
        context.Products.Add(new Product
        {
            Id = xfx7600Id,
            Name = "XFX Speedster SWFT 210 AMD Radeon RX 7600 8GB",
            Sku = "GPU-RX7600-XFX-SWFT210",
            CategoryId = gpuCategory.Id,
            BrandId = xfxBrand.Id,
            Price = 6790000,
            Stock = 48,
            Description = "Card đồ họa XFX Speedster SWFT 210 RX 7600 với 8GB GDDR6, thiết kế 2 quạt nhỏ gọn. Dual BIOS, build quality tốt. Card AMD budget-friendly cho gamer.",
            WarrantyMonth = 36,
            Status = ProductStatus.Available,
        });
        AddGpuSpecs(specValues, gpuSpecs, xfx7600Id, vram: 8, tdp: 165, length: 240, pcieSlot: "PCIe 4.0 x16", powerConnector: "1x 8-pin");
        productImages.Add(new ProductImage { ProductId = xfx7600Id, ImageUrl = "https://www.xfxforce.com/images/products/rx-7600-swft-210.png" });

        // ============= AMD RADEON RX 6700 XT =============

        // Sapphire NITRO+ RX 6700 XT
        var sapphire6700xtId = Guid.NewGuid();
        context.Products.Add(new Product
        {
            Id = sapphire6700xtId,
            Name = "Sapphire NITRO+ AMD Radeon RX 6700 XT 12GB",
            Sku = "GPU-RX6700XT-SAPPHIRE-NITRO",
            CategoryId = gpuCategory.Id,
            BrandId = sapphireBrand.Id,
            Price = 8990000,
            Stock = 35,
            Description = "Card đồ họa Sapphire NITRO+ RX 6700 XT với 12GB GDDR6, hệ thống tản nhiệt Tri-X với 3 quạt. ARGB LED, Dual BIOS. Card AMD tầm trung thế hệ trước với VRAM lớn.",
            WarrantyMonth = 36,
            Status = ProductStatus.Available,
        });
        AddGpuSpecs(specValues, gpuSpecs, sapphire6700xtId, vram: 12, tdp: 230, length: 310, pcieSlot: "PCIe 4.0 x16", powerConnector: "1x 8-pin + 1x 6-pin");
        productImages.Add(new ProductImage { ProductId = sapphire6700xtId, ImageUrl = "https://www.sapphiretech.com/images/products/6700xt-nitro.png" });

        // ============= AMD RADEON RX 6600 =============

        // PowerColor Fighter RX 6600
        var powercolor6600Id = Guid.NewGuid();
        context.Products.Add(new Product
        {
            Id = powercolor6600Id,
            Name = "PowerColor Fighter AMD Radeon RX 6600 8GB",
            Sku = "GPU-RX6600-POWERCOLOR-FIGHTER",
            CategoryId = gpuCategory.Id,
            BrandId = powerColorBrand.Id,
            Price = 5490000,
            Stock = 50,
            Description = "Card đồ họa PowerColor Fighter RX 6600 với 8GB GDDR6, thiết kế 2 quạt nhỏ gọn. Card AMD budget cho gaming 1080p, tiêu thụ điện năng thấp.",
            WarrantyMonth = 36,
            Status = ProductStatus.Available,
        });
        AddGpuSpecs(specValues, gpuSpecs, powercolor6600Id, vram: 8, tdp: 132, length: 230, pcieSlot: "PCIe 4.0 x16", powerConnector: "1x 8-pin");
        productImages.Add(new ProductImage { ProductId = powercolor6600Id, ImageUrl = "https://www.powercolor.com/images/products/rx-6600-fighter.png" });

        // XFX SWFT 210 RX 6600
        var xfx6600Id = Guid.NewGuid();
        context.Products.Add(new Product
        {
            Id = xfx6600Id,
            Name = "XFX Speedster SWFT 210 AMD Radeon RX 6600 8GB",
            Sku = "GPU-RX6600-XFX-SWFT210",
            CategoryId = gpuCategory.Id,
            BrandId = xfxBrand.Id,
            Price = 5290000,
            Stock = 55,
            Description = "Card đồ họa XFX Speedster SWFT 210 RX 6600 với 8GB GDDR6, thiết kế 2 quạt hiệu quả. Card AMD entry-level giá tốt nhất cho gaming 1080p.",
            WarrantyMonth = 36,
            Status = ProductStatus.Available,
        });
        AddGpuSpecs(specValues, gpuSpecs, xfx6600Id, vram: 8, tdp: 132, length: 235, pcieSlot: "PCIe 4.0 x16", powerConnector: "1x 8-pin");
        productImages.Add(new ProductImage { ProductId = xfx6600Id, ImageUrl = "https://www.xfxforce.com/images/products/rx-6600-swft-210.png" });
    }

    private static async Task InitCpuProducts(ApplicationDbContext context, List<ProductSpecValue> specValues, List<ProductImage> productImages)
    {
        var cpuCategory = await context.Categories.FirstOrDefaultAsync(c => c.Name == "CPU")
            ?? throw new NotFoundException("Không tìm thấy danh mục CPU");

        var cpuSpecs = await context.SpecDefinitions
            .Where(s => s.CategoryId == cpuCategory.Id)
            .ToListAsync();

        var intelBrand = await context.Brands.FirstOrDefaultAsync(b => b.Name == "Intel")
            ?? throw new NotFoundException("Không tìm thấy thương hiệu Intel");
        var amdBrand = await context.Brands.FirstOrDefaultAsync(b => b.Name == "AMD")
            ?? throw new NotFoundException("Không tìm thấy thương hiệu AMD");

        // ============= INTEL CPUs =============

        // Intel Core i3-12100F (Low-end, no iGPU)
        var i3_12100f_Id = Guid.NewGuid();
        context.Products.Add(new Product
        {
            Id = i3_12100f_Id,
            Name = "Intel Core i3-12100F",
            Sku = "CPU-INTEL-I3-12100F",
            CategoryId = cpuCategory.Id,
            BrandId = intelBrand.Id,
            Price = 2290000,
            Stock = 50,
            Description = "Bộ vi xử lý Intel Core i3-12100F thế hệ 12 Alder Lake, 4 nhân 8 luồng, xung nhịp cơ bản 3.3GHz, turbo lên đến 4.3GHz. Lựa chọn tiết kiệm cho PC văn phòng và gaming nhẹ.",
            WarrantyMonth = 36,
            Status = ProductStatus.Available,
        });
        AddCpuSpecs(specValues, cpuSpecs, i3_12100f_Id, socket: "LGA1700", cores: 4, threads: 8, baseClock: 3.3m, boostClock: 4.3m, tdp: 58, memoryType: "DDR4, DDR5", hasIgpu: false);
        productImages.Add(new ProductImage { ProductId = i3_12100f_Id, ImageUrl = "https://ark.intel.com/content/dam/ark/assets/images/box-shots/Intel%20Core%2012th%20Gen%20i3.png" });

        // Intel Core i3-14100 (Low-end, with iGPU)
        var i3_14100_Id = Guid.NewGuid();
        context.Products.Add(new Product
        {
            Id = i3_14100_Id,
            Name = "Intel Core i3-14100",
            Sku = "CPU-INTEL-I3-14100",
            CategoryId = cpuCategory.Id,
            BrandId = intelBrand.Id,
            Price = 3190000,
            Stock = 40,
            Description = "Bộ vi xử lý Intel Core i3-14100 thế hệ 14, 4 nhân 8 luồng, xung nhịp cơ bản 3.5GHz, turbo lên đến 4.7GHz. Tích hợp Intel UHD Graphics 730, phù hợp cho PC văn phòng và giải trí.",
            WarrantyMonth = 36,
            Status = ProductStatus.Available,
        });
        AddCpuSpecs(specValues, cpuSpecs, i3_14100_Id, socket: "LGA1700", cores: 4, threads: 8, baseClock: 3.5m, boostClock: 4.7m, tdp: 60, memoryType: "DDR4, DDR5", hasIgpu: true);
        productImages.Add(new ProductImage { ProductId = i3_14100_Id, ImageUrl = "https://ark.intel.com/content/dam/ark/assets/images/box-shots/Intel%20Core%2014th%20Gen%20i3.png" });

        // Intel Core i5-12400F (Mid-range, no iGPU)
        var i5_12400f_Id = Guid.NewGuid();
        context.Products.Add(new Product
        {
            Id = i5_12400f_Id,
            Name = "Intel Core i5-12400F",
            Sku = "CPU-INTEL-I5-12400F",
            CategoryId = cpuCategory.Id,
            BrandId = intelBrand.Id,
            Price = 3590000,
            Stock = 60,
            Description = "Bộ vi xử lý Intel Core i5-12400F thế hệ 12, 6 nhân 12 luồng, xung nhịp cơ bản 2.5GHz, turbo lên đến 4.4GHz. CPU gaming phổ thông với hiệu năng đơn nhân xuất sắc.",
            WarrantyMonth = 36,
            Status = ProductStatus.Available,
        });
        AddCpuSpecs(specValues, cpuSpecs, i5_12400f_Id, socket: "LGA1700", cores: 6, threads: 12, baseClock: 2.5m, boostClock: 4.4m, tdp: 65, memoryType: "DDR4, DDR5", hasIgpu: false);
        productImages.Add(new ProductImage { ProductId = i5_12400f_Id, ImageUrl = "https://ark.intel.com/content/dam/ark/assets/images/box-shots/Intel%20Core%2012th%20Gen%20i5.png" });

        // Intel Core i5-14400F (Mid-range, no iGPU)
        var i5_14400f_Id = Guid.NewGuid();
        context.Products.Add(new Product
        {
            Id = i5_14400f_Id,
            Name = "Intel Core i5-14400F",
            Sku = "CPU-INTEL-I5-14400F",
            CategoryId = cpuCategory.Id,
            BrandId = intelBrand.Id,
            Price = 4990000,
            Stock = 45,
            Description = "Bộ vi xử lý Intel Core i5-14400F thế hệ 14, 10 nhân (6P+4E) 16 luồng, xung nhịp cơ bản 2.5GHz, turbo lên đến 4.7GHz. Hiệu năng đa nhiệm tốt với kiến trúc hybrid.",
            WarrantyMonth = 36,
            Status = ProductStatus.Available,
        });
        AddCpuSpecs(specValues, cpuSpecs, i5_14400f_Id, socket: "LGA1700", cores: 10, threads: 16, baseClock: 2.5m, boostClock: 4.7m, tdp: 65, memoryType: "DDR4, DDR5", hasIgpu: false);
        productImages.Add(new ProductImage { ProductId = i5_14400f_Id, ImageUrl = "https://ark.intel.com/content/dam/ark/assets/images/box-shots/Intel%20Core%2014th%20Gen%20i5.png" });

        // Intel Core i5-14600K (Mid-high, with iGPU, unlocked)
        var i5_14600k_Id = Guid.NewGuid();
        context.Products.Add(new Product
        {
            Id = i5_14600k_Id,
            Name = "Intel Core i5-14600K",
            Sku = "CPU-INTEL-I5-14600K",
            CategoryId = cpuCategory.Id,
            BrandId = intelBrand.Id,
            Price = 7490000,
            Stock = 35,
            Description = "Bộ vi xử lý Intel Core i5-14600K thế hệ 14 unlocked, 14 nhân (6P+8E) 20 luồng, xung nhịp cơ bản 3.5GHz, turbo lên đến 5.3GHz. CPU gaming tầm trung cao cấp, hỗ trợ ép xung.",
            WarrantyMonth = 36,
            Status = ProductStatus.Available,
        });
        AddCpuSpecs(specValues, cpuSpecs, i5_14600k_Id, socket: "LGA1700", cores: 14, threads: 20, baseClock: 3.5m, boostClock: 5.3m, tdp: 125, memoryType: "DDR4, DDR5", hasIgpu: true);
        productImages.Add(new ProductImage { ProductId = i5_14600k_Id, ImageUrl = "https://ark.intel.com/content/dam/ark/assets/images/box-shots/Intel%20Core%2014th%20Gen%20i5%20K.png" });

        // Intel Core i7-14700K (High-end, with iGPU, unlocked)
        var i7_14700k_Id = Guid.NewGuid();
        context.Products.Add(new Product
        {
            Id = i7_14700k_Id,
            Name = "Intel Core i7-14700K",
            Sku = "CPU-INTEL-I7-14700K",
            CategoryId = cpuCategory.Id,
            BrandId = intelBrand.Id,
            Price = 10490000,
            Stock = 30,
            Description = "Bộ vi xử lý Intel Core i7-14700K thế hệ 14 unlocked, 20 nhân (8P+12E) 28 luồng, xung nhịp cơ bản 3.4GHz, turbo lên đến 5.6GHz. CPU cao cấp cho gaming và sáng tạo nội dung.",
            WarrantyMonth = 36,
            Status = ProductStatus.Available,
        });
        AddCpuSpecs(specValues, cpuSpecs, i7_14700k_Id, socket: "LGA1700", cores: 20, threads: 28, baseClock: 3.4m, boostClock: 5.6m, tdp: 125, memoryType: "DDR4, DDR5", hasIgpu: true);
        productImages.Add(new ProductImage { ProductId = i7_14700k_Id, ImageUrl = "https://ark.intel.com/content/dam/ark/assets/images/box-shots/Intel%20Core%2014th%20Gen%20i7%20K.png" });

        // Intel Core i9-14900K (Flagship, with iGPU, unlocked)
        var i9_14900k_Id = Guid.NewGuid();
        context.Products.Add(new Product
        {
            Id = i9_14900k_Id,
            Name = "Intel Core i9-14900K",
            Sku = "CPU-INTEL-I9-14900K",
            CategoryId = cpuCategory.Id,
            BrandId = intelBrand.Id,
            Price = 14990000,
            Stock = 20,
            Description = "Bộ vi xử lý Intel Core i9-14900K thế hệ 14 flagship unlocked, 24 nhân (8P+16E) 32 luồng, xung nhịp cơ bản 3.2GHz, turbo lên đến 6.0GHz. CPU desktop mạnh nhất của Intel cho gaming và workstation.",
            WarrantyMonth = 36,
            Status = ProductStatus.Available,
        });
        AddCpuSpecs(specValues, cpuSpecs, i9_14900k_Id, socket: "LGA1700", cores: 24, threads: 32, baseClock: 3.2m, boostClock: 6.0m, tdp: 125, memoryType: "DDR4, DDR5", hasIgpu: true);
        productImages.Add(new ProductImage { ProductId = i9_14900k_Id, ImageUrl = "https://ark.intel.com/content/dam/ark/assets/images/box-shots/Intel%20Core%2014th%20Gen%20i9%20K.png" });

        // Intel Xeon W3-2423 (Entry Workstation)
        var xeon_w3_2423_Id = Guid.NewGuid();
        context.Products.Add(new Product
        {
            Id = xeon_w3_2423_Id,
            Name = "Intel Xeon W3-2423",
            Sku = "CPU-INTEL-XEON-W3-2423",
            CategoryId = cpuCategory.Id,
            BrandId = intelBrand.Id,
            Price = 8990000,
            Stock = 15,
            Description = "Bộ vi xử lý Intel Xeon W3-2423 cho workstation, 6 nhân 12 luồng, xung nhịp cơ bản 2.1GHz, turbo lên đến 4.2GHz. Hỗ trợ bộ nhớ ECC, phù hợp cho máy trạm chuyên nghiệp.",
            WarrantyMonth = 36,
            Status = ProductStatus.Available,
        });
        AddCpuSpecs(specValues, cpuSpecs, xeon_w3_2423_Id, socket: "LGA4677", cores: 6, threads: 12, baseClock: 2.1m, boostClock: 4.2m, tdp: 120, memoryType: "DDR5 ECC", hasIgpu: false);
        productImages.Add(new ProductImage { ProductId = xeon_w3_2423_Id, ImageUrl = "https://ark.intel.com/content/dam/ark/assets/images/box-shots/Intel%20Xeon%20W.png" });

        // Intel Xeon W5-2455X (Mid Workstation)
        var xeon_w5_2455x_Id = Guid.NewGuid();
        context.Products.Add(new Product
        {
            Id = xeon_w5_2455x_Id,
            Name = "Intel Xeon W5-2455X",
            Sku = "CPU-INTEL-XEON-W5-2455X",
            CategoryId = cpuCategory.Id,
            BrandId = intelBrand.Id,
            Price = 24990000,
            Stock = 10,
            Description = "Bộ vi xử lý Intel Xeon W5-2455X cho workstation cao cấp, 12 nhân 24 luồng, xung nhịp cơ bản 3.2GHz, turbo lên đến 4.6GHz. Hiệu năng mạnh mẽ cho render, mô phỏng và tính toán khoa học.",
            WarrantyMonth = 36,
            Status = ProductStatus.Available,
        });
        AddCpuSpecs(specValues, cpuSpecs, xeon_w5_2455x_Id, socket: "LGA4677", cores: 12, threads: 24, baseClock: 3.2m, boostClock: 4.6m, tdp: 200, memoryType: "DDR5 ECC", hasIgpu: false);
        productImages.Add(new ProductImage { ProductId = xeon_w5_2455x_Id, ImageUrl = "https://ark.intel.com/content/dam/ark/assets/images/box-shots/Intel%20Xeon%20W.png" });

        // Intel Xeon W9-3495X (High-end Workstation)
        var xeon_w9_3495x_Id = Guid.NewGuid();
        context.Products.Add(new Product
        {
            Id = xeon_w9_3495x_Id,
            Name = "Intel Xeon W9-3495X",
            Sku = "CPU-INTEL-XEON-W9-3495X",
            CategoryId = cpuCategory.Id,
            BrandId = intelBrand.Id,
            Price = 129990000,
            Stock = 5,
            Description = "Bộ vi xử lý Intel Xeon W9-3495X flagship cho workstation, 56 nhân 112 luồng, xung nhịp cơ bản 1.9GHz, turbo lên đến 4.8GHz. CPU workstation mạnh nhất của Intel, 105MB cache, hỗ trợ 4TB RAM.",
            WarrantyMonth = 36,
            Status = ProductStatus.Available,
        });
        AddCpuSpecs(specValues, cpuSpecs, xeon_w9_3495x_Id, socket: "LGA4677", cores: 56, threads: 112, baseClock: 1.9m, boostClock: 4.8m, tdp: 350, memoryType: "DDR5 ECC", hasIgpu: false);
        productImages.Add(new ProductImage { ProductId = xeon_w9_3495x_Id, ImageUrl = "https://ark.intel.com/content/dam/ark/assets/images/box-shots/Intel%20Xeon%20W.png" });

        // ============= AMD CPUs =============

        // AMD Ryzen 3 4100 (Low-end, no iGPU)
        var r3_4100_Id = Guid.NewGuid();
        context.Products.Add(new Product
        {
            Id = r3_4100_Id,
            Name = "AMD Ryzen 3 4100",
            Sku = "CPU-AMD-RYZEN3-4100",
            CategoryId = cpuCategory.Id,
            BrandId = amdBrand.Id,
            Price = 1890000,
            Stock = 45,
            Description = "Bộ vi xử lý AMD Ryzen 3 4100 kiến trúc Zen 2, 4 nhân 8 luồng, xung nhịp cơ bản 3.8GHz, turbo lên đến 4.0GHz. Lựa chọn tiết kiệm nhất cho PC gaming entry-level.",
            WarrantyMonth = 36,
            Status = ProductStatus.Available,
        });
        AddCpuSpecs(specValues, cpuSpecs, r3_4100_Id, socket: "AM4", cores: 4, threads: 8, baseClock: 3.8m, boostClock: 4.0m, tdp: 65, memoryType: "DDR4", hasIgpu: false);
        productImages.Add(new ProductImage { ProductId = r3_4100_Id, ImageUrl = "https://www.amd.com/content/dam/amd/en/images/products/processors/ryzen/2505503-ryzen-3-702x702.png" });

        // AMD Ryzen 5 5600G (Low-mid, with iGPU - APU)
        var r5_5600g_Id = Guid.NewGuid();
        context.Products.Add(new Product
        {
            Id = r5_5600g_Id,
            Name = "AMD Ryzen 5 5600G",
            Sku = "CPU-AMD-RYZEN5-5600G",
            CategoryId = cpuCategory.Id,
            BrandId = amdBrand.Id,
            Price = 3290000,
            Stock = 55,
            Description = "Bộ vi xử lý AMD Ryzen 5 5600G APU kiến trúc Zen 3, 6 nhân 12 luồng, xung nhịp cơ bản 3.9GHz, turbo lên đến 4.4GHz. Tích hợp Radeon Vega 7 Graphics, có thể chơi game nhẹ mà không cần card rời.",
            WarrantyMonth = 36,
            Status = ProductStatus.Available,
        });
        AddCpuSpecs(specValues, cpuSpecs, r5_5600g_Id, socket: "AM4", cores: 6, threads: 12, baseClock: 3.9m, boostClock: 4.4m, tdp: 65, memoryType: "DDR4", hasIgpu: true);
        productImages.Add(new ProductImage { ProductId = r5_5600g_Id, ImageUrl = "https://www.amd.com/content/dam/amd/en/images/products/processors/ryzen/2505503-ryzen-5-702x702.png" });

        // AMD Ryzen 5 5600X (Mid-range, no iGPU)
        var r5_5600x_Id = Guid.NewGuid();
        context.Products.Add(new Product
        {
            Id = r5_5600x_Id,
            Name = "AMD Ryzen 5 5600X",
            Sku = "CPU-AMD-RYZEN5-5600X",
            CategoryId = cpuCategory.Id,
            BrandId = amdBrand.Id,
            Price = 3790000,
            Stock = 50,
            Description = "Bộ vi xử lý AMD Ryzen 5 5600X kiến trúc Zen 3, 6 nhân 12 luồng, xung nhịp cơ bản 3.7GHz, turbo lên đến 4.6GHz. CPU gaming tầm trung huyền thoại với hiệu năng đơn nhân xuất sắc.",
            WarrantyMonth = 36,
            Status = ProductStatus.Available,
        });
        AddCpuSpecs(specValues, cpuSpecs, r5_5600x_Id, socket: "AM4", cores: 6, threads: 12, baseClock: 3.7m, boostClock: 4.6m, tdp: 65, memoryType: "DDR4", hasIgpu: false);
        productImages.Add(new ProductImage { ProductId = r5_5600x_Id, ImageUrl = "https://www.amd.com/content/dam/amd/en/images/products/processors/ryzen/2505503-ryzen-5-702x702.png" });

        // AMD Ryzen 5 7600X (Mid-range, no iGPU, AM5)
        var r5_7600x_Id = Guid.NewGuid();
        context.Products.Add(new Product
        {
            Id = r5_7600x_Id,
            Name = "AMD Ryzen 5 7600X",
            Sku = "CPU-AMD-RYZEN5-7600X",
            CategoryId = cpuCategory.Id,
            BrandId = amdBrand.Id,
            Price = 5990000,
            Stock = 40,
            Description = "Bộ vi xử lý AMD Ryzen 5 7600X kiến trúc Zen 4, 6 nhân 12 luồng, xung nhịp cơ bản 4.7GHz, turbo lên đến 5.3GHz. Nền tảng AM5 mới với DDR5 và PCIe 5.0.",
            WarrantyMonth = 36,
            Status = ProductStatus.Available,
        });
        AddCpuSpecs(specValues, cpuSpecs, r5_7600x_Id, socket: "AM5", cores: 6, threads: 12, baseClock: 4.7m, boostClock: 5.3m, tdp: 105, memoryType: "DDR5", hasIgpu: true);
        productImages.Add(new ProductImage { ProductId = r5_7600x_Id, ImageUrl = "https://www.amd.com/content/dam/amd/en/images/products/processors/ryzen/2505503-amd-ryzen-702x702.png" });

        // AMD Ryzen 5 7600 (Mid-range, lower TDP)
        var r5_7600_Id = Guid.NewGuid();
        context.Products.Add(new Product
        {
            Id = r5_7600_Id,
            Name = "AMD Ryzen 5 7600",
            Sku = "CPU-AMD-RYZEN5-7600",
            CategoryId = cpuCategory.Id,
            BrandId = amdBrand.Id,
            Price = 5290000,
            Stock = 45,
            Description = "Bộ vi xử lý AMD Ryzen 5 7600 kiến trúc Zen 4, 6 nhân 12 luồng, xung nhịp cơ bản 3.8GHz, turbo lên đến 5.1GHz. Phiên bản tiết kiệm điện hơn 7600X, đi kèm tản nhiệt Wraith Stealth.",
            WarrantyMonth = 36,
            Status = ProductStatus.Available,
        });
        AddCpuSpecs(specValues, cpuSpecs, r5_7600_Id, socket: "AM5", cores: 6, threads: 12, baseClock: 3.8m, boostClock: 5.1m, tdp: 65, memoryType: "DDR5", hasIgpu: true);
        productImages.Add(new ProductImage { ProductId = r5_7600_Id, ImageUrl = "https://www.amd.com/content/dam/amd/en/images/products/processors/ryzen/2505503-amd-ryzen-702x702.png" });

        // AMD Ryzen 7 7800X3D (High-end Gaming King)
        var r7_7800x3d_Id = Guid.NewGuid();
        context.Products.Add(new Product
        {
            Id = r7_7800x3d_Id,
            Name = "AMD Ryzen 7 7800X3D",
            Sku = "CPU-AMD-RYZEN7-7800X3D",
            CategoryId = cpuCategory.Id,
            BrandId = amdBrand.Id,
            Price = 10990000,
            Stock = 25,
            Description = "Bộ vi xử lý AMD Ryzen 7 7800X3D với công nghệ 3D V-Cache, 8 nhân 16 luồng, 104MB tổng cache. CPU gaming mạnh nhất thế giới, vượt trội trong mọi tựa game.",
            WarrantyMonth = 36,
            Status = ProductStatus.Available,
        });
        AddCpuSpecs(specValues, cpuSpecs, r7_7800x3d_Id, socket: "AM5", cores: 8, threads: 16, baseClock: 4.2m, boostClock: 5.0m, tdp: 120, memoryType: "DDR5", hasIgpu: true);
        productImages.Add(new ProductImage { ProductId = r7_7800x3d_Id, ImageUrl = "https://www.amd.com/content/dam/amd/en/images/products/processors/ryzen/2505503-ryzen-7-702x702.png" });

        // AMD Ryzen 9 7900X (High-end)
        var r9_7900x_Id = Guid.NewGuid();
        context.Products.Add(new Product
        {
            Id = r9_7900x_Id,
            Name = "AMD Ryzen 9 7900X",
            Sku = "CPU-AMD-RYZEN9-7900X",
            CategoryId = cpuCategory.Id,
            BrandId = amdBrand.Id,
            Price = 11990000,
            Stock = 20,
            Description = "Bộ vi xử lý AMD Ryzen 9 7900X kiến trúc Zen 4, 12 nhân 24 luồng, xung nhịp cơ bản 4.7GHz, turbo lên đến 5.6GHz. Hiệu năng cao cho gaming và sáng tạo nội dung.",
            WarrantyMonth = 36,
            Status = ProductStatus.Available,
        });
        AddCpuSpecs(specValues, cpuSpecs, r9_7900x_Id, socket: "AM5", cores: 12, threads: 24, baseClock: 4.7m, boostClock: 5.6m, tdp: 170, memoryType: "DDR5", hasIgpu: true);
        productImages.Add(new ProductImage { ProductId = r9_7900x_Id, ImageUrl = "https://www.amd.com/content/dam/amd/en/images/products/processors/ryzen/2505503-ryzen-9-702x702.png" });

        // AMD Ryzen 9 7950X (Flagship)
        var r9_7950x_Id = Guid.NewGuid();
        context.Products.Add(new Product
        {
            Id = r9_7950x_Id,
            Name = "AMD Ryzen 9 7950X",
            Sku = "CPU-AMD-RYZEN9-7950X",
            CategoryId = cpuCategory.Id,
            BrandId = amdBrand.Id,
            Price = 14990000,
            Stock = 15,
            Description = "Bộ vi xử lý AMD Ryzen 9 7950X flagship kiến trúc Zen 4, 16 nhân 32 luồng, xung nhịp cơ bản 4.5GHz, turbo lên đến 5.7GHz. CPU AM5 mạnh nhất cho enthusiast và content creator.",
            WarrantyMonth = 36,
            Status = ProductStatus.Available,
        });
        AddCpuSpecs(specValues, cpuSpecs, r9_7950x_Id, socket: "AM5", cores: 16, threads: 32, baseClock: 4.5m, boostClock: 5.7m, tdp: 170, memoryType: "DDR5", hasIgpu: true);
        productImages.Add(new ProductImage { ProductId = r9_7950x_Id, ImageUrl = "https://www.amd.com/content/dam/amd/en/images/products/processors/ryzen/2505503-ryzen-9-702x702.png" });

        // AMD Ryzen Threadripper 7960X (HEDT Workstation)
        var tr_7960x_Id = Guid.NewGuid();
        context.Products.Add(new Product
        {
            Id = tr_7960x_Id,
            Name = "AMD Ryzen Threadripper 7960X",
            Sku = "CPU-AMD-THREADRIPPER-7960X",
            CategoryId = cpuCategory.Id,
            BrandId = amdBrand.Id,
            Price = 34990000,
            Stock = 10,
            Description = "Bộ vi xử lý AMD Ryzen Threadripper 7960X cho HEDT, 24 nhân 48 luồng, xung nhịp cơ bản 4.2GHz, turbo lên đến 5.3GHz. Nền tảng sTR5 với 152MB cache, hỗ trợ quad-channel DDR5.",
            WarrantyMonth = 36,
            Status = ProductStatus.Available,
        });
        AddCpuSpecs(specValues, cpuSpecs, tr_7960x_Id, socket: "sTR5", cores: 24, threads: 48, baseClock: 4.2m, boostClock: 5.3m, tdp: 350, memoryType: "DDR5 ECC", hasIgpu: false);
        productImages.Add(new ProductImage { ProductId = tr_7960x_Id, ImageUrl = "https://www.amd.com/content/dam/amd/en/images/products/processors/ryzen/2505503-threadripper-702x702.png" });

        // AMD Ryzen Threadripper PRO 7995WX (Ultimate Workstation)
        var tr_pro_7995wx_Id = Guid.NewGuid();
        context.Products.Add(new Product
        {
            Id = tr_pro_7995wx_Id,
            Name = "AMD Ryzen Threadripper PRO 7995WX",
            Sku = "CPU-AMD-THREADRIPPER-PRO-7995WX",
            CategoryId = cpuCategory.Id,
            BrandId = amdBrand.Id,
            Price = 249990000,
            Stock = 3,
            Description = "Bộ vi xử lý AMD Ryzen Threadripper PRO 7995WX ultimate workstation, 96 nhân 192 luồng, xung nhịp cơ bản 2.5GHz, turbo lên đến 5.1GHz. CPU desktop mạnh nhất thế giới với 384MB cache, hỗ trợ 2TB RAM 8-channel.",
            WarrantyMonth = 36,
            Status = ProductStatus.Available,
        });
        AddCpuSpecs(specValues, cpuSpecs, tr_pro_7995wx_Id, socket: "sWRX9", cores: 96, threads: 192, baseClock: 2.5m, boostClock: 5.1m, tdp: 350, memoryType: "DDR5 ECC", hasIgpu: false);
        productImages.Add(new ProductImage { ProductId = tr_pro_7995wx_Id, ImageUrl = "https://www.amd.com/content/dam/amd/en/images/products/processors/ryzen/2505503-threadripper-pro-702x702.png" });
    }

    private static void AddGpuSpecs(List<ProductSpecValue> specValues, List<SpecDefinition> specs, Guid productId, int vram, int tdp, int length, string pcieSlot, string powerConnector)
    {
        var vramSpec = specs.FirstOrDefault(s => s.Code == "gpu_vram");
        if (vramSpec != null) specValues.Add(new ProductSpecValue { ProductId = productId, SpecDefinitionId = vramSpec.Id, NumberValue = vram });

        var tdpSpec = specs.FirstOrDefault(s => s.Code == "gpu_tdp");
        if (tdpSpec != null) specValues.Add(new ProductSpecValue { ProductId = productId, SpecDefinitionId = tdpSpec.Id, NumberValue = tdp });

        var lengthSpec = specs.FirstOrDefault(s => s.Code == "gpu_length");
        if (lengthSpec != null) specValues.Add(new ProductSpecValue { ProductId = productId, SpecDefinitionId = lengthSpec.Id, NumberValue = length });

        var pcieSpec = specs.FirstOrDefault(s => s.Code == "gpu_pcie_slot");
        if (pcieSpec != null) specValues.Add(new ProductSpecValue { ProductId = productId, SpecDefinitionId = pcieSpec.Id, TextValue = pcieSlot });

        var powerSpec = specs.FirstOrDefault(s => s.Code == "gpu_power_connector");
        if (powerSpec != null) specValues.Add(new ProductSpecValue { ProductId = productId, SpecDefinitionId = powerSpec.Id, TextValue = powerConnector });
    }

    private static void AddCpuSpecs(List<ProductSpecValue> specValues, List<SpecDefinition> specs, Guid productId, string socket, int cores, int threads, decimal baseClock, decimal boostClock, int tdp, string memoryType, bool hasIgpu)
    {
        var socketSpec = specs.FirstOrDefault(s => s.Code == "cpu_socket");
        if (socketSpec != null) specValues.Add(new ProductSpecValue { ProductId = productId, SpecDefinitionId = socketSpec.Id, TextValue = socket });

        var coresSpec = specs.FirstOrDefault(s => s.Code == "cpu_cores");
        if (coresSpec != null) specValues.Add(new ProductSpecValue { ProductId = productId, SpecDefinitionId = coresSpec.Id, NumberValue = cores });

        var threadsSpec = specs.FirstOrDefault(s => s.Code == "cpu_threads");
        if (threadsSpec != null) specValues.Add(new ProductSpecValue { ProductId = productId, SpecDefinitionId = threadsSpec.Id, NumberValue = threads });

        var baseClockSpec = specs.FirstOrDefault(s => s.Code == "cpu_base_clock");
        if (baseClockSpec != null) specValues.Add(new ProductSpecValue { ProductId = productId, SpecDefinitionId = baseClockSpec.Id, DecimalValue = baseClock });

        var boostClockSpec = specs.FirstOrDefault(s => s.Code == "cpu_boost_clock");
        if (boostClockSpec != null) specValues.Add(new ProductSpecValue { ProductId = productId, SpecDefinitionId = boostClockSpec.Id, DecimalValue = boostClock });

        var tdpSpec = specs.FirstOrDefault(s => s.Code == "cpu_tdp");
        if (tdpSpec != null) specValues.Add(new ProductSpecValue { ProductId = productId, SpecDefinitionId = tdpSpec.Id, NumberValue = tdp });

        var memoryTypeSpec = specs.FirstOrDefault(s => s.Code == "cpu_memory_type");
        if (memoryTypeSpec != null) specValues.Add(new ProductSpecValue { ProductId = productId, SpecDefinitionId = memoryTypeSpec.Id, TextValue = memoryType });

        var igpuSpec = specs.FirstOrDefault(s => s.Code == "cpu_integrated_gpu");
        if (igpuSpec != null) specValues.Add(new ProductSpecValue { ProductId = productId, SpecDefinitionId = igpuSpec.Id, BoolValue = hasIgpu });
    }

    private static async Task InitMotherboardProducts(ApplicationDbContext context, List<ProductSpecValue> specValues, List<ProductImage> productImages)
    {
        var mbCategory = await context.Categories.FirstOrDefaultAsync(c => c.Name == "Bo mạch chủ")
            ?? throw new NotFoundException("Không tìm thấy danh mục Bo mạch chủ");

        var mbSpecs = await context.SpecDefinitions
            .Where(s => s.CategoryId == mbCategory.Id)
            .ToListAsync();

        // Load motherboard brands
        var asusBrand = await context.Brands.FirstOrDefaultAsync(b => b.Name == "ASUS")
            ?? throw new NotFoundException("Không tìm thấy thương hiệu ASUS");
        var msiBrand = await context.Brands.FirstOrDefaultAsync(b => b.Name == "MSI")
            ?? throw new NotFoundException("Không tìm thấy thương hiệu MSI");
        var gigabyteBrand = await context.Brands.FirstOrDefaultAsync(b => b.Name == "Gigabyte")
            ?? throw new NotFoundException("Không tìm thấy thương hiệu Gigabyte");
        var asrockBrand = await context.Brands.FirstOrDefaultAsync(b => b.Name == "ASRock")
            ?? throw new NotFoundException("Không tìm thấy thương hiệu ASRock");
        var biostarBrand = await context.Brands.FirstOrDefaultAsync(b => b.Name == "Biostar")
            ?? throw new NotFoundException("Không tìm thấy thương hiệu Biostar");

        // ============= INTEL LGA1700 Z790 (HIGH-END) =============

        // ASUS ROG MAXIMUS Z790 HERO
        var asusZ790HeroId = Guid.NewGuid();
        context.Products.Add(new Product
        {
            Id = asusZ790HeroId,
            Name = "ASUS ROG MAXIMUS Z790 HERO",
            Sku = "MB-ASUS-Z790-MAXIMUS-HERO",
            CategoryId = mbCategory.Id,
            BrandId = asusBrand.Id,
            Price = 16990000,
            Stock = 10,
            Description = "Bo mạch chủ ASUS ROG MAXIMUS Z790 HERO cao cấp cho Intel thế hệ 12/13/14. Hỗ trợ DDR5, PCIe 5.0 cho cả GPU và SSD, WiFi 6E, 2.5G LAN. VRM 20+1 phase, Aura Sync RGB. Lựa chọn hàng đầu cho enthusiast.",
            WarrantyMonth = 36,
            Status = ProductStatus.Available,
        });
        AddMotherboardSpecs(specValues, mbSpecs, asusZ790HeroId, socket: "LGA1700", chipset: "Z790", formFactor: "ATX", memoryType: "DDR5", memorySlots: 4, maxMemory: 192, m2Slots: 5, pcieVersion: "PCIe 5.0");
        productImages.Add(new ProductImage { ProductId = asusZ790HeroId, ImageUrl = "https://dlcdnwebimgs.asus.com/gain/z790-maximus-hero/w800" });

        // MSI MEG Z790 ACE
        var msiZ790AceId = Guid.NewGuid();
        context.Products.Add(new Product
        {
            Id = msiZ790AceId,
            Name = "MSI MEG Z790 ACE",
            Sku = "MB-MSI-Z790-MEG-ACE",
            CategoryId = mbCategory.Id,
            BrandId = msiBrand.Id,
            Price = 15990000,
            Stock = 12,
            Description = "Bo mạch chủ MSI MEG Z790 ACE flagship cho Intel LGA1700. Thiết kế VRM 24+1+2 phase, hỗ trợ DDR5-7800+, PCIe 5.0 x16, 5x M.2 slots. WiFi 6E, 10G LAN + 2.5G LAN. Bo mạch chủ cao cấp cho ép xung.",
            WarrantyMonth = 36,
            Status = ProductStatus.Available,
        });
        AddMotherboardSpecs(specValues, mbSpecs, msiZ790AceId, socket: "LGA1700", chipset: "Z790", formFactor: "ATX", memoryType: "DDR5", memorySlots: 4, maxMemory: 192, m2Slots: 5, pcieVersion: "PCIe 5.0");
        productImages.Add(new ProductImage { ProductId = msiZ790AceId, ImageUrl = "https://asset.msi.com/resize/image/global/product/product_z790ace.png" });

        // Gigabyte Z790 AORUS MASTER
        var gigabyteZ790MasterId = Guid.NewGuid();
        context.Products.Add(new Product
        {
            Id = gigabyteZ790MasterId,
            Name = "Gigabyte Z790 AORUS MASTER",
            Sku = "MB-GIGABYTE-Z790-AORUS-MASTER",
            CategoryId = mbCategory.Id,
            BrandId = gigabyteBrand.Id,
            Price = 14990000,
            Stock = 15,
            Description = "Bo mạch chủ Gigabyte Z790 AORUS MASTER cao cấp. VRM 20+1+2 phase, hỗ trợ DDR5-8000+, PCIe 5.0. Fins-Array III heatsink, WiFi 6E, 10G LAN. RGB Fusion 2.0. Thiết kế sang trọng cho enthusiast build.",
            WarrantyMonth = 36,
            Status = ProductStatus.Available,
        });
        AddMotherboardSpecs(specValues, mbSpecs, gigabyteZ790MasterId, socket: "LGA1700", chipset: "Z790", formFactor: "ATX", memoryType: "DDR5", memorySlots: 4, maxMemory: 192, m2Slots: 5, pcieVersion: "PCIe 5.0");
        productImages.Add(new ProductImage { ProductId = gigabyteZ790MasterId, ImageUrl = "https://www.gigabyte.com/FileUpload/Global/KeyFeature/z790-aorus-master.png" });

        // ============= INTEL LGA1700 Z790 (MID-HIGH) =============

        // ASRock Z790 Steel Legend WiFi
        var asrockZ790SteelId = Guid.NewGuid();
        context.Products.Add(new Product
        {
            Id = asrockZ790SteelId,
            Name = "ASRock Z790 Steel Legend WiFi",
            Sku = "MB-ASROCK-Z790-STEEL-LEGEND-WIFI",
            CategoryId = mbCategory.Id,
            BrandId = asrockBrand.Id,
            Price = 8990000,
            Stock = 20,
            Description = "Bo mạch chủ ASRock Z790 Steel Legend WiFi với thiết kế camo độc đáo. VRM 16+1+1 phase, hỗ trợ DDR5-6800+, PCIe 5.0. WiFi 6E, 2.5G LAN. Polychrome Sync RGB. Giá tốt cho Z790.",
            WarrantyMonth = 36,
            Status = ProductStatus.Available,
        });
        AddMotherboardSpecs(specValues, mbSpecs, asrockZ790SteelId, socket: "LGA1700", chipset: "Z790", formFactor: "ATX", memoryType: "DDR5", memorySlots: 4, maxMemory: 128, m2Slots: 4, pcieVersion: "PCIe 5.0");
        productImages.Add(new ProductImage { ProductId = asrockZ790SteelId, ImageUrl = "https://www.asrock.com/mb/photo/Z790%20Steel%20Legend%20WiFi.png" });

        // ============= INTEL LGA1700 B760 (MID-RANGE) =============

        // ASUS ROG STRIX B760-F GAMING WIFI
        var asusB760StrixId = Guid.NewGuid();
        context.Products.Add(new Product
        {
            Id = asusB760StrixId,
            Name = "ASUS ROG STRIX B760-F GAMING WIFI",
            Sku = "MB-ASUS-B760-STRIX-F-GAMING-WIFI",
            CategoryId = mbCategory.Id,
            BrandId = asusBrand.Id,
            Price = 6990000,
            Stock = 25,
            Description = "Bo mạch chủ ASUS ROG STRIX B760-F GAMING WIFI cho Intel thế hệ 12/13/14. VRM 12+1 phase, hỗ trợ DDR5-7800+, PCIe 5.0 cho SSD. WiFi 6E, 2.5G LAN. Aura Sync RGB. Gaming motherboard tầm trung cao cấp.",
            WarrantyMonth = 36,
            Status = ProductStatus.Available,
        });
        AddMotherboardSpecs(specValues, mbSpecs, asusB760StrixId, socket: "LGA1700", chipset: "B760", formFactor: "ATX", memoryType: "DDR5", memorySlots: 4, maxMemory: 192, m2Slots: 4, pcieVersion: "PCIe 5.0");
        productImages.Add(new ProductImage { ProductId = asusB760StrixId, ImageUrl = "https://dlcdnwebimgs.asus.com/gain/b760-strix-f-gaming-wifi/w800" });

        // MSI MAG B760M MORTAR WIFI
        var msiB760MortarId = Guid.NewGuid();
        context.Products.Add(new Product
        {
            Id = msiB760MortarId,
            Name = "MSI MAG B760M MORTAR WIFI",
            Sku = "MB-MSI-B760M-MORTAR-WIFI",
            CategoryId = mbCategory.Id,
            BrandId = msiBrand.Id,
            Price = 4990000,
            Stock = 30,
            Description = "Bo mạch chủ MSI MAG B760M MORTAR WIFI dạng Micro-ATX. VRM 12+1+1 phase, hỗ trợ DDR5-7000+. WiFi 6E, 2.5G LAN. Thiết kế nhỏ gọn nhưng đầy đủ tính năng, phù hợp cho build compact.",
            WarrantyMonth = 36,
            Status = ProductStatus.Available,
        });
        AddMotherboardSpecs(specValues, mbSpecs, msiB760MortarId, socket: "LGA1700", chipset: "B760", formFactor: "mATX", memoryType: "DDR5", memorySlots: 4, maxMemory: 128, m2Slots: 2, pcieVersion: "PCIe 4.0");
        productImages.Add(new ProductImage { ProductId = msiB760MortarId, ImageUrl = "https://asset.msi.com/resize/image/global/product/product_b760m-mortar-wifi.png" });

        // ============= INTEL LGA1700 B760 (BUDGET) =============

        // Gigabyte B760M DS3H DDR4
        var gigabyteB760MDs3hId = Guid.NewGuid();
        context.Products.Add(new Product
        {
            Id = gigabyteB760MDs3hId,
            Name = "Gigabyte B760M DS3H DDR4",
            Sku = "MB-GIGABYTE-B760M-DS3H-DDR4",
            CategoryId = mbCategory.Id,
            BrandId = gigabyteBrand.Id,
            Price = 2690000,
            Stock = 45,
            Description = "Bo mạch chủ Gigabyte B760M DS3H DDR4 giá rẻ cho Intel thế hệ 12/13/14. Hỗ trợ DDR4-5333, VRM 6+2+1 phase. 2x M.2 slots, PCIe 4.0. Lựa chọn tiết kiệm cho build văn phòng và gaming nhẹ.",
            WarrantyMonth = 36,
            Status = ProductStatus.Available,
        });
        AddMotherboardSpecs(specValues, mbSpecs, gigabyteB760MDs3hId, socket: "LGA1700", chipset: "B760", formFactor: "mATX", memoryType: "DDR4", memorySlots: 2, maxMemory: 64, m2Slots: 2, pcieVersion: "PCIe 4.0");
        productImages.Add(new ProductImage { ProductId = gigabyteB760MDs3hId, ImageUrl = "https://www.gigabyte.com/FileUpload/Global/KeyFeature/b760m-ds3h-ddr4.png" });

        // Biostar B760MX-E PRO
        var biostarB760MxId = Guid.NewGuid();
        context.Products.Add(new Product
        {
            Id = biostarB760MxId,
            Name = "Biostar B760MX-E PRO",
            Sku = "MB-BIOSTAR-B760MX-E-PRO",
            CategoryId = mbCategory.Id,
            BrandId = biostarBrand.Id,
            Price = 2290000,
            Stock = 35,
            Description = "Bo mạch chủ Biostar B760MX-E PRO giá cực rẻ cho Intel LGA1700. Hỗ trợ DDR4, PCIe 4.0, 1x M.2 slot. VRM cơ bản. Lựa chọn tiết kiệm nhất cho build văn phòng.",
            WarrantyMonth = 24,
            Status = ProductStatus.Available,
        });
        AddMotherboardSpecs(specValues, mbSpecs, biostarB760MxId, socket: "LGA1700", chipset: "B760", formFactor: "mATX", memoryType: "DDR4", memorySlots: 2, maxMemory: 64, m2Slots: 1, pcieVersion: "PCIe 4.0");
        productImages.Add(new ProductImage { ProductId = biostarB760MxId, ImageUrl = "https://www.biostar.com.tw/upload/Mainboard/b760mx-e-pro.png" });

        // ============= AMD AM5 X670E (HIGH-END) =============

        // ASUS ROG CROSSHAIR X670E HERO
        var asusX670EHeroId = Guid.NewGuid();
        context.Products.Add(new Product
        {
            Id = asusX670EHeroId,
            Name = "ASUS ROG CROSSHAIR X670E HERO",
            Sku = "MB-ASUS-X670E-CROSSHAIR-HERO",
            CategoryId = mbCategory.Id,
            BrandId = asusBrand.Id,
            Price = 17990000,
            Stock = 8,
            Description = "Bo mạch chủ ASUS ROG CROSSHAIR X670E HERO flagship cho AMD Ryzen 7000. VRM 18+2 phase, hỗ trợ DDR5-6400+, PCIe 5.0 cho cả GPU và SSD. WiFi 6E, 2.5G LAN. Aura Sync RGB. Bo mạch chủ AM5 cao cấp nhất.",
            WarrantyMonth = 36,
            Status = ProductStatus.Available,
        });
        AddMotherboardSpecs(specValues, mbSpecs, asusX670EHeroId, socket: "AM5", chipset: "X670E", formFactor: "ATX", memoryType: "DDR5", memorySlots: 4, maxMemory: 128, m2Slots: 5, pcieVersion: "PCIe 5.0");
        productImages.Add(new ProductImage { ProductId = asusX670EHeroId, ImageUrl = "https://dlcdnwebimgs.asus.com/gain/x670e-crosshair-hero/w800" });

        // MSI MEG X670E ACE
        var msiX670EAceId = Guid.NewGuid();
        context.Products.Add(new Product
        {
            Id = msiX670EAceId,
            Name = "MSI MEG X670E ACE",
            Sku = "MB-MSI-X670E-MEG-ACE",
            CategoryId = mbCategory.Id,
            BrandId = msiBrand.Id,
            Price = 16990000,
            Stock = 10,
            Description = "Bo mạch chủ MSI MEG X670E ACE cao cấp cho AMD AM5. VRM 22+2+1 phase, hỗ trợ DDR5-6600+, PCIe 5.0. 4x M.2 slots, WiFi 6E, 10G LAN + 2.5G LAN. Thiết kế sang trọng với Mystic Light RGB.",
            WarrantyMonth = 36,
            Status = ProductStatus.Available,
        });
        AddMotherboardSpecs(specValues, mbSpecs, msiX670EAceId, socket: "AM5", chipset: "X670E", formFactor: "ATX", memoryType: "DDR5", memorySlots: 4, maxMemory: 128, m2Slots: 4, pcieVersion: "PCIe 5.0");
        productImages.Add(new ProductImage { ProductId = msiX670EAceId, ImageUrl = "https://asset.msi.com/resize/image/global/product/product_x670e-ace.png" });

        // Gigabyte X670E AORUS MASTER
        var gigabyteX670EMasterId = Guid.NewGuid();
        context.Products.Add(new Product
        {
            Id = gigabyteX670EMasterId,
            Name = "Gigabyte X670E AORUS MASTER",
            Sku = "MB-GIGABYTE-X670E-AORUS-MASTER",
            CategoryId = mbCategory.Id,
            BrandId = gigabyteBrand.Id,
            Price = 15490000,
            Stock = 12,
            Description = "Bo mạch chủ Gigabyte X670E AORUS MASTER cho AMD Ryzen 7000. VRM 16+2+2 phase, hỗ trợ DDR5-6600+, PCIe 5.0. Fins-Array III heatsink, WiFi 6E, 10G LAN. RGB Fusion 2.0. Bo mạch chủ cao cấp cho enthusiast AMD.",
            WarrantyMonth = 36,
            Status = ProductStatus.Available,
        });
        AddMotherboardSpecs(specValues, mbSpecs, gigabyteX670EMasterId, socket: "AM5", chipset: "X670E", formFactor: "ATX", memoryType: "DDR5", memorySlots: 4, maxMemory: 128, m2Slots: 4, pcieVersion: "PCIe 5.0");
        productImages.Add(new ProductImage { ProductId = gigabyteX670EMasterId, ImageUrl = "https://www.gigabyte.com/FileUpload/Global/KeyFeature/x670e-aorus-master.png" });

        // ============= AMD AM5 B650 (MID-RANGE) =============

        // ASUS TUF GAMING B650-PLUS WIFI
        var asusB650TufId = Guid.NewGuid();
        context.Products.Add(new Product
        {
            Id = asusB650TufId,
            Name = "ASUS TUF GAMING B650-PLUS WIFI",
            Sku = "MB-ASUS-B650-TUF-GAMING-PLUS-WIFI",
            CategoryId = mbCategory.Id,
            BrandId = asusBrand.Id,
            Price = 5990000,
            Stock = 28,
            Description = "Bo mạch chủ ASUS TUF GAMING B650-PLUS WIFI cho AMD AM5. VRM 12+2 phase, hỗ trợ DDR5-6400+, PCIe 4.0. WiFi 6, 2.5G LAN. Thiết kế bền bỉ chuẩn quân sự TUF. Giá tốt cho nền tảng AM5.",
            WarrantyMonth = 36,
            Status = ProductStatus.Available,
        });
        AddMotherboardSpecs(specValues, mbSpecs, asusB650TufId, socket: "AM5", chipset: "B650", formFactor: "ATX", memoryType: "DDR5", memorySlots: 4, maxMemory: 128, m2Slots: 3, pcieVersion: "PCIe 4.0");
        productImages.Add(new ProductImage { ProductId = asusB650TufId, ImageUrl = "https://dlcdnwebimgs.asus.com/gain/b650-tuf-gaming-plus-wifi/w800" });

        // MSI MAG B650 TOMAHAWK WIFI
        var msiB650TomahawkId = Guid.NewGuid();
        context.Products.Add(new Product
        {
            Id = msiB650TomahawkId,
            Name = "MSI MAG B650 TOMAHAWK WIFI",
            Sku = "MB-MSI-B650-MAG-TOMAHAWK-WIFI",
            CategoryId = mbCategory.Id,
            BrandId = msiBrand.Id,
            Price = 5490000,
            Stock = 32,
            Description = "Bo mạch chủ MSI MAG B650 TOMAHAWK WIFI cho AMD AM5. VRM 12+2+1 phase, hỗ trợ DDR5-6400+, PCIe 4.0. WiFi 6E, 2.5G LAN. Extended heatsink design. Bo mạch chủ gaming tầm trung đáng mua nhất cho AM5.",
            WarrantyMonth = 36,
            Status = ProductStatus.Available,
        });
        AddMotherboardSpecs(specValues, mbSpecs, msiB650TomahawkId, socket: "AM5", chipset: "B650", formFactor: "ATX", memoryType: "DDR5", memorySlots: 4, maxMemory: 128, m2Slots: 2, pcieVersion: "PCIe 4.0");
        productImages.Add(new ProductImage { ProductId = msiB650TomahawkId, ImageUrl = "https://asset.msi.com/resize/image/global/product/product_b650-tomahawk-wifi.png" });

        // ASRock B650M PG Riptide WiFi
        var asrockB650MRiptideId = Guid.NewGuid();
        context.Products.Add(new Product
        {
            Id = asrockB650MRiptideId,
            Name = "ASRock B650M PG Riptide WiFi",
            Sku = "MB-ASROCK-B650M-PG-RIPTIDE-WIFI",
            CategoryId = mbCategory.Id,
            BrandId = asrockBrand.Id,
            Price = 4290000,
            Stock = 35,
            Description = "Bo mạch chủ ASRock B650M PG Riptide WiFi dạng Micro-ATX cho AMD AM5. VRM 8+2+1 phase, hỗ trợ DDR5-6200+, PCIe 4.0. WiFi 6E, 2.5G LAN. Giá cạnh tranh cho nền tảng AM5.",
            WarrantyMonth = 36,
            Status = ProductStatus.Available,
        });
        AddMotherboardSpecs(specValues, mbSpecs, asrockB650MRiptideId, socket: "AM5", chipset: "B650", formFactor: "mATX", memoryType: "DDR5", memorySlots: 4, maxMemory: 128, m2Slots: 2, pcieVersion: "PCIe 4.0");
        productImages.Add(new ProductImage { ProductId = asrockB650MRiptideId, ImageUrl = "https://www.asrock.com/mb/photo/B650M%20PG%20Riptide%20WiFi.png" });

        // ============= AMD AM5 B650 (BUDGET) =============

        // Gigabyte B650M DS3H
        var gigabyteB650MDs3hId = Guid.NewGuid();
        context.Products.Add(new Product
        {
            Id = gigabyteB650MDs3hId,
            Name = "Gigabyte B650M DS3H",
            Sku = "MB-GIGABYTE-B650M-DS3H",
            CategoryId = mbCategory.Id,
            BrandId = gigabyteBrand.Id,
            Price = 3290000,
            Stock = 40,
            Description = "Bo mạch chủ Gigabyte B650M DS3H giá rẻ nhất cho AMD AM5. Hỗ trợ DDR5-6000+, VRM 6+2+1 phase. 2x M.2 slots, PCIe 4.0. Lựa chọn tiết kiệm nhất để trải nghiệm Ryzen 7000.",
            WarrantyMonth = 36,
            Status = ProductStatus.Available,
        });
        AddMotherboardSpecs(specValues, mbSpecs, gigabyteB650MDs3hId, socket: "AM5", chipset: "B650", formFactor: "mATX", memoryType: "DDR5", memorySlots: 2, maxMemory: 96, m2Slots: 2, pcieVersion: "PCIe 4.0");
        productImages.Add(new ProductImage { ProductId = gigabyteB650MDs3hId, ImageUrl = "https://www.gigabyte.com/FileUpload/Global/KeyFeature/b650m-ds3h.png" });
    }

    private static void AddMotherboardSpecs(List<ProductSpecValue> specValues, List<SpecDefinition> specs, Guid productId, string socket, string chipset, string formFactor, string memoryType, int memorySlots, int maxMemory, int m2Slots, string pcieVersion)
    {
        var socketSpec = specs.FirstOrDefault(s => s.Code == "mb_socket");
        if (socketSpec != null) specValues.Add(new ProductSpecValue { ProductId = productId, SpecDefinitionId = socketSpec.Id, TextValue = socket });

        var chipsetSpec = specs.FirstOrDefault(s => s.Code == "mb_chipset");
        if (chipsetSpec != null) specValues.Add(new ProductSpecValue { ProductId = productId, SpecDefinitionId = chipsetSpec.Id, TextValue = chipset });

        var formFactorSpec = specs.FirstOrDefault(s => s.Code == "mb_form_factor");
        if (formFactorSpec != null) specValues.Add(new ProductSpecValue { ProductId = productId, SpecDefinitionId = formFactorSpec.Id, TextValue = formFactor });

        var memoryTypeSpec = specs.FirstOrDefault(s => s.Code == "mb_memory_type");
        if (memoryTypeSpec != null) specValues.Add(new ProductSpecValue { ProductId = productId, SpecDefinitionId = memoryTypeSpec.Id, TextValue = memoryType });

        var memorySlotsSpec = specs.FirstOrDefault(s => s.Code == "mb_memory_slots");
        if (memorySlotsSpec != null) specValues.Add(new ProductSpecValue { ProductId = productId, SpecDefinitionId = memorySlotsSpec.Id, NumberValue = memorySlots });

        var maxMemorySpec = specs.FirstOrDefault(s => s.Code == "mb_max_memory");
        if (maxMemorySpec != null) specValues.Add(new ProductSpecValue { ProductId = productId, SpecDefinitionId = maxMemorySpec.Id, NumberValue = maxMemory });

        var m2SlotsSpec = specs.FirstOrDefault(s => s.Code == "mb_m2_slots");
        if (m2SlotsSpec != null) specValues.Add(new ProductSpecValue { ProductId = productId, SpecDefinitionId = m2SlotsSpec.Id, NumberValue = m2Slots });

        var pcieVersionSpec = specs.FirstOrDefault(s => s.Code == "mb_pcie_version");
        if (pcieVersionSpec != null) specValues.Add(new ProductSpecValue { ProductId = productId, SpecDefinitionId = pcieVersionSpec.Id, TextValue = pcieVersion });
    }

    private static async Task InitRamProducts(ApplicationDbContext context, List<ProductSpecValue> specValues, List<ProductImage> productImages)
    {
        var ramCategory = await context.Categories.FirstOrDefaultAsync(c => c.Name == "RAM")
            ?? throw new NotFoundException("Không tìm thấy danh mục RAM");

        var ramSpecs = await context.SpecDefinitions
            .Where(s => s.CategoryId == ramCategory.Id)
            .ToListAsync();

        // Load RAM brands
        var corsairBrand = await context.Brands.FirstOrDefaultAsync(b => b.Name == "Corsair")
            ?? throw new NotFoundException("Không tìm thấy thương hiệu Corsair");
        var gskillBrand = await context.Brands.FirstOrDefaultAsync(b => b.Name == "G.Skill")
            ?? throw new NotFoundException("Không tìm thấy thương hiệu G.Skill");
        var kingstonBrand = await context.Brands.FirstOrDefaultAsync(b => b.Name == "Kingston")
            ?? throw new NotFoundException("Không tìm thấy thương hiệu Kingston");
        var crucialBrand = await context.Brands.FirstOrDefaultAsync(b => b.Name == "Crucial")
            ?? throw new NotFoundException("Không tìm thấy thương hiệu Crucial");
        var teamgroupBrand = await context.Brands.FirstOrDefaultAsync(b => b.Name == "TeamGroup")
            ?? throw new NotFoundException("Không tìm thấy thương hiệu TeamGroup");
        var adataBrand = await context.Brands.FirstOrDefaultAsync(b => b.Name == "ADATA")
            ?? throw new NotFoundException("Không tìm thấy thương hiệu ADATA");
        var patriotBrand = await context.Brands.FirstOrDefaultAsync(b => b.Name == "Patriot")
            ?? throw new NotFoundException("Không tìm thấy thương hiệu Patriot");

        // ============= DDR5 HIGH-END GAMING/ENTHUSIAST =============

        // Corsair Dominator Platinum RGB DDR5-6400
        var corsairDominatorId = Guid.NewGuid();
        context.Products.Add(new Product
        {
            Id = corsairDominatorId,
            Name = "Corsair Dominator Platinum RGB DDR5-6400 32GB (2x16GB)",
            Sku = "RAM-CORSAIR-DOMINATOR-DDR5-6400-32GB",
            CategoryId = ramCategory.Id,
            BrandId = corsairBrand.Id,
            Price = 5990000,
            Stock = 25,
            Description = "RAM Corsair Dominator Platinum RGB DDR5-6400 32GB kit (2x16GB) CL32. Thiết kế Patented DHX cooling, 12 đèn LED CAPELLIX. Tối ưu cho Intel XMP 3.0 và AMD EXPO. RAM cao cấp nhất cho gaming và enthusiast.",
            WarrantyMonth = 60,
            Status = ProductStatus.Available,
        });
        AddRamSpecs(specValues, ramSpecs, corsairDominatorId, type: "DDR5", speed: 6400, capacityPerStick: 16, sticks: 2, latency: 32);
        productImages.Add(new ProductImage { ProductId = corsairDominatorId, ImageUrl = "https://www.corsair.com/medias/sys_master/images/images/hd3/h25/16920665325598/-CMT32GX5M2X6400C32-Gallery-?"});

        // G.Skill Trident Z5 RGB DDR5-6000
        var gskillTridentZ5Id = Guid.NewGuid();
        context.Products.Add(new Product
        {
            Id = gskillTridentZ5Id,
            Name = "G.Skill Trident Z5 RGB DDR5-6000 32GB (2x16GB)",
            Sku = "RAM-GSKILL-TRIDENTZ5-DDR5-6000-32GB",
            CategoryId = ramCategory.Id,
            BrandId = gskillBrand.Id,
            Price = 4990000,
            Stock = 30,
            Description = "RAM G.Skill Trident Z5 RGB DDR5-6000 32GB kit (2x16GB) CL30. Thiết kế heatsink nhôm cao cấp với LED RGB. Hỗ trợ Intel XMP 3.0. RAM flagship của G.Skill cho DDR5 platform.",
            WarrantyMonth = 60,
            Status = ProductStatus.Available,
        });
        AddRamSpecs(specValues, ramSpecs, gskillTridentZ5Id, type: "DDR5", speed: 6000, capacityPerStick: 16, sticks: 2, latency: 30);
        productImages.Add(new ProductImage { ProductId = gskillTridentZ5Id, ImageUrl = "https://www.gskill.com/img/pr_img/F5-6000J3038F16GX2-TZ5RK_01.png" });

        // Kingston Fury Renegade DDR5-6400
        var kingstonRenegadeId = Guid.NewGuid();
        context.Products.Add(new Product
        {
            Id = kingstonRenegadeId,
            Name = "Kingston Fury Renegade DDR5-6400 32GB (2x16GB)",
            Sku = "RAM-KINGSTON-FURY-RENEGADE-DDR5-6400-32GB",
            CategoryId = ramCategory.Id,
            BrandId = kingstonBrand.Id,
            Price = 5490000,
            Stock = 28,
            Description = "RAM Kingston Fury Renegade DDR5-6400 32GB kit (2x16GB) CL32. Thiết kế tản nhiệt aggressive với LED RGB. Hỗ trợ Intel XMP 3.0 và AMD EXPO. Hiệu năng extreme cho overclocker.",
            WarrantyMonth = 60,
            Status = ProductStatus.Available,
        });
        AddRamSpecs(specValues, ramSpecs, kingstonRenegadeId, type: "DDR5", speed: 6400, capacityPerStick: 16, sticks: 2, latency: 32);
        productImages.Add(new ProductImage { ProductId = kingstonRenegadeId, ImageUrl = "https://media.kingston.com/kingston/product/ktc-product-memory-fury-renegade-ddr5-rgb-2-lg.jpg" });

        // TeamGroup T-Force Delta RGB DDR5-6000
        var teamDeltaId = Guid.NewGuid();
        context.Products.Add(new Product
        {
            Id = teamDeltaId,
            Name = "TeamGroup T-Force Delta RGB DDR5-6000 32GB (2x16GB)",
            Sku = "RAM-TEAMGROUP-DELTA-DDR5-6000-32GB",
            CategoryId = ramCategory.Id,
            BrandId = teamgroupBrand.Id,
            Price = 4290000,
            Stock = 35,
            Description = "RAM TeamGroup T-Force Delta RGB DDR5-6000 32GB kit (2x16GB) CL30. LED RGB 120 độ sáng full-zone. Hỗ trợ Intel XMP 3.0 và AMD EXPO. Giá cạnh tranh cho DDR5 high-end.",
            WarrantyMonth = 60,
            Status = ProductStatus.Available,
        });
        AddRamSpecs(specValues, ramSpecs, teamDeltaId, type: "DDR5", speed: 6000, capacityPerStick: 16, sticks: 2, latency: 30);
        productImages.Add(new ProductImage { ProductId = teamDeltaId, ImageUrl = "https://www.teamgroupinc.com/en/upload/product_catalog/product/ctk/ckcp/ff48e2dcc.png" });

        // ADATA XPG Lancer RGB DDR5-6000
        var adataLancerRgbId = Guid.NewGuid();
        context.Products.Add(new Product
        {
            Id = adataLancerRgbId,
            Name = "ADATA XPG Lancer RGB DDR5-6000 32GB (2x16GB)",
            Sku = "RAM-ADATA-XPG-LANCER-RGB-DDR5-6000-32GB",
            CategoryId = ramCategory.Id,
            BrandId = adataBrand.Id,
            Price = 3990000,
            Stock = 40,
            Description = "RAM ADATA XPG Lancer RGB DDR5-6000 32GB kit (2x16GB) CL30. Thiết kế RGB blade hiện đại, tản nhiệt hiệu quả. Hỗ trợ Intel XMP 3.0 và AMD EXPO. Lựa chọn RGB DDR5 giá tốt.",
            WarrantyMonth = 60,
            Status = ProductStatus.Available,
        });
        AddRamSpecs(specValues, ramSpecs, adataLancerRgbId, type: "DDR5", speed: 6000, capacityPerStick: 16, sticks: 2, latency: 30);
        productImages.Add(new ProductImage { ProductId = adataLancerRgbId, ImageUrl = "https://www.xpg.com/upload/images/products/xpg-lancer-rgb-ddr5/gallery-1.png" });

        // ============= DDR5 WORKSTATION/HIGH CAPACITY =============

        // Corsair Vengeance DDR5-5600 64GB (2x32GB)
        var corsairVengeance64Id = Guid.NewGuid();
        context.Products.Add(new Product
        {
            Id = corsairVengeance64Id,
            Name = "Corsair Vengeance DDR5-5600 64GB (2x32GB)",
            Sku = "RAM-CORSAIR-VENGEANCE-DDR5-5600-64GB",
            CategoryId = ramCategory.Id,
            BrandId = corsairBrand.Id,
            Price = 6490000,
            Stock = 20,
            Description = "RAM Corsair Vengeance DDR5-5600 64GB kit (2x32GB) CL40. Thiết kế low-profile phù hợp mọi hệ thống. Dung lượng lớn cho workstation, content creation, video editing. Intel XMP 3.0.",
            WarrantyMonth = 60,
            Status = ProductStatus.Available,
        });
        AddRamSpecs(specValues, ramSpecs, corsairVengeance64Id, type: "DDR5", speed: 5600, capacityPerStick: 32, sticks: 2, latency: 40);
        productImages.Add(new ProductImage { ProductId = corsairVengeance64Id, ImageUrl = "https://www.corsair.com/medias/sys_master/images/images/hda/h14/16694755188766/-CMK64GX5M2B5600C40-Gallery-?"});

        // G.Skill Trident Z5 Neo DDR5-6000 64GB (2x32GB)
        var gskillNeo64Id = Guid.NewGuid();
        context.Products.Add(new Product
        {
            Id = gskillNeo64Id,
            Name = "G.Skill Trident Z5 Neo DDR5-6000 64GB (2x32GB)",
            Sku = "RAM-GSKILL-TRIDENTZ5-NEO-DDR5-6000-64GB",
            CategoryId = ramCategory.Id,
            BrandId = gskillBrand.Id,
            Price = 7990000,
            Stock = 15,
            Description = "RAM G.Skill Trident Z5 Neo DDR5-6000 64GB kit (2x32GB) CL30. Tối ưu cho AMD Ryzen 7000 với EXPO. Dung lượng lớn cho workstation và content creation chuyên nghiệp.",
            WarrantyMonth = 60,
            Status = ProductStatus.Available,
        });
        AddRamSpecs(specValues, ramSpecs, gskillNeo64Id, type: "DDR5", speed: 6000, capacityPerStick: 32, sticks: 2, latency: 30);
        productImages.Add(new ProductImage { ProductId = gskillNeo64Id, ImageUrl = "https://www.gskill.com/img/pr_img/F5-6000J3038F32GX2-TZ5N_01.png" });

        // Kingston Fury Beast DDR5-5600 64GB (2x32GB)
        var kingstonBeast64Id = Guid.NewGuid();
        context.Products.Add(new Product
        {
            Id = kingstonBeast64Id,
            Name = "Kingston Fury Beast DDR5-5600 64GB (2x32GB)",
            Sku = "RAM-KINGSTON-FURY-BEAST-DDR5-5600-64GB",
            CategoryId = ramCategory.Id,
            BrandId = kingstonBrand.Id,
            Price = 5990000,
            Stock = 22,
            Description = "RAM Kingston Fury Beast DDR5-5600 64GB kit (2x32GB) CL40. Thiết kế tản nhiệt hiệu quả, low-profile. Dung lượng lớn cho workstation, máy ảo, render video.",
            WarrantyMonth = 60,
            Status = ProductStatus.Available,
        });
        AddRamSpecs(specValues, ramSpecs, kingstonBeast64Id, type: "DDR5", speed: 5600, capacityPerStick: 32, sticks: 2, latency: 40);
        productImages.Add(new ProductImage { ProductId = kingstonBeast64Id, ImageUrl = "https://media.kingston.com/kingston/product/ktc-product-memory-fury-beast-ddr5-2-lg.jpg" });

        // Crucial Pro DDR5-5600 128GB (4x32GB) - Workstation
        var crucialPro128Id = Guid.NewGuid();
        context.Products.Add(new Product
        {
            Id = crucialPro128Id,
            Name = "Crucial Pro DDR5-5600 128GB Kit (4x32GB)",
            Sku = "RAM-CRUCIAL-PRO-DDR5-5600-128GB",
            CategoryId = ramCategory.Id,
            BrandId = crucialBrand.Id,
            Price = 12990000,
            Stock = 10,
            Description = "RAM Crucial Pro DDR5-5600 128GB kit (4x32GB) CL46. Dung lượng cực lớn cho workstation chuyên nghiệp, máy chủ, AI/ML training. Micron technology đảm bảo độ tin cậy cao.",
            WarrantyMonth = 60,
            Status = ProductStatus.Available,
        });
        AddRamSpecs(specValues, ramSpecs, crucialPro128Id, type: "DDR5", speed: 5600, capacityPerStick: 32, sticks: 4, latency: 46);
        productImages.Add(new ProductImage { ProductId = crucialPro128Id, ImageUrl = "https://www.crucial.com/content/dam/crucial/dram-products/pro/images/in-use/crucial-pro-ddr5-in-use.png" });

        // TeamGroup T-Create Expert DDR5-6000 64GB (2x32GB) - Workstation
        var teamCreateId = Guid.NewGuid();
        context.Products.Add(new Product
        {
            Id = teamCreateId,
            Name = "TeamGroup T-Create Expert DDR5-6000 64GB (2x32GB)",
            Sku = "RAM-TEAMGROUP-TCREATE-DDR5-6000-64GB",
            CategoryId = ramCategory.Id,
            BrandId = teamgroupBrand.Id,
            Price = 6290000,
            Stock = 18,
            Description = "RAM TeamGroup T-Create Expert DDR5-6000 64GB kit (2x32GB) CL34. Dòng sản phẩm chuyên cho content creator và workstation. Thiết kế tối giản, hiệu năng ổn định cho công việc chuyên nghiệp.",
            WarrantyMonth = 60,
            Status = ProductStatus.Available,
        });
        AddRamSpecs(specValues, ramSpecs, teamCreateId, type: "DDR5", speed: 6000, capacityPerStick: 32, sticks: 2, latency: 34);
        productImages.Add(new ProductImage { ProductId = teamCreateId, ImageUrl = "https://www.teamgroupinc.com/en/upload/product_catalog/product/ctk/ckcp/t-create-expert-ddr5.png" });

        // ============= DDR5 MID-RANGE =============

        // Corsair Vengeance DDR5-5600 32GB (2x16GB)
        var corsairVengeance32Id = Guid.NewGuid();
        context.Products.Add(new Product
        {
            Id = corsairVengeance32Id,
            Name = "Corsair Vengeance DDR5-5600 32GB (2x16GB)",
            Sku = "RAM-CORSAIR-VENGEANCE-DDR5-5600-32GB",
            CategoryId = ramCategory.Id,
            BrandId = corsairBrand.Id,
            Price = 3290000,
            Stock = 45,
            Description = "RAM Corsair Vengeance DDR5-5600 32GB kit (2x16GB) CL36. Thiết kế low-profile compact, phù hợp mọi build. Intel XMP 3.0 và AMD EXPO. Lựa chọn phổ biến cho DDR5 mainstream.",
            WarrantyMonth = 60,
            Status = ProductStatus.Available,
        });
        AddRamSpecs(specValues, ramSpecs, corsairVengeance32Id, type: "DDR5", speed: 5600, capacityPerStick: 16, sticks: 2, latency: 36);
        productImages.Add(new ProductImage { ProductId = corsairVengeance32Id, ImageUrl = "https://www.corsair.com/medias/sys_master/images/images/h8e/h07/16694755680286/-CMK32GX5M2B5600C36-Gallery-VENGEANCE-DDR5-BLACK-01.png_1200Wx1200H" });

        // Kingston Fury Beast DDR5-5200 32GB (2x16GB)
        var kingstonBeast32Id = Guid.NewGuid();
        context.Products.Add(new Product
        {
            Id = kingstonBeast32Id,
            Name = "Kingston Fury Beast DDR5-5200 32GB (2x16GB)",
            Sku = "RAM-KINGSTON-FURY-BEAST-DDR5-5200-32GB",
            CategoryId = ramCategory.Id,
            BrandId = kingstonBrand.Id,
            Price = 2890000,
            Stock = 50,
            Description = "RAM Kingston Fury Beast DDR5-5200 32GB kit (2x16GB) CL40. Thiết kế heatsink hiệu quả, Intel XMP 3.0 và AMD EXPO. RAM DDR5 giá tốt cho người dùng phổ thông.",
            WarrantyMonth = 60,
            Status = ProductStatus.Available,
        });
        AddRamSpecs(specValues, ramSpecs, kingstonBeast32Id, type: "DDR5", speed: 5200, capacityPerStick: 16, sticks: 2, latency: 40);
        productImages.Add(new ProductImage { ProductId = kingstonBeast32Id, ImageUrl = "https://media.kingston.com/kingston/product/ktc-product-memory-fury-beast-ddr5-lg.jpg" });

        // Crucial DDR5-4800 32GB (2x16GB)
        var crucial4800Id = Guid.NewGuid();
        context.Products.Add(new Product
        {
            Id = crucial4800Id,
            Name = "Crucial DDR5-4800 32GB (2x16GB)",
            Sku = "RAM-CRUCIAL-DDR5-4800-32GB",
            CategoryId = ramCategory.Id,
            BrandId = crucialBrand.Id,
            Price = 2490000,
            Stock = 55,
            Description = "RAM Crucial DDR5-4800 32GB kit (2x16GB) CL40. RAM chuẩn JEDEC, tương thích rộng rãi. Micron technology đảm bảo độ ổn định. Lựa chọn giá rẻ nhất cho DDR5.",
            WarrantyMonth = 60,
            Status = ProductStatus.Available,
        });
        AddRamSpecs(specValues, ramSpecs, crucial4800Id, type: "DDR5", speed: 4800, capacityPerStick: 16, sticks: 2, latency: 40);
        productImages.Add(new ProductImage { ProductId = crucial4800Id, ImageUrl = "https://www.crucial.com/content/dam/crucial/dram-products/ddr5/images/in-use/crucial-ddr5-in-use-image.png" });

        // ADATA XPG Lancer DDR5-5200 32GB (2x16GB)
        var adataLancer32Id = Guid.NewGuid();
        context.Products.Add(new Product
        {
            Id = adataLancer32Id,
            Name = "ADATA XPG Lancer DDR5-5200 32GB (2x16GB)",
            Sku = "RAM-ADATA-XPG-LANCER-DDR5-5200-32GB",
            CategoryId = ramCategory.Id,
            BrandId = adataBrand.Id,
            Price = 2690000,
            Stock = 48,
            Description = "RAM ADATA XPG Lancer DDR5-5200 32GB kit (2x16GB) CL38. Thiết kế heatsink đơn giản, hiệu năng ổn định. Hỗ trợ Intel XMP 3.0. RAM DDR5 mid-range đáng mua.",
            WarrantyMonth = 60,
            Status = ProductStatus.Available,
        });
        AddRamSpecs(specValues, ramSpecs, adataLancer32Id, type: "DDR5", speed: 5200, capacityPerStick: 16, sticks: 2, latency: 38);
        productImages.Add(new ProductImage { ProductId = adataLancer32Id, ImageUrl = "https://www.xpg.com/upload/images/products/xpg-lancer-ddr5/gallery-1.png" });

        // ============= DDR5 BUDGET =============

        // Patriot Viper Venom DDR5-5200 16GB (2x8GB)
        var patriotViperId = Guid.NewGuid();
        context.Products.Add(new Product
        {
            Id = patriotViperId,
            Name = "Patriot Viper Venom DDR5-5200 16GB (2x8GB)",
            Sku = "RAM-PATRIOT-VIPER-VENOM-DDR5-5200-16GB",
            CategoryId = ramCategory.Id,
            BrandId = patriotBrand.Id,
            Price = 1690000,
            Stock = 60,
            Description = "RAM Patriot Viper Venom DDR5-5200 16GB kit (2x8GB) CL40. Thiết kế heatsink đẹp mắt, giá cạnh tranh. Intel XMP 3.0. Entry-level DDR5 cho người mới lên platform.",
            WarrantyMonth = 60,
            Status = ProductStatus.Available,
        });
        AddRamSpecs(specValues, ramSpecs, patriotViperId, type: "DDR5", speed: 5200, capacityPerStick: 8, sticks: 2, latency: 40);
        productImages.Add(new ProductImage { ProductId = patriotViperId, ImageUrl = "https://www.patriotmemory.com/products/viper-venom-ddr5.png" });

        // TeamGroup T-Force Vulcan DDR5-5200 16GB (2x8GB)
        var teamVulcanId = Guid.NewGuid();
        context.Products.Add(new Product
        {
            Id = teamVulcanId,
            Name = "TeamGroup T-Force Vulcan DDR5-5200 16GB (2x8GB)",
            Sku = "RAM-TEAMGROUP-VULCAN-DDR5-5200-16GB",
            CategoryId = ramCategory.Id,
            BrandId = teamgroupBrand.Id,
            Price = 1590000,
            Stock = 65,
            Description = "RAM TeamGroup T-Force Vulcan DDR5-5200 16GB kit (2x8GB) CL40. Thiết kế đơn giản, tản nhiệt tốt. Intel XMP 3.0 và AMD EXPO. RAM DDR5 giá rẻ nhất thị trường.",
            WarrantyMonth = 60,
            Status = ProductStatus.Available,
        });
        AddRamSpecs(specValues, ramSpecs, teamVulcanId, type: "DDR5", speed: 5200, capacityPerStick: 8, sticks: 2, latency: 40);
        productImages.Add(new ProductImage { ProductId = teamVulcanId, ImageUrl = "https://www.teamgroupinc.com/en/upload/product_catalog/product/ctk/ckcp/t-force-vulcan-ddr5.png" });

        // ============= DDR4 (BACKWARD COMPATIBILITY) =============

        // G.Skill Trident Z Royal DDR4-3600 32GB (2x16GB)
        var gskillRoyalId = Guid.NewGuid();
        context.Products.Add(new Product
        {
            Id = gskillRoyalId,
            Name = "G.Skill Trident Z Royal DDR4-3600 32GB (2x16GB)",
            Sku = "RAM-GSKILL-TRIDENTZ-ROYAL-DDR4-3600-32GB",
            CategoryId = ramCategory.Id,
            BrandId = gskillBrand.Id,
            Price = 3990000,
            Stock = 25,
            Description = "RAM G.Skill Trident Z Royal DDR4-3600 32GB kit (2x16GB) CL16. Thiết kế crystal crown luxury với RGB. DDR4 cao cấp nhất cho Intel/AMD platform cũ. Hiệu năng và thẩm mỹ.",
            WarrantyMonth = 60,
            Status = ProductStatus.Available,
        });
        AddRamSpecs(specValues, ramSpecs, gskillRoyalId, type: "DDR4", speed: 3600, capacityPerStick: 16, sticks: 2, latency: 16);
        productImages.Add(new ProductImage { ProductId = gskillRoyalId, ImageUrl = "https://www.gskill.com/img/pr_img/F4-3600C16D-32GTRG_01.png" });

        // Corsair Vengeance LPX DDR4-3200 32GB (2x16GB)
        var corsairLpx32Id = Guid.NewGuid();
        context.Products.Add(new Product
        {
            Id = corsairLpx32Id,
            Name = "Corsair Vengeance LPX DDR4-3200 32GB (2x16GB)",
            Sku = "RAM-CORSAIR-VENGEANCE-LPX-DDR4-3200-32GB",
            CategoryId = ramCategory.Id,
            BrandId = corsairBrand.Id,
            Price = 2290000,
            Stock = 55,
            Description = "RAM Corsair Vengeance LPX DDR4-3200 32GB kit (2x16GB) CL16. Thiết kế low-profile phổ biến nhất. XMP 2.0 compatible. RAM DDR4 best-seller cho mọi build.",
            WarrantyMonth = 60,
            Status = ProductStatus.Available,
        });
        AddRamSpecs(specValues, ramSpecs, corsairLpx32Id, type: "DDR4", speed: 3200, capacityPerStick: 16, sticks: 2, latency: 16);
        productImages.Add(new ProductImage { ProductId = corsairLpx32Id, ImageUrl = "https://www.corsair.com/medias/sys_master/images/images/h8e/hfb/9109818802206/-CMK32GX4M2E3200C16-Gallery-?"});

        // Kingston Fury Beast DDR4-3200 16GB (2x8GB)
        var kingstonBeastDdr4Id = Guid.NewGuid();
        context.Products.Add(new Product
        {
            Id = kingstonBeastDdr4Id,
            Name = "Kingston Fury Beast DDR4-3200 16GB (2x8GB)",
            Sku = "RAM-KINGSTON-FURY-BEAST-DDR4-3200-16GB",
            CategoryId = ramCategory.Id,
            BrandId = kingstonBrand.Id,
            Price = 1190000,
            Stock = 70,
            Description = "RAM Kingston Fury Beast DDR4-3200 16GB kit (2x8GB) CL16. Thiết kế heatsink đẹp, XMP ready. RAM DDR4 giá rẻ phổ biến cho gaming và văn phòng.",
            WarrantyMonth = 60,
            Status = ProductStatus.Available,
        });
        AddRamSpecs(specValues, ramSpecs, kingstonBeastDdr4Id, type: "DDR4", speed: 3200, capacityPerStick: 8, sticks: 2, latency: 16);
        productImages.Add(new ProductImage { ProductId = kingstonBeastDdr4Id, ImageUrl = "https://media.kingston.com/kingston/product/ktc-product-memory-fury-beast-ddr4-lg.jpg" });

        // Crucial Ballistix DDR4-3600 32GB (2x16GB)
        var crucialBallistixId = Guid.NewGuid();
        context.Products.Add(new Product
        {
            Id = crucialBallistixId,
            Name = "Crucial Ballistix DDR4-3600 32GB (2x16GB)",
            Sku = "RAM-CRUCIAL-BALLISTIX-DDR4-3600-32GB",
            CategoryId = ramCategory.Id,
            BrandId = crucialBrand.Id,
            Price = 2590000,
            Stock = 40,
            Description = "RAM Crucial Ballistix DDR4-3600 32GB kit (2x16GB) CL16. Micron E-die nổi tiếng OC tốt. XMP 2.0 compatible. RAM DDR4 high-performance cho enthusiast.",
            WarrantyMonth = 60,
            Status = ProductStatus.Available,
        });
        AddRamSpecs(specValues, ramSpecs, crucialBallistixId, type: "DDR4", speed: 3600, capacityPerStick: 16, sticks: 2, latency: 16);
        productImages.Add(new ProductImage { ProductId = crucialBallistixId, ImageUrl = "https://www.crucial.com/content/dam/crucial/dram-products/ballistix-line/images/in-use/crucial-ballistix-ddr4-in-use.png" });
    }

    private static void AddRamSpecs(List<ProductSpecValue> specValues, List<SpecDefinition> specs, Guid productId, string type, int speed, int capacityPerStick, int sticks, int latency)
    {
        var typeSpec = specs.FirstOrDefault(s => s.Code == "ram_type");
        if (typeSpec != null) specValues.Add(new ProductSpecValue { ProductId = productId, SpecDefinitionId = typeSpec.Id, TextValue = type });

        var speedSpec = specs.FirstOrDefault(s => s.Code == "ram_speed");
        if (speedSpec != null) specValues.Add(new ProductSpecValue { ProductId = productId, SpecDefinitionId = speedSpec.Id, NumberValue = speed });

        var capacitySpec = specs.FirstOrDefault(s => s.Code == "ram_capacity");
        if (capacitySpec != null) specValues.Add(new ProductSpecValue { ProductId = productId, SpecDefinitionId = capacitySpec.Id, NumberValue = capacityPerStick });

        var sticksSpec = specs.FirstOrDefault(s => s.Code == "ram_sticks");
        if (sticksSpec != null) specValues.Add(new ProductSpecValue { ProductId = productId, SpecDefinitionId = sticksSpec.Id, NumberValue = sticks });

        var latencySpec = specs.FirstOrDefault(s => s.Code == "ram_latency");
        if (latencySpec != null) specValues.Add(new ProductSpecValue { ProductId = productId, SpecDefinitionId = latencySpec.Id, NumberValue = latency });
    }
}
