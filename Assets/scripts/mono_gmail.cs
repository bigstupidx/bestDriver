using UnityEngine;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using UnityEngine.UI;

public class mono_gmail : MonoBehaviour
{

  public InputField nome;
  public InputField email;
  public InputField subject;
  public InputField message;
   

  public void sendEmail()
  {
    MailMessage mail = new MailMessage();

    mail.From = new MailAddress(email.text);
    mail.To.Add("unitybestdriver@gmail.com");
    mail.Subject = subject.text;
    mail.Body = "Message from: "+ nome.text + "\n" 
      + "Email from: " + email.text + "\n"
      + "Message: \n " + message.text;

    SmtpClient smtpServer = new SmtpClient("smtp.gmail.com");
    smtpServer.Port = 587;
    smtpServer.Credentials = new System.Net.NetworkCredential("unitybestdriver@gmail.com", "Vini1986") as ICredentialsByHost;
    smtpServer.EnableSsl = true;
    ServicePointManager.ServerCertificateValidationCallback =
        delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        { return true; };
    smtpServer.Send(mail);

  }
}