using System;
using System.Data;
using System.IO;
using System.Xml;
using System.Data.SqlClient;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Common;
using Microsoft.Win32;
using System.Windows;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Security;
//using System.Windows.Forms;

namespace LeerXML
{
    class Program
    {
        static void Main(string[] args)
        {
            String XmlDictionaryString = "C:\\XML\\B-760998_Exportacion.xml";
            CreateTable(XmlDictionaryString);
            InsertTable(XmlDictionaryString);
        }
        public static void Conectar(String script)
        {
            //using (SqlConnection cn = new SqlConnection("Server=tcp:sfnetlab.database.windows.net,1433;Initial Catalog=dbSoftHard;Persist Security Info=False;User ID=testuser;Password=Factory2020;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;")) ;
            string sqlConnectionString = @"Server=tcp:sfnetlab.database.windows.net,1433;Initial Catalog=dbSoftHard;Persist Security Info=False;User ID=testuser;Password=Factory2020;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            //string script = File.ReadAllText(@"E:\Project Docs\MX462-PD\MX756_ModMappings1.sql");
            SqlConnection conn = new SqlConnection(sqlConnectionString);
            Server server = new Server(new ServerConnection(conn));
            server.ConnectionContext.ExecuteNonQuery(script);
        }
        public static void InsertTable(String XmlString)
        {
            int i = 0;
            String Stored = "";
            XmlTextReader reader = new XmlTextReader(XmlString);
            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        //Console.WriteLine("<{0}>", reader.Name);
                        if (reader.AttributeCount != 0)
                        {
                            i = 0;
                            Stored = Stored+"INSERT INTO " + reader.LocalName +
                                " VALUES ("  ;
                            while (reader.MoveToNextAttribute()) // Read the attributes.
                            {
                                //Console.WriteLine("         " + reader.Name /* + "='" + reader.Value + "'    " /*+  reader.ValueType.FullName*/);
                                i = i + 1;
                                Stored = Stored + As(reader.Value)+reader.Value +As(reader.Value);
                                if (i != (reader.AttributeCount))
                                    Stored = Stored + ",";
                                /*else
                                    Stored = Stored + reader.LocalName;
                                //reader.ReadEndElement();*/
                            }
                            Stored = Stored + ")";
                        }
                        break;
                    case XmlNodeType.Text:
                        //Console.WriteLine(reader.Value);
                        break;
                    case XmlNodeType.CDATA:
                        //Console.WriteLine("<![CDATA[{0}]]>", reader.Value);
                        break;
                    case XmlNodeType.ProcessingInstruction:
                        //Console.WriteLine("<?{0} {1}?>", reader.Name, reader.Value);
                        break;
                    case XmlNodeType.Comment:
                        //Console.WriteLine("<!--{0}-->", reader.Value);
                        break;
                    case XmlNodeType.XmlDeclaration:
                        //Console.WriteLine("<?xml version='1.0'?>");
                        break;
                    case XmlNodeType.Document:
                        break;
                    case XmlNodeType.DocumentType:
                        //Console.WriteLine("<!DOCTYPE {0} [{1}]", reader.Name, reader.Value);
                        break;
                    case XmlNodeType.EntityReference:
                        //Console.WriteLine(reader.Name);
                        break;
                    case XmlNodeType.EndElement:
                        //Console.WriteLine("</{0}>", reader.Name);
                        {
                            break;
                        }
                }
                //Console.WriteLine(Stored);
                Stored = Stored + Environment.NewLine;
                //Console.ReadLine();
            }
            Console.WriteLine(Stored);
            Console.WriteLine(Stored.Length);

        }
        public static void CreateTable(String XmlString)

        {
                int i = 0;
                String Stored = "";
                XmlTextReader reader = new XmlTextReader(XmlString);
                //XmlTextReader reader = new XmlTextReader("books.xml");
                while (reader.Read())
                {
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element:
                            //Console.WriteLine("<{0}>", reader.Name);
                            if (reader.AttributeCount != 0)
                            {
                                i = 0;
                                Stored = Stored + "IF EXISTS " +
                                    " (SELECT[name] FROM sys.tables " +
                                    " WHERE[name] ='" + reader.LocalName + "')" +
                                    " DROP TABLE " + reader.LocalName +
                                    " CREATE TABLE " + reader.LocalName + "(";
                                while (reader.MoveToNextAttribute()) // Read the attributes.
                                {
                                    //Console.WriteLine("         " + reader.Name /* + "='" + reader.Value + "'    " /*+  reader.ValueType.FullName*/);
                                    i = i + 1;
                                    Stored = Stored + reader.LocalName + " " + Tipo(reader.Value);
                                    if (i != (reader.AttributeCount))
                                        Stored = Stored + ",";
                                    /*else
                                        Stored = Stored + reader.LocalName;
                                    //reader.ReadEndElement();*/
                                }
                                Stored = Stored + ")";
                            }
                            break;
                        case XmlNodeType.Text:
                            //Console.WriteLine(reader.Value);
                            break;
                        case XmlNodeType.CDATA:
                            //Console.WriteLine("<![CDATA[{0}]]>", reader.Value);
                            break;
                        case XmlNodeType.ProcessingInstruction:
                            //Console.WriteLine("<?{0} {1}?>", reader.Name, reader.Value);
                            break;
                        case XmlNodeType.Comment:
                            //Console.WriteLine("<!--{0}-->", reader.Value);
                            break;
                        case XmlNodeType.XmlDeclaration:
                            //Console.WriteLine("<?xml version='1.0'?>");
                            break;
                        case XmlNodeType.Document:
                            break;
                        case XmlNodeType.DocumentType:
                            //Console.WriteLine("<!DOCTYPE {0} [{1}]", reader.Name, reader.Value);
                            break;
                        case XmlNodeType.EntityReference:
                            //Console.WriteLine(reader.Name);
                            break;
                        case XmlNodeType.EndElement:
                            //Console.WriteLine("</{0}>", reader.Name);
                            {
                                break;
                            }
                    }
                    //Console.WriteLine(Stored);
                    Stored = Stored + Environment.NewLine;
                    //Console.ReadLine();
                }
            Stored = Stored + "";
            Conectar(Stored);
                Console.WriteLine(Stored);
                Console.WriteLine(Stored.Length);
        }
        public static string As(String Nodo)
        {
            string input = String.Empty;
            string Cadena = "'";
            try
            {
                float result = float.Parse(Nodo);
                Cadena = "";
                return Cadena;
                //Console.WriteLine(result);
            }
            catch (FormatException)
            {
                //Console.WriteLine($"Unable to parse '{Nodo}'");
            }
            try
            {
                DateTime result = DateTime.Parse(Nodo);
                Cadena = "'";
                //Console.WriteLine(result);
            }
            catch (FormatException)
            {
                //Console.WriteLine($"Unable to parse '{input}'");
            }

            return Cadena;

        }
        public static string Tipo(String Nodo)
        {
            string input = String.Empty;
            string Cadena = "VARCHAR";
            try
            {
                float result = float.Parse(Nodo);
                Cadena = "NUMERIC";
                return Cadena;
                //Console.WriteLine(result);
            }
            catch (FormatException)
            {
                //Console.WriteLine($"Unable to parse '{Nodo}'");
            }
            try
            {
                DateTime result = DateTime.Parse(Nodo);
                Cadena = "DATETIME";
                //Console.WriteLine(result);
            }
            catch (FormatException)
            {
                //Console.WriteLine($"Unable to parse '{input}'");
            }
            
            return Cadena;
        }
    }
}
