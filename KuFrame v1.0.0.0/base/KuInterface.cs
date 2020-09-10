using System;
using System.Collections.Generic;

namespace Ku
{
    public interface IDataHandler { void Handle(KuBuffer buffer); }
    public interface IError { void OnError(Exception ex); }
    public interface IEventTrigger{ void Trigger(string name, params object[] args); }
    public interface ILog { void AddLog(object text); }
    public interface ITask { }
    public interface ITaskHandler
    {
        void AddTask(ITask task);
        void RemoveTask(ITask task);
    }
    public interface IProtocol
    {
        byte[] Decode(KuBuffer input);
        List<byte[]> Encode(string name, params object[] args);
    }
    public interface IProgress: IError
    {
        void onStart();
        void onStop();
        void onProgress(long current, long max, params object[] args);
    }

}
