using System;
using System.IO;
using System.Xml;
using System.Text;
using System.Data.SqlClient;
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
            //XmlDeserializeTest();
            //BinarySerializeTest();
            //BinaryDeserializeTest();
            //JSONSerializeTest();
            //JSONDeserializeTest();

            System.Console.ReadLine();
        }
        
        private static void XmlSerializeTest()
        {
            /*
            UserDetail userDetail = new UserDetail
            {
                UserName = "Morgan",
                MailID = "Morgan@Domain.com"
            };

            using (var writer = XmlWriter.Create(@"C:\temp\userDetail.xml"))
                serializer.Serialize(writer, userDetail);
            */

            BlueConsolePrinter consolePrinter = new BlueConsolePrinter { Context = "PHD" };
            XmlSerializer serializer = new XmlSerializer(consolePrinter.GetType());

            StringBuilder sb = new StringBuilder();
            using (var writer = XmlWriter.Create(sb))
                serializer.Serialize(writer, consolePrinter);
            
            var xml = sb.ToString();

            using (SqlConnection cnn = new SqlConnection("Data Source=WayTooAwesome;Initial Catalog=Demos;Integrated Security=True"))
            {
                cnn.Open();

                string messageType = consolePrinter.GetType().AssemblyQualifiedName;
                string sql = "INSERT INTO tblSerializeXml VALUES (@AssemblyQualifiedName, @ObjectXML)";
                using (SqlCommand cmd = new SqlCommand(sql, cnn))
                {
                    cmd.Parameters.Add(new SqlParameter("AssemblyQualifiedName", messageType));
                    cmd.Parameters.Add(new SqlParameter("ObjectXML", xml));
                    cmd.ExecuteNonQuery();
                }
            }
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
          
            UserDetail userDetail = null;
            var serializer = new XmlSerializer(typeof(UserDetail));
            using (var reader = XmlReader.Create(@"C:\temp\userDetail.xml"))
                userDetail = (UserDetail)serializer.Deserialize(reader);
            */

            string messageType = null;
            string xml = null;

            using (SqlConnection cnn = new SqlConnection("Data Source=WayTooAwesome;Initial Catalog=Demos;Integrated Security=True"))
            {
                cnn.Open();

                string sql = "SELECT TOP 1 AssemblyQualifiedName, ObjectXML FROM tblSerializeXml";
                using (SqlCommand cmd = new SqlCommand(sql, cnn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            messageType = reader.GetString(0);
                            xml = reader.GetString(1);
                        }
                    }
                }
            }

            IConsolePrinter consolePrinter = null;
            var serializer = new XmlSerializer(Type.GetType(messageType));
            using (var stringReader = new StringReader(xml))
                using (var xmlReader = XmlReader.Create(stringReader))
                    consolePrinter = (IConsolePrinter)serializer.Deserialize(xmlReader);

            consolePrinter.Print();
        }
        
        static T ConvertXmlStringToObject<T>(string xmlString)
        {
            T classObject;

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            using (StringReader stringReader = new StringReader(xmlString))
                classObject = (T)xmlSerializer.Deserialize(stringReader);
            
            return classObject;
        }
        
        private static void BinarySerializeTest()
        {
            /*
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
            */

            BlueConsolePrinter consolePrinter = new BlueConsolePrinter { Context = "PHD" };
            byte[] data = null;
            using (MemoryStream ms = new MemoryStream())
            {
                var binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(ms, consolePrinter);
                ms.Seek(0, 0);
                data = ms.ToArray();
            }

            using (SqlConnection cnn = new SqlConnection("Data Source=WayTooAwesome;Initial Catalog=Demos;Integrated Security=True"))
            {
                cnn.Open();

                string messageType = consolePrinter.GetType().AssemblyQualifiedName;
                string sql = "INSERT INTO tblSerializeBinary VALUES (@AssemblyQualifiedName, @ObjectBinary)";
                using (SqlCommand cmd = new SqlCommand(sql, cnn))
                {
                    cmd.Parameters.Add(new SqlParameter("AssemblyQualifiedName", messageType));
                    cmd.Parameters.Add(new SqlParameter("ObjectBinary", data));
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private static void BinaryDeserializeTest()
        {
            /*
            UserDetail userDetail = null;
            using (Stream stream = File.Open(@"C:\temp\userDetail.dat", FileMode.Open))
            {
                var binaryFormatter = new BinaryFormatter();
                userDetail = (UserDetail)binaryFormatter.Deserialize(stream);
            }
            */

            string messageType = null;
            byte[] data = null;

            using (SqlConnection cnn = new SqlConnection("Data Source=WayTooAwesome;Initial Catalog=Demos;Integrated Security=True"))
            {
                cnn.Open();

                string sql = "SELECT TOP 1 AssemblyQualifiedName, ObjectBinary FROM tblSerializeBinary";
                using (SqlCommand cmd = new SqlCommand(sql, cnn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            messageType = reader.GetString(0);
                            data = (byte[])reader["ObjectBinary"];
                        }
                    }
                }
            }

            IConsolePrinter consolePrinter = null;            
            using (MemoryStream ms = new MemoryStream())
            {
                var binaryFormatter = new BinaryFormatter();
                ms.Write(data, 0, data.Length);
                ms.Seek(0, 0);
                consolePrinter = (IConsolePrinter)binaryFormatter.Deserialize(ms);
            }

            consolePrinter.Print();
        }

        private static void JSONSerializeTest()
        {
            /*
            UserDetail userDetail = new UserDetail
            {
                UserName = "mdo",
                MailID = "mdo9141@yahoo.com"
            };
            
            using (TextWriter writer = new StreamWriter(@"C:\temp\userDetail.json", false))
                writer.Write(JsonConvert.SerializeObject(userDetail));
            */

            RedConsolePrinter consolePrinter = new RedConsolePrinter { Context = "PHD" };
            using (SqlConnection cnn = new SqlConnection("Data Source=WayTooAwesome;Initial Catalog=Demos;Integrated Security=True"))
            {
                cnn.Open();

                string messageType = consolePrinter.GetType().AssemblyQualifiedName;
                string sql = "INSERT INTO tblSerializeJson VALUES (@AssemblyQualifiedName, @ObjectJSON)";
                using (SqlCommand cmd = new SqlCommand(sql, cnn))
                {
                    cmd.Parameters.Add(new SqlParameter("AssemblyQualifiedName", messageType));
                    cmd.Parameters.Add(new SqlParameter("ObjectJSON", JsonConvert.SerializeObject(consolePrinter)));
                    cmd.ExecuteNonQuery();
                }
            }
        }
    
        private static void JSONDeserializeTest()
        {
            /*
            UserDetail userDetail = null;

            using (TextReader reader = new StreamReader(@"C:\temp\userDetail.json"))
                userDetail = JsonConvert.DeserializeObject<UserDetail>(reader.ReadToEnd());   
            */

            string messageType = null;
            string json = null;

            using (SqlConnection cnn = new SqlConnection("Data Source=WayTooAwesome;Initial Catalog=Demos;Integrated Security=True"))
            {
                cnn.Open();

                string sql = "SELECT TOP 1 AssemblyQualifiedName, ObjectJSON FROM tblSerializeJson";
                using (SqlCommand cmd = new SqlCommand(sql, cnn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            messageType = reader.GetString(0);
                            json = reader.GetString(1);
                        }
                    }
                }
            }

            Type consoleType = Type.GetType(messageType);
            IConsolePrinter consolePrinter = (IConsolePrinter)JsonConvert.DeserializeObject(json, consoleType);
            consolePrinter.Print();
        }
    }
}
