using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConsumerApp
{
    public partial class Consumer : Form
    {
        private ConnectionFactory factory;
        private IConnection connection;
        private IModel channel;

        public Consumer()
        {
            InitializeComponent();
            button1.Enabled = true;
            button2.Enabled = false;
            CheckForIllegalCrossThreadCalls = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                factory = new ConnectionFactory()
                {
                    HostName = "localhost",//本地 rabbitmq 服务
                    UserName = "guest",
                    Password = "guest"
                };
                connection = factory.CreateConnection();
                channel = connection.CreateModel();
                channel.ExchangeDeclare(exchange: "SourceExchange", type: ExchangeType.Topic, durable: true, autoDelete: true, arguments: null);
                //定义消费者：进行消息接收处理
                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var msg = Encoding.UTF8.GetString(body.ToArray());
                    listBox1.Items.Add($"{listBox1.Items.Count + 1}、{msg}");
                };
                //处理消息
                channel.BasicConsume(queue: "testQueue", autoAck: true, consumer: consumer);

                button1.Enabled = false;
                button2.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"开启 MQ 监听失败，请重试！Error Message：{ex.Message}");
                CloseMQConnection();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            CloseMQConnection();
        }

        public void CloseMQConnection()
        {
            try
            {
                channel.ExchangeDelete("SourceExchange");
                channel.QueueDelete("testQueue");
                channel.Close();
                connection.Close();
                connection.Dispose();
            }
            catch (Exception ex)
            {
                _ = ex.Message;
            }
            listBox1.Items.Clear();
            button1.Enabled = true;
            button2.Enabled = false;
        }
    }
}
