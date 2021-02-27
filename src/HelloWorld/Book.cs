using System.Collections.Generic;
using Amazon.DynamoDBv2.DataModel;

namespace HelloWorld
{
    [DynamoDBTable("Books")]
    public class Book
    {
        [DynamoDBHashKey]
        public string Id { get; set; }

        public string Title { get; set; }
        public int ISBN { get; set; }

        [DynamoDBProperty("Authors")]
        public List<string> BookAuthors { get; set; }

        [DynamoDBIgnore]
        public string CoverPage { get; set; }
    }
}