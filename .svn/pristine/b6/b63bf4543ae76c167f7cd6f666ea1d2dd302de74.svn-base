using System;

namespace BizInfo.Harvesting.Services.Scouting.Management
{
    public static class ScoutingManagerTools
    {
        private static ScoutingManager defaultManager;

        public static ScoutingManager Default
        {
            get
            {
                if (defaultManager == null) defaultManager = CreateDefaultManager();
                return defaultManager;
            }
        }

        private static ScoutingManager CreateDefaultManager()
        {
            var manager = new ScoutingManager();
           
            manager.AddScout(@"inzertexpres.cz/reality/vymeny", new InzertExpresScout() { InitialPageUrl = @"http://www.inzertexpres.cz/reality/vymeny" });
            manager.AddScout(@"inzertexpres.cz/reality/pronajmy-podnajmy", new InzertExpresScout() { InitialPageUrl = @"http://www.inzertexpres.cz/reality/pronajmy-podnajmy" });
            manager.AddScout(@"inzertexpres.cz/reality/nemovitosti", new InzertExpresScout() { InitialPageUrl = @"http://www.inzertexpres.cz/reality/nemovitosti" });

            manager.AddScout(@"inzertexpres.cz/auto-moto/osobni-automobily", new InzertExpresScout() { InitialPageUrl = @"http://www.inzertexpres.cz/auto-moto/osobni-automobily" });
            manager.AddScout(@"inzertexpres.cz/auto-moto/motocykly", new InzertExpresScout() { InitialPageUrl = @"http://www.inzertexpres.cz/auto-moto/motocykly" });
            manager.AddScout(@"inzertexpres.cz/auto-moto/nahr-dily-a-prislusenstvi", new InzertExpresScout() { InitialPageUrl = @"http://www.inzertexpres.cz/auto-moto/nahr-dily-a-prislusenstvi" });
            manager.AddScout(@"inzertexpres.cz/auto-moto/uzitkova-vozidla", new InzertExpresScout() { InitialPageUrl = @"http://www.inzertexpres.cz/auto-moto/uzitkova-vozidla" });

            manager.AddScout(@"annonce.cz/byty-k-pronajmu", new AnnonceScout() { InitialPageUrl = @"http://www.annonce.cz/byty-k-pronajmu$334-filter.html?type=&disposal=&price_from=od&price_to=do&q=&location_country=&property_type=&listStyle=table&maxAge=&nabidkovy=2&perPage=1000&action.x=95&action.y=17" });
            manager.AddScout(@"annonce.cz/byty-na-prodej", new AnnonceScout() { InitialPageUrl = @"http://www.annonce.cz/byty-na-prodej$334-filter.html?type=&disposal=&price_from=od&price_to=do&q=&location_country=&property_type=&listStyle=table&maxAge=&nabidkovy=2&perPage=1000&action.x=95&action.y=17" });
            manager.AddScout(@"annonce.cz/domy-k-pronajmu", new AnnonceScout() { InitialPageUrl = @"http://www.annonce.cz/domy-k-pronajmu$334-filter.html?type=&disposal=&price_from=od&price_to=do&q=&location_country=&property_type=&listStyle=table&maxAge=&nabidkovy=2&perPage=1000&action.x=95&action.y=17" });
            manager.AddScout(@"annonce.cz/domy-na-prodej", new AnnonceScout() { InitialPageUrl = @"http://www.annonce.cz/domy-na-prodej$334-filter.html?type=&disposal=&price_from=od&price_to=do&q=&location_country=&property_type=&listStyle=table&maxAge=&nabidkovy=2&perPage=1000&action.x=95&action.y=17" });
            manager.AddScout(@"annonce.cz/komercni-prostory", new AnnonceScout() { InitialPageUrl = @"http://www.annonce.cz/komercni-prostory$334-filter.html?type=&disposal=&price_from=od&price_to=do&q=&location_country=&property_type=&listStyle=table&maxAge=&nabidkovy=2&perPage=1000&action.x=95&action.y=17" });
            manager.AddScout(@"annonce.cz/pozemky", new AnnonceScout() { InitialPageUrl = @"http://www.annonce.cz/pozemky$334-filter.html?type=&disposal=&price_from=od&price_to=do&q=&location_country=&property_type=&listStyle=table&maxAge=&nabidkovy=2&perPage=1000&action.x=95&action.y=17" });
            manager.AddScout(@"annonce.cz/vymeny-realit", new AnnonceScout() { InitialPageUrl = @"http://www.annonce.cz/vymeny-realit$334-filter.html?type=&disposal=&price_from=od&price_to=do&q=&location_country=&property_type=&listStyle=table&maxAge=&nabidkovy=2&perPage=1000&action.x=95&action.y=17" });
            manager.AddScout(@"annonce.cz/osobni-vozy", new AnnonceScout() { InitialPageUrl = @"http://www.annonce.cz/osobni-vozy$334-filter.html?type=&disposal=&price_from=od&price_to=do&q=&location_country=&property_type=&listStyle=table&maxAge=&nabidkovy=2&perPage=1000&action.x=95&action.y=17" });
            manager.AddScout(@"annonce.cz/uzitkova-vozidla-zemedelska-technika", new AnnonceScout() { InitialPageUrl = @"http://www.annonce.cz/uzitkova-vozidla-zemedelska-technika$334-filter.html?type=&disposal=&price_from=od&price_to=do&q=&location_country=&property_type=&listStyle=table&maxAge=&nabidkovy=2&perPage=1000&action.x=95&action.y=17" });
            manager.AddScout(@"annonce.cz/motocykly", new AnnonceScout() { InitialPageUrl = @"http://www.annonce.cz/motocykly$334-filter.html?type=&disposal=&price_from=od&price_to=do&q=&location_country=&property_type=&listStyle=table&maxAge=&nabidkovy=2&perPage=1000&action.x=95&action.y=17" });
            manager.AddScout(@"annonce.cz/nahradni-dily", new AnnonceScout() { InitialPageUrl = @"http://www.annonce.cz/nahradni-dily$334-filter.html?type=&disposal=&price_from=od&price_to=do&q=&location_country=&property_type=&listStyle=table&maxAge=&nabidkovy=2&perPage=1000&action.x=95&action.y=17" });
            manager.AddScout(@"annonce.cz/prislusenstvi-auto-moto", new AnnonceScout() { InitialPageUrl = @"http://www.annonce.cz/prislusenstvi-auto-moto$334-filter.html?type=&disposal=&price_from=od&price_to=do&q=&location_country=&property_type=&listStyle=table&maxAge=&nabidkovy=2&perPage=1000&action.x=95&action.y=17" });

            manager.AddScout(@"auto.bazos.cz", new BazosScout() { InitialPageUrl = @"http://auto.bazos.cz/" });
            manager.AddScout(@"motorky.bazos.cz", new BazosScout() { InitialPageUrl = @"http://motorky.bazos.cz/" });
            manager.AddScout(@"reality.bazos.cz", new BazosScout() { InitialPageUrl = @"http://reality.bazos.cz/" });

            manager.AddScout(@"nakladni-uzitkove-vozy.hyperinzerce.cz", new HyperInzerceScout() { InitialPageUrl = @"http://nakladni-uzitkove-vozy.hyperinzerce.cz/inzerce/" });
            manager.AddScout(@"autobazar.hyperinzerce.cz", new HyperInzerceScout() { InitialPageUrl = @"http://autobazar.hyperinzerce.cz/inzerce/" });
            manager.AddScout(@"motorky.hyperinzerce.cz", new HyperInzerceScout() { InitialPageUrl = @"http://motorky.hyperinzerce.cz/inzerce/" });
            manager.AddScout(@"nemovitosti-reality.hyperinzerce.cz", new HyperInzerceScout() { InitialPageUrl = @"http://nemovitosti-reality.hyperinzerce.cz/inzerce/" });
            
            // sbazar.cz
            manager.AddScout(@"autobazar.sbazar.cz", new SBazarScout() { InitialPageUrl = @"http://autobazar.sbazar.cz/prodam-koupim-aukce.html?order=date_create&by=desc" });
            manager.AddScout(@"motobazar.sbazar.cz", new SBazarScout() { InitialPageUrl = @"http://motobazar.sbazar.cz/prodam-koupim-aukce.html?order=date_create&by=desc" });
            manager.AddScout(@"byty-reality.sbazar.cz", new SBazarScout() { InitialPageUrl = @"http://byty-reality.sbazar.cz/prodam-koupim-aukce.html?order=date_create&by=desc" });

            // avizo.cz
            manager.AddScout(@"autobazar.avizo.cz/osobni-automobily", new AvizoScout() { InitialPageUrl = @"http://autobazar.avizo.cz/osobni-automobily/" });
            manager.AddScout(@"autobazar.avizo.cz/nove-a-zanovni-vozy", new AvizoScout() { InitialPageUrl = @"http://autobazar.avizo.cz/nove-a-zanovni-vozy/" });
            manager.AddScout(@"autobazar.avizo.cz/uzitkova-vozidla", new AvizoScout() { InitialPageUrl = @"http://autobazar.avizo.cz/uzitkova-vozidla/" });
            manager.AddScout(@"autobazar.avizo.cz/auto-hifi", new AvizoScout() { InitialPageUrl = @"http://autobazar.avizo.cz/auto-hifi/" });
            manager.AddScout(@"autobazar.avizo.cz/nakladni-automobily", new AvizoScout() { InitialPageUrl = @"http://autobazar.avizo.cz/nakladni-automobily/" });
            manager.AddScout(@"autobazar.avizo.cz/auto-prislusenstvi", new AvizoScout() { InitialPageUrl = @"http://autobazar.avizo.cz/auto-prislusenstvi/" });
            manager.AddScout(@"autobazar.avizo.cz/autobusy", new AvizoScout() { InitialPageUrl = @"http://autobazar.avizo.cz/autobusy/" });
            manager.AddScout(@"autobazar.avizo.cz/auto-moto-tuning", new AvizoScout() { InitialPageUrl = @"http://autobazar.avizo.cz/auto-moto-tuning/" });
            manager.AddScout(@"autobazar.avizo.cz/ostatni-vozidla", new AvizoScout() { InitialPageUrl = @"http://autobazar.avizo.cz/ostatni-vozidla/" });
            manager.AddScout(@"autobazar.avizo.cz/sluzby-pro-motoristy", new AvizoScout() { InitialPageUrl = @"http://autobazar.avizo.cz/sluzby-pro-motoristy/" });
            manager.AddScout(@"autobazar.avizo.cz/veterani", new AvizoScout() { InitialPageUrl = @"http://autobazar.avizo.cz/veterani/" });
            manager.AddScout(@"autobazar.avizo.cz/nahradni-dily", new AvizoScout() { InitialPageUrl = @"http://autobazar.avizo.cz/nahradni-dily/" });
            manager.AddScout(@"autobazar.avizo.cz/pneumatiky-kola", new AvizoScout() { InitialPageUrl = @"http://autobazar.avizo.cz/pneumatiky-kola/" });
            manager.AddScout(@"autobazar.avizo.cz/doprava-stehovani", new AvizoScout() { InitialPageUrl = @"http://autobazar.avizo.cz/doprava-stehovani/" });

            manager.AddScout(@"reality.avizo.cz/byty", new AvizoScout() { InitialPageUrl = @"http://reality.avizo.cz/byty/" });
            manager.AddScout(@"reality.avizo.cz/nove-bydleni-novostavby", new AvizoScout() { InitialPageUrl = @"http://reality.avizo.cz/nove-bydleni-novostavby/" });
            manager.AddScout(@"reality.avizo.cz/rodinne-domy", new AvizoScout() { InitialPageUrl = @"http://reality.avizo.cz/rodinne-domy/" });
            manager.AddScout(@"reality.avizo.cz/najemni-domy", new AvizoScout() { InitialPageUrl = @"http://reality.avizo.cz/najemni-domy/" });
            manager.AddScout(@"reality.avizo.cz/pozemky-zahrady", new AvizoScout() { InitialPageUrl = @"http://reality.avizo.cz/pozemky-zahrady/" });
            manager.AddScout(@"reality.avizo.cz/vyrobni-a-skladovaci-prostory", new AvizoScout() { InitialPageUrl = @"http://reality.avizo.cz/vyrobni-a-skladovaci-prostory/" });
            manager.AddScout(@"reality.avizo.cz/chaty-a-chalupy", new AvizoScout() { InitialPageUrl = @"http://reality.avizo.cz/chaty-a-chalupy/" });
            manager.AddScout(@"reality.avizo.cz/garaze", new AvizoScout() { InitialPageUrl = @"http://reality.avizo.cz/garaze/" });
            manager.AddScout(@"reality.avizo.cz/ubytovaci-a-stravovaci-prostory", new AvizoScout() { InitialPageUrl = @"http://reality.avizo.cz/ubytovaci-a-stravovaci-prostory/" });
            manager.AddScout(@"reality.avizo.cz/ostatni-nemovitosti", new AvizoScout() { InitialPageUrl = @"http://reality.avizo.cz/ostatni-nemovitosti/" });
            manager.AddScout(@"reality.avizo.cz/obchodni-a-kancelarske-prostory", new AvizoScout() { InitialPageUrl = @"http://reality.avizo.cz/obchodni-a-kancelarske-prostory/" });
            
            // bezrealitky.cz
            manager.AddScout(@"nabidka.bezrealitky.cz", new BezRealitkyScout() { InitialPageUrl = @"http://www.bezrealitky.cz/vyhledat-light/nabizim-vse/vse?order_type=date&order_direction=desc" });
            manager.AddScout(@"poptavka.bezrealitky.cz", new BezRealitkyScout() { InitialPageUrl = @"http://www.bezrealitky.cz/vyhledat-light/hledam-vse/vse?order_type=date&order_direction=desc" });
 
            // aukro.cz
            manager.AddScout(@"aukro.cz/auto", new AukroScout() { InitialPageUrl = @"http://auto.aukro.cz/listing.php/showcat?id=8503&order=td&p=1" });
            manager.AddScout(@"aukro.cz/moto", new AukroScout() { InitialPageUrl = @"http://auto.aukro.cz/listing.php/showcat?id=8502&order=td&p=1" });
            manager.AddScout(@"aukro.cz/auto-nahradni-dily", new AukroScout() { InitialPageUrl = @"http://auto.aukro.cz/listing.php/showcat?id=8500&order=td&p=1" });
            manager.AddScout(@"aukro.cz/moto-nahradni-dily", new AukroScout() { InitialPageUrl = @"http://auto.aukro.cz/listing.php/showcat?id=48787&order=td&p=1" });
            manager.AddScout(@"aukro.cz/cargo-nahradni-dily", new AukroScout() { InitialPageUrl = @"http://auto.aukro.cz/listing.php/showcat?id=82827&order=td&p=1" });
            manager.AddScout(@"aukro.cz/oldtimer", new AukroScout() { InitialPageUrl = @"http://auto.aukro.cz/listing.php/showcat?id=48796&order=td&p=1" });
            manager.AddScout(@"aukro.cz/privesy-navesy", new AukroScout() { InitialPageUrl = @"http://auto.aukro.cz/listing.php/showcat?id=73127&order=td&p=1" });
            manager.AddScout(@"aukro.cz/stroje", new AukroScout() { InitialPageUrl = @"http://auto.aukro.cz/listing.php/showcat?id=73128&order=td&p=1" });
            return manager;
        }
    }
}