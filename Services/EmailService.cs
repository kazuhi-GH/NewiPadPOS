namespace NewiPadPOS.Services
{
    public interface IEmailService
    {
        Task<bool> SendEmailAsync(string toEmail, string subject, string htmlBody);
        Task<bool> SendReceiptAsync(string toEmail, string receiptHtml);
    }
    
    public class EmailService : IEmailService
    {
        private readonly ILogger<EmailService> _logger;
        
        public EmailService(ILogger<EmailService> logger)
        {
            _logger = logger;
        }
        
        public async Task<bool> SendEmailAsync(string toEmail, string subject, string htmlBody)
        {
            try
            {
                // メール送信のシミュレーション
                await Task.Delay(1000); // 1秒の処理時間をシミュレート
                
                _logger.LogInformation("メール送信シミュレーション完了");
                _logger.LogInformation($"宛先: {toEmail}");
                _logger.LogInformation($"件名: {subject}");
                _logger.LogInformation($"本文サイズ: {htmlBody.Length} 文字");
                
                // 実際のメール送信サービス（SendGrid、SMTP等）との統合はここで行う
                // 例: await _sendGridClient.SendEmailAsync(message);
                
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "メール送信エラー: {Email}", toEmail);
                return false;
            }
        }
        
        public async Task<bool> SendReceiptAsync(string toEmail, string receiptHtml)
        {
            var subject = "📧 電子レシート - カフェ iPad POS";
            return await SendEmailAsync(toEmail, subject, receiptHtml);
        }
    }
}