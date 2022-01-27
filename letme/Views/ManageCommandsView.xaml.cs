using System.Windows.Controls;

namespace letme.Views
{
    /// <summary>
    /// Interaction logic for ManageCommandsView
    /// </summary>
    public partial class ManageCommandsView : UserControl
    {
        public ManageCommandsView()
        {
            InitializeComponent();
        }

        private void LB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LB.ScrollIntoView(LB.SelectedItem);
        }
    }
}
