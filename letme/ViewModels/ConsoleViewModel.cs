using letme.Classes;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace letme.ViewModels
{
    public class ConsoleViewModel : BindableBase
    {
        private SpeechRecognition _speechRecognition;
        public SpeechRecognition SpeechRecognition
        {
            get { return _speechRecognition; }
            set { SetProperty(ref _speechRecognition, value); }
        }

        public ConsoleViewModel(SpeechRecognition speechRecognition)
        {
            SpeechRecognition = speechRecognition;
        }
    }
}
