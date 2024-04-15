using Newtonsoft.Json;
using Npgsql;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Skyticket
{
    public class TerminalInfo
    {
        public string nombreSucursal { get; set; }
        public string nombreTerminal { get; set; }
        public bool Estado { get; set; }
        public int idsucursal { get; set; }
        public int idterminal { get; set; }
        public int id_cliente { get; set; }
        //***********************************//
        public static async Task<List<TerminalInfo>> LoadTerminals(string licenseKey)
        {
            List<TerminalInfo> infos = new List<TerminalInfo>();

            #region get Terminals
            try
            {

                var options = new RestClientOptions("https://skyticketapi.azurewebsites.net/")
                {
                    MaxTimeout = -1,
                };
                var client = new RestClient(options);
                var request = new RestRequest("/license?keylicense=" + licenseKey, Method.Get);
                RestResponse response = await client.ExecuteAsync(request);
                Console.WriteLine(response.Content);




                object[,] datos = JsonConvert.DeserializeObject<object[,]>(response.Content);

                // iterar sobre la matriz y crear un objeto TerminalInfo para cada elemento
                for (int i = 0; i < datos.GetLength(0); i++)
                {
                    // crear un nuevo objeto TerminalInfo
                    TerminalInfo terminal = new TerminalInfo();

                    // establecer los valores de las propiedades del objeto
                    terminal.nombreSucursal = datos[i, 0].ToString().Trim();
                    terminal.nombreTerminal = datos[i, 1].ToString().Trim();
                    terminal.Estado = (bool)datos[i, 2];
                    terminal.idsucursal = Convert.ToInt32(datos[i, 3]);
                    terminal.idterminal = Convert.ToInt32(datos[i, 4]);
                    terminal.id_cliente = Convert.ToInt32(datos[i, 5]);



                    // agregar el objeto TerminalInfo a la lista
                    infos.Add(terminal);
                }




            }
            catch (Exception ex)
            {
                //Console.WriteLine("remote DB Connection is: " + remoteConnection.State.ToString());
                Console.WriteLine("in LoadTerminals()2 " + ex.Message + " **** " + ex.StackTrace + " **** " + ex.InnerException);
            }
            #endregion

            return infos;
        }
        //***********************************//

    }
}