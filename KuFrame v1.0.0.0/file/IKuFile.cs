namespace Ku.file
{
    internal interface IKuFile
    {
        void Load(string path);
        void Save(string path = "");
    }
}
