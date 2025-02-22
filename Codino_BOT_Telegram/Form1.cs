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

        private void btnStart_Click(object sender, EventArgs e)
        {
            token = txtToken.Text;
            RunBot();
        }

        void RunBot()
        {

        }
    }
}
