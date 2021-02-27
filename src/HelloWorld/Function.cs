using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;

using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;

using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace HelloWorld
{

    public class Function
    {

        private static readonly HttpClient client = new HttpClient();
        
        private async Task<string> ScanReadingListAsync()
        {
            AmazonDynamoDBClient client = new AmazonDynamoDBClient();
            Book book;

            var guid = Guid.NewGuid().ToString();

            Book bookToAdd = new Book
            {
                Id = guid,
                Title = "Caio teste 2",
                BookAuthors = new List<string>{ "Aline" },
                CoverPage = "isso ai",
                ISBN = 1233333
            };

            using (DynamoDBContext context = new DynamoDBContext(client))
            {
                await context.SaveAsync(bookToAdd);
                book = await context.LoadAsync<Book>(guid);
            }

            var bookString = JsonConvert.SerializeObject(book);
            return bookString;
        } 

        private async Task<APIGatewayProxyResponse> GetAsync(APIGatewayProxyRequest request, ILambdaContext context)
        {
            context.Logger.LogLine("Get Request\n");

            var response = new APIGatewayProxyResponse
            {
                StatusCode = (int)HttpStatusCode.OK,
                Body = await ScanReadingListAsync(),
                Headers = new Dictionary<string, string> { { "Content-Type", "text/plain" } }
            };

            return response;
        }
    }
}
