if (TempData["ForumDeleteSuccess"] != null) {
        document.addEventListener("DOMContentLoaded", function () {
            Swal.fire({
                icon: 'success',
                title: 'Deleted!',
                text: '@TempData["ForumDeleteSuccess"]',
                confirmButtonColor: '#d81324',
                background: '#fff',
                customClass: {
                    popup: 'shadow rounded',
                    confirmButton: 'btn btn-danger fw-bold'
                },
                buttonsStyling: false
            });
        });
}