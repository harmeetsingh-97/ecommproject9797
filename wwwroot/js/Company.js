var dataTable;
$(document).ready(function () {
    loadTableData();
})

function loadTableData() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/admin/Company/GetAll"
        },
        "columns": [
            { "data": "name", "width": "15%" },
            { "data": "streetaddress", "width": "15%" },
            { "data": "city", "width": "15%" },
            { "data": "state", "width": "15%" },
            { "data": "phonenumber", "width": "15%" },
            {
                "data": "isauthorizedcompany",
                "render": function (data) {
                    if (data) {
                        return `
                        <input type="checkbox" checked disabled />
                        `;
                    }
                    else {
                        return `
                        <input type="checkbox"  checked disabled />
                        `;
                    }
                }
            },
            {
                "data": "id",
                "render": function (data) {
                    return `
                    <div class="text-center">
                    <a href="/admin/Company/Upsert/${data}" class="btn btn-success">
                    <i class="fas fa-edit"></i>
                    </a>
                    <a class="btn btn-danger" onclick=Delete('/admin/Company/Delete/${data}')>
                    <i class="fas fa-trash-alt"></i>
                    </a>
                    </div>
                    `;
                }
            }
        ]
    })
}
    
function Delete(url)
{
    swal({
        title: "Want TO Delete Data",
        icon: "error",
        Text: "Delete Information",
        buttons: true,
        dangerModel: true
    }).then((willdelete) => {
        if (willdelete) {
            $.ajax({
                url: url,
                type: "DELETE",
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        dataTable.ajax.reload();
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            })
        }
    })
}
