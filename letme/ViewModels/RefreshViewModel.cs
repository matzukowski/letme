using Prism.Mvvm;
using Prism.Regions;

namespace letme.ViewModels
{
    [RegionMemberLifetime(KeepAlive = false)]
    public class RefreshViewModel : BindableBase
    {
        public RefreshViewModel() { }
    }
}