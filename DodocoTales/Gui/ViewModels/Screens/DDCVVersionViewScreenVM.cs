using CommunityToolkit.Mvvm.ComponentModel;
using DodocoTales.Common.Enums;
using DodocoTales.Gui.Models;
using DodocoTales.Library;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DodocoTales.Gui.ViewModels.Screens
{
    public class DDCVVersionViewScreenVM : ObservableObject
    {
        private ObservableCollection<DDCVVersionItemModel> versions;
        public ObservableCollection<DDCVVersionItemModel> Versions
        {
            get => versions;
            set => SetProperty(ref versions, value);
        }




        public void ReloadData()
        {
            var ls = new List<DDCVVersionItemModel>();
            var typeEmpty = new Dictionary<DDCCPoolType, bool>();
            foreach(DDCCPoolType type in Enum.GetValues(typeof(DDCCPoolType)))
            {
                typeEmpty[type] = true;
            }
            var tz = DDCL.CurrentUser.GetActivatingTimeZone();
            foreach (var version in DDCL.BannerLib.Versions)
            {
                DDCVVersionItemModel vermodel = new DDCVVersionItemModel
                {
                    VersionId = version.id,
                    VersionName = version.name,
                    Version=version.version,
                    BeginTime=DDCL.GetSyncTimeOffset(version.beginTime).ToLocalTime(),
                    EndTime=DDCL.GetSyncTimeOffset(version.endTime).ToLocalTime()
                };
                var r5s = new List<DDCVUnitIndicatorModel>();
                var banls = new List<DDCVBannerItemModel>();
                var banners = DDCL.CurrentUser.GetBannersByVersion(version.id);
                foreach (var banner in banners)
                {
                    var baninfo = DDCL.BannerLib.GetBanner(banner.VersionId, banner.BannerId);
                    DDCVBannerItemModel banmodel = new DDCVBannerItemModel
                    {
                        VersionId = version.id,
                        Version = version.version,
                        BannerName = baninfo.name,
                        BannerId = banner.BannerId,
                        BannerHint = baninfo.hint,
                        PoolType = banner.CategorizedGachaType,
                        BeginTime = DDCL.GetBannerTimeOffset(baninfo.beginTime, baninfo.beginTimeSync, tz).ToLocalTime(),
                        EndTime = DDCL.GetBannerTimeOffset(baninfo.endTime, baninfo.endTimeSync, tz).ToLocalTime(),
                        
                    };

                    banmodel.Total = banner.Logs.Count;

                    if (banmodel.Total > 0)
                    {
                        typeEmpty[banner.CategorizedGachaType] = false;
                    }

                    var r5 = banner.Logs.FindAll(x => x.rank == 5);
                    banmodel.Rank5 = r5.Count;
                    var r5ups = new List<DDCVUnitIndicatorModel>();
                    foreach(var up in baninfo.rank5Up)
                    {
                        r5ups.Add(new DDCVUnitIndicatorModel
                        {
                            Name = DDCL.UnitLib.GetItemById(up).name,
                            Count = r5.FindAll(x => x.unitclass == up).Count
                        });
                    }
                    banmodel.Rank5Ups=new ObservableCollection<DDCVUnitIndicatorModel>(r5ups);
                    foreach(var item in r5)
                    {
                        r5s.Add(new DDCVUnitIndicatorModel
                        {
                            Name = DDCL.UnitLib.GetItemById(item.unitclass).name,
                            Time = DDCL.GetTimeOffset(item.time, tz),
                            Version = version.version,
                            Banner = baninfo.name,
                            InternalId = item.internal_id,
                            Count = DDCL.CurrentUser.Logs.Values.Where(x => x.round_id == item.round_id).Count()
                        });
                    }


                    var r4 = banner.Logs.FindAll(x => x.rank == 4);
                    banmodel.Rank4 = r4.Count; 
                    var r4ups = new List<DDCVUnitIndicatorModel>();
                    foreach (var up in baninfo.rank4Up)
                    {
                        r4ups.Add(new DDCVUnitIndicatorModel
                        {
                            Name = DDCL.UnitLib.GetItemById(up).name,
                            Count = r4.FindAll(x => x.unitclass == up).Count
                        });
                    }
                    r4ups.Sort((x, y) => x.Name.Length.CompareTo(y.Name.Length));
                    banmodel.Rank4Ups = new ObservableCollection<DDCVUnitIndicatorModel>(r4ups);


                    if (!typeEmpty[banner.CategorizedGachaType])
                    {
                        banls.Add(banmodel);
                    }
                    typeEmpty[DDCCPoolType.Beginner] = typeEmpty[DDCCPoolType.Null] = true;
                }
                

                if (banls.Count > 0)
                {
                    banls.Reverse();
                    vermodel.Banners = new ObservableCollection<DDCVBannerItemModel>(banls);
                    r5s.Sort((x, y) => y.InternalId.CompareTo(x.InternalId));
                    vermodel.Rank5s = new ObservableCollection<DDCVUnitIndicatorModel>(r5s);
                    ls.Add(vermodel);
                }

                
            }
            ls.Reverse();
            Versions = new ObservableCollection<DDCVVersionItemModel>(ls);
        }


    }
}
