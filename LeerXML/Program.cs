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
using System.Collections.Generic;
//using LeerXML.Clases.Utilities;


//using System.Windows.Forms;
namespace LeerXML
{
    class Program
    {
        
        static void Main(string[] args)
        {
            
            String header = " BEGIN TRY " +
                " BEGIN TRAN X; " +
                " DECLARE @tbl_pagos TABLE(id INT IDENTITY(1, 1), pago_nodo XML) " +
                " DECLARE @id_comprobante INT, @id_pago INT, @pagos_indice INT = 1, @pagos_maximo INT = 0, @pago_nodo XML ";

            String bottom = " END TRY" +
                " BEGIN CATCH "+
                " IF(XACT_STATE()) = -1 " +
                " BEGIN " +
                " ROLLBACK TRAN X; " +
                " THROW; " +
                " END "+
                " END CATCH ";
            //String XmlDictionaryString = "C:\\XML\\CP-1456814_Complemento_Pago.xml";
            String XmlDictionaryString = "C:\\XML\\B-55238797_Ingresos_Nacional.xml";
            CreateTable Crear = new CreateTable();
            Utilities util = new Utilities();
            string InstructionCreate = Crear.CreateTableInstruction(XmlDictionaryString);
           // Boolean errConexion = util.Conectar(InstructionCreate);
            string InstruccionInsert = InsertTable(XmlDictionaryString);
        }

        public static string InsertTable(String XmlString)
        {
            List<string> varList = new List<string>();
            List<String> NodoTree = new List<String>();
            int i, CurrentDepth = 0;
            string Node= "";
            string Script = "";
            string XMLVar = "@cfdi";
            Boolean MeanWhile = false;
            XmlTextReader reader = new XmlTextReader(XmlString);
            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                    { 
                        if (CurrentDepth < reader.Depth)
                        {
                            CurrentDepth += 1;
                            NodoTree.Add(Node);
                            varList.Add(Node);                        }

                        if (CurrentDepth > reader.Depth)
                        {
                            while (CurrentDepth != reader.Depth)
                            {
                                CurrentDepth -= 1;
                                NodoTree.RemoveAt(NodoTree.Count - 1);
                            }
                        }
                        //NodoTree.Add(reader.Name);
                        if (Node == reader.LocalName)
                        {
                            if (!MeanWhile)
                            {
                                MeanWhile = true;
                                Script = InsertWhile(Script, reader.LocalName, XMLVar, NodoTree, Node);
                               // Script = Script + "Abrir While" + Environment.NewLine ;
                                //Console.WriteLine("Abrir While");
                            }
                        }
                        else
                        {
                            if (MeanWhile)
                            {
                                MeanWhile = false;
                                Script = Script + "END" + Environment.NewLine;
                                //Console.WriteLine("Cerrar While");
                            }
                            Script = Script + Environment.NewLine;
                            Console.WriteLine("<{0}>", reader.Name);
                            Node = reader.LocalName;
                            if (reader.AttributeCount != 0)
                            {
                                Script = Script + " INSERT INTO tbl_" + reader.LocalName.ToLower() + Environment.NewLine;
                                Script = Script + " SELECT ";
                                i = 0;
                                while (reader.MoveToNextAttribute()) // Read the attributes.
                                {
                                    Script = Script + "XT.T.value('(@" + reader.Name.ToLower() +")[1]','" + Tipo(reader.Value)+"')" + Environment.NewLine;
                                    Console.WriteLine("               " + reader.Name.ToLower() /* + "='" + reader.Value + "'    " /*+  reader.ValueType.FullName*/);
                                    i = i + 1;
                                    /*Stored = Stored + reader.LocalName + " " + Tipo(reader.Value);
                                    if (i != (reader.AttributeCount))
                                        Stored = Stored + ",";
                                    /*else
                                        Stored = Stored + reader.LocalName;
                                    //reader.ReadEndElement();*/
                                }
                                Script = Script + " FROM " +XMLVar +".nodes('" + RutaXml(NodoTree,Node) + "') XT(T)" + Environment.NewLine;
                                //Script = Script + ")";
                            }
                        }
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
                    Console.WriteLine("<?{0} {1}?>", reader.Name, reader.Value);
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
                        break;
                }
            }
            Console.WriteLine(Script);
            Console.ReadLine();
            return Script;
        }
        public static string InsertWhile(string Cadena, string Table, string CFDI, List<String> Nodos, String NodoActual)
        {
            int start,at,end,count,ultimo = 0;
            /*int at;
            int end;
            int count;*/
            string Modified = " INSERT INTO @" + Table.ToLower() + Environment.NewLine +
                            " SELECT XT.T.query('.') " + Environment.NewLine +
                            " FROM " + CFDI+ ".nodes('"+ RutaXml(Nodos, NodoActual) + "') XT(T)" + Environment.NewLine+ 
                            " SELECT @"+ NodoActual+"_maximo = MAX(id) FROM @" + Table + Environment.NewLine +
                            " WHILE(@"+ NodoActual+"_indice <= @"+ NodoActual+"_maximo) " + Environment.NewLine +
                            " BEGIN " + Environment.NewLine +
                            "SELECT @"+NodoActual+"_nodo = "+ NodoActual+ "_nodo FROM @"+ Table.ToLower() +" WHERE id = @"+ NodoActual+"_indice" + Environment.NewLine;
            end = Cadena.Length;
            start = end / 2;
            count = 0;
            at = 0;
            ultimo = 0;
            while ((start <= end) && (at > -1))
            {
                // start+count must be a position within -str-.
                count = end - start;
                at = Cadena.IndexOf("INSERT", start, count);
                if (at == -1) break;
                ultimo = at;
                //Console.Write("{0} ", at);
                start = at + 1;
            }

            Cadena = Cadena.Insert(ultimo, Modified) + Environment.NewLine;
            Cadena = Cadena + Environment.NewLine;
            return Cadena;
        }
        public static string RutaXml(List<String> Nodos, String NodoActual)
        {
            string Cadena = "";
            for (int i = 0; i < Nodos.Count;  i++)
                {
            Cadena = Cadena + "*:" +Nodos[i];
                if (i != Nodos.Count)
                {
                    Cadena = Cadena + "/";
                }
            }
            Cadena = Cadena + "*:"+ NodoActual;
            return Cadena;
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
            string Cadena = "VARCHAR(" + Nodo.Length + ")";
            try
            {
                float result = float.Parse(Nodo);
                Cadena = "DECIMAL(18,6)";
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
