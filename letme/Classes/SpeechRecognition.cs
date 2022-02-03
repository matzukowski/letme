using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace letme.Classes
{
    public class SpeechRecognition : BindableBase, INotifyPropertyChanged
    {
        private string _AHKPath = "C:\\Program Files\\AutoHotkey\\AutoHotkey.exe";
        private string _filesPath = "..\\..\\Files";

        private SpeechRecognitionEngine _recognizer = new SpeechRecognitionEngine(new System.Globalization.CultureInfo("en-GB"));

        private SpeechSynthesizer _synthesizer = new SpeechSynthesizer();

        private ObservableCollection<Command> _commands = new ObservableCollection<Command>();
        public ObservableCollection<Command> Commands
        {
            get => _commands;
            set => SetProperty(ref _commands, value);
        }

        private string _recognisedText;
        public string RecognisedText
        {
            get => _recognisedText;
            set => SetProperty(ref _recognisedText, value);
        }

        private bool _isActive = false;
        public bool IsActive
        {
            get => _isActive; //ctrl +  .
            set => SetProperty(ref _isActive, value);
        }

        public SpeechRecognition()
        {
            // Configure the audio output.   
            _synthesizer.SetOutputToDefaultAudioDevice();

            _synthesizer.Rate = 0;

            LoadFromJSON();

            foreach (Command command in Commands)
            {
                if (command.Phrase != null)
                {
                    AddVocabulary(command.Phrase);
                }
            }

            _recognizer.RequestRecognizerUpdate();

            // Create and load a dictation grammar. 
            _recognizer.LoadGrammar(new DictationGrammar());

            // Add a handler for the speech recognized event.  
            _recognizer.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(Recognizer_SpeechRecognized);

            // Configure input to the speech recognizer.  
            _recognizer.SetInputToDefaultAudioDevice();
        }

        public void Start()
        {
            // Start asynchronous, continuous speech recognition.  
            _recognizer.RecognizeAsync(RecognizeMode.Multiple);
            IsActive = true;
        }

        public void Stop()
        {
            _recognizer.RecognizeAsyncStop();
            IsActive = false;
        }

        public void AddVocabulary(string phrase)
        {
            Choices vocabulary = new Choices(phrase);

            string[] words = phrase.Split(' ');

            if (words.Length > 1)
            {
                foreach (string word in words)
                {
                    vocabulary.Add(new Choices(word));
                }
            }

            Grammar grammar = new Grammar(new GrammarBuilder(vocabulary));

            _recognizer.LoadGrammar(grammar);
        }

        // Handle the SpeechRecognized event.  
        public async void Recognizer_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            string input = e.Result.Text.ToLower();

            RecognisedText += "> " + input + "\n";

            Command command = Commands.FirstOrDefault(x => x.Phrase == input);

            await Task.Factory.StartNew(() => { RunCommand(command); });
        }

        public void LoadFromJSON()
        {
            if (File.Exists(_filesPath + "/commands.json"))
            {
                var json = File.ReadAllText(_filesPath + "/commands.json");

                var serializer = new JavaScriptSerializer();

                Commands = serializer.Deserialize<ObservableCollection<Command>>(json);

                foreach (Command command in Commands)
                {
                    foreach (CommandAction action in command.CommandActions)
                    {
                        AddVocabulary(action.Parameter);
                    }
                }
            }
        }
        public void SaveToJSON()
        {
            var serializer = new JavaScriptSerializer();

            var json = serializer.Serialize(Commands.Where(command => command.Phrase != null));

            File.WriteAllText(_filesPath + "/commands.json", json);
        }

        private void RunCommand(Command command)
        {
            if (command?.Phrase != null)
            {
                for (int i = 0; i < command.CommandActions.Count; i++)
                {
                    switch (command.CommandActions[i].ActionType)
                    {
                        case ActionType.say:
                            SayString(command.CommandActions[i].Parameter);
                            break;

                        case ActionType.press:
                            KeyPress(command.CommandActions[i].Parameter);
                            break;

                        case ActionType.hold:
                            KeyHold(command.CommandActions[i].Parameter);
                            break;

                        case ActionType.release:
                            KeyRelease(command.CommandActions[i].Parameter);
                            break;

                        case ActionType.type:
                            TypeString(command.CommandActions[i].Parameter);
                            break;

                        case ActionType.wait:
                            Wait(command.CommandActions[i].Parameter);
                            break;

                        case ActionType.run:
                            Run(command.CommandActions[i].Parameter);
                            break;

                        default:
                            break;
                    }
                }
            }
        }

        private void SayString(string parameter)
        {
            _synthesizer.SpeakAsync(parameter);
        }

        private void KeyPress(string parameter)
        {
            Process.Start(_AHKPath, _filesPath + "\\Press.ahk " + parameter);
        }

        private void KeyHold(string parameter)
        {
            Process.Start(_AHKPath, _filesPath + "\\Hold.ahk " + parameter);
        }

        private void KeyRelease(string parameter)
        {
            KeyPress(parameter);
        }

        public void TypeString(string parameter)
        {
            Process.Start(_AHKPath, _filesPath + "\\Type.ahk " + "\"" + parameter + "\"");
        }

        public void Wait(string parameter)
        {
            if(int.TryParse(parameter, out _))
            {
                Thread.Sleep(int.Parse(parameter));
            }
        }

        public void Run(string parameter)
        {
            if (File.Exists(parameter))
            {
                Process.Start(parameter);
            }
        }
    }
}