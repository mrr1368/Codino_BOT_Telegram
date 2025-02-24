using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telegram.Bot;
using Telegram.Bot.Types;
using Color = System.Drawing.Color;

namespace Codino_BOT_Telegram
{
    public partial class Form1 : Form
    {
        private static string token;
        private TelegramBotClient bot;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private async void btnStart_Click(object sender, EventArgs e)
        {
            token = txtToken.Text;
            await RunBot();
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
                        sb.AppendLine(from.Username + "\nبه بات ما خوش آمدید");
                        sb.AppendLine("میتوانید از امکاناتی  که در اختیار شما قرار داده ایم استفاده کنید...");
                        sb.AppendLine("درباره ما : /AboutUs");
                        sb.AppendLine("تماس با ما : /ContactUs");
                        sb.AppendLine("آدرس ما : /Address");
                        await bot.SendMessage(chatid, sb.ToString());
                    }

                    else if (text.Contains("/aboutus"))
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.AppendLine("ما خیلی خوبیم");
                        await bot.SendMessage(chatid, sb.ToString());
                    }
                    
                    else if (text.Contains("/contactus"))
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.AppendLine("شماره تماس : 09354648853");
                        sb.AppendLine("ایمیل : mrrajabali@gmail.com");
                        await bot.SendMessage(chatid, sb.ToString());
                    }

                    else if (text.Contains("/address"))
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.AppendLine("مجموعه کدینو");
                        await bot.SendMessage(chatid, sb.ToString());
                    }

                }

                await Task.Delay(1000);
            }
        }
    }
}
