using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace BizInfo.Model.Entities
{
    public class StructuredContent
    {
        private readonly XElement root;

        public StructuredContent() : this(new XElement("structured"))
        {
        }

        public StructuredContent(XElement root)
        {
            this.root = root;
        }

        public string KindOfInfo
        {
            get { return GetValueOf("kindOfInfo"); }
            set { SetValueOf("kindOfInfo", value); }
        }

        public string Transaction
        {
            get { return GetValueOf("transaction"); }
            set { SetValueOf("transaction", value); }
        }

        public string Price
        {
            get { return GetValueOf("price"); }
            set { SetValueOf("price", value); }
        }

        public string Location
        {
            get { return GetValueOf("location"); }
            set { SetValueOf("location", value); }
        }

        public string Author
        {
            get { return GetValueOf("author"); }
            set { SetValueOf("author", value); }
        }

        /// <summary>
        /// Fyzická adresa
        /// </summary>
        public string Address
        {
            get { return GetValueOf("address"); }
            set { SetValueOf("address", value); }
        }

        /// <summary>
        /// Zemì, stát
        /// </summary>
        public string Country
        {
            get { return GetValueOf("country"); }
            set { SetValueOf("country", value); }
        }

        /// <summary>
        /// Kraj
        /// </summary>
        public string County
        {
            get { return GetValueOf("county"); }
            set { SetValueOf("county", value); }
        }

        /// <summary>
        /// Okres
        /// </summary>
        public string District
        {
            get { return GetValueOf("district"); }
            set { SetValueOf("district", value); }
        }

        /// <summary>
        /// Mìsto
        /// </summary>
        public string City
        {
            get { return GetValueOf("city"); }
            set { SetValueOf("city", value); }
        }

        /// <summary>
        /// Ulice
        /// </summary>
        public string Street
        {
            get { return GetValueOf("street"); }
            set { SetValueOf("street", value); }
        }

        public string Brand
        {
            get { return GetValueOf("brand"); }
            set { SetValueOf("brand", value); }
        }

        public string ModelRange
        {
            get { return GetValueOf("modelRange"); }
            set { SetValueOf("modelRange", value); }
        }

        /// <summary>
        /// Varianta modelu
        /// </summary>
        public string Variation
        {
            get { return GetValueOf("variation"); }
            set { SetValueOf("variation", value); }
        }

        public string Model
        {
            get { return GetValueOf("model"); }
            set { SetValueOf("model", value); }
        }

        public string Manufactured
        {
            get { return GetValueOf("manufactured"); }
            set { SetValueOf("manufactured", value); }
        }

        public string Accepted
        {
            get { return GetValueOf("accepted"); }
            set { SetValueOf("accepted", value); }
        }

        public string Reconstructed
        {
            get { return GetValueOf("reconstructed"); }
            set { SetValueOf("reconstructed", value); }
        }

        public string Kind
        {
            get { return GetValueOf("kind"); }
            set { SetValueOf("kind", value); }
        }

        public string Mileage
        {
            get { return GetValueOf("mileage"); }
            set { SetValueOf("mileage", value); }
        }

        public string KindOfBody
        {
            get { return GetValueOf("kindOfBody"); }
            set { SetValueOf("kindOfBody", value); }
        }

        /// <summary>
        /// Palivo, druh paliva (nafta, benzín...)
        /// </summary>
        public string Fuel
        {
            get { return GetValueOf("fuel"); }
            set { SetValueOf("fuel", value); }
        }

        /// <summary>
        /// Pøevodovka
        /// </summary>
        public string Gearbox
        {
            get { return GetValueOf("gearbox"); }
            set { SetValueOf("gearbox", value); }
        }

        /// <summary>
        /// Stav. Obecný stav pøedmìtu informace (u auta napøíklad ojetý, nový...)
        /// </summary>
        public string State
        {
            get { return GetValueOf("state"); }
            set { SetValueOf("state", value); }
        }

        /// <summary>
        /// Objem motoru
        /// </summary>
        public string Cubature
        {
            get { return GetValueOf("cubature"); }
            set { SetValueOf("cubature", value); }
        }

        /// <summary>
        /// Výkon
        /// </summary>
        public string Power
        {
            get { return GetValueOf("power"); }
            set { SetValueOf("power", value); }
        }

        /// <summary>
        /// Výbava, pøíslušenství
        /// </summary>
        public string Accessories
        {
            get { return GetValueOf("accessories"); }
            set { SetValueOf("accessories", value); }
        }

        /// <summary>
        /// Rozšíøená výbava
        /// </summary>
        public string Extensions
        {
            get { return GetValueOf("extensions"); }
            set { SetValueOf("extensions", value); }
        }

        public string Color
        {
            get { return GetValueOf("color"); }
            set { SetValueOf("color", value); }
        }

        public string Material
        {
            get { return GetValueOf("material"); }
            set { SetValueOf("material", value); }
        }

        public string Construction
        {
            get { return GetValueOf("construction"); }
            set { SetValueOf("construction", value); }
        }

        /// <summary>
        /// Vytápìní
        /// </summary>
        public string Heating
        {
            get { return GetValueOf("heating"); }
            set { SetValueOf("heating", value); }
        }

        /// <summary>
        /// Zaøízený? (byt)
        /// </summary>
        public string Furnished
        {
            get { return GetValueOf("furnished"); }
            set { SetValueOf("furnished", value); }
        }

        /// <summary>
        /// Poèet garáží
        /// </summary>
        public string NumberOfGarages
        {
            get { return GetValueOf("numOfGarages"); }
            set { SetValueOf("numOfGarages", value); }
        }

        /// <summary>
        /// Dispozice (3+1, 1+kk..)
        /// </summary>
        public string Disposition
        {
            get { return GetValueOf("disposition"); }
            set { SetValueOf("disposition", value); }
        }

        /// <summary>
        /// Vlastnictví
        /// </summary>
        public string Ownership
        {
            get { return GetValueOf("ownership"); }
            set { SetValueOf("ownership", value); }
        }

        /// <summary>
        /// Celkový poèet podlaží
        /// </summary>
        public string TotalFloors
        {
            get { return GetValueOf("totalFloors"); }
            set { SetValueOf("totalFloors", value); }
        }

        /// <summary>
        /// Podlaží
        /// </summary>
        public string Floor
        {
            get { return GetValueOf("floor"); }
            set { SetValueOf("floor", value); }
        }

        /// <summary>
        /// Balkon
        /// </summary>
        public string Balcony
        {
            get { return GetValueOf("balcony"); }
            set { SetValueOf("balcony", value); }
        }

        /// <summary>
        /// Výtah
        /// </summary>
        public string Elevator
        {
            get { return GetValueOf("elevator"); }
            set { SetValueOf("elevator", value); }
        }

        /// <summary>
        /// Elektøina
        /// </summary>
        public string Electricity
        {
            get { return GetValueOf("electricity"); }
            set { SetValueOf("electricity", value); }
        }

        /// <summary>
        /// Voda
        /// </summary>
        public string Water
        {
            get { return GetValueOf("water"); }
            set { SetValueOf("water", value); }
        }

        /// <summary>
        /// Plyn
        /// </summary>
        public string Gas
        {
            get { return GetValueOf("gas"); }
            set { SetValueOf("gas", value); }
        }

        /// <summary>
        /// Kanalizace
        /// </summary>
        public string Sewerage
        {
            get { return GetValueOf("sewerage"); }
            set { SetValueOf("sewerage", value); }
        }

        /// <summary>
        /// DPH, daò z pøidané hodnoty
        /// </summary>
        public string Vat
        {
            get { return GetValueOf("vat"); }
            set { SetValueOf("vat", value); }
        }

        /// <summary>
        /// Havarováno
        /// </summary>
        public string Crash
        {
            get { return GetValueOf("crash"); }
            set { SetValueOf("crash", value); }
        }

        /// <summary>
        /// Nové
        /// </summary>
        public string New
        {
            get { return GetValueOf("new"); }
            set { SetValueOf("new", value); }
        }

        public string Cellar
        {
            get { return GetValueOf("cellar"); }
            set { SetValueOf("cellar", value); }
        }

        public string FloorArea
        {
            get { return GetValueOf("floorArea"); }
            set { SetValueOf("floorArea", value); }
        }

        public string ParcelArea
        {
            get { return GetValueOf("parcelArea"); }
            set { SetValueOf("parcelArea", value); }
        }

        /// <summary>
        /// Plocha, výmìra
        /// </summary>
        public string Area
        {
            get { return GetValueOf("area"); }
            set { SetValueOf("area", value); }
        }

        public string PlotArea
        {
            get { return GetValueOf("plotArea"); }
            set { SetValueOf("plotArea", value); }
        }

        /// <summary>
        /// Užitná plocha
        /// </summary>
        public string UsableArea
        {
            get { return GetValueOf("usableArea"); }
            set { SetValueOf("usableArea", value); }
        }

        /// <summary>
        /// Zastavìná plocha
        /// </summary>
        public string BuiltUpArea
        {
            get { return GetValueOf("builtUpArea"); }
            set { SetValueOf("builtUpArea", value); }
        }

        public string KindOfLocality
        {
            get { return GetValueOf("kindOfLocality"); }
            set { SetValueOf("kindOfLocality", value); }
        }

        public string KindOfUsage
        {
            get { return GetValueOf("kindOfUsage"); }
            set { SetValueOf("kindOfUsage", value); }
        }

        /// <summary>
        /// Povinné ruèení
        /// </summary>
        public string Liability
        {
            get { return GetValueOf("liability"); }
            set { SetValueOf("liability", value); }
        }

        /// <summary>
        /// Státní technická kontrola, State technical inspection
        /// </summary>
        public string Sti
        {
            get { return GetValueOf("sti"); }
            set { SetValueOf("sti", value); }
        }

        public string NumberOfFloors
        {
            get { return GetValueOf("numOfFloors"); }
            set { SetValueOf("numOfFloors", value); }
        }

        public string NumberOfPersons
        {
            get { return GetValueOf("numOfPersons"); }
            set { SetValueOf("numOfPersons", value); }
        }

        public string Parking
        {
            get { return GetValueOf("parking"); }
            set { SetValueOf("parking", value); }
        }

        public string NoAgencyWarning
        {
            get { return GetValueOf("noAgencyWarning"); }
            set { SetValueOf("noAgencyWarning", value); }
        }

        public string ImpactArea
        {
            get { return GetValueOf("impactArea"); }
            set { SetValueOf("impactArea", value); }
        }

        private void SetValueOf(string elementName, string value, Func<string, string> sanitizer = null)
        {
            if (sanitizer == null) sanitizer = SanitizeValue;
            value = sanitizer(value);
            var element = root.Elements(elementName).SingleOrDefault();
            if (element == null)
            {
                if (string.IsNullOrWhiteSpace(value)) return;
                root.Add(element = new XElement(elementName));
            }
            else
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    element.Remove();
                    return;
                }
            }
            element.Value = value;
        }

        private static string SanitizeValue(string value)
        {
            var sanitized = value.Trim(' ', '\r', '\n', '\t', ',', ':');
            if (sanitized == "neuvedeno" || sanitized == "Spoèítat povinné ruèení" || sanitized == "V textu") return null;
            return sanitized;
        }

        private string GetValueOf(string rootElementName)
        {
            var priceElement = root.Descendants(rootElementName).SingleOrDefault();
            return priceElement != null ? priceElement.Value : null;
        }

        public bool IsEmpty
        {
            get { return root.IsEmpty; }
        }

        public IEnumerable<string> Phones
        {
            get { return root.Elements("phone").Select(e => e.Value); }
        }

        public IEnumerable<string> Mails
        {
            get { return root.Elements("mail").Select(e => e.Value); }
        }

        public IEnumerable<KeyValuePair<string, string>> Items
        {
            get
            {
                foreach (var element in root.Elements())
                {
                    if (element.Name.LocalName == "unknown")
                    {
                        yield return new KeyValuePair<string, string>(element.Attribute("key").Value, element.Value);
                    }
                    else
                    {
                        yield return new KeyValuePair<string, string>(element.Name.LocalName, element.Value);
                    }
                }
            }
        }

        public static StructuredContent Create(IEnumerable<KeyValuePair<string, string>> structured)
        {
            if (structured == null) return null;
            var structuredContent = new StructuredContent();
            foreach (var keyAndValue in structured)
            {
                var setter = GetStructuredValueSetter(keyAndValue.Key);
                if (setter != null) setter(structuredContent, keyAndValue.Value);
            }
            return structuredContent;
        }

        private static Action<StructuredContent, string> GetStructuredValueSetter(string valueKey)
        {
            valueKey = valueKey.Trim(' ', ':', '\r', '\n', '\t').ToLower();
            if (valueKey == "èíslo inzerátu" || valueKey == "poèet zhlédnutí" || valueKey == "zhlédnutí detailu") return null;
            if (valueKey == "inzerent") return (sc, @value) => sc.Author = @value;
            if (valueKey == "adresa") return (sc, @value) => sc.Address = @value;
            if (valueKey.Contains("telefon") || valueKey.Contains("tel.")) return (sc, @value) => sc.AddPhone(@value);
            if (valueKey.Contains("fax")) return (sc, @value) => sc.AddFax(@value);
            if (valueKey.Contains("mail")) return (sc, @value) => sc.AddMail(@value);
            if (valueKey.Contains("odkaz") || valueKey == "web" || valueKey == "www") return (sc, @value) => sc.AddLink(@value);
            if (valueKey == "skype") return (sc, @value) => sc.AddSkype(@value);
            if (valueKey == "icq") return (sc, @value) => sc.AddIcq(@value);
            if (valueKey == "msn") return (sc, @value) => sc.AddMsn(@value);
            if (valueKey == "zemì") return (sc, @value) => sc.Country = @value;
            if (valueKey == "kraj") return (sc, @value) => sc.County = @value;
            if (valueKey == "okres") return (sc, @value) => sc.District = @value;
            if (valueKey == "mìsto") return (sc, @value) => sc.City = @value;
            if (valueKey == "ulice") return (sc, @value) => sc.Street = @value;
            if (valueKey == "povaha inzerátu") return (sc, @value) => sc.KindOfInfo = @value;
            if (valueKey == "transakce") return (sc, @value) => sc.Transaction = @value;
            if (valueKey == "cena") return (sc, @value) => sc.Price = @value;
            if (valueKey == "oblast inzerce") return (sc, @value) => sc.ImpactArea = @value;
            if (valueKey == "druh vozidla" || valueKey == "druh autobusu" || valueKey == "druh nemovitosti" || valueKey == "druh pozemku" || valueKey == "druh objektu" || valueKey == "druh motocyklu" || valueKey == "typ bytu" || valueKey == "typ prostor") return (sc, @value) => sc.Kind = @value;
            if (valueKey == "znaèka" || valueKey == "znaèka, výrobce" || valueKey == "znaèka vozu" || valueKey == "znaèka motocyklu") return (sc, @value) => sc.Brand = @value;
            if (valueKey == "modelová øada") return (sc, @value) => sc.ModelRange = @value;
            if (valueKey == "varianta modelu") return (sc, @value) => sc.Variation = @value;
            if (valueKey == "model") return (sc, @value) => sc.Model = @value;
            if (valueKey == "rok výroby") return (sc, @value) => sc.Manufactured = @value;
            if (valueKey == "rok kolaudace") return (sc, @value) => sc.Accepted = @value;
            if (valueKey == "rok rekonstrukce") return (sc, @value) => sc.Reconstructed = @value;
            if (valueKey == "poèet míst") return (sc, @value) => sc.NumberOfPersons = @value;
            if (valueKey == "najeto" || valueKey == "stav tachometru") return (sc, @value) => sc.Mileage = @value;
            if (valueKey == "karoserie" || valueKey == "druh karoserie") return (sc, @value) => sc.KindOfBody = @value;
            if (valueKey == "palivo") return (sc, @value) => sc.Fuel = @value;
            if (valueKey == "barva" || valueKey == "barva vozu") return (sc, @value) => sc.Color = @value;
            if (valueKey == "pøevodovka") return (sc, @value) => sc.Gearbox = @value;
            if (valueKey == "stav" || valueKey == "stav vozu" || valueKey == "stav vozidla") return (sc, @value) => sc.State = @value;
            if (valueKey == "objem" || valueKey == "obsah motoru" || valueKey == "objem motoru") return (sc, @value) => sc.Cubature = @value;
            if (valueKey == "výkon" || valueKey == "výkon motoru") return (sc, @value) => sc.Power = @value;
            if (valueKey == "výbava" || valueKey == "výbava vozu") return (sc, @value) => sc.Accessories = @value;
            if (valueKey == "rozšíøená výbava") return (sc, @value) => sc.Extensions = @value;
            if (valueKey == "podlahová plocha" || valueKey == "celkova plocha domu") return (sc, @value) => sc.FloorArea = @value;
            if (valueKey == "plocha" || valueKey == "rozloha") return (sc, @value) => sc.Area = @value;
            if (valueKey == "velikost parcely") return (sc, @value) => sc.ParcelArea = @value;
            if (valueKey == "plocha pozemku") return (sc, @value) => sc.PlotArea = @value;
            if (valueKey == "užitná plocha") return (sc, @value) => sc.UsableArea = @value;
            if (valueKey == "zastavìná plocha") return (sc, @value) => sc.BuiltUpArea = @value;
            if (valueKey == "konstrukce domu") return (sc, @value) => sc.Construction = @value;
            if (valueKey == "materiál budovy") return (sc, @value) => sc.Material = @value;
            if (valueKey == "druh umístìní") return (sc, @value) => sc.KindOfLocality = @value;
            if (valueKey == "poèet podlaží") return (sc, @value) => sc.NumberOfFloors = @value;
            if (valueKey == "možnost parkování") return (sc, @value) => sc.Parking = @value;
            if (valueKey == "úèel nemovitosti") return (sc, @value) => sc.KindOfUsage = @value;
            if (valueKey == "povinné ruèení") return (sc, @value) => sc.Liability = @value;
            if (valueKey == "stk") return (sc, @value) => sc.Sti = @value;
            if (valueKey == "vytápìní") return (sc, @value) => sc.Heating = @value;
            if (valueKey == "zaøízený" || valueKey == "zaøizený") return (sc, @value) => sc.Furnished = @value;
            if (valueKey == "poèet garáží") return (sc, @value) => sc.NumberOfGarages = @value;
            if (valueKey == "dispozice") return (sc, @value) => sc.Disposition = @value;
            if (valueKey == "vlastnictví") return (sc, @value) => sc.Ownership = @value;
            if (valueKey == "podlaží") return (sc, @value) => sc.Floor = @value;
            if (valueKey == "poèet podlaží domu") return (sc, @value) => sc.TotalFloors = @value;
            if (valueKey == "balkon") return (sc, @value) => sc.Balcony = @value;
            if (valueKey == "výtah") return (sc, @value) => sc.Elevator = @value;
            if (valueKey == "elektøina") return (sc, @value) => sc.Electricity = @value;
            if (valueKey == "voda") return (sc, @value) => sc.Water = @value;
            if (valueKey == "plyn") return (sc, @value) => sc.Gas = @value;
            if (valueKey == "kanalizace") return (sc, @value) => sc.Sewerage = @value;
            if (valueKey == "odpoèet dph") return (sc, @value) => sc.Vat = @value;
            if (valueKey == "havarováno") return (sc, @value) => sc.Crash = @value;
            if (valueKey == "nové / ojeté") return (sc, @value) => sc.New = @value;
            if (valueKey.StartsWith("sklep")) return (sc, @value) => sc.Cellar = @value;
            if (valueKey == "upozornìní pro realitní kanceláøe") return (sc, @value) => sc.NoAgencyWarning = string.IsNullOrWhiteSpace(@value) ? (string)null : "noAgencyWarning";
            return (sc, value) => sc.AddUnknown(valueKey, value);
        }

        private void AddUnknown(string key, string value)
        {
            if (string.IsNullOrEmpty(key)) return;
            value = SanitizeValue(value);
            if (string.IsNullOrEmpty(value)) return;
            var element = root.Elements("unknown").Where(e => e.Attribute("key").Value == key && e.Value == value).SingleOrDefault();
            if (element != null) return;
            element = new XElement("unknown");
            element.SetAttributeValue("key", key);
            element.Value = value;
            root.Add(element);
        }

        public void AddPhone(string value)
        {
            AddValue("phone", value, SanitizePhone);
        }

        private static string SanitizePhone(string phone)
        {
            var sanitized = Regex.Replace(phone, @"\D", "");
            sanitized = sanitized.TrimStart('0');
            return sanitized;
        }

        public void AddSkype(string value)
        {
            AddValue("skype", value);
        }

        public void AddFax(string value)
        {
            AddValue("fax", value, SanitizePhone);
        }

        public void AddMail(string value)
        {
            AddValue("mail", value);
        }

        public void AddLink(string value)
        {
            AddValue("link", value);
        }

        public void AddIcq(string value)
        {
            AddValue("icq", value);
        }

        
        /// <summary>
        /// Contact on Microsoft network
        /// </summary>
        /// <param name="value"></param>
        public void AddMsn(string value)
        {
            AddValue("msn", value);
        }

        private void AddValue(string elementName, string value, Func<string, string> sanitizer = null)
        {
            if (sanitizer == null) sanitizer = SanitizeValue;
            value = sanitizer(value);
            if (string.IsNullOrWhiteSpace(value)) return;
            // Only unique values are allowed
            var element = root.Elements(elementName).Where(e => e.Value == value).SingleOrDefault();
            if (element != null) return;
            element = new XElement(elementName);
            root.Add(element);
            element.Value = value;
        }

        public string ToXml()
        {
            if (IsEmpty) return null;
            var xml = root.ToString(SaveOptions.OmitDuplicateNamespaces | SaveOptions.DisableFormatting);
            return xml;
        }
    }
}