var DataTable;
$(document).ready(function () {
    loadTableData();
})

function loadTableData()
{
    DataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/admin/CoverType/GetAll"
        },
        "columns": [
            { "data": "name", "width": "70%" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                <div class="text-center">
                <a href="/Admin/CoverType/upsert/${data}" class="btn btn-info">
                <i class="fas fa-edit"></i>
                </a>
                <a class="btn btn-danger" onclick=Delete('/Admin/CoverType/Delete/${data}')>
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
                        DataTable.ajax.reload();
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            })
        }
    })
}


