using HtmlAgilityPack;

namespace Movie_App.DataModel
{
    public class HTMLScraper
    { /*
        /// <summary>
        ///     Scrape movie data from www.google.com/movies.
        /// </summary>
        /// <param name="movie">Movie title to search for</param>   
        private async void Scrape(string movie, string location, int dateoffset)
        {
            //Set url
            var url = string.Format("http://google.com/movies?near={0}&q={1}&date={2}", location, movie, dateoffset);
            var webget = new HtmlWeb();
            var doc = await webget.LoadFromWebAsync(url);

            try
            {
                //See if exists
                if (doc.DocumentNode.SelectSingleNode("//h2[@itemprop = 'name']") != null)
                {
                    movieName = doc.DocumentNode.SelectSingleNode("//h2[@itemprop = 'name']").InnerText;
                }
                if (doc.DocumentNode.SelectSingleNode("//div[@class = 'theater']") != null)
                {
                    //Select all divs with class name theater
                    foreach (
                        var node in
                            doc.DocumentNode.SelectNodes("//div[@class = 'theater']"))
                    {
                        var item = new BiosItem();
                        //Get name, address and time from child divs
                        foreach (var t in node.ChildNodes)
                        {
                            //name
                            var xpath = node.XPath + "//div[@class = 'name']//a";
                            foreach (var childnode in t.SelectNodes(@xpath))
                            {
                                item.Name = childnode.InnerText;
                            }

                            //address
                            xpath = node.XPath + "//div[@class = 'address']";
                            foreach (var childnode in t.SelectNodes(@xpath))
                            {
                                item.Address = childnode.InnerText;
                            }

                            //get time of different showings
                            xpath = node.XPath + "//div[@class = 'times']";
                            foreach (var childnode in t.SelectNodes(@xpath))
                            {
                                item.Time = FormatTime(childnode.InnerText);
                            }
                        }
                        //Add to list
                        items.Add(item);
                    }
                    //Enable controls
                }
            }
            catch
            {
                //Uh Oh! Error! Really not found this time! Normally program would crash and burn!!
            }
        }*/
    }
}