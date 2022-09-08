using System.Text;
using Newtonsoft.Json;
using System.Net.Http.Headers;
namespace MainProgram;

class Authorization 
{
    static readonly HttpClient client = new HttpClient();

    public class ResponseClass
    {
        public string? guid { get; set; }
        public string? userName { get; set; }
        public string? fullName { get; set; }
        public string? token { get; set; }
    }

    static public async Task<string> Authorize(Dictionary<string, string> credentials)
    {
         try	
        {
            string payloadForFirstAuth = "{\"Username\":\"" + credentials["user"] + "\",\"Password\":\"" + credentials["password"] + "\"}";

            HttpContent content = new StringContent(payloadForFirstAuth, Encoding.UTF8, "application/json");

            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken token = source.Token;

            var responseBody = await client.PostAsync("https://my.chili-publish.com/api/v1/auth/login", content, token);

            string responseContent = await responseBody.Content.ReadAsStringAsync();   

            ResponseClass? responseContentJson = JsonConvert.DeserializeObject<ResponseClass>(responseContent);

            using (var requestMessage = new HttpRequestMessage(HttpMethod.Post, "https://my.chili-publish.com/api/v1/backoffice/generate")){
                if(responseContentJson is not null){

                    string payloadForSecondAuth = "{\"Url\":\""+ credentials["workAdminURL"] +"\"}";

                    requestMessage.Content = new StringContent(payloadForSecondAuth, Encoding.UTF8, "application/json");
                    requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", responseContentJson.token);

                    var responseBody2 = await client.SendAsync(requestMessage);
                    
                    var responseContent2 = await responseBody2.Content.ReadAsStringAsync();

                    ResponseClass? responseContentJson2 = JsonConvert.DeserializeObject<ResponseClass>(responseContent2);
                    
                    if(responseContentJson2 is not null){
                        Console.WriteLine("Login Token is: " + responseContentJson2.token);
                        if(responseContentJson2.token is not null) return responseContentJson2.token;
                    }
                }
            }

        }
        catch(HttpRequestException e)
        {
            Console.WriteLine("\nException Caught!");	
            Console.WriteLine("Message :{0} ",e.Message);
        }
        return "Failed";
    }
}