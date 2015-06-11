using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Movie_App.DataModel;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Movie_App
{
    /// <summary>
    ///     An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var buttonData = ((Button) e.OriginalSource).DataContext;
            NameStorage.MovieTitle = ((MovieData) buttonData).Title;
        }

        private void movieListview_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MoviesView.SelectedIndex = movieListview.SelectedIndex;
        }

        private void SearchBox_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            
        }

        private void MoviesView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            movieListview.SelectedIndex = MoviesView.SelectedIndex;
        }

        private void SearchBox_QueryChanged(Windows.UI.Xaml.Controls.SearchBox sender, Windows.UI.Xaml.Controls.SearchBoxQueryChangedEventArgs args)
        {
            
        }

        private void SearchBox_QueryChanged_1(Windows.UI.Xaml.Controls.SearchBox sender, Windows.UI.Xaml.Controls.SearchBoxQueryChangedEventArgs args)
        {
           
            
        }

        private void searchBoxTemp_QuerySubmitted(Windows.UI.Xaml.Controls.SearchBox sender, Windows.UI.Xaml.Controls.SearchBoxQuerySubmittedEventArgs args)
        {
            NameStorage.MovieTitle = args.QueryText;
        	this.Frame.Navigate(typeof(SearchResultsPage1), args.QueryText);
        }
    }
}