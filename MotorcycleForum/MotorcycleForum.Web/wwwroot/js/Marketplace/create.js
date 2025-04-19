const dropZone = document.getElementById('dropZone');
const imageInput = document.getElementById('imageInput');
const previewContainer = document.getElementById('previewContainer');
const uploadTrigger = document.getElementById('uploadTrigger');

let fileList = [];

uploadTrigger.onclick = () => imageInput.click();

dropZone.addEventListener('dragover', e => {
    e.preventDefault();
    dropZone.classList.add('bg-secondary', 'text-white');
});

dropZone.addEventListener('dragleave', () => {
    dropZone.classList.remove('bg-secondary', 'text-white');
});

dropZone.addEventListener('drop', async (e) => {
    e.preventDefault();
    dropZone.classList.remove('bg-secondary', 'text-white');
    await handleFiles(e.dataTransfer.files);
});

imageInput.addEventListener('change', async () => {
    await handleFiles(imageInput.files);
});

async function handleFiles(files) {
    for (let i = 0; i < files.length; i++) {
        const compressed = await imageCompression(files[i], {
            maxSizeMB: 1,
            maxWidthOrHeight: 1024,
            useWebWorker: true
        });

        fileList.push(compressed);
        addPreview(compressed);
    }

    updateInputFileList();
}

function addPreview(file) {
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
            const index = Array.from(previewContainer.children).indexOf(col);
            fileList.splice(index, 1);
            col.remove();
            updateInputFileList();
        };

        col.appendChild(img);
        col.appendChild(removeBtn);
        previewContainer.appendChild(col);
    };

    reader.readAsDataURL(file);
}

function updateInputFileList() {
    const dataTransfer = new DataTransfer();
    fileList.forEach(file => dataTransfer.items.add(file));
    imageInput.files = dataTransfer.files;
}

new Sortable(previewContainer, {
    animation: 150,
    onEnd: () => {
        const newOrder = Array.from(previewContainer.children);
        fileList = newOrder.map(div => {
            const index = Array.from(previewContainer.children).indexOf(div);
            return imageInput.files[index];
        });
        updateInputFileList();
    }
});

document.querySelector('form').addEventListener('submit', function (e) {
    if (imageInput.files.length === 0) {
        e.preventDefault();

        const validationMessage = document.createElement('span');
        validationMessage.className = 'text-danger d-block mt-2';
        validationMessage.textContent = 'Please upload at least one image.';

        const existing = document.querySelector('[data-image-required]');
        if (!existing) {
            imageInput.parentElement.appendChild(validationMessage);
            validationMessage.setAttribute('data-image-required', true);
        }
    }
});