using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace letme.Classes
{
    public class Command : BindableBase
    {
        private string _phrase;
        public string Phrase
        {
            get { return _phrase; }
            set { SetProperty(ref _phrase, value); }
        }

        private ObservableCollection<CommandAction> _commandActions;
        public ObservableCollection<CommandAction> CommandActions
        {
            get { return _commandActions; }
            set { SetProperty(ref _commandActions, value); }
        }

        public Command() { }

        public Command(string phrase, ObservableCollection<ActionType> actionTypes, List<String> parameters)
        {
            this.Phrase = phrase;
            this.CommandActions = new ObservableCollection<CommandAction>();

            for(int i = 0; i < actionTypes.Count; i++)
            {
                this.CommandActions.Add(new CommandAction(actionTypes[i], parameters[i]));
            }
        }

        public Command(Command command)
        {
            this.Phrase = command.Phrase;
            this.CommandActions = new ObservableCollection<CommandAction>(command.CommandActions);
        }

        public override String ToString()
        {
            if (CommandActions == null)
            {
                return null;
            }
            else
            {
                if (CommandActions.Count < 1)
                {
                    if (Phrase.Length > 30)
                    {
                        return "> " + Phrase.Substring(0, 25) + "(...) : ";
                    }

                    return "> " + Phrase.PadRight(30) + " : ";
                }
                else
                {
                    string more = "";

                    if (CommandActions.Count() > 1) more = " (...)";

                    if (Phrase.Length > 30)
                    {
                        return "> " + Phrase.Substring(0, 25) + "(...) : " + CommandActions[0].ToString() + more;
                    }

                    return "> " + Phrase.PadRight(30) + " : " + CommandActions[0].ToString() + more;
                }
            }
        }
    }
}
