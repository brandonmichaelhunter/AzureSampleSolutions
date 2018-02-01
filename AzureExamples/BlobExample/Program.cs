using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.Configuration;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure;
namespace BlobExample
{
    class Program
    {
        static void Main(string[] args)
        {
            string containerName = "UnicornChoresProfileImagesContainer";
            string blobName = "UnicornChoresProfileImagesBlob";
            Console.WriteLine("Azure Blob Example - Connect to a Storage Account Programmatically");
            Console.WriteLine("");
            // Retrieve the storage account keys information.
            Console.WriteLine("Grabbing Azure Storage configuration information.");
            string storageconnection = ConfigurationManager.AppSettings.Get("StorageConnectionString");
            // Create an instance of the cloud storage account
            Console.WriteLine("Creating an instance of the CloudStorageAccount object.");
            CloudStorageAccount storageAcccount = CloudStorageAccount.Parse(storageconnection);
            // Create an instance of the CloudBlobClient which represents a client side representation of the a StorageBlob account. 
            Console.WriteLine("Creating a client to access my Storage Blob account");
            CloudBlobClient blobClient = storageAcccount.CreateCloudBlobClient();
            //Grab a reference of the Blob container called objective2
            Console.WriteLine("Create an reference point to a container in my storage account.");
            CloudBlobContainer container = blobClient.GetContainerReference(containerName.ToLower());
            //Since the container does not exists, let's create it
            container.CreateIfNotExists();
            //This will create a blob object within our container.
            Console.WriteLine("Create a reference to an blob object within the container.");
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(blobName.ToLower());
            UploadFile(blockBlob);

            //List the attributes of the container.
            container.FetchAttributes();
            Console.WriteLine("Container name " + container.StorageUri.PrimaryUri.ToString());
            Console.WriteLine("Last Modified " + container.Properties.LastModified.ToString());
            // Setting your own metadata
            container.Metadata.Clear();
            container.Metadata.Add("Author", "Brandon M. Hunter");
            container.Metadata["AuthoredOn"] = DateTime.Now.ToShortDateString();
            container.SetMetadata();//This will save the metadata to the container.

            //To confirm that metadata set
            container.FetchAttributes();
            foreach(var item in container.Metadata)
            {
                Console.WriteLine("Key " + item.Key);
                Console.WriteLine("Value " + item.Value);
            }

            Console.Read();
        }

        private static void UploadFile(CloudBlockBlob blockBlob)
        {
            string file = @"C:\Users\Brandon\Downloads\INV0796942_HUNTER_BRANDON_RCLDTQ.pdf";
            using (var fileStream = System.IO.File.OpenRead(file))
            {
                Console.WriteLine(string.Format("Uploading file {0} to my storage account.", file));
                blockBlob.UploadFromStream(fileStream);
                Console.WriteLine("File has been uploaded.");
            }
        }
    }
}
