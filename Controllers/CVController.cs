using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;


public class CVController {
	const string subscriptionKey = "bd1177c4b63442f4b3db051dd71feb39";
	
	const string uriBase = "https://westeurope.api.cognitive.microsoft.com/vision/v1.0/analyze";
	
	public async Task<string> GetAsync(byte[] byteData)
	{
                HttpClient client = new HttpClient();
        
                // Request headers.
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);
        
                // Request parameters. A third optional parameter is "details".
                string requestParameters = "visualFeatures=Categories,Description,Color&language=en";
        
                // Assemble the URI for the REST API Call.
                string uri = uriBase + "?" + requestParameters;
        
                HttpResponseMessage response;
        
                using (ByteArrayContent content = new ByteArrayContent(byteData))
                {
                    // This example uses content type "application/octet-stream".
                    // The other content types you can use are "application/json" and "multipart/form-data".
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
        
                    // Execute the REST API call.
                    response = await client.PostAsync(uri, content);
        
                    // Get the JSON response.
                    string contentString = await response.Content.ReadAsStringAsync();
                    var res = JsonConvert.DeserializeObject<CV.RootObject>(contentString);       

                    return string.Join(",",res.description.tags);
                }
        }
}