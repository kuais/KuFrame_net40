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
        public bool IsStopping { get; private set; } = false;
        public IProgress Listener;

        private static long _sid = 0;
        private long id;
        public KuThread()
        {
            id = _sid;
            _sid++;
        }

        /// <summary>
        /// Run execution in UI thread
        /// </summary>
        /// <param name="o">Invoke object</param>
        /// <param name="action">action need to do</param>
        public static void Invoke(ISynchronizeInvoke o, Action action)
        {
            if (o.InvokeRequired)
                o.Invoke(action, null);
            else
                action();
        }

        /// <summary>
        /// Run execution once in new Thread
        /// </summary>
        /// <param name="action">action will do</param>
        /// <param name="delay">will run after deley[ms]</param>
        /// <returns></returns>
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
        /// <summary>
        /// Loop execution in new Thread
        /// </summary>
        /// <param name="action">action will do</param>
        /// <param name="interval">The interval[ms] between two actions</param>
        /// <returns></returns>
        public KuThread Loop(Action action, int interval = 0)
        {
            if (IsStopping) return this;
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
            IsStopping = true;
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
            IsStopping = false;
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