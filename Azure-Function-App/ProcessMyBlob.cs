using System;
using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace Company.Function
{
    public static class ProcessMyBlob
    {
        [FunctionName("ProcessMyBlob")]
        public static void Run(
            [BlobTrigger("demo/{name}", Connection = "azureblobraju001")]Stream myBlob, 
            [Blob("output/{name}", FileAccess.Write, Connection = "azureblobraju001")]Stream outputBlob, 
        string name, ILogger log)
        {
            var len= myBlob.Length;
            myBlob.CopyTo(outputBlob);
            log.LogInformation($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {myBlob.Length} Bytes");
        }
    }
}
