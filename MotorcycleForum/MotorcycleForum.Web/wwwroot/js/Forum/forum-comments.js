document.addEventListener("DOMContentLoaded", () => {
    const commentForm = document.getElementById("comment-form");
    const commentInput = document.getElementById("comment-content");
    const commentsList = document.getElementById("comments-list");
    const commentsSection = document.getElementById("comments-section");
    const toggleCommentsBtn = document.getElementById("toggle-comments-btn");

    const postId = commentForm.dataset.postId;
    const currentUserAvatar = commentForm.dataset.currentUserAvatar;

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

    toggleCommentsBtn.addEventListener("click", () => {
        const visible = commentsSection.style.display !== "none";
        commentsSection.style.display = visible ? "none" : "block";
        toggleCommentsBtn.textContent = visible ? "💬 Show Comments" : "Hide Comments";
    });

    commentForm.addEventListener("submit", e => {
        e.preventDefault();
        const content = commentInput.value.trim();
        if (!content) return;

        fetchWithCsrf("/Forum/AddComment", "POST", { postId, content })
            .then(res => res.json())
            .then(data => {
                if (!data.success) return Swal.fire("Error", data.message, "error");
                buildCommentHtml(data.commentId, content);
                commentInput.value = "";
            });
    });

    commentsList.addEventListener("click", e => {
        const btn = e.target.closest("button");
        if (!btn) return;

        const commentId = btn.dataset.commentId;

        if (btn.classList.contains("reply-btn")) {
            const form = document.querySelector(`form.reply-form[data-parent-id="${commentId}"]`);
            if (form) {
                form.classList.toggle("d-none");
                if (!form.classList.contains("d-none")) {
                    form.querySelector("textarea")?.focus();
                }
            }
        }

        if (btn.classList.contains("delete-comment-btn")) {
            Swal.fire({
                title: 'Delete this comment?',
                text: "This cannot be undone.",
                icon: 'warning',
                showCancelButton: true,
                confirmButtonText: 'Yes, delete it',
                cancelButtonText: 'Cancel',
                confirmButtonColor: '#d81324',
                cancelButtonColor: '#6c757d',
                background: '#fff',
                customClass: {
                    popup: 'shadow rounded',
                    confirmButton: 'btn btn-danger fw-bold',
                    cancelButton: 'btn btn-secondary ms-2'
                },
                buttonsStyling: false
            }).then(result => {
                if (result.isConfirmed) {
                    fetchWithCsrf("/Forum/DeleteComment", "POST", { commentId })
                        .then(res => res.json())
                        .then(data => {
                            if (!data.success) return Swal.fire("Error", data.message, "error");

                            const card = btn.closest(".card[data-comment-id]");
                            const wrapper = card.closest(".replies-wrapper");
                            card.classList.add("fade-out");

                            setTimeout(() => {
                                card.remove();

                                if (wrapper) {
                                    const repliesLeft = wrapper.querySelectorAll(".reply-card");
                                    if (repliesLeft.length === 0) {
                                        wrapper.remove();
                                    }
                                }
                            }, 300);
                        });
                }
            });
        }
    });

    function handleReplySubmit(e) {
        e.preventDefault();
        const form = e.target;
        const parentId = form.dataset.parentId;
        const content = form.querySelector(".reply-content").value.trim();
        if (!content) return;

        fetchWithCsrf("/Forum/ReplyToComment", "POST", {
            postId,
            parentCommentId: parentId,
            content
        })
            .then(res => res.json())
            .then(data => {
                if (!data.success) return;

                const commentCard = commentsList.querySelector(`.card[data-comment-id="${parentId}"]`);
                if (!commentCard) return;

                let wrapper = commentCard.querySelector(".replies-wrapper");
                if (!wrapper) {
                    wrapper = createRepliesWrapper();
                    commentCard.querySelector(".card-body").appendChild(wrapper);
                }

                const repliesContainer = wrapper.querySelector(".replies");
                repliesContainer.insertAdjacentHTML("beforeend", buildReplyHtml(data.replyId, content));
                wrapper.querySelector(".toggle-replies-btn").style.display = "inline-block";
                repliesContainer.style.display = "block";

                form.reset();
                form.classList.add("d-none");
            });
    }

    document.querySelectorAll(".toggle-replies-btn").forEach(btn => {
        const repliesContainer = btn.nextElementSibling;
        btn.addEventListener("click", () => {
            const isVisible = repliesContainer.style.display !== "none";
            repliesContainer.style.display = isVisible ? "none" : "block";
            btn.textContent = isVisible ? "Show Replies" : "Hide Replies";
        });
    });


    function createRepliesWrapper() {
        const wrapper = document.createElement("div");
        wrapper.className = "replies-wrapper";

        const toggleBtn = document.createElement("button");
        toggleBtn.textContent = "Hide Replies";
        toggleBtn.className = "btn btn-sm btn-link text-secondary toggle-replies-btn mb-2 px-2";
        toggleBtn.addEventListener("click", () => {
            const replies = wrapper.querySelector(".replies");
            const visible = replies.style.display !== "none";
            replies.style.display = visible ? "none" : "block";
            toggleBtn.textContent = visible ? "Show Replies" : "Hide Replies";
        });

        const repliesContainer = document.createElement("div");
        repliesContainer.className = "replies ps-4 mt-3 border-start border-danger border-2";

        wrapper.appendChild(toggleBtn);
        wrapper.appendChild(repliesContainer);

        return wrapper;
    }

    function buildCommentHtml(id, content) {
        const html = `
        <div class="card my-3 comment-card border-0 shadow-sm" data-comment-id="${id}">
            <div class="card-body">
                <div class="d-flex justify-content-between align-items-start mb-2">
                    <div class="d-flex align-items-start">
                        <img src="${currentUserAvatar}" class="rounded-circle me-3 border-secondary border"
                             style="width: 40px; height: 40px; object-fit: cover;" />
                        <div>
                            <p class="mb-1 text-dark">${content}</p>
                            <small class="text-muted"><strong>You</strong> · Just now</small>
                        </div>
                    </div>
                    <div class="d-flex align-items-center gap-2">
                        <button class="btn btn-sm btn-outline-secondary reply-btn" data-comment-id="${id}" title="Reply">
                            <i class="bi bi-reply"></i>
                        </button>
                        <button class="btn btn-sm btn-outline-danger delete-comment-btn" data-comment-id="${id}" title="Delete">
                            <i class="bi bi-trash3"></i>
                        </button>
                    </div>
                </div>
                <form class="reply-form d-none mt-3" data-parent-id="${id}">
                    <textarea class="form-control mb-2 reply-content" rows="2" placeholder="Write a reply..." required></textarea>
                    <button type="submit" class="btn btn-primary btn-sm">
                        <i class="bi bi-send"></i> Submit Reply
                    </button>
                </form>
                <div class="replies-wrapper"></div>
            </div>
        </div>`;
        commentsList.insertAdjacentHTML("beforeend", html);
        const newForm = commentsList.querySelector(`form.reply-form[data-parent-id="${id}"]`);
        if (newForm) {
            newForm.addEventListener("submit", handleReplySubmit);
        }
    }

    function buildReplyHtml(id, content) {
        return `
            <div class="card my-2 reply-card" data-comment-id="${id}">
                <div class="card-body d-flex justify-content-between align-items-start">
                    <div class="d-flex align-items-start">
                        <img src="${currentUserAvatar}" class="rounded-circle me-3 border-secondary border"
                             style="width: 36px; height: 36px; object-fit: cover;" />
                        <div>
                            <p class="mb-1 text-dark">${content}</p>
                            <small class="text-muted"><strong>You</strong> · Just now</small>
                        </div>
                    </div>
                    <button class="btn btn-sm btn-outline-danger delete-comment-btn" data-comment-id="${id}">
                        <i class="bi bi-trash3"></i>
                    </button>
                </div>
            </div>`;
    }

    document.querySelectorAll("form.reply-form").forEach(form => {
        form.addEventListener("submit", handleReplySubmit);
    });
});
