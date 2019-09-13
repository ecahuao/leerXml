using System;
using System.Data;
using System.IO;
using System.Xml;

namespace LeerXML
{
    class Program
    {
        static void Main(string[] args)
        {
            XmlTextReader reader = new XmlTextReader("C:\\XML\\CP-1456814_Complemento_Pago.xml");
            //XmlTextReader reader = new XmlTextReader("books.xml");
            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        Console.WriteLine("<{0}>", reader.Name);
                        while (reader.MoveToNextAttribute()) // Read the attributes.
                            Console.WriteLine("         " + reader.Name + "='"/* + reader.Value + "'    " +  reader.ValueType.FullName*/);
                        break;
                    case XmlNodeType.Text:
                        Console.WriteLine(reader.Value);
                        break;
                    case XmlNodeType.CDATA:
                        Console.WriteLine("<![CDATA[{0}]]>", reader.Value);
                        break;
                    case XmlNodeType.ProcessingInstruction:
                        Console.WriteLine("<?{0} {1}?>", reader.Name, reader.Value);
                        break;
                    case XmlNodeType.Comment:
                        Console.WriteLine("<!--{0}-->", reader.Value);
                        break;
                    case XmlNodeType.XmlDeclaration:
                        Console.WriteLine("<?xml version='1.0'?>");
                        break;
                    case XmlNodeType.Document:
                        break;
                    case XmlNodeType.DocumentType:
                        Console.WriteLine("<!DOCTYPE {0} [{1}]", reader.Name, reader.Value);
                        break;
                    case XmlNodeType.EntityReference:
                        Console.WriteLine(reader.Name);
                        break;
                    case XmlNodeType.EndElement:
                        //Console.WriteLine("</{0}>", reader.Name);
                        break;
                }
            }
            Console.ReadLine();
        }
        
    }
}
