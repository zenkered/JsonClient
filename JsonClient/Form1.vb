Imports System.Text
Imports System.Net
Imports System.IO
Imports System.Web.Services.Protocols
Imports System.Xml

Public Class Form1

    Private tbl As New DataTable

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        For Each address As String In New String() {"http://localhost:1937/AbbConciseDocumentPrinting.asmx",
                                                    "http://172.25.1.106:1001/AbbConciseDocumentPrinting.asmx",
                                                   "http://172.25.1.107:1001/AbbConciseDocumentPrinting.asmx",
                                                   "http://172.25.1.100:1003/AbbConciseDocumentPrinting.asmx",
                                                   "http://172.25.1.100:1004/AbbConciseDocumentPrinting.asmx",
                                                   "http://172.25.1.100/abbogdocumentprinting/AbbConciseDocumentPrinting.asmx",
                                                   "http://192.168.130.190/abbogdocumentprinting/AbbConciseDocumentPrinting.asmx",
                                                   "http://uatnew.opticaldg.com/ABBOGDocumentPrinting/AbbConciseDocumentPrinting.asmx",
                                                   "http://mi-uatweb/ABBOGDocumentPrinting/AbbConciseDocumentPrinting.asmx",
                                                   "http://mi-webproj/ABBOGDocumentPrinting/AbbConciseDocumentPrinting.asmx",
                                                   "http://172.25.10.246/ABBOGDocumentPrinting/AbbConciseDocumentPrinting.asmx",
                                                   "http://mi-webproj2/ABBOGDocumentPrinting/AbbConciseDocumentPrinting.asmx",
                                                   "http://mi-printproj/ABBOGDocumentPrinting/AbbConciseDocumentPrinting.asmx",
                                                   "http://mi-uatweb/ABBOGDocumentPrinting/AbbConciseDocumentPrinting.asmx",
                                                   "http://mi-qawhprint2:1001/AbbConciseDocumentPrinting.asmx"}
            cmbURL.Items.Add(address)

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

            'For Each formName As String In New String() {"FORM00", "FORM01", "FORM06" _
            '                                       , "FORM11", "FORM15", "FORM16", "FORM17", "FORM18" _
            '                                       , "FORM21", "FORM22", "FORM23", "FORM24", "FORM25", "FORM26", "FORM27", "FORM28", "FORM29" _
            '                                       , "FORM30", "FORM31", "FORM32", "FORM33", "FORM34", "FORM35", "FORM36", "FORM37", "FORM38", "FORM39" _
            '                                       , "FORM40", "FORM41", "FORM42", "FORM43", "FORM44", "FORM45", "FORM46", "FORM47", "FORM48", "FORM49" _
            '                                       , "FORM50", "FORM51", "FORM52", "FORM53", "FORM54", "FORM55" _
            '                                       , "FORM60", "FORM61", "FORM62", "FORM63", "FORM64", "FORM65", "FORM66" _
            '                                       , "FORM74", "FORM80", "FORM91", "FORM92", "FORM93", "LABEL01", "LABEL02"}

            '    tbl.Rows.Add(New Object() {0, formName})
            'Next

            grdForms.DataSource = tbl
            grdForms.DisplayLayout.Bands(0).SortedColumns.Add("FORM", False)
        Next
    End Sub


    Private Sub btnPrintLocal_Click(sender As System.Object, e As System.EventArgs) Handles btnPrintLocal.Click
        printDocUsingXML()
    End Sub

    Private Sub PrintInvoiceDocumentAndyDavis(ByVal DocumentName As String)

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
            .InvoiceDate = "06/24/2013"
            .InvoiceNo = "104427758901"
            .InvoiceType = "C"
            .CustomerPONumber = "4304530/4307530"
            .CustomerNo = "1043154"
            .OrderNo = "1004277589"
            .ShipMethodDescription = "MAIL INNOVATIONS"
            .OrderByCallerName = "vbrook@advanticabene"
            .TermsDescription = "NET 10 DAYS MONTH END"
            .OrderDate = "06/17/13 19:28:59   *E*"
            .MerchandiseTotal = -91.8
        End With

        Dim numdetails As Int16 = 2

        ReDim docPrintingRequestType.DocumentHeader(1).InvoiceDetails(numdetails)
        For index As Int16 = 1 To numdetails
            docPrintingRequestType.DocumentHeader(1).InvoiceDetails(index) = New AbbConciseDocumentPrinting.InvoiceDetails
        Next

        With docPrintingRequestType.DocumentHeader(1).InvoiceDetails(1)
            .InvoiceLno = 1
            .QuantityShipped = -2
            .QuantityOrdered = -2
            .QuantityDue = 0
            .ItemCode = "OSIBT002582"
            .ItemDescription = "BIOTOR 8.70 14,50 -2.00-2.25x060"
            .ItemDescription2 = "BIOMEDICS TORIC 6PK"
            .PatientName = "Left Eye"
            .UnitPrice = -22.95
            .ExtendedPrice = -45.9
        End With

        With docPrintingRequestType.DocumentHeader(1).InvoiceDetails(2)
            .InvoiceLno = 2
            .QuantityShipped = -2
            .QuantityOrdered = -2
            .QuantityDue = 0
            .ItemCode = "OSIBT002625"
            .ItemDescription = "BIOTOR 8.70 14,50 -1.50-2.25x130"
            .ItemDescription2 = "BIOMEDICS TORIC 6PK"
            .PatientName = "Right Eye"
            .UnitPrice = -22.95
            .ExtendedPrice = -45.9
        End With

        ReDim docPrintingRequestType.DocumentHeader(1).InvoiceAddresses(2)
        docPrintingRequestType.DocumentHeader(1).InvoiceAddresses(1) = New AbbConciseDocumentPrinting.Address
        docPrintingRequestType.DocumentHeader(1).InvoiceAddresses(2) = New AbbConciseDocumentPrinting.Address

        With docPrintingRequestType.DocumentHeader(1).InvoiceAddresses(1)
            .AddressType = AbbConciseDocumentPrinting.AddressTypes.BillTo
            .Name = "ALLIED EYECARE LLC"
            .Name2 = "ADVANTICA"
            .Contact = ""
            .PhoneNumber = ""
            .FaxNumber = ""
            .AddressLine1 = "19321 C US HWY 19 N STE 320"
            .AddressLine2 = ""
            .AddressLine3 = String.Empty
            .City = "CLEARWATER"
            .StateProvinceCode = "FL"
            .PostalCode = "33764"
            .CountryCode = ""
        End With

        With docPrintingRequestType.DocumentHeader(1).InvoiceAddresses(2)
            .AddressType = AbbConciseDocumentPrinting.AddressTypes.ShipTo
            .Name = "Andy Davis"
            .Contact = ""
            .PhoneNumber = String.Empty
            .FaxNumber = String.Empty
            .AddressLine1 = "16321C US HIGHWAY 19 N"
            .AddressLine2 = String.Empty
            .AddressLine3 = String.Empty
            .City = "CLEARWATER"
            .StateProvinceCode = "FL"
            .PostalCode = "33764-3102"
            .CountryCode = ""
        End With

        ReDim docPrintingRequestType.DocumentHeader(1).InvoiceNotes(1)
        docPrintingRequestType.DocumentHeader(1).InvoiceNotes(1) = New AbbConciseDocumentPrinting.InvoiceNotes
        With docPrintingRequestType.DocumentHeader(1).InvoiceNotes(1)
            .SequenceNo = 1
            .NoteCode = "TRACKING"
            .NoteText = "TRACKING: 02612836114000127053"
        End With

        ReDim docPrintingRequestType.DocumentHeader(1).InvoiceCharges(3)
        docPrintingRequestType.DocumentHeader(1).InvoiceCharges(1) = New AbbConciseDocumentPrinting.InvoiceHeaderCharges
        docPrintingRequestType.DocumentHeader(1).InvoiceCharges(2) = New AbbConciseDocumentPrinting.InvoiceHeaderCharges
        docPrintingRequestType.DocumentHeader(1).InvoiceCharges(3) = New AbbConciseDocumentPrinting.InvoiceHeaderCharges
        With docPrintingRequestType.DocumentHeader(1).InvoiceCharges(1)
            .SequenceNo = 1
            .ChargeDescription = "MAIL INNOVATIONS"
            .ChargeAmount = -6.29
            .isSubTotalDivider = "N"
        End With

        With docPrintingRequestType.DocumentHeader(1).InvoiceCharges(2)
            .SequenceNo = 2
            .ChargeDescription = ""
            .ChargeAmount = 0
            .isSubTotalDivider = "Y"
        End With

        With docPrintingRequestType.DocumentHeader(1).InvoiceCharges(3)
            .SequenceNo = 3
            .ChargeDescription = "**** INVOICE TOTAL ****"
            .ChargeAmount = -98.09
            .isSubTotalDivider = "N"
        End With

        makeRequest(DocumentName, docPrintingRequestType)

    End Sub

    Private Sub PrintInvoiceDocumentPaulaSaunders(ByVal DocumentName As String)

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
            .InvoiceDate = "07/12/2013"
            .InvoiceNo = "100421294601"
            .InvoiceType = "I"
            .CustomerPONumber = "010255316a/SAUNDERS"
            .CustomerNo = "BL0001"
            .OrderNo = "1004212946"
            .ShipMethodDescription = "B&L UPS Mail Innovations"
            .OrderByCallerName = ""
            .TermsDescription = "NET 10 DAYS MONTH END"
            .OrderDate = "05/24/13 06:45:52   *E*"
            .MerchandiseTotal = 74
        End With

        Dim numdetails As Int16 = 2

        ReDim docPrintingRequestType.DocumentHeader(1).InvoiceDetails(numdetails)
        For index As Int16 = 1 To numdetails
            docPrintingRequestType.DocumentHeader(1).InvoiceDetails(index) = New AbbConciseDocumentPrinting.InvoiceDetails
        Next

        With docPrintingRequestType.DocumentHeader(1).InvoiceDetails(1)
            .InvoiceLno = 1
            .QuantityShipped = 2
            .QuantityOrdered = 2
            .QuantityDue = 0
            .ItemCode = "BLSFT2070446"
            .ItemDescription = "SOFLENS TC 8.5 14.50 -2.00-0.75x030"
            .ItemDescription2 = "SOFLENS TORIC 6PK"
            .PatientName = "RIGHT"
            .UnitPrice = 18.5
            .ExtendedPrice = 37
        End With

        With docPrintingRequestType.DocumentHeader(1).InvoiceDetails(2)
            .InvoiceLno = 2
            .QuantityShipped = 2
            .QuantityOrdered = 2
            .QuantityDue = 0
            .ItemCode = "BLSFT2151821"
            .ItemDescription = "SOFLENS TC 8.5 14.50 +2.50-2.25x130"
            .ItemDescription2 = "SOFLENS TORIC 6PK"
            .PatientName = "LEFT"
            .UnitPrice = 18.5
            .ExtendedPrice = 37
        End With



        ReDim docPrintingRequestType.DocumentHeader(1).InvoiceAddresses(2)
        docPrintingRequestType.DocumentHeader(1).InvoiceAddresses(1) = New AbbConciseDocumentPrinting.Address
        docPrintingRequestType.DocumentHeader(1).InvoiceAddresses(2) = New AbbConciseDocumentPrinting.Address

        With docPrintingRequestType.DocumentHeader(1).InvoiceAddresses(1)
            .AddressType = AbbConciseDocumentPrinting.AddressTypes.BillTo
            .Name = "BAUSCH & LOMB"
            .Name2 = ""
            .Contact = ""
            .PhoneNumber = ""
            .FaxNumber = ""
            .AddressLine1 = "1400 N GOODMAN ST"
            .AddressLine2 = ""
            .AddressLine3 = String.Empty
            .City = "ROCHESTER"
            .StateProvinceCode = "NY"
            .PostalCode = "14609"
            .CountryCode = ""
        End With

        With docPrintingRequestType.DocumentHeader(1).InvoiceAddresses(2)
            .AddressType = AbbConciseDocumentPrinting.AddressTypes.ShipTo
            .Name = "PAULA SAUNDERS"
            .Contact = ""
            .PhoneNumber = String.Empty
            .FaxNumber = String.Empty
            .AddressLine1 = "1289 FENWAY CIRCLE"
            .AddressLine2 = String.Empty
            .AddressLine3 = String.Empty
            .City = "Decatur"
            .StateProvinceCode = "GA"
            .PostalCode = "30030"
            .CountryCode = ""
        End With

        ReDim docPrintingRequestType.DocumentHeader(1).InvoiceCharges(3)
        docPrintingRequestType.DocumentHeader(1).InvoiceCharges(1) = New AbbConciseDocumentPrinting.InvoiceHeaderCharges
        docPrintingRequestType.DocumentHeader(1).InvoiceCharges(2) = New AbbConciseDocumentPrinting.InvoiceHeaderCharges
        docPrintingRequestType.DocumentHeader(1).InvoiceCharges(3) = New AbbConciseDocumentPrinting.InvoiceHeaderCharges
        With docPrintingRequestType.DocumentHeader(1).InvoiceCharges(1)
            .SequenceNo = 1
            .ChargeDescription = "B&L Mail Innov"
            .ChargeAmount = 0
            .isSubTotalDivider = "N"
        End With

        With docPrintingRequestType.DocumentHeader(1).InvoiceCharges(2)
            .SequenceNo = 2
            .ChargeDescription = ""
            .ChargeAmount = 0
            .isSubTotalDivider = "Y"
        End With

        With docPrintingRequestType.DocumentHeader(1).InvoiceCharges(3)
            .SequenceNo = 3
            .ChargeDescription = "**** INVOICE TOTAL ****"
            .ChargeAmount = 74
            .isSubTotalDivider = "N"
        End With

        makeRequest(DocumentName, docPrintingRequestType)

    End Sub

    Private Sub PrintInvoiceDocumentSandraPatrick(ByVal DocumentName As String)

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
            .InvoiceDate = "06/11/2013"
            .InvoiceNo = "100425742701"
            .CustomerPONumber = "6976975"
            .CustomerNo = "1043955"
            .OrderNo = "1004257427"
            .ShipMethodDescription = "Mail Innovations"
            .OrderByCallerName = "SUSAN JONG OD"
            .TermsDescription = "NET 10 DAYS MONTH END"
            .OrderDate = "06/10/13 15:48:42   *E*"
            .MerchandiseTotal = 37.5
        End With

        Dim numdetails As Int16 = 2

        ReDim docPrintingRequestType.DocumentHeader(1).InvoiceDetails(numdetails)
        For index As Int16 = 1 To numdetails
            docPrintingRequestType.DocumentHeader(1).InvoiceDetails(index) = New AbbConciseDocumentPrinting.InvoiceDetails
        Next

        With docPrintingRequestType.DocumentHeader(1).InvoiceDetails(1)
            .InvoiceLno = 1
            .QuantityShipped = 1
            .QuantityOrdered = 1
            .QuantityDue = 0
            .ItemCode = "COOBXC000028"
            .ItemDescription = "BIOMED XC 6PK 8.5 14.20 +1.00"
            .ItemDescription2 = "BIOMED XC 6PK"
            .PatientName = "Left Eye"
            .UnitPrice = 18.75
            .ExtendedPrice = 18.75
        End With

        With docPrintingRequestType.DocumentHeader(1).InvoiceDetails(2)
            .InvoiceLno = 2
            .QuantityShipped = 1
            .QuantityOrdered = 1
            .QuantityDue = 0
            .ItemCode = "COOBXC000038"
            .ItemDescription = "BIOMED XC 6PK 8.5 14.20 +3.50"
            .ItemDescription2 = "BIOMED XC 6PK"
            .PatientName = "Left Eye"
            .UnitPrice = 18.75
            .ExtendedPrice = 18.75
        End With


        ReDim docPrintingRequestType.DocumentHeader(1).InvoiceAddresses(2)
        docPrintingRequestType.DocumentHeader(1).InvoiceAddresses(1) = New AbbConciseDocumentPrinting.Address
        docPrintingRequestType.DocumentHeader(1).InvoiceAddresses(2) = New AbbConciseDocumentPrinting.Address

        With docPrintingRequestType.DocumentHeader(1).InvoiceAddresses(1)
            .AddressType = AbbConciseDocumentPrinting.AddressTypes.BillTo
            .Name = "SUSAN JONG OD"
            .Name2 = "ADVANCED EYE CENTER INC"
            .Contact = ""
            .PhoneNumber = ""
            .FaxNumber = ""
            .AddressLine1 = "5151 BLUEBONNET BLVD"
            .AddressLine2 = ""
            .AddressLine3 = String.Empty
            .City = "BATON ROUGE"
            .StateProvinceCode = "LA"
            .PostalCode = "70809-3076"
            .CountryCode = ""
        End With

        With docPrintingRequestType.DocumentHeader(1).InvoiceAddresses(2)
            .AddressType = AbbConciseDocumentPrinting.AddressTypes.ShipTo
            .Name = "Sandra Patrick"
            .Contact = ""
            .PhoneNumber = String.Empty
            .FaxNumber = String.Empty
            .AddressLine1 = "15655 AIRLINE HWY UNIT 95"
            .AddressLine2 = String.Empty
            .AddressLine3 = String.Empty
            .City = "PRAIRIEVILLE"
            .StateProvinceCode = "LA"
            .PostalCode = "70769-2320"
            .CountryCode = ""
        End With

        ReDim docPrintingRequestType.DocumentHeader(1).InvoiceCharges(7)
        For iCtr As Int16 = 1 To 7
            docPrintingRequestType.DocumentHeader(1).InvoiceCharges(iCtr) = New AbbConciseDocumentPrinting.InvoiceHeaderCharges
        Next
        With docPrintingRequestType.DocumentHeader(1).InvoiceCharges(1)
            .SequenceNo = 1
            .ChargeDescription = "MAIL INNOVATIONS PATIENT GROUND"
            .ChargeAmount = 6.99
            .isSubTotalDivider = "N"
        End With

        With docPrintingRequestType.DocumentHeader(1).InvoiceCharges(2)
            .SequenceNo = 2
            .ChargeDescription = ""
            .ChargeAmount = 0
            .isSubTotalDivider = "Y"
        End With

        With docPrintingRequestType.DocumentHeader(1).InvoiceCharges(3)
            .SequenceNo = 3
            .ChargeDescription = "**** INVOICE TOTAL ****"
            .ChargeAmount = 44.49
            .isSubTotalDivider = "N"
        End With

        With docPrintingRequestType.DocumentHeader(1).InvoiceCharges(4)
            .SequenceNo = 4
            .ChargeDescription = ""
            .ChargeAmount = 0
            .isSubTotalDivider = "N"
        End With

        With docPrintingRequestType.DocumentHeader(1).InvoiceCharges(5)
            .SequenceNo = 5
            .ChargeDescription = "Discount Taken"
            .ChargeAmount = 0
            .isSubTotalDivider = "N"
        End With

        With docPrintingRequestType.DocumentHeader(1).InvoiceCharges(6)
            .SequenceNo = 6
            .ChargeDescription = "Amount Paid - CC"
            .ChargeAmount = 44.49
            .isSubTotalDivider = "N"
        End With

        With docPrintingRequestType.DocumentHeader(1).InvoiceCharges(7)
            .SequenceNo = 7
            .ChargeDescription = "**** BALANCE DUE ****"
            .ChargeAmount = 0
            .isSubTotalDivider = "N"
        End With

        makeRequest(DocumentName, docPrintingRequestType)

    End Sub

    Private Sub PrintInvoiceDocumentReneeDianimoore(ByVal DocumentName As String)

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
            .InvoiceDate = "06/10/2013"
            .InvoiceNo = "500948609301"
            .CustomerPONumber = "05411295491"
            .CustomerNo = "CW202054"
            .OrderNo = "5009486093"
            .ShipMethodDescription = "UOS GROUND KAISER"
            .OrderByCallerName = "Maureen Dickinson"
            .TermsDescription = "Contract Accounts"
            .OrderDate = "06/10/13 12:22:10   *E*"
            .MerchandiseTotal = 74
            .DisplayPricing = "N"
        End With

        Dim numdetails As Int16 = 2

        ReDim docPrintingRequestType.DocumentHeader(1).InvoiceDetails(numdetails)
        For index As Int16 = 1 To numdetails
            docPrintingRequestType.DocumentHeader(1).InvoiceDetails(index) = New AbbConciseDocumentPrinting.InvoiceDetails
        Next

        With docPrintingRequestType.DocumentHeader(1).InvoiceDetails(1)
            .InvoiceLno = 1
            .QuantityShipped = 1
            .QuantityOrdered = 1
            .QuantityDue = 0
            .ItemCode = "CIBFD902060540"
            .ItemDescription = "FOC DAILIES 90 PK 806 13.80-5.50"
            .ItemDescription2 = "FOCUS DAILIES AQUARELEASE 90PK"
            .PatientName = "05411295491"
            .UnitPrice = 18.5
            .ExtendedPrice = 37
        End With

        With docPrintingRequestType.DocumentHeader(1).InvoiceDetails(2)
            .InvoiceLno = 2
            .QuantityShipped = 1
            .QuantityOrdered = 1
            .QuantityDue = 0
            .ItemCode = "CIBFD902060542"
            .ItemDescription = "FOC DAILIES 90 PK 806 13.80-5.00"
            .ItemDescription2 = "FOCUS DAILIES AQUARELEASE 90PK"
            .PatientName = "05411295492"
            .UnitPrice = 18.5
            .ExtendedPrice = 37
        End With

        ReDim docPrintingRequestType.DocumentHeader(1).InvoiceAddresses(2)
        docPrintingRequestType.DocumentHeader(1).InvoiceAddresses(1) = New AbbConciseDocumentPrinting.Address
        docPrintingRequestType.DocumentHeader(1).InvoiceAddresses(2) = New AbbConciseDocumentPrinting.Address

        With docPrintingRequestType.DocumentHeader(1).InvoiceAddresses(1)
            .AddressType = AbbConciseDocumentPrinting.AddressTypes.BillTo
            .Name = "THE PERMANENTE MEDICAL GROUP"
            .Name2 = ""
            .Contact = ""
            .PhoneNumber = ""
            .FaxNumber = ""
            .AddressLine1 = "#NS84-1107 BRANCH 054"
            .AddressLine2 = "200 MUIR ROAD"
            .AddressLine3 = String.Empty
            .City = "MARTINEZ"
            .StateProvinceCode = "CA"
            .PostalCode = "94553"
            .CountryCode = ""
        End With

        With docPrintingRequestType.DocumentHeader(1).InvoiceAddresses(2)
            .AddressType = AbbConciseDocumentPrinting.AddressTypes.ShipTo
            .Name = "Renee Dianimoore"
            .Contact = ""
            .PhoneNumber = String.Empty
            .FaxNumber = String.Empty
            .AddressLine1 = "3726 SAN MICHELLE DR"
            .AddressLine2 = String.Empty
            .AddressLine3 = String.Empty
            .City = "CONCORD"
            .StateProvinceCode = "CA"
            .PostalCode = "94520-1335"
            .CountryCode = ""
        End With

        ReDim docPrintingRequestType.DocumentHeader(1).InvoiceCharges(3)
        docPrintingRequestType.DocumentHeader(1).InvoiceCharges(1) = New AbbConciseDocumentPrinting.InvoiceHeaderCharges
        docPrintingRequestType.DocumentHeader(1).InvoiceCharges(2) = New AbbConciseDocumentPrinting.InvoiceHeaderCharges
        docPrintingRequestType.DocumentHeader(1).InvoiceCharges(3) = New AbbConciseDocumentPrinting.InvoiceHeaderCharges
        With docPrintingRequestType.DocumentHeader(1).InvoiceCharges(1)
            .SequenceNo = 1
            .ChargeDescription = "B&L Mail Innov"
            .ChargeAmount = 0
            .isSubTotalDivider = "N"
        End With

        With docPrintingRequestType.DocumentHeader(1).InvoiceCharges(2)
            .SequenceNo = 2
            .ChargeDescription = ""
            .ChargeAmount = 0
            .isSubTotalDivider = "Y"
        End With

        With docPrintingRequestType.DocumentHeader(1).InvoiceCharges(3)
            .SequenceNo = 3
            .ChargeDescription = "**** INVOICE TOTAL ****"
            .ChargeAmount = 74
            .isSubTotalDivider = "N"
        End With

        makeRequest(DocumentName, docPrintingRequestType)

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

    Private Sub PrintInvoiceDocumentFields(ByVal DocumentName As String)

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
            .BillToAccount = "BTA"
            .Carrier = "CARR"
            .CustomerID = "CustID"
            .CustomerNo = "CustNo"
            .CustomerOrderID = "CustOrdId"
            .CustomerPONumber = "CustPO"
            .EdiReferenceNumber = "EDI Cust Ref"
            .HeaderBodyText = "HeadBodyText"
            .ImportedPatientDiscount = 0
            .ImportedPatientFreight = 0
            .ImportedPatientInvoiceSales = 0
            .ImportedPatientSalesTax = 0
            .ImportedPatientTotalSales = 0
            .InvoiceDate = "Inv Date"
            .InvoiceFreight = 0
            .InvoiceMiscSales = 0
            .InvoiceNo = DocumentName '"Inv No"
            .InvoicePrintDate = "Inv P Date"
            .InvoiceSalesTax = 0
            .InvoiceTotalSales = 0
            .InvoiceType = "I"
            .IsReprint = "Y"
            .MerchandiseTotal = 0
            .OfficeWebSite = "OfficeWebSite"
            .OrderByCallerName = "OCallerName"
            .OrderComment = "OrderComment"
            .OrderDate = "Ord Date"
            .OrderNo = "Ord No"
            .OrderSource = "Ord Source"
            .OrderTakenBy = "OTB"
            .PackSlipFreight = 0
            .PackSlipFuelSurcharge = 0
            .PackSlipSalesTax = 0
            .PackSlipTotal = 0
            .PartnerOrderOrigin = "POOrigin"
            .PatientDifference = 0
            .PatientDiscount = 0
            .PatientFreight = 0
            .PatientID = ""
            .PatientInvoiceSales = 0
            .PatientSalesTax = 0
            .PatientTotalSales = 0
            .PaymentMethod = "Pmeth"
            .PrescribingDoctor = "PreDoct"
            .PromoCode = "PrCode"
            .ProviderNumber = "ProNum"
            .RebateText = "RebateText"
            .SalesRepCode = "SRC"
            .ShipmentNumber = "ShipmentNo"
            .ShipMethodDescription = "SMDesc"
            .ShipMethodWebDescription = "SMWDesc"
            .ShipToNo = "STNo"
            .ShipToPatient = "Y"
            .ShipVia = "SVia"
            .TermsDescription = "Terms"
            .TransmissionDate = "TrDate"
            .WebOrderNo = "WONo"
            .WDSIIInvoiceNumber = "WDSInvNo"
            .DisplayPricing = "Y"
        End With

        Dim InvoiceCharges As Int16 = 3
        ReDim docPrintingRequestType.DocumentHeader(1).InvoiceCharges(InvoiceCharges)

        For SequenceNo As Int16 = 1 To InvoiceCharges
            docPrintingRequestType.DocumentHeader(1).InvoiceCharges(SequenceNo) = New AbbConciseDocumentPrinting.InvoiceHeaderCharges
            With docPrintingRequestType.DocumentHeader(1).InvoiceCharges(SequenceNo)
                .ChargeAmount = SequenceNo
                .ChargeDescription = "InvoiceCharges.ChargeDescription"
                .SequenceNo = SequenceNo
            End With
        Next

        Dim lbs As Int16 = 3
        ReDim docPrintingRequestType.DocumentHeader(1).InvoiceLensBankSummary(lbs)
        For lb As Int16 = 1 To lbs
            docPrintingRequestType.DocumentHeader(1).InvoiceLensBankSummary(lb) = New AbbConciseDocumentPrinting.InvoiceLensBankSummary
            With docPrintingRequestType.DocumentHeader(1).InvoiceLensBankSummary(lb)
                .Description = "This will be all the text to display as the LensBank Description"
                .SequenceNo = lb
            End With
        Next

        Dim numdetails As Int16 = 6
        If DocumentName.StartsWith("SOR") Then
            numdetails = 2
        End If
        Dim MerchandiseTotal As Decimal = 0
        ReDim docPrintingRequestType.DocumentHeader(1).InvoiceDetails(numdetails)
        For InvoiceLno As Int16 = 1 To numdetails
            docPrintingRequestType.DocumentHeader(1).InvoiceDetails(InvoiceLno) = New AbbConciseDocumentPrinting.InvoiceDetails
            With docPrintingRequestType.DocumentHeader(1).InvoiceDetails(InvoiceLno)
                .AddPower = "Add"
                .Axis = "Axis"
                .BaseCurve = "Bcurve"
                .Color = "Color"
                .Cylinder = "Cyl"
                .Diameter = "Dia"
                .InvoiceLno = InvoiceLno
                .ItemBinLocation = "Bin Loc"
                .ItemCode = "Item Code"
                .ItemDescription = "Item Desc"
                .ItemDescription2 = "Item Desc 2"
                .LeftRightIndicator = "L"
                .OrderGroup = "OGroup"
                .PatientExtendedPrice = 0
                .PatientName = "PatientName"
                .PatientPrice = 0
                .PatientReference = "PatRef"
                .QuantityDue = 0
                .QuantityOrdered = InvoiceLno
                .QuantityShipped = InvoiceLno
                .SpherePower = "Sph"
                .UnitPrice = CInt(Math.Ceiling(Rnd() * 20))
                .ExtendedPrice = .UnitPrice * .QuantityShipped
                MerchandiseTotal += .ExtendedPrice
                .WebItemDescription = "WebItemDescription"
            End With

            With docPrintingRequestType.DocumentHeader(1)
                .MerchandiseTotal = MerchandiseTotal
                .InvoiceSalesTax = 2.25
                .InvoiceMiscSales = 3.95
                .InvoiceFreight = 5
                .InvoiceTotalSales = .MerchandiseTotal + .InvoiceSalesTax + .InvoiceMiscSales + .InvoiceFreight
            End With

            ReDim docPrintingRequestType.DocumentHeader(1).InvoiceDetails(InvoiceLno).DetailAddons(1)
            docPrintingRequestType.DocumentHeader(1).InvoiceDetails(InvoiceLno).DetailAddons(1) = New AbbConciseDocumentPrinting.InvoiceDetailAddon
            With docPrintingRequestType.DocumentHeader(1).InvoiceDetails(InvoiceLno).DetailAddons(1)
                .AddonAmount = CInt(Math.Ceiling(Rnd() * 9))
                .AddonDescription = "DetailAddons.AddonDescription"
                .SequenceNo = 1
            End With
        Next

        ReDim docPrintingRequestType.DocumentHeader(1).InvoiceAddresses(6)
        Dim lbl As String() = New String() {"ShipFrom", "BillTo", "ShipTo", "ReturnTo", "Office", "BillingOffice"}
        Dim numAddresses = lbl.Length

        For addrNum As Int16 = 1 To numAddresses - 1
            docPrintingRequestType.DocumentHeader(1).InvoiceAddresses(addrNum) = New AbbConciseDocumentPrinting.Address
            Dim prefix As String = lbl(addrNum - 1)

            With docPrintingRequestType.DocumentHeader(1).InvoiceAddresses(addrNum)

                Select Case addrNum
                    Case 1 : .AddressType = AbbConciseDocumentPrinting.AddressTypes.ShipFrom
                    Case 2 : .AddressType = AbbConciseDocumentPrinting.AddressTypes.BillTo
                    Case 3 : .AddressType = AbbConciseDocumentPrinting.AddressTypes.ShipTo
                    Case 4 : .AddressType = AbbConciseDocumentPrinting.AddressTypes.ReturnTo
                    Case 5 : .AddressType = AbbConciseDocumentPrinting.AddressTypes.Office
                    Case 6 : .AddressType = AbbConciseDocumentPrinting.AddressTypes.BillingOffice
                End Select

                .AddressLine1 = prefix & " AddressLine1"
                .AddressLine2 = prefix & " AddressLine2"
                .AddressLine3 = prefix & " AddressLine3"
                .City = prefix & " City"
                .Contact = prefix & " Contact"
                .CountryCode = prefix & " Country"
                .EmailAddress = prefix & " Email address"
                .FaxNumber = prefix & " FaxNumber"
                .Name = prefix & " Name"
                .Name2 = prefix & " Name2"
                .PhoneNumber = prefix & " PhoneNumber"
                .PostalCode = prefix & " PostalCode"
                .StateProvinceCode = prefix & " State"
            End With
        Next

        Dim objDocumentPrinting As New AbbConciseDocumentPrinting.AbbConciseDocumentPrinting
        'objDocumentPrinting.Url = "http://192.168.130.190/abbogdocumentprinting/AbbConciseDocumentPrinting.asmx"
        Dim docResponse As AbbConciseDocumentPrinting.DocumentPrintingResponseType = objDocumentPrinting.LoadAndRequestDocumentObject("", docPrintingRequestType)

        If docResponse.ResponseCode <> 0 Then
            TextBox1.Text = "Document (" & DocumentName & "): " & docResponse.ResponseMessage & Environment.NewLine & TextBox1.Text
        Else
            TextBox1.Text = "Done" & Environment.NewLine & TextBox1.Text
        End If

        'Dim pdfpath As String = "c:\temp\" & DocumentName & "_" & System.Guid.NewGuid.ToString() & ".pdf"
        'Dim bb As Byte() = Convert.FromBase64String(docResponse.ResponseDocumentText)

        'Using fs As IO.FileStream = New IO.FileStream(pdfpath, IO.FileMode.OpenOrCreate, IO.FileAccess.Write)
        '    Using bw As IO.BinaryWriter = New IO.BinaryWriter(fs)
        '        bw.Write(bb)
        '        bw.Flush()
        '        bw.Close()
        '    End Using
        '    fs.Close()
        '    fs.Dispose()
        'End Using

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
                            MyXMLNode.InnerText = "192.168.141.63:9100" ' "192.168.141.191:9100" ' "\\sstringer2-nb\ZDesignerGX420d" ' "192.168.142.49:9100" ' Can you try 192.168.2.66? This is a label printer in the UAT test lab. "192.168.142.49:9100" '"192.168.135.200:9100" 
                        End If
                    End If

                    'ReturnPDF
                    MyXMLNode = xmlDoc.SelectSingleNode("/LoadAndRequestDocumentObjectSoapIn/parameters/DocumentPrintingRequestType/PrintRequestInfo/ReturnPDF")
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

    Private Sub printDocUsingXMLDirectory()

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

        For Each xmlFile As String In My.Computer.FileSystem.GetFiles("C:\VS\AbbConciseDocumentPrinting\AbbConciseDocumentPrinting\XML")

            Try

                If Not xmlFile.EndsWith("FORM11.XML") Then
                    Continue For
                End If

                Dim xmlDoc As New XmlDocument

                TextBox1.AppendText(Environment.NewLine & "Processing file " & xmlFile & Environment.NewLine)

                If My.Computer.FileSystem.FileExists(xmlFile) Then
                    xmlDoc.Load(xmlFile)
                    Dim MyXMLNode As XmlNode = xmlDoc.SelectSingleNode("/DocumentPrintingRequestType/PrintRequestInfo/ReportName")
                    If MyXMLNode IsNot Nothing Then
                        'MyXMLNode.ChildNodes(0).InnerText = formName
                        'xmlDoc.Save(xmlFile)
                    End If

                    MyXMLNode = xmlDoc.SelectSingleNode("/DocumentPrintingRequestType/PrintRequestInfo/PrinterIp")
                    If MyXMLNode IsNot Nothing Then
                        MyXMLNode.InnerText = "192.168.2.223:9100" '"192.168.142.65:9100" ' "192.168.100.88:9100" 
                        xmlDoc.Save(xmlFile)
                    End If

                    'ReturnPDF
                    MyXMLNode = xmlDoc.SelectSingleNode("/DocumentPrintingRequestType/PrintRequestInfo/ReturnPDF")
                    If MyXMLNode IsNot Nothing Then
                        MyXMLNode.InnerText = "Y"
                        'xmlDoc.Save(xmlFile)
                    End If

                    'xmlDoc.Save(xmlFile)
                End If

                TextBox1.AppendText(Environment.NewLine & "Requesting form " & xmlFile & Environment.NewLine)

                Application.DoEvents()

                ' Wait 10 seconds to the IP Address is not flooded with data.
                If count > 0 Then
                    TextBox1.AppendText("Waiting 2 Seconds between requests!" & Environment.NewLine)
                    Application.DoEvents()
                    System.Threading.Thread.Sleep(2000)
                End If
                count += 1

                Dim docResponse As XmlNode = objDocumentPrinting.LoadAndRequestDocument("", xmlDoc)

                Dim respNode As XmlNode = docResponse.SelectSingleNode("/Response/ResponseCode")
                Dim ResponseCode As Int16 = respNode.InnerText

                respNode = docResponse.SelectSingleNode("/Response/ResponseMessage")
                Dim ResponseMessage As String = respNode.InnerText

                respNode = docResponse.SelectSingleNode("/Response/ResponseDocument")
                Dim responseDocumentText As String = respNode.InnerText

                If ResponseCode <> 0 Then
                    TextBox1.AppendText("Service error processing form (" & xmlFile & "): " & ResponseMessage & Environment.NewLine)
                    'MessageBox.Show(formName & ": " & ResponseMessage)
                Else
                    TextBox1.AppendText("Service is printing form " & xmlFile & Environment.NewLine)
                End If

                Application.DoEvents()


                Dim pdfpath As String = "c:\temp\" & System.Guid.NewGuid.ToString() & ".pdf"
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
            Catch ex As Exception
                TextBox1.AppendText(ex.Message & Environment.NewLine)
            End Try

        Next
        TextBox1.AppendText(Environment.NewLine & "Process Completed" & Environment.NewLine)

    End Sub

    Private Sub printDocUsingTestXML()

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

        Dim formName As String = String.Empty

        For Each xmlFile As String In My.Computer.FileSystem.GetFiles("C:\VS\AbbConciseDocumentPrinting\TestDocs", FileIO.SearchOption.SearchTopLevelOnly, "*.xml")

            Try
                Dim xmlDoc As New XmlDocument
                Application.DoEvents()
                Application.DoEvents()
                Application.DoEvents()
                Application.DoEvents()

                If Not My.Computer.FileSystem.FileExists(xmlFile) Then
                    TextBox1.AppendText(Environment.NewLine & "Cannot locate File: " & xmlFile & Environment.NewLine)
                    Continue For
                End If

                TextBox1.AppendText(Environment.NewLine & "Filename: " & xmlFile & Environment.NewLine)
                xmlDoc.Load(xmlFile)
                TextBox1.AppendText(Environment.NewLine & "Requesting form " & formName & Environment.NewLine)

                Application.DoEvents()

                ' Wait 10 seconds to the IP Address is not flooded with data.
                If count > 0 Then
                    'TextBox1.AppendText("Waiting 10 Seconds between requests!" & Environment.NewLine)
                    Application.DoEvents()
                    'System.Threading.Thread.Sleep(10000)
                End If
                count += 1

                TextBox1.AppendText(Environment.NewLine & "Start Request " & DateTime.Now & Environment.NewLine)

                objDocumentPrinting.Timeout = 180000 ' milliseconds
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

    Private Sub makeRequest(ByVal DocumentName As String, ByVal docPrintingRequestType As AbbConciseDocumentPrinting.DocumentPrintingRequestType)

        Dim objDocumentPrinting As New AbbConciseDocumentPrinting.AbbConciseDocumentPrinting
        'http://192.168.130.190/abbogdocumentprinting/AbbConciseDocumentPrinting.asmx
        'objDocumentPrinting.Url = "http://192.168.130.190/abbogdocumentprinting/AbbConciseDocumentPrinting.asmx"
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

    Private Sub btnPrintUAT_Click(sender As Object, e As EventArgs) Handles btnPrintUAT.Click
        printDocUsingXMLDirectory()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        printDocUsingTestXML()
    End Sub

End Class

Public Class Clients

    Dim cCLIENT_CODE As String
    Dim cCLIENT_NAME As String
    Dim cCLIENT_ADDR_L1 As String
    Dim cCLIENT_ADDR_L2 As String
    Dim cCLIENT_CITY As String
    Dim cCLIENT_STATE_ID As String
    Dim cCLIENT_ZIP_CODE As String
    Dim cINACTIVE_IND As String
    Dim cYYYYMM_1ST_BILL As String
    Dim cYYYYMM_RATE_CHG As String
    Dim cSYS_ANALYST_ID As String
    Dim cPHONE_NO_VOICE As String
    Dim cPHONE_NO_DATA As String
    Dim cPHONE_NO_FAX As String
    Dim cCLIENT_CONTACT As String
    Dim cCLIENT_RECPTNST As String
    Dim cCLIENT_VP_MIS As String
    Dim cCLIENT_RATE_HR As Double
    Dim cPROJECT_CODE_REQD As String
    Dim cCLIENT_BILLING_NOTE As String
    Dim cCLIENT_EMAIL_INVOICE As String
    Dim cCLIENT_EMAIL As String
    Dim cCLIENT_CC As String
    Dim cCLIENT_ABBR As String
    Dim cCLIENT_SALUTATION As String

    Public Sub New()

    End Sub

    Public Property CLIENT_CODE As String
        Get
            Return cCLIENT_CODE
        End Get
        Set(value As String)
            cCLIENT_CODE = value
        End Set
    End Property

    Public Property CLIENT_NAME As String
        Get
            Return cCLIENT_NAME
        End Get
        Set(value As String)
            cCLIENT_NAME = value
        End Set
    End Property

    Public Property CLIENT_ADDR_L1 As String
        Get
            Return cCLIENT_ADDR_L1
        End Get
        Set(value As String)
            cCLIENT_ADDR_L1 = value
        End Set
    End Property

    Public Property CLIENT_ADDR_L2 As String
        Get
            Return cCLIENT_ADDR_L2
        End Get
        Set(value As String)
            cCLIENT_ADDR_L2 = value
        End Set
    End Property

    Public Property CLIENT_CITY As String
        Get
            Return cCLIENT_CITY
        End Get
        Set(value As String)
            cCLIENT_CITY = value
        End Set
    End Property

    Public Property CLIENT_STATE_ID As String
        Get
            Return cCLIENT_STATE_ID
        End Get
        Set(value As String)
            cCLIENT_STATE_ID = value
        End Set
    End Property

    Public Property CLIENT_ZIP_CODE As String
        Get
            Return cCLIENT_ZIP_CODE
        End Get
        Set(value As String)
            cCLIENT_ZIP_CODE = value
        End Set
    End Property

    Public Property INACTIVE_IND As String
        Get
            Return cINACTIVE_IND
        End Get
        Set(value As String)
            cINACTIVE_IND = value
        End Set
    End Property

    Public Property YYYYMM_1ST_BILL As String
        Get
            Return cYYYYMM_1ST_BILL
        End Get
        Set(value As String)
            cYYYYMM_1ST_BILL = value
        End Set
    End Property

    Public Property YYYYMM_RATE_CHG As String
        Get
            Return cYYYYMM_RATE_CHG
        End Get
        Set(value As String)
            cYYYYMM_RATE_CHG = value
        End Set
    End Property

    Public Property SYS_ANALYST_ID As String
        Get
            Return cSYS_ANALYST_ID
        End Get
        Set(value As String)
            cSYS_ANALYST_ID = value
        End Set
    End Property

    Public Property PHONE_NO_VOICE As String
        Get
            Return cPHONE_NO_VOICE
        End Get
        Set(value As String)
            cPHONE_NO_VOICE = value
        End Set
    End Property

    Public Property PHONE_NO_DATA As String
        Get
            Return cPHONE_NO_DATA
        End Get
        Set(value As String)
            cPHONE_NO_DATA = value
        End Set
    End Property

    Public Property PHONE_NO_FAX As String
        Get
            Return cPHONE_NO_FAX
        End Get
        Set(value As String)
            cPHONE_NO_FAX = value
        End Set
    End Property

    Public Property CLIENT_CONTACT As String
        Get
            Return cCLIENT_CONTACT
        End Get
        Set(value As String)
            cCLIENT_CONTACT = value
        End Set
    End Property

    Public Property CLIENT_RECPTNST As String
        Get
            Return cCLIENT_RECPTNST
        End Get
        Set(value As String)
            cCLIENT_RECPTNST = value
        End Set
    End Property

    Public Property CLIENT_VP_MIS As String
        Get
            Return cCLIENT_VP_MIS
        End Get
        Set(value As String)
            cCLIENT_VP_MIS = value
        End Set
    End Property

    Public Property CLIENT_RATE_HR As Double
        Get
            Return cCLIENT_RATE_HR
        End Get
        Set(value As Double)
            cCLIENT_RATE_HR = value
        End Set
    End Property

    Public Property PROJECT_CODE_REQD As String
        Get
            Return cPROJECT_CODE_REQD
        End Get
        Set(value As String)
            cPROJECT_CODE_REQD = value
        End Set
    End Property

    Public Property CLIENT_BILLING_NOTE As String
        Get
            Return cCLIENT_BILLING_NOTE
        End Get
        Set(value As String)
            cCLIENT_BILLING_NOTE = value
        End Set
    End Property

    Public Property CLIENT_EMAIL_INVOICE As String
        Get
            Return cCLIENT_EMAIL_INVOICE
        End Get
        Set(value As String)
            cCLIENT_EMAIL_INVOICE = value
        End Set
    End Property

    Public Property CLIENT_EMAIL As String
        Get
            Return cCLIENT_EMAIL
        End Get
        Set(value As String)
            cCLIENT_EMAIL = value
        End Set
    End Property

    Public Property CLIENT_CC As String
        Get
            Return cCLIENT_CC
        End Get
        Set(value As String)
            cCLIENT_CC = value
        End Set
    End Property

    Public Property CLIENT_ABBR As String
        Get
            Return cCLIENT_ABBR
        End Get
        Set(value As String)
            cCLIENT_ABBR = value
        End Set
    End Property

    Public Property CLIENT_SALUTATION As String
        Get
            Return cCLIENT_SALUTATION
        End Get
        Set(value As String)
            cCLIENT_SALUTATION = value
        End Set
    End Property

End Class