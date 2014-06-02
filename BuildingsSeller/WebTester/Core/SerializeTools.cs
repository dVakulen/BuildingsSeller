
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace BuildSeller.Core
{

    public static class SerializeTools
    {

        public static string SerializeObject<T>(T objectToSerialize)
        {
            using (var memStm = new MemoryStream())
            {
                var serializer = new DataContractSerializer(typeof(T));
                serializer.WriteObject(memStm, objectToSerialize);

                memStm.Seek(0, SeekOrigin.Begin);

                using (var streamReader = new StreamReader(memStm))
                {
                    string result = streamReader.ReadToEnd();
                    return result;
                }
            }
        }

        public static T Deserialize<T>(string x)
        {
            var serializer = new DataContractSerializer(typeof(T));
            XmlReader streamReader = XmlReader.Create(new StringReader(x));
            var t = (T)serializer.ReadObject(streamReader);
            return t;
        }
    }
}
