using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Web;
using System.Windows.Forms;

namespace Skyticket
{
    public class CodiAPI
    {
        internal static CodiInfo codiInfo;

        public static string token { get; set; }

        public static string ExecuteRequest(string address, string Data, string Method = "POST")
        {
            string ResponseFromServer = "";
            try
            {
                ServicePointManager.Expect100Continue = true;
                //ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)192 | (SecurityProtocolType)768 | (SecurityProtocolType)3072;

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(address);
                request.Method = Method;

                request.Timeout = 30 * 1000;
                request.ReadWriteTimeout = 30 * 1000;

                request.Accept = "application/json";
                request.ContentType = "application/json";
                request.ContentLength = Data.Length;

                if (Method != "GET")
                    using (System.IO.Stream s = request.GetRequestStream())
                    {
                        using (System.IO.StreamWriter sw = new System.IO.StreamWriter(s))
                        {
                            sw.Write(Data);
                            sw.Flush();
                        }
                    }

                using (System.IO.Stream s = request.GetResponse().GetResponseStream())
                {
                    using (System.IO.StreamReader sr = new System.IO.StreamReader(s))
                    {
                        ResponseFromServer = sr.ReadToEnd();
                    }
                }

                return ResponseFromServer;
            }
            catch (WebException wex)
            {
                if (wex.Response != null)
                {
                    using (var errorResponse = (HttpWebResponse)wex.Response)
                    {
                        using (var reader = new StreamReader(errorResponse.GetResponseStream()))
                        {
                            ResponseFromServer = reader.ReadToEnd();
                        }
                    }
                }
                else
                    ResponseFromServer = wex.Message;
                return ResponseFromServer;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        //************************************//
        public static void IgnoreBadCertificates()
        {
            System.Net.ServicePointManager.ServerCertificateValidationCallback
             = new System.Net.Security.RemoteCertificateValidationCallback(AcceptAllCertifications);
        }
        //************************************//
        public static bool AcceptAllCertifications
        (
            object sender,
            System.Security.Cryptography.X509Certificates.X509Certificate certification,
            System.Security.Cryptography.X509Certificates.X509Chain chain,
            System.Net.Security.SslPolicyErrors sslPolicyErrors
        )
        {
            return true;
        }
        //************************************//
        public static bool GetAccessToken(out string message)
        {
            bool retVal = false;
            message = "";
            token = "";

            var client = new RestClient(codiInfo.codiurlws + "/oauth/token");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            string svcCredentials = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(codiInfo.user_codi + ":" + codiInfo.pass_codi));
            request.AddHeader("Authorization", "Basic "+ svcCredentials);
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddParameter("username", codiInfo.user_body);
            request.AddParameter("password", codiInfo.password_body);
            request.AddParameter("grant_type", "password");

            try
            {
                IRestResponse resp = client.Execute(request);
                string response = resp.Content;

                JObject responseObj = (JObject)JsonConvert.DeserializeObject(response);
                token = responseObj["access_token"].ToString();
                retVal = true;
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            return retVal;
        }
        //************************************//
        public static bool GenerateCodiPayment(double amount, string phone, CodiPaymentType paymentType, out string id, out string qrStr)
        {
            bool retVal = false;
            id = "";
            qrStr = "";

            string url = codiInfo.codiurlws;
            if (paymentType == CodiPaymentType.BankAppPayment)
                url += "/notificaciones-push/";
            else if(paymentType == CodiPaymentType.ScreenQR || paymentType == CodiPaymentType.WhatsappQR)
                url += "/codigos-qr/";
            var client = new RestClient(url);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", "Bearer " + token);

            JObject paymentObj = new JObject();
            paymentObj.Add("idComercio", codiInfo.idComercio);
            paymentObj.Add("idCuentaBancaria", codiInfo.idCuentaBancaria);
            paymentObj.Add("concepto", "payment test");
            paymentObj.Add("monto", amount.ToString("0.00"));
            paymentObj.Add("referenciaNumerica", Settings.CurrentSettings.TerminalID + Settings.CurrentSettings.CodiRefSequence.ToString());
            Settings.CurrentSettings.CodiRefSequence++;
            Settings.SaveSettings();

            paymentObj.Add("tipoPago", 20);

            TimeSpan epochTime = DateTime.UtcNow.AddMinutes(10) - new DateTime(1970, 1, 1);
            
            paymentObj.Add("fechaVencimiento", (long)epochTime.TotalMilliseconds);//to do: calculate timestamp based on X minutes from now, specified in settings
            paymentObj.Add("celularComprador", phone);
            string payloadStr = paymentObj.ToString();
            payloadStr = payloadStr.Replace("\r\n", "");

            request.AddParameter("application/json", payloadStr, ParameterType.RequestBody);
            try
            {
                IRestResponse resp = client.Execute(request);
                string response = resp.Content;

                id = resp.StatusCode.ToString();


                JObject responseObj = (JObject)JsonConvert.DeserializeObject(response);

                qrStr = "";

                if (responseObj != null)
                {
                    if (responseObj.ContainsKey("codigoResultado"))
                    {
                        if (responseObj["codigoResultado"].ToString() == "0")
                        {
                            id = responseObj["idMensaje"].ToString();
                            retVal = true;
                        }
                    }

                    if (responseObj.ContainsKey("imageMCQR"))
                    {
                        qrStr = responseObj["imageMCQR"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                id = ex.Message;
            }

            return retVal;
        }
        //************************************//
        public static bool GetPaymentStatus(string idcobro, CodiPaymentType paymentType,out string status, out string idcodi)
        {
            bool retVal = false;
            status = "";
            idcodi = "";

            string url = codiInfo.codiurlws;
            if (paymentType == CodiPaymentType.BankAppPayment)
                url += "/notificaciones-push/{0}/{1}";
            else if (paymentType == CodiPaymentType.ScreenQR || paymentType == CodiPaymentType.WhatsappQR)
                url += "/codigos-qr/{0}/{1}";


            url = string.Format(url, codiInfo.idComercio, idcobro);
            var client = new RestClient(url);

            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", "Bearer " + token);

            try
            {
                IRestResponse resp = client.Execute(request);
                string response = resp.Content;

                status = resp.StatusCode.ToString();

                JObject responseObj = (JObject)JsonConvert.DeserializeObject(response);

                if (responseObj != null)
                {
                    if (paymentType == CodiPaymentType.ScreenQR || paymentType == CodiPaymentType.WhatsappQR)
                    {
                        if (responseObj.ContainsKey("listQR"))
                        {
                            if (responseObj["listQR"].Count() > 0)
                            {
                                status = responseObj["listQR"][0]["estadoCobro"].ToString();
                                idcodi = responseObj["listQR"][0]["idTransaccion"].ToString();
                                retVal = true;
                            }
                        }
                    }
                    else if (paymentType == CodiPaymentType.BankAppPayment)
                    {
                        if (responseObj.ContainsKey("estadoMensaje"))
                        {
                            status = responseObj["estadoMensaje"].ToString();
                            retVal = true;
                        }
                    }

                    
                }
            }
            catch (Exception ex)
            {
                status = ex.Message;
            }

            return retVal;
        }
        //************************************//
        public static bool CancelPayment(string id, out string status)
        {
            bool retVal = false;
            status = "";

            string address = "/getCobroCodi/{0}/{1}";
            address = string.Format(address, id, codiInfo.idComercio);
            var client = new RestClient(codiInfo.codiurlws + address);

            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", "Bearer " + token);

            //JObject paymentObj = new JObject();
            //paymentObj.Add("idComercio", codiInfo.idComercio);
            //paymentObj.Add("idCuentaBancaria", codiInfo.idCuentaBancaria);

            //string payloadStr = paymentObj.ToString();
            //payloadStr = payloadStr.Replace("\r\n", "");

            //request.AddParameter("application/json", payloadStr, ParameterType.RequestBody);
            try
            {
                IRestResponse resp = client.Execute(request);
                string response = resp.Content;

                status = resp.StatusCode.ToString();

                JObject responseObj = (JObject)JsonConvert.DeserializeObject(response);

                if (responseObj != null)
                {
                    if (responseObj.ContainsKey("estatus"))
                    {
                        status = responseObj["estatus"].ToString();
                        retVal = true;
                    }
                }
            }
            catch (Exception ex)
            {
                status = ex.Message;
            }

            return retVal;
        }
    }
}
