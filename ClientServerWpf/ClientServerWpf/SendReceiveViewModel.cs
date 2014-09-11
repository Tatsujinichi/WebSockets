using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Mime;
using System.Text;
using System.Windows;
using System.Windows.Threading;

namespace ClientServerWpf
{
    public class SendReceiveViewModel
    {
        public ObservableCollection<string> ReceiveList { get; set; }
        public ObservableCollection<string> SendList { get; set; }
        public ObservableCollection<MessageServer> MessageServers { get; set; }
        private Queue<MessageServer> MessageServersQueue { get; set; }
        public MessageServer SelectedMessageServer { get; set; }
        public string Message { get; set; }

        public SendReceiveViewModel()
        {
            Message = "Enter text";
            ReceiveList = new ObservableCollection<string>();
            SendList = new ObservableCollection<string>();
            MessageServers = new ObservableCollection<MessageServer>();
            MessageServersQueue = new Queue<MessageServer>();
        }

        internal void Start()
        {
            var newServer = new MessageServer(555, 1024);
            MessageServers.Add(newServer);
            MessageServersQueue.Enqueue(newServer);
            newServer.ReceiveEvent += ServerReceiveEvent;
            newServer.Start();
        }

        private void ServerReceiveEvent(object sender, ByteArrayEventArgs e)
        {
            string decodedString = Encoding.UTF8.GetString(e.BufferBytes, e.Offset, e.Length);
            Application.Current.Dispatcher.BeginInvoke(((Action)(() => ReceiveList.Add(decodedString))));
        }

        public void Stop()
        {
            var serverToStop = MessageServersQueue.Dequeue();
            MessageServers.Remove(serverToStop);
            serverToStop.Stop();
            serverToStop.ReceiveEvent -= ServerReceiveEvent;
        }

        internal void Send()
        {
            if (SelectedMessageServer != null)
            {
                Application.Current.Dispatcher.BeginInvoke(((Action)(() => SendList.Add(Message))));
                SelectedMessageServer.Send(Message);
            }
        }
    }
}