using Newtonsoft.Json;
using System;
using System.Net;
using System.Text;


namespace DriveLingo.Services
{
    public static class EmailService
    {
        private static string emailJsApiUrl = "https://api.emailjs.com/api/v1.0/email/send";
        private static string serviceId = "service_drivelingo";
        private static string templateId = "template_w0czcsa";
        private static string userId = "7f6GWPVgA3ok7tUsF";
        private static string publicKey = "7f6GWPVgA3ok7tUsF";

        public static bool SendPasswordResetEmail(
            string toEmail,
            string newPassword
        )
        {
            try
            {
                var payload = new
                {
                    service_id = serviceId,
                    template_id = templateId,
                    user_id = userId,
                    template_params = new
                    {
                        email = toEmail,
                        new_password = newPassword
                    }
                };

                string jsonPayload = JsonConvert.SerializeObject(payload);

                using (var client = new WebClient())
                {
                    client.Headers[HttpRequestHeader.ContentType] = "application/json";
                    client.Encoding = Encoding.UTF8;

                    string response = client.UploadString(emailJsApiUrl, "POST", jsonPayload);

                    System.Diagnostics.Debug.WriteLine($"Password reset email sent successfully to {toEmail}. Response: {response}");
                    return true;
                }
            }
            catch (WebException webEx)
            {
                string responseDetails = string.Empty;
                if (webEx.Response != null)
                {
                    using (var reader = new System.IO.StreamReader(webEx.Response.GetResponseStream()))
                    {
                        responseDetails = reader.ReadToEnd();
                    }
                }
                System.Diagnostics.Debug.WriteLine($"Failed to send reset email. Status: {webEx.Status}. Details: {responseDetails}");
                return false;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error sending reset email: {ex.Message}");
                return false;
            }
        }
    }
}