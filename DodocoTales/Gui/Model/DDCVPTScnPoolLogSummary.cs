using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DodocoTales.Gui.Model
{
    public class DDCVPTScnPoolLogSummary : DDCVModel
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

        public string R5CWR
        {
            get
            {
                return r5CWR;
            }
            set
            {
                r5CWR = value;
                CheckPropertyChanged("R5CWR");
            }
        }
        private string r5CWR;

        public string R4CWR
        {
            get
            {
                return r4CWR;
            }
            set
            {
                r4CWR = value;
                CheckPropertyChanged("R4CWR");
            }
        }
        private string r4CWR;
    }
}
