  $(document).ready(function() {
        //var ddProvince = document.getElementById("form-group-select-province");
        $.getJSON("api/values", function (obj) {
            $.each(obj.plainlList,function(key,value)
                {$("#form-group-select-province").append("<option>" + value.plainlistitem + "</option>");}
                  );
            })
        });
