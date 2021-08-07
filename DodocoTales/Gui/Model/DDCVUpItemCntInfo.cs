using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DodocoTales.Gui.Model
{
    public class DDCVUpItemCntInfo : DDCVModel
    {
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                CheckPropertyChanged("Name");
            }
        }
        private string name;

        public string Cnt
        {
            get
            {
                return cnt;
            }
            set
            {
                cnt = value;
                CheckPropertyChanged("Cnt");
            }
        }
        private string cnt;
    }
}
