namespace Ku.file
{
    interface IKuFile
    {
        void Load(string path);
        void Save(string path = "");
    }
}
