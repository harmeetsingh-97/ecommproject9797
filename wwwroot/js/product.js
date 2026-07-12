var dataTable;
$(document).ready(function () {
    loadTableData();
})

function loadTableData() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url":"/admin/product/GetAll"
        },
        "columns": [
            { "data": "title", "width": "15%" },
            { "data": "description", "width": "15%" },
            { "data": "author", "width": "15%" },
            { "data": "price", "width": "15%" },
            { "data": "isbn", "width": "15%" },
            {  
                "data": "id",
                "render": function (Data) {
                    return `
                    <div class="text-center">
                        <a href="/admin/product/upsert/${Data}" class="btn btn-info">
                        <i class="fas fa-edit"></i>
                        </a>
                        <a class="btn btn-danger" onclick=Delete('/admin/product/Delete/${Data}')>
                        <i class="fas fa-trash-alt"></i>
                    </a>
                    </div>
                    `;
                    
                }
            }
        ]
    })
}
function Delete (url)
{
   // alert(url);
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
