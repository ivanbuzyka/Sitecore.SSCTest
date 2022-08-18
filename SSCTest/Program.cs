using System;
using System.Net;
using System.Net.Http;
using System.Text;

namespace SSCTest
{
  internal class Program
  {
    static void Main(string[] args)
    {
      // this tools calls Sitecore Services Client:
      // first request: POST to the login endpoint /sitecore/api/ssc/auth/login
      // second request the one you can pass through parameters. Note - that should be an URL for GET request only
      if (args.Length == 0)
      {
        Console.WriteLine("No parameters passed.");
        Console.WriteLine("Pass arguments in following order: https://cmurl domain name username password urlToGETSubsequentlyAfterLogin");
        return;
      }

      var loginUrl = $"{args[0]}/sitecore/api/ssc/auth/login";
      var domain = args[1];
      var userName = args[2];
      var password = args[3];
      var secondRequestUrl = args[4];

      using (HttpClient client = new HttpClient())
      {
        StringContent authenticationjson = new StringContent("{ \"domain\": \"" + domain + "\", \"username\": \"" + userName + "\", \"password\": \"" + password + "\" }", Encoding.UTF8, "application/json");
        HttpResponseMessage authResult = client.PostAsync(loginUrl, authenticationjson).Result;

        if (authResult.StatusCode != HttpStatusCode.OK)
        {
          Console.WriteLine("Login NOt successfull");
          Console.WriteLine($"Login response status: {authResult.StatusCode}");          
        }

        HttpResponseMessage getItemResult = client.GetAsync(secondRequestUrl).Result;

        if (authResult.StatusCode != HttpStatusCode.OK)
        {
          Console.WriteLine($"Error when fetching the URL. Status: {authResult.StatusCode}");
        }

        var result = getItemResult.Content.ReadAsStringAsync().Result;

        Console.WriteLine($"Response content: {result}");
      }
    }
  }
}
