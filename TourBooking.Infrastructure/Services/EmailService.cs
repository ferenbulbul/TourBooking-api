using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using TourBooking.Application.Interfaces.Services;
using TourBooking.Domain.Comman;

namespace TourBooking.Infrastructure.Services
{
     public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;

        // IOptions pattern'i ile appsettings'ten ayarları alıyoruz.
        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var emailMessage = new MimeMessage();

            // GÖNDEREN BİLGİLERİ
            // SenderName (görünecek isim) ve From (gerçek e-posta adresi)
            emailMessage.From.Add(new MailboxAddress(_emailSettings.SenderName, _emailSettings.From));
            
            // ALICI BİLGİLERİ
            // Alıcının ismi olmadığı için ilk parametre boş
            emailMessage.To.Add(new MailboxAddress("", toEmail));

            // E-POSTA İÇERİĞİ
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart("html") // HTML formatında e-posta
            {
                Text = body
            };

            // SMTP İŞLEMLERİ
            using (var client = new SmtpClient())
            {
                try
                {
                    // 1. SMTP SUNUCUSUNA BAĞLAN
                    // Port 587 genellikle StartTls gerektirir. Bu, bağlantıyı güvenli hale getirir.
                    // Port 465 olsaydı SecureSocketOptions.SslOnConnect kullanırdık.
                    // Bağlantı kurulamazsa, buradaki seçeneği değiştirmek gerekebilir.
                    await client.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.Port, SecureSocketOptions.StartTls);
                    
                    // 2. KİMLİK DOĞRULA
                    // Not: Bazı sunucular Username olarak tam e-posta adresini ister.
                    // Eğer "TezGel" ile çalışmazsa, "info@fallinfal.com" ile denemek gerekebilir.
                    await client.AuthenticateAsync(_emailSettings.Username, _emailSettings.Password);

                    // 3. E-POSTAYI GÖNDER
                    await client.SendAsync(emailMessage);
                }
                catch (Exception ex)
                {
                    // Hata yönetimi çok önemli! Loglama yapılmalı.
                    // Geliştirme aşamasında hatayı görmek için fırlatabiliriz.
                    throw new InvalidOperationException($"E-posta gönderilemedi: {ex.Message}", ex);
                }
                finally
                {
                    // 4. BAĞLANTIYI KES
                    await client.DisconnectAsync(true);
                }
            }
        }
    }
}