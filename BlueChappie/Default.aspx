<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="BlueChappie.WebForm1" %>
<%@ Register assembly="BlueChappie" namespace="BlueChappie" tagprefix="cc1" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Blue Chappie</title>
    <meta name="description" content="The description of my page" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <link href="Content/Site.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/jquery-1.10.2.js"></script>
    <script src="js/userinfo.js"></script>
    <script src="js/imageinfo.js"></script>
    
    <%--<script>
        $("tbllocations").slideUp(0)
        $("divuser").slideUp(0)
    </script>--%>
</head>
<body id="imgForm">
    <form id="form1" runat="server">
        <div id="mainForm">
        <div class="imageinfo" id="imginfo">
            <table style="width:100%">
                <tr >
                    <td rowspan="2"  ><p id="img" class="currentimage">[Image]</p></td>
                    <td >
                    <table ><tr>
                        <td><p class="imgtxtinfo">Title</p></td>
                        <td><p class="imgtxtdata" id="imgtitle"></p></td>
                        
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
                        <td><p class="imgtxtinfo">Local Tag</p></td>
                        <td><p class="imgtxtdata" id="imgtag"></p></td>
                        <td>&nbsp;</td>
                    </tr></table></td>
                    <td style="vertical-align:top;width:25px"><img style="padding-top:5px;height:20px;width:20px" src="images/close.png" onclick="closeimginfo();"/></td></tr>
                    <tr ><td colspan="2" style ="border-style:solid;border-color:#232323;border-radius:15px;padding-left:10px;color:white">&nbsp; <br/> &nbsp; <br/> &nbsp; <br/> &nbsp; <br/> &nbsp; <br/>&nbsp; <br/>&nbsp; <br/>&nbsp; <br/></td>

                </tr>
               
            </table>




        </div>
    <div style="width:1010px;margin:0 auto;" >
        <table id="divform">
            <tr>
                <td><img src="Images/Durban.jpg" alt="Durban" height="330" width="250" onclick="imgSelectLocation('durban');"/></td>
                <td><img src="Images/CapeTown.jpg" alt="Cape Town" height="330" width="250" onclick="imgSelectLocation('cape town');"/></td>
                <td><img src="Images/Pretoria.jpg" alt="Pretoria" height="330" width="250" onclick="imgSelectLocation('pretoria');"/></td>
                <td><img src="Images/Johannesburg.jpg" alt="Johannesburg" height="330" width="250" onclick="imgSelectLocation('johannesburg');"/></td>

            </tr>

        </table>

     
       
        <br />
        <table style="border-color:white;border-style:solid;width:100%"><tr>
            <td>
            <p onclick="showhideLocations()" id="locClick" class="sitebuttons">Show/Hide uploaded locations</p>
            </td>
            <td>
            <p onclick="showhideUserLocations()" id="usrlocClick" class="sitebuttons">Show/Hide favorite locations</p>
            </td>
            <td>
            
                <asp:Label ID="lblUserClick" onclick="showhideLogin()" runat="server" Text="Login" class="sitebuttons" Width="80%"></asp:Label>
                <asp:Label ID="lblUserEmail" runat="server" Text="Login" class="sitebuttons" Width="80%" Visible="False"></asp:Label>
            </td>
            </tr>
        </table>
        <div id="divloc" class="locations">
        <div id="divlocations" class="locations"  >
            <cc1:ctlLocations  Id="ctlLocations1" runat="server" style="width:100%"/>
            </div>

                <div style="centreScreen">
                  <table style="align-self:center;width:100%">
                      <tr><th colspan="3">BlueChapppie will run an online scan for images based on the location keyword you provide below:</th></tr>
                        <tr><td style="text-align:right"><asp:TextBox ID="txtLocation" runat="server" Width="239px"></asp:TextBox></td>
                            <td style="text-align:center">
       
                        <asp:Button ID="btnLookUpAndSync" runat="server" OnClick="btnLookUpAndSync_Click" Text="Check online for more images." Width="250px" CssClass="sitebuttons" OnClientClick="showWaitScreen();" />
                            </td>
                            <td style="text-align:left">This will be a lengthy process</td>
                        </tr>
                    </table>
                </div>
            
        </div>
        <div id="divUserlocations" class="locations"  >
            <cc1:ctlUserLocations  Id="ctlUserLocations1" runat="server" style="width:100%"/>
            
        </div>

        <div id="divUser" class="divuser"  >
            <table>
                <tr><th colspan="2">Please provide login details</th></tr>
                <tr><td>Email address</td><td><input id="emailaddress" type="email" /></td></tr>
                <tr><td>Password</td><td><input id="password" type="password" /></td></tr>
                <tr><th colspan="2" style="color:red" id="loginError">&nbsp</th></tr>
                <tr><td colspan="2"><p onclick="getUserData()" id="login" class="sitebuttons">Login</p></td></tr>
            </table>
        </div>
       
        <cc1:ctrlImages ID="ctrlImages1" runat="server" />
       
    
    </div>
    <p>
        <asp:HiddenField ID="hvUserEmailAddress" runat="server" />
        <asp:HiddenField ID="hvUserId" runat="server" />
        </p>
            
        <div id="waitScreen" class="waitscreen">
            <img src="Images/ajax-loader.gif" />
        </div>
    </form>
    
</body>
</html>
