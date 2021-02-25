using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
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
                    UserName = "admin",
                    Password = "admin",
                    Port = 5672,
                };
                connection = factory.CreateConnection();
                channel = connection.CreateModel();
                //配置消费者预取消息长度 prefetchSize 和预取消息条数 prefetchCount
                channel.BasicQos(prefetchSize: 0,
                                 prefetchCount: 2,
                                 global: true);
                channel.ExchangeDeclare(exchange: "test-Exchange", type: ExchangeType.Fanout, durable: true, autoDelete: false, arguments: null);
                //定义消费者：进行消息接收处理
                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var msg = Encoding.UTF8.GetString(body.ToArray());
                    //处理消息
                    _ = Task.Run(() =>
                    {
                        if (process(msg))
                        {
                            channel.BasicAck(ea.DeliveryTag, false);
                        }
                        else
                        {
                            channel.BasicNack(ea.DeliveryTag, false, true);
                        }
                    });
                };
                channel.BasicConsume(queue: "test-Queue", consumer: consumer);

                button1.Enabled = false;
                button2.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"开启 MQ 监听失败，请重试！Error Message：{ex.Message}");
                CloseMQConnection();
            }
        }

        private bool process(string message)
        {
            try
            {
                Thread.Sleep(10000);
                listBox1.Items.Add($"{listBox1.Items.Count + 1}、{message}");
                return true;
            }
            catch (Exception ex)
            {
                _ = ex.Message;
                return false;
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
