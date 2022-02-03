using Prism.Mvvm;
using System;

namespace letme.Classes
{
    public class CommandAction : BindableBase
    {
        private ActionType _actionType;
        public ActionType ActionType
        {
            get { return _actionType; }
            set { SetProperty(ref _actionType, value); }
        }

        private string _parameter;
        public string Parameter
        {
            get { return _parameter; }
            set { SetProperty(ref _parameter, value); }
        }

        public CommandAction() { }

        public CommandAction(ActionType actionType, string parameter)
        {
            this.ActionType = actionType;
            this.Parameter = parameter;
        }

        public CommandAction(CommandAction commandAction)
        {
            if (commandAction != null)
            {
                this.ActionType = commandAction.ActionType;
                this.Parameter = commandAction.Parameter;
            }
        }

        public override string ToString() => ActionType.ToString().PadRight(10) + " " + Parameter;
    }
}