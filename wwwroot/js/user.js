var dataTable;
$(document).ready(function () {
    loadDataTable();
})

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/admin/User/GetAll"
        },
        "columns": [
            {"data": "name", "width": "15%" },
            {"data": "email", "width": "15%" },
            {"data": "phoneNumber", "width": "15%" },
            {"data": "company.name", "width": "15%" },
            {"data": "role", "width": "15%" },
            {
                "data": {
                   Id: "id", lockoutEnd: "lockoutEnd"
                },
                "render": function (data) {
                    var today = new Date().getTime();
                    var lockout = new Date(data.lockoutEnd).getTime();
                    if (lockout > today)
                    {
                        // if greater then lock user
                        return `
                        <div class="text-center">
                        <a class="btn btn-danger" onclick=LockUnlock('${data.id}')>
                       <i class ="fas fa-unlock"></i>&nbsp;
                        Unlock
                        </a>
                        </div>
                        `;
                    }
                    else
                    {
                        //user not greater then unlock user
                        return `
                        <div class="text-center">
                        <a class="btn btn-success" onclick=LockUnlock('${data.id}')>
                         <i class ="fas fa-lock"></i>&nbsp;
                        Lock
                        </a>
                        </div>

                        `;
                    }
                }
            }

        ]
    })
}



function LockUnlock(id) {
  //  alert(Id);
    $.ajax({
        url: "/admin/User/LockUnlock",
        type: "POST",
        data: JSON.stringify(id),
        contentType: "application/Json",
        success: function (data) {
            if (data.success) {
                toastr.success(data.message);
                dataTable.ajax.reload();
            }
           else {
                toastr.error(data.message);
                dataTable.ajax.reload();
            }
        }
   })
}

