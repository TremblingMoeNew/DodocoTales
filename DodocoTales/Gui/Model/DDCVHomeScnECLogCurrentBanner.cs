using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DodocoTales.Gui.Model
{
    public class DDCVHomeScnECLogCurrentBanner : DDCVModel
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

        public string BannerUpUnits
        {
            get
            {
                return bannerUpUnits;
            }
            set
            {
                bannerUpUnits = value;
                CheckPropertyChanged("BannerUpUnits");
            }
        }
        private string bannerUpUnits;

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

        public string R5PS
        {
            get
            {
                return r5PS;
            }
            set
            {
                r5PS = value;
                CheckPropertyChanged("R5PS");
            }
        }
        private string r5PS;

        public string R4PS
        {
            get
            {
                return r4PS;
            }
            set
            {
                r4PS = value;
                CheckPropertyChanged("R4PS");
            }
        }
        private string r4PS;

        public string R5Up
        {
            get
            {
                return r5Up;
            }
            set
            {
                r5Up = value;
                CheckPropertyChanged("R5Up");
            }
        }
        private string r5Up;

        public string R4Up
        {
            get
            {
                return r4Up;
            }
            set
            {
                r4Up = value;
                CheckPropertyChanged("R4Up");
            }
        }
        private string r4Up;


        public string R5NextPS
        {
            get
            {
                return r5NextPS;
            }
            set
            {
                r5NextPS = value;
                CheckPropertyChanged("R5NextPS");
            }
        }
        private string r5NextPS;

        public string R4NextPS
        {
            get
            {
                return r4NextPS;
            }
            set
            {
                r4NextPS = value;
                CheckPropertyChanged("R4NextPS");
            }
        }
        private string r4NextPS;

    }
}
