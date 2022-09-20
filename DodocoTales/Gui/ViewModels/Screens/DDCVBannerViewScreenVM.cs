using CommunityToolkit.Mvvm.ComponentModel;
using DodocoTales.Common.Enums;
using DodocoTales.Gui.Enums;
using DodocoTales.Gui.Models;
using DodocoTales.Library;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DodocoTales.Gui.ViewModels.Screens
{
    public class DDCVBannerViewScreenVM : ObservableObject
    {
        public ulong VersionID { get; set; }
        public ulong BannerID { get; set; }
        public void SetBanner(ulong versionid, ulong bannerid)
        {
            VersionID = versionid;
            BannerID = bannerid;
        }

        #region Title

        private string bannerName;
        public string BannerName
        {
            get => bannerName;
            set => SetProperty(ref bannerName, value);
        }

        private string version;
        public string Version
        {
            get => version;
            set => SetProperty(ref version, value);
        }

        private string versionName;
        public string VersionName
        {
            get => versionName;
            set => SetProperty(ref versionName, value);
        }

        private DDCCPoolType poolType;
        public DDCCPoolType PoolType
        {
            get => poolType;
            set
            {
                SetProperty(ref poolType, value);
                IsEventPool = (PoolType == DDCCPoolType.EventCharacter || PoolType == DDCCPoolType.EventWeapon);
            }
        }

        private bool isEventPool;
        public bool IsEventPool { get => isEventPool; set => SetProperty(ref isEventPool, value); }

        private DateTimeOffset beginTime;
        public DateTimeOffset BeginTime
        {
            get => beginTime;
            set => SetProperty(ref beginTime, value);
        }
        private DateTimeOffset endTime;
        public DateTimeOffset EndTime
        {
            get => endTime;
            set => SetProperty(ref endTime, value);
        }


        #endregion

        #region General
        private int total;
        public int Total { get => total; set => SetProperty(ref total, value); }

        private int totalIncludesInherited;
        public int TotalIncludesInherited { get => totalIncludesInherited; set => SetProperty(ref totalIncludesInherited, value); }

        private int rank5;
        public int Rank5 { get => rank5; set => SetProperty(ref rank5, value); }

        private int rank5IncludesInherited;
        public int Rank5IncludesInherited { get => rank5IncludesInherited; set => SetProperty(ref rank5IncludesInherited, value); }

        private int rank4;
        public int Rank4 { get => rank4; set => SetProperty(ref rank4, value); }

        private int rank5Up;
        public int Rank5Up { get => rank5Up; set => SetProperty(ref rank5Up, value); }

        private int rank4Up;
        public int Rank4Up { get => rank4Up; set => SetProperty(ref rank4Up, value); }


        private ObservableCollection<DDCVUnitIndicatorModel> rank5Ups;
        public ObservableCollection<DDCVUnitIndicatorModel> Rank5Ups
        {
            get => rank5Ups;
            set => SetProperty(ref rank5Ups, value);
        }
        private ObservableCollection<DDCVUnitIndicatorModel> rank4Ups;
        public ObservableCollection<DDCVUnitIndicatorModel> Rank4Ups
        {
            get => rank4Ups;
            set => SetProperty(ref rank4Ups, value);
        }


        #endregion

        #region Dashboard
        private List<PieSeries<ObservableValue>> seriesGlobalR5;
        public List<PieSeries<ObservableValue>> SeriesGlobalR5
        {
            get => seriesGlobalR5;
            set => SetProperty(ref seriesGlobalR5, value);
        }
        public ObservableValue DBVGlobalR5 = new ObservableValue(0.5);
        public ObservableValue DBVGlobalR5Up = new ObservableValue(0.5);

        private List<PieSeries<ObservableValue>> seriesGlobalR4;
        public List<PieSeries<ObservableValue>> SeriesGlobalR4
        {
            get => seriesGlobalR4;
            set => SetProperty(ref seriesGlobalR4, value);
        }
        public ObservableValue DBVGlobalR4 = new ObservableValue(0.5);
        public ObservableValue DBVGlobalR4Up = new ObservableValue(0.5);


        public void SetDBVRate(ObservableValue ov, int divident, int divisor, double lowerlimit, double upperlimit)
        {
            if (divisor == 0)
            {
                ov.Value = 0;
                return;
            }
            var rate = divident * 100.0 / divisor;
            if (rate < lowerlimit)
            {
                ov.Value = 0;
            }
            else if (rate > upperlimit)
            {
                ov.Value = 1;
            }
            else
            {
                ov.Value = (rate - lowerlimit) / (upperlimit - lowerlimit);
            }
        }
        public void InitializeDashboard()
        {
            SeriesGlobalR5 = new GaugeBuilder()
                .WithInnerRadius(36)
                .WithBackgroundInnerRadius(36)
                .AddValue(DBVGlobalR5Up, "", SKColor.Parse("#D0BA6FD9"))
                .AddValue(DBVGlobalR5, "", SKColor.Parse("#048CFF"))
                .WithLabelsSize(0)
                .BuildSeries();
            foreach (var ser in SeriesGlobalR5)
            {
                ser.IsHoverable = false;
            }
            SeriesGlobalR4 = new GaugeBuilder()
                .WithInnerRadius(36)
                .WithBackgroundInnerRadius(36)
                .AddValue(DBVGlobalR4Up, "", SKColor.Parse("#D0BA6FD9"))
                .AddValue(DBVGlobalR4, "", SKColor.Parse("#048CFF"))
                .WithLabelsSize(0)
                .BuildSeries();
            foreach (var ser in SeriesGlobalR4)
            {
                ser.IsHoverable = false;
            }
        }

        public void RefreshDashboard()
        {
            switch (PoolType)
            {
                case DDCCPoolType.EventCharacter:
                    SetDBVRate(DBVGlobalR5, Rank5IncludesInherited, TotalIncludesInherited, 1.1, 2.1);
                    SetDBVRate(DBVGlobalR5Up, Rank5Up, TotalIncludesInherited, 0.4, 1.6);
                    SetDBVRate(DBVGlobalR4, Rank4, Total, 8, 18);
                    SetDBVRate(DBVGlobalR4Up, Rank4Up, Total, 3.6, 13.6);
                    break;
                case DDCCPoolType.EventWeapon:
                    SetDBVRate(DBVGlobalR5, Rank5IncludesInherited, TotalIncludesInherited, 1.25, 2.5);
                    SetDBVRate(DBVGlobalR5Up, Rank5Up, TotalIncludesInherited, 0.6, 2.4);
                    SetDBVRate(DBVGlobalR4, Rank4, Total, 9, 20);
                    SetDBVRate(DBVGlobalR4Up, Rank4Up, Total, 5, 18.8);
                    break;
                case DDCCPoolType.Permanent:
                default:
                    SetDBVRate(DBVGlobalR5, Rank5IncludesInherited, TotalIncludesInherited, 1.1, 2.1);
                    SetDBVRate(DBVGlobalR5Up, 0, TotalIncludesInherited, 1.1, 2.1);
                    SetDBVRate(DBVGlobalR4, Rank4, Total, 8, 18);
                    SetDBVRate(DBVGlobalR4Up, 0, Total, 8, 18);
                    break;
            }
            
        }

                    

    #endregion

        #region RoundList

        private ObservableCollection<DDCVRoundItemModel> rounds;
        public ObservableCollection<DDCVRoundItemModel> Rounds
        {
            get => rounds;
            set
            {
                SetProperty(ref rounds, value);
                RoundCnt = Rounds.Count;
            }
        }
        private int roundCnt;
        public int RoundCnt { get => roundCnt; set => SetProperty(ref roundCnt, value); }

        #endregion

        #region SelectedRound
        private DDCVRoundItemModel selectedRound;
        public DDCVRoundItemModel SelectedRound
        {
            get => selectedRound;
            set
            {
                if (value != selectedRound)
                {
                    SetProperty(ref selectedRound, value);
                    UpdateSelectedRound();
                }
                
            }
        }

        private ObservableCollection<DDCVUnitIndicatorModel> rank5InRound;
        public ObservableCollection<DDCVUnitIndicatorModel> Rank5InRound
        {
            get => rank5InRound;
            set => SetProperty(ref rank5InRound, value);
        }
        private ObservableCollection<DDCVUnitIndicatorModel> rank4InRound;
        public ObservableCollection<DDCVUnitIndicatorModel> Rank4InRound
        {
            get => rank4InRound;
            set => SetProperty(ref rank4InRound, value);
        }


        public void UpdateSelectedRound()
        {
            if(SelectedRound == null || SelectedRound == DependencyProperty.UnsetValue) return;
            var tz = DDCL.CurrentUser.GetActivatingTimeZone();

            var r5s = new List<DDCVUnitIndicatorModel>();
            foreach(var rnd in SelectedRound.LogItem.Logs.GroupBy(x => x.round_id))
            {
                var item = rnd.LastOrDefault();
                if (item == null) continue;
                if (item.rank != 5) continue;
                var ver = DDCL.BannerLib.GetVersion(item.version_id);
                var ban = DDCL.BannerLib.GetBanner(item.version_id, item.banner_id);
                r5s.Add(new DDCVUnitIndicatorModel
                {
                    Name = item.name,
                    Count = rnd.Count(),
                    Version = ver.version,
                    Banner = ban.name,
                    Time = DDCL.GetTimeOffset(item.time, tz)
                });
            }
            Rank5InRound = new ObservableCollection<DDCVUnitIndicatorModel>(r5s);

            var r4s = new List<DDCVUnitIndicatorModel>();
            foreach (var item in SelectedRound.LogItem.Logs.FindAll(x => x.rank == 4)) 
            {
                var ver = DDCL.BannerLib.GetVersion(item.version_id);
                var ban = DDCL.BannerLib.GetBanner(item.version_id, item.banner_id);
                r4s.Add(new DDCVUnitIndicatorModel
                {
                    Name = item.name,
                    Version = ver.version,
                    Banner = ban.name,
                    Time = DDCL.GetTimeOffset(item.time, tz)
                });
            }
            Rank4InRound = new ObservableCollection<DDCVUnitIndicatorModel>(r4s);

            foreach(var item in GridIndicatorModels)
            {
                item.IndicatorType = DDCVUnitIndicatorType.Default;
                item.Inherited = false;
            }

            switch(SelectedRound.LogItem.CategorizedGachaType)
            {
                case DDCCPoolType.Beginner:
                case DDCCPoolType.Permanent:
                    GreaterRoundType = DDCVGreaterRoundType.Permanent;
                    break;
                case DDCCPoolType.EventCharacter:
                case DDCCPoolType.EventCharacter2:
                default:
                    GreaterRoundType = DDCVGreaterRoundType.EventCharacter;
                    break;
                case DDCCPoolType.EventWeapon:
                    if (SelectedRound.LogItem.IsEPExtendedRound)
                        GreaterRoundType = DDCVGreaterRoundType.EventWeaponEPExtended;
                    else if (SelectedRound.LogItem.EpitomizedPathID != 0)
                        GreaterRoundType = DDCVGreaterRoundType.EventWeaponEP;
                    else
                        GreaterRoundType = DDCVGreaterRoundType.EventWeapon;
                    break;
            }

            for (int i = 0; i < SelectedRound.LogItem.Logs.Count; i++)
            {
                var item = SelectedRound.LogItem.Logs[i];
                var model = GridIndicatorModels[i];

                var ver = DDCL.BannerLib.GetVersion(item.version_id);
                var ban = DDCL.BannerLib.GetBanner(item.version_id, item.banner_id);

                model.Name = item.name;
                model.Version = ver.version;
                model.Banner = ban.name;
                model.Time = DDCL.GetTimeOffset(item.time, tz);
                model.Inherited = !(item.version_id == VersionID && item.banner_id == BannerID);
                if (item.rank == 3)
                {
                    model.IndicatorType = DDCVUnitIndicatorType.Rank3;
                }
                else if (item.rank == 4)
                {
                    if (ban.rank4Up.Contains(item.unitclass)) 
                        model.IndicatorType = DDCVUnitIndicatorType.Rank4Up;
                    else if (item.unittype == DDCCUnitType.Character) 
                        model.IndicatorType = DDCVUnitIndicatorType.Rank4PermanentChar;
                    else 
                        model.IndicatorType = DDCVUnitIndicatorType.Rank4PermanentWeap;
                }
                else
                {
                    if (ban.rank5Up.Contains(item.unitclass))
                    {
                        if (SelectedRound.LogItem.EpitomizedPathID == item.unitclass) 
                            model.IndicatorType = DDCVUnitIndicatorType.Rank5Epitomized;
                        else 
                            model.IndicatorType = DDCVUnitIndicatorType.Rank5Up;
                    }
                    else if (item.unittype == DDCCUnitType.Character)
                        model.IndicatorType = DDCVUnitIndicatorType.Rank5PermanentChar;
                    else
                        model.IndicatorType = DDCVUnitIndicatorType.Rank5PermanentWeap;
                }
            }
        }

        #endregion

        #region SelectedRound_GridIndicator
        private ObservableCollection<ObservableCollection<DDCVUnitIndicatorModel>> gridIndicatorItems;
        public ObservableCollection<ObservableCollection<DDCVUnitIndicatorModel>> GridIndicatorItems
        {
            get => gridIndicatorItems;
            set => SetProperty(ref gridIndicatorItems, value);
        }

        private List<DDCVUnitIndicatorModel> gridIndicatorModels;
        public List<DDCVUnitIndicatorModel> GridIndicatorModels
        {
            get => gridIndicatorModels;
            set => SetProperty(ref gridIndicatorModels, value);
        }

        private DDCVGreaterRoundType greaterRoundType;
        public DDCVGreaterRoundType GreaterRoundType
        {
            get => greaterRoundType;
            set
            {
                if(value != greaterRoundType)
                {
                    SetProperty(ref greaterRoundType, value);
                    ResizeGridIndicator();
                }
            }
        }

        public void ResizeGridIndicator()
        {
            int row = 0, col = 0;
            switch(GreaterRoundType)
            {
                case DDCVGreaterRoundType.Permanent:
                    row = 3;
                    col = 30;
                    break;
                case DDCVGreaterRoundType.EventCharacter:
                    row = 5;
                    col = 36;
                    break;
                case DDCVGreaterRoundType.EventWeapon:
                    row = 5;
                    col = 32;
                    break;
                case DDCVGreaterRoundType.EventWeaponEP:
                    row = 6;
                    col = 40;
                    break;
                case DDCVGreaterRoundType.EventWeaponEPExtended:
                    row = 8;
                    col = 40;
                    break;
            }
            GridIndicatorItems.Clear();
            for (int i = 0; i < col; i++)
            {
                var ls = new ObservableCollection<DDCVUnitIndicatorModel>();
                for (int j = 0; j < row; j++)
                {
                    ls.Add(GridIndicatorModels[i * row + j]);
                }
                GridIndicatorItems.Add(ls);
            }
        }
        public void InitializeGridIndicator()
        {
            GridIndicatorItems = new ObservableCollection<ObservableCollection<DDCVUnitIndicatorModel>>();
            GridIndicatorModels = new List<DDCVUnitIndicatorModel>();
            for (int i = 0; i < 320; i++) 
                GridIndicatorModels.Add(new DDCVUnitIndicatorModel { Index = i + 1 });
            GreaterRoundType = DDCVGreaterRoundType.Permanent;
        }

        #endregion



        public DDCVBannerViewScreenVM()
        {
            InitializeDashboard();
            InitializeGridIndicator();
        }

        public void ReloadData()
        {
            var version = DDCL.BannerLib.GetVersion(VersionID);
            Version = version.version;
            VersionName = version.name;

            var banner = DDCL.BannerLib.GetBanner(VersionID, BannerID);
            BannerName = banner.name;
            PoolType = banner.type;

            var tz = DDCL.CurrentUser.GetActivatingTimeZone();
            BeginTime = DDCL.GetBannerTimeOffset(banner.beginTime, banner.beginTimeSync, tz).ToLocalTime();
            EndTime = DDCL.GetBannerTimeOffset(banner.endTime, banner.endTimeSync, tz).ToLocalTime();

            var bannerlog = DDCL.CurrentUser.GetBanner(VersionID, BannerID);
            Total = bannerlog.Logs.Count;
            Rank5 = bannerlog.Logs.Count(x => x.rank == 5);
            Rank5Up = bannerlog.Logs.Count(x => banner.rank5Up.Contains(x.unitclass));
            Rank4 = bannerlog.Logs.Count(x => x.rank == 4);
            Rank4Up = bannerlog.Logs.Count(x => banner.rank4Up.Contains(x.unitclass));
            TotalIncludesInherited = Total + 
                bannerlog.GreaterRounds.First()?.Logs.Count(x => x.banner_id != BannerID || x.version_id != VersionID) ?? 0;
            Rank5IncludesInherited = Rank5 + 
                bannerlog.GreaterRounds.First()?.Logs.Count(
                    x => x.rank == 5 && (x.banner_id != BannerID || x.version_id != VersionID)
                ) ?? 0;

            var r5ups = new List<DDCVUnitIndicatorModel>();
            var r4ups = new List<DDCVUnitIndicatorModel>();

            if (IsEventPool)
            {
                foreach(var r5 in banner.rank5Up)
                {
                    var item = DDCL.UnitLib.GetItemById(r5);
                    r5ups.Add(new DDCVUnitIndicatorModel
                    {
                        Name = item.name,
                        Count = bannerlog.Logs.Count(x => x.unitclass == r5)
                    });
                }
                r5ups.Add(new DDCVUnitIndicatorModel
                {
                    Name = "常驻",
                    Count = Rank5 - Rank5Up
                });
                foreach (var r4 in banner.rank4Up)
                {
                    var item = DDCL.UnitLib.GetItemById(r4);
                    r4ups.Add(new DDCVUnitIndicatorModel
                    {
                        Name = item.name,
                        Count = bannerlog.Logs.Count(x => x.unitclass == r4)
                    });
                }
                r4ups.Add(new DDCVUnitIndicatorModel
                {
                    Name = "常驻",
                    Count = Rank4 - Rank4Up
                });
            }
            else
            {
                var r5chr = bannerlog.Logs.Count(x => x.rank == 5 && x.unittype == DDCCUnitType.Character);
                var r4chr = bannerlog.Logs.Count(x => x.rank == 4 && x.unittype == DDCCUnitType.Character);
                r5ups.Add(new DDCVUnitIndicatorModel
                {
                    Name = "常驻角色",
                    Count = r5chr
                });
                r5ups.Add(new DDCVUnitIndicatorModel
                {
                    Name = "常驻武器",
                    Count = Rank5 - r5chr
                });
                r4ups.Add(new DDCVUnitIndicatorModel
                {
                    Name = "常驻角色",
                    Count = r4chr
                });
                r4ups.Add(new DDCVUnitIndicatorModel
                {
                    Name = "常驻武器",
                    Count = Rank4 - r4chr
                });
            }
            Rank5Ups = new ObservableCollection<DDCVUnitIndicatorModel>(r5ups);
            Rank4Ups = new ObservableCollection<DDCVUnitIndicatorModel>(r4ups);


            var rnds = new List<DDCVRoundItemModel>();
            var idx = 1;
            foreach(var r in bannerlog.GreaterRounds)
            {
                rnds.Add(new DDCVRoundItemModel
                {
                    Index = idx++,
                    VersionId = VersionID,
                    BannerId = BannerID,
                    LogItem = r
                });
            }
            rnds.Reverse();
            SelectedRound = rnds.First();
            Rounds = new ObservableCollection<DDCVRoundItemModel>(rnds);

            RefreshDashboard();
        }
    }
}
