using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DodocoTales.Gui.Model
{
    public class DDCVBanScnRoundCardSummary : DDCVModel
    {
        public string Title
        {
            get
            {
                return title;
            }
            set
            {
                title = value;
                CheckPropertyChanged("Title");
            }
        }
        private string title;

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

        public string InheritedCnt
        {
            get
            {
                return inheritedCnt;
            }
            set
            {
                inheritedCnt = value;
                CheckPropertyChanged("InheritedCnt");
            }
        }
        private string inheritedCnt;

        public string TotalCnt
        {
            get
            {
                return totalCnt;
            }
            set
            {
                totalCnt = value;
                CheckPropertyChanged("TotalCnt");
            }
        }
        private string totalCnt;
    }
}
