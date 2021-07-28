using DodocoTales.Common;
using DodocoTales.Common.Enums;
using DodocoTales.Gui.Model;
using DodocoTales.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DodocoTales.Gui.View.Card
{
    /// <summary>
    /// DDCVHomeScreenEventCharLogCard.xaml 的交互逻辑
    /// </summary>
    public partial class DDCVHomeScreenEventCharLogCard : UserControl
    {

        public static readonly DependencyProperty SummaryProperty = DependencyProperty.Register("Summary", typeof(DDCVHomeScnECLogSummary), typeof(DDCVHomeScreenEventCharLogCard));
        public DDCVHomeScnECLogSummary Summary
        {
            set { SetValue(SummaryProperty, value); }
            get { return (DDCVHomeScnECLogSummary)GetValue(SummaryProperty); }
        }

        public DDCVHomeScreenEventCharLogCard()
        {
            InitializeComponent();
            DDCS.LogReloadCompleted += new DDCSCommonDelegate(LoadSummaryData);
        }
        public void LoadSummaryData()
        {
            var curlog = DDCL.Users.getCurrentUser();

            int cnt = 0;
            int r5cnt = 0, r4cnt = 0, upr5cnt = 0, upr4cnt = 0;
            int curr5cnt = 0;
            List<DDCVUnitTextIndicatorInfo> info = new List<DDCVUnitTextIndicatorInfo>();

            foreach (var v in curlog.V)
            {
                foreach (var b in v.B.FindAll(x => x.poolType == DDCCPoolType.EventCharacter))
                {
                    var lvs = DDCL.Banners.eventPools.FindAll(x => x.id == v.id);
                    if (lvs.Count != 1) 
                    {
                        // TODO: 报错
                        continue;
                    }
                    var lv = lvs[0];
                    var lbs = lv.banners.FindAll(x => x.id == b.id);
                    if (lbs.Count != 1) 
                    {
                        // TODO: 报错
                        continue;
                    }
                    var lb = lbs[0];
                    foreach (var r in b.R)
                    {
                        cnt += r.L.Count;
                        curr5cnt += r.L.Count;
                        
                        if (r.L.Count > 0 && r.L[r.L.Count - 1].rank == 5)
                        {
                            r5cnt++;
                            var unit = r.L[r.L.Count - 1];
                            if (lb.rank5Up.Contains(unit.unitclass))
                            {
                                upr5cnt++;
                            }
                            info.Add(new DDCVUnitTextIndicatorInfo
                            {
                                name = unit.name,
                                bannerid = b.id,
                                versionid = v.id,
                                id = unit.id,
                                time = unit.time,
                                unitclass = unit.unitclass,
                                cnt = curr5cnt
                            });
                            curr5cnt = 0;
                            
                        }
                        foreach (var unit in r.L.FindAll(x => x.rank == 4))
                        {
                            r4cnt++;
                            if (lb.rank4Up.Contains(unit.unitclass))
                            {
                                upr4cnt++;
                            }
                        }
                    }
                }
            }
            info.Reverse();
            var summary = new DDCVHomeScnECLogSummary
            {
                TotalCnt = cnt.ToString(),
                R5Cnt = r5cnt.ToString(),
                R4Cnt = r4cnt.ToString(),
                R5Avg = r5cnt > 0 ? string.Format("{0:F1}", (cnt - curr5cnt) * 1.0 / (r5cnt)) : "——",
                R5PS = cnt > 0 ? string.Format("[{0:P1}]", r5cnt * 1.0 / cnt) : "[—%]",
                R4PS = cnt > 0 ? string.Format("[{0:P1}]", r4cnt * 1.0 / cnt) : "[—%]",
                R5Up = upr5cnt.ToString(),
                R4Up = upr4cnt.ToString(),
                IndicatorInfo = info
            };
            Action act = () => { Summary = summary; RebuildIndicator(); };
            Dispatcher.BeginInvoke(act);
        }
        public void RebuildIndicator()
        {
            // 临时方案
            IndicatorContainer.Children.Clear();
            foreach (var info in Summary.IndicatorInfo)
            {
                IndicatorContainer.Children.Add(new TextBlock
                {
                    Text = string.Format("{0}[{1}]", info.name, info.cnt)
                });
            }
        }
    }
}
