using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DodocoTales.Gui.Model
{
    public class DDCVHomeScnECLogSummary : DDCVModel
    {
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

        public string R5Avg
        {
            get
            {
                return r5Avg;
            }
            set
            {
                r5Avg = value;
                CheckPropertyChanged("R5Avg");
            }
        }
        private string r5Avg;

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

        public List<DDCVUnitTextIndicatorInfo> IndicatorInfo { get; set; }
    }
}
