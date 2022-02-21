using letme.Classes;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace letme.ViewModels
{
    [RegionMemberLifetime(KeepAlive = false)]
    public class EditCommandViewModel : BindableBase, INavigationAware
    {
        public string Title => "edit command";

        private bool _newCommand = false;
        private bool _addingNew = false;

        private IRegionManager _regionManager;

        public DelegateCommand AddNewCommand { get; private set; }
        public DelegateCommand DuplicateCommand { get; private set; }
        public DelegateCommand DeleteCommand { get; private set; }
        public DelegateCommand MoveUpCommand { get; private set; }
        public DelegateCommand MoveDownCommand { get; private set; }
        public DelegateCommand EditCommand { get; private set; }
        public DelegateCommand ApplyChangesCommand { get; private set; }
        public DelegateCommand CancelChangesCommand { get; private set; }
        public DelegateCommand DoneCancelCommand { get; private set; }
        public DelegateCommand DoneConfirmCommand { get; private set; }

        private SpeechRecognition _speechRecognition;
        public SpeechRecognition SpeechRecognition
        {
            get { return _speechRecognition; }
            set { SetProperty(ref _speechRecognition, value); }
        }

        private int _selectedCommandIndex;
        public int SelectedCommandIndex
        {
            get { return _selectedCommandIndex; }
            set { SetProperty(ref _selectedCommandIndex, value); }
        }

        private Command _selectedCommand;
        public Command SelectedCommand
        {
            get { return _selectedCommand; }
            set { SetProperty(ref _selectedCommand, value); }
        }

        private int _selectedActionIndex;
        public int SelectedActionIndex
        {
            get { return _selectedActionIndex; }
            set { SetProperty(ref _selectedActionIndex, value); }
        }

        private CommandAction _selectedCommandAction;
        public CommandAction SelectedCommandAction
        {
            get { return _selectedCommandAction; }
            set 
            {
                SetProperty(ref _selectedCommandAction, value);
            }
        }

        private CommandAction _selectedCommandsActionDuplicate;
        public CommandAction SelectedCommandActionDuplicate
        {
            get { return _selectedCommandsActionDuplicate; }
            set
            {
                SetProperty(ref _selectedCommandsActionDuplicate, value);
            }
        }

        private bool _editing;
        public bool Editing
        {
            get { return _editing; }
            set { SetProperty(ref _editing, value); }
        }

        public EditCommandViewModel(IRegionManager regionManager, SpeechRecognition speechRecognition)
        {
            _regionManager = regionManager;

            SpeechRecognition = speechRecognition;

            AddNewCommand = new DelegateCommand(AddNew);
            DuplicateCommand = new DelegateCommand(Duplicate);
            DeleteCommand = new DelegateCommand(Delete);
            MoveUpCommand = new DelegateCommand(MoveUp);
            MoveDownCommand = new DelegateCommand(MoveDown);
            EditCommand = new DelegateCommand(Edit);
            ApplyChangesCommand = new DelegateCommand(ApplyChanges);
            CancelChangesCommand = new DelegateCommand(CancelChanges);
            DoneCancelCommand = new DelegateCommand(DoneCancel);
            DoneConfirmCommand = new DelegateCommand(DoneConfirm);

            SelectedActionIndex = -1;

            Editing = false;
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            _newCommand = navigationContext.Parameters.GetValue<bool>("NewCommand");
            SelectedCommandIndex = navigationContext.Parameters.GetValue<int>("SelectedIndex");
            SelectedActionIndex = navigationContext.Parameters.GetValue<int>("SelectedActionIndex");
            SelectedCommand = navigationContext.Parameters.GetValue<Command>("SelectedCommand");

            if (SelectedActionIndex == -1 && SelectedCommand.CommandActions.Count > 0)
            {
                SelectedActionIndex = 0;
            }
        }
        
        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }

        private void AddNew()
        {
            _addingNew = true;

            if (SelectedActionIndex > -1)
            {
                int index = SelectedActionIndex + 1;

                SelectedCommand.CommandActions.Insert(index, new CommandAction(ActionType.say, "new action"));

                SelectedActionIndex = index;

                SelectedCommandActionDuplicate = new CommandAction(SelectedCommandAction);
            }
            else
            {
                SelectedActionIndex = SelectedCommand.CommandActions.Count;

                SelectedCommand.CommandActions.Insert(SelectedCommand.CommandActions.Count, new CommandAction(ActionType.say, "new action"));
            }

            Edit();
        }

        private void Duplicate()
        {
            if (SelectedActionIndex > -1)
            {
                SelectedCommand.CommandActions.Insert(SelectedActionIndex + 1, new CommandAction(SelectedCommandAction));

                SelectedCommandActionDuplicate = new CommandAction(SelectedCommandAction);

                SelectedActionIndex++;
            }
        }

        private void Delete()
        {
            if (SelectedActionIndex > -1)
            {
                int index = SelectedActionIndex;

                SelectedCommand.CommandActions.RemoveAt(index);

                if (index < SelectedCommand.CommandActions.Count)
                {
                    SelectedActionIndex = index;
                }
                else
                {
                    SelectedActionIndex = SelectedCommand.CommandActions.Count - 1;
                }
            }
        }

        private void MoveUp()
        {
            if (SelectedActionIndex > 0)
            {
                SelectedCommand.CommandActions.Move(SelectedActionIndex, SelectedActionIndex - 1);
            }
        }

        private void MoveDown()
        {
            if (SelectedActionIndex < SelectedCommand.CommandActions.Count - 1)
            {
                SelectedCommand.CommandActions.Move(SelectedActionIndex, SelectedActionIndex + 1);
            }
        }

        private void Edit()
        {
            if (SelectedCommandAction != null)
            {
                SelectedCommandActionDuplicate = new CommandAction(SelectedCommandAction);
                Editing = true;
            }
        }

        private void ApplyChanges()
        {
            int index = SelectedActionIndex;

            SelectedCommand.CommandActions.RemoveAt(index);

            SelectedCommand.CommandActions.Insert(index, SelectedCommandActionDuplicate);

            SelectedActionIndex = index;

            Editing = false;

            _addingNew = false;

            Refresh();
        }

        private void CancelChanges()
        {
            Editing = false;

            if (_addingNew)
            {
                int offset = 0;

                if (SelectedActionIndex != SelectedCommand.CommandActions.Count - 1)
                {
                    offset = -1;
                }

                Delete();

                SelectedActionIndex = SelectedActionIndex + offset;

                _addingNew = false;
            }
        }

        private void DoneCancel()
        {
            SpeechRecognition.SaveToJSON();

            NavigationParameters parameters = new NavigationParameters
            {
                { "SelectedIndex", SelectedCommandIndex }
            };

            _regionManager.RequestNavigate(Names.contentRegion, Names.manageCommandsView, parameters);
        }

        private void DoneConfirm()
        {
            if (_newCommand)
            {
                SelectedCommandIndex++;

                SpeechRecognition.Commands.Insert(SelectedCommandIndex ,SelectedCommand);
            }
            else
            {
                SpeechRecognition.Commands[SelectedCommandIndex] = SelectedCommand;
            }

            SpeechRecognition.SaveToJSON();

            SpeechRecognition.AddVocabulary(SelectedCommand.Phrase);

            NavigationParameters parameters = new NavigationParameters
            {
                { "SelectedIndex", SelectedCommandIndex }
            };

            _regionManager.RequestNavigate(Names.contentRegion, Names.manageCommandsView, parameters);
        }

        private void Refresh()
        {
            _regionManager.RequestNavigate(Names.contentRegion, Names.refresView);

            NavigationParameters parameters = new NavigationParameters
            {
                { "NewCommand", _newCommand },
                { "SelectedIndex", SelectedCommandIndex },
                { "SelectedActionIndex", SelectedActionIndex },
                { "SelectedCommand", SelectedCommand }
            };

            _regionManager.RequestNavigate(Names.contentRegion, Names.editCommandView, parameters);
        }
    }
}