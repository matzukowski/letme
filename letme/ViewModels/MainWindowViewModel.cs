using letme.Classes;
using letme.Views;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace letme.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private string _filesPath;

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

        private string _ascii;
        public string ASCII
        {
            get { return _ascii; }
            set { SetProperty(ref _ascii, value); }
        }

        public MainWindowViewModel(IRegionManager regionManager, SpeechRecognition speechRecognition)
        {
            string exeFile = new System.Uri(Assembly.GetEntryAssembly().CodeBase).AbsolutePath;

            string exeDir = Path.GetDirectoryName(exeFile);

            _filesPath = Path.Combine(exeDir, @"..\..\Files").Replace("%20", " ");

            StreamReader reader = new StreamReader(_filesPath + @"\ASCII.txt", Encoding.GetEncoding(850));

            ASCII = reader.ReadToEnd();

            regionManager.RegisterViewWithRegion(Names.contentRegion, typeof(StartScreenView));

            _speechRecognition = speechRecognition;

            ClosingCommand = new DelegateCommand(new Action(Closing));
        }

        private void Closing()
        {
            _speechRecognition.SaveToJSON();
        }
    }
}
