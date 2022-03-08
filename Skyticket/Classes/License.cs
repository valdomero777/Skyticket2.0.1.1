using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Windows.Forms;

namespace Skyticket
{
    public class License
    {
        internal static bool VerifyLicense(int clientID, string activationKey)
        {
            try
            {
                LicenseService.ctLicenciaBeans licenseFields = new LicenseService.ctLicenciaBeans();
                licenseFields.id_ciente = clientID;
                licenseFields.keylicencia = activationKey;
                licenseFields.id_cienteSpecified = true;
                

                LicenseService.ws_LicenciasClient client = new LicenseService.ws_LicenciasClient();
                var resp = client.licencia(licenseFields);
                if (resp.ToLower().Contains("activ"))
                    return true;
            }
            catch (Exception)
            {
                if (MessageBox.Show("License validation failed!", "Skyticket license", MessageBoxButtons.RetryCancel) == DialogResult.Retry)
                    return VerifyLicense(clientID, activationKey);
            }
            return false;
        }

        internal static bool VerifyClientTerminal(int clientID, int terminalID)
        {
            try
            {
                ClientValidationService.ctClienteBeans clientFields = new ClientValidationService.ctClienteBeans();
                clientFields.id_cliente = clientID;
                clientFields.id_terminal = terminalID;
                clientFields.id_clienteSpecified = true;
                clientFields.id_terminalSpecified = true;

                ClientValidationService.ws_valida_clienteClient client = new ClientValidationService.ws_valida_clienteClient();
                var resp = client.respuesta(clientFields);
                if (resp.ToLower().StartsWith("correct"))
                    return true;
            }
            catch (Exception)
            {
                if (MessageBox.Show("Client validation failed!", "Skyticket", MessageBoxButtons.RetryCancel) == DialogResult.Retry)
                    return VerifyClientTerminal(clientID, terminalID);
            }

            return false;
        }
    }
}