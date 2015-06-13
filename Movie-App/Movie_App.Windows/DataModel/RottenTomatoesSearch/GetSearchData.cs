namespace Movie_App.DataModel.RottenTomatoesSearch
{
    public class GetSearchData
    {
        public GetSearchData(string image, string title, string year)
        {
            Title = title;
            Image = image;
            Year = year;
        }

        public string Title { get; set; }
        public string Image { get; set; }
        public string Year { get; set; }
    }
}