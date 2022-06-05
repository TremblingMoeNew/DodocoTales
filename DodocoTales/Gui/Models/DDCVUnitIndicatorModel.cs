using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DodocoTales.Gui.Models
{
    public class DDCVUnitIndicatorModel : ObservableObject
    {
        private string name;
        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }

        private int count;
        public int Count
        {
            get => count;
            set => SetProperty(ref count, value);
        }

        private DateTimeOffset time;
        public DateTimeOffset Time
        {
            get => time;
            set => SetProperty(ref time, value);
        }

        private string version;
        public string Version
        {
            get => version;
            set => SetProperty(ref version, value);
        }
        private string banner;
        public string Banner
        {
            get => banner;
            set => SetProperty(ref banner, value);
        }
    }
}
