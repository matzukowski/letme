using letme.Classes;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;

namespace letme.ViewModels
{
    public class CommandListViewModel : BindableBase
    {
        private SpeechRecognition _speechRecognition;

        public SpeechRecognition SpeechRecognition
        {
            get { return _speechRecognition; }
            set { SetProperty(ref _speechRecognition, value); }
        }

        public CommandListViewModel(SpeechRecognition speechRecognition)
        {
            SpeechRecognition = speechRecognition;
        }
    }
}
