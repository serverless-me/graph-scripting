using GraphScripting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            LPGraph.CreateClient();

            LPGraph.Query("g.V().drop()");
            LPGraph.Query("g.addV('user').property('id', 'u1').property('userhash', '1').property('division', 'C')");
            LPGraph.Query("g.addV('user').property('id', 'u2').property('userhash', '2').property('division', 'CI')");
            LPGraph.Query("g.addV('user').property('id', 'u3').property('userhash', '3').property('division', 'RBEV')");
            LPGraph.Query("g.addV('playlist').property('id', 'p1').property('userhash', '1').property('title', 'My Playlist A').property('edited', '" + DateTime.Now.ToString("yyyyMMdd-HHmmss") + "')");
            LPGraph.Query("g.addV('playlist').property('id', 'p2').property('userhash', '1').property('title', 'My Playlist B').property('edited', '" + DateTime.Now.ToString("yyyyMMdd-HHmmss") + "')");
            LPGraph.Query("g.addV('playlist').property('id', 'p3').property('userhash', '2').property('title', 'My Playlist C').property('edited', '" + DateTime.Now.ToString("yyyyMMdd-HHmmss") + "')");
            LPGraph.Query("g.addV('playlist').property('id', 'p4').property('userhash', '3').property('title', 'My Playlist D').property('edited', '" + DateTime.Now.ToString("yyyyMMdd-HHmmss") + "')");
            LPGraph.Query("g.V('u1').addE('owns').to(g.V('p1'))");
            LPGraph.Query("g.V('u1').addE('owns').to(g.V('p2'))");
            LPGraph.Query("g.V('u2').addE('owns').to(g.V('p3'))");
            LPGraph.Query("g.V('u3').addE('owns').to(g.V('p4'))");
            LPGraph.Query("g.addV('content').property('id', 'c1').property('userhash', '1').property('url', 'https://bosch.sharepoint.com/sites/EDT').property('list', 'listX').property('itemid', '1').property('title', 'My Content A').property('edited', '" + DateTime.Now.ToString("yyyyMMdd-HHmmss") + "')");
            LPGraph.Query("g.addV('content').property('id', 'c2').property('userhash', '1').property('url', 'https://bosch.sharepoint.com/sites/EDT').property('list', 'listX').property('itemid', '2').property('title', 'My Content AA').property('edited', '" + DateTime.Now.ToString("yyyyMMdd-HHmmss") + "')");
            LPGraph.Query("g.addV('content').property('id', 'c3').property('userhash', '2').property('url', 'https://bosch.sharepoint.com/sites/EDT').property('list', 'listX').property('itemid', '3').property('title', 'My Content AAA').property('edited', '" + DateTime.Now.ToString("yyyyMMdd-HHmmss") + "')");
            LPGraph.Query("g.addV('content').property('id', 'c4').property('userhash', '2').property('url', 'https://bosch.sharepoint.com/sites/EDT').property('list', 'listY').property('itemid', '1').property('title', 'My Content B').property('edited', '" + DateTime.Now.ToString("yyyyMMdd-HHmmss") + "')");
            LPGraph.Query("g.addV('content').property('id', 'c5').property('userhash', '3').property('url', 'https://bosch.sharepoint.com/sites/EDT').property('list', 'listY').property('itemid', '2').property('title', 'My Content BB').property('edited', '" + DateTime.Now.ToString("yyyyMMdd-HHmmss") + "')");
            LPGraph.Query("g.addV('content').property('id', 'c6').property('userhash', '3').property('url', 'https://bosch.sharepoint.com/sites/EDT').property('list', 'listY').property('itemid', '3').property('title', 'My Content BBB').property('edited', '" + DateTime.Now.ToString("yyyyMMdd-HHmmss") + "')");
            LPGraph.Query("g.V('p1').addE('contains').to(g.V('c1'))");
            LPGraph.Query("g.V('p1').addE('contains').to(g.V('c2'))");
            LPGraph.Query("g.V('p2').addE('contains').to(g.V('c1'))");
            LPGraph.Query("g.V('p2').addE('contains').to(g.V('c3'))");
            LPGraph.Query("g.V('p2').addE('contains').to(g.V('c4'))");
            LPGraph.Query("g.V('p3').addE('contains').to(g.V('c5'))");
            LPGraph.Query("g.V('p3').addE('contains').to(g.V('c6'))");
            LPGraph.Query("g.V('p4').addE('contains').to(g.V('c1'))");
            LPGraph.Query("g.V('p4').addE('contains').to(g.V('c2'))");
            LPGraph.Query("g.V('p4').addE('contains').to(g.V('c6'))");

            // Get Playlist p1
            LPGraph.Query("g.V('p1')");
            // Get all Contents that Playlist p1 contains
            LPGraph.Query("g.V('p1').out('contains')");
            // Get all other Playlists that contain the same contents as p1
            LPGraph.Query("g.V('p1').out('contains').in('contains').dedup().not(values('id').is('p1'))");
            // Get all contents that the other Playlists contain
            LPGraph.Query("g.V('p1').out('contains').in('contains').dedup().not(values('id').is('p1')).out('contains').dedup()");
            // Get all contents that only the other contents contain
            LPGraph.Query("g.V('p1').out('contains').store('p1contents').in('contains').dedup().not(values('id').is('p1')).out('contains').where(without('p1contents')).dedup()");
            // Order the contents by number of playlists in which they occure
            LPGraph.Query("g.V('p1').out('contains').store('p1contents').in('contains').dedup().not(values('id').is('p1')).out('contains').where(without('p1contents')).dedup().order().by(inE('contains').count(), decr)");
            // Limit the number of contents that are returned = Best Recommendation for 1 playlist
            LPGraph.Query("g.V('p1').out('contains').dedup().store('p1contents').in('contains').dedup().not(values('id').is('p1')).out('contains').where(without('p1contents')).dedup().order().by(inE('contains').count(), decr).limit(3)");
            // Get all contents from all playlists for a user, look for others playlists with the same contents, look for other contents, prioritize contents that are more often used
            LPGraph.Query("g.V('u1').out('owns').store('u1playlists').out('contains').dedup().store('u1contents').in('contains').where(without('u1playlists')).dedup().out('contains').where(without('u1contents')).dedup().order().by(inE('contains').count(), decr).limit(3)");
        }
    }
}
