const dropZone = document.getElementById("dropZone");
const imageInput = document.getElementById("imageInput");
const previewContainer = document.getElementById("previewContainer");

let selectedFiles = [];

dropZone.onclick = () => imageInput.click();

dropZone.addEventListener("dragover", e => {
    e.preventDefault();
    dropZone.classList.add("bg-secondary", "text-white");
});

dropZone.addEventListener("dragleave", () => {
    dropZone.classList.remove("bg-secondary", "text-white");
});

dropZone.addEventListener("drop", e => {
    e.preventDefault();
    dropZone.classList.remove("bg-secondary", "text-white");

    const files = Array.from(e.dataTransfer.files);
    addFiles(files);
});

imageInput.addEventListener("change", () => {
    const files = Array.from(imageInput.files);
    addFiles(files);
});

function addFiles(files) {
    const total = selectedFiles.length + files.length;
    if (total > 10) {
        Swal.fire('Oops!', 'You can upload a maximum of 10 images.', 'warning');
        return;
    }

    for (const file of files) {
        if (!selectedFiles.some(f => f.name === file.name && f.lastModified === file.lastModified)) {
            selectedFiles.push(file);
        }
    }

    updatePreviews();
}

function updatePreviews() {
    previewContainer.innerHTML = "";

    selectedFiles.forEach((file, index) => {
        const reader = new FileReader();
        reader.onload = e => {
            const col = document.createElement("div");
            col.className = "col-md-3 col-sm-4 col-6 mb-3 position-relative preview-img";
            col.draggable = true;
            col.dataset.index = index;

            const img = document.createElement("img");
            img.src = e.target.result;
            img.className = "img-fluid rounded border shadow-sm";

            const removeBtn = document.createElement("button");
            removeBtn.className = "btn btn-sm btn-danger position-absolute top-0 end-0 m-1 rounded-circle";
            removeBtn.innerHTML = '<i class="bi bi-x"></i>';
            removeBtn.onclick = () => {
                selectedFiles.splice(index, 1);
                updatePreviews();
            };

            col.appendChild(img);
            col.appendChild(removeBtn);
            previewContainer.appendChild(col);
        };
        reader.readAsDataURL(file);
    });

    syncInputFiles();
}

// Drag & Drop Reordering
let dragStartIndex = null;

previewContainer.addEventListener("dragstart", e => {
    if (e.target.closest(".preview-img")) {
        dragStartIndex = +e.target.closest(".preview-img").dataset.index;
    }
});

previewContainer.addEventListener("dragover", e => {
    e.preventDefault();
});

previewContainer.addEventListener("drop", e => {
    const dropTarget = e.target.closest(".preview-img");
    if (!dropTarget) return;

    const dropIndex = +dropTarget.dataset.index;
    if (dragStartIndex === null || dragStartIndex === dropIndex) return;

    const draggedItem = selectedFiles[dragStartIndex];
    selectedFiles.splice(dragStartIndex, 1);
    selectedFiles.splice(dropIndex, 0, draggedItem);
    updatePreviews();
});

function syncInputFiles() {
    const dataTransfer = new DataTransfer();
    selectedFiles.forEach(f => dataTransfer.items.add(f));
    imageInput.files = dataTransfer.files;
}
