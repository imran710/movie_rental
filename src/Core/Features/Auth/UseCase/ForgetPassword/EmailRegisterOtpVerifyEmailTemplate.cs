using Core.Domain.Helper;
using Core.Domain.Interfaces;
using Core.Infrastructure.Email.Common;

namespace Core.Features.Auth.UseCase.ForgetPassword;

public class ForgetPasswordEmailTemplate(ForgetPasswordEmailTemplate.Model model) : IEmailTemplate
{
    public string Subject => "Password Reset OTP for Prato Ai";
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
                    background-color: #1a73e8;
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
                    color: #1a73e8;
                    margin: 20px 0;
                    padding: 10px;
                    background-color: #f0f4ff;
                    border-radius: 4px;
                    display: inline-block;
                }
                .footer {
                    background-color: #f1f1f1;
                    text-align: center;
                    padding: 10px;
                    color: #777777;
                    font-size: 12px;
                }
                a.button {
                    display: inline-block;
                    padding: 10px 20px;
                    margin-top: 20px;
                    color: #ffffff;
                    background-color: #1a73e8;
                    text-decoration: none;
                    border-radius: 4px;
                    font-weight: bold;
                }
                a.button:hover {
                    background-color: #1558b0;
                }
            </style>
        </head>
        <body>
            <div class='email-container'>
                <div class='header'>
                    <h1>Password Reset Request</h1>
                </div>
                <div class='content'>
                    <p>Hello,</p>
                    <p>You have requested to reset your password. Use the following One-Time Password (OTP) to proceed:</p>
                    <div class='otp'>{{model.OTP}}</div>
                    <p>This code will expire in {{model.OTPExpireInMinutes}} minutes. If you did not request a password reset, please ignore this email.</p>
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

