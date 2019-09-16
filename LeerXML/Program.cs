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
            int i = 0;
            String Stored = "CREATE PROCEDURE uspProductList AS BEGIN";


            XmlTextReader reader = new XmlTextReader("C:\\XML\\CP-1456814_Complemento_Pago.xml");
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
                                " WHERE[name] =" + reader.LocalName+ ")" +
                                " CREATE TABLE "+ reader.LocalName + "(";
                            while (reader.MoveToNextAttribute()) // Read the attributes.
                            {
                                //Console.WriteLine("         " + reader.Name /* + "='" + reader.Value + "'    " /*+  reader.ValueType.FullName*/);
                                i = i + 1;
                                Stored = Stored + reader.LocalName + " AS " + Tipo(reader.Value);
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

        public static string Tipo(String Nodo)
        {
            string input = String.Empty;
            string Cadena = "varchar(255)";
            try
            {
                float result = float.Parse(Nodo);
                Cadena = "numeric";
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
                Cadena = "datetime";
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
