using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DodocoTales.Gui.Model
{
    public class DDCVVerScnVerLogSummary : DDCVModel
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

        public string VersionTime
        {
            get
            {
                return versionTime;
            }
            set
            {
                versionTime = value;
                CheckPropertyChanged("VersionTime");
            }
        }
        private string versionTime;

        public string BannerCnt
        {
            get
            {
                return bannerCnt;
            }
            set
            {
                bannerCnt = value;
                CheckPropertyChanged("BannerCnt");
            }
        }
        private string bannerCnt;

        public string PermanentCnt
        {
            get
            {
                return permanentCnt;
            }
            set
            {
                permanentCnt = value;
                CheckPropertyChanged("PermanentCnt");
            }
        }
        private string permanentCnt;

        public string EventCharCnt
        {
            get
            {
                return eventCharCnt;
            }
            set
            {
                eventCharCnt = value;
                CheckPropertyChanged("EventCharCnt");
            }
        }
        private string eventCharCnt;

        public string EventWeapCnt
        {
            get
            {
                return eventWeapCnt;
            }
            set
            {
                eventWeapCnt = value;
                CheckPropertyChanged("EventWeapCnt");
            }
        }
        private string eventWeapCnt;

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
        public List<DDCVUnitTextIndicatorInfo> IndicatorInfo { get; set; }
    }
}
