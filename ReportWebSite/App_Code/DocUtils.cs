//****************************** Module Header ******************************\
//Module Name:    Utility.cs
//Project:        EmbedExcelIntoWordDoc
//Copyright (c) Microsoft Corporation

// The project illustrates how to embed excel sheet into word document using using Open XML SDK

//This source is subject to the Microsoft Public License.
//See http://www.microsoft.com/en-us/openness/resources/licenses.aspx#MPL.
//All other rights reserved.

//*****************************************************************************/
using System;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml;
using OVML = DocumentFormat.OpenXml.Vml.Office;
using System.IO;
using System.Drawing;
using V = DocumentFormat.OpenXml.Vml;
using DocumentFormat.OpenXml.Math;
using DocumentFormat.OpenXml.Vml.Office;

public static class DocUtils
{
//    public static void CreatePackage(string containingDocumentPath, string embeddedDocumentPath)
//    {
//        using (WordprocessingDocument package =
//          WordprocessingDocument.Create(containingDocumentPath,
//            WordprocessingDocumentType.Document))
//        {
//            AddParts(package, embeddedDocumentPath);
//        }
//    }

//    private static void AddParts(WordprocessingDocument parent,
//      string embeddedDocumentPath)
//    {
//        var mainDocumentPart = parent.AddMainDocumentPart();
//        GenerateMainDocumentPart().Save(mainDocumentPart);

//        var embeddedPackagePart =
//          mainDocumentPart.AddNewPart<EmbeddedPackagePart>(
//          "application/vnd.openxmlformats-" +
//          "officedocument.spreadsheetml.sheet",
//          "rId1");

//        GenerateEmbeddedPackagePart(embeddedPackagePart,
//          embeddedDocumentPath);

//        var imagePart =
//          mainDocumentPart.AddNewPart<ImagePart>(
//          "image/x-emf", "rId2");

//        GenerateImagePart(imagePart);
//    }

//    private static Document GenerateMainDocumentPart()
//    {
//        var element =
//          new Document(
//            new Body(
//              new Paragraph(
//                new Run(
//                  new Text(
//                    "This is the containing document."))),
//              new Paragraph(
//                new Run(
//                  new Text(
//                    "This is the embedded document: "))),
//              new Paragraph(
//                new Run(
//                  new EmbeddedObject(
//                    new ovml.OleObject()
//                    {
//                        Type = ovml.OleValues.Embed,
//                        ProgId = "Excel.Sheet.12",
//                        //ShapeId = "_x0000_i1025",
//                        DrawAspect = ovml.OleDrawAspectValues.Content,
//                        ObjectId = "_1299573545",
//                        Id = "rId1UUU9098",
                        
//                    }
//                  )
//                )
//              )
//            )
//          );

//        return element;
//    }

//    public static void GenerateEmbeddedPackagePart(OpenXmlPart part,
//      string embeddedDocumentPath)
//    {
//        byte[] embeddedDocumentBytes;

//        // The following code will generate an exception if an invalid
//        // filename is passed.
//        using (FileStream fsEmbeddedDocument =
//          File.OpenRead(embeddedDocumentPath))
//        {
//            embeddedDocumentBytes =
//              new byte[fsEmbeddedDocument.Length];

//            fsEmbeddedDocument.Read(embeddedDocumentBytes, 0,
//              embeddedDocumentBytes.Length);
//        }

//        using (BinaryWriter writer =
//          new BinaryWriter(part.GetStream()))
//        {
//            writer.Write(embeddedDocumentBytes);
//            writer.Flush();
//        }
//    }

//    public static void GenerateImagePart(OpenXmlPart part)
//    {
//        //using (BinaryWriter writer = new BinaryWriter(part.GetStream()))
//        {
//            part.FeedData(File.Open(@"d:\12494669_1024778694231558_1292043138065265713_n.png", FileMode.Open));
////            writer.Write(

//            //writer.Flush();
//        }
//    }

    public static DocumentFormat.OpenXml.Wordprocessing.Paragraph CreateEmbeddedObjectParagraph(string imageId, string embedId)
    {
        return
        new DocumentFormat.OpenXml.Wordprocessing.Paragraph(
          new DocumentFormat.OpenXml.Wordprocessing.Run(
            new DocumentFormat.OpenXml.Wordprocessing.EmbeddedObject(
              new V.Shape(
                new V.ImageData()
                {
                    Title = "",
                    RelationshipId = imageId//"rId2"
                }
              )
              {
                  Id = "_x0000_i1025",
                  Style = "width:450.5pt;height:250.5pt",
                  //Style = "width:15,82pt;height:9,92pt",
                  //Style = "width:100%;height:100%",
              },
              new OVML.OleObject()
              {
                  Type = OVML.OleValues.Embed,
                  ProgId = "Excel.Sheet.12",
                  ShapeId = "_x0000_i1025",
                  DrawAspect = OVML.OleDrawAspectValues.Content,
                  ObjectId = "_1299573545",
                  Id = embedId//"rId1"
              }
            )
          )
        );







        DocumentFormat.OpenXml.Wordprocessing.Paragraph p =
        new DocumentFormat.OpenXml.Wordprocessing.Paragraph(
        new DocumentFormat.OpenXml.Wordprocessing.Run(
        new EmbeddedObject(
        new V.Shapetype(
            new V.Stroke() 
                { 
                    JoinStyle = V.StrokeJoinStyleValues.Miter 
                },
            new V.Formulas(),
            //new V.Formula() { Equation = "if lineDrawn pixelLineWidth 0" },
            //new V.Formula() { Equation = "sum @0 1 0" },
            //new V.Formula() { Equation = "sum 0 0 @1" },
            //new V.Formula() { Equation = "prod @2 1 2" },
            //new V.Formula() { Equation = "prod @3 21600 pixelWidth" },
            //new V.Formula() { Equation = "prod @3 21600 pixelHeight" },
            //new V.Formula() { Equation = "sum @0 0 1" },
            //new V.Formula() { Equation = "prod @6 1 2" },
            //new V.Formula() { Equation = "prod @7 21600 pixelWidth" },
            //new V.Formula() { Equation = "sum @8 21600 0" },
            //new V.Formula() { Equation = "prod @7 21600 pixelHeight" },
            //new V.Formula() { Equation = "sum @10 21600 0" }),
            new V.Path() 
                { 
                    AllowGradientShape = TrueFalseValue.FromBoolean(true), 
                    ConnectionPointType = OVML.ConnectValues.Rectangle, 
                    AllowExtrusion = TrueFalseValue.FromBoolean(false) 
                },
            new OVML.Lock() 
                { 
                    Extension = V.ExtensionHandlingBehaviorValues.Edit, 
                    AspectRatio = TrueFalseValue.FromBoolean(true) 
                }
            ) 
            { 
                Id = "_x0000_t75", 
                //CoordinateSize = "21600,21600", 
                Filled = TrueFalseValue.FromBoolean(false), 
                Stroked = TrueFalseValue.FromBoolean(false), 
                OptionalNumber = 75, 
                PreferRelative = TrueFalseValue.FromBoolean(true), 
                EdgePath = "m@4@5l@4@11@9@11@9@5xe" 
            },
            new V.Shape(
                new V.ImageData() 
                    { 
                        Title = "", 
                        RelationshipId = imageId 
                    }
            ) 
                { 
                    Id = "_x0000_i1025", 
                    Style = "width:100%;height:100%", 
                    Ole = TrueFalseBlankValue.FromBoolean(false), 
                    Type = "#_x0000_t75", 
                },
                new OVML.OleObject() 
                    { 
                        Type = OVML.OleValues.Embed, 
                        ProgId = "Excel.Sheet.12", 
                        ShapeId = "_x0000_i1025", 
                        DrawAspect = OleDrawAspectValues.Icon, 
                        ObjectId = "_1307530183", 
                        Id = embedId 
                    }
                ) 
                { 
                    DxaOriginal = "10957U", 
                    DyaOriginal = "8455U" 
                })
        );
        return p;
    }
}