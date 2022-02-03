using letme.Classes;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace letme.ViewModels
{
    [RegionMemberLifetime(KeepAlive = false)]
    public class ManageCommandsViewModel : BindableBase, INavigationAware
    {
        public string Title => "manage commands";

        private IRegionManager _regionManager;

        public DelegateCommand NewButtonClickCommand { get; private set; }
        public DelegateCommand DuplicateCommand { get; private set; }
        public DelegateCommand DeleteCommand { get; private set; }
        public DelegateCommand MoveUpCommand { get; private set; }
        public DelegateCommand MoveDownCommand { get; private set; }
        public DelegateCommand EditButtonClickCommand { get; private set; }
        public DelegateCommand GoBackCommand { get; private set; }

        private SpeechRecognition _speechRecognition;
        public SpeechRecognition SpeechRecognition
        {
            get { return _speechRecognition; }
            set { SetProperty(ref _speechRecognition, value); }
        }

        private int _selectedIndex;
        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set { SetProperty(ref _selectedIndex, value); }
        }

        private Command _selectedItem;
        public Command SelectedItem
        {
            get { return _selectedItem; }
            set { SetProperty(ref _selectedItem, value); }
        }

        public ManageCommandsViewModel(SpeechRecognition speechRecognition, IDialogService dialogService, IRegionManager regionManager)
        {
            _regionManager = regionManager;

            SpeechRecognition = speechRecognition;

            NewButtonClickCommand = new DelegateCommand(OpenNewCommandWindow);
            DuplicateCommand = new DelegateCommand(Duplicate);
            DeleteCommand = new DelegateCommand(Delete);
            MoveUpCommand = new DelegateCommand(MoveUp);
            MoveDownCommand = new DelegateCommand(MoveDown);
            EditButtonClickCommand = new DelegateCommand(OpenEditCommandWindow);
            GoBackCommand = new DelegateCommand(GoBack);

            if (SpeechRecognition.Commands.Count > 0)
            {
                SelectedItem = SpeechRecognition.Commands[0];
            }
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (SpeechRecognition.Commands.Count < 1)
            {
                SelectedIndex = -1;
            }
            else
            {
                if (navigationContext != null) SelectedIndex = navigationContext.Parameters.GetValue<int>("SelectedIndex");
            }
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }

        private void OpenNewCommandWindow()
        {
            NavigationParameters parameters = new NavigationParameters
            {
                { "NewCommand", true},
                { "SelectedIndex", SelectedIndex },
                { "SelectedActionIndex", -1 },
                { "SelectedCommand", new Command("phrase", new ObservableCollection<ActionType> { ActionType.say }, new List<string> { "new command parameter" }) }
            };

            _regionManager.RequestNavigate(Names.contentRegion, Names.editCommandView, parameters);
        }

        private void Duplicate()
        {
            if (SelectedIndex > -1)
            {
                SpeechRecognition.Commands.Insert(SelectedIndex + 1, new Command(SelectedItem));

                SelectedIndex++;

                SpeechRecognition.SaveToJSON();
            }
        }

        private void Delete()
        {
            if (SelectedItem != null)
            {
                int temp = SelectedIndex;

                if (temp >= 0) SpeechRecognition.Commands.RemoveAt(temp);

                if (SpeechRecognition.Commands.Count > 0)
                {
                    if (temp < SpeechRecognition.Commands.Count) SelectedIndex = temp;
                    else SelectedIndex = temp - 1;
                }
                else
                {
                    SelectedItem = null;
                }

                SpeechRecognition.SaveToJSON();
            }
        }

        private void OpenEditCommandWindow()
        {
            if (SelectedItem != null)
            {
                NavigationParameters parameters = new NavigationParameters
                {
                    { "NewCommand", false},
                    { "SelectedIndex", SelectedIndex },
                    { "SelectedActionIndex", -1 },
                    { "SelectedCommand", new Command(SelectedItem) }
                };

                _regionManager.RequestNavigate(Names.contentRegion, Names.editCommandView, parameters);
            }
            else
            {
                OpenNewCommandWindow();
            }
        }

        private void MoveUp()
        {
            if (SelectedIndex > 0)
            {
                SpeechRecognition.Commands.Move(SelectedIndex, SelectedIndex - 1);
            }
        }

        private void MoveDown()
        {
            if (SelectedIndex < SpeechRecognition.Commands.Count - 1)
            {
                SpeechRecognition.Commands.Move(SelectedIndex, SelectedIndex + 1);
            }
        }

        private void GoBack()
        {
            _regionManager.RequestNavigate(Names.contentRegion, Names.startScreenView);
        }
    }
}