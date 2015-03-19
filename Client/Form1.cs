using Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
                Event1 ev = new Event1 { Id = 4, Name = "Paolo Costa" };

                if (!MessageQueue.Exists(string.Format(".\\Private$\\{0}", ev.GetType().FullName))) MessageQueue.Create(string.Format(".\\Private$\\{0}", ev.GetType().FullName));
                System.Messaging.Message msg = new System.Messaging.Message();
                msg.Body = ev;
                MessageQueue msgQ = new MessageQueue(string.Format(".\\Private$\\{0}", ev.GetType().FullName));
                msgQ.Send(msg);
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Event2 ev = new Event2 { Id = Guid.NewGuid(), Name = "Paolo Costa", CreationDate= DateTime.Now };
            if (!MessageQueue.Exists(string.Format(".\\Private$\\{0}", ev.GetType().FullName))) MessageQueue.Create(string.Format(".\\Private$\\{0}", ev.GetType().FullName));
            
            System.Messaging.Message msg = new System.Messaging.Message();
            msg.Body = ev;
            MessageQueue msgQ = new MessageQueue(string.Format(".\\Private$\\{0}", ev.GetType().FullName));
            msgQ.Send(msg);
        }
    }
}
