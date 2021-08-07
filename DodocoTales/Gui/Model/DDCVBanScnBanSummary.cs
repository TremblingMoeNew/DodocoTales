using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DodocoTales.Gui.Model
{
    public class DDCVBanScnBanSummary : DDCVModel
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

        public string Subtitle
        {
            get
            {
                return subtitle;
            }
            set
            {
                subtitle = value;
                CheckPropertyChanged("Subtitle");
            }
        }
        private string subtitle;

        public string BannerTime
        {
            get
            {
                return bannerTime;
            }
            set
            {
                bannerTime = value;
                CheckPropertyChanged("BannerTime");
            }
        }
        private string bannerTime;

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

        public string RoundCnt
        {
            get
            {
                return roundCnt;
            }
            set
            {
                roundCnt = value;
                CheckPropertyChanged("RoundCnt");
            }
        }
        private string roundCnt;

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


        public string R5Cnt
        {
            get
            {
                return r5Cnt;
            }
            set
            {
                r5Cnt = value;
                CheckPropertyChanged("R5Cnt");
            }
        }
        private string r5Cnt;

        public string R4Cnt
        {
            get
            {
                return r4Cnt;
            }
            set
            {
                r4Cnt = value;
                CheckPropertyChanged("R4Cnt");
            }
        }
        private string r4Cnt;

        public List<DDCVUpItemCntInfo> R5Items
        {
            get
            {
                return r5Items;
            }
            set
            {
                r5Items = value;
                CheckPropertyChanged("R5Items");
            }
        }
        private List<DDCVUpItemCntInfo> r5Items;

        public List<DDCVUpItemCntInfo> R4Items
        {
            get
            {
                return r4Items;
            }
            set
            {
                r4Items = value;
                CheckPropertyChanged("R4Items");
            }
        }
        private List<DDCVUpItemCntInfo> r4Items;
    }
}
