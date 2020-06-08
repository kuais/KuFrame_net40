using System;
using System.ComponentModel;
using System.Threading;

namespace Ku
{
    public class KuThread
    {
        public IProgress Listener;
        public bool IsStop { get; private set; } = false;
        public bool IsLoop { get; private set; } = false;

        public KuThread() { }
        public void Invoke(ISynchronizeInvoke o, Action action)
        {
            if (o.InvokeRequired) o.Invoke(action, null);
            else action();
        }
        public void Run(Action action, int delay = 0)
        {
            new Thread(() =>
            {
                OnStart();
                try
                {
                    Thread.Sleep(delay);
                    action?.Invoke();
                }
                catch(Exception ex)
                {
                    Listener?.OnError(ex);
                }
                OnStop();
            }).Start();
        }
        public void Loop(Action action, int interval = 0)
        {
            IsLoop = true;
            new Thread(() =>
            {
                OnStart();
                try
                {
                    while (IsLoop)
                    {
                        Thread.Sleep(interval);
                        action?.Invoke();
                    }
                }
                catch (Exception ex)
                {
                    Listener?.OnError(ex);
                }
                OnStop();
            }).Start();
        }
        public void StopLoop() { IsLoop = false; }
        public void WaitStop()
        {
            while (!IsStop)
            {
                try
                {
                    Thread.Sleep(1);
                }
                catch (Exception ex)
                {
                    Listener?.OnError(ex);
                }
            }
        }

        private void OnStart()
        {
            IsStop = false;
            Listener?.onStart();
        }
        private void OnStop()
        {
            IsStop = true;
            Listener?.onStop();
        }
    }
}