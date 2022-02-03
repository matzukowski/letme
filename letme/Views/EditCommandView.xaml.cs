using System.Windows.Controls;

namespace letme.Views
{
    /// <summary>
    /// Interaction logic for EditCommandView
    /// </summary>
    public partial class EditCommandView : UserControl
    {
        public EditCommandView()
        {
            InitializeComponent();
        }

        private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            System.Diagnostics.Process.Start(e.Uri.ToString());
        }
    }
}
