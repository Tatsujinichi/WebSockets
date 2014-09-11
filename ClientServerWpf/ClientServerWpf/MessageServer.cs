using System;
using System.ComponentModel;
using System.Net;
using System.Net.Sockets;
using System.Text;
using ClientServerWpf.Annotations;

namespace ClientServerWpf
{
    public class MessageServer : IDisposable, INotifyPropertyChanged
    {
        private readonly TcpListener _listener;
        private NetworkStream _networkStream;
        private TcpClient _client;
        private byte[] _receiveBuffer;
        private bool _connected;

        public bool Connected
        {
            get { return _connected; }
            set
            {
                _connected = value; 
                OnPropertyChanged("Connected");
            }
        }

        public MessageServer(int port, int receiveBufferSize)
        {
            _listener = new TcpListener(IPAddress.Any, port);
            _receiveBuffer = new byte[receiveBufferSize];
        }

        internal void Start()
        {
            _listener.Start();
            _listener.BeginAcceptTcpClient(StartCallback, _listener);
        }

        private void StartCallback(IAsyncResult ar)
        {
            _client = _listener.EndAcceptTcpClient(ar);
            _listener.Stop();
            _networkStream = _client.GetStream();
            if (_networkStream.CanRead)
            {
                _networkStream.BeginRead(_receiveBuffer, 0, _receiveBuffer.Length, ReceiveCallback, null);
            }
            Connected = true;
            OnPropertyChanged("ConnectionStatus");
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            int length = _networkStream.EndRead(ar);
            ByteArrayEventArgs args = new ByteArrayEventArgs(_receiveBuffer, 0, length);
            OnReceiveEvent(args);
            _receiveBuffer = new byte[_receiveBuffer.Length];
        }

        internal void Stop()
        {
            _listener.Stop();
            _networkStream.Dispose();
            _client.Close();
            Connected = false;
        }

        public void Dispose()
        {
            Stop();
        }

        internal void Send(string message)
        {
            byte[] messageBytes = Encoding.UTF8.GetBytes(message);
            _networkStream.BeginWrite(messageBytes, 0, messageBytes.Length, SendCallBack, null);
        }

        private void SendCallBack(IAsyncResult ar)
        {
            _networkStream.EndWrite(ar);
        }

        public event EventHandler<ByteArrayEventArgs> ReceiveEvent;

        protected virtual void OnReceiveEvent(ByteArrayEventArgs e)
        {
            EventHandler<ByteArrayEventArgs> handler = ReceiveEvent;
            if (handler != null)
                handler(this, e);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}