$(document).ready(() => {

    let backPath = window.location.pathname.split('/').slice(-1)[0];
    $('#back-link').attr('href', backPath.replaceAll('-', '/'));

    $("#likes").click(() => {

        let btn = $('#likes');
        let tweetId = $("#current-tweet").attr('tweet-id');

        if (btn.hasClass("liked")) {

            $.ajax({
                type: "DELETE",
                url: "/Tweet/UnlikeTweet",
                data: { tweetId: tweetId },
                success: () => {
                    btn.removeClass("liked");
                    btn[0].childNodes[2].textContent--;
                    $("#likes i").removeClass("fa-solid");
                    $("#likes i").addClass("fa-regular");
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
                    $("#likes i").addClass("fa-solid");
                    $("#likes i").removeClass("fa-regular");
                }
            });
        }
    });

    $("#retweets").click(e => {

        let btn = $('#retweets');
        let tweetId = $("#current-tweet").attr('tweet-id');

        if (btn.hasClass("retweeted")) {

            $.ajax({
                type: "DELETE",
                url: "Tweet/DeleteRetweet",
                data: { tweetId: tweetId },
                success: () => {
                    btn.removeClass("retweeted");
                    btn[0].childNodes[2].textContent--;
                }
            });

        } else {

            $.ajax({
                type: "POST",
                url: "/Tweet/CreateRetweet",
                data: { tweetId: tweetId },
                success: () => {
                    btn.addClass("retweeted");
                    btn[0].childNodes[2].textContent++;
                }
            });
        }
    });
});