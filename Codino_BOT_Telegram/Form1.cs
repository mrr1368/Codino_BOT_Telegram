using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Color = System.Drawing.Color;

namespace Codino_BOT_Telegram
{
    public partial class Form1 : Form
    {
        private static string token;
        private TelegramBotClient bot;
        private bool isBotRunning;
        private ReplyKeyboardMarkup mainKeyboardmarkup;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            mainKeyboardmarkup = new ReplyKeyboardMarkup();
            KeyboardButton[] row1 =
            {
                new KeyboardButton("درباره ما" + "\U00002764"),
                new KeyboardButton("تماس با ما" + "\U00002709")
            };
            KeyboardButton[] row2 =
            {
                new KeyboardButton("آدرس ما" + "\U0001F68C"),
                new KeyboardButton("نظرسنجی" + "\U0001F6A5")
            };
            mainKeyboardmarkup.Keyboard = new KeyboardButton[][]
            {
                row1,
                row2
            };
        }

        private async void btnStart_Click(object sender, EventArgs e)
        {
            token = txtToken.Text;

            if (isBotRunning) return;
            isBotRunning = true;
            await RunBot();
            isBotRunning = false;
        }

        async Task RunBot()
        {
            try
            {
                bot = new TelegramBotClient(token); 
                var me = await bot.GetMe();                     // (اول مطمئن شویم که بات آنلاین شده) 
                this.Invoke(new Action(() =>
                {
                    lblstatus.Text = "Online";                  // فقط وقتی که آنلاین شد تغییر کنه
                    lblstatus.ForeColor = Color.Green;
                }));
            }
            catch (Exception ex)                                // در صورت وقوع خطا هیچ کد بعدی اجرا نمیشه

            {
                this.Invoke(new Action(() =>
                {
                    lblstatus.Text = "Error";
                    lblstatus.ForeColor = Color.Red;
                }));

                MessageBox.Show($"خطا در راه اندازی بات : {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;                                         // از اینجا به بعد هیچ کدی اجرا نمیشه، متد کامل متوقف میشه
            }

            int offset = 0;

            while (true)
            {
                try
                {
                    Update[] updates = await bot.GetUpdates(offset);

                    foreach (var update in updates)
                    {
                        offset = update.Id + 1;

                        if (update.Message == null) continue;

                        var text = update.Message.Text.ToLower();
                        var from = update.Message.From;
                        var chatid = update.Message.Chat.Id;

                        if (text.Contains("/start"))
                        {
                            StringBuilder sb = new StringBuilder();
                            sb.AppendLine(from.Username + "\nبه بات ما خوش آمدید" + "\U00002764");
                            sb.AppendLine("میتوانید از امکاناتی  که در اختیار شما قرار داده ایم استفاده کنید...");
                            sb.AppendLine("درباره ما : /AboutUs");
                            sb.AppendLine("تماس با ما : /ContactUs");
                            sb.AppendLine("آدرس ما : /Address");
                            await bot.SendMessage(chatid, sb.ToString(), replyMarkup: mainKeyboardmarkup);
                          
                        }

                        else if (text.Contains("/aboutus") || text.Contains("درباره ما"))
                        {
                            StringBuilder sb = new StringBuilder();
                            sb.AppendLine("ما خیلی خوبیم");
                            await bot.SendMessage(chatid, sb.ToString());
                        }

                        else if (text.Contains("/contactus") || text.Contains("تماس با ما"))
                        {
                            StringBuilder sb = new StringBuilder();
                            sb.AppendLine("شماره تماس : 09354648853");
                            sb.AppendLine("ایمیل : mrrajabali@gmail.com");

                            ReplyKeyboardMarkup contactKeyboardMarkup = new ReplyKeyboardMarkup();
                            KeyboardButton[] row1 =
                            {
                                new KeyboardButton("تماس با مدیریت"),
                                new KeyboardButton("تماس با پشتیبانی"),
                                new KeyboardButton("تماس با واحد فروش")
                            };
                            KeyboardButton[] row2 =
                            {
                                new KeyboardButton("بازگشت")
                            };
                            
                            contactKeyboardMarkup.Keyboard = new KeyboardButton[][]
                            {
                                row1,
                                row2
                            };
                                
                            await bot.SendMessage(chatid, sb.ToString(), replyMarkup: contactKeyboardMarkup);
                        }

                        else if (text.Contains("/address") || text.Contains("آدرس ما"))
                        {
                            StringBuilder sb = new StringBuilder();
                            sb.AppendLine("مجموعه کدینو");
                            await bot.SendMessage(chatid, sb.ToString());
                        }

                        else if (text.Contains("بازگشت"))
                        {
                            await bot.SendMessage(chatid, "بازکشت به منوی اصلی",replyMarkup: mainKeyboardmarkup);
                        }

                        dgvReport.Invoke(new Action(() =>
                        {
                            dgvReport.Rows.Add(chatid, from.Username, text, update.Message.MessageId,
                                update.Message.Date.ToString("yyyy/MM/dd - HH:mm"));
                        }));

                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("خطایی پیش آمد لطفا بعدا دوباره امتحان کنید.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                await Task.Delay(2000);
            }
        }

        private async void btnSend_Click(object sender, EventArgs e)
        {
            if (dgvReport.CurrentRow != null)
            {
                int chatId = int.Parse(dgvReport.CurrentRow.Cells[0].Value.ToString());

                try
                {
                    await bot.SendMessage(chatId, txtMessage.Text, parseMode: ParseMode.Html, linkPreviewOptions: true);
                }

                catch (Telegram.Bot.Exceptions.ApiRequestException ex)
                {
                    MessageBox.Show($"خطا در ارسال پیام:\n{ex.Message}", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                catch (Exception ex)
                {
                    MessageBox.Show($"یک خطای نامشخص رخ داد:\n{ex.Message}", "خطای غیرمنتظره", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                txtMessage.Text = "";
            }
        }

        private void btnSelectFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();

            if (openFile.ShowDialog() == DialogResult.OK)
            {
                txtFilePath.Text = openFile.FileName;
            }
        }

        private async void btnSendPhoto_Click(object sender, EventArgs e)
        {
            if (dgvReport.CurrentRow != null)
            {
                int chatId = int.Parse(dgvReport.CurrentRow.Cells[0].Value.ToString());

                FileStream fileName = new FileStream(txtFilePath.Text, FileMode.Open);

                await bot.SendPhoto(chatId, photo: InputFile.FromStream(fileName, "image.jpg"), caption: txtMessage.Text);
                 
            }
        }
    }
}
