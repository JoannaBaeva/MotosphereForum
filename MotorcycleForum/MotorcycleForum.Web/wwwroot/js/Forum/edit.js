document.addEventListener("DOMContentLoaded", () => {
    const dropZone = document.getElementById("dropZone");
    const imageInput = document.getElementById("imageInput");
    const previewContainer = document.getElementById("previewContainer");
    const form = document.querySelector("form");

    let newImages = [];

    // 🔥 This is the correct fix
    dropZone.addEventListener("click", () => imageInput.click());

    dropZone.addEventListener("dragover", e => {
        e.preventDefault();
        dropZone.classList.add("bg-secondary", "text-white");
    });

    dropZone.addEventListener("dragleave", () => {
        dropZone.classList.remove("bg-secondary", "text-white");
    });

    dropZone.addEventListener("drop", (e) => {
        e.preventDefault();
        dropZone.classList.remove("bg-secondary", "text-white");
        const files = [...e.dataTransfer.files];
        imageInput.files = createFileList(files);
        handleNewFiles(files);
    });

    imageInput.addEventListener("change", () => {
        const files = [...imageInput.files];
        handleNewFiles(files);
    });

    function createFileList(files) {
        const dt = new DataTransfer();
        files.forEach(file => dt.items.add(file));
        return dt.files;
    }

    function handleNewFiles(files) {
        if ((window.existingImagesCount + files.length) > 10) {
            Swal.fire('Limit Reached', 'You can have a maximum of 10 images total.', 'warning');
            imageInput.value = "";
            return;
        }

        newImages = files;
        renderPreviews();
    }

    function renderPreviews() {
        previewContainer.innerHTML = '';

        newImages.forEach((file, index) => {
            const reader = new FileReader();

            reader.onload = e => {
                const col = document.createElement("div");
                col.className = 'col-md-3 mb-3 position-relative animate__animated animate__fadeInUp';

                const img = document.createElement("img");
                img.src = e.target.result;
                img.className = "img-fluid rounded border shadow-sm w-100";

                const removeBtn = document.createElement("button");
                removeBtn.type = "button";
                removeBtn.className = "btn btn-sm btn-danger position-absolute top-0 end-0 m-1";
                removeBtn.innerHTML = '<i class="bi bi-x-lg"></i>';

                removeBtn.addEventListener("click", () => {
                    const updatedImages = [...newImages];
                    updatedImages.splice(index, 1);
                    newImages = updatedImages;
                    imageInput.files = createFileList(newImages);
                    renderPreviews();
                });

                col.appendChild(img);
                col.appendChild(removeBtn);
                previewContainer.appendChild(col);
            };

            reader.readAsDataURL(file);
        });
    }
});
