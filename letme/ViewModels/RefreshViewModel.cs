using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace letme.ViewModels
{
    [RegionMemberLifetime(KeepAlive = false)]
    public class RefreshViewModel : BindableBase
    {
        public RefreshViewModel() { }
    }
}
