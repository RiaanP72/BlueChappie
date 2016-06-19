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

}
function loggin(emailaddress,password)
{
     $.ajax({
        url: "api/Login?emailaddress=" + emailaddress + "&password="+password
        }).then(function(data) {
            if (data.userId=="z") {
                document.getElementById("loginError").innerHTML = "Login failed!";
                document.getElementById("lblUserClick").innerHTML = "Re-enter login details";
            } else {
                document.getElementById("lblUserClick").innerHTML = data.emailaddress;
                document.getElementById("lblUserClick").onclick = "";
                $(document.getElementById("divUser")).slideToggle(200);
                document.location = "Default.aspx?user=" + data.userId;
            }
        });
}
function addLocationToFavourite(location) {
    var userid = document.getElementById("hvUserId").value;
    if (userid != null) {
       $.ajax({
            url: "api/UserLocations?userID=" + userid + "&tKey=" + location
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
    var userid = document.getElementById("hvUserId").value;
    if (userid != null) {
       $.ajax({
            url: "api/UserLocations?userID=" + userid + "&tKey=" + location
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

      var userid = document.getElementById("hvUserId").value;
      if (userid != null) {
          $.ajax({
              url: "api/UserLocationsHTML?userID=" + userid 
          }).then(function (data) {
              document.getElementById("divUserlocations").innerHTML = data;
          }
               
      )
    };

}