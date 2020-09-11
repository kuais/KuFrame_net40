using System;
using System.IO;
using System.IO.Ports;

namespace Ku.io.serial
{
    public class KuSerial
    {
        protected int _receivedMax = 1024;              //单次接收数据上限
        protected long _timeActived = 0;
        protected long _timeoutActived = 5000;
        protected Exception _lastError;
        protected SerialPort _sp;
        protected KuThread _rRead = new KuThread();

        #region 
        public int Baudrate { get; set; } = 9600;
        public string Port { get; set; } = "";
        public bool IsOpened { get; private set; } = false;
        public bool IsActivated => (_timeActived + _timeoutActived * 10000) > DateTime.Now.Ticks;
        public Exception LastError => _lastError;
        public KuBuffer Buffer { get; protected set; } = new KuBuffer();
        public Stream Stream => _sp.BaseStream;
        public ISerialListener SerialListener { get; set; }
        #endregion

        public void Open()
        {
            Close();
            _sp = new SerialPort(Port, Baudrate, 0);
            _rRead.Loop(TaskRead, 100);
            IsOpened = true;
        }
        public void Open(string port)
        {
            Port = port;
            Open();
        }
        public void Open(string port, int baudrate)
        {
            Baudrate = baudrate;
            Open(port);
        }
        public void Close()
        {
            _rRead.WaitStop();
            if (_sp != null) _sp.Close();
            _sp = null;
            IsOpened = false;
        }
        public void Write(byte[] datas)
        {
            _sp.Write(datas, 0, datas.Length);
            _timeActived = DateTime.Now.Ticks;
            OnWrote(datas);
        }
        protected void OnRead(byte[] datas)
        {
            if (SerialListener != null) SerialListener.OnRead(datas);
        }
        protected void OnWrote(byte[] datas)
        {
            if (SerialListener != null) SerialListener.OnWrote(datas);
        }
        protected void OnError(Exception ex)
        {
            _lastError = ex;
            if (SerialListener != null) SerialListener.OnError(ex);
        }

        private void TaskRead()
        {
            try
            {
                if (_sp.BytesToRead <= 0) return;
                _timeActived = DateTime.Now.Ticks;
                byte[] b = new byte[_receivedMax];
                int len = _sp.Read(b, 0, b.Length);
                byte[] datas = new byte[len];
                System.Buffer.BlockCopy(b, 0, datas, 0, len);
                Buffer.Put(datas);
                OnRead(datas);
            }
            catch (Exception ex)
            {
                OnError(ex);
            }
        }

        public interface ISerialListener : IError
        {
            void OnRead(byte[] data);
            void OnWrote(byte[] data);
        }
    }
}
