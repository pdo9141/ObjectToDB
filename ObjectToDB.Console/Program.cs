using System.IO;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Newtonsoft.Json;
using ObjectToDB.Contracts;

namespace ObjectToDB.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            //XmlSerializeTest();
            XmlDeserializeTest();
            //BinarySerializeTest();
            BinaryDeserializeTest();
            //JSONSerializeTest();
            JSONDeserializeTest();
        }
        
        private static void XmlSerializeTest()
        {
            UserDetail userDetail = new UserDetail
            {
                UserName = "Morgan",
                MailID = "Morgan@Domain.com"
            };

            string xmlString = ConvertObjectToXMLString(userDetail);

            // Save C# class object into Xml file
            XElement xElement = XElement.Parse(xmlString);
            xElement.Save(@"C:\temp\userDetail.xml");
        }

        static string ConvertObjectToXMLString(object classObject)
        {
            string xmlString = null;
            XmlSerializer xmlSerializer = new XmlSerializer(classObject.GetType());
            using (MemoryStream memoryStream = new MemoryStream())
            {
                xmlSerializer.Serialize(memoryStream, classObject);
                memoryStream.Position = 0;
                xmlString = new StreamReader(memoryStream).ReadToEnd();
            }
            return xmlString;
        }

        private static void XmlDeserializeTest()
        {
            /*
            string xmlString = @"<UserDetail>
                            <UserName>Morgan</UserName>
                            <MailID>Morgan@Domain.com</MailID>
                            </UserDetail>";
            */

            XElement xmlObject = XElement.Load(@"C:\temp\userDetail.xml");
            string xmlString = xmlObject.ToString();

            UserDetail userDetail = (UserDetail)ConvertXmlStringtoObject<UserDetail>(xmlString);
        }
        
        static T ConvertXmlStringtoObject<T>(string xmlString)
        {
            T classObject;

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            using (StringReader stringReader = new StringReader(xmlString))
            {
                classObject = (T)xmlSerializer.Deserialize(stringReader);
            }
            return classObject;
        }
        
        private static void BinarySerializeTest()
        {
            UserDetail userDetail = new UserDetail
            {
                UserName = "pdo",
                MailID = "pdo9141@gmail.com"
            };

            using (Stream stream = File.Open(@"C:\temp\userDetail.dat", FileMode.Create))
            {
                var binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(stream, userDetail);
            }
        }

        private static void BinaryDeserializeTest()
        {
            UserDetail userDetail = null;
            using (Stream stream = File.Open(@"C:\temp\userDetail.dat", FileMode.Open))
            {
                var binaryFormatter = new BinaryFormatter();
                userDetail = (UserDetail)binaryFormatter.Deserialize(stream);
            }
        }

        private static void JSONSerializeTest()
        {
            UserDetail userDetail = new UserDetail
            {
                UserName = "mdo",
                MailID = "mdo9141@yahoo.com"
            };

            using (TextWriter writer = new StreamWriter(@"C:\temp\userDetail.json", false))
                writer.Write(JsonConvert.SerializeObject(userDetail));
        }
    
        private static void JSONDeserializeTest()
        {
            UserDetail userDetail = null;

            using (TextReader reader = new StreamReader(@"C:\temp\userDetail.json"))
                userDetail = JsonConvert.DeserializeObject<UserDetail>(reader.ReadToEnd());            
        }
    }
}
