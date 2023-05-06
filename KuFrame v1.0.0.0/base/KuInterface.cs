using System;
using System.Collections.Generic;

namespace Ku
{
    public interface IError { void OnError(Exception ex); }
    public interface IEventTrigger { void Trigger(string name, params object[] args); }
    public interface ILog { void AddLog(object text); }
    public interface ITask { }
    public interface ITaskHandler
    {
        void AddTask(ITask task);
        void RemoveTask(ITask task);
    }
    public interface IProtocol
    {
        byte[] Decode(KuBuffer buf);
        List<byte[]> Encode(string name, params object[] args);
    }
    public interface IProgress : IError
    {
        void OnStart();
        void OnStop();
        void OnProgress(long current, long max, params object[] args);
    }

    //// TODO 将在后续移除
    //public interface IDataHandler { void Handle(KuBuffer buffer); }
}
