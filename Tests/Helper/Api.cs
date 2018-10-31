using RestSharp;

namespace Tests
{
    public static  class Api
    {
        public static IRestResponse Post(string url, object body)
        {
            RestClient client = new RestClient(url);
            RestRequest request = new RestRequest(Method.POST);

            request.AddHeader("Content-Type", "application/json");
            request.RequestFormat = DataFormat.Json;
            request.AddJsonBody(body);

            IRestResponse response = client.Execute(request);
            return response;
        }
    }
}
