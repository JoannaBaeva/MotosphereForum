document.addEventListener("DOMContentLoaded", function () {
    const categoryFilter = document.getElementById("categoryFilter");
    const dateFilter = document.getElementById("dateFilter");
    const eventsContainer = document.getElementById("eventsContainer");

    // Create "No events found" message
    const noEventsMessage = document.createElement("div");
    noEventsMessage.className = "alert alert-warning w-100 text-center animate__animated animate__fadeIn";
    noEventsMessage.innerHTML = "<i class='bi bi-exclamation-circle'></i> No events found matching your filters.";
    noEventsMessage.style.display = "none";
    eventsContainer.parentNode.appendChild(noEventsMessage);

    function filterEvents() {
        const selectedCategory = categoryFilter.value.toLowerCase();
        const selectedDate = dateFilter.value;
        const now = new Date();

        let anyVisible = false;
        const eventCards = document.querySelectorAll(".event-card");

        eventCards.forEach(card => {
            const cardCategory = card.getAttribute("data-category").toLowerCase();
            const cardDateStr = card.getAttribute("data-date");
            const cardDate = new Date(cardDateStr);

            const matchesCategory = !selectedCategory || cardCategory === selectedCategory;
            let matchesDate = true;

            if (selectedDate === "upcoming") {
                matchesDate = cardDate > now;
            } else if (selectedDate === "past") {
                matchesDate = cardDate <= now;
            } // if "all" - matchesDate stays true

            if (matchesCategory && matchesDate) {
                card.style.display = "block";
                card.classList.add("animate__fadeInUp");
                anyVisible = true;
            } else {
                card.style.display = "none";
                card.classList.remove("animate__fadeInUp");
            }
        });

        noEventsMessage.style.display = anyVisible ? "none" : "block";
    }


    categoryFilter.addEventListener("change", filterEvents);
    dateFilter.addEventListener("change", filterEvents);

    // Initial setup: "All" selected
    filterEvents();
});
