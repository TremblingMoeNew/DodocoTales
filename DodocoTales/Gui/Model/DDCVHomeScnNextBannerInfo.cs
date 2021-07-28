using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DodocoTales.Gui.Model
{
    public class DDCVHomeScnNextBannerInfo : DDCVModel
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
    }
}
