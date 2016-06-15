<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="BlueChappie.WebForm1" %><%@ Register assembly="BlueChappie" namespace="BlueChappie" tagprefix="cc1" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta name="description" content="The description of my page" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <link href="Content/Site.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .auto-style1 {
            width: 100%;
        }
    </style>
    <script src="Scripts/jquery-1.10.2.js"></script>
    <script src="js/imageinfo.js"></script>
</head>
<body id="imgForm">
    <form id="form1" runat="server">
        <div class="imageinfo" id="imginfo">




            <table class="auto-style1">
                <tr>
                    <td rowspan="12" style="width:50%"><p id="img">[Image]</p></td>
                    <td><p class="imgtxtinfo">Title</p></td>
                    <td><p class="imgtxtdata" id="imgtitle"></p></td>
                    <td><img src="/images/close.png" onclick="closeimginfo();"/></td>
                </tr>
                <tr>
                    <td><p class="imgtxtinfo">Tags</p></td>
                    <td><p class="imgtxtdata" id="imgtags"></p></td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td><p class="imgtxtinfo">Owner</p></td>
                    <td><p class="imgtxtdata" id="imgowner"></p></td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td><p class="imgtxtinfo">Date Taken</p></td>
                    <td><p class="imgtxtdata" id="imgdatetaken"></p></td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td><p class="imgtxtinfo">Date Found</p></td>
                    <td><p class="imgtxtdata" id="imgdatehit"></p></td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td><p class="imgtxtinfo">Live Link</p></td>
                    <td><p class="imgtxtdata" id="imgsourceurl"></p></td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td><p class="imgtxtinfo">Source</p></td>
                    <td><p class="imgtxtdata" id="imgsource"></p></td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td><p class="imgtxtinfo">Local Tag</p></td>
                    <td><p class="imgtxtdata" id="imgtag"></p></td>
                    <td>&nbsp;</td>
                </tr>
               
            </table>




        </div>
    <div style="width:1010px;margin:0 auto;" >
        <table id="divform">
            <tr>
                <td><img src="Images/Durban.jpg" alt="Durban" height="330" width="250"/></td>
                <td><img src="Images/CapeTown.jpg" alt="Cape Town" height="330" width="250"/></td>
                <td><img src="Images/Pretoria.jpg" alt="Pretoria" height="330" width="250"/></td>
                <td><img src="Images/Johannesburg.jpg" alt="Johannesburg" height="330" width="250"/></td>

            </tr>

        </table>

       
        <asp:TextBox ID="txtLocation" runat="server" Width="239px"></asp:TextBox>
       
        <asp:Button ID="btnLookUpAndSync" runat="server" OnClick="btnLookUpAndSync_Click" Text="Locate Images" Width="250px" />
    
       
        <cc1:ctrlImages ID="ctrlImages1" runat="server" />
       
    
    </div>
    </form>
    <p>
        &nbsp;</p>
    <p>
        &nbsp;</p>
</body>
</html>
