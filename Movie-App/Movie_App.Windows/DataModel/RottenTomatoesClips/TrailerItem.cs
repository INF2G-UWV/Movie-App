using System.Collections.Generic;

namespace Movie_App.DataModel.RottenTomatoesClips
{
    public class TrailerItem
    {
        public class Value
        {
            public int Publishedid { get; set; }
            public string Title { get; set; }
            public string DateCreated { get; set; }
            public string DateModified { get; set; }
            public int FirstReleasedYear { get; set; }
            public int GameCategoryId { get; set; }
            public int GameWarningId { get; set; }
            public int MediaId { get; set; }
            public string SongAlbumTitle { get; set; }
            public int SongCategoryId { get; set; }
            public int SongRiaaId { get; set; }
            public int SongWarningId { get; set; }
            public int PromotesPublishedId { get; set; }
            public int ProprietaryCustomerID { get; set; }
            public int StreamLengthinseconds { get; set; }
            public int MovieCategoryId { get; set; }
            public int MovieMpaaId { get; set; }
            public int MovieWarningId { get; set; }
            public double BoxOfficeInMillions { get; set; }
            public string MediaReceivedDate { get; set; }
            public object ExpirationDate { get; set; }
            public int CountryOforiginId { get; set; }
            public int LanguageId { get; set; }
            public string OfficialSiteUrl { get; set; }
            public bool OkToEncodeAndServe { get; set; }
            public bool IsATitle { get; set; }
            public int Sequence { get; set; }
            public int OverallSequence { get; set; }
            public string DisplayTitle { get; set; }
            public string NormalizedTitle { get; set; }
            public int TvCategoryid { get; set; }
            public int TvRatingId { get; set; }
            public int CopyrightholderId { get; set; }
            public int DirectorId { get; set; }
            public int Rank1Day { get; set; }
            public int Rank7Day { get; set; }
            public int Rank30Day { get; set; }
            public int RankAllTime { get; set; }
        }

          
            public List<Value> value { get; set; }
        
    }
}
