
$(document).ready(function () {
    $(document.getElementById("divloc")).slideUp(20);
    $(document.getElementById("divlocations")).slideUp(20);
    $(document.getElementById("divUser")).slideUp(20);
    $(document).ajaxStart(function () { showWaitScreen(); });
    $(document.getElementById("divUserlocations")).slideUp(20);
    $(document).ajaxComplete(function(){removeWaitScreen();});
    $("img").hover(function(){
      
        $(this).css("border-color", "#000099");
        $(this).css("cursor", "pointer");
      
        }, function(){
      
        $(this).css("border-color", "transparent");
        });
    var userid =  getCookie('UserId')
    if (userid != "") { document.getElementById("usrlocClick").style.visibility = "visible"; } else { document.getElementById("usrlocClick").style.visibility = "hidden" };
    if (getCookie("EmailAddress") != ""){ document.getElementById("lblUserClick").innerHTML= getCookie("EmailAddress");}
});
function showhideLocations()
{
    document.getElementById("Log2").innerHTML = "Loading locations";
      $(document).ajaxStart(function () { showWaitScreen(); });
      if (document.getElementById("Log").innerHTML != "Idle...") {
          $(document.getElementById("userScan")).slideUp(100);
      } else {
              $(document.getElementById("userScan")).slideDown(100);
      }
     var usr_id =  getCookie('UserId')
          $.ajax({
             url:  window.location.href + "api/LocationsHTML?UserId=" +  usr_id
         }).then(function (data) {
             document.getElementById("divlocations").innerHTML = data;
         }
         )
          
     $(document.getElementById("divUser")).slideUp(100);
     $(document.getElementById("divUserlocations")).slideUp(100);
     $(document.getElementById("divloc")).slideToggle(200);
     $(document.getElementById("divlocations")).slideDown(200);
     if (document.getElementById("Log").innerHTML != "Idle...") {
              $(document.getElementById("userScan")).slideUp(100);
          }
    $(document).ajaxComplete(function(){removeWaitScreen();});
    document.getElementById("Log2").innerHTML = "Idle...";
}


function imgSelectLocation(location) {
    document.getElementById("Log2").innerHTML = "Loading " + location;
    $(document).ajaxStart(function () { showWaitScreen(); });
    $.ajax({
        url:  window.location.href + "api/MainImageListHTML?Location=" + location 
    }).then(function (data) {
        document.getElementById("divMainImageList").innerHTML = data;
    })
    $(document.getElementById("divlocations")).slideUp(100);
    $(document.getElementById("divloc")).slideUp(100);
    $(document.getElementById("divUserlocations")).slideUp(100);
    $(document.getElementById("divUser")).slideUp(100);
    $(document).ajaxComplete(function () { removeWaitScreen(); });
    document.getElementById("Log2").innerHTML = "Idle...";
}
function scanOnline() {
    var slocation = document.getElementById("txtLocation").value;
    if (slocation.length < 5) {
        alert("Please enter some search criteria with a length more than 4 characters!")
    } else {
        $(document.getElementById("userScan")).slideUp(500);
        document.getElementById("Log").innerHTML = "Scan for " + slocation + " started <br/>";
        var _guid = 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
            var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
            return v.toString(16);
        }).replace("-","") ;
        setCookie('ClientRequestId', _guid, 0.1);
        document.getElementById("txtLocation").value = "";
        document.getElementById("Log").innerHTML = "Scan Initiated...";
        checkStatus();
        $.ajax({
            type: "POST",
            async: true,
            url: window.location.href + "api/ScanOnline?Location=" + slocation + "&ClientRequestId=" + _guid
        }).then(function (data) {
            document.getElementById("Log").innerHTML = "<br />Scan for " + slocation + " completed" + document.getElementById("Log").innerHTML;
            imgSelectLocation(location);
        })
        document.getElementById("txtLocation").value = "";
    }
}
function checkStatus() {
    try {
        window.clearInterval(tmr);
    } catch (e) {

    }
    if (document.getElementById("Log").innerHTML != "Idle...") {
        $.ajax({
            url: window.location.href + "api/ScanOnline?ClientRequestId=" + getCookie('ClientRequestId')
        }).then(function (data) {
            document.getElementById("Log").innerHTML = data;
            if (data.substring(0, 10) == "Completed ") {
                window.clearInterval(tmr);
                document.getElementById("Log").innerHTML = "Idle...";
                imgSelectLocation(data.replace("Completed ", ""));
            } else {
                if (document.getElementById("Log").innerHTML != "Idle...")
                { var tmr = setInterval(checkStatus, 1500); 
                }
            }
        })
    }
}
function getImgData(imgGUID)
    {
    $(document.getElementById("divlocations")).slideUp(20);
        $(document.getElementById("divloc")).slideUp(20);
    $(document.getElementById("divUser")).slideUp(20);
    $(document.getElementById("divUserlocations")).slideUp(20);
    $.ajax({
           
            url:  window.location.href + "api/ImageInfo?imgGUID=" + imgGUID
        }).then(function(data) {
            document.getElementById("img").innerHTML = '<img class="img" src="data:image/jpg;base64,' + data.webImageBase64Encoded + '">';
            document.getElementById("imgtitle").innerHTML=data.title;
            document.getElementById("imgtags").innerHTML=data.tags;
            document.getElementById("imgowner").innerHTML=data.owner;
            document.getElementById("imgdatetaken").innerHTML=data.dateTaken;
            document.getElementById("imgdatehit").innerHTML = data.dateHit;
            document.getElementById("imgtag").innerHTML = data.tag;
            document.getElementById("imginfo").style.visibility = "visible";
        });

}
function closeimginfo() {
    document.getElementById("imginfo").style.visibility = "hidden";
}
function showWaitScreen() {
    $(document.getElementById("mainForm")).css("cursor", "wait");
    $(document.getElementById("mainForm")).css("opacity", "0.7");
    $(document.getElementById("waitScreen")).css("top", "0");
    $(document.getElementById("waitScreen")).css("left", "0");
    $(document.getElementById("waitScreen")).css("height", "100%");
    $(document.getElementById("waitScreen")).css("width", "100%");
    $(document.getElementById("waitScreen")).css("z-index", "0");
    $(document.getElementById("waitScreen")).css("visibility", "visible");
    
}
function removeWaitScreen() {
    $(document.getElementById("mainForm")).css("opacity", "1");
    $(document.getElementById("waitScreen")).css("visibility", "hidden");
    $(document.getElementById("mainForm")).css("cursor", "auto");
   
}


function showhideLogin()
{
    $(document.getElementById("divlocations")).slideUp(100);
    $(document.getElementById("divloc")).slideUp(100);
    $(document.getElementById("divUserlocations")).slideUp(100);
    $(document.getElementById("divUser")).slideToggle(400);
}
function showhideUserLocations()
{
    buildUserLocations();
    $(document.getElementById("divloc")).slideUp(100);
    $(document.getElementById("divlocations")).slideUp(100);
    $(document.getElementById("divUser")).slideUp(100);
    $(document.getElementById("divUserlocations")).slideToggle(400);
}

function getUserData()
    {
    document.getElementById("lblUserClick").innerHTML = "Checking login information";
    
    loggin(document.getElementById("emailaddress").value, document.getElementById("password").value);
    
}
function loggin(emailaddress,password)
{
    
     $.ajax({
        url: window.location.href + "api/Login?emailaddress=" + emailaddress + "&password="+password
        }).then(function(data) {
            if (data.userId=="z") {
                document.getElementById("loginError").innerHTML = "Login failed!";
                document.getElementById("lblUserClick").innerHTML = "Re-enter login details";
            } else {
                document.getElementById("hvUserId").value = data.emailaddress;
                document.getElementById("lblUserClick").innerHTML = data.emailaddress;
                document.getElementById("lblUserClick").onclick = "";
                $(document.getElementById("divUser")).slideUp(200);
                $(document.getElementById("usrlocClick")).css("visibility", "visible");
               // $.cookie('UserId', data.userId);
                setCookie('UserId', data.userId, 1);
                setCookie('EmailAddress', data.emailaddress, 1);
            }
        });
    
}
function addLocationToFavourite(location) {
    var userid = getCookie('UserId')
    
    if (userid != null) {
       $.ajax({
            url:  window.location.href + "api/UserLocations?userID=" + userid + "&tKey=" + location
       }).then(function (data) {
           if (data) {
               document.getElementById(location).src = "images/favrem.png";
           } else {
               document.getElementById(location).src = "images/favadd.png";
           }
           buildUserLocations();
       });
       
    };
}
function addUserLocationToFavourite(location) {
    var userid =  getCookie('UserId')
    if (userid != null) {
       $.ajax({
            url:  window.location.href + "api/UserLocations?userID=" + userid + "&tKey=" + location
       }).then(function (data) {
           if (data) {
               document.getElementById("u" + location).src = "images/favrem.png";
           } else {
               document.getElementById("u" + location).src = "images/favadd.png";
           }
         buildUserLocations();
        });
    };
}
function buildUserLocations()
{
    

    var userid =  getCookie('UserId')
      if (userid != null) {
          $.ajax({
              url:  window.location.href + "api/UserLocationsHTML?userID=" + userid 
          }).then(function (data) {
              document.getElementById("divUserlocations").innerHTML = data;
          }
               
      )
    };
    
}


function setCookie(cname,cvalue,exdays) {
    var d = new Date();
    d.setTime(d.getTime() + (exdays*24*60*60*1000));
    var expires = "expires=" + d.toGMTString();
    document.cookie = cname+"="+cvalue+"; "+expires;
}

function getCookie(cname) {
    var name = cname + "=";
    var ca = document.cookie.split(';');
    for(var i=0; i<ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0)==' ') {
            c = c.substring(1);
        }
        if (c.indexOf(name) == 0) {
            return c.substring(name.length, c.length);
        }
    }
    return "";
}

function checkCookie() {
    var user=getCookie("username");
    if (user != "") {
        alert("Welcome again " + user);
    } else {
       user = prompt("Please enter your name:","");
       if (user != "" && user != null) {
           setCookie("username", user, 30);
       }
    }
}