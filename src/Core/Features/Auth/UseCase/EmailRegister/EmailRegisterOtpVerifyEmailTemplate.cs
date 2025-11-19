using Core.Domain.Helper;
using Core.Domain.Interfaces;
using Core.Infrastructure.Email.Common;

namespace Core.Features.Auth.UseCase.EmailRegister;

public class EmailRegisterOtpVerifyEmailTemplate(EmailRegisterOtpVerifyEmailTemplate.Model model) : IEmailTemplate
{
    public string Subject => "OTP Verification for Prato Ai";
    private string TemplateString => $$"""
        <!DOCTYPE html>
        <html>
        <head>
            <style>
                body {
                    font-family: Arial, sans-serif;
                    background-color: #f9f9f9;
                    margin: 0;
                    padding: 0;
                }
                .email-container {
                    max-width: 600px;
                    margin: 20px auto;
                    background-color: #ffffff;
                    border: 1px solid #dddddd;
                    border-radius: 8px;
                    overflow: hidden;
                    box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
                }
                .header {
                    background-color: #DD6D01;
                    color: #ffffff;
                    text-align: center;
                    padding: 20px;
                }
                .content {
                    padding: 20px;
                    text-align: center;
                    color: #333333;
                }
                .otp {
                    font-size: 24px;
                    font-weight: bold;
                    color: #DD6D01;
                    margin: 20px 0;
                }
                .footer {
                    background-color: #f1f1f1;
                    text-align: center;
                    padding: 10px;
                    color: #777777;
                    font-size: 12px;
                }
            </style>
        </head>
        <body>
            <div class='email-container'>
                <div class='header'>
                    <h1>Your OTP Code</h1>
                </div>
                <div class='content'>
                    <p>Hello,</p>
                    <p>Use the following One-Time Password (OTP) to complete your process:</p>
                    <div class='otp'>{{model.OTP}}</div>
                    <p>This code will expire in {{model.OTPExpireInMinutes}} minutes. Please do not share this code with anyone.</p>
                </div>
                <div class='footer'>
                    <p>If you did not request this, please ignore this email.</p>
                    <p>&copy; {{DateTimeHelperStatic.UtcNow.Year}} Prato Ai. All rights reserved.</p>
                </div>
            </div>
        </body>
        </html>
        """;

    public EmailBody Body => EmailBody.CreateHtml(TemplateString);

    public class Model
    {
        public required string OTP { get; init; }
        public required int OTPExpireInMinutes { get; init; }
    }
}

