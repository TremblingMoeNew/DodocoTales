using DodocoTales.Common.Enums;
using DodocoTales.Library;
using DodocoTales.Library.Models;
using DodocoTales.Loader.Compatibility.KeqingNiuza.Model;
using DodocoTales.Loader.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DodocoTales.Loader.Compatibility.KeqingNiuza
{
    public class DDCIKeqingNiuzaLogImporter
    {

        public DDCGGachaLogImportResult ConvertToDDCLogs(string logs)
        {
            var wishdata = DeserializeWishData(logs);
            if (wishdata == null) return null;
            if (!wishdata.Any()) return null;
            var result = new DDCGGachaLogImportResult
            {
                Uid = wishdata.First().uid,
                Beginner = new List<DDCGGachaLogImportedItem>(),
                Permanent = new List<DDCGGachaLogImportedItem>(),
                EventCharacter = new List<DDCGGachaLogImportedItem>(),
                EventWeapon = new List<DDCGGachaLogImportedItem>()
            };
            ConvertPool(wishdata.FindAll(x => x.gacha_type == DDCCPoolType.Beginner), result.Beginner);
            ConvertPool(wishdata.FindAll(x => x.gacha_type == DDCCPoolType.Permanent), result.Permanent);
            ConvertPool(wishdata.FindAll(x => x.gacha_type == DDCCPoolType.EventCharacter), result.EventCharacter);
            ConvertPool(wishdata.FindAll(x => x.gacha_type == DDCCPoolType.EventWeapon), result.EventWeapon);
            return result;
        }


        public List<DDCIKeqingNiuzaWishData>DeserializeWishData(string logs)
        {
            try
            {
                return JsonConvert.DeserializeObject<List<DDCIKeqingNiuzaWishData>>(logs);
            }
            catch (Exception)
            {
                return null;
            }
        }
        public void ConvertPool(List<DDCIKeqingNiuzaWishData> wishdata,List<DDCGGachaLogImportedItem> result)
        {
            foreach(var item in wishdata)
            {
                result.Add(ConvertToDDCItem(item));
            }
        }
        public DDCGGachaLogImportedItem ConvertToDDCItem(DDCIKeqingNiuzaWishData item)
        {
            DDCLUnitInfo unitclass = null;
            if (item.lang == "zh-cn") unitclass = DDCL.Units.getItemByName(item.name); else unitclass = DDCL.Units.getItemByCode(item.name);

            if (unitclass == DDCL.Units.Unknown)
            {
                DDCCUnitType unittype = DDCCUnitType.Unknown;
                if (item.lang == "zh-cn")
                {
                    if (item.item_type == "角色") unittype = DDCCUnitType.Character; else if (item.item_type == "武器") unittype = DDCCUnitType.Weapon;
                }
                else
                {
                    unittype = (DDCCUnitType)Enum.Parse(typeof(DDCCUnitType), item.item_type);
                }
               
                return new DDCGGachaLogImportedItem()
                {
                    id = item.id,
                    idLost = item.IsLostId,
                    code = item.name,
                    rank = item.rank_type,
                    time = item.time,
                    unittype = unittype,
                    unitclass = unitclass.id,
                    name = unitclass.name,
                    ItemUnitClassNotFound = true
                };
            }
            else
            {
                return new DDCGGachaLogImportedItem()
                {
                    id = item.id,
                    idLost = item.IsLostId,
                    code = unitclass.code,
                    rank = unitclass.rank,
                    time = item.time,
                    unittype = unitclass.type,
                    unitclass = unitclass.id,
                    name = unitclass.name
                };
            }
        }
    }
}
