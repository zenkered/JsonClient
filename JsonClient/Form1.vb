Imports System.Text
Imports System.Net
Imports System.IO
Imports System.Web.Services.Protocols
Imports System.Xml

Public Class Form1

    Private tbl As New DataTable

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        For Each address As String In New String() {"http://localhost:1937/AbbConciseDocumentPrinting.asmx",
                                                    "http://mi-qawhprint2:1001/AbbConciseDocumentPrinting.asmx",
                                                    "http://mi-uatwhprint:1001/AbbConciseDocumentPrinting.asmx"}
            cmbURL.Items.Add(address)
        Next

        '"http://10.80.140.63:1937/AbbConciseDocumentPrinting.asmx",
        '"http://172.25.1.106:1001/AbbConciseDocumentPrinting.asmx",
        '"http://172.25.1.107:1001/AbbConciseDocumentPrinting.asmx",
        ' "http://172.25.1.100:1003/AbbConciseDocumentPrinting.asmx",
        '"http://172.25.1.100:1004/AbbConciseDocumentPrinting.asmx",
        '"http://172.25.1.100/abbogdocumentprinting/AbbConciseDocumentPrinting.asmx",
        '"http://192.168.130.190/abbogdocumentprinting/AbbConciseDocumentPrinting.asmx",
        '"http://uatnew.opticaldg.com/ABBOGDocumentPrinting/AbbConciseDocumentPrinting.asmx",
        '"http://mi-uatweb/ABBOGDocumentPrinting/AbbConciseDocumentPrinting.asmx",
        '"http://mi-webproj/ABBOGDocumentPrinting/AbbConciseDocumentPrinting.asmx",
        '"http://172.25.10.246/ABBOGDocumentPrinting/AbbConciseDocumentPrinting.asmx",
        '"http://mi-webproj2/ABBOGDocumentPrinting/AbbConciseDocumentPrinting.asmx",
        '"http://mi-printproj/ABBOGDocumentPrinting/AbbConciseDocumentPrinting.asmx",
        '"http://mi-uatweb/ABBOGDocumentPrinting/AbbConciseDocumentPrinting.asmx",
        ' "http://mi-qawhprint2:1001/AbbConciseDocumentPrinting.asmx"

        tbl = New DataTable
        tbl.Columns.Add("SELECTED", GetType(System.Int16))
        tbl.Columns.Add("FORM", GetType(System.String))
        tbl.Columns.Add("XMLSIZE", GetType(System.String))

        cmbURL.Text = cmbURL.Items(0).DisplayText

        Dim xmlDirectory As String = "C:\VS\AbbConciseDocumentPrinting\AbbConciseDocumentPrinting\XML\"

        For Each formfilename As String In My.Computer.FileSystem.GetFiles("C:\VS\AbbConciseDocumentPrinting\AbbConciseDocumentPrinting\Reports")
            Dim fileInfo As New FileInfo(formfilename)
            If fileInfo.Name.StartsWith("FORM") OrElse fileInfo.Name.StartsWith("LABEL") Then
                formfilename = fileInfo.Name.Split(".")(0)
                If fileInfo.Name.StartsWith("FORM") Then
                    formfilename = formfilename.Substring(4)
                Else
                    formfilename = formfilename.Substring(5)
                End If

                Dim xmlSize As Int64 = -1
                If My.Computer.FileSystem.FileExists(xmlDirectory & fileInfo.Name.Split(".")(0) & ".xml") Then
                    Dim xmlfileInfo As New FileInfo(xmlDirectory & fileInfo.Name.Split(".")(0) & ".xml")
                    xmlSize = Math.Round(xmlfileInfo.Length / 1000, 0, MidpointRounding.AwayFromZero)
                End If

                If IsNumeric(formfilename) Then
                    tbl.Rows.Add(New Object() {0, fileInfo.Name.Split(".")(0), xmlSize})
                End If
            End If
        Next

        grdForms.DataSource = tbl
        grdForms.DisplayLayout.Bands(0).SortedColumns.Add("FORM", False)
    End Sub

    Private Sub btnPrintLocal_Click(sender As System.Object, e As System.EventArgs) Handles btnPrintLocal.Click
        printDocUsingXML()
    End Sub

    Private Sub PrintInvoiceDocument(ByVal DocumentName As String)

        Dim docPrintingRequestType As AbbConciseDocumentPrinting.DocumentPrintingRequestType
        docPrintingRequestType = New AbbConciseDocumentPrinting.DocumentPrintingRequestType
        docPrintingRequestType.PrintRequestInfo = New AbbConciseDocumentPrinting.PrintRequest

        With docPrintingRequestType.PrintRequestInfo
            .PrinterIp = "192.168.142.65:9100"
            .ReportName = DocumentName
            .RequestType = "Invoice"
            .ReturnPDF = True
        End With

        ReDim docPrintingRequestType.DocumentHeader(1)
        docPrintingRequestType.DocumentHeader(1) = New AbbConciseDocumentPrinting.InvoiceHeader
        With docPrintingRequestType.DocumentHeader(1)
            .BillToAccount = "012345"
            .Carrier = "UPS"
            .CustomerID = String.Empty
            .CustomerNo = "077780"
            .CustomerOrderID = "O1231D"
            .CustomerPONumber = "7138968"
            .DisplayPricing = True
            .EdiReferenceNumber = "My Edi Ref"
            .HeaderBodyText = "This is some text that prints on the Pick Slip. Varies from form to form."
            .ImportedPatientDiscount = 0
            .ImportedPatientFreight = 5.99
            .ImportedPatientInvoiceSales = 25.0
            .ImportedPatientSalesTax = 2.5
            .ImportedPatientTotalSales = 33.49
            .InvoiceNo = "100434757301"
            .InvoiceType = "I"
            .InvoiceDate = "04/12/2013"
            .CustomerNo = "833556"
            .ShipToNo = "0000"
            .OrderNo = "1004347573"
            .OrderDate = "07/12/13 06:20:45 *E*"
            .WebOrderNo = "0004605380"
            .SalesRepCode = "FSM"
            .ShipMethodDescription = "MAIL INNOVATIONS"
            .TermsDescription = "PROX 15TH"
            .OrderComment = "My Comment"
            .OrderByCallerName = ""
            .OrderTakenBy = "Yvette"
            .OrderSource = "W"
            .PartnerOrderOrigin = String.Empty
            .MerchandiseTotal = 110
            .PatientID = String.Empty
            .PatientInvoiceSales = 0
            .PatientSalesTax = 0
            .PatientFreight = 0
            .PatientTotalSales = 110
            .ShipToPatient = "N"
            .IsReprint = "Y"

            .PromoCode = String.Empty
            .TransmissionDate = String.Empty
            .OfficeWebSite = String.Empty
            .PaymentMethod = String.Empty
            .PrescribingDoctor = String.Empty
            .RebateText = "Rebate Text"
        End With

        Dim numdetails As Int16 = 6

        ReDim docPrintingRequestType.DocumentHeader(1).InvoiceDetails(numdetails)
        For index As Int16 = 1 To numdetails
            docPrintingRequestType.DocumentHeader(1).InvoiceDetails(index) = New AbbConciseDocumentPrinting.InvoiceDetails
        Next

        With docPrintingRequestType.DocumentHeader(1).InvoiceDetails(1)
            .InvoiceLno = 1
            .ItemCode = "CIBFD902060551"
            .ItemDescription = "FOC DAILIES 90 PK 8.60 13.80-2.75"
            .ItemDescription2 = "FOCUS DAILIES AQUARELEASE 90PK"
            .PatientName = "Laura Florek"
            .LeftRightIndicator = "L"
            .QuantityOrdered = 1
            .QuantityShipped = 1
            .QuantityDue = 0
            .BaseCurve = "8.4"
            .Diameter = "14.4"
            .SpherePower = "5.25"
            .Cylinder = "-1.75"
            .Axis = "85"
            .Color = "Blue"
            .AddPower = "1.50"
            .UnitPrice = 55
            .PatientPrice = 55
            .ExtendedPrice = 55
            .PatientExtendedPrice = 55
            .ItemBinLocation = "040506A"
        End With

        With docPrintingRequestType.DocumentHeader(1).InvoiceDetails(2)
            .InvoiceLno = 2
            .ItemCode = "CIBFD902060551"
            .ItemDescription = "FOC DAILIES 90 PK 8.60 13.80-2.75"
            .ItemDescription2 = "FOCUS DAILIES AQUARELEASE 90PK"
            .PatientName = "Laura Florek"
            .LeftRightIndicator = "R"
            .QuantityOrdered = 1
            .QuantityShipped = 1
            .QuantityDue = 0
            .BaseCurve = "8.4"
            .Diameter = "14.4"
            .SpherePower = "5"
            .Cylinder = "-1.75"
            .Axis = "90"
            .Color = "Red"
            .AddPower = "1.50"
            .UnitPrice = 55
            .PatientPrice = 55
            .ExtendedPrice = 55
            .PatientExtendedPrice = 55
            .ItemBinLocation = "040506A"
        End With

        With docPrintingRequestType.DocumentHeader(1).InvoiceDetails(3)
            .InvoiceLno = 3
            .ItemCode = "VKAO000010"
            .ItemDescription = "ACUVUE OASIS 6PK 8.40 14.00 -3.50"
            .ItemDescription2 = "ACUVUE OASIS 6PK"
            .PatientName = "Laura Florek"
            .LeftRightIndicator = "L"
            .QuantityOrdered = 1
            .QuantityShipped = 1
            .QuantityDue = 0
            .BaseCurve = "8.4"
            .Diameter = "14.00"
            .SpherePower = "5"
            .Cylinder = "-1.75"
            .Axis = "90"
            .Color = ""
            .AddPower = "-3.50"
            .UnitPrice = 33.99
            .PatientPrice = 33.99
            .ExtendedPrice = 33.99
            .PatientExtendedPrice = 33.99
            .ItemBinLocation = "060404D"
        End With

        With docPrintingRequestType.DocumentHeader(1).InvoiceDetails(4)
            .InvoiceLno = 4
            .ItemCode = "VKAO000010"
            .ItemDescription = "ACUVUE OASIS 6PK 8.40 14.00 -3.50"
            .ItemDescription2 = "ACUVUE OASIS 6PK"
            .PatientName = "Laura Florek"
            .LeftRightIndicator = "R"
            .QuantityOrdered = 1
            .QuantityShipped = 1
            .QuantityDue = 0
            .BaseCurve = "8.4"
            .Diameter = "14.00"
            .SpherePower = "5"
            .Cylinder = "-3.50"
            .Axis = "90"
            .Color = ""
            .AddPower = "1.50"
            .UnitPrice = 33.99
            .PatientPrice = 33.99
            .ExtendedPrice = 33.99
            .PatientExtendedPrice = 33.99
            .ItemBinLocation = "060404D"
        End With

        With docPrintingRequestType.DocumentHeader(1).InvoiceDetails(5)
            .InvoiceLno = 5
            .ItemCode = "COOPD90000020"
            .ItemDescription = "PRCLR 1 DAY 90PK 8.70 14.20 -1.00"
            .ItemDescription2 = "PROCLEAR 1-DAY 90PK"
            .PatientName = "Laura Florek"
            .LeftRightIndicator = "L"
            .QuantityOrdered = 1
            .QuantityShipped = 1
            .QuantityDue = 0
            .BaseCurve = "8.70"
            .Diameter = "14.20"
            .SpherePower = "5"
            .Cylinder = "-1.00"
            .Axis = "90"
            .Color = ""
            .AddPower = "1.50"
            .UnitPrice = 33.99
            .PatientPrice = 33.99
            .ExtendedPrice = 33.99
            .PatientExtendedPrice = 33.99
            .ItemBinLocation = "060506D"
        End With

        With docPrintingRequestType.DocumentHeader(1).InvoiceDetails(6)
            .InvoiceLno = 6
            .ItemCode = "COOPD90000020"
            .ItemDescription = "PRCLR 1 DAY 90PK 8.70 14.20 -1.00"
            .ItemDescription2 = "PROCLEAR 1-DAY 90PK"
            .PatientName = "Laura Florek"
            .LeftRightIndicator = "L"
            .QuantityOrdered = 1
            .QuantityShipped = 1
            .QuantityDue = 0
            .BaseCurve = "8.70"
            .Diameter = "14.20"
            .SpherePower = "5"
            .Cylinder = "-1.00"
            .Axis = "90"
            .Color = ""
            .AddPower = "1.50"
            .UnitPrice = 33.99
            .PatientPrice = 33.99
            .ExtendedPrice = 33.99
            .PatientExtendedPrice = 33.99
            .ItemBinLocation = "060506D"
        End With

        ReDim docPrintingRequestType.DocumentHeader(1).InvoiceAddresses(2)
        docPrintingRequestType.DocumentHeader(1).InvoiceAddresses(1) = New AbbConciseDocumentPrinting.Address
        docPrintingRequestType.DocumentHeader(1).InvoiceAddresses(2) = New AbbConciseDocumentPrinting.Address

        With docPrintingRequestType.DocumentHeader(1).InvoiceAddresses(1)
            .AddressType = AbbConciseDocumentPrinting.AddressTypes.BillTo
            .Name = "BONITA VISION CENTER"
            .Contact = "TRAVIS A GRESHAM III OD"
            .PhoneNumber = "7637849049"
            .FaxNumber = "7637176939"
            .AddressLine1 = "8800 BERNWOOD PKWY"
            .AddressLine2 = "STE #7"
            .AddressLine3 = String.Empty
            .City = "BONITA SPRINGS"
            .StateProvinceCode = "FL"
            .PostalCode = "34135"
            .CountryCode = "US"
        End With

        With docPrintingRequestType.DocumentHeader(1).InvoiceAddresses(2)
            .AddressType = AbbConciseDocumentPrinting.AddressTypes.ShipTo
            .Name = "NICHOLE LOVETRO"
            .Contact = "NICHOLE LOVETRO"
            .PhoneNumber = String.Empty
            .FaxNumber = String.Empty
            .AddressLine1 = "23421 ALAMANDA DR UNIT 103"
            .AddressLine2 = String.Empty
            .AddressLine3 = String.Empty
            .City = "BONITA SPRINGS"
            .StateProvinceCode = "FL"
            .PostalCode = "34125-1863"
            .CountryCode = "US"
        End With

        Dim objDocumentPrinting As New AbbConciseDocumentPrinting.AbbConciseDocumentPrinting
        'http://192.168.130.190/abbogdocumentprinting/AbbConciseDocumentPrinting.asmx
        objDocumentPrinting.Url = "http://192.168.130.190/abbogdocumentprinting/AbbConciseDocumentPrinting.asmx"
        Dim docResponse As AbbConciseDocumentPrinting.DocumentPrintingResponseType = objDocumentPrinting.LoadAndRequestDocumentObject("", docPrintingRequestType)

        If docResponse.ResponseCode <> 0 Then
            MessageBox.Show(DocumentName & ": " & docResponse.ResponseMessage)
            Exit Sub
        End If

        Dim pdfpath As String = "c:\temp\" & DocumentName & "_" & System.Guid.NewGuid.ToString() & ".pdf"
        Dim bb As Byte() = Convert.FromBase64String(docResponse.ResponseDocumentText)

        Using fs As IO.FileStream = New IO.FileStream(pdfpath, IO.FileMode.OpenOrCreate, IO.FileAccess.Write)
            Using bw As IO.BinaryWriter = New IO.BinaryWriter(fs)
                bw.Write(bb)
                bw.Flush()
                bw.Close()
            End Using
            fs.Close()
            fs.Dispose()
        End Using

    End Sub

    Private Sub printDocUsingXML()

        Dim count As Int16 = 0

        Dim address As String = cmbURL.Text

        If address.Length = 0 Then
            MessageBox.Show("Select a URL frpom the drop down list")
            Exit Sub
        End If

        Dim objDocumentPrinting As New AbbConciseDocumentPrinting.AbbConciseDocumentPrinting

        Try
            objDocumentPrinting.Url = address
            TextBox1.AppendText(Environment.NewLine & "Connecting to: " & objDocumentPrinting.Url & Environment.NewLine)

        Catch ex As Exception
            TextBox1.AppendText(Environment.NewLine & "Error connecting to: " & address & " " & ex.Message)
            Exit Sub
        End Try

        Dim useStandardInputData As Boolean = False


        For Each row As DataRow In tbl.Select("SELECTED = 1")

            Dim formName As String = row.Item("FORM") & String.Empty

            Try
                Dim xmlDoc As New XmlDocument
                Dim xmlFile As String = String.Empty
                Application.DoEvents()
                Application.DoEvents()
                Application.DoEvents()
                Application.DoEvents()

                If useStandardInputData Then
                    xmlFile = "C:\VS\AbbConciseDocumentPrinting\AbbConciseDocumentPrinting\XML\input.xml"
                Else
                    xmlFile = "C:\VS\AbbConciseDocumentPrinting\AbbConciseDocumentPrinting\XML\" & formName & ".xml"
                    If Not My.Computer.FileSystem.FileExists(xmlFile) Then
                        xmlFile = "C:\VS\AbbConciseDocumentPrinting\AbbConciseDocumentPrinting\XML\input.xml"
                    End If
                End If

                If My.Computer.FileSystem.FileExists(xmlFile) Then
                    xmlDoc.Load(xmlFile)
                    Dim MyXMLNode As XmlNode = xmlDoc.SelectSingleNode("/DocumentPrintingRequestType/PrintRequestInfo/ReportName")
                    If MyXMLNode IsNot Nothing Then
                        MyXMLNode.ChildNodes(0).InnerText = formName
                    End If

                    MyXMLNode = xmlDoc.SelectSingleNode("/DocumentPrintingRequestType/PrintRequestInfo/PrinterIp")
                    If MyXMLNode Is Nothing Then
                        MyXMLNode = xmlDoc.SelectSingleNode("/LoadAndRequestDocumentObjectSoapIn/parameters/LoadAndRequestDocumentObject/RequestInfo/PrintRequestInfo/PrinterIp")
                    End If

                    If MyXMLNode IsNot Nothing Then
                        If formName.StartsWith("LABEL") Then
                            MyXMLNode.InnerText = "\\sstringer-tb\ZDesigner GX420d" ' "192.168.142.49:9100"  
                        Else
                            MyXMLNode.InnerText = "" ' "192.168.141.63:9100" ' "192.168.141.191:9100" ' "\\sstringer2-nb\ZDesignerGX420d" ' "192.168.142.49:9100" ' Can you try 192.168.2.66? This is a label printer in the UAT test lab. "192.168.142.49:9100" '"192.168.135.200:9100" 
                        End If
                    End If

                    'ReturnPDF
                    MyXMLNode = xmlDoc.SelectSingleNode("/DocumentPrintingRequestType/PrintRequestInfo/ReturnPDF")
                    If MyXMLNode Is Nothing Then
                        MyXMLNode = xmlDoc.SelectSingleNode("/LoadAndRequestDocumentObjectSoapIn/parameters/DocumentPrintingRequestType/PrintRequestInfo/ReturnPDF")
                    End If
                    If MyXMLNode IsNot Nothing Then
                        If formName.StartsWith("LABEL") Then
                            MyXMLNode.InnerText = "Y"
                            ''Else
                            MyXMLNode.InnerText = "Y"
                        End If
                    End If

                    xmlDoc.Save(xmlFile)
                End If

                TextBox1.AppendText(Environment.NewLine & "Requesting form " & formName & Environment.NewLine)

                Application.DoEvents()

                ' Wait 10 seconds to the IP Address is not flooded with data.
                If count > 0 Then
                    'TextBox1.AppendText("Waiting 10 Seconds between requests!" & Environment.NewLine)
                    Application.DoEvents()
                    'System.Threading.Thread.Sleep(10000)
                End If
                count += 1

                'xmlDoc.InnerXml = xmlDoc.InnerXml.Replace("PrintRequestInfo", "PrintRequestInfo1")

                TextBox1.AppendText(Environment.NewLine & "Start Request " & DateTime.Now & Environment.NewLine)

                'objDocumentPrinting.Timeout = 180000 ' milliseconds
                Dim docResponse As XmlNode = objDocumentPrinting.LoadAndRequestDocument("", xmlDoc)

                TextBox1.AppendText(Environment.NewLine & "Request Complete" & DateTime.Now & Environment.NewLine)

                Dim respNode As XmlNode = docResponse.SelectSingleNode("/Response/ResponseCode")
                Dim ResponseCode As Int16 = respNode.InnerText

                respNode = docResponse.SelectSingleNode("/Response/ResponseMessage")
                Dim ResponseMessage As String = respNode.InnerText

                respNode = docResponse.SelectSingleNode("/Response/ResponseDocument")
                Dim responseDocumentText As String = respNode.InnerText

                Dim success As Boolean = False
                If ResponseCode <> 0 Then
                    TextBox1.AppendText(Environment.NewLine & "Service error processing form (" & formName & "): " & ResponseMessage & Environment.NewLine)
                    'MessageBox.Show(formName & ": " & ResponseMessage)
                Else
                    TextBox1.AppendText(Environment.NewLine & "Service generated form " & formName & Environment.NewLine)
                    success = True
                End If

                If success AndAlso responseDocumentText.Length > 0 Then
                    Application.DoEvents()

                    Dim pdfpath As String = "c:\temp\" & formName & "_" & System.Guid.NewGuid.ToString() & ".pdf"
                    Dim bb As Byte() = Convert.FromBase64String(responseDocumentText)

                    Using fs As IO.FileStream = New IO.FileStream(pdfpath, IO.FileMode.OpenOrCreate, IO.FileAccess.Write)
                        Using bw As IO.BinaryWriter = New IO.BinaryWriter(fs)
                            bw.Write(bb)
                            bw.Flush()
                            bw.Close()
                        End Using
                        fs.Close()
                        fs.Dispose()
                    End Using
                End If

            Catch ex As Exception
                TextBox1.AppendText(ex.Message & DateTime.Now & Environment.NewLine)

            Finally
                TextBox1.AppendText(Environment.NewLine & "------------------------------------------------------------" & Environment.NewLine & Environment.NewLine)

            End Try

        Next

        TextBox1.AppendText(Environment.NewLine & "Process Completed" & Environment.NewLine)

    End Sub

End Class
