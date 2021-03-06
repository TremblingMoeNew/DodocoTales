using DodocoTales.Common.Enums;
using DodocoTales.Common.Models;
using DodocoTales.Library;
using DodocoTales.Library.Models;
using DodocoTales.Loader.Compatibility.GenWishExport.Models;
using DodocoTales.Loader.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DodocoTales.Loader.Compatibility.GenWishExport
{
    public class DDCIGenWishExpLogImporter
    {

        public DDCGGachaLogImportResult ConvertToDDCLogs(string logs)
        {
            try
            {
                var main = JsonConvert.DeserializeObject<DDCIGenWishExpLogMain>(logs);
                bool isSimplifiedChinese = false;
                if (main.lang == "zh-cn") isSimplifiedChinese = true; else if (main.lang == "en-us") isSimplifiedChinese = false; else return null;
                var result = ConvertResultPropertyToLists(main.result, isSimplifiedChinese);
                result.Uid = Convert.ToInt64(main.uid);
                return result;
            }
            catch(Exception)
            {
                return null;
            }
        }



        public DDCGGachaLogImportResult ConvertResultPropertyToLists(List<JArray> list, bool isSimplifiedChinese)
        {
            var result = new DDCGGachaLogImportResult
            {
                Beginner = new List<DDCGGachaLogImportedItem>(),
                Permanent = new List<DDCGGachaLogImportedItem>(),
                EventCharacter = new List<DDCGGachaLogImportedItem>(),
                EventWeapon = new List<DDCGGachaLogImportedItem>()
            };
            foreach(var poolitem in list)
            {
                if (poolitem.Count < 2)
                {
                    continue;
                }
                if (poolitem[0].Type != JTokenType.String) continue;
                List<DDCGGachaLogImportedItem> ptr = null;
                var pooltype = poolitem[0].ToString();
                switch(pooltype)
                {
                    case "100":
                        ptr = result.Beginner;
                        break;
                    case "200":
                        ptr = result.Permanent;
                        break;
                    case "301":
                        ptr = result.EventCharacter;
                        break;
                    case "302":
                        ptr = result.EventWeapon;
                        break;
                }
                if (ptr == null) continue;
                if (poolitem[1].Type != JTokenType.Array) continue;
                ConvertPool(ptr, poolitem[1] as JArray, isSimplifiedChinese);
            }
            return result;
        }



        public void ConvertPool(List<DDCGGachaLogImportedItem> list, JArray items, bool isSimplifiedChinese)
        {
            foreach(var item in items)
            {
                if (item.Type == JTokenType.Array) 
                {
                    var res = ConvertToDDCItem(item as JArray, isSimplifiedChinese);
                    if (res != null) 
                    {
                        list.Add(res);
                    }
                }
            }
        }

        public DDCGGachaLogImportedItem ConvertToDDCItem(JArray item, bool isSimplifiedChinese)
        {
            if (item.Count < 4) return null;
            DDCGGachaLogImportedItem ddccitem = null;
            try
            {
                DateTime time = item[0].ToObject<DateTime>();
                string name = item[1].ToString();
                
                DDCLUnitInfo unitclass = null;
                if (isSimplifiedChinese) unitclass = DDCL.Units.getItemByName(name); else unitclass = DDCL.Units.getItemByCode(name);
                if (unitclass == DDCL.Units.Unknown)
                {
                    DDCCUnitType unittype = DDCCUnitType.Unknown;
                    if(isSimplifiedChinese)
                    {
                        string type = item[2].ToString();
                        if (type == "角色") unittype = DDCCUnitType.Character; else if (type == "武器") unittype = DDCCUnitType.Weapon;
                    }
                    else
                    {
                        unittype = item[2].ToObject<DDCCUnitType>();
                    }
                    int rank = Convert.ToInt32(item[3].ToString());
                    ddccitem = new DDCGGachaLogImportedItem()
                    {
                        id = 0,
                        idLost = true,
                        code = name,
                        rank = rank,
                        time = time,
                        unittype = unittype,
                        unitclass = unitclass.id,
                        name = unitclass.name,
                        ItemUnitClassNotFound = true
                    };
                }
                else
                {
                    ddccitem = new DDCGGachaLogImportedItem()
                    {
                        id = 0,
                        idLost = true,
                        code = unitclass.code,
                        rank = unitclass.rank,
                        time = time,
                        unittype = unitclass.type,
                        unitclass = unitclass.id,
                        name = unitclass.name
                    };
                }
            }
            catch(Exception)
            {
                return null;
            }
            return ddccitem;
        }
    }
}
