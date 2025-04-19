document.addEventListener("DOMContentLoaded", () => {
    const modal = document.createElement("div");
    modal.id = "imageZoomModal";
    modal.style.cssText = `
        position: fixed;
        top: 0; left: 0;
        width: 100vw; height: 100vh;
        background: rgba(0,0,0,0.8);
        display: none;
        justify-content: center;
        align-items: center;
        z-index: 1050;
    `;

    const img = document.createElement("img");
    img.style.cssText = `
        max-width: 90%;
        max-height: 90%;
        border-radius: 10px;
        box-shadow: 0 0 20px rgba(0,0,0,0.5);
        transition: transform 0.3s ease;
    `;
    modal.appendChild(img);

    modal.addEventListener("click", () => {
        modal.style.display = "none";
        img.src = "";
    });

    document.body.appendChild(modal);

    document.querySelectorAll(".zoomable").forEach(el => {
        el.addEventListener("click", () => {
            img.src = el.src;
            modal.style.display = "flex";
        });
    });
});
