using letme.Classes;
using letme.Views;
using Prism.Ioc;
using Prism.Modularity;
using System.Windows;

namespace letme
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<StartScreenView>(Names.startScreenView);
            containerRegistry.RegisterForNavigation<ManageCommandsView>(Names.manageCommandsView);
            containerRegistry.RegisterForNavigation<EditCommandView>(Names.editCommandView);
            containerRegistry.RegisterForNavigation<RefreshView>(Names.refresView);

            containerRegistry.RegisterSingleton<SpeechRecognition, SpeechRecognition>();
        }
    }
}
