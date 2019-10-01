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
    public class CreateTable
    {
        //string list<Tables> = new 
        void Main(string[] args)
        {
            /*String XmlDictionaryString = "C:\\XML\\B-760998_Exportacion.xml";
            CreateTableInstruction(XmlDictionaryString);*/
         }
        public string CreateTableInstruction(String XmlString)
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
                            Stored = Stored + "IF NOT EXISTS " +
                                " (SELECT[name] FROM sys.tables " +
                                " WHERE[name] ='tbl_" + reader.LocalName.ToLower() + "')" +
                                " CREATE TABLE tbl_" + reader.LocalName.ToLower() + "(";
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
            //Conectar(Stored);
            Console.WriteLine(Stored);
            Console.WriteLine(Stored.Length);
            return Stored;
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
