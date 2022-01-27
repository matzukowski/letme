using Prism.Mvvm;
using System;
using System.Collections.Generic;
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
        SpeechRecognitionEngine _recognizer = new SpeechRecognitionEngine(new System.Globalization.CultureInfo("en-GB"));

        SpeechSynthesizer _synthesizer = new SpeechSynthesizer();

        Choices _vocabulary = new Choices();


        private ObservableCollection<Command> _commands = new ObservableCollection<Command>();
        public ObservableCollection<Command> Commands
        {
            get { return _commands; }
            set { SetProperty(ref _commands, value); }
        }

        private string _recognisedText;
        public string RecognisedText
        {
            get { return _recognisedText; }
            set { SetProperty(ref _recognisedText, value); }
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

            //synthesizer.SelectVoice(synthesizer.GetInstalledVoices()[2].VoiceInfo.Name

            LoadFromJSON();

            LoadDefaultCommands();

            foreach (Command command in Commands)
            {
                if (command.Phrase != null)
                {
                    _vocabulary.Add(command.Phrase);
                }
            }

            _vocabulary.Add("hi");

            Grammar grammar = new Grammar(new GrammarBuilder(_vocabulary));

            _recognizer.RequestRecognizerUpdate();

            // Create and load a dictation grammar. 
            _recognizer.LoadGrammar(new DictationGrammar());

            // Add our vocabulary
            _recognizer.LoadGrammar(grammar);

            // Add a handler for the speech recognized event.  
            _recognizer.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(Recognizer_SpeechRecognized);

            // Configure input to the speech recognizer.  
            _recognizer.SetInputToDefaultAudioDevice();
        }

        private void LoadDefaultCommands()
        {
            List<Command> defaultCommands = new List<Command>();

            defaultCommands.Add(new Command("left", new ObservableCollection<ActionType> { ActionType.stroke }, new List<string> { "a" }));
            defaultCommands.Add(new Command("right", new ObservableCollection<ActionType> { ActionType.stroke }, new List<string> { "d" }));
            defaultCommands.Add(new Command("jump", new ObservableCollection<ActionType> { ActionType.stroke }, new List<string> { "Space" }));
            defaultCommands.Add(new Command("left", new ObservableCollection<ActionType> { ActionType.stroke }, new List<string> { "z" }));
            defaultCommands.Add(new Command("test", new ObservableCollection<ActionType> { ActionType.stroke, ActionType.wait, ActionType.stroke, ActionType.say }, new List<string> { "z", "3000", "s", "dupa" }));
            defaultCommands.Add(new Command("say", new ObservableCollection<ActionType> { ActionType.say }, new List<string> { "wholoolololooololo" }));

            foreach (Command command in defaultCommands)
            {
                if (Commands.Where(c => c.Phrase == command.Phrase).Count() == 0)
                {
                    Commands.Add(command);
                }
            }
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

        //public void AddCommand(string phrase, ObservableCollection<ActionType> actionTypes, List<String> parameters) // zbędne
        //{
        //    if (Commands.Where(c => c.Phrase == phrase).Count() == 0)
        //    {
        //        Commands.Add(new Command("fuck", actionTypes, parameters));
        //    }
        //}

        //public void EditCommand(string oldPhrase, string newPhrase, ObservableCollection<ActionType> actionTypes, List<String> parameters)
        //{
        //    int index = Commands.IndexOf(Commands.FirstOrDefault(c => c.Phrase == oldPhrase));

        //    Commands[index] = new Command(newPhrase, actionTypes, parameters);
        //}

        //public void DeleteCommand(string phrase)
        //{
        //    int index = Commands.IndexOf(Commands.FirstOrDefault(c => c.Phrase == phrase));
            
        //    Commands.RemoveAt(index);
        //}

        // Handle the SpeechRecognized event.  
        public async void Recognizer_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            string input = e.Result.Text.ToLower();

            RecognisedText += "> " + input + "\n";

            Command command = Commands.FirstOrDefault(x => x.Phrase == input);

            Task.Factory.StartNew(() =>
            {
                RunCommand(command);
            });
        }

        public void LoadFromJSON()
        {
            if (File.Exists("commands.json"))
            {
                var json = File.ReadAllText("../../Files/commands.json");

                var serializer = new JavaScriptSerializer();

                Commands = serializer.Deserialize<ObservableCollection<Command>>(json);
            }
        }
        public void SaveToJSON()
        {
            var serializer = new JavaScriptSerializer();

            var json = serializer.Serialize(Commands.Where(command => command.Phrase != null));

            File.WriteAllText("../../Files/commands.json", json);
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
                            Say(command.CommandActions[i].Parameter);
                            break;

                        case ActionType.stroke:
                            TypeString(command.CommandActions[i].Parameter); //TODO: KeyStroke.ahk
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

                        case ActionType.buildIn:
                            Wait(command.CommandActions[i].Parameter);
                            break;

                        default:
                            break;
                    }
                }
            }
        }

        private void Say(string parameter)
        {
            _synthesizer.SpeakAsync(parameter);
        }

        private void KeyStroke(string parameter)
        {
            Process.Start(@"C:\Program Files\AutoHotkey\AutoHotkey.exe", @"C:\KeyStroke.ahk " + parameter);
        }

        private void KeyHold(string parameter)
        {

        }

        private void KeyRelease(string parameter)
        {

        }

        public void TypeString(string parameter)
        {
            Process.Start(@"C:\Program Files\AutoHotkey\AutoHotkey.exe", @"C:\Scripts\KeyStroke.ahk " + parameter);
        }

        public void Wait(string parameter)
        {
            Thread.Sleep(int.Parse(parameter));
        }

        public void Run(string parameter)
        {
            Process.Start(parameter);
        }
    }
}



//TODO: (anulowane) Dodatkowe okno z komendami on/off
//TODO: (anulowane) zamiast boola, można dodawać znak specjalny na początku komendy, która ma zostać wyłączona, o!
