using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using ZXing;
using ZXing.Common;
using ZXing.QrCode.Internal;

namespace Skyticket.Classes
{
    public class GenerateBarCode
    {
        public static Boolean BarCodeImg(string imgPath, string barCode)
        {
            string barpngpath = Settings.CurrentSettings.OutputPath + "\\barcode.png";

            try
            {
                BarcodeWriter writer = new BarcodeWriter()
                {
                    Format = BarcodeFormat.CODE_128,
                    Options = new EncodingOptions
                    {
                        Height = 200,
                        Width = 800,
                        PureBarcode = false,
                        Margin = 10,
                    },
                };

                var bitmap = writer.Write(barCode);
                bitmap.Save(barpngpath, System.Drawing.Imaging.ImageFormat.Png);
                bitmap.Dispose();
                return true; // there's no need to return a `FileContentResult` by `File(...);`

            }
            catch (Exception ex)
            {
                MainForm.UpdateLogBox(ex.Message);
                return false;
            }
        }

        public static string GetCode(string ticketText)
        {
            string barcode = "";
            try
            {
                Regex regex = new Regex(@"T001-\d{7}\b"); //ABC
                //Regex regex = new Regex(@"\b\d{24}\b"); //Calimax
                MatchCollection matches = regex.Matches(ticketText);

                foreach (Match match in matches)
                {
                    Console.WriteLine(match.Value);
                    barcode = match.Value;
                }
                MainForm.UpdateLogBox(barcode);
                BarCodeImg("", barcode);
            }
            catch (Exception ex) 
            {
                MainForm.UpdateLogBox(ex.Message);
            }
            return barcode;

        }
    }
}
