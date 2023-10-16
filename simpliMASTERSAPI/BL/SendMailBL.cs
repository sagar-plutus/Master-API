using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using ODLMWebAPI.Models;
using System.IO;
using Microsoft.AspNetCore;
using System.Net;
using MailKit.Net.Smtp;
using MimeKit;
using System.Collections;
using System.Text;
using System.Data;
using ODLMWebAPI.DAL;
using ODLMWebAPI.StaticStuff;
using ODLMWebAPI.BL.Interfaces;
using System.Net.Mail;
using ODLMWebAPI.DAL.Interfaces;

namespace ODLMWebAPI.DAL
{
    #region sending the mail through the gmail account by vinod Thorat Dated:2/10/2017
     
    public class SendMailBL : ISendMailBL
    {
        private readonly ITblEmailConfigrationDAO _iTblEmailConfigrationDAO;
        public SendMailBL(ITblEmailConfigrationDAO iTblEmailConfigrationDAO)
        {
            _iTblEmailConfigrationDAO = iTblEmailConfigrationDAO;
        }
        public ResultMessage SendEmail(SendMail tblsendTO, String fileName)
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                #region Set configration of mail  
                TblEmailConfigrationTO DimEmailConfigrationTO = new TblEmailConfigrationTO();
                if (tblsendTO.locationId >0 )
                    DimEmailConfigrationTO = _iTblEmailConfigrationDAO.SelectDimEmailConfigrationIsActive(tblsendTO.locationId);
                else
                    DimEmailConfigrationTO = _iTblEmailConfigrationDAO.SelectDimEmailConfigrationIsActive();
                if (DimEmailConfigrationTO != null)
                {

                    tblsendTO.From = DimEmailConfigrationTO.EmailId;
                    tblsendTO.UserName = DimEmailConfigrationTO.EmailId;
                    tblsendTO.Password = DimEmailConfigrationTO.Password;
                    tblsendTO.Port = DimEmailConfigrationTO.PortNumber;
                }
                else
                {
                    resultMessage.DefaultBehaviour("DimEmailConfigrationTO Found Null");
                    resultMessage.DisplayMessage = "Error While Sending Email";
                    resultMessage.MessageType = ResultMessageE.Error;
                    return resultMessage;

                }
                #endregion
                var mimeMessage = new MimeMessage();
                mimeMessage.From.Add(new MailboxAddress(tblsendTO.FromTitle, tblsendTO.From));
                string[] sendList = tblsendTO.To.Split(',');
                foreach (string sentTo in sendList)
                {

                    mimeMessage.To.Add(new MailboxAddress(tblsendTO.ToTitle, sentTo));
                }

                //mimeMessage.Subject = "Regards New Visit Details ";
                mimeMessage.Subject = tblsendTO.Subject;
                var bodybuilder = new BodyBuilder();
                if (String.IsNullOrEmpty(tblsendTO.BodyContent))
                    bodybuilder.HtmlBody = "<h4>Dear Sir, </h4><p>I am sharing  Visit information with  you in regard to a new Visit Details that has been captured during  visit.   You may find the pdf file attached.</p><h4>Kind Regards,";
                else
                    bodybuilder.HtmlBody = tblsendTO.BodyContent;

                // mimeMessage.Body = bodybuilder.ToMessageBody();
                //byte[] bytes = System.Convert.FromBase64String(tblsendTO.Message.Replace("data:application/pdf;base64,", String.Empty));
                if (@tblsendTO.Attachements != null)
                {
                    byte[] bytes = new byte[32768];//Reshma Added
                    if (tblsendTO.Message.Contains ("data:application/pdf;base64,"))
                    {
                         bytes = System.Convert.FromBase64String(tblsendTO.Message.Replace("data:application/pdf;base64,", String.Empty));

                    }
                    else
                    {
                        tblsendTO.Message =  tblsendTO.Attachements;
                        string saveLocation = tblsendTO.Attachements;
                        bytes = DeleteFile(saveLocation, tblsendTO.Attachements); 
                        //var result = System.Text.Encoding.Unicode.GetBytes(tblsendTO.Message.ToString());
                        //string s = Convert.ToBase64String(result);
                        //byte[] newBytes = Convert.FromBase64String(s);
                        //bytes = System.Convert.FromBase64String(s);  
                    }
                    //byte[] bytes = System.Convert.FromBase64String(tblsendTO.Message.Replace("data:application/pdf;base64,", String.Empty));
                    bodybuilder.Attachments.Add(fileName, bytes, ContentType.Parse("application/pdf"));

                    //bodybuilder.Attachments.Add(@tblsendTO.Attachements);//Deepali commented
                }
                mimeMessage.Body = bodybuilder.ToMessageBody();
                using (MailKit.Net.Smtp.SmtpClient client = new MailKit.Net.Smtp.SmtpClient())
                {
                    //client.Connect("smtp.gmail.com", tblsendTO.Port, false);
                    client.Connect(string.IsNullOrWhiteSpace(DimEmailConfigrationTO.Host) ? "smtp.gmail.com" : DimEmailConfigrationTO.Host, tblsendTO.Port, false);
                    client.Authenticate(tblsendTO.UserName, tblsendTO.Password);
                    client.Send(mimeMessage);
                    client.Disconnect(true);
                    resultMessage.DefaultSuccessBehaviour();
                    resultMessage.DisplayMessage = "Email Sent Succesfully";
                    resultMessage.MessageType = ResultMessageE.Information;
                    return resultMessage;

                }

            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SendEmail");
                resultMessage.DisplayMessage = "Error While Sending Email";
                resultMessage.MessageType = ResultMessageE.Error;
                return resultMessage;

            }
            finally
            {

            }
        }
        public Byte[] DeleteFile(string saveLocation, string filePath)
        {
            String fileName1 = Path.GetFileName(saveLocation);
            Byte[] bytes = File.ReadAllBytes(filePath);
            if (bytes != null && bytes.Length > 0)
            {

                string resFname = Path.GetFileNameWithoutExtension(saveLocation);
                string directoryName;

                directoryName = Path.GetDirectoryName(saveLocation);
                string[] fileEntries = Directory.GetFiles(directoryName, "*Bill*");
                string[] filesList = Directory.GetFiles(directoryName, "*Bill*");

                foreach (string file in filesList)
                {
                    //if (file.ToUpper().Contains(resFname.ToUpper()))
                    {
                        File.Delete(file);
                    }
                }
            }
            return bytes;
        }

        public ResultMessage SendEmail(SendMail tblsendTO, TblEmailConfigrationTO DimEmailConfigrationTOExist = null)
          {

            ResultMessage res = new ResultMessage();
            try
             {

                #region Set configration of mail  
                TblEmailConfigrationTO DimEmailConfigrationTO = new TblEmailConfigrationTO();
                if (DimEmailConfigrationTOExist != null)
                {
                    DimEmailConfigrationTO = DimEmailConfigrationTOExist;
                }
                else
                {
                    DimEmailConfigrationTO = _iTblEmailConfigrationDAO.SelectDimEmailConfigrationIsActive();
                }

                if (DimEmailConfigrationTO != null)
                {
                
                        tblsendTO.From = DimEmailConfigrationTO.EmailId;
                        tblsendTO.UserName = DimEmailConfigrationTO.EmailId;
                        tblsendTO.Password = DimEmailConfigrationTO.Password;
                        tblsendTO.Port = DimEmailConfigrationTO.PortNumber;
                }
                else
                {
                     res.DefaultBehaviour("Failed to get Configuration in Send Mail");
                    return res;
                }
                #endregion
                string[] sendList = tblsendTO.To.Split(',');
               
                //Hrushikesh added new code for mail
                using (var client = new System.Net.Mail.SmtpClient(DimEmailConfigrationTO.Host, tblsendTO.Port))
                {
                    // Pass SMTP credentials
                    client.Credentials =
                        new NetworkCredential(tblsendTO.UserName, tblsendTO.Password);

                    // Enable SSL encryption
                    client.EnableSsl = true;

                    // Try to send the message. Show status in console.
                    try
                    {
                        MailMessage mail = new MailMessage();
                        mail.From = new MailAddress(DimEmailConfigrationTO.EmailId);
                        foreach (string sentTo in sendList)
                        {

                           mail.To.Add( sentTo);
                        }
                        mail.Subject = tblsendTO.Subject;
                        mail.IsBodyHtml = true;
                        if (String.IsNullOrEmpty(tblsendTO.BodyContent))
                            mail.Body = "<h4>Dear Client, </h4><p>We are contacting you in regard to a new Purchase Order that has been created on your account. You may find the Purchase Order attached.</p><h4>Kind Regards,</h4>";
                        else
                            mail.Body = tblsendTO.BodyContent;

                        if (!String.IsNullOrEmpty(tblsendTO.Attachements))
                        {
                            Attachment att = new Attachment(tblsendTO.Attachements);
                            mail.Attachments.Add(att);
                        }
                        client.Send(mail);
                        res.DefaultSuccessBehaviour();
                        return res;
                    }
                    catch (Exception ex)
                    {

                        res.DefaultBehaviour();
                        res.Exception = ex;
                        return res;
                       
                    }
                }
                //end
            }        
            catch (Exception ex)
                {
                res.DefaultBehaviour();
                res.Exception = ex;
                return res;
            }
            finally
            {

            }
        }
        //added by aniket
        public int SendEmailNotification(SendMail tblsendTO)
        {
            try
            {
                #region Set configration of mail  
                TblEmailConfigrationTO DimEmailConfigrationTO = _iTblEmailConfigrationDAO.SelectDimEmailConfigrationIsActive();

                if (DimEmailConfigrationTO != null)
                {

                    tblsendTO.From = DimEmailConfigrationTO.EmailId;
                    tblsendTO.UserName = DimEmailConfigrationTO.EmailId;
                    tblsendTO.Password = DimEmailConfigrationTO.Password;
                    tblsendTO.Port = DimEmailConfigrationTO.PortNumber;
                }
                else
                {
                    return -1;
                }
                #endregion
                var mimeMessage = new MimeMessage();
                mimeMessage.From.Add(new MailboxAddress(tblsendTO.FromTitle, tblsendTO.From));
                mimeMessage.To.Add(new MailboxAddress(tblsendTO.ToTitle, tblsendTO.To));
                mimeMessage.Subject = tblsendTO.Subject;
                var bodybuilder = new BodyBuilder();
                bodybuilder.HtmlBody = tblsendTO.BodyContent;
                //commented by aniket 
                //bodybuilder.HtmlBody = "<h4>Dear Client, </h4><p>We are contacting you in regard to a new invoice that has been created on your account. You may find the invoice attached.</p><h4>Kind Regards,</h4>";
                mimeMessage.Body = bodybuilder.ToMessageBody();
               // byte[] bytes = System.Convert.FromBase64String(tblsendTO.Message.Replace("data:application/pdf;base64,", String.Empty));
               // bodybuilder.Attachments.Add("Invoice.pdf", bytes, ContentType.Parse("application/pdf"));
                mimeMessage.Body = bodybuilder.ToMessageBody();
                using (MailKit.Net.Smtp.SmtpClient client = new MailKit.Net.Smtp.SmtpClient())
                {
                    //client.Connect("smtp.gmail.com", tblsendTO.Port, false);
                    client.Connect(DimEmailConfigrationTO.Host, tblsendTO.Port, false);
                    client.Authenticate(tblsendTO.UserName, tblsendTO.Password);
                   // client.SendAsync(mimeMessage); // added by aniket
                  client.Send(mimeMessage);
                  
                    client.Disconnect(true);
                    return 1;
                }

            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {

            }
        }
    }
    #endregion
}
