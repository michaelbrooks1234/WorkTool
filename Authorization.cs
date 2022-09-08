using System.Text;    
namespace MainProgram;

class Authorization 
{
    static readonly HttpClient client = new HttpClient();

    static public async Task<string> Authorize(Dictionary<string, string> credentials)
    {
         try	
        {
            string payload = "{\"Password: \"" + credentials["password"] + ", \"Username\": " + credentials["user"] + "";
            Console.WriteLine(payload);

            HttpContent content = new StringContent(payload, Encoding.UTF8, "application/json");

            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken token = source.Token;

            var responseBody = await client.PostAsync("https://my.chili-publish.com/api/v1/auth/login", content, token);

            Console.WriteLine(responseBody);
        }
        catch(HttpRequestException e)
        {
            Console.WriteLine("\nException Caught!");	
            Console.WriteLine("Message :{0} ",e.Message);
        }
        return "";
    }
}