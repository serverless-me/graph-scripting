using Gremlin.Net.Driver;
using Gremlin.Net.Driver.Exceptions;
using Gremlin.Net.Structure.IO.GraphSON;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphScripting
{
    public static class LPGraph
    {
        private static string hostname = "<your hostname>.gremlin.cosmos.azure.com";
        private static int port = 443;
        private static string authKey = "<your key>";
        private static string database = "<your database>";
        private static string collection = "<your collection>";
        private static GremlinClient gremlinClient;

        public static void CreateClient()
        {
            var gremlinServer = new GremlinServer(hostname, port, enableSsl: true,
                                                    username: "/dbs/" + database + "/colls/" + collection,
                                                    password: authKey);
            gremlinClient = new GremlinClient(gremlinServer, new GraphSON2Reader(), new GraphSON2Writer(), GremlinClient.GraphSON2MimeType);
        }


        // Starts a console application that executes every Gremlin query in the gremlinQueries dictionary. 
        public static void Query(string Query)
        {
            Console.WriteLine(String.Format("Running this query: {0}", Query));

            // Create async task to execute the Gremlin query.
            var resultSet = SubmitRequest(gremlinClient, new KeyValuePair<string, string>("NewQuery", Query)).Result;
            if (resultSet.Count > 0)
            {
                Console.WriteLine("\tResult:");
                foreach (var result in resultSet)
                {
                    // The vertex results are formed as Dictionaries with a nested dictionary for their properties
                    string output = JsonConvert.SerializeObject(result);
                    Console.WriteLine($"\t{output}");
                }
                Console.WriteLine();
            }

            // Print the status attributes for the result set.
            // This includes the following:
            //  x-ms-status-code            : This is the sub-status code which is specific to Cosmos DB.
            //  x-ms-total-request-charge   : The total request units charged for processing a request.
            PrintStatusAttributes(resultSet.StatusAttributes);
            Console.WriteLine();
        }

        private static Task<ResultSet<dynamic>> SubmitRequest(GremlinClient gremlinClient, KeyValuePair<string, string> query)
        {
            try
            {
                return gremlinClient.SubmitAsync<dynamic>(query.Value);
            }
            catch (ResponseException e)
            {
                Console.WriteLine("\tRequest Error!");

                // Print the Gremlin status code.
                Console.WriteLine($"\tStatusCode: {e.StatusCode}");

                // On error, ResponseException.StatusAttributes will include the common StatusAttributes for successful requests, as well as
                // additional attributes for retry handling and diagnostics.
                // These include:
                //  x-ms-retry-after-ms         : The number of milliseconds to wait to retry the operation after an initial operation was throttled. This will be populated when
                //                              : attribute 'x-ms-status-code' returns 429.
                //  x-ms-activity-id            : Represents a unique identifier for the operation. Commonly used for troubleshooting purposes.
                PrintStatusAttributes(e.StatusAttributes);
                Console.WriteLine($"\t[\"x-ms-retry-after-ms\"] : { GetValueAsString(e.StatusAttributes, "x-ms-retry-after-ms")}");
                Console.WriteLine($"\t[\"x-ms-activity-id\"] : { GetValueAsString(e.StatusAttributes, "x-ms-activity-id")}");

                throw;
            }
        }

        private static void PrintStatusAttributes(IReadOnlyDictionary<string, object> attributes)
        {
            Console.WriteLine($"\tStatusAttributes:");
            Console.WriteLine($"\t[\"x-ms-status-code\"] : { GetValueAsString(attributes, "x-ms-status-code")}");
            Console.WriteLine($"\t[\"x-ms-total-request-charge\"] : { GetValueAsString(attributes, "x-ms-total-request-charge")}");
        }

        public static string GetValueAsString(IReadOnlyDictionary<string, object> dictionary, string key)
        {
            return JsonConvert.SerializeObject(GetValueOrDefault(dictionary, key));
        }

        public static object GetValueOrDefault(IReadOnlyDictionary<string, object> dictionary, string key)
        {
            if (dictionary.ContainsKey(key))
            {
                return dictionary[key];
            }

            return null;
        }
    }
}
