const dropZone = document.getElementById('dropZone');
const imageInput = document.getElementById('imageInput');
const previewContainer = document.getElementById('previewContainer');
const uploadTrigger = document.getElementById('uploadTrigger');

uploadTrigger.onclick = () => imageInput.click();

dropZone.addEventListener('dragover', e => {
    e.preventDefault();
    dropZone.classList.add('bg-secondary', 'text-white');
});

dropZone.addEventListener('dragleave', () => {
    dropZone.classList.remove('bg-secondary', 'text-white');
});

dropZone.addEventListener('drop', (e) => {
    e.preventDefault();
    dropZone.classList.remove('bg-secondary', 'text-white');

    imageInput.files = e.dataTransfer.files;
    updatePreviews(e.dataTransfer.files);
});

imageInput.addEventListener('change', () => {
    updatePreviews(imageInput.files);
});

function updatePreviews(files) {
    previewContainer.innerHTML = '';

    for (let i = 0; i < files.length; i++) {
        const reader = new FileReader();

        reader.onload = function (e) {
            const col = document.createElement('div');
            col.className = 'col-md-3 mb-3 position-relative animate__animated animate__fadeInUp';

            const img = document.createElement('img');
            img.src = e.target.result;
            img.className = 'img-fluid rounded';

            const removeBtn = document.createElement('button');
            removeBtn.type = 'button';
            removeBtn.className = 'btn btn-sm btn-danger position-absolute';
            removeBtn.style.top = '5px';
            removeBtn.style.right = '5px';
            removeBtn.innerHTML = '<i class="bi bi-x-lg"></i>';

            removeBtn.onclick = () => {
                col.remove();
                clearFileInput(); // if an image is removed, reset the input
            };

            col.appendChild(img);
            col.appendChild(removeBtn);
            previewContainer.appendChild(col);
        };

        reader.readAsDataURL(files[i]);
    }
}

function clearFileInput() {
    imageInput.value = ''; // This clears input safely without breaking submission
}