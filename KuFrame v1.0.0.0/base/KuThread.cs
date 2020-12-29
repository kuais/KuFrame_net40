using System;
using System.ComponentModel;
using System.Threading;

namespace Ku
{
    public class KuThread
    {
        public bool IsStop { get; private set; } = true;
        public bool IsLoop { get; private set; } = false;
        public bool IsPause { get; private set; } = false;

        public IProgress Listener;

        public KuThread() { }
        public static void Invoke(ISynchronizeInvoke o, Action action)
        {
            if (o.InvokeRequired)
                o.Invoke(action, null);
            else
                action();
        }
        public KuThread Run(Action action, int delay = 0)
        {
            new Thread(() =>
            {
                OnStart();
                try
                {
                    Thread.Sleep(delay);
                    action?.Invoke();
                }
                catch (Exception ex)
                {
                    Listener?.OnError(ex);
                }
                OnStop();
            }).Start();
            return this;
        }
        public KuThread Loop(Action action, int interval = 0)
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
                        if (IsPause) continue;
                        action?.Invoke();
                    }
                }
                catch (Exception ex)
                {
                    IsLoop = false;
                    Listener?.OnError(ex);
                }
                OnStop();
            }).Start();
            return this;
        }

        public void Pause(bool flag) { IsPause = flag; }
        public void StopLoop() { IsLoop = false; }
        public void WaitStop()
        {
            StopLoop();
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
            Listener?.OnStart();
        }
        private void OnStop()
        {
            IsStop = true;
            Listener?.OnStop();
        }
    }
}