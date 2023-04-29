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

});