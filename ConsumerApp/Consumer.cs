using log4net;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
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
        private readonly IBasicProperties properties;
        private readonly ILog logger;

        public Consumer()
        {
            InitializeComponent();
            logger = LogManager.GetLogger(typeof(Consumer));
            button1.Enabled = true;
            button2.Enabled = false;
            CheckForIllegalCrossThreadCalls = false;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            try
            {
                factory = new ConnectionFactory()
                {
                    HostName = "192.168.253.128",//rabbitmq server 地址，默认为本地 "localhost"
                    UserName = "admin",
                    Password = "123",
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
                        if (Process(msg))
                        {
                            channel.BasicAck(ea.DeliveryTag, false);
                        }
                        else
                        {
                            channel.BasicNack(ea.DeliveryTag, false, true);
                        }
                    });
                    AsyncProcessor(msg);
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

        private bool Process(string message)
        {
            try
            {
                Thread.Sleep(10000);
                listBox1.Items.Add($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] {message}");
                return true;
            }
            catch (Exception ex)
            {
                _ = ex.Message;
                return false;
            }
        }

        private void Button2_Click(object sender, EventArgs e)
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

        /// <summary>
        /// 使用线程池排队处理事务
        /// </summary>
        /// <param name="message"></param>
        private void AsyncProcessor(string message)
        {
            try
            {
                ThreadPool.SetMinThreads(1, 1);
                ThreadPool.SetMaxThreads(5, 5);
                WaitCallback callback = index =>
                 {
                     //监听线程执行时间
                     Stopwatch watch = new Stopwatch();
                     watch.Start();
                     logger.Info(string.Format("消费者接收消息：{0}", message));
                     watch.Stop();
                     TimeSpan timespan = watch.Elapsed;
                     logger.Info(string.Format("线程执行时间：{0} 毫秒", timespan.TotalMilliseconds));
                 };
                //在线程池中加入线程队列
                _ = ThreadPool.QueueUserWorkItem(callback);
            }
            catch (Exception ex)
            {
                _ = ex.Message;
            }
        }

    }
}
