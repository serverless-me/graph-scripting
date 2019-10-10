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
            LPGraph.Query("g.addV('playlist').property('id', 'p1').property('userhash', '1').property('title', 'My Playlist A').property('edited', '" + DateTime.Now.ToString("yyyyMMdd-HHmmss") + "')");
            LPGraph.Query("g.addV('playlist').property('id', 'p4').property('userhash', '2').property('title', 'My Playlist B').property('edited', '" + DateTime.Now.ToString("yyyyMMdd-HHmmss") + "')");
            LPGraph.Query("g.addV('playlist').property('id', 'p6').property('userhash', '3').property('title', 'My Playlist C').property('edited', '" + DateTime.Now.ToString("yyyyMMdd-HHmmss") + "')");
            LPGraph.Query("g.addV('playlist').property('id', 'p8').property('userhash', '4').property('title', 'My Playlist D').property('edited', '" + DateTime.Now.ToString("yyyyMMdd-HHmmss") + "')");
            LPGraph.Query("g.addV('content').property('id', 'c1').property('userhash', '1').property('url', 'https://bosch.sharepoint.com/sites/EDT').property('list', 'listX').property('itemid', '1').property('title', 'My Content A').property('edited', '" + DateTime.Now.ToString("yyyyMMdd-HHmmss") + "')");
            LPGraph.Query("g.addV('content').property('id', 'c2').property('userhash', '1').property('url', 'https://bosch.sharepoint.com/sites/EDT').property('list', 'listX').property('itemid', '2').property('title', 'My Content AA').property('edited', '" + DateTime.Now.ToString("yyyyMMdd-HHmmss") + "')");
            LPGraph.Query("g.addV('content').property('id', 'c3').property('userhash', '4').property('url', 'https://bosch.sharepoint.com/sites/EDT').property('list', 'listX').property('itemid', '3').property('title', 'My Content AAA').property('edited', '" + DateTime.Now.ToString("yyyyMMdd-HHmmss") + "')");
            LPGraph.Query("g.addV('content').property('id', 'c4').property('userhash', '4').property('url', 'https://bosch.sharepoint.com/sites/EDT').property('list', 'listY').property('itemid', '1').property('title', 'My Content B').property('edited', '" + DateTime.Now.ToString("yyyyMMdd-HHmmss") + "')");
            LPGraph.Query("g.addV('content').property('id', 'c5').property('userhash', '6').property('url', 'https://bosch.sharepoint.com/sites/EDT').property('list', 'listY').property('itemid', '2').property('title', 'My Content BB').property('edited', '" + DateTime.Now.ToString("yyyyMMdd-HHmmss") + "')");
            LPGraph.Query("g.addV('content').property('id', 'c6').property('userhash', '8').property('url', 'https://bosch.sharepoint.com/sites/EDT').property('list', 'listY').property('itemid', '3').property('title', 'My Content BBB').property('edited', '" + DateTime.Now.ToString("yyyyMMdd-HHmmss") + "')");
            LPGraph.Query("g.V('p1').addE('contains').to(g.V('c1'))");
            LPGraph.Query("g.V('p1').addE('contains').to(g.V('c2'))");
            LPGraph.Query("g.V('p4').addE('contains').to(g.V('c1'))");
            LPGraph.Query("g.V('p4').addE('contains').to(g.V('c3'))");
            LPGraph.Query("g.V('p4').addE('contains').to(g.V('c4'))");
            LPGraph.Query("g.V('p4').addE('contains').to(g.V('c6'))");
            LPGraph.Query("g.V('p6').addE('contains').to(g.V('c5'))");
            LPGraph.Query("g.V('p6').addE('contains').to(g.V('c6'))");
            LPGraph.Query("g.V('p8').addE('contains').to(g.V('c1'))");
            LPGraph.Query("g.V('p8').addE('contains').to(g.V('c2'))");
            LPGraph.Query("g.V('p8').addE('contains').to(g.V('c6'))");

            LPGraph.Query("g.V('p1')");
            LPGraph.Query("g.V('p1').out('contains').in('contains').dedup().not(values('id').is('p1')).out('contains')");
            LPGraph.Query("g.V('p1').out('contains').in('contains').out('contains').dedup()");
            LPGraph.Query("g.V('p1').out('contains').in('contains').dedup().not(values('id').is('p1')).out('contains').dedup()");
            LPGraph.Query("g.V('p1').out('contains').in('contains').dedup().not(values('id').is('p1')).out('contains').dedup().order().by('id', incr)");
            LPGraph.Query("g.V('p1').out('contains').in('contains').dedup().not(values('id').is('p1')).out('contains').dedup().order().by(inE('contains').count(), decr)");

        }
    }
}
