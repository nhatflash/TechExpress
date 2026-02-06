using System;
using Microsoft.EntityFrameworkCore;
using TechExpress.Repository.Contexts;
using TechExpress.Repository.Models;

namespace TechExpress.Service.Initializers;

public static class BrandsInitializer
{
    public static async Task Init(ApplicationDbContext context)
    {
        if (await context.Brands.AnyAsync())
        {
            return;
        }

        var brands = new List<Brand>
        {
            // ============= CPU MANUFACTURERS =============
            new Brand { Id = Guid.NewGuid(), Name = "Intel", ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/8/85/Intel_logo_2023.svg/1280px-Intel_logo_2023.svg.png" },
            new Brand { Id = Guid.NewGuid(), Name = "AMD", ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/7/7c/AMD_Logo.svg/960px-AMD_Logo.svg.png" },

            // ============= GPU / GRAPHICS CARD BRANDS =============
            new Brand { Id = Guid.NewGuid(), Name = "NVIDIA", ImageUrl = "https://www.nvidia.com/content/dam/en-zz/Solutions/about-nvidia/logo-and-brand/01-nvidia-logo-vert-500x200-2c50-d@2x.png" },
            new Brand { Id = Guid.NewGuid(), Name = "ASUS", ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/d/de/AsusTek-black-logo.png" },
            new Brand { Id = Guid.NewGuid(), Name = "MSI", ImageUrl = "https://upload.wikimedia.org/wikipedia/vi/6/6c/Msi_logo.png" },
            new Brand { Id = Guid.NewGuid(), Name = "Gigabyte", ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/c/c3/Gigabyte_Technology_logo_20080107.svg/1280px-Gigabyte_Technology_logo_20080107.svg.png" },
            new Brand { Id = Guid.NewGuid(), Name = "EVGA", ImageUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSb9klXtuRViECX_cYjSYEKjSMZ1gL-FoTVXw&s" },
            new Brand { Id = Guid.NewGuid(), Name = "Zotac", ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/7/7c/Logo_of_Zotac_International.svg/1280px-Logo_of_Zotac_International.svg.png" },
            new Brand { Id = Guid.NewGuid(), Name = "PNY", ImageUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRmMeTiFo5R7GFq--uT83SJk8ZlilOTiUsrBw&s" },
            new Brand { Id = Guid.NewGuid(), Name = "Palit", ImageUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQDVSmYsCAx8a1_2Dj2nBGxjjtjoZiihZZ-aw&s" },
            new Brand { Id = Guid.NewGuid(), Name = "Gainward", ImageUrl = "https://1000logos.net/wp-content/uploads/2020/05/Gainward-logo.jpg" },
            new Brand { Id = Guid.NewGuid(), Name = "Galax", ImageUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcR-EenurrlU2TUjWmAfbp-DJjs63LbH8jZkHg&s" },
            new Brand { Id = Guid.NewGuid(), Name = "INNO3D", ImageUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcS9f2lUVqc8cyNCjNNn4QFf8g9sSpedMmLtOA&s" },
            new Brand { Id = Guid.NewGuid(), Name = "Colorful", ImageUrl = "https://colorful.vn/wp-content/uploads/2018/10/Colorful-logo-03.png" },
            new Brand { Id = Guid.NewGuid(), Name = "XFX", ImageUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRNR8gH2j5ycHO1rDpDZZqCpAiJs2oSgchBNg&s" },
            new Brand { Id = Guid.NewGuid(), Name = "Sapphire", ImageUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQVwzMhMSb7KZ34eBv7qqXOqbBAKA7EAXCtsQ&s" },
            new Brand { Id = Guid.NewGuid(), Name = "PowerColor", ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/7/70/PowerColor_Logo.png" },

            // ============= MOTHERBOARD BRANDS =============
            new Brand { Id = Guid.NewGuid(), Name = "ASRock", ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/e/ed/ASRock_logo.svg" },
            new Brand { Id = Guid.NewGuid(), Name = "Biostar", ImageUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSnm1s6izPFPq2n8XRIkeJs5L_4Zi5PTSDE3g&s" },

            // ============= RAM / MEMORY BRANDS =============
            new Brand { Id = Guid.NewGuid(), Name = "Corsair", ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/e/e1/Corsair.svg/960px-Corsair.svg.png" },
            new Brand { Id = Guid.NewGuid(), Name = "G.Skill", ImageUrl = "https://upload.wikimedia.org/wikipedia/fr/7/78/G.Skill.gif" },
            new Brand { Id = Guid.NewGuid(), Name = "Kingston", ImageUrl = "https://media.kingston.com/kingston/opengraph/ktc-opengraph-homepage.jpg" },
            new Brand { Id = Guid.NewGuid(), Name = "Crucial", ImageUrl = "https://www.nicepng.com/png/detail/254-2544058_crucial-logo-computer-logo-marketing-information-crucial-by.png" },
            new Brand { Id = Guid.NewGuid(), Name = "TeamGroup", ImageUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcT-JgOZMpnw8lFsceUNMH6uFAhcMJzTJtXzDQ&s" },
            new Brand { Id = Guid.NewGuid(), Name = "ADATA", ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/d/d5/ADATA_Technology_logo_svg.png/1280px-ADATA_Technology_logo_svg.png" },
            new Brand { Id = Guid.NewGuid(), Name = "Patriot", ImageUrl = "https://wiselink.com.sg/wp-content/uploads/2021/05/client-patriot.png" },
            new Brand { Id = Guid.NewGuid(), Name = "Lexar", ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/d/d3/Lexar.png" },
            new Brand { Id = Guid.NewGuid(), Name = "V-Color", ImageUrl = "https://v-color.net/cdn/shop/files/V-COLOR-LOGO_7eedbcb5-d5fb-458b-ae78-b57f1d365359.png?v=1694080722&width=500" },

            // ============= STORAGE BRANDS (SSD/HDD) =============
            new Brand { Id = Guid.NewGuid(), Name = "Samsung", ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/f/f1/Samsung_logo_blue.png" },
            new Brand { Id = Guid.NewGuid(), Name = "Western Digital", ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/2/2b/Western_Digital_logo.svg" },
            new Brand { Id = Guid.NewGuid(), Name = "Seagate", ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/7/7f/Seagate_logo.svg/3840px-Seagate_logo.svg.png" },
            new Brand { Id = Guid.NewGuid(), Name = "SK Hynix", ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/8/81/SK_Hynix_logo_%282022%29.svg" },
            new Brand { Id = Guid.NewGuid(), Name = "Sabrent", ImageUrl = "https://m.media-amazon.com/images/S/aplus-media-library-service-media/8a448124-a01e-403b-b5a4-d30dfaf11626.__CR0,0,970,300_PT0_SX970_V1___.png" },
            new Brand { Id = Guid.NewGuid(), Name = "Kioxia", ImageUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRUVCabI1q8MJn1Gb0rCCPqRUixvc4vWwe6Pg&s" },

            // ============= PSU / POWER SUPPLY BRANDS =============
            new Brand { Id = Guid.NewGuid(), Name = "Seasonic", ImageUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQjpM5OvxGjgIAnuCs-v4ooZFnPYLOtvZyxWA&s" },
            new Brand { Id = Guid.NewGuid(), Name = "Cooler Master", ImageUrl = "https://logos-world.net/wp-content/uploads/2023/03/Cooler-Master-Logo.jpg" },
            new Brand { Id = Guid.NewGuid(), Name = "Thermaltake", ImageUrl = "https://media.bunnings.com.au/api/public/content/9e4df2c4aed14a49a8fd39d22e27b87d?v=89a1594a" },
            new Brand { Id = Guid.NewGuid(), Name = "be quiet!", ImageUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRFe3Qu8X3Op6Qtqy4JrOUCjISnkwl_cL8fFA&s" },
            new Brand { Id = Guid.NewGuid(), Name = "NZXT", ImageUrl = "https://assets.streamlinehq.com/image/private/w_300,h_300,ar_1/f_auto/v1/icons/logos/nzxt-9p37q3c2vb8kkphw1hvq1a.png/nzxt-z2nswkyv4jdj9sppxn2wc.png?_a=DATAiZAAZAA0" },
            new Brand { Id = Guid.NewGuid(), Name = "SilverStone", ImageUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcT1LsFvCA9_bHUNYcyKBdLopIU5AJtxLFPI8Q&s" },
            new Brand { Id = Guid.NewGuid(), Name = "Super Flower", ImageUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQKiIFg5q-bvI_nsL0qRGmPs3sgJxtkhLrHuA&s" },
            new Brand { Id = Guid.NewGuid(), Name = "FSP", ImageUrl = "https://tpucdn.com/review/fsp-np5-black-cpu-air-cooler/images/logo.jpg" },
            new Brand { Id = Guid.NewGuid(), Name = "Antec", ImageUrl = "https://storage.googleapis.com/www.taiwantradeshow.com.tw/exhibitor-logo/202503/T-37072848.jpg" },
            new Brand { Id = Guid.NewGuid(), Name = "Fractal Design", ImageUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSpqVbo474S4He6Vhzw7h2fIUeNUaAtHSMlZg&s" },
            new Brand { Id = Guid.NewGuid(), Name = "XPG", ImageUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSPLaZVWVzJX_aQRIQhg1_umpbxATtc1E7t_w&s" },
            new Brand { Id = Guid.NewGuid(), Name = "DeepCool", ImageUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQDXxooGVTSGhpfTFOyPsjUP9GqKEqoVuwSvg&s" },
            new Brand { Id = Guid.NewGuid(), Name = "Enermax", ImageUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTP29dQFjM_86Z8Fvd8utNs8Z0XuKFUuKyjaw&s" },

            // ============= CASE BRANDS =============
            new Brand { Id = Guid.NewGuid(), Name = "Lian Li", ImageUrl = "https://www.asetek.com/wp-content/uploads/2023/10/Lian-Li-LOGO2069x1138-black-1.jpg" },
            new Brand { Id = Guid.NewGuid(), Name = "Phanteks", ImageUrl = "https://dqov5rvavbmnl.cloudfront.net/images/feature_variant/4/Phanteks_logo.webp?t=1598074270" },
            new Brand { Id = Guid.NewGuid(), Name = "BitFenix", ImageUrl = "https://api.delta-computer.net/storage/brands/$2y$12$G8pto.lLWnciqN7SATtGQOImBdTZYTIQkfr31B0LSfNiv2zkUfO..webp" },
            new Brand { Id = Guid.NewGuid(), Name = "InWin", ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/a/aa/In_Win_2018_logo.svg" },
            new Brand { Id = Guid.NewGuid(), Name = "Cougar", ImageUrl = "https://www.nicepng.com/png/detail/267-2677150_cougar-case-logo.png" },
            new Brand { Id = Guid.NewGuid(), Name = "Montech", ImageUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQhnFYfnBy7NhI9G_PL0MC8knOgPciD4rfPkg&s" },
            new Brand { Id = Guid.NewGuid(), Name = "Jonsbo", ImageUrl = "https://www.tncstore.vn/media/brand/jonsbo.jpg" },

            // ============= CPU COOLER BRANDS =============
            new Brand { Id = Guid.NewGuid(), Name = "Noctua", ImageUrl = "https://dqov5rvavbmnl.cloudfront.net/images/feature_variant/4/noctua_logo.webp?t=1598074270" },
            new Brand { Id = Guid.NewGuid(), Name = "Arctic", ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/b/b5/ARCTIC_logo_white.png" },
            new Brand { Id = Guid.NewGuid(), Name = "Scythe", ImageUrl = "https://www.legitreviews.com/wp-content/uploads/2015/12/Scythe_Logo.jpg" },
            new Brand { Id = Guid.NewGuid(), Name = "Thermalright", ImageUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTv3zAyztQwGC8p9G2fFCNHp_3qptCbw7PNKg&s" },
            new Brand { Id = Guid.NewGuid(), Name = "ID-Cooling", ImageUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQ2h_J2ln8CVVyhKLvfPuHikulddNwdc01Yag&s" },
            new Brand { Id = Guid.NewGuid(), Name = "EKWB", ImageUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRBbYLsu7vVWCT8w4NZdhpuUSk3J3-CXvDgow&s" },
            new Brand { Id = Guid.NewGuid(), Name = "Alphacool", ImageUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcR-0K068phb61Pc65daY6Z-JjELLiGOSdhChQ&s" },

            // ============= PERIPHERAL BRANDS (Keyboard, Mouse, Headset) =============
            new Brand { Id = Guid.NewGuid(), Name = "Logitech", ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/a/a3/Logitech_logo.svg" },
            new Brand { Id = Guid.NewGuid(), Name = "Razer", ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/8/82/Razer_logo.svg" },
            new Brand { Id = Guid.NewGuid(), Name = "SteelSeries", ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/7/7f/SteelSeries_Logo.svg" },
            new Brand { Id = Guid.NewGuid(), Name = "HyperX", ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/3/3b/HyperX_logo.svg" },
            new Brand { Id = Guid.NewGuid(), Name = "Roccat", ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/c/ce/Roccat_logo.svg" },
            new Brand { Id = Guid.NewGuid(), Name = "Ducky", ImageUrl = "https://www.duckychannel.com.tw/images/logo.png" },
            new Brand { Id = Guid.NewGuid(), Name = "Keychron", ImageUrl = "https://www.keychron.com/images/logo.png" },
            new Brand { Id = Guid.NewGuid(), Name = "Akko", ImageUrl = "https://en.akkogear.com/images/logo.png" },
            new Brand { Id = Guid.NewGuid(), Name = "Glorious", ImageUrl = "https://www.gloriousgaming.com/images/logo.png" },
            new Brand { Id = Guid.NewGuid(), Name = "Finalmouse", ImageUrl = "https://finalmouse.com/images/logo.png" },
            new Brand { Id = Guid.NewGuid(), Name = "Zowie", ImageUrl = "https://zowie.benq.com/images/logo.png" },
            new Brand { Id = Guid.NewGuid(), Name = "Pulsar", ImageUrl = "https://www.pulsargamingmedia.com/images/logo.png" },
            new Brand { Id = Guid.NewGuid(), Name = "Endgame Gear", ImageUrl = "https://www.endgamegear.com/images/logo.png" },
            new Brand { Id = Guid.NewGuid(), Name = "Varmilo", ImageUrl = "https://en.varmilo.com/images/logo.png" },
            new Brand { Id = Guid.NewGuid(), Name = "Leopold", ImageUrl = "https://leopold.co.kr/images/logo.png" },
            new Brand { Id = Guid.NewGuid(), Name = "Das Keyboard", ImageUrl = "https://www.daskeyboard.com/images/logo.png" },
            new Brand { Id = Guid.NewGuid(), Name = "Wooting", ImageUrl = "https://wooting.io/images/logo.png" },
            new Brand { Id = Guid.NewGuid(), Name = "Lamzu", ImageUrl = "https://lamzu.com/images/logo.png" },
            new Brand { Id = Guid.NewGuid(), Name = "Vaxee", ImageUrl = "https://vaxee.com/images/logo.png" },
            new Brand { Id = Guid.NewGuid(), Name = "Lethal Gaming Gear", ImageUrl = "https://lethalgaminggear.com/images/logo.png" },

            // ============= MONITOR BRANDS =============
            new Brand { Id = Guid.NewGuid(), Name = "BenQ", ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/5/59/BenQ_Logo.svg" },
            new Brand { Id = Guid.NewGuid(), Name = "LG", ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/8/8a/LG_Logo.svg" },
            new Brand { Id = Guid.NewGuid(), Name = "Dell", ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/4/48/Dell_Logo.svg" },
            new Brand { Id = Guid.NewGuid(), Name = "Acer", ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/0/00/Acer_2011_logo.svg" },
            new Brand { Id = Guid.NewGuid(), Name = "ViewSonic", ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/f/fd/ViewSonic_Logo.svg" },
            new Brand { Id = Guid.NewGuid(), Name = "AOC", ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/7/74/AOC_logo.svg" },
            new Brand { Id = Guid.NewGuid(), Name = "Philips", ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/4/47/Philips_logo_logotype_emblem.svg" },
            new Brand { Id = Guid.NewGuid(), Name = "Alienware", ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/a/a7/Alienware_Logo.svg" },

            // ============= NETWORKING BRANDS =============
            new Brand { Id = Guid.NewGuid(), Name = "TP-Link", ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/e/e5/TP-Link_logo.svg" },
            new Brand { Id = Guid.NewGuid(), Name = "Netgear", ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/6/61/Netgear_logo.svg" },
            new Brand { Id = Guid.NewGuid(), Name = "Linksys", ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/f/f1/Linksys_logo.svg" },
            new Brand { Id = Guid.NewGuid(), Name = "D-Link", ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/7/78/D-Link_Logo.svg" },
            new Brand { Id = Guid.NewGuid(), Name = "Ubiquiti", ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/4/4f/Ubiquiti_Networks_2016.svg" },

            // ============= AUDIO BRANDS =============
            new Brand { Id = Guid.NewGuid(), Name = "Audio-Technica", ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/2/21/Audio-Technica_logo.svg" },
            new Brand { Id = Guid.NewGuid(), Name = "Sennheiser", ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/0/0b/Sennheiser_logo.svg" },
            new Brand { Id = Guid.NewGuid(), Name = "Beyerdynamic", ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/7/71/Beyerdynamic_logo.svg" },
            new Brand { Id = Guid.NewGuid(), Name = "Sony", ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/c/c9/Sony_logo.svg" },
            new Brand { Id = Guid.NewGuid(), Name = "JBL", ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/1/1c/JBL_logo.svg" },
            new Brand { Id = Guid.NewGuid(), Name = "Creative", ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/1/1e/Creative_Technology_logo.svg" },
            new Brand { Id = Guid.NewGuid(), Name = "Edifier", ImageUrl = "https://www.edifier.com/images/logo.png" },
            new Brand { Id = Guid.NewGuid(), Name = "Bose", ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/8/8c/Bose_logo.svg" },

            // ============= WEBCAM / STREAMING BRANDS =============
            new Brand { Id = Guid.NewGuid(), Name = "Elgato", ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/b/b2/Elgato_logo.svg" },
            new Brand { Id = Guid.NewGuid(), Name = "AVerMedia", ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/a/a1/AVerMedia_logo.svg" },
            new Brand { Id = Guid.NewGuid(), Name = "Blue Microphones", ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/2/21/Blue_Microphones_Logo.svg" },
            new Brand { Id = Guid.NewGuid(), Name = "Rode", ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/4/4e/R%C3%98DE_logo.svg" },
            new Brand { Id = Guid.NewGuid(), Name = "Shure", ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/4/4b/Shure_logo.svg" },

            // ============= GAMING CHAIR / FURNITURE BRANDS =============
            new Brand { Id = Guid.NewGuid(), Name = "Secretlab", ImageUrl = "https://secretlab.co/images/logo.png" },
            new Brand { Id = Guid.NewGuid(), Name = "noblechairs", ImageUrl = "https://www.noblechairs.com/images/logo.png" },
            new Brand { Id = Guid.NewGuid(), Name = "DXRacer", ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/b/ba/DXRacer_logo.svg" },
            new Brand { Id = Guid.NewGuid(), Name = "Vertagear", ImageUrl = "https://www.vertagear.com/images/logo.png" },
            new Brand { Id = Guid.NewGuid(), Name = "AndaSeat", ImageUrl = "https://www.andaseat.com/images/logo.png" },
            new Brand { Id = Guid.NewGuid(), Name = "Herman Miller", ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/e/e0/Herman_Miller_logo.svg" },
            new Brand { Id = Guid.NewGuid(), Name = "Autonomous", ImageUrl = "https://www.autonomous.ai/images/logo.png" },

            // ============= UPS BRANDS =============
            new Brand { Id = Guid.NewGuid(), Name = "APC", ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/8/8a/APC_by_Schneider_Electric_logo.svg" },
            new Brand { Id = Guid.NewGuid(), Name = "CyberPower", ImageUrl = "https://www.cyberpowersystems.com/images/logo.png" },
            new Brand { Id = Guid.NewGuid(), Name = "Eaton", ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/7/7f/Eaton_logo.svg" },

            // ============= LAPTOP BRANDS =============
            new Brand { Id = Guid.NewGuid(), Name = "Lenovo", ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/b/b8/Lenovo_logo_2015.svg" },
            new Brand { Id = Guid.NewGuid(), Name = "HP", ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/a/ad/HP_logo_2012.svg" },
            new Brand { Id = Guid.NewGuid(), Name = "Apple", ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/f/fa/Apple_logo_black.svg" },
            new Brand { Id = Guid.NewGuid(), Name = "Xiaomi", ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/2/29/Xiaomi_logo.svg" },
            new Brand { Id = Guid.NewGuid(), Name = "Huawei", ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/e/e8/Huawei_Logo.svg" },
            new Brand { Id = Guid.NewGuid(), Name = "Razer Blade", ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/8/82/Razer_logo.svg" },
        };

        context.Brands.AddRange(brands);
        await context.SaveChangesAsync();
    }
}
