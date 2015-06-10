using System.Collections.Generic;

namespace Movie_App.DataModel.RottenTomatoes
{
    public class RottenTomatoesClips
    {
        public IEnumerable<RootObject> rootObj { get; set; }
        public string title { get; set; }
        public string duration { get; set; }
        public string thumbnail { get; set; }
        public string alternateClip {get;set;}
        public class Links
        {
            public string alternate { get; set; }
        }

        public class Clip
        {
            public string title { get; set; }
            public string duration { get; set; }
            public string thumbnail { get; set; }
            public Links links { get; set; }
        }

        public class Links2
        {
            public string self { get; set; }
            public string alternate { get; set; }
            public string rel { get; set; }
        }

        public class RootObject
        {
            public List<Clip> clips { get; set; }
            public Links2 links { get; set; }
        }
    }
}