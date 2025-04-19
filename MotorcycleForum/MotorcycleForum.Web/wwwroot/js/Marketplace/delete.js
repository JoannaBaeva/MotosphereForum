document.getElementById("confirm-delete-btn")?.addEventListener("click", function () {
    Swal.fire({
        title: 'Are you sure?',
        text: "This post will be permanently deleted.",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Yes, delete it!',
        cancelButtonText: 'Cancel',
        confirmButtonColor: '#d81324',
        cancelButtonColor: '#6c757d',
        background: '#fff',
        customClass: {
            popup: 'shadow rounded',
            confirmButton: 'btn btn-danger fw-bold',
            cancelButton: 'btn btn-secondary fw-bold ms-2'
        },
        buttonsStyling: false
    }).then((result) => {
        if (result.isConfirmed) {
            document.getElementById("delete-form").submit();
        }
    });
});