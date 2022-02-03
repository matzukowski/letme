using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Data;

namespace letme.Classes
{
    public class NegationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool)
            {
                return !(bool)value;
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class BoolToVisilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool)
            {
                if ((bool)value)
                {
                    return "Visible";
                }
                else
                {
                    return "Hidden";
                }
            }
            return "Visible";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class BoolToVisilityNegatedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool)
            {
                if ((bool)value)
                {
                    return "Hidden";
                }
                else
                {
                    return "Visible";
                }
            }
            return "Visible";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class CommandsToPhrasesStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ObservableCollection<Command>)
            {
                ObservableCollection<Command> commands = (ObservableCollection<Command>)value;

                string phrases = "";

                for (int i = 0; i < commands.Count; i++)
                {
                    phrases += "> " + commands[i].Phrase + "\n";
                }

                return phrases;
            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class CommandDetailsToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Command)
            {
                Command command = (Command)value;

                if (command.Phrase != null)
                {
                    string actions = "> ";

                    if (command.CommandActions.Count > 0)
                    {
                        actions += command.CommandActions[0].ToString() + "\n";

                        for (int i = 1; i < command.CommandActions.Count; i++)
                        {
                            actions += "> " + command.CommandActions[i].ToString() + "\n";
                        }
                    }

                    return actions;
                }
                return "";
            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
    
    public class ActionTypeToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ActionType)
            {
                ActionType actionType = (ActionType)value;

                if (actionType.ToString() == (string)parameter)
                {
                    return true;
                }
                return false;
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var obj = Enum.Parse(typeof(ActionType), (string)parameter);
            return Enum.Parse(typeof(ActionType), (string)parameter);
        }
    }
}