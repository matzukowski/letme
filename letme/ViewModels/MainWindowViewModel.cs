using letme.Classes;
using letme.Views;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows;

namespace letme.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        public string Title => "letme";

        private string _filesPath;

        public DelegateCommand MinimizeCommand { get; private set; }
        public DelegateCommand CloseCommand { get; private set; }
        public DelegateCommand ClosingCommand { get; private set; }

        private SpeechRecognition _speechRecognition;
        public SpeechRecognition SpeechRecognition
        {
            get { return _speechRecognition; }
            set { SetProperty(ref _speechRecognition, value); }
        }

        private string _ascii;
        public string ASCII
        {
            get { return _ascii; }
            set { SetProperty(ref _ascii, value); }
        }
        
        private string _ascii_Minimize;
        public string ASCII_Minimize
        {
            get { return _ascii_Minimize; }
            set { SetProperty(ref _ascii_Minimize, value); }
        }
        
        private string _ascii_Close;
        public string ASCII_Close
        {
            get { return _ascii_Close; }
            set { SetProperty(ref _ascii_Close, value); }
        }

        private WindowState _windowState;
        public WindowState WindowState
        {
            get { return _windowState; }
            set { SetProperty(ref _windowState, value); }
        }

        public MainWindowViewModel(IRegionManager regionManager, SpeechRecognition speechRecognition)
        {
            string exeFile = new System.Uri(Assembly.GetEntryAssembly().CodeBase).AbsolutePath;

            string exeDir = Path.GetDirectoryName(exeFile);

            _filesPath = Path.Combine(exeDir, @"..\..\Files").Replace("%20", " ");

            StreamReader reader = new StreamReader(_filesPath + @"\ASCII.txt", Encoding.GetEncoding(850));

            ASCII = reader.ReadToEnd();

            reader = new StreamReader(_filesPath + @"\ASCII_Minimize.txt", Encoding.GetEncoding(850));

            ASCII_Minimize = reader.ReadToEnd();

            reader = new StreamReader(_filesPath + @"\ASCII_Close.txt", Encoding.GetEncoding(850));

            ASCII_Close = reader.ReadToEnd();

            regionManager.RegisterViewWithRegion(Names.contentRegion, typeof(StartScreenView));

            SpeechRecognition = speechRecognition;

            MinimizeCommand = new DelegateCommand(new Action(MinimizeApp));

            CloseCommand = new DelegateCommand(new Action(CloseApp));

            ClosingCommand = new DelegateCommand(new Action(Closing));
        }

        private void MinimizeApp()
        {
            WindowState = WindowState.Minimized;
        }

        private void CloseApp()
        {
            Application.Current.Shutdown();
        }

        private void Closing()
        {
            SpeechRecognition.SaveToJSON();
        }
    }
}
