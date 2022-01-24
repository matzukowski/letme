using System.Windows.Controls;

namespace letme.Views
{
    /// <summary>
    /// Interaction logic for StartScreenView
    /// </summary>
    public partial class StartScreenView : UserControl
    {
        public StartScreenView()
        {
            InitializeComponent();
        }

        private void TB_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            TB.ScrollToEnd();
        }
    }
}
