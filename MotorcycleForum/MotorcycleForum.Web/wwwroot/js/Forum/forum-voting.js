document.addEventListener("DOMContentLoaded", () => {
    const upvoteBtn = document.getElementById("upvote-btn");
    const downvoteBtn = document.getElementById("downvote-btn");
    const upvotesCount = document.getElementById("upvotes-count");
    const downvotesCount = document.getElementById("downvotes-count");

    const postId = upvoteBtn.dataset.postId;
    let currentVote = upvoteBtn.dataset.currentVote;

    function fetchWithCsrf(url, method, body = null) {
        const csrfToken = document.querySelector('input[name="__RequestVerificationToken"]')?.value;
        return fetch(url, {
            method,
            headers: {
                "Content-Type": "application/json",
                "RequestVerificationToken": csrfToken
            },
            body: body ? JSON.stringify(body) : null
        });
    }

    function updateVoteStyles(voteType) {
        upvoteBtn.classList.remove("btn-success", "text-white", "active", "btn-outline-success");
        downvoteBtn.classList.remove("btn-danger", "text-white", "active", "btn-outline-danger");

        if (voteType === "upvote") {
            upvoteBtn.classList.add("btn-success", "text-white", "active");
            downvoteBtn.classList.add("btn-outline-danger");
        } else if (voteType === "downvote") {
            downvoteBtn.classList.add("btn-danger", "text-white", "active");
            upvoteBtn.classList.add("btn-outline-success");
        } else {
            upvoteBtn.classList.add("btn-outline-success");
            downvoteBtn.classList.add("btn-outline-danger");
        }
    }

    function handleVote(voteType) {
        const action = voteType.charAt(0).toUpperCase() + voteType.slice(1);
        const toggling = (currentVote === voteType);

        fetchWithCsrf(`/Forum/${action}/${postId}`, "POST")
            .then(res => res.json())
            .then(data => {
                if (!data.success) return alert(data.message || "Vote failed");

                upvotesCount.textContent = data.upvotes;
                downvotesCount.textContent = data.downvotes;

                currentVote = toggling ? null : voteType;
                updateVoteStyles(currentVote);
            });
    }

    // Bind vote events
    upvoteBtn.addEventListener("click", () => handleVote("upvote"));
    downvoteBtn.addEventListener("click", () => handleVote("downvote"));

    // Initial state
    updateVoteStyles(currentVote);
});
