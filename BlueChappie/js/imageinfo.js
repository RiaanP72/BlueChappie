$(document).ready(function () {
    $(document.getElementById("divlocations")).slideUp(20);
    $(document.getElementById("divUser")).slideUp(20);
    $(document.getElementById("divloc")).slideUp(20);
    $(document.getElementById("divUserlocations")).slideUp(20);
    $("img").hover(function(){
      
        $(this).css("border-color", "#000099");
        $(this).css("cursor", "pointer");
      
        }, function(){
      
        $(this).css("border-color", "transparent");
        });
    var userid = document.getElementById("hvUserId").value;
    if (userid != "") { document.getElementById("usrlocClick").style.visibility = "visible" } else { document.getElementById("usrlocClick").style.visibility = "hidden"  };
});
function showhideLocations()
{
     var userid = document.getElementById("hvUserId").value;
     if (userid != null) {
         $.ajax({
             url: "~/api/LocationsHTML?userID=" + userid
         }).then(function (data) {
             document.getElementById("divlocations").innerHTML = data;
         }

     )
     } else {

          $.ajax({
             url: "~/api/LocationsHTML"
         }).then(function (data) {
             document.getElementById("divlocations").innerHTML = data;
         }

         )
     };
     $(document.getElementById("divUser")).slideUp(100);
        
        $(document.getElementById("divUserlocations")).slideUp(100);
    $(document.getElementById("divloc")).slideToggle(200);
    $(document.getElementById("divlocations")).slideToggle(200);
  
}


function imgSelectLocation(location) {
    //document.getElementById("txtLocation").innerText = location;
    //document.getElementById("btnLookUpAndSync").click;
    showWaitScreen()
    document.location = "Default.aspx?location=" + location;
}

function getImgData(imgGUID)
    {
    $(document.getElementById("divlocations")).slideUp(20);
        $(document.getElementById("divloc")).slideUp(20);
    $(document.getElementById("divUser")).slideUp(20);
    $(document.getElementById("divUserlocations")).slideUp(20);
    $.ajax({
            url: "~/api/ImageInfo?imgGUID=" + imgGUID
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
    $(document.getElementById("mainForm")).css("opacity", "0.4");
    $(document.getElementById("waitScreen")).css("visibility", "visible");
}
