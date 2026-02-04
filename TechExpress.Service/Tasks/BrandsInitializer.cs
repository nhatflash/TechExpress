using System;
using Microsoft.EntityFrameworkCore;
using TechExpress.Repository.Contexts;
using TechExpress.Repository.Models;

namespace TechExpress.Service.Tasks;

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
            new Brand { Id = Guid.NewGuid(), Name = "Intel", ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/7/7d/Intel_logo_%282006-2020%29.svg" },
            new Brand { Id = Guid.NewGuid(), Name = "AMD", ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/7/7c/AMD_Logo.svg" },

            // ============= GPU / GRAPHICS CARD BRANDS =============
            new Brand { Id = Guid.NewGuid(), Name = "NVIDIA", ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/2/21/Nvidia_logo.svg" },
            new Brand { Id = Guid.NewGuid(), Name = "ASUS", ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/d/d9/Asus_logo.svg" },
            new Brand { Id = Guid.NewGuid(), Name = "MSI", ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/1/13/MSI_Logo.svg" },
            new Brand { Id = Guid.NewGuid(), Name = "Gigabyte", ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/3/32/Gigabyte_Technology_logo_20080107.svg" },
            new Brand { Id = Guid.NewGuid(), Name = "EVGA", ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/e/e6/EVGA_logo.svg" },
            new Brand { Id = Guid.NewGuid(), Name = "Zotac", ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/e/e1/ZOTAC_logo.svg" },
            new Brand { Id = Guid.NewGuid(), Name = "PNY", ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/c/c2/PNY_Logo.svg" },
            new Brand { Id = Guid.NewGuid(), Name = "Palit", ImageUrl = "https://www.palit.com/images/logo/palit_logo.png" },
            new Brand { Id = Guid.NewGuid(), Name = "Gainward", ImageUrl = "https://www.gainward.com/images/logo/gainward_logo.png" },
            new Brand { Id = Guid.NewGuid(), Name = "Galax", ImageUrl = "https://www.galax.com/images/logo/galax_logo.png" },
            new Brand { Id = Guid.NewGuid(), Name = "INNO3D", ImageUrl = "https://inno3d.com/images/logo.png" },
            new Brand { Id = Guid.NewGuid(), Name = "Colorful", ImageUrl = "https://en.colorful.cn/images/logo.png" },
            new Brand { Id = Guid.NewGuid(), Name = "XFX", ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/5/5f/XFX_Logo.svg" },
            new Brand { Id = Guid.NewGuid(), Name = "Sapphire", ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/8/80/Sapphire_Technology_logo.svg" },
            new Brand { Id = Guid.NewGuid(), Name = "PowerColor", ImageUrl = "https://www.powercolor.com/images/logo.png" },

            // ============= MOTHERBOARD BRANDS =============
            new Brand { Id = Guid.NewGuid(), Name = "ASRock", ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/1/1e/ASRock_Logo.svg" },
            new Brand { Id = Guid.NewGuid(), Name = "Biostar", ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/7/7b/Biostar_logo.svg" },

            // ============= RAM / MEMORY BRANDS =============
            new Brand { Id = Guid.NewGuid(), Name = "Corsair", ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/9/9c/Corsair_logo.svg" },
            new Brand { Id = Guid.NewGuid(), Name = "G.Skill", ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/8/85/G.Skill_logo.svg" },
            new Brand { Id = Guid.NewGuid(), Name = "Kingston", ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/9/9a/Kingston_Technology_logo.svg" },
            new Brand { Id = Guid.NewGuid(), Name = "Crucial", ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/2/2a/Crucial_logo.svg" },
            new Brand { Id = Guid.NewGuid(), Name = "TeamGroup", ImageUrl = "https://www.teamgroupinc.com/images/logo.png" },
            new Brand { Id = Guid.NewGuid(), Name = "ADATA", ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/e/e6/ADATA_Technology_logo_image.svg" },
            new Brand { Id = Guid.NewGuid(), Name = "Patriot", ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/e/e0/Patriot_Memory_logo.svg" },
            new Brand { Id = Guid.NewGuid(), Name = "Lexar", ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/4/44/Lexar_logo.svg" },
            new Brand { Id = Guid.NewGuid(), Name = "V-Color", ImageUrl = "https://www.v-color.net/images/logo.png" },

            // ============= STORAGE BRANDS (SSD/HDD) =============
            new Brand { Id = Guid.NewGuid(), Name = "Samsung", ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/2/24/Samsung_Logo.svg" },
            new Brand { Id = Guid.NewGuid(), Name = "Western Digital", ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/2/2d/Western_Digital_logo.svg" },
            new Brand { Id = Guid.NewGuid(), Name = "Seagate", ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/8/86/Seagate_logo.svg" },
            new Brand { Id = Guid.NewGuid(), Name = "SK Hynix", ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/8/81/SK_Hynix_logo_%282022%29.svg" },
            new Brand { Id = Guid.NewGuid(), Name = "Sabrent", ImageUrl = "https://www.sabrent.com/images/logo.png" },
            new Brand { Id = Guid.NewGuid(), Name = "Kioxia", ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/5/5e/Kioxia_logo.svg" },

            // ============= PSU / POWER SUPPLY BRANDS =============
            new Brand { Id = Guid.NewGuid(), Name = "Seasonic", ImageUrl = "https://seasonic.com/images/logo.png" },
            new Brand { Id = Guid.NewGuid(), Name = "Cooler Master", ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/4/4d/Cooler_Master_Logo.svg" },
            new Brand { Id = Guid.NewGuid(), Name = "Thermaltake", ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/8/86/Thermaltake_logo.svg" },
            new Brand { Id = Guid.NewGuid(), Name = "be quiet!", ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/1/1a/Be_quiet_logo.svg" },
            new Brand { Id = Guid.NewGuid(), Name = "NZXT", ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/9/9d/NZXT_logo.svg" },
            new Brand { Id = Guid.NewGuid(), Name = "SilverStone", ImageUrl = "https://www.silverstonetek.com/images/logo.png" },
            new Brand { Id = Guid.NewGuid(), Name = "Super Flower", ImageUrl = "https://www.super-flower.com/images/logo.png" },
            new Brand { Id = Guid.NewGuid(), Name = "FSP", ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/5/5e/FSP_Group_logo.svg" },
            new Brand { Id = Guid.NewGuid(), Name = "Antec", ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/9/96/Antec_Logo.svg" },
            new Brand { Id = Guid.NewGuid(), Name = "Fractal Design", ImageUrl = "https://www.fractal-design.com/images/logo.png" },
            new Brand { Id = Guid.NewGuid(), Name = "XPG", ImageUrl = "https://www.xpg.com/images/logo.png" },
            new Brand { Id = Guid.NewGuid(), Name = "DeepCool", ImageUrl = "https://www.deepcool.com/images/logo.png" },
            new Brand { Id = Guid.NewGuid(), Name = "Enermax", ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/3/3e/Enermax_logo.svg" },

            // ============= CASE BRANDS =============
            new Brand { Id = Guid.NewGuid(), Name = "Lian Li", ImageUrl = "https://lian-li.com/images/logo.png" },
            new Brand { Id = Guid.NewGuid(), Name = "Phanteks", ImageUrl = "https://www.phanteks.com/images/logo.png" },
            new Brand { Id = Guid.NewGuid(), Name = "BitFenix", ImageUrl = "https://www.bitfenix.com/images/logo.png" },
            new Brand { Id = Guid.NewGuid(), Name = "InWin", ImageUrl = "https://www.in-win.com/images/logo.png" },
            new Brand { Id = Guid.NewGuid(), Name = "Cougar", ImageUrl = "https://cougargaming.com/images/logo.png" },
            new Brand { Id = Guid.NewGuid(), Name = "Montech", ImageUrl = "https://www.montech.co/images/logo.png" },
            new Brand { Id = Guid.NewGuid(), Name = "Jonsbo", ImageUrl = "https://www.jonsbo.com/images/logo.png" },

            // ============= CPU COOLER BRANDS =============
            new Brand { Id = Guid.NewGuid(), Name = "Noctua", ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/f/f2/Noctua_Logo.svg" },
            new Brand { Id = Guid.NewGuid(), Name = "Arctic", ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/1/1f/ARCTIC_Logo.svg" },
            new Brand { Id = Guid.NewGuid(), Name = "Scythe", ImageUrl = "https://www.scythe-eu.com/images/logo.png" },
            new Brand { Id = Guid.NewGuid(), Name = "Thermalright", ImageUrl = "https://www.thermalright.com/images/logo.png" },
            new Brand { Id = Guid.NewGuid(), Name = "ID-Cooling", ImageUrl = "https://www.idcooling.com/images/logo.png" },
            new Brand { Id = Guid.NewGuid(), Name = "EKWB", ImageUrl = "https://www.ekwb.com/images/logo.png" },
            new Brand { Id = Guid.NewGuid(), Name = "Alphacool", ImageUrl = "https://www.alphacool.com/images/logo.png" },

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
