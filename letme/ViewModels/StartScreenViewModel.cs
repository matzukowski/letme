using letme.Classes;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;

namespace letme.ViewModels
{
    [RegionMemberLifetime(KeepAlive = false)]
    public class StartScreenViewModel : BindableBase, INavigationAware
    {
        //private IDialogService _dialogService;
        private IRegionManager _regionManager;

        public DelegateCommand StartCommand { get; private set; }
        public DelegateCommand StopCommand { get; private set; }
        public DelegateCommand OpenManageCommandsViewCommand { get; private set; }

        private SpeechRecognition _speechRecognition;
        public SpeechRecognition SpeechRecognition
        {
            get { return _speechRecognition; }
            set { SetProperty(ref _speechRecognition, value); }
        }

        public StartScreenViewModel(IRegionManager regionManager, IDialogService dialogService, SpeechRecognition speechRecognition)
        {
            _regionManager = regionManager;

            _speechRecognition = speechRecognition;

            StartCommand = new DelegateCommand(Start);
            StopCommand = new DelegateCommand(Stop);
            OpenManageCommandsViewCommand = new DelegateCommand(OpenManageCommandsView);
        }

        private void Start()
        {
            SpeechRecognition.Start();
        }

        private void Stop()
        {
            SpeechRecognition.Stop();
        }

        private void OpenManageCommandsView()
        {
            SpeechRecognition.RecognisedText = "";

            _regionManager.RequestNavigate(Names.contentRegion, Names.manageCommandsView);
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {

        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }
    }
}