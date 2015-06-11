namespace Movie_App.DataModel.RottenTomatoesSearch
{
    public class GetSearchData
    {
        public GetSearchData(string image, string title, string year)
        {
            this.Title = title;
            this.Image = image;
            this.Year = year;
        }

        public string Title { get; set; }
        public string Image { get; set; }
        public string Year { get; set; }
     }
}