using System.Net.Http.Headers;
class Program
{
    static void Main(string[] args)
    {

        var baseURL = "http://localhost:5004";
        using HttpClient client = new();

        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        client.BaseAddress = new Uri(baseURL);

        UserInterface.run(args, client);
    }
}

