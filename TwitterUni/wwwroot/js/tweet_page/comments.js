$(document).ready(() => {

    $("#reply-btn").click(() => {

        let replyContent = $("#reply-input").val().trim();
        let tweetId = $("#current-tweet").attr('tweet-id');
        $("#reply-input").val('');

        if (replyContent.length > 0) {

            $.ajax({
                type: "POST",
                url: "/Comment/CreateComment",
                data: {
                    Id: null,
                    TweetId: tweetId,
                    Text: replyContent
                },
                success: (data) => {
                    let reply = $("#reply-sample").clone();
                    reply[0].id = data.id;
                    reply.removeClass('d-none').insertAfter('#reply-box');
                    $(`#${data.id} .text-content`).text(data.text);
                }
            });

        }

    });

    $(".comment-like").click(e => {

        let id = e.target.closest("article").id;
        let btn = $(e.target.closest("button"));

        if (btn.hasClass("liked")) {

            $.ajax({
                type: "DELETE",
                url: "/Comment/UnikeComment",
                data: { commentId: id },
                success: () => {
                    btn.removeClass("liked");
                    btn[0].childNodes[2].textContent--;
                    btn.children("i").removeClass("fa-solid");
                    btn.children("i").addClass("fa-regular");
                }
            });

        } else {

            $.ajax({
                type: "POST",
                url: "/Comment/LikeComment",
                data: { commentId: id },
                success: () => {
                    btn.addClass("liked");
                    btn[0].childNodes[2].textContent++;
                    btn.children("i").addClass("fa-solid");
                    btn.children("i").removeClass("fa-regular");
                }
            });

        }
    });

    $("#comments").click(() => {
        $("#reply-input").focus();
    });

    $(".delete-reply-btn").click(e => {

        let id = e.target.closest("article").id;

        $.ajax({
            type: "DELETE",
            url: "/Comment/DeleteComment",
            data: { commentId: id },
            success: () => {
                e.target.closest("article").remove();
            }
        });

    });
});