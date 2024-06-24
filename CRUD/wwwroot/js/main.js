const URL = 'https://localhost:7199/Authentication/Validar';
const BUTTON_AUTH = document.getElementById('autenticar');

let data = {
    "Correo": "Alejoquevedo0598@gmail.com",
    "Password": "123"
};

const autenticar = () => {
    fetch(URL, {
    method: 'POST',
    headers: {
        'Content-Type': 'application/json'
    },
    body: JSON.stringify(data)
    }).then(response => {
        if (!response.ok) {
            throw new Error('Error en la creacion del token' + response.statusText);
        }
        return response.json();
    }).then(data => {
        localStorage.setItem("Token",data.token)
    }).catch(error => {
        console.error('No se puede realizar la solicitud fetch:', error);
    });
}

BUTTON_AUTH.addEventListener('click', autenticar)



const mostrarDatos = () => {
    $(function () {
        $("#dataGridContainer").dxDataGrid({
            dataSource: [{
                idProveedor: 1,
                nombre: "a",
                rut: "a"
            }],
            columnResizingMode: 'nextColumn',
            columnMinWidth: 50,
            columnAutoWidth: true,
            columns: [
                { dataField: "idProveedor", caption: "ID" },
                { dataField: "nombre", caption: "Nombre" },
                { dataField: "rut", caption: "Rut" },
                {
                    type: "buttons",
                    buttons: ["edit", "delete"]
                }
            ],
            scrolling: {
                rowRenderingMode: 'virtual',
            },
            showBorders: true,
            rowAlternationEnabled: true,
            showColumnLines: false,
            showRowLines: true,
            searchPanel: {
                visible: true,
            },
            paging: {
                pageSize: 15,
            },
            pager: {
                visible: true,
                allowedPageSizes: [10, 15, 20, 'all'],
                showPageSizeSelector: true,
                showInfo: true,
                showNavigationButtons: true,
            },
            export: {
                enabled: true
            },
            onExporting: function (e) {
                const workbook = new ExcelJS.Workbook();
                const worksheet = workbook.addWorksheet('Main sheet');

                DevExpress.excelExporter.exportDataGrid({
                    worksheet: worksheet,
                    component: e.component
                }).then(function () {
                    workbook.xlsx.writeBuffer().then(function (buffer) {
                        saveAs(new Blob([buffer], { type: 'application/octet-stream' }), 'DataGrid.xlsx');
                    });
                });
            },
            editing: {
                mode: "row",
                allowUpdating: true,
                allowDeleting: true,
                allowAdding: false
            },
            onRowUpdating: e => {
                let { oldData, newData } = e;
                let User = {
                    IdProveedor: oldData.idProveedor,
                    Nombre: newData.nombre != undefined ? newData.nombre : oldData.nombre,
                    Rut: newData.rut != undefined ? newData.rut : oldData.rut
                };

                fetch(`${url}EditProveedor`, {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify(User)
                })
                    .then(response => {
                        if (!response.ok) {
                            throw new Error(`Error en la solicitud: ${response.status} ${response.statusText}`);
                        }
                        return response.json();
                    })
                    .then(data => {
                        console.log('Respuesta exitosa:', data);
                    })
                    .catch(error => {
                        console.error('Error en la solicitud:', error);
                    });
            },
            onRowRemoving: function (e) {
                fetch(`${url}DeleteUser?UserId=${e.data.id}`);
            }
        });
    });
}
