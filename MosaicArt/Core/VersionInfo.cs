using MessagePack;

namespace MosaicArt.Core
{
    [MessagePackObject(true)]
    public class VersionInfo
    {
        /// <summary>
        /// バージョン
        /// このクラスのメンバーを変えたらこの値も変更する。
        /// </summary>
        public int Version { get; set; } = -1;
        public VersionInfo()
        {
        }
        public VersionInfo(int version)
        {
            Version = version;
        }
        public static VersionInfo Load(string path)
        {
            var bytes = File.ReadAllBytes(path);
            return MessagePackSerializer.Deserialize<VersionInfo>(bytes);
        }
    }
}