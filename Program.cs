namespace MainProgram;

class Program2
{
    static readonly HttpClient client = new HttpClient();

    static async Task Main()
    {

        string path = Directory.GetCurrentDirectory();
        string[] credentials = System.IO.File.ReadAllLines(@path+"/src/startupConfig.txt");
        Dictionary<string, string> credentialsDict = GetCredentials(credentials);

        await Authorization.Authorize(credentialsDict);
    }

    static Dictionary<string,string> GetCredentials(string[] credentials)
    {

        Dictionary<string, string> parsedCredentials = new Dictionary<string, string>();

        for(int i = 0; i < credentials.Length; i++){
            string[] splitCreds = credentials[i].Split("=");
            parsedCredentials.Add(splitCreds[0], splitCreds[1]);
        }

        return parsedCredentials;
    }
}
