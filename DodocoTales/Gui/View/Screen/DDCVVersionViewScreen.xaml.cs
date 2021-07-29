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
using DodocoTales.Gui.View.Card;
using DodocoTales.Gui.Model;
using DodocoTales.Common.Enums;
using DodocoTales.Common;

namespace DodocoTales.Gui.View.Screen
{
    /// <summary>
    /// DDCVVersionViewScreen.xaml 的交互逻辑
    /// </summary>
    public partial class DDCVVersionViewScreen : UserControl
    {
        public DDCVVersionViewScreen()
        {
            InitializeComponent();
            DDCS.LogReloadCompleted += new DDCSCommonDelegate(LoadVersions);
        }


        public void LoadVersions()
        {
            var versioninfos = new List<DDCVVersionInfo>();
            bool firstfound = false;
            DDCVInheritedRounds inherit = new DDCVInheritedRounds
            {
                Beginner = new List<DDCVInheritedRound>(),
                Permanent = new List<DDCVInheritedRound>(),
                EventCharacter = new List<DDCVInheritedRound>(),
                EventWeapon = new List<DDCVInheritedRound>()
            };
            var curlog = DDCL.Users.getCurrentUser();
            foreach(var ver in curlog.V)
            {
                var verl = DDCL.Banners.eventPools.FindAll(x => x.id == ver.id);
                if (verl.Count != 1) 
                {
                    // TODO
                    continue;
                }
                var verinfo = verl[0];

                DDCVVersionInfo model = new DDCVVersionInfo
                {
                    Info = verinfo,
                    Logs = ver,
                    Inherited = inherit
                };

                inherit = new DDCVInheritedRounds
                {
                    Beginner = inherit.Beginner.FindAll(x => true),
                    Permanent = inherit.Permanent.FindAll(x => true),
                    EventCharacter = inherit.EventCharacter.FindAll(x => true),
                    EventWeapon = inherit.EventWeapon.FindAll(x => true)
                };

                // Inherit - Beginner
                foreach (var banner in ver.B.FindAll(x => x.poolType == DDCCPoolType.Beginner)) 
                {
                    var banl = DDCL.Banners.beginnerPools.FindAll(x => x.id == banner.id);
                    if (banl.Count != 1)
                    {
                        // TODO
                        continue;
                    }
                    var baninfo = banl[0];
                    for (int i = 0; i < banner.R.Count; i++) 
                    {
                        var r = banner.R[i];
                        if (r.L.Count == 0) continue;
                        firstfound = true;
                        if (r.L[r.L.Count - 1].rank == 5)
                        {
                            inherit.Beginner = new List<DDCVInheritedRound>();
                        }
                        else
                        {
                            inherit.Beginner.Add(new DDCVInheritedRound
                            {
                                version = verinfo,
                                banner = baninfo,
                                RoundIndex = i + 1,
                                Logs = r
                            });
                        }
                    }
                }
                // Inherit - Permanent
                foreach (var banner in ver.B.FindAll(x => x.poolType == DDCCPoolType.Permanent))
                {
                    var banl = DDCL.Banners.permanentPools.FindAll(x => x.id == banner.id);
                    if (banl.Count != 1)
                    {
                        // TODO
                        continue;
                    }
                    var baninfo = banl[0];
                    for (int i = 0; i < banner.R.Count; i++)
                    {
                        var r = banner.R[i];
                        if (r.L.Count == 0) continue;
                        firstfound = true;
                        if (r.L[r.L.Count - 1].rank == 5)
                        {
                            inherit.Permanent = new List<DDCVInheritedRound>();
                        }
                        else
                        {
                            inherit.Permanent.Add(new DDCVInheritedRound
                            {
                                version = verinfo,
                                banner = baninfo,
                                RoundIndex = i + 1,
                                Logs = r
                            });
                        }
                    }
                }
                // Inherit - EventCharacter
                foreach (var banner in ver.B.FindAll(x => x.poolType == DDCCPoolType.EventCharacter))
                {
                    var banl = verinfo.banners.FindAll(x => x.id == banner.id);
                    if (banl.Count != 1)
                    {
                        // TODO
                        continue;
                    }
                    var baninfo = banl[0];
                    for (int i = 0; i < banner.R.Count; i++)
                    {
                        var r = banner.R[i];
                        if (r.L.Count == 0) continue;
                        firstfound = true;
                        if (r.L[r.L.Count - 1].rank == 5)
                        {
                            inherit.EventCharacter = new List<DDCVInheritedRound>();
                        }
                        else
                        {
                            inherit.EventCharacter.Add(new DDCVInheritedRound
                            {
                                version = verinfo,
                                banner = baninfo,
                                RoundIndex = i + 1,
                                Logs = r
                            });
                        }
                    }
                }
                // Inherit - EventWeapon
                foreach (var banner in ver.B.FindAll(x => x.poolType == DDCCPoolType.EventWeapon))
                {
                    var banl = verinfo.banners.FindAll(x => x.id == banner.id);
                    if (banl.Count != 1)
                    {
                        // TODO
                        continue;
                    }
                    var baninfo = banl[0];
                    for (int i = 0; i < banner.R.Count; i++)
                    {
                        var r = banner.R[i];
                        if (r.L.Count == 0) continue;
                        firstfound = true;
                        if (r.L[r.L.Count - 1].rank == 5)
                        {
                            inherit.EventWeapon = new List<DDCVInheritedRound>();
                        }
                        else
                        {
                            inherit.EventWeapon.Add(new DDCVInheritedRound
                            {
                                version = verinfo,
                                banner = baninfo,
                                RoundIndex = i + 1,
                                Logs = r
                            });
                        }
                    }
                }

                if(firstfound)
                {
                    versioninfos.Add(model);
                }
            }
            Action act = () => { ApplyVersionCards(versioninfos); };
            Dispatcher.BeginInvoke(act);
        }

        public void ApplyVersionCards(List<DDCVVersionInfo> infos)
        {
            ListView.Children.Clear();
            infos.Reverse();
            foreach(var info in infos)
            {
                DDCVVersionViewScreenVersionLogCard card = new DDCVVersionViewScreenVersionLogCard();
                Action act = () => { card.LoadVersionInfo(info); };
                act.BeginInvoke(null, null);
                ListView.Children.Add(card);
            }
        }
    }
}
