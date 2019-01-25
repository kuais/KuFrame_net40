using System;
using System.Threading;

namespace Ku
{
    public abstract class KuThread
    {
        public Action onStart;
        public Action onStop;
        public Action action;
        public int interval;
        public abstract void Start();
    }
    public class RunThread : KuThread
    {
        public RunThread()
        {
            interval = 0;
        }

        public override void Start()
        {
            ThreadStart s = delegate {
                onStart?.Invoke();
                Thread.Sleep(interval);
                action?.Invoke();
                onStop?.Invoke();
            };
            new Thread(s).Start();
            onStart?.Invoke();
        }
    }

    public class LoopThread : KuThread
    {
        public LoopThread()
        {
            interval = 1000;
        }

        bool flagRun = false;
        public override void Start()
        {
            ThreadStart s = delegate
            {
                onStart?.Invoke();
                while (flagRun)
                {
                    Thread.Sleep(interval);
                    action?.Invoke();
                }
                onStop?.Invoke();
            };
            flagRun = true;
            new Thread(s).Start();
        }
        public void Stop()
        {
            flagRun = false;
        }
    }
}