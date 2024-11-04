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

namespace SocialNetworkWpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly string _email;
        public MainWindow(string email)
        {
            InitializeComponent();
            _email = email;
            MainContent.Content = new HomepageUserControl();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
        private void NavigateToHomepage(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new HomepageUserControl();
        }

        private void NavigateToProfile(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new ProfileUserControl();
        }

        private void NavigateToPeople(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new PeopleUserControl();
        }

        private void NavigateToPopular(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new PopularUserControl();
        }
    }
}