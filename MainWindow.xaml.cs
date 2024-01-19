using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfXamlSettingsFile
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string selectedEnumValue=string.Empty;
        public MainWindow()
        {
            InitializeComponent();
            SettingsHelper.LoadSettings();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;

            if (comboBox.SelectedItem != null)
            {
                // Get the selected item's value as a string
                selectedEnumValue = ((ComboBoxItem)comboBox.SelectedItem).Content.ToString();
            }

        }

        private void GetValue_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void SetDefaultValue_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void BtnGetValue_Click(object sender, RoutedEventArgs e)
        {
            eXmlSettingsKeys enumValue = (eXmlSettingsKeys)Enum.Parse(typeof(eXmlSettingsKeys), selectedEnumValue);
            TxtGetValue.Text = SettingsHelper.GetValue(enumValue);
        }

        private void BtnSetDefaultValue_Click(object sender, RoutedEventArgs e)
        {
            eXmlSettingsKeys enumValue = (eXmlSettingsKeys)Enum.Parse(typeof(eXmlSettingsKeys), selectedEnumValue);
            bool result = SettingsHelper.SetValue(enumValue, TxtSetValue.Text);
        }
    }
}