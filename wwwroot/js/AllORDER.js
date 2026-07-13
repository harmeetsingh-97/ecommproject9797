var DataTable;
$(document).ready(function () {
    loadTableData();
})

function loadTableData() {
    DataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/admin/AllOrder/GetAll"
        },
        "columns": [
            { data: "id", "width": "25%" },         // Changed from orderId to id
            { data: "orderDate", "width": "25%" },  // Use the exact property name from your model in camelCase
            { data: "name", "width": "25%" },
            { data: "orderTotal", "width": "200%" }, // Changed from totalamount to orderTotal (or whatever your C# property is)
            {
                data: "id",
                "render": function (data) {
                    return `
                <div class="text-center">
                    <a href="/Admin/AllOrder/Details?orderId=${data}" class="btn btn-success">
                        DETAILS
                    </a>
                </div>
            `;
                },
               
            }
        ]
    })
}