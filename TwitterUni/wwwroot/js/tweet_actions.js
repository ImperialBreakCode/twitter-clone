$(document).ready(() => {

    $(".likes-stat").click(e => {

        let btn = $(e.target.closest(".likes-stat"));
        let tweetId = e.target.closest(".main-tweet").id.split(',')[1];

        if (btn.hasClass("liked")) {

            $.ajax({
                type: "DELETE",
                url: "/Tweet/UnlikeTweet",
                data: { tweetId: tweetId },
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
                url: "/Tweet/LikeTweet",
                data: { tweetId: tweetId },
                success: () => {
                    btn.addClass("liked");
                    btn[0].childNodes[2].textContent++;
                    btn.children("i").addClass("fa-solid");
                    btn.children("i").removeClass("fa-regular");
                }
            });
        }
    });

});