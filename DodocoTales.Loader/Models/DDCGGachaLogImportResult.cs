using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DodocoTales.Loader.Models
{
    public class DDCGGachaLogImportResult
    {
        public long Uid { get; set; }
        public List<DDCGGachaLogImportedItem> Beginner { get; set; }
        public List<DDCGGachaLogImportedItem> Permanent { get; set; }
        public List<DDCGGachaLogImportedItem> EventCharacter { get; set; }
        public List<DDCGGachaLogImportedItem> EventWeapon { get; set; }

        public bool BeginnerOriginalEmptyAndLastIdLost { get; set; }
        public bool PermanentOriginalEmptyAndLastIdLost { get; set; }
        public bool EvnetCharOriginalEmptyAndLastIdLost { get; set; }
        public bool EventWeapOriginalEmptyAndLastIdLost { get; set; }
        public bool UserLogNotExist { get; set; }
    }
}
