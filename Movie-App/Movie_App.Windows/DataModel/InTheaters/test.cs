using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using Simple.OData.Client;

namespace Movie_App.DataModel.RottenTomatoesClips
{
    class test
    {
        public void get()
        {
            var client = new ODataClient(new ODataClientSettings("http://api.internetvideoarchive.com/2.0/DataService", new System.Net.NetworkCredential("14c9d1e0-329a-47d9-a29a-2d8268a67a48","")));
            var test = client.FindEntriesAsync()
            
         

        }
    }
}
