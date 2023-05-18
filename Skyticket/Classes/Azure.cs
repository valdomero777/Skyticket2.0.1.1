using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Skyticket.Classes
{
    public class Azure
    {
        public static async Task<bool> UploadImageAsync(string path)
        {
            string filename = Path.GetFileName(path);
            try
            {
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=skyticketstorage;AccountKey=DL05OV3pdweTGj1+7v3qI/bK8jcTOdiaIjcrg8pTFRHYX3fZXF1z0AI17I/lhP2DaARLnU+cOBaPOGb1neRCrg==;EndpointSuffix=core.windows.net");

                // Crear un cliente de Blob Storage
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

                // Obtener una referencia al contenedor donde deseas subir el archivo
                CloudBlobContainer container = blobClient.GetContainerReference("skytickets");

                // Crear el contenedor si no existe
                container.CreateIfNotExists();

                // Obtener una referencia al archivo que deseas subir
                
                CloudBlockBlob blob = container.GetBlockBlobReference(filename);

                // Subir el archivo al Blob Storage
                using (var fileStream = System.IO.File.OpenRead(path))
                {
                    blob.UploadFromStream(fileStream);
                }
                bool existe = await blob.ExistsAsync();
                if (existe)
                {
                    MainForm.UpdateLogBox("La imagen se ha subido al Blob Storage.");
                }
                else
                {
                    MainForm.UpdateLogBox("No se ha podido subir la imagen al Blob Storage.");
                }
                
                return true;
            }
            catch (Exception ex) 
            {
                MainForm.UpdateLogBox("AzureUploadError:" + ex.Message);
                return false;
            }

            
        }
        public static Boolean DownloadImage(string path, string destination)
        {
            try
            {
                string filename = Path.GetFileName(path);
              
                using (var client = new WebClient())
                {
                    // Descargar la imagen
                    byte[] bytes = client.DownloadData(path);

                    // Guardar la imagen en una carpeta

                  
                    string rutaCompleta = Path.Combine(destination, filename);
                    File.WriteAllBytes(rutaCompleta, bytes);
                }
                MainForm.UpdateLogBox("Se decargo del blob " + filename);
                return true;

            }
            catch (Exception ex) 
            {
                MainForm.UpdateLogBox("AzureDownloadError:"+ex.Message);
                return false;
            }
           

            
        }
    }
}
