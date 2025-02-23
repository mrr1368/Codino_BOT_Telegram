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
                var me = await bot.GetMe();                     // (اول مطمئن شو که بات آنلاین شده) 
                this.Invoke(new Action(() => 
                {
                    lblstatus.Text = "Online";                  // فقط وقتی که آنلاین شد تغییر کنه
                    lblstatus.ForeColor = Color.Green;
                }));
            }
            catch (Exception ex)
            {
                this.Invoke(new Action(() =>
                {
                    lblstatus.Text = "Error";
                    lblstatus.ForeColor = Color.Red;
                }));

                MessageBox.Show($"خطا در راه اندازی بات : {ex.Message}" , "Error" , MessageBoxButtons.OK , MessageBoxIcon.Error);
            }
        }
    }
}
