using letme.Classes;
using letme.Views;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace letme.ViewModels
{
    //[RegionMemberLifetime(KeepAlive = false)]
    public class MainWindowViewModel : BindableBase
    {
        public DelegateCommand ClosingCommand { get; private set; }

        private SpeechRecognition _speechRecognition;
        public SpeechRecognition SpeechRecognition
        {
            get { return _speechRecognition; }
            set { SetProperty(ref _speechRecognition, value); }
        }

        private string _title = "letme";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public MainWindowViewModel(IRegionManager regionManager, SpeechRecognition speechRecognition)
        {
            //regionManager.RegisterViewWithRegion(Names.consoleViewRegion, typeof(ConsoleView));
            //regionManager.RegisterViewWithRegion(Names.commandListViewRegion, typeof(CommandListView));

            regionManager.RegisterViewWithRegion(Names.contentRegion, typeof(StartScreenView));

            _speechRecognition = speechRecognition;

            ClosingCommand = new DelegateCommand(new Action(Closing));
        }

        private void Refresh()
        {

        }

        private void Closing()
        {
            _speechRecognition.SaveToJSON();
        }
    }
}
