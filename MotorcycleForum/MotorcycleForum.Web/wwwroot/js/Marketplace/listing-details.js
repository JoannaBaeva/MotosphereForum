document.addEventListener("DOMContentLoaded", function () {
    const mainImage = document.getElementById("mainImage");
    const thumbnails = document.querySelectorAll(".listing-thumbnail");

    thumbnails.forEach(thumb => {
        thumb.addEventListener("click", function () {
            const newSrc = this.getAttribute("data-src");
            if (newSrc) {
                mainImage.setAttribute("src", newSrc);
            }
        });
    });
});
