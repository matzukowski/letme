using letme.Classes;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;

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
        public DelegateCommand OpenSettingsViewCommand { get; private set; }

        private SpeechRecognition _speechRecognition;
        public SpeechRecognition SpeechRecognition
        {
            get { return _speechRecognition; }
            set { SetProperty(ref _speechRecognition, value); }
        }

        public StartScreenViewModel(IRegionManager regionManager, IDialogService dialogService, SpeechRecognition speechRecognition)
        {
            //regionManager.RegisterViewWithRegion(Names.consoleViewRegion, typeof(ConsoleView));
            //regionManager.RegisterViewWithRegion(Names.commandListViewRegion, typeof(CommandListView));
            _regionManager = regionManager;

            //_dialogService = dialogService;

            _speechRecognition = speechRecognition;

            StartCommand = new DelegateCommand(Start);
            StopCommand = new DelegateCommand(Stop);
            OpenManageCommandsViewCommand = new DelegateCommand(OpenManageCommandsView);
            OpenSettingsViewCommand = new DelegateCommand(OpenSettingsView);
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
            _regionManager.RequestNavigate(Names.contentRegion, Names.manageCommandsView);
        }

        private void OpenSettingsView()
        {

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
