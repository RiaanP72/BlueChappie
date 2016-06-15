function getImgData(imgGUID)
    {

    $.ajax({
            url: "/api/ImageInfo?imgGUID=" + imgGUID
        }).then(function(data) {
            
            //document.getElementById("imginfo").style.visibility = "visible";
            
            document.getElementById("img").innerHTML = '<img class="img" src="data:image/jpg;base64,' + data.webImageBase64Encoded + '">';
            document.getElementById("imgtitle").innerHTML=data.title;
            document.getElementById("imgtags").innerHTML=data.tags;
            document.getElementById("imgowner").innerHTML=data.owner;
            document.getElementById("imgdatetaken").innerHTML=data.dateTaken;
            document.getElementById("imgdatehit").innerHTML=data.dateShot;
            document.getElementById("imgsourceurl").innerHTML=data.sourceURL;
            document.getElementById("imgtag").innerHTML = data.tag;
            document.getElementById("imginfo").style.visibility = "visible";
            


            
        });

}
function closeimginfo() {
    
    document.getElementById("imginfo").style.visibility = "hidden";

}