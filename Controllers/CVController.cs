using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Web;
using System.Linq;

public class CVController {
	const string subscriptionKey = "bd1177c4b63442f4b3db051dd71feb39";
	
	const string uriBase = "https://westeurope.api.cognitive.microsoft.com/vision/v1.0/analyze";

    public async Task<string> GetAsync(string uriImage)
    {
        HttpClient client = new HttpClient();

        // Request headers.
        client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);

        // Request parameters. A third optional parameter is "details".
        string requestParameters = "visualFeatures=Categories,Description,Color&language=en";

        // Assemble the URI for the REST API Call.
        string uri = uriBase + "?" + requestParameters;

        HttpResponseMessage response;

        var queryString = HttpUtility.ParseQueryString(string.Empty);

        // Request parameters
        queryString["visualFeatures"] = "Categories";
        //queryString["details"] = "{string}";
        queryString["language"] = "en";


        // Request body
        byte[] byteData = Encoding.UTF8.GetBytes("{\"url\":\"" + uriImage + "\"}");

        using (var content = new ByteArrayContent(byteData))
        {
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            response = await client.PostAsync(uri, content);

            string contentString = await response.Content.ReadAsStringAsync();
            var res = JsonConvert.DeserializeObject<CV.RootObject>(contentString);

            var message = "Captions: " + string.Join(";", res.description.captions.Select(x => x.text))
                    + "\nTags: "
                    + string.Join(",", res.description.tags);

            return message;
        }
    }
}