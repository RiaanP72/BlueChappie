
$(document).ready(function () {
    $(document.getElementById("divloc")).slideUp(20);
    $(document.getElementById("divlocations")).slideUp(20);
    $(document.getElementById("divUser")).slideUp(20);
    
    $(document.getElementById("divUserlocations")).slideUp(20);
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
    showWaitScreen();
     var usr_id =  getCookie('UserId')
          $.ajax({
             url:  window.location.pathname + "api/LocationsHTML?UserId=" +  getCookie('UserId')
         }).then(function (data) {
             document.getElementById("divlocations").innerHTML = data;
         }

         )
    
     $(document.getElementById("divUser")).slideUp(100);
     $(document.getElementById("divUserlocations")).slideUp(100);
     $(document.getElementById("divloc")).slideToggle(200);
     $(document.getElementById("divlocations")).slideDown(200);
     removeWaitScreen();
}


function imgSelectLocation(location) {
    //window.setTimeout(showWaitScreen, 500);
   // var x = window.setTimeout(showWaitScreen, 500);
    showWaitScreen();
    $.ajax({
              url:  window.location.pathname + "api/MainImageListHTML?Location=" + location 
          }).then(function (data) {
              document.getElementById("divMainImageList").innerHTML = data;
          })

    
    $(document.getElementById("divlocations")).slideUp(100);
    $(document.getElementById("divloc")).slideUp(100);
    $(document.getElementById("divUserlocations")).slideUp(100);
    $(document.getElementById("divUser")).slideUp(100);
    //  window.clearTimeout(x);
    //window.clearTimeout();
    removeWaitScreen();
   
}
function scanOnline() {
    var slocation = document.getElementById("txtLocation").value;
    $.ajax({
            url:  window.location.pathname + "api/ScanOnline?Location=" + slocation
    }).then(function (data) {
        imgSelectLocation(location)
    })
}
function getImgData(imgGUID)
    {
    $(document.getElementById("divlocations")).slideUp(20);
        $(document.getElementById("divloc")).slideUp(20);
    $(document.getElementById("divUser")).slideUp(20);
    $(document.getElementById("divUserlocations")).slideUp(20);
    $.ajax({
            url:  window.location.pathname + "api/ImageInfo?imgGUID=" + imgGUID
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
    $(document.getElementById("mainForm")).css("opacity", "0.4");
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
    showWaitScreen();   
    loggin(document.getElementById("emailaddress").value, document.getElementById("password").value);
    removeWaitScreen();
}
function loggin(emailaddress,password)
{
     $.ajax({
        url: window.location.pathname + "api/Login?emailaddress=" + emailaddress + "&password="+password
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
            url:  window.location.pathname + "api/UserLocations?userID=" + userid + "&tKey=" + location
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
            url:  window.location.pathname + "api/UserLocations?userID=" + userid + "&tKey=" + location
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
    showWaitScreen();

    var userid =  getCookie('UserId')
      if (userid != null) {
          $.ajax({
              url:  window.location.pathname + "api/UserLocationsHTML?userID=" + userid 
          }).then(function (data) {
              document.getElementById("divUserlocations").innerHTML = data;
          }
               
      )
    };
    removeWaitScreen();
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